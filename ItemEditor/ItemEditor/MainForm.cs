using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ItemEditor
{
    public partial class MainForm : Form
    {
        private List<ItemEntry> _items = new List<ItemEntry>();
        private List<ItemEntry> _filtered = new List<ItemEntry>();
        private string _currentFile = "";
        private bool _dirty = false;

        public MainForm()
        {
            InitializeComponent();
        }

        // ── Load ─────────────────────────────────────────────────────────────────

        private void btnOpen_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog())
            {
                dlg.Title  = "Open itemtype.txt";
                dlg.Filter = "Item files (itemtype.txt;itemtype.dat)|itemtype.txt;itemtype.dat|All files (*.*)|*.*";
                if (dlg.ShowDialog() != DialogResult.OK) return;
                LoadFile(dlg.FileName);
            }
        }

        private void LoadFile(string path)
        {
            try
            {
                var lines = File.ReadAllLines(path, Encoding.UTF8);
                _items.Clear();

                foreach (var line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    if (line.StartsWith("Amount=")) continue;

                    var item = ItemEntry.Parse(line);
                    if (item != null)
                        _items.Add(item);
                }

                _currentFile = path;
                _dirty       = false;
                txtSearch.Text = "";
                ApplyFilter();
                UpdateStatus();
                Text = "Item Editor — " + Path.GetFileName(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load file:\n" + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── Save ─────────────────────────────────────────────────────────────────

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_currentFile))
            {
                btnSaveAs_Click(sender, e);
                return;
            }
            SaveToFile(_currentFile);
        }

        private void btnSaveAs_Click(object sender, EventArgs e)
        {
            using (var dlg = new SaveFileDialog())
            {
                dlg.Title    = "Save itemtype.txt";
                dlg.Filter   = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                dlg.FileName = "itemtype.txt";
                if (dlg.ShowDialog() != DialogResult.OK) return;
                SaveToFile(dlg.FileName);
            }
        }

        private void SaveToFile(string path)
        {
            try
            {
                CommitGridEdits();

                var sb = new StringBuilder();
                sb.AppendLine("Amount=" + _items.Count);
                foreach (var item in _items)
                    sb.AppendLine(item.ToLine());

                File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
                _currentFile = path;
                _dirty       = false;
                Text = "Item Editor — " + Path.GetFileName(path);
                lblStatus.Text = $"{_items.Count} items saved.";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to save:\n" + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── Grid ─────────────────────────────────────────────────────────────────

        private void ApplyFilter()
        {
            string q = txtSearch.Text.Trim().ToLower();
            _filtered = string.IsNullOrEmpty(q)
                ? new List<ItemEntry>(_items)
                : _items.Where(i =>
                    i.ID.ToString().Contains(q) ||
                    i.DisplayName.ToLower().Contains(q) ||
                    i.SpriteName.ToLower().Contains(q)).ToList();

            grid.DataSource = null;
            grid.DataSource = _filtered;
            UpdateStatus();
        }

        private void CommitGridEdits()
        {
            grid.EndEdit();

            foreach (var fi in _filtered)
            {
                var existing = _items.FirstOrDefault(x => x.ID == fi.ID);
                if (existing != null)
                {
                    var idx = _items.IndexOf(existing);
                    _items[idx] = fi;
                }
            }
        }

        private void UpdateStatus()
        {
            lblStatus.Text = $"Total: {_items.Count} items   |   Showing: {_filtered.Count}";
        }

        // ── Add / Delete ─────────────────────────────────────────────────────────

        private void btnAdd_Click(object sender, EventArgs e)
        {
            uint newId = _items.Count > 0 ? _items.Max(x => x.ID) + 1 : 100000;
            var item = new ItemEntry
            {
                ID          = newId,
                SpriteName  = "NewItem",
                DisplayName = "New Item",
                ItemSet     = "None",
                Quality     = 3,
                Durability    = 1000,
                MaxDurability = 1000,
                BuyPrice    = 1000
            };
            _items.Add(item);
            _dirty = true;
            txtSearch.Text = "";
            ApplyFilter();

            // Select and scroll to the new row
            var row = grid.Rows.Cast<DataGridViewRow>()
                .FirstOrDefault(r => ((ItemEntry)r.DataBoundItem).ID == newId);
            if (row != null)
            {
                grid.ClearSelection();
                row.Selected = true;
                grid.FirstDisplayedScrollingRowIndex = row.Index;
                grid.CurrentCell = grid.Rows[row.Index].Cells["ID"];
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (grid.SelectedRows.Count == 0) return;

            var selected = grid.SelectedRows.Cast<DataGridViewRow>()
                .Select(r => (ItemEntry)r.DataBoundItem).ToList();

            string names = string.Join(", ", selected.Select(i => $"{i.ID} ({i.DisplayName})"));
            if (MessageBox.Show($"Delete {selected.Count} item(s)?\n{names}",
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            foreach (var item in selected)
                _items.Remove(item);

            _dirty = true;
            ApplyFilter();
        }

        // ── Duplicate ────────────────────────────────────────────────────────────

        private void btnDuplicate_Click(object sender, EventArgs e)
        {
            if (grid.SelectedRows.Count == 0) return;
            CommitGridEdits();

            var src = (ItemEntry)grid.SelectedRows[0].DataBoundItem;
            uint newId = _items.Max(x => x.ID) + 1;

            var copy = new ItemEntry
            {
                ID            = newId,
                SpriteName    = src.SpriteName,
                Class         = src.Class,
                Proficiency   = src.Proficiency,
                Level         = src.Level,
                Gender        = src.Gender,
                ReqStrength   = src.ReqStrength,
                ReqAgility    = src.ReqAgility,
                Col8          = src.Col8,
                Col9          = src.Col9,
                Type          = src.Type,
                Weight        = src.Weight,
                BuyPrice      = src.BuyPrice,
                SellPrice     = src.SellPrice,
                MaxAttack     = src.MaxAttack,
                MinAttack     = src.MinAttack,
                PhyDefense    = src.PhyDefense,
                Frequency     = src.Frequency,
                Dodge         = src.Dodge,
                ItemHP        = src.ItemHP,
                ItemMP        = src.ItemMP,
                Durability    = src.Durability,
                MaxDurability = src.MaxDurability,
                Col23         = src.Col23,
                Col24         = src.Col24,
                Col25         = src.Col25,
                Col26         = src.Col26,
                Col27         = src.Col27,
                Col28         = src.Col28,
                MagicAttack   = src.MagicAttack,
                MagicDefense  = src.MagicDefense,
                AttackRange   = src.AttackRange,
                Col32         = src.Col32,
                Col33         = src.Col33,
                Col34         = src.Col34,
                Col35         = src.Col35,
                CPWorth       = src.CPWorth,
                DisplayName   = src.DisplayName + "_copy",
                ItemSet       = src.ItemSet,
                Quality       = src.Quality,
                ExtraData     = src.ExtraData
            };

            _items.Add(copy);
            _dirty = true;
            txtSearch.Text = "";
            ApplyFilter();
        }

        // ── Search ───────────────────────────────────────────────────────────────

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        // ── Grid events ──────────────────────────────────────────────────────────

        private void grid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            _dirty = true;
        }

    }
}
