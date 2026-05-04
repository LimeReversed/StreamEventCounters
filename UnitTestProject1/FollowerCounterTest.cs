using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Counters;
using System.Collections.Generic;
using System.Collections;
using Streamer.bot.Plugin.Interface;

namespace Test_Chamber
{
    [TestClass]
    public class FollowerCounterTest
    {
        MockCPH CPH = null;
        CounterLoopable currentCounter = null;

        public Dictionary<string, Source> GetAllCountItems(SceneSourcePair pair, int mod, MockCPH cph)
        {
            var result = new Dictionary<string, Source>();

            for (int i = 0; i < mod; i++)
            {
                int position = (i + 1) % mod;
                var sourceName = pair.Source.Replace("#", position.ToString());

                result[sourceName] = cph._sources[sourceName];

            }

            return result;
        }

        public bool ItemsAreEqual(Dictionary<string, Source> dict1, Dictionary<string, Source> dict2)
        {
            if (dict1.Count != dict2.Count)
                return false;
            foreach (var kvp in dict1)
            {
                if (!dict2[kvp.Key].Equals(dict1[kvp.Key]))
                    return false;
            }
            return true;
        }

        public void PrintList<T>(List<T> list)
        {
            foreach (T item in list)
            {
                Console.Write(item.ToString() + " ");
            }
            Console.WriteLine();
        }

        [TestInitialize()]
        public void Initialize()
        {
            CPH = new MockCPH();
            CounterManager.InitializeAll(CPH);
            currentCounter = CounterManager.followCounter;
        }

        // **** INCREMENT COUNT TESTS ****
        [TestMethod]
        public void IncrementCount_GoesFrom15To18_CurrentCountIsCorrect()
        {
            CPH.SetGlobalVar(MockCPH.lastFollowerCount, 15);
            CPH.SetArg(MockCPH.followerCount, 3);
            currentCounter.DataHandler.Initialize();

            currentCounter.DataHandler.IncrementCount(MockCPH.followerCount);

            int currentCount = currentCounter.DataHandler.CountOutput;

            Assert.AreEqual(18, currentCount);
        }

        [TestMethod]
        public void IncrementCount_GoesFrom15To18_Shows8Hearts()
        {
            CPH.SetGlobalVar(MockCPH.lastFollowerCount, 15);
            CPH.SetArg(MockCPH.followerCount, 3);
            currentCounter.DataHandler.Initialize();
            DataHandlerWithItem currentDataHandler = currentCounter.DataHandler as DataHandlerWithItem;
            currentCounter.ResetToLast();

            currentCounter.DataHandler.IncrementCount(MockCPH.followerCount);
            currentCounter.WriteToOBS();
            SceneSourcePair currentSources = currentDataHandler.ItemSource;
            var allItems = GetAllCountItems(currentSources, 10, CPH);
            var expectedItems = new Dictionary<string, Source>()
            {
                { "Heart 1 Full", new Source( null ,true) },
                { "Heart 2 Full", new Source( null ,true) },
                { "Heart 3 Full", new Source( null ,true) },
                { "Heart 4 Full", new Source( null ,true) },
                { "Heart 5 Full", new Source( null ,true) },
                { "Heart 6 Full", new Source( null ,true) },
                { "Heart 7 Full", new Source( null ,true) },
                { "Heart 8 Full", new Source( null ,true) },
                { "Heart 9 Full", new Source( null ,false) },
                { "Heart 0 Full", new Source( null ,false) }
            };

            Assert.IsTrue(ItemsAreEqual(expectedItems, allItems));
        }

