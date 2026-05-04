using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Counters;

namespace Test_Chamber
{
    [TestClass]
    public class BitCounterTest
    {
        // bitCounter halves every value output because Twitch takes half the bits. So 10 bits / 2 = 5 bits per Update.
        MockCPH CPH = null;
        Counter currentCounter = null;

        [TestInitialize()]
        public void Initialize()
        {
            CPH = new MockCPH();
            CounterManager.InitializeAll(CPH);
            currentCounter = CounterManager.bitCounter;
        }

        
        [TestMethod]
        public void Initialize_InitializingThroughConstructor_LastCountOutputShowsHalfOfLastBits()
        {

            CPH.SetArg(MockCPH.bits, 10);
            CPH.SetGlobalVar(MockCPH.lastBits, 6);

            var dataHandler = new DataHandler(CPH, MockCPH.bits, MockCPH.lastBits, CounterManager.BIT_INACTIVE_SOURCES, CounterManager.BIT_ACTIVE_SOURCES, CounterManager.BIT_COUNTER_SOURCES, (nr) => nr / 2);
            Counter newCounter = new Counter(CPH, dataHandler, 1, null);

            Assert.AreEqual(3, newCounter.DataHandler.LastCountOutput);
        }

        [TestMethod]
        public void Initialize_InitializingUsingInitializeMethod_LastCountOutputAfterExecuteIsHalfTheBitsRecieved()
        {

            CPH.SetArg(MockCPH.bits, 10);
            CPH.SetGlobalVar(MockCPH.lastBits, 6);
            currentCounter.DataHandler.Initialize();

            Assert.AreEqual(3, currentCounter.DataHandler.LastCountOutput);
        }

        [TestMethod]
        public void Initialize_InitializingThroughConstructor_CountOutputShowsHalfOfLastBits()
        {

            CPH.SetArg(MockCPH.bits, 10);
            CPH.SetGlobalVar(MockCPH.lastBits, 6);

            var dataHandler = new DataHandler(CPH, MockCPH.bits, MockCPH.lastBits, CounterManager.BIT_INACTIVE_SOURCES, CounterManager.BIT_ACTIVE_SOURCES, CounterManager.BIT_COUNTER_SOURCES, (nr) => nr / 2);
            Counter newCounter = new Counter(CPH, dataHandler, 1, null);

            Assert.AreEqual(3, newCounter.DataHandler.CountOutput);
        }

        [TestMethod]
        public void Initialize_InitializingUsingInitializeMethod_CountOutputAfterExecuteIsHalfTheBitsRecieved()
        {

            CPH.SetArg(MockCPH.bits, 10);
            CPH.SetGlobalVar(MockCPH.lastBits, 6);
            currentCounter.DataHandler.Initialize();

            Assert.AreEqual(3, currentCounter.DataHandler.CountOutput);
        }

        // Run Once - IncrementCount - Previous count
        [TestMethod]
        public void IncrementCount_RunOnce_LastCountTheSameAfterFirstIncrement()
        {
            CPH.SetArg(MockCPH.bits, 10);
            CPH.SetGlobalVar(MockCPH.lastBits, 5);
            currentCounter.DataHandler.Initialize();

            int lastBitCountBefore = currentCounter.DataHandler.LastCountOutput;
            currentCounter.DataHandler.IncrementCount(MockCPH.bits);
            int lastBitCountAfter = currentCounter.DataHandler.LastCountOutput;

            // On initialization Count and Initial count are both set to the last saved count. When the count updates then LastCount = Count, meaning that the first
            // time IncrementCount is called, it should not change. 
            Assert.AreEqual(lastBitCountBefore, lastBitCountAfter);
        }

        //// Run Once - Current count.  

        [TestMethod]
        public void IncrementCount_LastCountIsHigherThanCurrent_TheyAreStillAdded()
        {
            CPH.SetArg(MockCPH.bits, 2);
            CPH.SetGlobalVar(MockCPH.lastBits, 8);
            currentCounter.DataHandler.Initialize();

            currentCounter.DataHandler.IncrementCount(MockCPH.bits);

            // 2 + 8 = 10 then 10 / 2 = 5.
            Assert.AreEqual(5, currentCounter.DataHandler.CountOutput);
        }

        [TestMethod]
        public void IncrementCount_LastBitsIs0_CountAfterExecuteIsHalfTheBitsRecieved()
        {
            CPH.SetArg(MockCPH.bits, 10);
            CPH.SetGlobalVar(MockCPH.lastBits, 0);
            currentCounter.DataHandler.Initialize();

            currentCounter.DataHandler.IncrementCount(MockCPH.bits);
            int currentBitCountAfter = currentCounter.DataHandler.CountOutput;

            Assert.AreEqual(5, currentBitCountAfter);
        }

        //// Run Once - OBS Source.

        [TestMethod]
        public void WriteToOBS_RunOnce_ObsCountIsHalfTheRecievedBitsAfterExecute()
        {

            CPH.SetArg(MockCPH.bits, 10);
            CPH.SetGlobalVar(MockCPH.lastBits, 0);
            currentCounter.DataHandler.Initialize();

            currentCounter.DataHandler.IncrementCount(MockCPH.bits);
            currentCounter.WriteToOBS();

            bool result = CPH._sources.TryGetValue(SOURCES.BIT_COUNT.Source, out Source afterValue);
            int sourceCountAfter = Convert.ToInt16(result ? afterValue.Value : "-1");

            Assert.AreEqual(5, sourceCountAfter);
        }

