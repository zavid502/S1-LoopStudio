// <copyright file="TrackManager.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SampleSystem.Classes
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Manages all tracks in the project.
    /// </summary>
    public class TrackManager
    {
        private Project project;

        /// <summary>
        /// Initializes a new instance of the <see cref="TrackManager"/> class.
        /// </summary>
        /// <param name="project">Containing project.</param>
        /// <param name="length">The length this system should be initialized with.</param>
        /// <param name="tracks">The tracks this system should be initialized with, null if none.</param>
        public TrackManager(Project project, int length, List<Track>? tracks)
        {
            this.Tracks = tracks ?? new List<Track>();
            this.project = project;
        }

        /// <summary>
        /// Gets the list of tracks in this system.
        /// </summary>
        public List<Track> Tracks { get; private set; }

        /// <summary>
        /// Replace every pattern point of every track to TriggerEmpty.
        /// </summary>
        public void Clear()
        {
            foreach (Track track in this.Tracks)
            {
                track.Clear();
            }

            this.project.AudioHandler.ResetCache();
        }

        /// <summary>
        /// Adds a new track to the loop.
        /// </summary>
        /// <param name="newTrackName">The name of the new track.</param>
        /// <param name="sample">The sample the new track should have.</param>
        /// <returns>The track that has been created.</returns>
        public Track AddTrack(string newTrackName, Sample sample)
        {
            Track newTrack = new (this.project, newTrackName, this.project.LoopSystem.Length, sample);
            this.Tracks.Add(newTrack);
            return newTrack;
        }

        /// <summary>
        /// Remove a track.
        /// </summary>
        /// <param name="track">The track to be removed.</param>
        public void RemoveTrack(Track track)
        {
            this.Tracks.Remove(track);
            this.project.AudioHandler.ResetCache();
        }

        /// <summary>
        /// Gets the Track with a certain name.
        /// </summary>
        /// <param name="name">The name of the track.</param>
        /// <returns>The track at the specified position. Null if a track with that name is not found.</returns>
        public Track? GetTrack(string name)
        {
            foreach (Track track in this.Tracks)
            {
                if (track.Name == name)
                {
                    return track;
                }
            }

            return null;
        }

        /// <summary>
        /// Check whether a track name is taken.
        /// </summary>
        /// <param name="name">The name being checked.</param>
        /// <returns>A bool indicating whether the name is already taken.</returns>
        public bool NameExists(string name)
        {
            return this.Tracks.Where(x => x.Name == name).Any();
        }

        /// <summary>
        /// Get a track from the index.
        /// </summary>
        /// <param name="index">The index of the track.</param>
        /// <returns>The specified track.</returns>
        public Track GetTrack(int index)
        {
            return this.Tracks[index];
        }
    }
}
