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
using JackSharp.Processing;

namespace JackSharpTest.Dummies {
    public class ClientReceiver {
        public int Called { get; private set; }

        public Action<ProcessBuffer> CopyInToOutAction;
        public Action<ProcessBuffer> PlayMidiNoteAction;
        public Action<ProcessBuffer> SequenceMidiAction;
        public Action<ProcessBuffer> ChannelCounterAction;
        public Action<ProcessBuffer> CallBackOneAction;
        public Action<ProcessBuffer> CallBackTwoAction;

        public ClientReceiver() {
            CopyInToOutAction = CopyInToOut;
            PlayMidiNoteAction = PlayMidiNote;
            ChannelCounterAction = ChannelCounter;
            CallBackOneAction = CallBackOne;
            CallBackTwoAction = CallBackTwo;
            SequenceMidiAction = SequenceMidi;
        }

        void CopyInToOut(ProcessBuffer processItems) {
            for (int i = 0; i < Math.Min(processItems.AudioIn.Length, processItems.AudioOut.Length); i++) {
                Array.Copy(processItems.AudioIn[i].Audio, processItems.AudioOut[i].Audio, processItems.AudioIn[i].BufferSize);
            }
            Called++;
        }

        void PlayMidiNote(ProcessBuffer processItems) {
            foreach (MidiEventCollection<MidiInEvent> eventCollection in processItems.MidiIn) {
                Called++;
            }
        }

        void ChannelCounter(ProcessBuffer processItems) {
            Called = processItems.AudioIn.Length;
        }

        void CallBackOne(ProcessBuffer processItems) {
            Called |= 1;
        }

        void CallBackTwo(ProcessBuffer processItems) {
            Called |= 2;
        }

        public void SequenceMidi(ProcessBuffer processItems) {
            foreach (MidiEventCollection<MidiOutEvent> eventCollection in processItems.MidiOut) {
                var noteOn = new MidiOutEvent(processItems.Frames / 4,
                                             new byte[] {
                        0x90 /* note on */,
                        30 /* note */,
                        64 /* velocity */
					});
                var noteOff = new MidiOutEvent(processItems.Frames / 2,
                                              new byte[] {
                        0x80 /* note off */,
                        30 /* note */,
                        64 /* velocity */
					});
                eventCollection.AddEvent(noteOn);
                eventCollection.AddEvent(noteOff);
                Called++;
            }
        }
    }
}