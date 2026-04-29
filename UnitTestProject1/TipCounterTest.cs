using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Counters;

namespace Test_Chamber
{
    [TestClass]
    public class TipCounterTest
    {
        MockCPH CPH = null;
        Counter currentCounter = null;

        [TestInitialize()]
        public void Initialize()
        {
            CPH = new MockCPH();
            CounterManager.InitializeAll(CPH);
            currentCounter = CounterManager.tipCounter;
        }

        // Run Once - Previous count.
        // tipCounter multiplies every value by 5 because 1€ costs 5 tipAmount. So after one execute, it should be 50.
        [TestMethod]
        public void Execute_RunOnce_PreviousCountAfterExecuteIsHalfTheTipsRecieved()
        {
            CPH.SetGlobalVar(MockCPH.lastTips, 0);
            CPH.SetArg(MockCPH.tipAmount, 10);

            currentCounter.Execute();
            int previousTipCountAfter = currentCounter.DataHandler.PreviousCount;

            Assert.AreEqual(50, previousTipCountAfter);
        }

        [TestMethod]
        public void Execute_RunOnce_PreviousCountTheSameAsRecievedLastTipsBeforeExecute()
        {
            CPH.SetGlobalVar(MockCPH.lastTips, 5);
            CPH.SetArg(MockCPH.tipAmount, 10);
            int lastTipCountBefore = currentCounter.DataHandler.PreviousCount;

            currentCounter.Execute();

            Assert.AreEqual(5, lastTipCountBefore);
        }

        [TestMethod]
        public void Execute_PreviousCountIsHigherThatCurrent_PreviousCountIsSetToCurrent()
        {
            CPH.SetGlobalVar(MockCPH.lastTips, 10);
            CPH.SetArg(MockCPH.tipAmount, 5);

            currentCounter.Execute();
            int previousTipCountAfter = currentCounter.DataHandler.PreviousCount;

            // Since the tipAmount is 5, the current count will be 5 * 5 = 25. The previous count is higher than the current count, so it should be set to the current count, which is 25.
            Assert.AreEqual(25, previousTipCountAfter);
        }

        // Run Once - Current count.
        [TestMethod]
        public void Execute_RunOnce_CurrentCountAfterExecuteIsFiveTimesTheTipsRecieved()
        {
            CPH.SetGlobalVar(MockCPH.lastTips, 0);
            CPH.SetArg(MockCPH.tipAmount, 10);

            currentCounter.Execute();
            int currentTipCountAfter = currentCounter.DataHandler.CurrentCount;

            Assert.AreEqual(50, currentTipCountAfter);
        }

        // Run Once - OBS Source.
        [TestMethod]
        public void Execute_RunOnce_ObsCountIsFiveTimesTheRecievedTipsAfterExecute()
        {

            CPH.SetGlobalVar(MockCPH.lastTips, 0);
            CPH.SetArg(MockCPH.tipAmount, 10);

            currentCounter.Execute();

            bool result = CPH._sources.TryGetValue(SOURCES.TIP_COUNT.Source, out Source<string> afterValue);
            int sourceCountAfter = Convert.ToInt16(result ? afterValue.Value : "-1");

            Assert.AreEqual(50, sourceCountAfter);
        }

        [TestMethod]
        public void Execute_RunOnce_ObsCountCountCorrectBeforeExecute()
        {

            CPH.SetGlobalVar(MockCPH.lastTips, 0);
            CPH.SetArg(MockCPH.tipAmount, 10);


            bool result = CPH._sources.TryGetValue(SOURCES.TIP_COUNT.Source, out Source<string> beforeValue);
            int sourceCountAfter = Convert.ToInt16(result ? beforeValue.Value : "-1");
            currentCounter.Execute();

            Assert.AreEqual(-1, sourceCountAfter);
        }

        // Run Three Times - OBS source.
        // bitCounter halves every value because Twitch takes half the tipAmount. So 10 tipAmount / 2 = 5 tipAmount per execute. After three executes, it should be 15.
        [TestMethod]
        public void Execute_RunThreeTimes_ObsCountIsIncrementedThreeTimes()
        {

            CPH.SetArg(MockCPH.tipAmount, 10);
            CPH.SetGlobalVar(MockCPH.lastTips, 0);

            currentCounter.Execute();
            currentCounter.Execute();
            currentCounter.Execute();

            bool result = CPH._sources.TryGetValue(SOURCES.TIP_COUNT.Source, out Source<string> afterValue);
            int sourceContentAfter = Convert.ToInt16(result ? afterValue.Value : "-1");

            Assert.AreEqual(150, sourceContentAfter);
        }

        // Run Three Times - Previous count.
        [TestMethod]
        public void Execute_RunThreeTimes_PreviousCountIsCorrectAfterThreeExecutes()
        {
            CPH.SetArg(MockCPH.tipAmount, 10);
            CPH.SetGlobalVar(MockCPH.lastTips, 0);

            currentCounter.Execute();
            currentCounter.Execute();
            currentCounter.Execute();
            int previousTipCountAfter = currentCounter.DataHandler.PreviousCount;

            // (10 + 10 + 10) * 5 = 150
            Assert.AreEqual(150, previousTipCountAfter);
        }

        [TestMethod]
        public void Execute_RunThreeTimesWithAnOddNumber5_PreviousCountIs75AfterThreeExecutes()
        {
            CPH.SetArg(MockCPH.tipAmount, 5);
            CPH.SetGlobalVar(MockCPH.lastTips, 0);

            currentCounter.Execute();
            currentCounter.Execute();
            currentCounter.Execute();
            int previousTipCountAfter = currentCounter.DataHandler.PreviousCount;

            // (5 + 5 + 5) * 5 = 75
            Assert.AreEqual(75, previousTipCountAfter);
        }
    }
}
