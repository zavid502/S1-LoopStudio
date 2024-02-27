// <copyright file="MainWindow.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace LoopyGUI
{
    using System;
    using System.IO;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using SampleSystem;

    /// <summary>
    /// Main editor logic.
    /// </summary>
    public partial class MainWindow : Window
    {
        private EditorProject editorProject;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
            this.Title = "Loopy";
            this.WindowStyle = WindowStyle.SingleBorderWindow;
            this.ResizeMode = ResizeMode.CanResizeWithGrip;
            this.AllowsTransparency = false;

            this.editorProject = new ();

            this.UpdateGUI();

            this.BpmSelector.KeyDown += this.BpmChanged;
            this.AddNewTrackButton.Click += this.CreateNewTrack;
            this.LengthSelector.KeyDown += this.ChangeLength;

            this.TrackZoomSlider.ValueChanged += this.Zoomer;
            this.ButtonPlay.Click += this.editorProject.Play;
            this.ButtonStop.Click += this.editorProject.Stop;

            this.PositionLabel.Content = 0.ToString() + " / " + this.editorProject.Project.LoopSystem.Length.ToString();

            this.editorProject.Project.LoopSystem.PositionChanged += (sender, e) =>
            {
                this.PositionLabel.Content = (e + 1).ToString() + " / " + this.editorProject.Project.LoopSystem.Length.ToString();
            };

            this.AddSampleSingle.Click += (sender, e) =>
            {
                var dialog = new Microsoft.Win32.OpenFileDialog();
                dialog.DefaultExt = ".wav"; // Default file extension
                dialog.Filter = "Audio Files (.wav)|*.wav"; // Filter files by extension

                // Show open file dialog box
                bool? result = dialog.ShowDialog();

                // Process open file dialog box results
                if (result == true)
                {
                    // Open document
                    string filename = dialog.FileName;
                    this.editorProject.Project.SampleLibrary.AddSample(filename, Path.GetFileName(filename));
                    this.UpdateGUI();
                }
            };

            this.AddSampleDirectory.Click += (sender, e) =>
            {
                System.Windows.Forms.FolderBrowserDialog openFileDlg = new System.Windows.Forms.FolderBrowserDialog();
                var result = openFileDlg.ShowDialog();
                if (result.ToString() != string.Empty)
                {
                    this.editorProject.Project.SampleLibrary.AddDirectoryAsSamples(openFileDlg.SelectedPath, this.RecursiveSampleDirSelector.IsChecked.GetValueOrDefault());
                    this.UpdateGUI();
                }
            };
        }

        private void Zoomer(object sender, EventArgs e)
        {
            this.editorProject.EditorTrackManager.SetTrackZoom((int)this.TrackZoomSlider.Value);
        }

        private void UpdateGUI()
        {
            this.NewTrackSampleSelector.Items.Clear();
            foreach (KeyValuePair<string, List<string>> kv in this.editorProject.Project.SampleLibrary.GetCategorySampleList())
            {
                TreeViewItem category_tree_item = new ()
                {
                    Header = kv.Key,
                    Focusable = false,
                    Foreground = Brushes.WhiteSmoke,
                };

                foreach (string name in kv.Value)
                {
                    TreeViewItem sample_tree_item = new ()
                    {
                        Header = name,
                        Foreground = Brushes.WhiteSmoke,
                    };

                    category_tree_item.Items.Add(sample_tree_item);
                }

                this.NewTrackSampleSelector.Items.Add(category_tree_item);
            }

            this.SegmentPresetList.Children.Clear();
            foreach (EditorSegmentPreset preset in this.editorProject.EditorSegmentPresetLibrary.EditorSegmentPresets)
            {
                this.SegmentPresetList.Children.Add(preset);
            }

            this.LengthSelector.Text = this.editorProject.Project.LoopSystem.Length.ToString(new CultureInfo("en-US"));
            this.BpmSelector.Text = this.editorProject.Project.LoopSystem.Bpm.ToString(new CultureInfo("en-US"));
        }

        private void BpmChanged(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
            {
                return;
            }

            if (double.TryParse(this.BpmSelector.Text, out double newBpm) != true)
            {
                MessageBox.Show("BPM must be a numeric value.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (newBpm <= 0)
            {
                MessageBox.Show("BPM must be higher than 0.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            this.editorProject.Project.LoopSystem.SetBPM(newBpm);
            Keyboard.ClearFocus();
        }

        private void CreateNewTrack(object sender, EventArgs e)
        {
            if (this.NewTrackName.Text.Length == 0)
            {
                MessageBox.Show("Track name is not allowed to be blank.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (this.editorProject.Project.TrackManager.GetTrack(this.NewTrackName.Text) != null)
            {
                MessageBox.Show("Track with name \"" + this.NewTrackName.Text + "\" already exists.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            TreeViewItem selection = (TreeViewItem)this.NewTrackSampleSelector.SelectedItem;
            if (selection == null)
            {
                MessageBox.Show("Please select a sample first.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            string? selecteditemString = selection.Header.ToString();
            if (selecteditemString == null)
            {
                MessageBox.Show("Please select a sample first.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            Sample? sample = selection.HasHeader ? this.editorProject.Project.SampleLibrary.GetSample(selecteditemString) : null;
            if (sample == null)
            {
                MessageBox.Show("Could not find sample with name " + selection + ".", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            EditorTrack? newEditorTrack = this.editorProject.EditorTrackManager.AddTrack(this.NewTrackName.Text, sample);
            if (newEditorTrack == null)
            {
                MessageBox.Show("Failed to add track " + this.NewTrackName + ".", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            this.TrackContainer.Children.Add(newEditorTrack.EditorTrackVisual);
            this.NewTrackName.Text = "NewTrack" + new Random().Next(999, 99999);
            Keyboard.ClearFocus();
        }

        private void ChangeLength(object sender, KeyEventArgs e)
        {
            sender = (TextBox)sender;
            if (e.Key != Key.Enter)
            {
                return;
            }

            if (int.TryParse(this.LengthSelector.Text, out int newLength) == true)
            {
                if (newLength <= 0)
                {
                    MessageBoxResult result = MessageBox.Show("Pattern size should be above 0.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    Keyboard.ClearFocus();
                    return;
                }

                if (newLength > 300)
                {
                    MessageBoxResult result = MessageBox.Show("Attempted to set pattern size to a very high value (>300). This might slow doen the program significantly. Continue?", "Warning", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                    if (result != MessageBoxResult.OK)
                    {
                        Keyboard.ClearFocus();
                        return;
                    }
                }

                this.editorProject.EditorTrackManager.ResizeTracks(newLength);
                Keyboard.ClearFocus();
            }
        }

        private void PreRenderButton_Click(object sender, RoutedEventArgs e)
        {
            this.editorProject.Project.AudioHandler.FullRender();
        }

        private void RenderToFile_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.DefaultExt = ".wav"; // Default file extension
            dialog.Filter = "Audio Files (.wav)|*.wav"; // Filter files by extension

            // Show open file dialog box
            bool? result = dialog.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string filename = dialog.FileName;
                this.editorProject.Project.AudioHandler.FullRenderToFile(filename);
                this.UpdateGUI();
            }
        }
    }
}