        [TestMethod]
        public void IncrementCount_GoesFrom15To25_Shows5Hearts()
        {
            CPH.SetGlobalVar(MockCPH.lastFollowerCount, 15);
            CPH.SetArg(MockCPH.followerCount, 10);
            currentCounter.DataHandler.Initialize();
            DataHandlerWithItem currentDataHandler = currentCounter.DataHandler as DataHandlerWithItem;
            currentCounter.ResetToLast();

            currentCounter.DataHandler.IncrementCount(MockCPH.followerCount);
            currentCounter.WriteToOBS();

            SceneSourcePair currentSources = currentDataHandler.ItemSource;
            var allItems = GetAllCountItems(currentSources, 10, CPH);
            var expectedItems = new Dictionary<string, Source>()
            {
                { "Heart 1 Full", new Source( null ,true) },
                { "Heart 2 Full", new Source( null ,true) },
                { "Heart 3 Full", new Source( null ,true) },
                { "Heart 4 Full", new Source( null ,true) },
                { "Heart 5 Full", new Source( null ,true) },
                { "Heart 6 Full", new Source( null ,false) },
                { "Heart 7 Full", new Source( null ,false) },
                { "Heart 8 Full", new Source( null ,false) },
                { "Heart 9 Full", new Source( null ,false) },
                { "Heart 0 Full", new Source( null ,false) }
            };

            Assert.IsTrue(ItemsAreEqual(expectedItems, allItems));
        }

        [TestMethod]
        public void IncrementCount_ThreeTimes_ShowsCorrectHearts()
        {
            CPH.SetGlobalVar(MockCPH.lastFollowerCount, 15);
            CPH.SetArg(MockCPH.followerCount, 2);
            currentCounter.DataHandler.Initialize();
            DataHandlerWithItem currentDataHandler = currentCounter.DataHandler as DataHandlerWithItem;
            currentCounter.ResetToLast();

            currentCounter.DataHandler.IncrementCount(MockCPH.followerCount);
            currentCounter.WriteToOBS();
            currentCounter.DataHandler.IncrementCount(MockCPH.followerCount);
            currentCounter.WriteToOBS();
            currentCounter.DataHandler.IncrementCount(MockCPH.followerCount);
            currentCounter.WriteToOBS();

            SceneSourcePair currentSources = currentDataHandler.ItemSource;
            var allItems = GetAllCountItems(currentSources, 10, CPH);
            var expectedItems = new Dictionary<string, Source>()
            {
                { "Heart 1 Full", new Source( null ,true) },
                { "Heart 2 Full", new Source( null ,false) },
                { "Heart 3 Full", new Source( null ,false) },
                { "Heart 4 Full", new Source( null ,false) },
                { "Heart 5 Full", new Source( null ,false) },
                { "Heart 6 Full", new Source( null ,false) },
                { "Heart 7 Full", new Source( null ,false) },
                { "Heart 8 Full", new Source( null ,false) },
                { "Heart 9 Full", new Source( null ,false) },
                { "Heart 0 Full", new Source( null ,false) }
            };

            Assert.IsTrue(ItemsAreEqual(expectedItems, allItems));
        }

        [TestMethod]
        public void IncrementCount_UpdateThreeTimes_ShowsCorrectHearts()
        {
            CPH.SetGlobalVar(MockCPH.lastFollowerCount, 15);
            CPH.SetArg(MockCPH.followerCount, 18);
            currentCounter.DataHandler.Initialize();
            DataHandlerWithItem currentDataHandler = currentCounter.DataHandler as DataHandlerWithItem;
            currentCounter.ResetToLast();

            currentCounter.DataHandler.IncrementCount(MockCPH.followerCount);
            currentCounter.WriteToOBS();
            CPH.SetArg(MockCPH.followerCount, 2);
            currentCounter.DataHandler.IncrementCount(MockCPH.followerCount);
            currentCounter.WriteToOBS();
            CPH.SetArg(MockCPH.followerCount, 9);
            currentCounter.DataHandler.IncrementCount(MockCPH.followerCount);
            currentCounter.WriteToOBS();

            SceneSourcePair currentSources = currentDataHandler.ItemSource;
            var allItems = GetAllCountItems(currentSources, 10, CPH);
            var expectedItems = new Dictionary<string, Source>()
            {
                { "Heart 1 Full", new Source( null ,true) },
                { "Heart 2 Full", new Source( null ,true) },
                { "Heart 3 Full", new Source( null ,true) },
                { "Heart 4 Full", new Source( null ,true) },
                { "Heart 5 Full", new Source( null ,false) },
                { "Heart 6 Full", new Source( null ,false) },
                { "Heart 7 Full", new Source( null ,false) },
                { "Heart 8 Full", new Source( null ,false) },
                { "Heart 9 Full", new Source( null ,false) },
                { "Heart 0 Full", new Source( null ,false) }
            };

            Assert.IsTrue(ItemsAreEqual(expectedItems, allItems));
        }

