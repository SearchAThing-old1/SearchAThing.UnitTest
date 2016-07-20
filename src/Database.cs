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
using SearchAThing.MongoDB;
using SearchAThing.UnitTests.MongoConcurrencyTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Xunit;
using MongoDB.Driver.Linq;
using MongoDB.Driver;

namespace SearchAThing.UnitTests
{

    namespace MongoConcurrencyTypes
    {

        public class I
        {

            public I()
            {
            }

            public string iprop1 { get; set; }
            public string iprop2 { get; set; }

            public List<I> Set { get; set; }
        }

        public class A : MongoEntity
        {


            public string prop1 { get; set; }
            public string prop2 { get; set; }

            public List<I> Set { get; set; }

        }

    }

    public class Database
    {

        [Fact(DisplayName = "MongoConcurrency")]
        public void MongoConcurrency()
        {
            const string connectionString = @"mongodb://localhost/searchathing_unittest_mongoconcurrency";

            {
                var ctx = new MongoContext(connectionString);

                // set data            
                ctx.FindAll<A>().ToList().Foreach(w => w.Delete());
                var iz = new I() { iprop1 = "z1", iprop2 = "z2" };
                var iy = new I() { iprop1 = "y1", iprop2 = "y2" }; //, Set = new List<I>() { iz } };
                var a = ctx.New<A>();
                a.prop1 = "x1";
                a.prop2 = "x2";
                a.Set = new List<I>() { iy };
                ctx.Save();

                // set coll prop
                iy.Set = new List<I>() { iz };
                ctx.Save();

                // retrieve two istance of the same document
                var doc1 = ctx.FindAll<A>().First();
                var doc2 = ctx.FindAll<A>().First();

                Assert.False(object.ReferenceEquals(doc1, doc2));

                // modify (1)
                {
                    doc1.prop1 = "_x1_"; // [1]

                    var itemy = doc1.Set.First();
                    var sety1 = itemy.Set;
                    var toremove = sety1.First();
                    sety1.Remove(toremove); // [1.1]

                    var iyy1 = new I() { iprop1 = "yy1", iprop2 = "yy2" };
                    doc1.Set.Add(iyy1); // [1.2]
                }

                // modify (2)
                {
                    doc2.prop2 = "_x2_"; // [2]
                }

                ctx.Save();
            }

            {
                // reload
                var ctx = new MongoContext(connectionString);
                var repo = ctx.GetRepository<A>();

                // retrieve two istance of the same document
                var doc = repo.Collection.AsQueryable().First();

                Assert.True(doc.prop1 == "_x1_"); // [1]
                Assert.True(doc.prop2 == "_x2_"); // [2]

                Assert.True(doc.Set.First().Set.Count == 0); // [1.1]

            }

        }

    }

}
