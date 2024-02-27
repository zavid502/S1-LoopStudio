// <copyright file="Track.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SampleSystem
{
    using SampleSystem.Classes;
    using System.Globalization;
    using System.Linq;

    /// <summary>
    /// Represents a track.
    /// </summary>
    public class Track
    {
        private Project project;
        private List<SegmentPreset> pattern;

        /// <summary>
        /// Initializes a new instance of the <see cref="Track"/> class.
        /// </summary>
        /// <param name="newName">Name of the track.</param>
        /// <param name="newLength">Length of the track. Should usually be identical to the total length of the pattern.</param>
        /// <param name="selectedSample">The currently selected sample of this track.</param>
        public Track(Project project, string? newName, int newLength, Sample selectedSample)
        {
            this.project = project;
            newLength = (newLength <= 0) ? 1 : newLength;

            if (newName == null)
            {
                this.Name = "NewTrack" + new Random().Next(9999, 99999999).ToString(new CultureInfo("en-US"));
            }
            else
            {
                this.Name = newName;
            }

            this.pattern = Enumerable.Repeat(new EmptySegmentPreset(), newLength).ToList<SegmentPreset>();
            this.SelectedSample = selectedSample;
            this.CachedSample = new CachedSample(this.SelectedSample.FilePath, this.project.AudioHandler.WaveFormat);
        }

        /// <summary>
        /// Gets the name of the track.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the currently selected sample.
        /// </summary>
        public Sample SelectedSample { get; private set; }

        /// <summary>
        /// Gets the cached sample data of the loaded sample.
        /// </summary>
        public CachedSample CachedSample { get; private set; }

        /// <summary>
        /// Change the currently selected sample.
        /// </summary>
        /// <param name="sample">The sample which should be used.</param>
        public void SetSample(Sample sample)
        {
            this.SelectedSample = sample;
            this.CachedSample = new CachedSample(this.SelectedSample.FilePath, this.project.AudioHandler.WaveFormat);
            this.project.AudioHandler.ResetCache();
        }

        /// <summary>
        /// Applies a <see cref="SegmentPreset"/> to a certain segment.
        /// </summary>
        /// <param name="position">The pattern point that should be set.</param>
        /// <param name="segmentPreset">The <see cref="SegmentPreset"/> that should be used.</param>
        public void Set(int position, SegmentPreset segmentPreset)
        {
            if (position > this.pattern.Count)
            {
                return;
            }

            this.pattern[position] = segmentPreset;
            this.project.AudioHandler.ResetCache(position);
        }

        /// <summary>
        /// Get the <see cref="SegmentPreset"/> at a certain position.
        /// </summary>
        /// <param name="position">The target positionn.</param>
        /// <returns>The requested TrackTrigger.</returns>
        public SegmentPreset Get(int position)
        {
            // if(Position > Pattern.Count())
            // {
            //     return null;
            // }

            if (position >= this.pattern.Count)
            {
                return new EmptySegmentPreset();
            }

            return this.pattern[position];
        }

        /// <summary>
        /// Resizes the track.
        /// </summary>
        /// <param name="newLength">The new length of the track.</param>
        public void Resize(int newLength)
        {
            int oldLength = this.pattern.Count;

            if (newLength <= 0)
            {
                return;
            }

            if (newLength > oldLength)
            {
                for (int i = 0; i < newLength - oldLength; i++)
                {
                    this.pattern.Add(new EmptySegmentPreset());
                }
            }
            else
            {
                for (int i = 0; i < oldLength - newLength; i++)
                {
                    this.pattern.RemoveAt(this.pattern.Count - 1);
                }
            }
        }

        /// <summary>
        /// Get the length of the track.
        /// </summary>
        /// <returns>The length of the track.</returns>
        public int GetLength()
        {
            return this.pattern.Count;
        }

        /// <summary>
        /// Replaces every pattern point with an EmptyTrigger trigger.
        /// </summary>
        public void Clear()
        {
            int count = this.pattern.Count;
            this.pattern = Enumerable.Repeat(new EmptySegmentPreset(), count).ToList<SegmentPreset>();
        }
    }
}