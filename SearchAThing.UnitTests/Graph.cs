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
using SearchAThing.Graph;
using System;
using System.Linq;
using Xunit;

namespace SearchAThing.UnitTests
{

    public class Graph
    {

        #region monitor plot [tests]

        [Fact]
        public void MonitorPlotTest1()
        {
            IMonitorPlot plot = new MonitorPlot(3);

            var dsmean = plot.AddDataSet("6 seconds", TimeSpan.FromSeconds(6));

            var dtnow = DateTime.Now;
            var ts = TimeSpan.FromSeconds(1);

            plot.Add(new MonitorData(dtnow, 5), dtnow); dtnow += ts;
            plot.Add(new MonitorData(dtnow, 6), dtnow); dtnow += ts;
            plot.Add(new MonitorData(dtnow, 1), dtnow); dtnow += ts;
            plot.Add(new MonitorData(dtnow, 2), dtnow); dtnow += ts;
            plot.Add(new MonitorData(dtnow, 3), dtnow); dtnow += ts;
            plot.Add(new MonitorData(dtnow, 4), dtnow); dtnow += ts;

            var q1 = plot.DataSets.First(w => w.Name == "default");
            var q1lst = q1.Points.ToList();
            Assert.True(q1lst.Count == 3);
            // checks that the first "default" set window move to the last three data
            Assert.True(q1lst[0].Value.EqualsAutoTol(2));
            Assert.True(q1lst[1].Value.EqualsAutoTol(3));
            Assert.True(q1lst[2].Value.EqualsAutoTol(4));

            // now check that the "6 seconds" dataset mean 3 points over last 6 seconds
            // in other words it will get (5.5, 1.5, 3.5) that are means of ( (5,6), (1,2), (3,4) )
            var meanlst = dsmean.Points.ToList();
            Assert.True(meanlst.Count == 3);
            Assert.True(meanlst[0].Value.EqualsAutoTol(5.5));
            Assert.True(meanlst[1].Value.EqualsAutoTol(1.5));
            Assert.True(meanlst[2].Value.EqualsAutoTol(3.5));

            // proceed adding 2 more items
            plot.Add(new MonitorData(dtnow, 6), dtnow); dtnow += ts;
            plot.Add(new MonitorData(dtnow, 7), dtnow); dtnow += ts;

            q1lst = q1.Points.ToList();
            Assert.True(q1lst.Count == 3);
            Assert.True(q1lst[0].Value.EqualsAutoTol(4));
            Assert.True(q1lst[1].Value.EqualsAutoTol(6));
            Assert.True(q1lst[2].Value.EqualsAutoTol(7));

            // check the mean, should be (1.5, 3.5, 6.5)
            meanlst = dsmean.Points.ToList();
            Assert.True(meanlst.Count == 3);
            Assert.True(meanlst[0].Value.EqualsAutoTol(1.5));
            Assert.True(meanlst[1].Value.EqualsAutoTol(3.5));
            Assert.True(meanlst[2].Value.EqualsAutoTol(6.5));
        }
        #endregion

    }

}
