// Author:
//	   Thomas Mayer <thomas@residuum.org>
//
// Copyright (c) 2016 Thomas Mayer
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using NAudio.Wave;
using JackSharp;
using System.Linq;
using JackSharp.Ports;
using JackSharp.Processing;

namespace Jack.NAudio {
    public sealed class AudioOut : IWavePlayer {
        readonly Processor _client;

        IWaveProvider _waveStream;

        PlaybackState _playbackState;
        float _volume = 1;

        public AudioOut(Processor client) {
            _client = client;
            _playbackState = PlaybackState.Stopped;
        }

        ~AudioOut() {
            Dispose(false);
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public WaveFormat OutputWaveFormat {
            get {
                return WaveFormat.CreateIeeeFloatWaveFormat(_client.SampleRate, _client.AudioOutPorts.Count());
            }
        }

        public event EventHandler<StoppedEventArgs> PlaybackStopped;

        public void Play() {
            switch (_playbackState) {
                case PlaybackState.Playing:
                    return;
                case PlaybackState.Paused:
                    _playbackState = PlaybackState.Playing;
                    return;
                case PlaybackState.Stopped:
                    if (_client.Start()) {
                        _playbackState = PlaybackState.Playing;
                    }
                    break;
            }
        }

        public void Stop() {
            if (_client.Stop()) {
                _playbackState = PlaybackState.Stopped;
                if (PlaybackStopped != null) {
                    PlaybackStopped(this, new StoppedEventArgs());
                }
            }
        }

        public void Pause() {
            _playbackState = PlaybackState.Paused;
        }

        void ProcessAudio(ProcessBuffer processingChunk) {
            if (_playbackState != PlaybackState.Playing) {
                return;
            }
            int bufferCount = processingChunk.AudioOut.Length;
            if (bufferCount == 0) {
                return;
            }
            int bufferSize = processingChunk.Frames;
            int floatsCount = bufferCount * bufferSize;
            int bytesCount = floatsCount * sizeof(float);
            byte[] fromWave = new byte[bytesCount];

            _waveStream.Read(fromWave, 0, bytesCount);

            float[] interlacedSamples = new float[floatsCount];
            Buffer.BlockCopy(fromWave, 0, interlacedSamples, 0, bytesCount);
            for (int i = 0; i < floatsCount; i++) {
                interlacedSamples[i] = interlacedSamples[i] * _volume;
            }
            BufferOperations.DeinterlaceAudio(interlacedSamples, processingChunk.AudioOut, bufferSize, bufferCount);
        }

        public void Init(IWaveProvider waveProvider) {
            _waveStream = waveProvider;

            _playbackState = PlaybackState.Stopped;
            _client.ProcessFunc += ProcessAudio;
        }

        public PlaybackState PlaybackState {
            get { return _playbackState; }
        }

        public float Volume {
            get { return _volume; }
            set {
                if (value < 0) {
                    throw new ArgumentOutOfRangeException("value", "Volume must be between 0.0 and 1.0");
                }
                if (value > 1) {
                    throw new ArgumentOutOfRangeException("value", "Volume must be between 0.0 and 1.0");
                }
                _volume = value;
            }
        }

        void Dispose(bool isDisposing) {
            _client.ProcessFunc -= ProcessAudio;
            Stop();
        }
    }
}
