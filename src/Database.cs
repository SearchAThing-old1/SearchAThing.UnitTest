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

using Repository.Mongo;
using SearchAThing.Core;
using SearchAThing.Graph;
using SearchAThing.MongoDB;
using SearchAThing.UnitTests.MongoConcurrencyTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Xunit;

namespace SearchAThing.UnitTests
{

    namespace MongoConcurrencyTypes
    {

        public class I : IMongoEntityTrackChanges, INotifyPropertyChanged, ISupportInitialize
        {

            public I()
            {
                _TrackChanges = new MongoEntityTrackChanges();
            }

            #region INotifyPropertyChanged [pce]       
            public event PropertyChangedEventHandler PropertyChanged;
            protected void SendPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion

            #region IMongoEntityTrackChanges
            MongoEntityTrackChanges _TrackChanges;
            public MongoEntityTrackChanges TrackChanges { get { return _TrackChanges; } }
            #endregion

            #region ISupportInitialize
            public void BeginInit()
            {
                _TrackChanges = null;
            }

            public void EndInit()
            {
                _TrackChanges = new MongoEntityTrackChanges();
            }
            #endregion

            #region iprop1 [pc]
            string _iprop1;
            public string iprop1
            {
                get
                {
                    return _iprop1;
                }
                set
                {
                    if (_iprop1 != value)
                    {
                        _iprop1 = value;
                        TrackChanges?.ChangedProperties.Add("iprop1");
                        SendPropertyChanged("iprop1");
                    }
                }
            }
            #endregion

            #region iprop2 [pc]
            string _iprop2;
            public string iprop2
            {
                get
                {
                    return _iprop2;
                }
                set
                {
                    if (_iprop2 != value)
                    {
                        _iprop2 = value;
                        TrackChanges?.ChangedProperties.Add("iprop2");
                        SendPropertyChanged("iprop2");
                    }
                }
            }
            #endregion

            #region Set [pc]
            List<I> _Set;
            public List<I> Set
            {
                get
                {
                    return _Set;
                }
                set
                {
                    if (_Set != value)
                    {
                        _Set = value;
                        SendPropertyChanged("Set");
                    }
                }
            }
            #endregion

        }

        public class A : Entity, IMongoEntityTrackChanges, INotifyPropertyChanged, ISupportInitialize
        {

            public A()
            {
                _TrackChanges = new MongoEntityTrackChanges();
            }

            #region INotifyPropertyChanged [pce]       
            public event PropertyChangedEventHandler PropertyChanged;
            protected void SendPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion

            #region IMongoEntityTrackChanges
            MongoEntityTrackChanges _TrackChanges;
            public MongoEntityTrackChanges TrackChanges { get { return _TrackChanges; } }
            #endregion

            #region ISupportInitialize
            public void BeginInit()
            {
                _TrackChanges = null;
            }

            public void EndInit()
            {
                _TrackChanges = new MongoEntityTrackChanges();
            }
            #endregion

            #region prop1 [pc]
            string _prop1;
            public string prop1
            {
                get
                {
                    return _prop1;
                }
                set
                {
                    if (_prop1 != value)
                    {
                        _prop1 = value;
                        TrackChanges?.ChangedProperties.Add("prop1");
                        SendPropertyChanged("prop1");
                    }
                }
            }
            #endregion

            #region prop2 [pc]
            string _prop2;
            public string prop2
            {
                get
                {
                    return _prop2;
                }
                set
                {
                    if (_prop2 != value)
                    {
                        _prop2 = value;
                        TrackChanges?.ChangedProperties.Add("prop2");
                        SendPropertyChanged("prop2");
                    }
                }
            }
            #endregion

            #region Set [pc]
            List<I> _Set;
            public List<I> Set
            {
                get
                {
                    return _Set;
                }
                set
                {
                    if (_Set != value)
                    {
                        _Set = value;
                        SendPropertyChanged("Set");
                    }
                }
            }
            #endregion

        }

    }

    public class Database
    {

        [Fact(DisplayName = "MongoConcurrency")]
        public void MongoConcurrency()
        {
            const string connectionString = @"mongodb://localhost:27017/searchathing_unittest_mongoconcurrency";

            {
                var repo = new Repository<A>(connectionString);

                // set data            
                repo.FindAll().ToList().Foreach(w => repo.Delete(w));
                var iz = new I() { iprop1 = "z1", iprop2 = "z2" };
                var iy = new I() { iprop1 = "y1", iprop2 = "y2", Set = new List<I>() { iz } };
                var a = new A() { prop1 = "x1", prop2 = "x2", Set = new List<I>() { iy } };
                repo.Insert(a);

                // retrieve two istance of the same document
                var doc1 = repo.First();
                var doc2 = repo.First();

                // modify (1)
                {
                    doc1.prop1 = "_x1_"; // [1]

                    var itemy = doc1.Set.First();
                    var sety1 = itemy.Set;
                    var toremove = sety1.First();
                    sety1.Remove(toremove);
                    sety1.SetAsDeleted(itemy, toremove); // [1.1]

                    var iyy1 = new I() { iprop1 = "yy1", iprop2 = "yy2" };
                    doc1.Set.Add(iyy1); // [1.2]
                }

                // modify (2)
                {
                    doc2.prop2 = "_x2_"; // [2]
                }

                // commmit w/track
                doc1.UpdateWithTrack(repo);
                doc2.UpdateWithTrack(repo);
            }

            {
                // reload
                var repo = new Repository<A>(connectionString);

                // retrieve two istance of the same document
                var doc = repo.First();

                Assert.True(doc.prop1 == "_x1_"); // [1]
                Assert.True(doc.prop2 == "_x2_"); // [2]

                Assert.True(doc.Set.First().Set.Count == 0); // [1.1]

            }

        }

    }

}
