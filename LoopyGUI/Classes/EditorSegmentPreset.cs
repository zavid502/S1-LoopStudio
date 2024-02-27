// <copyright file="EditorSegmentPreset.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace LoopyGUI
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using SampleSystem;

    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// </summary>
    public class EditorSegmentPreset : Button
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EditorSegmentPreset"/> class.
        /// Class initializer.
        /// </summary>
        /// <param name="name">Displayed name of the segment.</param>
        /// <param name="segment">The actual TrackSegment object.</param>
        /// <param name="presetColor">The color of the segment.</param>
        /// <exception cref="ArgumentException">Gets thrown if the name is empty.</exception>
        public EditorSegmentPreset(string name, SegmentPreset segment, Color presetColor)
        {
            LinearGradientBrush visualStyle = new (presetColor, presetColor.ChangeLightness(0.95f), 90);

            this.PresetName = name;
            this.SegmentPreset = segment;
            this.VisualStyle = visualStyle;
            this.Background = visualStyle;
            this.Foreground = new SolidColorBrush(presetColor.ChangeLightness(0.2f));
            this.Content = name;
            this.BorderBrush = Brushes.White;
            this.Cursor = Cursors.Hand;
            this.Width = double.NaN;
            this.Height = 40;
            this.Margin = new Thickness(0, 0, 2, 2);
            this.HorizontalContentAlignment = HorizontalAlignment.Center;
            this.VerticalContentAlignment = VerticalAlignment.Center;
            this.FontWeight = FontWeights.SemiBold;
            this.FontSize = 12;
            this.BorderThickness = new Thickness(0);
        }

        /// <summary>
        /// Gets the name of the trigger as displayed in the editor.
        /// </summary>
        public string PresetName { get; private set; }

        /// <summary>
        /// Gets the TrackTrigger associated with this entry.
        /// </summary>
        public SegmentPreset SegmentPreset { get; private set; }

        /// <summary>
        /// Gets the visual brush that gets used on the ui element.
        /// </summary>
        public GradientBrush VisualStyle { get; private set; }

        /// <summary>
        /// Visually mark this preset as selected.
        /// </summary>
        public void Select()
        {
            this.BorderThickness = new Thickness(5);
        }

        /// <summary>
        /// Visually mark this preset as not selected.
        /// </summary>
        public void Deselect()
        {
            this.BorderThickness = new Thickness(0);
        }
    }
}
