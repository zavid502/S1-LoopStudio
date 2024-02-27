// <copyright file="Sample.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SampleSystem
{
    using System.IO;
    using SampleSystem.Classes;

    /// <summary>
    /// Object that represents a sample.
    /// </summary>
    public class Sample
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Sample"/> class.
        /// </summary>
        /// <param name="category">The category of the sample.</param>
        /// <param name="filepath">The file path of the sample.</param>
        /// <param name="name">The unique name of the sample.</param>
        /// <exception cref="FileNotFoundException">Thrown when the sample file is not found.</exception>
        public Sample(string category, string filepath, string name)
        {
            Shared.EmptyChecker(category);
            Shared.EmptyChecker(filepath);
            Shared.EmptyChecker(name);

            if (!File.Exists(filepath))
            {
                throw new FileNotFoundException();
            }

            if (category.Length == 0)
            {
                category = "Other";
            }

            this.Category = category;
            this.FilePath = filepath;
            this.Name = name;
        }

        /// <summary>
        /// Gets the category of the sample.
        /// </summary>
        public string Category { get; private set; }

        /// <summary>
        /// Gets the name of the sample.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the file path of the sample.
        /// </summary>
        public string FilePath { get; private set; }
    }
}