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
using JackSharp.ApiWrapper;
using JackSharp.Pointers;
using JackSharp.Processing;

namespace JackSharp.Ports {
    /// <summary>
    /// Buffer operations.
    /// </summary>
    public static class BufferOperations {
        internal static MidiEventCollection<MidiInEvent> GetMidiBuffer(this MidiInPort port, uint nframes) {
            MidiEventCollection<MidiInEvent> eventCollection = new(port);
            foreach (MidiInEvent midiEvent in port.GetMidiEvents(nframes)) {
                eventCollection.AddEvent(midiEvent);
            }
            return eventCollection;
        }

        /// <summary>
        /// Merges audio buffers such that all samples of a frame appear directly after each other.
        /// </summary>
        /// <returns>The audio.</returns>
        /// <param name="audioBuffers">Audio buffers.</param>
        /// <param name="bufferSize">Buffer size.</param>
        /// <param name="bufferCount">Buffer count.</param>
        public static float[] InterlaceAudio(AudioBuffer[] audioBuffers, int bufferSize, int bufferCount) {
            float[] interlaced = new float[bufferSize * bufferCount];

            for (int i = 0; i < bufferSize; i++) {
                for (int j = 0; j < bufferCount; j++) {
                    interlaced[i * bufferCount + j] = audioBuffers[j].Audio[i];
                }
            }
            return interlaced;
        }

        /// <summary>
        /// Unmerges interlaced audio such that samples of a frame are put into audio buffers.
        /// </summary>
        /// <param name="interlaced">Interlaced.</param>
        /// <param name="audioBuffers">Audio buffers.</param>
        /// <param name="bufferSize">Buffer size.</param>
        /// <param name="bufferCount">Buffer count.</param>
        public static void DeinterlaceAudio(float[] interlaced, AudioBuffer[] audioBuffers, int bufferSize, int bufferCount) {
            for (int i = 0; i < bufferSize; i++) {
                for (int j = 0; j < bufferCount; j++) {
                    audioBuffers[j].Audio[i] = interlaced[i * bufferCount + j];
                }
            }
        }

        internal static unsafe void WriteToJackMidi(this MidiEventCollection<MidiOutEvent> midiEvents, uint nframes) {
            float* portBuf = PortApi.GetBuffer(midiEvents.Port._port, nframes);
            MidiApi.ClearBuffer(portBuf);
            foreach (MidiOutEvent midiEvent in midiEvents) {
                byte* buffer = MidiApi.ReserveEvent(portBuf, (uint)midiEvent.Time, (uint)midiEvent.MidiData.Length);
                StructPointer<byte> bufferPointer = new((IntPtr)buffer, (uint)midiEvent.MidiData.Length);
                bufferPointer.Array = midiEvent.MidiData;
                bufferPointer.CopyToPointer();
            }
        }
    }
}
