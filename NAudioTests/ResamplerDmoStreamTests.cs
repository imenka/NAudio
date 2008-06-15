﻿using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NAudio.Wave;
using System.Diagnostics;

namespace NAudioTests
{
    [TestFixture]
    public class ResamplerDmoStreamTests
    {
        [Test]
        public void CanCreateResamplerStream()
        {
            using (WaveFileReader reader = new WaveFileReader("C:\\Users\\Mark\\Recording\\REAPER\\ideas-2008-05-17.wav"))
            {
                using (ResamplerDmoStream resampler = new ResamplerDmoStream(reader, WaveFormat.CreateIeeeFloatWaveFormat(48000,2)))
                {
                    Assert.Greater(resampler.Length, reader.Length, "Length");
                    Assert.AreEqual(0, reader.Position, "Position");
                    Assert.AreEqual(0, resampler.Position, "Position");            
                }
            }
        }

        [Test]
        public void CanReadABlockFromResamplerStream()
        {
            using (WaveFileReader reader = new WaveFileReader("C:\\Users\\Mark\\Recording\\REAPER\\ideas-2008-05-17.wav"))
            {
                using (ResamplerDmoStream resampler = new ResamplerDmoStream(reader, WaveFormat.CreateIeeeFloatWaveFormat(48000, 2)))
                {
                    // try to read 10 ms;
                    int bytesToRead = resampler.WaveFormat.AverageBytesPerSecond / 100;
                    byte[] buffer = new byte[bytesToRead];
                    int count = resampler.Read(buffer, 0, bytesToRead);
                    Assert.AreEqual(count, bytesToRead, "Bytes Read");
                }
            }
        }

        [Test]
        public void CanResampleAWholeStream()
        {
            using (WaveFileReader reader = new WaveFileReader("C:\\Users\\Mark\\Recording\\REAPER\\ideas-2008-05-17.wav"))
            {
                using (ResamplerDmoStream resampler = new ResamplerDmoStream(reader, WaveFormat.CreateIeeeFloatWaveFormat(48000, 2)))
                {
                    // try to read 10 ms;
                    int bytesToRead = resampler.WaveFormat.AverageBytesPerSecond / 100;
                    byte[] buffer = new byte[bytesToRead];
                    int count;
                    int total = 0;
                    do
                    {
                        count = resampler.Read(buffer, 0, bytesToRead);
                        total += count;
                        //Assert.AreEqual(count, bytesToRead, "Bytes Read");
                    } while (count > 0);
                    Debug.WriteLine(String.Format("Converted input length {0} to {1}", reader.Length, total));

                }
            }
        }

        [Test]
        public void CanResampleToWav()
        {
            using (WaveFileReader reader = new WaveFileReader("C:\\Users\\Mark\\Recording\\REAPER\\ideas-2008-05-17.wav"))
            {
                using (ResamplerDmoStream resampler = new ResamplerDmoStream(reader, new WaveFormat(48000, 16, 2)))
                {
                    using (WaveFileWriter writer = new WaveFileWriter("C:\\Users\\Mark\\Recording\\REAPER\\ideas-converted.wav", resampler.WaveFormat))
                    {
                        // try to read 10 ms;
                        int bytesToRead = resampler.WaveFormat.AverageBytesPerSecond / 100;
                        byte[] buffer = new byte[bytesToRead];
                        int count;
                        int total = 0;
                        do
                        {
                            count = resampler.Read(buffer, 0, bytesToRead);
                            writer.WriteData(buffer, 0, count);
                            total += count;
                            //Assert.AreEqual(count, bytesToRead, "Bytes Read");
                        } while (count > 0);
                        Debug.WriteLine(String.Format("Converted input length {0} to {1}", reader.Length, total));
                    }
                }
            }
        }
    }
}