        [TestMethod]
        public void WriteToOBS_RunOnce_ObsCountCorrectBeforeExecute()
        {

            CPH.SetGlobalVar(MockCPH.lastSubscriberCount, 0);
            CPH.SetArg(MockCPH.subscriberCount, 10);
            currentCounter.DataHandler.Initialize();


            bool result = CPH._sources.TryGetValue(SOURCES.BIT_COUNT.Source, out Source beforeValue);
            int sourceCountAfter = Convert.ToInt16(result ? beforeValue.Value : "-1");
            currentCounter.DataHandler.IncrementCount(MockCPH.bits);
            currentCounter.WriteToOBS();

            Assert.AreEqual(-1, sourceCountAfter);
        }

        // Run Three Times - OBS source.
        [TestMethod]
        public void WriteToOBS_RunThreeTimes_ObsCountIsIncrementedThreeTimes()
        {

            CPH.SetArg(MockCPH.bits, 10);
            CPH.SetGlobalVar(MockCPH.lastBits, 0);
            currentCounter.DataHandler.Initialize();

            currentCounter.DataHandler.IncrementCount(MockCPH.bits);
            currentCounter.WriteToOBS();
            currentCounter.DataHandler.IncrementCount(MockCPH.bits);
            currentCounter.WriteToOBS();
            currentCounter.DataHandler.IncrementCount(MockCPH.bits);
            currentCounter.WriteToOBS();

            bool result = CPH._sources.TryGetValue(SOURCES.BIT_COUNT.Source, out Source afterValue);
            int sourceContentAfter = Convert.ToInt16(result ? afterValue.Value : "-1");

            // 10 + 10 + 10 = 30 then 30 / 2 = 15.
            Assert.AreEqual(15, sourceContentAfter);
        }

        // Run Three Times - Previous count.
        [TestMethod]
        public void IncrementCount_RunThreeTimes_LastCountIsCorrectAfterThreeExecutes()
        {
            CPH.SetArg(MockCPH.bits, 10);
            CPH.SetGlobalVar(MockCPH.lastBits, 0);
            currentCounter.DataHandler.Initialize();

            currentCounter.DataHandler.IncrementCount(MockCPH.bits);
            currentCounter.DataHandler.IncrementCount(MockCPH.bits);
            currentCounter.DataHandler.IncrementCount(MockCPH.bits);
            int previousBitCountAfter = currentCounter.DataHandler.LastCountOutput;

            // Previous count should only have been incremented 2 times. So (10 / 2) * 2 = 10.
            Assert.AreEqual(10, previousBitCountAfter);
        }

        [TestMethod]
        public void IncrementCount_RunFourTimesWithAnOddNumber5_PreviousCountIs7PointAfterThreeExecutes()
        {
            CPH.SetArg(MockCPH.bits, 5);
            CPH.SetGlobalVar(MockCPH.lastBits, 0);
            currentCounter.DataHandler.Initialize();

            currentCounter.DataHandler.IncrementCount(MockCPH.bits);
            currentCounter.DataHandler.IncrementCount(MockCPH.bits);
            currentCounter.DataHandler.IncrementCount(MockCPH.bits);
            currentCounter.DataHandler.IncrementCount(MockCPH.bits);

            // Previous Count is expected to increment 3 times, so (5 + 5 + 5) / 2 = 7.5, but since the count is an integer, it should round down to 7.
            Assert.AreEqual(7, currentCounter.DataHandler.LastCountOutput);
        }

        // Run Three Times - Current count.
        [TestMethod]
        public void IncrementCount_RunThreeTimes_CountIsCorrectAfterThreeExecutes()
        {
            CPH.SetArg(MockCPH.bits, 10);
            CPH.SetGlobalVar(MockCPH.lastBits, 0);
            currentCounter.DataHandler.Initialize();

            currentCounter.DataHandler.IncrementCount(MockCPH.bits);
            currentCounter.DataHandler.IncrementCount(MockCPH.bits);
            currentCounter.DataHandler.IncrementCount(MockCPH.bits);
            int previousBitCountAfter = currentCounter.DataHandler.CountOutput;

            // Previous count should only have been incremented 2 times. So (10 / 2) * 2 = 10.
            Assert.AreEqual(15, previousBitCountAfter);
        }

        [TestMethod]
        public void IncrementCount_RunFourTimesWithAnOddNumber5_CountIs10PointAfterThreeExecutes()
        {
            CPH.SetArg(MockCPH.bits, 5);
            CPH.SetGlobalVar(MockCPH.lastBits, 0);
            currentCounter.DataHandler.Initialize();

            currentCounter.DataHandler.IncrementCount(MockCPH.bits);
            currentCounter.DataHandler.IncrementCount(MockCPH.bits);
            currentCounter.DataHandler.IncrementCount(MockCPH.bits);
            currentCounter.DataHandler.IncrementCount(MockCPH.bits);

            // Previous Count is expected to increment 3 times, so (5 + 5 + 5) / 2 = 7.5, but since the count is an integer, it should round down to 7.
            Assert.AreEqual(10, currentCounter.DataHandler.CountOutput);
        }
    }
}
