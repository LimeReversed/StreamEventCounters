using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Counters
{
    public class CPHInline
    {
        //public bool initialized = false;
        //// Since I have "Ignore gift sub" checket the gift sub event will only trigger if it's one gift sub, otherwise it's a giftbomb. 
        //static string GIFT_BOMB_AMOUNT_ARG = "gifts";
        //static string BIT_AMOUNT_ARG = "bits";
        //static string TIP_AMOUNT_ARG = "tipAmount";

        //// Subs
        //public bool HandleNewSubscriber()
        //{
        //    if (!initialized)
        //    {
        //        Initialize();
        //    }

        //    CounterManager.subCounter.DataHandler.IncrementCount(1);
        //    CounterManager.subCounter.WriteToOBS();
        //    return true;
        //}

        //public bool HandleGiftBomb()
        //{
        //    if (!initialized)
        //    {
        //        Initialize();
        //    }

        //    CounterManager.subCounter.DataHandler.IncrementCount(GIFT_BOMB_AMOUNT_ARG);
        //    CounterManager.subCounter.WriteToOBS();
        //    return true;
        //}

        //// Follows
        //public bool HandleNewFollower()
        //{
        //    if (!initialized)
        //    {
        //        Initialize();
        //    }

        //    CounterManager.followCounter.DataHandler.IncrementCount(1);
        //    CounterManager.followCounter.WriteToOBS();
        //    return true;
        //}

        //public bool RefreshHearts()
        //{
        //    if (!initialized)
        //    {
        //        Initialize();
        //    }

        //    CounterManager.followCounter?.Refresh();
        //    return true;
        //}

        //// ----- Bits -----
        //public bool HandleBitGet()
        //{
        //    if (!initialized)
        //    {
        //        Initialize();
        //    }

        //    CounterManager.bitCounter.DataHandler.IncrementCount(BIT_AMOUNT_ARG);
        //    CounterManager.bitCounter.WriteToOBS();
        //    return true;
        //}

        //// ----- Tips -----
        //public bool HandleTipGet()
        //{
        //    if (!initialized)
        //    {
        //        Initialize();
        //    }

        //    CounterManager.tipCounter.DataHandler.IncrementCount(TIP_AMOUNT_ARG);
        //    CounterManager.tipCounter.WriteToOBS();
        //    return true;
        //}

        //public bool Initialize()
        //{
        //    CounterManager.InitializeAll(CPH);
        //    CounterManager.tipCounter.ResetToLast();
        //    CounterManager.bitCounter.ResetToLast();
        //    CounterManager.subCounter.ResetToLast();
        //    CounterManager.followCounter.ResetToLast();
        //    initialized = true;
        //    return true;
        //}

        //public bool Execute()
        //{
        //    return true;
        //}
    }
}
