// <copyright file="EditorProject.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace LoopyGUI
{
    using System;
    using LoopyGUI.Classes;
    using SampleSystem;

    /// <summary>
    /// Visual representation of the <see cref="Project"/> class.
    /// </summary>
    public class EditorProject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EditorProject"/> class.
        /// </summary>
        /// <param name="segmentWidth">Width of the track segments.</param>
        public EditorProject(double segmentWidth = 75)
        {
            this.Project = new ();
            this.EditorSegmentPresetLibrary = new (this.Project.SegmentPresetLibrary, this);
            this.selectedPreset = this.EditorSegmentPresetLibrary.EditorSegmentPresets[0];
            this.EditorTrackManager = new EditorTrackManager(this, this.Project.TrackManager, segmentWidth);
        }

        /// <summary>
        /// Gets the project object this instance uses.
        /// </summary>
        public Project Project { get; private set; }

        /// <summary>
        /// Gets the editor track manager.
        /// </summary>
        public EditorTrackManager EditorTrackManager { get; private set; }

        public EditorSegmentPreset selectedPreset { get; set; }

        /// <summary>
        /// Gets the editorsegmentpresetlibrary this instance uses.
        /// </summary>
        public EditorSegmentPresetLibrary EditorSegmentPresetLibrary { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the loop is currently playing.
        /// </summary>
        public bool Playing { get; private set; } = false;

        /// <summary>
        /// Start playing.
        /// </summary>
        /// <param name="sender">.</param>
        /// <param name="e">..</param>
        public void Play(object sender, EventArgs e)
        {
            Console.WriteLine("Start playing");
            this.Project.LoopSystem.Play();
        }

        /// <summary>
        /// Stop playing.
        /// </summary>
        /// <param name="sender">.</param>
        /// <param name="e">..</param>
        public void Stop(object sender, EventArgs e)
        {
            Console.WriteLine("Stopping");
            this.Project.LoopSystem.Stop();
        }
    }
}
