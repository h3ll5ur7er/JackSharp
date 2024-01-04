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
using System.Linq;
using JackSharp;
using NUnit.Framework;
using Assert = NUnit.Framework.Legacy.ClassicAssert;

namespace JackSharpTest
{
	[TestFixture]
	public class PortTest
	{
		static Processor _client;

		[SetUp]
		public static void CreateClient ()
		{
			_client = new Processor ("testPorts", 2, 4, 1, 3);
		}

		[Test]
		public virtual void AudioInPortsAreCreated ()
		{
			_client.Start ();
			Assert.AreEqual (2, _client.AudioInPorts.Count ());
		}

		[Test]
		public virtual void AudioOutPortsAreCreated ()
		{
			_client.Start ();
			Assert.AreEqual (4, _client.AudioOutPorts.Count ());
		}

		[Test]
		public virtual void MidiInPortsAreCreated ()
		{
			_client.Start ();
			Assert.AreEqual (1, _client.MidiInPorts.Count ());
		}

		[Test]
		public virtual void MidiOutPortsAreCreated ()
		{
			_client.Start ();
			Assert.AreEqual (3, _client.MidiOutPorts.Count ());
		}

		[TearDown]
		public static void DestroyClient ()
		{
			_client.Dispose ();
		}
	}
}
