using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace VestigeLauncher
{
    public partial class MainWindow : Window
    {
        // ── Server Settings ──────────────────────────────────────────────────────
        private const string ServerIP   = "25.2.56.134";
        private const int    AccPort    = 9958;   // Account server port
        private const int    GamePort   = 5816;   // Game server port

        // ── Paths (relative to launcher exe location) ────────────────────────────
        private static readonly string BaseDir    = AppDomain.CurrentDomain.BaseDirectory;
        private static readonly string GameExe    = Path.Combine(BaseDir, "Play.exe");
        private static readonly string PatchNotes = Path.Combine(BaseDir, "patchnotes.json");

        // All config files the game may read — launcher patches all of them
        private static readonly string[] ConfigPaths = new[]
        {
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "_cfg.ini"),
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.ini"),
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ini", "config.ini"),
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin", "_cfg.ini"),
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin", "config.ini"),
        };

        public MainWindow()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            LoadPatchNotes();
            await CheckFilesAsync();
        }

        // ── Patch Notes ──────────────────────────────────────────────────────────

        private void LoadPatchNotes()
        {
            if (!File.Exists(PatchNotes)) return;

            try
            {
                string json = File.ReadAllText(PatchNotes, Encoding.UTF8);
                var serializer = new DataContractJsonSerializer(typeof(List<PatchEntry>));
                List<PatchEntry> entries;
                using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
                    entries = (List<PatchEntry>)serializer.ReadObject(ms);

                foreach (var entry in entries)
                {
                    PatchNotesPanel.Children.Add(new TextBlock
                    {
                        Text = $"Patch Notes ({entry.Date})",
                        FontSize = 17,
                        FontWeight = FontWeights.Bold,
                        Foreground = Brushes.White,
                        Margin = new Thickness(0, 8, 0, 4)
                    });

                    PatchNotesPanel.Children.Add(new Separator
                    {
                        Background = new SolidColorBrush(Color.FromRgb(50, 50, 50)),
                        Margin = new Thickness(0, 0, 0, 6)
                    });

                    foreach (var line in entry.Notes)
                    {
                        PatchNotesPanel.Children.Add(new TextBlock
                        {
                            Text = line,
                            Foreground = new SolidColorBrush(Color.FromRgb(180, 180, 180)),
                            FontSize = 13,
                            Margin = new Thickness(4, 2, 0, 2),
                            TextWrapping = TextWrapping.Wrap
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                PatchNotesPanel.Children.Add(new TextBlock
                {
                    Text = "Could not load patch notes: " + ex.Message,
                    Foreground = Brushes.IndianRed,
                    FontSize = 12
                });
            }
        }

        // ── File Check ───────────────────────────────────────────────────────────

        private async Task CheckFilesAsync()
        {
            SetStatus("Checking files...", 0);
            PlayButton.IsEnabled = false;

            await Task.Delay(400);
            SetStatus("Checking files...", 50);
            await Task.Delay(300);

            if (File.Exists(GameExe))
            {
                SetStatus("ready to play", 100);
                PlayButton.IsEnabled = true;
            }
            else
            {
                SetStatus("Game not found — Play.exe missing from game folder", 0);
                PlayButton.IsEnabled = false;
            }
        }

        private void SetStatus(string text, double progress)
        {
            StatusText.Text  = text;
            ProgressBar.Value = progress;
        }

        // ── Config Patch ─────────────────────────────────────────────────────────
        // Writes the correct IP and port into ini\config.ini before launching.
        // Handles the standard CO config.ini format:
        //   [Server]
        //   IP=...
        //   Port=...

        private void PatchConfig()
        {
            foreach (var path in ConfigPaths)
            {
                if (!File.Exists(path)) continue;

                var lines = File.ReadAllLines(path);
                for (int i = 0; i < lines.Length; i++)
                {
                    string trimmed = lines[i].TrimStart();
                    if (trimmed.StartsWith("IP=", StringComparison.OrdinalIgnoreCase) ||
                        trimmed.StartsWith("ServerIP=", StringComparison.OrdinalIgnoreCase))
                    {
                        lines[i] = "IP=" + ServerIP;
                    }
                    else if (trimmed.StartsWith("Port=", StringComparison.OrdinalIgnoreCase) ||
                             trimmed.StartsWith("ServerPort=", StringComparison.OrdinalIgnoreCase))
                    {
                        lines[i] = "Port=" + AccPort;
                    }
                }
                File.WriteAllLines(path, lines, Encoding.UTF8);
            }
        }

        // ── Button Handlers ──────────────────────────────────────────────────────

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PatchConfig();

                Process.Start(new ProcessStartInfo
                {
                    FileName         = GameExe,
                    WorkingDirectory = Path.GetDirectoryName(GameExe)
                });

                Application.Current.Shutdown();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Failed to launch game:\n" + ex.Message,
                    "Vestige Launcher",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }
    }

    // ── Data Models ──────────────────────────────────────────────────────────────

    [DataContract]
    public class PatchEntry
    {
        [DataMember(Name = "date")]
        public string Date { get; set; }

        [DataMember(Name = "notes")]
        public List<string> Notes { get; set; }
    }
}
