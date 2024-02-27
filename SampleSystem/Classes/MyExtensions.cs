namespace SampleSystem.Classes
{
    using NAudio.Wave;
    using NAudio.Wave.SampleProviders;
    using System.IO;

    public static class MyExtensions
    {

        public static byte[] ToBytes(this MixingSampleProvider obj)
        {
            Stream stream = new MemoryStream();
            WaveFileWriter writer = new WaveFileWriter(stream, obj.WaveFormat);

            return ((MemoryStream)stream).ToArray();
        }

        public static MemoryStream ToWavFileMemoryStream(this ISampleProvider obj)
        {
            Stream stream = new MemoryStream();
            WaveFileWriter.WriteWavFileToStream(stream, obj.ToWaveProvider());
            return (MemoryStream)stream;
        }
    }
}
