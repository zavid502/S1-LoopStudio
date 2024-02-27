// <copyright file="EditorTrackManager.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace LoopyGUI.Classes
{
    using SampleSystem;
    using SampleSystem.Classes;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The editor track manager.
    /// </summary>
    public class EditorTrackManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EditorTrackManager"/> class.
        /// </summary>
        /// <param name="trackManager">The track manager.</param>
        public EditorTrackManager(EditorProject editorProject, TrackManager trackManager, double segmentWidth)
        {
            this.EditorProject = editorProject;
            this.SegmentWidth = segmentWidth;
        }

        public double SegmentWidth;

        public EditorProject EditorProject { get; private set; }

        /// <summary>
        /// Gets a list of editor tracks this instance uses.
        /// </summary>
        public List<EditorTrack> EditorTracks { get; private set; } = new List<EditorTrack>();

        /// <summary>
        /// Adds a track.
        /// </summary>
        /// <param name="newTrackName">The name of the new track.</param>
        /// <param name="sample">The selected sample.</param>
        /// <returns>A bool indicating whether the track was added successfully.</returns>
        public EditorTrack? AddTrack(string newTrackName, Sample sample)
        {
            if (this.EditorProject.Project.TrackManager.NameExists(newTrackName))
            {
                return null;
            }

            Track newTrack = this.EditorProject.Project.TrackManager.AddTrack(newTrackName, sample);
            EditorTrack newEditorTrack = new (newTrack, this.SegmentWidth, this.EditorProject);

            foreach (EditorSegment segment in newEditorTrack.EditorTrackSegments)
            {
                segment.EditorSegmentVisual.Click += this.EditorSegmentClickHandler;
            }

            this.EditorTracks.Add(newEditorTrack);

            return newEditorTrack;
        }

        /// <summary>
        /// Removes a track.
        /// </summary>
        /// <param name="editorTrack">The track to be removed.</param>
        public void RemoveTrack(EditorTrack editorTrack)
        {
            this.EditorTracks.Remove(editorTrack);
            this.EditorProject.Project.TrackManager.RemoveTrack(editorTrack.Track);
        }

        /// <summary>
        /// Resizes every track to a new length.
        /// </summary>
        /// <param name="newLength">THe new length of the tracks.</param>
        public void ResizeTracks(int newLength)
        {
            if (newLength <= 0)
            {
                return;
            }

            this.EditorProject.Project.LoopSystem.Resize(newLength);
            foreach (EditorTrack track in this.EditorTracks)
            {
                track.Resize(newLength);
            }
        }

        /// <summary>
        /// Sets the zoom level of the tracks.
        /// </summary>
        /// <param name="zoomLevel">The zoom amount.</param>
        public void SetTrackZoom(int zoomLevel)
        {
            if (zoomLevel <= 10)
            {
                return;
            }

            this.SegmentWidth = zoomLevel;
            foreach (EditorTrack editorTrack in this.EditorTracks)
            {
                editorTrack.SetSegmentWidth(zoomLevel);
            }
        }

        /// <summary>
        /// Click handler for editor segments.
        /// </summary>
        /// <param name="sender">The sender segment.</param>
        /// <param name="e">The event args.</param>
        public void EditorSegmentClickHandler(object sender, EventArgs e)
        {
            ((EditorSegmentVisual)sender).EditorSegment.SetPreset(this.EditorProject.selectedPreset);
        }

        /// <summary>
        /// Returns a list of every editor segment at a certain position.
        /// </summary>
        /// <param name="position">The position of the segments.</param>
        /// <returns>A list of editor segments at the specified position.</returns>
        public List<EditorSegment>? GetEditorSegmentsAtPosition(int position)
        {
            if (position >= this.EditorProject.Project.LoopSystem.Length)
            {
                return null;
            }

            List<EditorSegment>? editorSegments = new ();

            foreach (EditorTrack track in this.EditorTracks)
            {
                editorSegments.Add(track.EditorTrackSegments.First(x => x.SegmentPosition == position));
            }

            return editorSegments;
        }
    }
}
