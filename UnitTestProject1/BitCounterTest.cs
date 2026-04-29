using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Counters;

namespace Test_Chamber
{
    [TestClass]
    public class BitCounterTest
    {
        MockCPH CPH = null;
        Counter currentCounter = null;

        [TestInitialize()]
        public void Initialize()
        {
            CPH = new MockCPH();
            CounterManager.InitializeAll(CPH);
            currentCounter = CounterManager.bitCounter;
        }

        // Run Once - Previous count.
        // bitCounter halves every value because Twitch takes half the bits. So 10 bits / 2 = 5 bits per execute. After one execute, it should be 5.
        [TestMethod]
        public void Execute_RunOnce_PreviousCountAfterExecuteIsHalfTheBitsRecieved()
        {
            CPH.SetArg(MockCPH.bits, 10);
            CPH.SetGlobalVar(MockCPH.lastBits, 0);

            currentCounter.Execute();
            int previousBitCountAfter = currentCounter.DataHandler.PreviousCount;

            Assert.AreEqual(5, previousBitCountAfter);
        }

        [TestMethod]
        public void Execute_RunOnce_PreviousCountTheSameAsRecievedLastBitsBeforeExecute()
        {
            CPH.SetArg(MockCPH.bits, 10);
            CPH.SetGlobalVar(MockCPH.lastBits, 5);
            int lastBitCountBefore = currentCounter.DataHandler.PreviousCount;

            currentCounter.Execute();

            Assert.AreEqual(5, lastBitCountBefore);
        }

        [TestMethod]
        public void Execute_PreviousCountIsHigherThatCurrent_PreviousCountIsSetToCurrent()
        {
            CPH.SetArg(MockCPH.bits, 5);
            CPH.SetGlobalVar(MockCPH.lastBits, 10);

            currentCounter.Execute();
            int previousBitCountAfter = currentCounter.DataHandler.PreviousCount;

            // Recieved bit count is 5, so the current count will be 5 / 2 = 2. The previous count is higher than the current count, so it should be set to the current count, which is 2.
            Assert.AreEqual(2, previousBitCountAfter);
        }

        // Run Once - Current count.
        [TestMethod]
        public void Execute_RunOnce_CurrentCountAfterExecuteIsHalfTheBitsRecieved()
        {
            CPH.SetArg(MockCPH.bits, 10);
            CPH.SetGlobalVar(MockCPH.lastBits, 0);

            currentCounter.Execute();
            int currentBitCountAfter = currentCounter.DataHandler.CurrentCount;

            Assert.AreEqual(5, currentBitCountAfter);
        }

        // Run Once - OBS Source.
        [TestMethod]
        public void Execute_RunOnce_ObsCountIsHalfTheRecievedBitsAfterExecute()
        {

            CPH.SetArg(MockCPH.bits, 10);
            CPH.SetGlobalVar(MockCPH.lastBits, 0);

            currentCounter.Execute();

            bool result = CPH._sources.TryGetValue(SOURCES.BIT_COUNT.Source, out Source afterValue);
            int sourceCountAfter = Convert.ToInt16(result ? afterValue.Value : "-1");

            Assert.AreEqual(5, sourceCountAfter);
        }

        [TestMethod]
        public void Execute_RunOnce_ObsCountCountCorrectBeforeExecute()
        {

            CPH.SetGlobalVar(MockCPH.lastSubscriberCount, 0);
            CPH.SetArg(MockCPH.subscriberCount, 10);


            bool result = CPH._sources.TryGetValue(SOURCES.BIT_COUNT.Source, out Source beforeValue);
            int sourceCountAfter = Convert.ToInt16(result ? beforeValue.Value : "-1");
            currentCounter.Execute();

            Assert.AreEqual(-1, sourceCountAfter);
        }

        // Run Three Times - OBS source.
        // bitCounter halves every value because Twitch takes half the bits. So 10 bits / 2 = 5 bits per execute. After three executes, it should be 15.
        [TestMethod]
        public void Execute_RunThreeTimes_ObsCountIsIncrementedThreeTimes()
        {

            CPH.SetArg(MockCPH.bits, 10);
            CPH.SetGlobalVar(MockCPH.lastBits, 0);

            currentCounter.Execute();
            currentCounter.Execute();
            currentCounter.Execute();

            bool result = CPH._sources.TryGetValue(SOURCES.BIT_COUNT.Source, out Source afterValue);
            int sourceContentAfter = Convert.ToInt16(result ? afterValue.Value : "-1");

            Assert.AreEqual(15, sourceContentAfter);
        }

        // Run Three Times - Previous count.
        [TestMethod]
        public void Execute_RunThreeTimes_PreviousCountIsCorrectAfterThreeExecutes()
        {
            CPH.SetArg(MockCPH.bits, 10);
            CPH.SetGlobalVar(MockCPH.lastBits, 0);

            currentCounter.Execute();
            currentCounter.Execute();
            currentCounter.Execute();
            int previousBitCountAfter = currentCounter.DataHandler.PreviousCount;

            Assert.AreEqual(15, previousBitCountAfter);
        }

        [TestMethod]
        public void Execute_RunThreeTimesWithAnOddNumber5_PreviousCountIs7Point5AfterThreeExecutes()
        {
            CPH.SetArg(MockCPH.bits, 5);
            CPH.SetGlobalVar(MockCPH.lastBits, 0);

            currentCounter.Execute();
            currentCounter.Execute();
            currentCounter.Execute();
            int previousBitCountAfter = currentCounter.DataHandler.PreviousCount;

            // (5 + 5 + 5) / 2 = 7.5, but since the count is an integer, it should round down to 7.
            Assert.AreEqual(7, previousBitCountAfter);
        }
    }
}
