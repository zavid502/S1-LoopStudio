// <copyright file="Shared.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace LoopyGUI
{
    using System.Windows.Media;

    /// <summary>
    /// Collection of shared functions.
    /// </summary>
    public static class Shared
    {
        /// copy pasted van stackoverflow
        /// <summary>
        /// Change the lightness of a color.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <param name="coef">The coefficient.</param>
        /// <returns>The new color.</returns>
        public static Color ChangeLightness(this Color color, float coef)
        {
            return Color.FromRgb(
                                 (byte)(int)(color.R * coef),
                                 (byte)(int)(color.G * coef),
                                 (byte)(int)(color.B * coef));
        }
    }
}
