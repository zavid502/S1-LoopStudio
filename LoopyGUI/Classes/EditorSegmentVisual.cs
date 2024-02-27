// <copyright file="EditorSegmentVisual.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace LoopyGUI
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    /// <summary>
    /// Visual representation of a track segment.
    /// </summary>
    public class EditorSegmentVisual : Button
    {
        private SolidColorBrush bgA = new SolidColorBrush(Color.FromRgb(80, 80, 80));
        private SolidColorBrush bgB = new SolidColorBrush(Color.FromRgb(100, 100, 100));
        private int segmentPosition;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorSegmentVisual"/> class.
        /// </summary>
        /// <param name="editorSegment">The parent editor segment.</param>
        /// <param name="width">The width of this segment.</param>
        /// <param name="segmentPosition">The position of the segment.</param>
        public EditorSegmentVisual(EditorSegment editorSegment, double width, int segmentPosition)
        {
            this.segmentPosition = segmentPosition;
            this.CheckeredBackground();
            this.EditorSegment = editorSegment;

            this.Content = string.Empty;
            this.Width = width;
            this.Height = double.NaN;
            this.BorderThickness = new Thickness(5, 5, 5, 5);
            this.BorderBrush = this.Background;
            this.HorizontalContentAlignment = HorizontalAlignment.Center;
            this.VerticalContentAlignment = VerticalAlignment.Center;
            this.FontWeight = FontWeights.Bold;
            this.Foreground = new SolidColorBrush(Color.FromRgb(245, 245, 245));
            this.Cursor = Cursors.Hand;
            this.SnapsToDevicePixels = true;
        }

        /// <summary>
        /// Gets the parent editor segment.
        /// </summary>
        public EditorSegment EditorSegment { get; private set; }

        /// <summary>
        /// Checkered background.
        /// </summary>
        public void CheckeredBackground()
        {
            // this.Background = (this.segmentPosition % 2 == 0) ? new SolidColorBrush(Color.FromArgb(15, 0, 0, 0)) : new SolidColorBrush(Color.FromArgb(30, 0, 0, 0));
            this.Background = (this.segmentPosition % 2 == 0) ? this.bgA : this.bgB;
            this.BorderBrush = this.Background;
        }

        /// <summary>
        /// Visually select this segment.
        /// </summary>
        public void Select()
        {
            this.Dispatcher.Invoke(() =>
            {
                this.BorderBrush = Brushes.WhiteSmoke;
            });
        }

        /// <summary>
        /// Visually deselect this item.
        /// </summary>
        public void Deselect()
        {
            this.Dispatcher.Invoke(() =>
            {
                this.BorderBrush = this.Background;
            });
        }

        /// <summary>
        /// Resize the visual container.
        /// </summary>
        /// <param name="newSizePX">The new size of the element.</param>
        public void Resize(double newSizePX)
        {
            if (newSizePX <= 0)
            {
                return;
            }

            this.Width = newSizePX;
        }
    }
}
