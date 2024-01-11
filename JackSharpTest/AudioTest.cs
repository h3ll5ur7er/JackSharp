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

using NUnit.Framework;
using JackSharp;
using System.Threading;
using JackSharpTest.Dummies;
using Assert = NUnit.Framework.Legacy.ClassicAssert;

namespace JackSharpTest {
    [TestFixture]
    public class AudioTest {
        static Processor _client;

        [SetUp]
        public static void CreateClient() {
            _client = new Processor("testAudio", 2, 2);
        }

        [Test]
        public virtual void AudioCopying() {
            ClientReceiver receiver = new();
            _client.ProcessFunc = receiver.CopyInToOutAction;
            _client.Start();
            Thread.Sleep(2000);
            Assert.IsTrue(receiver.Called > 0);
        }

        [Test]
        public virtual void AudioAddMultipleActions() {
            ClientReceiver receiver = new();
            _client.ProcessFunc += receiver.CallBackOneAction;
            _client.ProcessFunc += receiver.CallBackTwoAction;
            _client.Start();
            Thread.Sleep(200);
            _client.Stop();
            Assert.AreEqual(3, receiver.Called);
        }

        [Test]
        public virtual void AudioAddRemoveAction() {
            ClientReceiver receiver = new();
            _client.ProcessFunc += receiver.CallBackOneAction;
            _client.ProcessFunc += receiver.CallBackTwoAction;
            _client.ProcessFunc -= receiver.CallBackOneAction;
            _client.Start();
            Thread.Sleep(200);
            _client.Stop();
            Assert.AreEqual(2, receiver.Called);
        }

        [TearDown]
        public static void DestroyClient() {
            _client.Dispose();
        }
    }
}