// <copyright file="SegmentPresetLibrary.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SampleSystem
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents a lbrary of track segments.
    /// </summary>
    public class SegmentPresetLibrary
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SegmentPresetLibrary"/> class.
        /// </summary>
        public SegmentPresetLibrary()
        {
            this.SegmentPresets = new List<SegmentPreset>
            {
                new EmptySegmentPreset(),
            };
        }

        /// <summary>
        /// Gets every <see cref="SegmentPreset"/> in this library.
        /// </summary>
        public List<SegmentPreset> SegmentPresets { get; private set; }

        /// <summary>
        /// Add a segment preset to this library.
        /// </summary>
        /// <param name="points">The points of the segment preset.</param>
        /// <param name="name">The name of this preset.</param>
        /// <returns>SegmentPreset if successfully added, null if failed.</returns>
        public SegmentPreset? AddPreset(List<double> points, string name)
        {
            if (this.NameExists(name))
            {
                return null;
            }

            SegmentPreset newPreset = new (points, name);
            this.SegmentPresets.Add(newPreset);
            return newPreset;
        }

        /// <summary>
        /// Check whether a preset name is already taken.
        /// </summary>
        /// <param name="name">The name that is being checked.</param>
        /// <returns>A bool indicating whether the name already exists.</returns>
        public bool NameExists(string name)
        {
            return this.SegmentPresets.Where(x => x.Name == name).Any();
        }
    }
}
