// Author:
//       Thomas Mayer <thomas@residuum.org>
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

namespace JackSharp.Processing {
    /// <summary>
    /// Buffer containing audio and MIDI data in arrays.
    /// </summary>
    public class ProcessBuffer {
        /// <summary>
        /// Audio in buffers. Should not be changed.
        /// </summary>
        public AudioBuffer[] AudioIn { get; set; }

        /// <summary>
        /// Audio out buffers.
        /// </summary>
        public AudioBuffer[] AudioOut { get; set; }

        /// <summary>
        /// MIDI in buffers. Should not be changed.
        /// </summary>
        public MidiEventCollection<MidiInEvent>[] MidiIn { get; set; }

        /// <summary>
        /// Midi out buffers.
        /// </summary>
        public MidiEventCollection<MidiOutEvent>[] MidiOut { get; set; }

        /// <summary>
        /// Number of frames for this buffer.
        /// </summary>
        public int Frames { get; private set; }

        internal ProcessBuffer(uint nframes, AudioBuffer[] audioInBuffers, AudioBuffer[] audioOutBuffers, MidiEventCollection<MidiInEvent>[] midiInEvents, MidiEventCollection<MidiOutEvent>[] midiOutEvents) {
            Frames = (int)nframes;
            AudioIn = audioInBuffers;
            AudioOut = audioOutBuffers;
            MidiIn = midiInEvents;
            MidiOut = midiOutEvents;
        }

    }
}
