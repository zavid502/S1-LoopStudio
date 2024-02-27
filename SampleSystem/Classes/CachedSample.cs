namespace SampleSystem.Classes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NAudio.Wave;

    // aangepaste snippets van mark heath

    /// <summary>
    /// Cached sample data.
    /// </summary>
    public class CachedSample
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CachedSample"/> class.
        /// </summary>
        /// <param name="samplePath">Path of the audio file.</param>
        public CachedSample(string samplePath, WaveFormat format)
        {
            using (var audioFileReader = new AudioFileReader(samplePath))
            {
                // this.WaveFormat = audioFileReader.WaveFormat;
                this.WaveFormat = format;
                var wholeFile = new List<float>((int)(audioFileReader.Length / 4));
                var readBuffer = new float[audioFileReader.WaveFormat.SampleRate * audioFileReader.WaveFormat.Channels];
                int samplesRead;
                while ((samplesRead = audioFileReader.Read(readBuffer, 0, readBuffer.Length)) > 0)
                {
                    wholeFile.AddRange(readBuffer.Take(samplesRead));
                }

                this.AudioData = wholeFile.ToArray();
            }
        }

        public CachedSample(ISampleProvider sampleProvider)
        {
            this.WaveFormat = sampleProvider.WaveFormat;

            List<float> totalSamples = new List<float>();

            float[] buffer = new float[1];

            int offset = 0;

            while (sampleProvider.Read(buffer, 0, 1) > 0)
            {
                totalSamples.AddRange(buffer);
                offset++;
            }

            this.AudioData = totalSamples.ToArray()[..totalSamples.Count];
        }

        /// <summary>
        /// Gets the raw audio data.
        /// </summary>
        public float[] AudioData { get; private set; }

        /// <summary>
        /// Gets the wave format of this sample.
        /// </summary>
        public WaveFormat WaveFormat { get; private set; }
    }

    class CachedSoundSampleProvider : ISampleProvider
    {
        private readonly CachedSample cachedSample;
        private long position;

        public CachedSoundSampleProvider(CachedSample cachedSample)
        {
            this.cachedSample = cachedSample;
        }

        public WaveFormat WaveFormat
        {
            get
            {
                return this.cachedSample.WaveFormat;
            }
        }

        public int Read(float[] buffer, int offset, int count)
        {
            var availableSamples = this.cachedSample.AudioData.Length - this.position;
            var samplesToCopy = Math.Min(availableSamples, count);
            Array.Copy(this.cachedSample.AudioData, this.position, buffer, offset, samplesToCopy);
            this.position += samplesToCopy;
            return (int)samplesToCopy;
        }
    }
}
