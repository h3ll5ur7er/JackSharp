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
    /// Midi out event.
    /// </summary>
    public class MidiOutEvent : IMidiEvent {
        /// <summary>
        /// Gets the time.
        /// </summary>
        /// <value>The index of the MIDI event in the current processing frame.</value>
        public int Time { get; private set; }

        /// <summary>
        /// Gets the midi data. Please read the MIDI specifications for valid content.
        /// </summary>
        /// <value>The midi data.</value>
        public byte[] MidiData { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="JackSharp.Processing.MidiOutEvent"/> class.
        /// </summary>
        /// <param name="time">Time.</param>
        /// <param name="midiData">Midi data.</param>
        public MidiOutEvent(int time, byte[] midiData) {
            Time = time;
            MidiData = midiData;
        }
    }
}