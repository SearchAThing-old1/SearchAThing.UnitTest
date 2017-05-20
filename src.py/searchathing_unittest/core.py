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
from searchathing_core.number import *


class Core(unittest.TestCase):
    def test_equals_auto_tol(self):
        self.assertTrue(equals_auto_tol(1, 1))
        self.assertTrue(equals_auto_tol(1, 1 + 1e-20))
        self.assertFalse(equals_auto_tol(1, 2))
        self.assertTrue(equals_auto_tol(1, 2, precision=2))

    def test_mround(self):
        self.assertTrue(equals_tol(1e-10, mround(4, 3), 3))
        self.assertTrue(equals_tol(1e-10, mround(5, 3), 6))
        self.assertTrue(equals_tol(1e-10, mround(-3.21, .1), -3.2))
        self.assertTrue(equals_tol(1e-10, mround(-3.29, .1), -3.3))

    def test_angle(self):
        self.assertTrue(equals_tol(1e-6, to_deg(.21294), 12.200563))
        self.assertTrue(equals_tol(1e-6, to_rad(140.3), 2.448697))


if __name__ == '__main__':
    unittest.main()
