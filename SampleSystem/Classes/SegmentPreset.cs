// <copyright file="SegmentPreset.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SampleSystem
{
    /// <summary>
    /// Represents a single pattern point.
    /// </summary>
    public class SegmentPreset
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SegmentPreset"/> class.
        /// </summary>
        /// <param name="points">The delays of this pattern point.</param>
        /// <param name="name">The name of this preset.</param>
        public SegmentPreset(List<double> points, string name)
        {
            this.SegmentPoints = points;
            this.Name = name;
        }

        /// <summary>
        /// Gets the list of delays.
        /// </summary>
        public List<double> SegmentPoints { get; private set; }

        /// <summary>
        /// Gets the name of this track segment.
        /// </summary>
        public string Name { get; private set; }
    }
}