        // **** UpdateCount Tests ****
        [TestMethod]
        public void UpdateCount_GoesFrom15To18_CurrentCountIsCorrect()
        {
            CPH.SetGlobalVar(MockCPH.lastFollowerCount, 15);
            CPH.SetArg(MockCPH.followerCount, 18);
            currentCounter.DataHandler.Initialize();

            currentCounter.DataHandler.UpdateCount(MockCPH.followerCount);
            currentCounter.WriteToOBS();

            int currentCount = currentCounter.DataHandler.CountOutput;

            Assert.AreEqual(18, currentCount);
        }

        [TestMethod]
        public void UpdateCount_GoesFrom15To18_Shows8Hearts()
        {
            CPH.SetGlobalVar(MockCPH.lastFollowerCount, 15);
            CPH.SetArg(MockCPH.followerCount, 18);
            currentCounter.DataHandler.Initialize();
            DataHandlerWithItem currentDataHandler = currentCounter.DataHandler as DataHandlerWithItem;
            currentCounter.ResetToLast();

            currentCounter.DataHandler.UpdateCount(MockCPH.followerCount);
            currentCounter.WriteToOBS();
            SceneSourcePair currentSources = currentDataHandler.ItemSource;
            var allItems = GetAllCountItems(currentSources, 10, CPH);
            var expectedItems = new Dictionary<string, Source>()
            {
                { "Heart 1 Full", new Source( null ,true) },
                { "Heart 2 Full", new Source( null ,true) },
                { "Heart 3 Full", new Source( null ,true) },
                { "Heart 4 Full", new Source( null ,true) },
                { "Heart 5 Full", new Source( null ,true) },
                { "Heart 6 Full", new Source( null ,true) },
                { "Heart 7 Full", new Source( null ,true) },
                { "Heart 8 Full", new Source( null ,true) },
                { "Heart 9 Full", new Source( null ,false) },
                { "Heart 0 Full", new Source( null ,false) }
            };

            Assert.IsTrue(ItemsAreEqual(expectedItems, allItems));
        }

        [TestMethod]
        public void UpdateCount_GoesFrom15To25_Shows5Hearts()
        {
            CPH.SetGlobalVar(MockCPH.lastFollowerCount, 10);
            CPH.SetArg(MockCPH.followerCount, 25);
            currentCounter.DataHandler.Initialize();
            DataHandlerWithItem currentDataHandler = currentCounter.DataHandler as DataHandlerWithItem;
            currentCounter.ResetToLast();

            currentCounter.DataHandler.UpdateCount(MockCPH.followerCount);
            currentCounter.WriteToOBS();

            SceneSourcePair currentSources = currentDataHandler.ItemSource;
            var allItems = GetAllCountItems(currentSources, 10, CPH);
            var expectedItems = new Dictionary<string, Source>()
            {
                { "Heart 1 Full", new Source( null ,true) },
                { "Heart 2 Full", new Source( null ,true) },
                { "Heart 3 Full", new Source( null ,true) },
                { "Heart 4 Full", new Source( null ,true) },
                { "Heart 5 Full", new Source( null ,true) },
                { "Heart 6 Full", new Source( null ,false) },
                { "Heart 7 Full", new Source( null ,false) },
                { "Heart 8 Full", new Source( null ,false) },
                { "Heart 9 Full", new Source( null ,false) },
                { "Heart 0 Full", new Source( null ,false) }
            };

            Assert.IsTrue(ItemsAreEqual(expectedItems, allItems));
        }

