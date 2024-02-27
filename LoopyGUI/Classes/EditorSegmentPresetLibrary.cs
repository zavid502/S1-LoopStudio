// <copyright file="EditorSegmentPresetLibrary.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace LoopyGUI
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Media;
    using ColorHelper;
    using SampleSystem;

    /// <summary>
    /// Visual representation of the segment preset library.
    /// </summary>
    public class EditorSegmentPresetLibrary
    {
        private EditorProject editorProject;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorSegmentPresetLibrary"/> class.
        /// </summary>
        /// <param name="segmentPresetLibrary">The segment preset library to be used.</param>
        /// <param name="editorProject">The editor project instance.</param>
        public EditorSegmentPresetLibrary(SegmentPresetLibrary segmentPresetLibrary, EditorProject editorProject)
        {
            this.editorProject = editorProject;
            this.SegmentPresetLibrary = segmentPresetLibrary;
            this.EditorSegmentPresets = new ();

            foreach (SegmentPreset preset in segmentPresetLibrary.SegmentPresets)
            {

                EditorSegmentPreset newPreset = new (preset.Name, preset, this.SegmentColorGenerator(preset));
                newPreset.Click += this.EditorSegmentPresetClickHandler;
                this.EditorSegmentPresets.Add(newPreset);
            }

            this.SelectedPreset = this.EditorSegmentPresets[0];
        }

        /// <summary>
        /// Click handler for editor segment presets.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Eventargs.</param>
        public void EditorSegmentPresetClickHandler(object sender, EventArgs e)
        {
            this.editorProject.selectedPreset.Deselect();
            this.editorProject.selectedPreset = (EditorSegmentPreset)sender;
            this.editorProject.selectedPreset.Select();
        }

        /// <summary>
        /// Gets the list of segment presets in this library.
        /// </summary>
        public SegmentPresetLibrary SegmentPresetLibrary { get; private set; }

        /// <summary>
        /// Gets the list of editor segment presets.
        /// </summary>
        public List<EditorSegmentPreset> EditorSegmentPresets { get; private set; }

        /// <summary>
        /// Gets the currently selected segment preset.
        /// </summary>
        public EditorSegmentPreset SelectedPreset { get; private set; }

        /// <summary>
        /// Adds a preset to this library.
        /// </summary>
        /// <param name="points">The segment preset to add.</param>
        /// <param name="name">The name this preset should have.</param>
        /// <param name="backgroundColor">The background color of this preset.</param>
        /// <returns>A bool indicating whether the preset was added successfully.</returns>
        public EditorSegmentPreset? AddPreset(List<double> points, string name, Color backgroundColor)
        {
            if (this.ContainsName(name))
            {
                return null;
            }

            SegmentPreset? preset = this.SegmentPresetLibrary.AddPreset(points, name);
            if (preset == null)
            {
                return null;
            }

            EditorSegmentPreset editorPreset = new (name, preset, backgroundColor);
            this.EditorSegmentPresets.Add(editorPreset);
            return editorPreset;
        }

        /// <summary>
        /// CHeck whether a name already exists in this library.
        /// </summary>
        /// <param name="name">The name being checked.</param>
        /// <returns>A bool indicating whether this name already exists.</returns>
        public bool ContainsName(string name)
        {
            return this.SegmentPresetLibrary.NameExists(name);
        }

        private Color SegmentColorGenerator(SegmentPreset preset)
        {
            Random randomSeeded = new Random(preset.GetHashCode());
            RGB color;
            if (preset is EmptySegmentPreset)
            {
                color = new (70, 70, 70);
            }
            else
            {
                color = ColorHelper.ColorConverter.HslToRgb(new HSL(randomSeeded.Next(0, 255), 20, 255));
            }

            return Color.FromRgb(color.R, color.G, color.B);
        }
    }
}