#region SearchAThing.UnitTests, Copyright(C) 2016 Lorenzo Delana, License under MIT
/*
* The MIT License(MIT)
* Copyright(c) 2016 Lorenzo Delana, https://searchathing.com
*
* Permission is hereby granted, free of charge, to any person obtaining a
* copy of this software and associated documentation files (the "Software"),
* to deal in the Software without restriction, including without limitation
* the rights to use, copy, modify, merge, publish, distribute, sublicense,
* and/or sell copies of the Software, and to permit persons to whom the
* Software is furnished to do so, subject to the following conditions:
*
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
* FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
* DEALINGS IN THE SOFTWARE.
*/
#endregion

using SearchAThing.Core;
using SearchAThing.Sci;
using System;
using Xunit;
using static System.Math;

namespace SearchAThing.UnitTests
{

    public class Sci
    {
        IModel model = new SampleModel();

        [Fact]
        public void Vector3DTest()
        {
            // length
            Assert.True(new Vector3D(1, 5.9, 4).Length.EqualsTolLen(7.198, model));

            // normalized
            Assert.True(new Vector3D(1, 5.9, 4).Normalized().EqualsTolNormLen(new Vector3D(0.13893, 0.81968, 0.55572), model));

            // distance
            Assert.True(new Vector3D(1, 5.9, 4).Distance(new Vector3D(3, 4.3, 1.03)).EqualsTolLen(3.922, model));

            // dot product
            Assert.True(new Vector3D(5, 1, 3).DotProduct(new Vector3D(5, 4, 6)).EqualsTolLen(47, model));

            // cross product
            Assert.True(new Vector3D(2, 4, 12).CrossProduct(new Vector3D(3, 6, 1)).EqualsTolLen(new Vector3D(-68, 34, 0), model));

            // angle rad
            Assert.True(new Vector3D(3.48412, 2.06577, 0).AngleRad(new Vector3D(1.4325, 2.70248, 0), model)
                .EqualsTol(0.548, MUCollection.PlaneAngle.rad.Tolerance(model)));

            // angle rad
            Assert.True(new Vector3D(3.48412, 2.06577, 0).AngleRad(new Vector3D(-3.48412, -2.066, 0), model)
                .EqualsTol(PI, MUCollection.PlaneAngle.rad.Tolerance(model)));
        }

    }

}
