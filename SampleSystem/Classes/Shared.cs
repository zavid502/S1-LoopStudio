// <copyright file="Shared.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SampleSystem.Classes
{
    /// <summary>
    /// Collection of generic methods.
    /// </summary>
    public class Shared
    {
        /// <summary>
        /// Checks if an object is null.
        /// </summary>
        /// <param name="obj">The object to check.</param>
        /// <returns>Bool indicating whether the object is empty or not.</returns>
        public static bool EmptyChecker(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (obj is string str)
            {
                if (str == string.Empty)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
