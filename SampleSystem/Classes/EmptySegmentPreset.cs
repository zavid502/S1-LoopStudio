// <copyright file="EmptySegmentPreset.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SampleSystem
{
    /// <summary>
    /// Represents an empty segment.
    /// </summary>
    public class EmptySegmentPreset : SegmentPreset
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmptySegmentPreset"/> class.
        /// </summary>
        public EmptySegmentPreset()
            : base(new List<double>(), "Empty")
        {
        }
    }
}
