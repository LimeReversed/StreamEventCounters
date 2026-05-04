using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Counters;

namespace Test_Chamber
{
    [TestClass]
    public class TipCounterTest
    {
        // tipCounter multiplies every value output by 5 so it matches the bit point.
        MockCPH CPH = null;
        Counter currentCounter = null;

        [TestInitialize()]
        public void Initialize()
        {
            CPH = new MockCPH();
            CounterManager.InitializeAll(CPH);
            currentCounter = CounterManager.tipCounter;
        }

        
        [TestMethod]
        public void Initialize_InitializingThroughConstructor_LastCountOutputShows5TimesLastTips()
        {

            CPH.SetArg(MockCPH.tipAmount, 10);
            CPH.SetGlobalVar(MockCPH.lastTips, 6);

            var dataHandler = new DataHandler(CPH, MockCPH.tipAmount, MockCPH.lastTips, CounterManager.TIP_INACTIVE_SOURCES, CounterManager.TIP_ACTIVE_SOURCES, CounterManager.TIP_COUNTER_SOURCES, (nr) => nr * 5);
            Counter newCounter = new Counter(CPH, dataHandler, 1, null);

            Assert.AreEqual(30, newCounter.DataHandler.LastCountOutput);
        }

        [TestMethod]
        public void Initialize_InitializingUsingInitializeMethod_LastCountOutputAfterExecuteIs5TimeTheTipsRecieved()
        {

            CPH.SetArg(MockCPH.tipAmount, 10);
            CPH.SetGlobalVar(MockCPH.lastTips, 6);
            currentCounter.DataHandler.Initialize();

            Assert.AreEqual(30, currentCounter.DataHandler.LastCountOutput);
        }

        [TestMethod]
        public void Initialize_InitializingThroughConstructor_CountOutputShows5TimesLastTips()
        {

            CPH.SetArg(MockCPH.tipAmount, 10);
            CPH.SetGlobalVar(MockCPH.lastTips, 6);

            var dataHandler = new DataHandler(CPH, MockCPH.tipAmount, MockCPH.lastTips, CounterManager.TIP_INACTIVE_SOURCES, CounterManager.TIP_ACTIVE_SOURCES, CounterManager.TIP_COUNTER_SOURCES, (nr) => nr * 5);
            Counter newCounter = new Counter(CPH, dataHandler, 1, null);

            Assert.AreEqual(30, newCounter.DataHandler.CountOutput);
        }

        [TestMethod]
        public void Initialize_InitializingUsingInitializeMethod_CountOutputAfterExecuteIs5TimeTheTipsRecieved()
        {

            CPH.SetArg(MockCPH.tipAmount, 10);
            CPH.SetGlobalVar(MockCPH.lastTips, 6);
            currentCounter.DataHandler.Initialize();

            Assert.AreEqual(30, currentCounter.DataHandler.CountOutput);
        }

        // Run Once - IncrementCount - Previous count
        [TestMethod]
        public void IncrementCount_RunOnce_LastCountTheSameAfterFirstIncrement()
        {
            CPH.SetArg(MockCPH.tipAmount, 10);
            CPH.SetGlobalVar(MockCPH.lastTips, 5);
            currentCounter.DataHandler.Initialize();

            int lastBitCountBefore = currentCounter.DataHandler.LastCountOutput;
            currentCounter.DataHandler.IncrementCount(MockCPH.tipAmount);
            int lastBitCountAfter = currentCounter.DataHandler.LastCountOutput;

            // On initialization Count and Initial count are both set to the last saved count. When the count updates then LastCount = Count, meaning that the first
            // time IncrementCount is called, it should not change. 
            Assert.AreEqual(lastBitCountBefore, lastBitCountAfter);
        }

        //// Run Once - Current count.  

        [TestMethod]
        public void IncrementCount_LastCountIsHigherThanCurrent_TheyAreStillAdded()
        {
            CPH.SetArg(MockCPH.tipAmount, 2);
            CPH.SetGlobalVar(MockCPH.lastTips, 8);
            currentCounter.DataHandler.Initialize();

            currentCounter.DataHandler.IncrementCount(MockCPH.tipAmount);

            // 2 + 8 = 10 then 10 * 5 = 50.
            Assert.AreEqual(50, currentCounter.DataHandler.CountOutput);
        }

        [TestMethod]
        public void IncrementCount_LastTipsIs0_CountAfterExecuteIs5TimeTheTipsRecieved()
        {
            CPH.SetArg(MockCPH.tipAmount, 10);
            CPH.SetGlobalVar(MockCPH.lastTips, 0);
            currentCounter.DataHandler.Initialize();

            currentCounter.DataHandler.IncrementCount(MockCPH.tipAmount);
            int currentBitCountAfter = currentCounter.DataHandler.CountOutput;

            Assert.AreEqual(50, currentBitCountAfter);
        }

        //// Run Once - OBS Source.

        [TestMethod]
        public void WriteToOBS_RunOnce_ObsCountIs5TimesTheRecievedTipsAfterExecute()
        {

            CPH.SetArg(MockCPH.tipAmount, 10);
            CPH.SetGlobalVar(MockCPH.lastTips, 0);
            currentCounter.DataHandler.Initialize();

            currentCounter.DataHandler.IncrementCount(MockCPH.tipAmount);
            currentCounter.WriteToOBS();

            bool result = CPH._sources.TryGetValue(SOURCES.TIP_COUNT.Source, out Source afterValue);
            int sourceCountAfter = Convert.ToInt16(result ? afterValue.Value : "-1");

            Assert.AreEqual(50, sourceCountAfter);
        }

