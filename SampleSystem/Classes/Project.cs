// <copyright file="Project.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SampleSystem
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using SampleSystem.Classes;

    /// <summary>
    /// Class that contains and manages all the parts needed for the loop system.
    /// </summary>
    public class Project
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Project"/> class.
        /// </summary>
        public Project()
        {
            this.SampleLibrary = new SampleLibrary();

            this.SampleLibrary.AddSample("./StarterPack/Kick.wav", "Kick 1");
            this.SampleLibrary.AddSample("./StarterPack/Closed Hat.wav", "Closed Hat 1");

            this.SegmentPresetLibrary.AddPreset(new List<double>() { 0 }, "Single");
            this.SegmentPresetLibrary.AddPreset(new List<double>() { 0.75 }, "Last");
            this.SegmentPresetLibrary.AddPreset(new List<double>() { 0, 0.5 }, "Double");
            this.SegmentPresetLibrary.AddPreset(new List<double>() { 0, 0.25, 0.50 }, "Triple");
            this.SegmentPresetLibrary.AddPreset(new List<double>() { 0, 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9 }, "yeow");

            this.LoopSystem = new LoopSystem(this, this.SampleLibrary);
            this.TrackManager = new TrackManager(this, this.LoopSystem.Length, null);

            this.AudioHandler = new AudioHandler(this);

            this.LoopSystem.PositionChanged += (sender, e) =>
            {
                if (!this.LoopSystem.IsPlaying)
                {
                    return;
                }

                Task.Run(() => this.AudioHandler.RenderPosition(e, 1));
            };
        }

        /// <summary>
        /// Gets the audio handler.
        /// </summary>
        public AudioHandler AudioHandler { get; private set; }

        /// <summary>
        /// Gets the SampleLibrary that the instance uses.
        /// </summary>
        public SampleLibrary SampleLibrary { get; private set; }

        /// <summary>
        /// Gets the LoopSystem that the instance uses.
        /// </summary>
        public LoopSystem LoopSystem { get; private set; }

        /// <summary>
        /// Gets the trackmanager.
        /// </summary>
        public TrackManager TrackManager { get; private set; }

        /// <summary>
        /// Gets the <see cref="SegmentPresetLibrary"/> that the instance uses.
        /// </summary>
        public SegmentPresetLibrary SegmentPresetLibrary { get; private set; } = new ();
    }
}
