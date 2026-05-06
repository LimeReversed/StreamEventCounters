using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Counters;

namespace Test_Chamber
{
    [TestClass]
    public class SubCounterTest
    {
        MockCPH CPH = null;
        Counter currentCounter = null;

        [TestInitialize()]
        public void Initialize()
        {
            CPH = new MockCPH();
            CounterManager.InitializeAll(CPH);
            currentCounter = CounterManager.subCounter;
        }

        [TestMethod]
        public void Execute_saved_previous_count_correct_after_execute()
        {

            CPH.SetArg(MockCPH.subscriberCount, 6);
            CPH.SetGlobalVar(MockCPH.lastSubscriberCount, 5);
            currentCounter.DataHandler.Initialize();

            currentCounter.DataHandler.IncrementCount(1);
            currentCounter.WriteToOBS();

            bool result = CPH._sources.TryGetValue(SOURCES.SUB_COUNT.Source, out Source afterValue);
            int sourceContentAfter = Convert.ToInt16(result ? afterValue.Value : "-1");

            Assert.AreEqual(6, sourceContentAfter);
        }

        //[TestMethod]
        //public void Execute_saved_previous_count_correct_before_execute()
        //{

        //    CPH.SetArg(MockCPH.subscriberCount, 7);
        //    CPH.SetGlobalVar(MockCPH.lastSubscriberCount, 5);

        //    bool result = CPH._sources.TryGetValue(SOURCES.SUB_COUNT.Source, out Source beforeValue);
        //    int sourceContentBefore = Convert.ToInt16(result ? beforeValue.Value : "-1");
        //    CounterManager.subCounter.Execute();

        //    Assert.AreEqual(-1, sourceContentBefore);
        //}

        //[TestMethod]
        //public void Execute_previous_count_is_correct_after_three_executes()
        //{
        //    CPH.SetArg(MockCPH.subscriberCount, 5);

        //    CounterManager.subCounter.Execute();
        //    CounterManager.subCounter.Execute();
        //    CounterManager.subCounter.Execute();
        //    int previousSubCountAfter = CounterManager.subCounter.DataHandler.PreviousCount;

        //    Assert.AreEqual(5, previousSubCountAfter);
        //}

        //[TestMethod]
        //public void Execute_previous_count_is_higher_than_current_count()
        //{
        //    CPH.SetArg(MockCPH.subscriberCount, 5);
        //    CPH.SetGlobalVar(MockCPH.lastSubscriberCount, 10);

        //    CounterManager.subCounter.Execute();
        //    int previousSubCountAfter = CounterManager.subCounter.DataHandler.PreviousCount;

        //    // Also check saved.
        //    Assert.AreEqual(5, previousSubCountAfter);
        //}
    }
}
