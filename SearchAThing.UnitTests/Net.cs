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
using SearchAThing.Net;

namespace SearchAThing.UnitTests
{

    public class Net
    {

        #region checksum [tests]

        /// <summary>
        /// Checksum tests
        /// </summary>
        [Fact]
        public void Test1()
        {
            /*0000   45 00 00 28 2e 27 40 00 80 06 xx07 xxee c0 a8 00 64  E..(.'@........d
0010   6c a8 97 06                                      l...*/
          
            Assert.True(new byte[]
            {
                0x45, 0x00, 0x00, 0x28, 0x2e, 0x27, 0x40, 0x00, 0x80, 0x06,
                0x00, 0x00, 0xc0, 0xa8, 0x00, 0x64, 0x6c, 0xa8, 0x97, 0x06
            }.Checksum() == 0x07ee);

            Assert.True(new byte[] { 0 }.Checksum() == 0xffff);            
            Assert.True(new byte[] { 0, 0xff }.Checksum() == 0xff00);
            Assert.True(new byte[] { 0xff }.Checksum() == 0xff);
        }

        #endregion

    }

}
