// <copyright file="LoopSystem.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SampleSystem
{
    using System.IO;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Class that handles the tracks and the play loop.
    /// </summary>
    public class LoopSystem
    {
        private int position;
        private int length;
        private bool playing = false;
        private CancellationTokenSource tokenSource = new ();
        private Project project;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoopSystem"/> class.
        /// </summary>
        /// <param name="project">Project.</param>
        /// <param name="samples">The sample library used in the loop system.</param>
        /// <param name="patternLength">The length of the loop.</param>
        /// <param name="bpm">The beats per minute of the loop.</param>
        public LoopSystem(Project project, SampleLibrary samples, int patternLength = 4, int bpm = 130)
        {
            if (patternLength <= 0)
            {
                patternLength = 1;
            }

            this.SampleLibrary = samples;
            this.Length = patternLength;
            this.Bpm = bpm;
            this.project = project;
        }

        /// <summary>
        /// Gets raised when the current position of the loop system changes.
        /// </summary>
        public event EventHandler<int>? PositionChanged;

        /// <summary>
        /// Gets raised when the length of the loop system changes.
        /// </summary>
        public event EventHandler<int>? LengthChanged;

        /// <summary>
        /// Gets a value indicating whether the loop is currently playing.
        /// </summary>
        public bool IsPlaying
        {
            get
            {
                return this.playing;
            }
        }

        /// <summary>
        /// Gets the sample library of this loop system.
        /// </summary>
        public SampleLibrary SampleLibrary { get; private set; }

        /// <summary>
        /// Gets the current position of the loop.
        /// </summary>
        public int Position
        {
            get
            {
                return this.position;
            }

            private set
            {
                this.position = value;
                this.PositionChanged?.Invoke(this, this.position);
            }
        }

        /// <summary>
        /// Gets the current BPM.
        /// </summary>
        public double Bpm { get; private set; }

        /// <summary>
        /// Gets the length of the loop.
        /// </summary>
        public int Length
        {
            get
            {
                return this.length;
            }

            private set
            {
                this.length = value;
                this.LengthChanged?.Invoke(this, this.length);
            }
        }

        /// <summary>
        /// Sets the BPM of the loop.
        /// </summary>
        /// <param name="newBPM">The BPM the loop should be set to.</param>
        public void SetBPM(double newBPM)
        {
            if (newBPM > 0)
            {
                this.Bpm = newBPM;
            }
        }

        /// <summary>
        /// Set the length of the loop to a new value.
        /// </summary>
        /// <param name="newLength">The new length of the loop.</param>
        public void Resize(int newLength)
        {
            if (this.Position >= newLength)
            {
                this.Position = 0;
            }

            this.Length = (newLength <= 0) ? 1 : newLength;
            foreach (Track track in this.project.TrackManager.Tracks)
            {
                track.Resize(this.Length);
            }
        }

        /// <summary>
        /// Set the position of the loop to 0.
        /// </summary>
        public void ResetPos()
        {
            this.Position = 0;
        }

        /// <summary>
        /// Start the loop.
        /// </summary>
        public async void Play()
        {
            if (this.playing)
            {
                this.tokenSource.Cancel();
                this.playing = false;
                return;
            }

            this.tokenSource.Dispose();
            this.tokenSource = new CancellationTokenSource();

            this.playing = true;
            while (this.playing)
            {
                await this.Step();
            }
        }

        /// <summary>
        /// Stop the loop.
        /// </summary>
        public void Stop()
        {
            this.playing = false;
            this.tokenSource.Cancel();
            this.ResetPos();
        }

        /// <summary>
        /// Calculate at what delays the samples should be played at.
        /// </summary>
        /// <param name="track">The track of which to calculate the delays.</param>
        /// <param name="position">The position to calculate.</param>
        /// <returns>A list of doubles that represent the delays in seconds.</returns>
        public double[] CalcDelays(Track track, int? position = null)
        {
            SegmentPreset? trig = (position == null) ? track.Get(this.Position) : track.Get(position.GetValueOrDefault());
            if (trig == null) 
            {
                return Array.Empty<double>(); 
            }

            if (trig.SegmentPoints == null)
            {
                return Array.Empty<double>();
            }

            double[] thistrackdelays = new double[trig.SegmentPoints.Count];

            double tickTime = 60D / (double)this.Bpm;

            for (int i = 0; i < trig.SegmentPoints.Count; i++)
            {
                thistrackdelays[i] = trig.SegmentPoints[i] * tickTime;
            }

            return thistrackdelays;
        }

        /// <summary>
        /// Step the loop forward by one.
        /// </summary>
        /// <returns>A task that completes after 60/BPM seconds.</returns>
        public async Task Step()
        {
            float waitMS = (60 / (float)this.Bpm) * 1000;

            var delayTask = Task.Delay((int)waitMS, this.tokenSource.Token).ContinueWith(task => { });
            await delayTask;
            this.Position = (this.Position >= this.Length - 1) ? 0 : this.Position + 1;
        }

        private void ConsoleOverview()
        {
            Console.Clear();
            StringBuilder sb = new (new string('-', this.Length));
            sb[this.Position] = '#';
            Console.WriteLine((this.Position + 1) + " " + sb.ToString() + "\n");
            Console.WriteLine("BPM " + this.Bpm);
            foreach (Track track in this.project.TrackManager.Tracks)
            {
                Console.WriteLine("\n[Track " + track.Name + "]");
                Console.WriteLine("Current trigger: " + track.Get(this.Position)?.ToString());
                Console.WriteLine("Current sample: " + Path.GetFileName(track.SelectedSample.FilePath));
                double[] delays = this.CalcDelays(track);
                Console.WriteLine("Delays: " + string.Join(' ', delays));
            }

            Console.WriteLine("\nLoaded samples: " + this.SampleLibrary.Samples.Count);
        }
    }
}