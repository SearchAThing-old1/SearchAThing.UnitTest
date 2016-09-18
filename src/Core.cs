#region SearchAThing.UnitTests, Copyright(C) 2015-2016 Lorenzo Delana, License under MIT
/*
* The MIT License(MIT)
* Copyright(c) 2016 Lorenzo Delana, https://searchathing.com
*
* Permission is hereby granted, free of charge, to any person obtaining
* a copy of this software and associated documentation files
* (the "Software"), to deal in the Software without restriction,
* including without limitation the rights to use, copy, modify, merge,
* publish, distribute, sublicense, and/or sell copies of the Software,
* and to permit persons to whom the Software is furnished to do so,
* subject to the following conditions:
*
* The above copyright notice and this permission notice shall be
* included in all copies or substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
* EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
* MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
* IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
* CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
* TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
* SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/
#endregion

using SearchAThing.Core;
using System;
using Xunit;
using System.Linq;
using System.Collections.Generic;

namespace SearchAThing.UnitTests
{

    public class Core
    {

        #region event operation [tests]

        public enum EventTypes { EventA, EventB };

        public class MyEventArgs : EventArgs
        {
            public EventTypes Type { get; private set; }

            public MyEventArgs(EventTypes type)
            {
                Type = type;
            }
        }

        /// <summary>
        /// Event opertion with custom args.
        /// </summary>
        [Fact(DisplayName = "EventOperation_CustomArg")]
        public void Test1()
        {
            IEventOperation<MyEventArgs> op = new EventOperation<MyEventArgs>();

            op.Event += (a, b) =>
            {
                Assert.True(b.Type == EventTypes.EventA);
            };

            op.Fire(null, new MyEventArgs(EventTypes.EventA));

            Assert.True(op.FireCount == 1);
            Assert.True(op.HandledCount == 1);
        }

        /// <summary>
        /// FireCount, HandledCount.
        /// </summary>
        [Fact(DisplayName = "EventOperation_Count")]
        public void Test2()
        {
            IEventOperation op = new EventOperation();

            op.Event += (a, b) =>
            {
            };

            op.Event += (a, b) =>
            {
            };

            op.Fire();

            op.Event += (a, b) =>
            {
            };

            Assert.True(op.FireCount == 1);
            Assert.True(op.HandledCount == 2);

            op.Fire();

            Assert.True(op.FireCount == 2);
            Assert.True(op.HandledCount == 2 + 3);
        }

        /// <summary>
        /// Event fired, but not handler yet attached.
        /// </summary>
        [Fact(DisplayName = "EventOperation_NotYet")]
        public void Test3()
        {
            IEventOperation op = new EventOperation();

            op.Fire();

            op.Event += (a, b) =>
            {
            };

            Assert.True(op.FireCount == 1);
            Assert.True(op.HandledCount == 0);
        }

        /// <summary>
        /// Be notified after the event fired.
        /// </summary>
        [Fact(DisplayName = "EventOperation_After")]
        public void Test4()
        {
            IEventOperation op = new EventOperation(EventOperationBehaviorTypes.RemindPastEvents);

            op.Fire();

            Assert.True(op.FireCount == 1);
            Assert.True(op.HandledCount == 0);

            op.Event += (a, b) =>
            {
            };

            Assert.True(op.FireCount == 1);
            Assert.True(op.HandledCount == 1);
        }

        /// <summary>
        /// Be notified after the event fired (multiple listeners).
        /// </summary>
        [Fact(DisplayName = "EventOperation_Multiple")]
        public void Test5()
        {
            IEventOperation op = new EventOperation(EventOperationBehaviorTypes.RemindPastEvents);

            var listener1HitCount = 0;
            var listener2HitCount = 0;

            {
                op.Fire(); // event fired ( no handlers yet connected )
                Assert.True(op.FireCount == 1 && op.HandledCount == 0);

                op.Event += (a, b) => // listener1 connects and receive 1 event
                {
                    ++listener1HitCount;
                };
                Assert.True(op.FireCount == 1 && op.HandledCount == 1);
            }

            {
                op.Fire(); // event fired ( listener1 will receive its 2-th event )
                Assert.True(op.FireCount == 2 && op.HandledCount == 2);

                op.Event += (a, b) => // listener2 connected and receive 2 events
                {
                    ++listener2HitCount;
                };
            }

            Assert.True(listener1HitCount == 2 && listener2HitCount == 2);
            Assert.True(op.FireCount == 2 && op.HandledCount == listener1HitCount + listener2HitCount);
        }

        #endregion

        #region string [tests]

        [Fact(DisplayName = "String")]
        public void StringTest1()
        {
            var s1 = "Hi this is a sample";
            var s2 = " and this is another";

            Assert.True(s1.StripBegin("Hi ") == "this is a sample");
            Assert.True(s1.StripEnd("a sample") == "Hi this is ");
            Assert.True(s2.StripBegin("and this") == " and this is another"); // begin part not stripped cause not trimmed
            Assert.True(s2.Trim().StripBegin("and this") == " is another");
        }

        #endregion

        #region circular list [tests]
        [Fact(DisplayName = "CircularEnumerator")]
        public void CircularEnumeratorTest()
        {
            var s = new List<int>() { 1, 2, 3 }.AsCircularEnumerable();

            Assert.True(s.Take(7).SequenceEqual(new List<int>() { 1, 2, 3, 1, 2, 3, 1 }));
            Assert.True(s.Skip(4).Take(4).SequenceEqual(new List<int>() { 2, 3, 1, 2 }));
        }
        #endregion

        #region circular list [tests]
        [Fact(DisplayName = "CircularList")]
        public void CircularListTest1()
        {
            var lst = new CircularList<int>(5);

            for (int i = 0; i < 5; ++i) lst.Add(i);

            // verify assert's integrity
            Assert.True(lst.Count == 5);
            Assert.True(lst.GetItem(0) == 0);
            Assert.True(lst.GetItem(4) == 4);

            lst.Add(5); // step out from the max size

            Assert.True(lst.Count == 5);
            Assert.True(lst.GetItem(0) == 1);
            Assert.True(lst.GetItem(4) == 5);

            // complete count items
            for (int i = 0; i < 4; ++i) lst.Add(6 + i);

            // verify assert's integrity
            Assert.True(lst.Count == 5);
            Assert.True(lst.GetItem(0) == 5);
            Assert.True(lst.GetItem(4) == 9);
        }

        /// <summary>
        /// Test enumerator
        /// </summary>
        [Fact(DisplayName = "CircularList_enum")]
        public void CircularListTest2()
        {
            var lst = new CircularList<int>(5);

            for (int i = 0; i < 15; ++i) lst.Add(i);

            // verify assert's integrity
            Assert.True(lst.Count == 5);
            Assert.True(lst.GetItem(0) == 10);
            Assert.True(lst.GetItem(4) == 14);

            var l = lst.Items.ToList();

            Assert.True(l.Count == 5);
            Assert.True(l[0] == 10);
            Assert.True(l[4] == 14);
        }
        #endregion

        #region fluent
        [Fact(DisplayName = "Fluent")]
        public void FluentTest()
        {
            int cycles = 0;
            Func<int, int> IntensiveFn = (x) => { ++cycles; return x; };
            {
                cycles = 0;

                Assert.True(IntensiveFn(1) == 10 || IntensiveFn(1) == 1);
                Assert.True(cycles == 2);
            }

            {
                cycles = 0;

                Assert.True(IntensiveFn(1).Function(x => x == 10 || x == 1));
                Assert.True(cycles == 1);
            }
        }
        #endregion        
    }

}
