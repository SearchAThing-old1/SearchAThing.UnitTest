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

using SearchAThing.Collections;
using System.Linq;
using Xunit;

namespace SearchAThing.UnitTests
{

    public class Collections
    {

        #region circular list [tests]
        [Fact]
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
        [Fact]
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

    }

}
