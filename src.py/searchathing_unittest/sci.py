"""
* SearchAThing.UnitTest, Copyright(C) 2015-2017 Lorenzo Delana, License under MIT
*
* The MIT License(MIT)
* Copyright(c) 2015-2017 Lorenzo Delana, https://searchathing.com
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

"""

import unittest
import numpy

from searchathing_sci.vector3d import *
from searchathing_core.number import *
from pint import UnitRegistry

ur = UnitRegistry(system='mks')

# 1/10 millimeter - length tolerance
tol_len = (1e-1 * ur.millimeter).to_base_units().magnitude

# 1/10 deg - angle tolerance
tol_rad = (1e-1 * ur.deg).to_base_units().magnitude


class Sci(unittest.TestCase):
    def test_vector3d_base(self):
        v = Vector3D(11, 22, 33)
        self.assertTrue(v[0] == 11 and v[1] == 22 and v[2] == 33)

    def test_vector3d_length(self):
        self.assertTrue(equals_tol(tol_len, Vector3D(1, 5.9, 4).length, 7.198))

    def test_vector3d_normalized(self):
        self.assertTrue(Vector3D(1, 5.9, 4).normalized().equals_tol(tol_len, 0.13893, 0.81968, 0.55572))

    def test_vector3d_distance(self):
        self.assertTrue(equals_tol(tol_len, Vector3D(1, 5.9, 4).distance(Vector3D(3, 4.3, 1.03)), 3.9218))

    def test_vector3d_dotproduct(self):
        self.assertTrue(equals_tol(tol_len, Vector3D(5, 1, 3).dotproduct(Vector3D(5, 4, 6)), 47))

    def test_vector3d_crossproduct(self):
        self.assertTrue(Vector3D(2, 4, 12).crossproduct(Vector3D(3, 6, 1)).equals_tol(tol_len, -68, 34, 0))

    def test_vector3d_anglerad(self):
        self.assertTrue(equals_tol(tol_rad, Vector3D(3.48412, 2.06577, 0).anglerad(tol_len, Vector3D(1.4325, 2.70248, 0)), 0.548))
        self.assertTrue(equals_tol(tol_rad, Vector3D(3.48412, 2.06577, 0).anglerad(tol_len, Vector3D(-3.48412, -2.066, 0)), numpy.pi))

    def test_vector3d_project(self):
        self.assertTrue(Vector3D(101.546, 25.186, 1.3).project(Vector3D(48.362, 46.564, 5)).equals_tol2(tol_len, Vector3D(64.9889, 62.5728, 6.719)))

    def test_vector3d_from_array(self):
        v = Vector3D.from_array([1, 2, 3])
        self.assertTrue(equals_tol(1e-6, v.x, 1))
        self.assertTrue(equals_tol(1e-6, v.y, 2))
        self.assertTrue(equals_tol(1e-6, v.z, 3))

    def test_vector3d_operators(self):
        v1 = Vector3D(1, 2, 3)
        v2 = Vector3D(4, 5, 6)
        self.assertTrue((v1 + v2).equals_tol(1e-6, 1 + 4, 2 + 5, 3 + 6))
        self.assertTrue((v1 - v2).equals_tol(1e-6, 1 - 4, 2 - 5, 3 - 6))
        self.assertTrue((v1 * 2).equals_tol(1e-6, 1 * 2, 2 * 2, 3 * 2))
        self.assertTrue((v1 / 2).equals_tol(1e-6, 1 / 2, 2 / 2, 3 / 2))

    def test_vector3d_concordant(self):
        self.assertTrue(Vector3D(101.546, 25.186, 1.3).concordant(tol_len, Vector3D(50.773, 12.593, .65)))
        self.assertFalse(Vector3D(101.546, 25.186, 1.3).concordant(tol_len, Vector3D(-50.773, -12.593, .65)))

    def test_vector3d_angletoward(self):
        self.assertTrue(equals_tol(tol_len,
                                   Vector3D(120.317, 42.914, 0).angle_toward(tol_len, Vector3D(28.549, 63.771, 0), Vector3D.zaxis()),
                                   0.80726))
        self.assertFalse(equals_tol(tol_len,
                                    Vector3D(120.317, 42.914, 0).angle_toward(tol_len, Vector3D(28.549, 63.771, 0), -Vector3D.zaxis()),
                                    0.80726))
        self.assertTrue(equals_tol(tol_len,
                                   Vector3D(120.317, 42.914, 0).angle_toward(tol_len, Vector3D(28.549, 63.771, 0), -Vector3D.zaxis()),
                                   2 * numpy.pi - 0.80726))
        self.assertTrue(abs(Vector3D(-6.95, -5.1725, 0).angle_toward(1e-6, Vector3D(6.95, 5.1725, 0), Vector3D(0, 0, 71.89775)) - numpy.pi) < tol_rad)


if __name__ == '__main__':
    unittest.main()