        [TestMethod]
        public void UpdateCount_ThreeTimes_ShowsCorrectHearts()
        {
            CPH.SetGlobalVar(MockCPH.lastFollowerCount, 15);
            CPH.SetArg(MockCPH.followerCount, 18);
            currentCounter.DataHandler.Initialize();
            DataHandlerWithItem currentDataHandler = currentCounter.DataHandler as DataHandlerWithItem;
            currentCounter.ResetToLast();

            currentCounter.DataHandler.UpdateCount(MockCPH.followerCount);
            currentCounter.WriteToOBS();
            currentCounter.DataHandler.UpdateCount(MockCPH.followerCount);
            currentCounter.WriteToOBS();
            currentCounter.DataHandler.UpdateCount(MockCPH.followerCount);
            currentCounter.WriteToOBS();

            SceneSourcePair currentSources = currentDataHandler.ItemSource;
            var allItems = GetAllCountItems(currentSources, 10, CPH);
            var expectedItems = new Dictionary<string, Source>()
            {
                { "Heart 1 Full", new Source( null ,true) },
                { "Heart 2 Full", new Source( null ,true) },
                { "Heart 3 Full", new Source( null ,true) },
                { "Heart 4 Full", new Source( null ,true) },
                { "Heart 5 Full", new Source( null ,true) },
                { "Heart 6 Full", new Source( null ,true) },
                { "Heart 7 Full", new Source( null ,true) },
                { "Heart 8 Full", new Source( null ,true) },
                { "Heart 9 Full", new Source( null ,false) },
                { "Heart 0 Full", new Source( null ,false) }
            };

            Assert.IsTrue(ItemsAreEqual(expectedItems, allItems));
        }

        [TestMethod]
        public void UpdateCount_UpdateThreeTimes_ShowsCorrectHearts()
        {
            CPH.SetGlobalVar(MockCPH.lastFollowerCount, 15);
            CPH.SetArg(MockCPH.followerCount, 18);
            currentCounter.DataHandler.Initialize();
            DataHandlerWithItem currentDataHandler = currentCounter.DataHandler as DataHandlerWithItem;
            currentCounter.ResetToLast();

            currentCounter.DataHandler.UpdateCount(MockCPH.followerCount);
            currentCounter.WriteToOBS();
            CPH.SetArg(MockCPH.followerCount, 22);
            currentCounter.DataHandler.UpdateCount(MockCPH.followerCount);
            currentCounter.WriteToOBS();
            CPH.SetArg(MockCPH.followerCount, 29);
            currentCounter.DataHandler.UpdateCount(MockCPH.followerCount);
            currentCounter.WriteToOBS();

            SceneSourcePair currentSources = currentDataHandler.ItemSource;
            var allItems = GetAllCountItems(currentSources, 10, CPH);
            var expectedItems = new Dictionary<string, Source>()
            {
                { "Heart 1 Full", new Source( null ,true) },
                { "Heart 2 Full", new Source( null ,true) },
                { "Heart 3 Full", new Source( null ,true) },
                { "Heart 4 Full", new Source( null ,true) },
                { "Heart 5 Full", new Source( null ,true) },
                { "Heart 6 Full", new Source( null ,true) },
                { "Heart 7 Full", new Source( null ,true) },
                { "Heart 8 Full", new Source( null ,true) },
                { "Heart 9 Full", new Source( null ,true) },
                { "Heart 0 Full", new Source( null ,false) }
            };

            Assert.IsTrue(ItemsAreEqual(expectedItems, allItems));
        }
    }
}
