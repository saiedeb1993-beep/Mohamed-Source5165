namespace ItemEditor
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.toolStrip      = new System.Windows.Forms.ToolStrip();
            this.btnOpen        = new System.Windows.Forms.ToolStripButton();
            this.btnSave        = new System.Windows.Forms.ToolStripButton();
            this.btnSaveAs      = new System.Windows.Forms.ToolStripButton();
            this.sep1           = new System.Windows.Forms.ToolStripSeparator();
            this.btnAdd         = new System.Windows.Forms.ToolStripButton();
            this.btnDuplicate   = new System.Windows.Forms.ToolStripButton();
            this.btnDelete      = new System.Windows.Forms.ToolStripButton();
            this.sep2           = new System.Windows.Forms.ToolStripSeparator();
            this.lblSearchLabel = new System.Windows.Forms.ToolStripLabel();
            this.txtSearch      = new System.Windows.Forms.ToolStripTextBox();
            this.grid           = new System.Windows.Forms.DataGridView();
            this.statusStrip    = new System.Windows.Forms.StatusStrip();
            this.lblStatus      = new System.Windows.Forms.ToolStripStatusLabel();

            // ── ToolStrip ────────────────────────────────────────────────────────
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.btnOpen, this.btnSave, this.btnSaveAs,
                this.sep1,
                this.btnAdd, this.btnDuplicate, this.btnDelete,
                this.sep2,
                this.lblSearchLabel, this.txtSearch
            });
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Size     = new System.Drawing.Size(1400, 27);

            this.btnOpen.Text        = "📂 Open itemtype.txt";
            this.btnOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnOpen.Click       += new System.EventHandler(this.btnOpen_Click);

            this.btnSave.Text        = "💾 Save";
            this.btnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnSave.Click       += new System.EventHandler(this.btnSave_Click);

            this.btnSaveAs.Text        = "Save As...";
            this.btnSaveAs.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnSaveAs.Click       += new System.EventHandler(this.btnSaveAs_Click);

            this.btnAdd.Text        = "➕ Add Item";
            this.btnAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnAdd.Click       += new System.EventHandler(this.btnAdd_Click);

            this.btnDuplicate.Text        = "📋 Duplicate";
            this.btnDuplicate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnDuplicate.Click       += new System.EventHandler(this.btnDuplicate_Click);

            this.btnDelete.Text        = "🗑 Delete";
            this.btnDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnDelete.ForeColor   = System.Drawing.Color.DarkRed;
            this.btnDelete.Click       += new System.EventHandler(this.btnDelete_Click);

            this.lblSearchLabel.Text = "  Search:";

            this.txtSearch.Size            = new System.Drawing.Size(200, 27);
            this.txtSearch.TextChanged     += new System.EventHandler(this.txtSearch_TextChanged);

            // ── DataGridView ─────────────────────────────────────────────────────
            this.grid.Dock                    = System.Windows.Forms.DockStyle.Fill;
            this.grid.AllowUserToAddRows      = false;
            this.grid.AllowUserToDeleteRows   = false;
            this.grid.AutoSizeColumnsMode     = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.None;
            this.grid.SelectionMode           = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid.MultiSelect             = true;
            this.grid.RowHeadersWidth         = 40;
            this.grid.Font                    = new System.Drawing.Font("Consolas", 9f);
            this.grid.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(245, 245, 255);
            this.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid.CellValueChanged        += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_CellValueChanged);

            // Set friendly column widths when data is bound
            this.grid.DataBindingComplete += (s, ev) =>
            {
                SetColumnWidths();
            };

            // ── StatusStrip ──────────────────────────────────────────────────────
            this.statusStrip.Items.Add(this.lblStatus);
            this.lblStatus.Text = "Open an itemtype.txt file to begin.";

            // ── Form ─────────────────────────────────────────────────────────────
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize          = new System.Drawing.Size(1400, 700);
            this.Controls.Add(this.grid);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.statusStrip);
            this.Text            = "Item Editor — Vestige";
            this.StartPosition   = System.Windows.Forms.FormStartPosition.CenterScreen;
        }

        private void SetColumnWidths()
        {
            var widths = new System.Collections.Generic.Dictionary<string, int>
            {
                { "ID",           80  },
                { "SpriteName",   120 },
                { "Class",        50  },
                { "Proficiency",  60  },
                { "Level",        50  },
                { "Gender",       55  },
                { "ReqStrength",  75  },
                { "ReqAgility",   70  },
                { "Col8",         45  },
                { "Col9",         45  },
                { "Type",         50  },
                { "Weight",       60  },
                { "BuyPrice",     80  },
                { "SellPrice",    75  },
                { "MaxAttack",    75  },
                { "MinAttack",    70  },
                { "PhyDefense",   75  },
                { "Frequency",    70  },
                { "Dodge",        50  },
                { "ItemHP",       55  },
                { "ItemMP",       55  },
                { "Durability",   75  },
                { "MaxDurability",90  },
                { "Col23",        45  },
                { "Col24",        45  },
                { "Col25",        45  },
                { "Col26",        45  },
                { "Col27",        45  },
                { "Col28",        45  },
                { "MagicAttack",  80  },
                { "MagicDefense", 85  },
                { "AttackRange",  80  },
                { "Col32",        45  },
                { "Col33",        45  },
                { "Col34",        45  },
                { "Col35",        45  },
                { "CPWorth",      65  },
                { "DisplayName",  140 },
                { "ItemSet",      80  },
                { "Quality",      55  },
                { "ExtraData",    80  },
            };

            foreach (System.Windows.Forms.DataGridViewColumn col in grid.Columns)
            {
                if (widths.TryGetValue(col.Name, out int w))
                    col.Width = w;
                else
                    col.Width = 60;

                // Rename headers to be more readable
                col.HeaderText = col.Name switch
                {
                    "ID"           => "ID",
                    "SpriteName"   => "Sprite",
                    "Class"        => "Class",
                    "Proficiency"  => "Prof",
                    "Level"        => "Lvl",
                    "Gender"       => "Sex",
                    "ReqStrength"  => "ReqStr",
                    "ReqAgility"   => "ReqAgi",
                    "Type"         => "Type",
                    "Weight"       => "Weight",
                    "BuyPrice"     => "Buy $",
                    "SellPrice"    => "Sell $",
                    "MaxAttack"    => "MaxAtk",
                    "MinAttack"    => "MinAtk",
                    "PhyDefense"   => "PhyDef",
                    "Frequency"    => "Freq",
                    "Dodge"        => "Dodge",
                    "ItemHP"       => "HP+",
                    "ItemMP"       => "MP+",
                    "Durability"   => "Dur",
                    "MaxDurability"=> "MaxDur",
                    "MagicAttack"  => "MagAtk",
                    "MagicDefense" => "MagDef",
                    "AttackRange"  => "Range",
                    "CPWorth"      => "CP $",
                    "DisplayName"  => "Display Name",
                    "ItemSet"      => "Set",
                    "Quality"      => "Qual",
                    "ExtraData"    => "Extra",
                    _ => col.Name
                };
            }
        }

        private System.Windows.Forms.ToolStrip           toolStrip;
        private System.Windows.Forms.ToolStripButton     btnOpen;
        private System.Windows.Forms.ToolStripButton     btnSave;
        private System.Windows.Forms.ToolStripButton     btnSaveAs;
        private System.Windows.Forms.ToolStripSeparator  sep1;
        private System.Windows.Forms.ToolStripButton     btnAdd;
        private System.Windows.Forms.ToolStripButton     btnDuplicate;
        private System.Windows.Forms.ToolStripButton     btnDelete;
        private System.Windows.Forms.ToolStripSeparator  sep2;
        private System.Windows.Forms.ToolStripLabel      lblSearchLabel;
        private System.Windows.Forms.ToolStripTextBox    txtSearch;
        private System.Windows.Forms.DataGridView        grid;
        private System.Windows.Forms.StatusStrip         statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
    }
}
