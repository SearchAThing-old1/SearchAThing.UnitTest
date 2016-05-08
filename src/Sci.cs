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

            // vector projection
            Assert.True(new Vector3D(101.546, 25.186, 1.3).Project(new Vector3D(48.362, 46.564, 5))
                .EqualsTolLen(new Vector3D(56.491, 14.011, 0.723), model));

            // vector vers
            Assert.True(new Vector3D(101.546, 25.186, 1.3).Concordant(new Vector3D(50.773, 12.593, .65)));
            Assert.False(new Vector3D(101.546, 25.186, 1.3).Concordant(new Vector3D(-50.773, -12.593, .65)));

            // angle toward
            Assert.True(new Vector3D(120.317, 42.914, 0).AngleToward(new Vector3D(28.549, 63.771, 0), Vector3D.ZAxis, model)
                .EqualsTol(0.80726, MUCollection.PlaneAngle.rad.Tolerance(model)));

            Assert.False(new Vector3D(120.317, 42.914, 0).AngleToward(new Vector3D(28.549, 63.771, 0), -Vector3D.ZAxis, model)
                .EqualsTol(0.80726, MUCollection.PlaneAngle.rad.Tolerance(model)));

            Assert.True(new Vector3D(120.317, 42.914, 0).AngleToward(new Vector3D(28.549, 63.771, 0), -Vector3D.ZAxis, model)
                .EqualsTol(2 * PI - 0.80726, MUCollection.PlaneAngle.rad.Tolerance(model)));

            // z-axis rotation
            Assert.True(new Vector3D(109.452, 38.712, 0).RotateAboutZAxis((50.0).ToRad()).EqualsTolLen(new Vector3D(40.699, 108.728, 0), model));

            // arbitrary axis rotation
            Assert.True(new Vector3D(747.5675, 259.8335, 0).RotateAboutAxis(new Vector3D(123.151, 353.8977, 25.6), (50.0).ToRad())
                .EqualsTolLen(new Vector3D(524.3462, 370.9603, -462.4069), model));

            // rotate relative
            Assert.True(
                new Vector3D(69.1831, 157.1155, 300).RotateAs(
                    new Vector3D(443.6913, 107.8843, 0), new Vector3D(342.7154, 239.6307, 0), model)
                .EqualsTolLen(new Vector3D(7.3989, 171.5134, 300), model));

            // vector parallel (1-d)
            Assert.True(new Vector3D(1, 0, 0).IsParallelTo(new Vector3D(-1, 0, 0), model));
            Assert.True(new Vector3D(0, 1, 0).IsParallelTo(new Vector3D(0, -2, 0), model));
            Assert.True(new Vector3D(0, 0, 1).IsParallelTo(new Vector3D(0, 0, -3), model));

            // vector parallel (2-d)
            Assert.True(new Vector3D(1, 1, 0).IsParallelTo(new Vector3D(-2, -2, 0), model));
            Assert.False(new Vector3D(1, 1, 0).IsParallelTo(new Vector3D(-2, 2, 0), model));

            Assert.True(new Vector3D(1, 0, 1).IsParallelTo(new Vector3D(-2, 0, -2), model));
            Assert.False(new Vector3D(1, 0, 1).IsParallelTo(new Vector3D(-2, 0, 2), model));

            Assert.True(new Vector3D(0, 1, 1).IsParallelTo(new Vector3D(0, -2, -2), model));
            Assert.False(new Vector3D(0, 1, 1).IsParallelTo(new Vector3D(0, -2, 2), model));

            // vector parallel (3-d)
            Assert.True(new Vector3D(1, 1, 1).IsParallelTo(new Vector3D(-2, -2, -2), model));
            Assert.True(new Vector3D(253.6625, 162.6347, 150).IsParallelTo(new Vector3D(380.4937, 243.952, 225), model));

            // vector parallel ( tolerance checks )
            {
                // ensure mm tolerance 1e-1
                var tmpModel = new SampleModel();

                tmpModel.MUDomain.SetLength(new Measure(1e-1, MUCollection.Length.mm));

                // Z component of first vector will be considered zero cause < 1e-1 model tolerance     
                Assert.True(new Vector3D(10, 0, 0.05).IsParallelTo(new Vector3D(-2, 0, 0), tmpModel));

                // Z component of first vector will be considered not-zero cause > 1e-1 model tolerance
                Assert.False(new Vector3D(10, 0, 0.11).IsParallelTo(new Vector3D(-2, 0, 0), tmpModel));

                // X, Z components of first vector force internal usage of normalized tolerance 1e-4
                // cause the length of the shortest vector here is < 1.5 and may represents a normalized vector
                // or a result of such type of operation
                Assert.True(new Vector3D(0.09, 0, 0.09).IsParallelTo(new Vector3D(-20, 0, -20), tmpModel));
            }

        }

    }

}