        [TestMethod]
        public void WriteToOBS_RunOnce_ObsCountCorrectBeforeExecute()
        {

            CPH.SetGlobalVar(MockCPH.lastSubscriberCount, 0);
            CPH.SetArg(MockCPH.subscriberCount, 10);
            currentCounter.DataHandler.Initialize();


            bool result = CPH._sources.TryGetValue(SOURCES.TIP_COUNT.Source, out Source beforeValue);
            int sourceCountAfter = Convert.ToInt16(result ? beforeValue.Value : "-1");
            currentCounter.DataHandler.IncrementCount(MockCPH.tipAmount);
            currentCounter.WriteToOBS();

            Assert.AreEqual(-1, sourceCountAfter);
        }

        // Run Three Times - OBS source.
        [TestMethod]
        public void WriteToOBS_RunThreeTimes_ObsCountIsIncrementedThreeTimes()
        {

            CPH.SetArg(MockCPH.tipAmount, 10);
            CPH.SetGlobalVar(MockCPH.lastTips, 0);
            currentCounter.DataHandler.Initialize();

            currentCounter.DataHandler.IncrementCount(MockCPH.tipAmount);
            currentCounter.WriteToOBS();
            currentCounter.DataHandler.IncrementCount(MockCPH.tipAmount);
            currentCounter.WriteToOBS();
            currentCounter.DataHandler.IncrementCount(MockCPH.tipAmount);
            currentCounter.WriteToOBS();

            bool result = CPH._sources.TryGetValue(SOURCES.TIP_COUNT.Source, out Source afterValue);
            int sourceContentAfter = Convert.ToInt16(result ? afterValue.Value : "-1");

            // 10 + 10 + 10 = 30 then 30 * 5 = 150.
            Assert.AreEqual(150, sourceContentAfter);
        }

        // Run Three Times - Previous count.
        [TestMethod]
        public void IncrementCount_RunThreeTimes_LastCountIsCorrectAfterThreeExecutes()
        {
            CPH.SetArg(MockCPH.tipAmount, 10);
            CPH.SetGlobalVar(MockCPH.lastTips, 0);
            currentCounter.DataHandler.Initialize();

            currentCounter.DataHandler.IncrementCount(MockCPH.tipAmount);
            currentCounter.DataHandler.IncrementCount(MockCPH.tipAmount);
            currentCounter.DataHandler.IncrementCount(MockCPH.tipAmount);
            int previousBitCountAfter = currentCounter.DataHandler.LastCountOutput;

            // Previous count should only have been incremented 2 times. So (10 * 2) * 5 = 100.
            Assert.AreEqual(100, previousBitCountAfter);
        }

        [TestMethod]
        public void IncrementCount_RunFourTimesWithAnOddNumber5_PreviousCountIs75AfterThreeExecutes()
        {
            CPH.SetArg(MockCPH.tipAmount, 5);
            CPH.SetGlobalVar(MockCPH.lastTips, 0);
            currentCounter.DataHandler.Initialize();

            currentCounter.DataHandler.IncrementCount(MockCPH.tipAmount);
            currentCounter.DataHandler.IncrementCount(MockCPH.tipAmount);
            currentCounter.DataHandler.IncrementCount(MockCPH.tipAmount);
            currentCounter.DataHandler.IncrementCount(MockCPH.tipAmount);

            // Previous Count is expected to increment 3 times, so (5 + 5 + 5) * 5 = 75.
            Assert.AreEqual(75, currentCounter.DataHandler.LastCountOutput);
        }

        // Run Three Times - Current count.
        [TestMethod]
        public void IncrementCount_RunThreeTimes_CountIsCorrectAfterThreeExecutes()
        {
            CPH.SetArg(MockCPH.tipAmount, 10);
            CPH.SetGlobalVar(MockCPH.lastTips, 0);
            currentCounter.DataHandler.Initialize();

            currentCounter.DataHandler.IncrementCount(MockCPH.tipAmount);
            currentCounter.DataHandler.IncrementCount(MockCPH.tipAmount);
            currentCounter.DataHandler.IncrementCount(MockCPH.tipAmount);
            int previousBitCountAfter = currentCounter.DataHandler.CountOutput;

            // Previous count should only have been incremented 2 times. So (10 * 3) * 5 = 150.
            Assert.AreEqual(150, previousBitCountAfter);
        }

        [TestMethod]
        public void IncrementCount_RunFourTimesWithAnOddNumber5_CountIs75AfterThreeExecutes()
        {
            CPH.SetArg(MockCPH.tipAmount, 5);
            CPH.SetGlobalVar(MockCPH.lastTips, 0);
            currentCounter.DataHandler.Initialize();

            currentCounter.DataHandler.IncrementCount(MockCPH.tipAmount);
            currentCounter.DataHandler.IncrementCount(MockCPH.tipAmount);
            currentCounter.DataHandler.IncrementCount(MockCPH.tipAmount);
            currentCounter.DataHandler.IncrementCount(MockCPH.tipAmount);

            // (5 * 4) * 5 = 100.
            Assert.AreEqual(100, currentCounter.DataHandler.CountOutput);
        }
    }
}
