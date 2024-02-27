// <copyright file="EditorTrackVisual.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace LoopyGUI
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using ColorHelper;

    /// <summary>
    /// Visual representation of a track.
    /// </summary>
    public class EditorTrackVisual : StackPanel
    {
        private Label trackNameLabel;
        private Label selectedSampleLabel;
        private StackPanel segmentStackPanel;
        private StackPanel infoStackPanel;
        private Button removeButton;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorTrackVisual"/> class.
        /// </summary>
        /// <param name="segmentWidth">The width of the child segments.</param>
        /// <param name="parentTrack">The parent track object.</param>
        public EditorTrackVisual(double segmentWidth, EditorTrack parentTrack)
        {
            RGB trackColorA = ColorHelper.ColorConverter.HslToRgb(new HSL(new Random().Next(0, 255), 255, 25));
            Color color = Color.FromRgb(trackColorA.R, trackColorA.G, trackColorA.B);

            this.Width = double.NaN;
            this.Height = 90;
            this.Background = Brushes.Transparent;
            this.Orientation = Orientation.Horizontal;
            this.Margin = new Thickness(0, 0, 0, 4);

            this.infoStackPanel = new StackPanel
            {
                Width = 120,
                Orientation = Orientation.Vertical,
                Background = new LinearGradientBrush(color, color.ChangeLightness(0.9f), 45),
                Margin = new Thickness(0, 0, 5, 0),
            };

            this.trackNameLabel = new Label
            {
                Content = parentTrack.Track.Name,
                Width = double.NaN,
                FontWeight = FontWeights.Bold,
            };

            this.selectedSampleLabel = new Label
            {
                Content = parentTrack.Track.SelectedSample.Name,
                Width = double.NaN,
                FontWeight = FontWeights.Light,
            };

            this.segmentStackPanel = new StackPanel
            {
                Height = double.NaN,
                Orientation = Orientation.Horizontal,
            };

            this.removeButton = new Button
            {
                Content = "remove",
                Background = Brushes.Transparent,
                BorderBrush = Brushes.Transparent,
                HorizontalContentAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(3, 0, 0, 3),
                VerticalAlignment = VerticalAlignment.Bottom,
            };

            this.removeButton.Click += (sender, e) =>
            {
                parentTrack.Remove();
                ((StackPanel)this.Parent).Children.Remove(this);
            };

            this.infoStackPanel.Children.Add(this.trackNameLabel);
            this.infoStackPanel.Children.Add(this.selectedSampleLabel);
            this.infoStackPanel.Children.Add(this.removeButton);

            this.Children.Add(this.infoStackPanel);
            this.Children.Add(this.segmentStackPanel);
        }
    }
}
