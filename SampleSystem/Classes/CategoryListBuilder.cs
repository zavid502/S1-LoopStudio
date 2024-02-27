// <copyright file="CategoryListBuilder.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SampleSystem.Classes
{
    using System.Collections.Generic;

    /// <summary>
    /// Helps generate a list of categories based off of a filename.
    /// </summary>
    public class CategoryListBuilder
    {
        private string fileName;
        private List<string> categories = new ();
        private List<string> possibleCategories = new () {
            "kick", "snare", "hihat", "closedhihat", "openhihat",
            "closedhat", "openhat", "sfx", "clap", "loop",
            "shaker", "scratch", "tom", "drum", "djembe",
            "piano", "guitar", "vocal", "bass",
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryListBuilder"/> class.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        public CategoryListBuilder(string fileName)
        {
            this.fileName = fileName.ToLower().Replace(" ", string.Empty).Replace("_", string.Empty).Replace("-", string.Empty);
        }

        /// <summary>
        /// Add a number of categories to check against.
        /// </summary>
        /// <param name="categories">The categories to check against.</param>
        public void AddCategoriesToCheckAgainst(params string[] categories)
        {
            for (int i = 0; i < categories.Length; i++)
            {
                string category = categories[i].ToLower();
                if (string.IsNullOrEmpty(category) || this.possibleCategories.Contains(category))
                {
                    continue;
                }
            }
        }

        /// <summary>
        /// Generates a list of categories.
        /// </summary>
        /// <returns>A list of categories.</returns>
        public List<string> GetCategories()
        {
            this.categories.Clear();
            foreach (string category in this.possibleCategories)
            {
                this.CheckForCategory(category);
            }

            return (this.categories.Count == 0) ? new List<string>() { "other" } : this.categories;
        }

        /// <summary>
        /// Check if the file has a category.
        /// </summary>
        /// <param name="category">The category to be checked.</param>
        private void CheckForCategory(string category)
        {
            category = category.ToLower().Replace(" ", string.Empty);

            if (string.IsNullOrEmpty(category))
            {
                return;
            }

            if (!this.fileName.Contains(category))
            {

                return;
            }

            this.categories.Add(category);
        }
    }
}
