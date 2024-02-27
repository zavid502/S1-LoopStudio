// <copyright file="EditorTrack.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace LoopyGUI
{
    using System.Collections.Generic;
    using System.Linq;
    using SampleSystem;

    /// <summary>
    /// Visually represents a track in the editor.
    /// </summary>
    public class EditorTrack
    {
        private EditorProject editorProject;
        private int previousSegment;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorTrack"/> class.
        /// </summary>
        /// <param name="track">The underlying <see cref="Track"/> object.</param>
        /// <param name="segmentWidth">The width of the segments of the new track.</param>
        /// <param name="editorProject">The editor project instance.</param>
        public EditorTrack(Track track, double segmentWidth, EditorProject editorProject)
        {
            this.Track = track;
            this.EditorTrackVisual = new EditorTrackVisual(segmentWidth, this);
            this.SegmentWidth = segmentWidth;
            for (int i = 0; i < this.Track.GetLength(); i++)
            {
                SegmentPreset preset = this.Track.Get(i);
                EditorSegment newSegment = new (editorProject, this, i, preset, this.SegmentWidth);
                this.EditorTrackSegments.Add(newSegment);
                this.EditorTrackVisual.Children.Add(newSegment.EditorSegmentVisual);
            }

            this.editorProject = editorProject;
            this.editorProject.Project.LoopSystem.PositionChanged += SelectHandler;
        }

        /// <summary>
        /// Gets the underlying <see cref="Track"/> object.
        /// </summary>
        public Track Track { get; private set; }

        /// <summary>
        /// Gets a list of every track segment.
        /// </summary>
        public List<EditorSegment> EditorTrackSegments { get; private set; } = new ();

        /// <summary>
        /// Gets the visual representation of the editor track.
        /// </summary>
        public EditorTrackVisual EditorTrackVisual { get; private set; }

        /// <summary>
        /// Gets the width of the segments in this editor track.
        /// </summary>
        public double SegmentWidth { get; private set; }

        /// <summary>
        /// Change the zoom of the segments.
        /// </summary>
        /// <param name="pixels">The new width.</param>
        public void SetSegmentWidth(double pixels)
        {
            if (pixels <= 0)
            {
                return;
            }

            this.SegmentWidth = pixels;
            foreach (EditorSegment segment in this.EditorTrackSegments)
            {
                segment.EditorSegmentVisual.Resize(pixels);
            }
        }

        /// <summary>
        /// Select handler.
        /// </summary>
        /// <param name="sender">.</param>
        /// <param name="e">..</param>
        public void SelectHandler(object? sender, int e)
        {
            this.EditorTrackVisual.Dispatcher.Invoke(() =>
            {
                var prevSegment = this.EditorTrackSegments.ElementAtOrDefault(this.previousSegment);
                if (prevSegment != default)
                {
                    prevSegment.EditorSegmentVisual.Deselect();
                }

                var elem = this.EditorTrackSegments.ElementAtOrDefault(e);
                if (elem != default) {
                    elem.EditorSegmentVisual.Select();
                }

            });
            this.previousSegment = e;
        }

        /// <summary>
        /// Change the width of the segments by an amount.
        /// </summary>
        /// <param name="change">Positive or negative number indicating the change.</param>
        public void ChangeSegmentWidth(double change)
        {
            this.SetSegmentWidth(this.SegmentWidth + change);
        }

        /// <summary>
        /// Remove this track.
        /// </summary>
        public void Remove()
        {
            this.editorProject.EditorTrackManager.RemoveTrack(this);
        }

        /// <summary>
        /// Apply a segment preset to a certain track segment.
        /// </summary>
        /// <param name="position">The position of the segment.</param>
        /// <param name="preset">The segment preset to be applied.</param>
        public void SetSegmentPreset(int position, EditorSegmentPreset preset)
        {
            if (position >= this.EditorTrackSegments.Count)
            {
                return;
            }

            this.EditorTrackSegments[position].SetPreset(preset);
            this.EditorTrackSegments[position].OwnerEditorTrack.Track.Set(position, preset.SegmentPreset);
        }

        /// <summary>
        /// Resize the track.
        /// </summary>
        /// <param name="newLength">The new length of this track.</param>
        public void Resize(int newLength)
        {
            if (newLength <= 0)
            {
                return;
            }

            this.Track.Resize(newLength);

            int diff = newLength - this.EditorTrackSegments.Count;

            if (newLength > this.EditorTrackSegments.Count)
            {
                for (int i = 0; i < diff; i++)
                {
                    EditorSegment segment = new (this.editorProject, this, this.EditorTrackSegments.Count, new EmptySegmentPreset(), this.SegmentWidth);
                    segment.EditorSegmentVisual.Click += this.editorProject.EditorTrackManager.EditorSegmentClickHandler;
                    this.EditorTrackSegments.Add(segment);
                    this.EditorTrackVisual.Children.Add(segment.EditorSegmentVisual);
                }
            }
            else
            {
                for (int i = 0; i < -diff; i++)
                {
                    this.EditorTrackSegments.RemoveAt(this.EditorTrackSegments.Count - 1);
                    this.EditorTrackVisual.Children.RemoveAt(this.EditorTrackVisual.Children.Count - 1);
                }
            }
        }
    }
}
