// <copyright file="AudioHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SampleSystem
{
    using NAudio.Wave;
    using NAudio.Wave.SampleProviders;
    using SampleSystem.Classes;

    /// <summary>
    /// Handles audio.
    /// </summary>
    public class AudioHandler
    {
        private Project project;
        private CachedSample[] cache;

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioHandler"/> class.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <param name="samplerate">Sample rate.</param>
        /// <param name="channels">Channel count.</param>
        public AudioHandler(Project project, int samplerate = 44100, int channels = 2)
        {
            this.WaveFormat = WaveFormat.CreateIeeeFloatWaveFormat(samplerate, channels);

            this.MixingSampleProvider = new MixingSampleProvider(this.WaveFormat);
            this.MixingSampleProvider.ReadFully = true;

            this.WaveOutEvent = new WaveOutEvent();
            this.WaveOutEvent.Init(this.MixingSampleProvider);
            this.WaveOutEvent.Play();

            this.project = project;

            this.cache = new CachedSample[this.project.LoopSystem.Length];

            project.LoopSystem.LengthChanged += (sender, e) =>
            {
                Console.WriteLine("Resized cache to " + e);
                this.cache = new CachedSample[this.project.LoopSystem.Length];
            };
        }

        /// <summary>
        /// Gets the global mixer.
        /// </summary>
        public MixingSampleProvider MixingSampleProvider { get; private set; }

        /// <summary>
        /// Gets the wasapi output device.
        /// </summary>
        public WaveOutEvent WaveOutEvent { get; private set; }

        /// <summary>
        /// Gets the wave format.
        /// </summary>
        public WaveFormat WaveFormat { get; private set; }

        /// <summary>
        /// Reset the entire cache, or the cache at a specific point.
        /// </summary>
        /// <param name="position">Position to remove.</param>
        public void ResetCache(int? position = null)
        {
            Console.WriteLine("Clearing cache (" + ((position == null) ? "everything" : ("position " + (position + 1))) + ")");

            if (position == null)
            {
                this.cache = new CachedSample[this.project.LoopSystem.Length];
                return;
            }

            this.cache.SetValue(null, position.Value);

        }

        /// <summary>
        /// Render a track as audio.
        /// </summary>
        /// <param name="position">Render the audio at a certain position.</param>
        /// <param name="length">How many segments should be rendered.</param>
        /// <param name="playOnRender">Play on render.</param>
        public void RenderPosition(int position, int length, bool playOnRender = true)
        {
            if (this.cache[position] != null)
            {
                Console.WriteLine();
                this.MixingSampleProvider.AddMixerInput(new CachedSoundSampleProvider(this.cache[position]));
                Console.WriteLine(position + 1 + ": audio retrieved from cache");
                return;
            }

            bool renderHasAudio = false;

            MixingSampleProvider sampleProvider = new MixingSampleProvider(this.WaveFormat);
            foreach (Track track in this.project.TrackManager.Tracks.Where(x => x.Get(position) is not EmptySegmentPreset))
            {
                renderHasAudio = true;
                sampleProvider.AddMixerInput(this.SampleAndDelaysToWaveProvider(this.project.LoopSystem.CalcDelays(track, position), track.CachedSample));
            }

            if (renderHasAudio)
            {
                this.cache[position] = new CachedSample(sampleProvider);

                if (playOnRender)
                {
                    this.MixingSampleProvider.AddMixerInput(new CachedSoundSampleProvider(this.cache[position]));
                }
            }
        }

        /// <summary>
        /// Prerender the entire project.
        /// </summary>
        public void FullRender()
        {
            for (int i = 0; i < this.project.LoopSystem.Length; i++)
            {
                this.RenderPosition(i, 1, false);
            }
        }

        public void FullRenderToFile(string filePath)
        {
            this.FullRender();
            ISampleProvider provider = new CachedSoundSampleProvider(this.cache[0]);
            for (int i = 1; i < this.cache.Length; i++)
            {
                provider = provider.FollowedBy(new CachedSoundSampleProvider(this.cache[i]));
            }

            WaveFileWriter.CreateWaveFile(filePath, provider.ToWaveProvider16());
        }

        private IWaveProvider SampleAndDelaysToWaveProvider(double[] delays, CachedSample cachedSample)
        {
            MixingSampleProvider output = new MixingSampleProvider(this.WaveFormat);

            foreach (double delay in delays)
            {
                ISampleProvider readerCopy = new CachedSoundSampleProvider(cachedSample);
                OffsetSampleProvider offset = new OffsetSampleProvider(readerCopy);
                offset.DelayBy = TimeSpan.FromSeconds(delay);
                output.AddMixerInput(offset);
            }

            return output.ToWaveProvider();
        }
    }
}
