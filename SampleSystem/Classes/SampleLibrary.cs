// <copyright file="SampleLibrary.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SampleSystem
{
    using System.IO;
    using System.Linq;
    using SampleSystem.Classes;

    /// <summary>
    /// Represents a library of samples.
    /// </summary>
    public class SampleLibrary
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SampleLibrary"/> class.
        /// </summary>
        public SampleLibrary()
        {
            this.Samples = new List<Sample>();
        }

        /// <summary>
        /// Gets the list of samples in this library.
        /// </summary>
        public List<Sample> Samples { get; private set; }

        /// <summary>
        /// Get every wave file in a directory and add it as a sample.
        /// </summary>
        /// <param name="sampleDirectory">The path of the directory.</param>
        /// <param name="recursive">Indicates whether directory should be recursively read.</param>
        public void AddDirectoryAsSamples(string sampleDirectory, bool recursive = false)
        {
            if (!Directory.Exists(sampleDirectory))
            {
                Console.Error.WriteLine("Sample directory " + sampleDirectory + " does not exist.");
                return;
            }
            else
            {
                foreach (string path in Directory.EnumerateFiles(sampleDirectory, "*.wav",  new EnumerationOptions() { RecurseSubdirectories = recursive }))
                {
                    this.AddSample(path, Path.GetFileName(path));
                }
            }
        }

        /// <summary>
        /// Adds a sample to the library.
        /// </summary>
        /// <param name="filePath">The file path of the sample.</param>
        /// <param name="name">The name of the sample.</param>
        public void AddSample(string filePath, string name)
        {
            Console.WriteLine("Attempting to add sample: " + filePath);
            if (!File.Exists(filePath))
            {
                Console.Error.WriteLine("Sample " + filePath + " does not exist.");
                return;
            }

            if (this.NameExists(name))
            {
                return;
            }

            if (Path.GetExtension(filePath) == ".wav")
            {
                foreach (string cat in new CategoryListBuilder(Path.GetFileName(filePath)).GetCategories())
                {
                    this.Samples.Add(new Sample(cat, filePath, name));
                }
            }
        }

        /// <summary>
        /// Get the sample object.
        /// </summary>
        /// <param name="sampleName">The name of the sample you're searching for.</param>
        /// <returns>The sample, or null if not found.</returns>
        public Sample? GetSample(string sampleName)
        {
            Sample? result = this.Samples.Where(x => x.Name == sampleName).First();
            return result;
        }

        /// <summary>
        /// Check whether a name is already taken.
        /// </summary>
        /// <param name="name">The name being checked.</param>
        /// <returns>A bool indicating whether the name is taken.</returns>
        public bool NameExists(string name)
        {
            return this.Samples.Where(x => x.Name == name).Any();
        }

        /// <summary>
        /// Sorts samples by categories.
        /// </summary>
        /// <returns>A dictionary with distinct categories as keys and a list of samples as values.</returns>
        public Dictionary<string, List<string>> GetCategorySampleList()
        {
            Dictionary<string, List<string>> ret = new ();

            foreach (string category in this.Samples.Select(x => x.Category).Distinct())
            {
                ret.Add(category, new List<string>());
                foreach (string name in this.Samples.Where(x => x.Category == category).Select(x => x.Name))
                {
                    ret[category].Add(name);
                }
            }

            return ret;
        }
    }
}