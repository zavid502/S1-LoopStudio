// <copyright file="EditorSegment.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace LoopyGUI
{
    using SampleSystem;

    /// <summary>
    /// Represents a track segment.
    /// </summary>
    public class EditorSegment
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EditorSegment"/> class.
        /// </summary>
        /// <param name="editorProject">..</param>
        /// <param name="owner">The <see cref="Track"/> object that owns this segment.</param>
        /// <param name="position">The position this segment is at in the track.</param>
        /// <param name="width">The width of the segment.</param>
        /// <param name="segmentPreset">The segment preset this segment has.</param>
        public EditorSegment(EditorProject editorProject, EditorTrack owner, int position, SegmentPreset segmentPreset, double width)
        {
            this.EditorSegmentVisual = new (this, width, position);
            this.OwnerEditorTrack = owner;
            this.SegmentPosition = position;

            this.SegmentPosition = position;

            if (segmentPreset != null)
            {
                this.SegmentPreset = segmentPreset;
            }
            this.EditorSegmentVisual = new (this, width, position);

            // editorProject.Project.LoopSystem.PositionChanged += this.SelectHandler;
        }

        /// <summary>
        /// Gets the <see cref="Track"/> object this segment belongs to.
        /// </summary>
        public EditorTrack OwnerEditorTrack { get; private set; }

        /// <summary>
        /// Gets the preset of the segment.
        /// </summary>
        public SegmentPreset SegmentPreset { get; private set; } = new EmptySegmentPreset();

        /// <summary>
        /// Gets the position of this segment.
        /// </summary>
        public int SegmentPosition { get; private set; }

        /// <summary>
        /// Gets the visual representation of this editor segment.
        /// </summary>
        public EditorSegmentVisual EditorSegmentVisual { get; private set; }

        /// <summary>
        /// Sets the preset of this segment.
        /// </summary>
        /// <param name="preset">The preset this segment should be set to.</param>
        public void SetPreset(EditorSegmentPreset preset)
        {
            this.SegmentPreset = preset.SegmentPreset;
            this.OwnerEditorTrack.Track.Set(this.SegmentPosition, preset.SegmentPreset);

            if (preset.SegmentPreset is not EmptySegmentPreset)
            {
                this.EditorSegmentVisual.Background = preset.VisualStyle;
                this.EditorSegmentVisual.BorderBrush = preset.VisualStyle;
                this.EditorSegmentVisual.Content = preset.PresetName;
            }
            else
            {
                this.EditorSegmentVisual.CheckeredBackground();
                this.EditorSegmentVisual.Content = string.Empty;
            }
        }
    }
}
