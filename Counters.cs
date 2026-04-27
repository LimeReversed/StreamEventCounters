using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Counters
{
    public class CounterManager
    {
        public TipCounter tipCounter = null;
        public BitCounter bitCounter = null;
        public LoopableCounter followerCounter = null;
        public StarCounter subscriberCounter = null;
        public Counter testCounter = null;
        public bool initialized = false;
        public IInlineInvokeProxy CPH { get; set; } = new MockCPH();
        public bool UpdateHearts()
        {
            if (followerCounter == null)
            {
                InitializeFollowCounter();
                followerCounter.UpdatePackage();
                return true;
            }

            followerCounter.Execute();
            return true;
        }

        public bool RefreshHearts()
        {
            if (followerCounter == null)
            {
                InitializeFollowCounter();
            }

            followerCounter.Refresh();
            return true;
        }

        public bool UpdateStars()
        {
            if (subscriberCounter == null)
            {
                subscriberCounter = new StarCounter(CPH);
                subscriberCounter.UpdatePackage();
                return true;
            }

            subscriberCounter.Execute();
            return true;
        }

        // ----- Bits -----
        public bool UpdateBits()
        {
            if (bitCounter == null)
            {
                bitCounter = new BitCounter(CPH);
                bitCounter.UpdatePackage();
                return true;
            }

            bitCounter.Execute();
            return true;
        }

        // ----- Tips -----
        public bool UpdateTips()
        {
            if (tipCounter == null)
            {
                tipCounter = new TipCounter(CPH);
                tipCounter.UpdatePackage();
                return true;
            }

            tipCounter.Execute();
            return true;
        }

        public void InitializeFollowCounter()
        {
            SceneSourcePair followerIncrementSound = new SceneSourcePair(SOURCES.SOUND_FOLLOWER_INCREMENT.Scene, SOURCES.SOUND_FOLLOWER_INCREMENT.Source);
            SceneSourcePair followerDecrementSound = new SceneSourcePair(SOURCES.SOUND_FOLLOWER_DECREMENT.Scene, SOURCES.SOUND_FOLLOWER_DECREMENT.Source);
            SceneSourcePair followerItemSource = new SceneSourcePair(SOURCES.HEART_FULL.Scene, SOURCES.HEART_FULL.Source);
            followerCounter = new LoopableCounter("followerCount", "savedFollowerCount", null, null, null, followerItemSource, followerIncrementSound, followerDecrementSound, 400, 10, CPH);
        }

        public bool Initialize()
        {
            tipCounter = new TipCounter(CPH);
            bitCounter = new BitCounter(CPH);
            subscriberCounter = new StarCounter(CPH);
            InitializeFollowCounter();
            // bitCounter.CurrentCountName = "message";
            tipCounter.UpdatePackage();
            bitCounter.UpdatePackage();
            subscriberCounter.UpdatePackage();
            followerCounter.UpdatePackage();
            initialized = true;
            return true;
        }

        public bool Execute()
        {
            if (!initialized)
            {
                Initialize();
                return true;
            }

            tipCounter.Execute();
            bitCounter.Execute();
            subscriberCounter.Execute();
            followerCounter.Execute();
            return true;
        }
    }

    // *************************************************************************************************************************************
    // ********************************************* CONSTANTS *****************************************************************************
    public static class SCENES
    {
        public static string OVERLAY_UNIVERSAL = "Overlay - Universal";
        public static string OVERLAY_UNIVERSAL_COUNTERS = "Overlay - Universal - Counters";
        public static string OVERLAY_UNIVERSAL_COUNTERS_SCREEN = "Overlay - Universal - Counters (Screen)";
        public static string OVERLAY_CHAT = "Overlay - Chat";
        public static string OVERLAY_CHAT_COUNTERS = "Overlay - Chat - Counters";
    }

    public static class SOURCES
    {
        public static SceneSourcePair VIDEO_BIT_GET_SCREEN = new SceneSourcePair(SCENES.OVERLAY_UNIVERSAL_COUNTERS_SCREEN, "Video - Bit Get Screen");
        public static SceneSourcePair VIDEO_TIP_GET_SCREEN = new SceneSourcePair(SCENES.OVERLAY_UNIVERSAL_COUNTERS_SCREEN, "Video - Tip Get Screen");
        public static SceneSourcePair VIDEO_BIT_GET = new SceneSourcePair(SCENES.OVERLAY_UNIVERSAL_COUNTERS, "Video - Bit Get");
        public static SceneSourcePair VIDEO_TIP_GET = new SceneSourcePair(SCENES.OVERLAY_UNIVERSAL_COUNTERS, "Video - Tip Get");
        public static SceneSourcePair BIT_COUNT = new SceneSourcePair(SCENES.OVERLAY_UNIVERSAL_COUNTERS, "Bit Count");
        public static SceneSourcePair BIT_COUNT_ACTIVE = new SceneSourcePair(SCENES.OVERLAY_UNIVERSAL_COUNTERS, "Bit Count Active");
        public static SceneSourcePair BIT_COUNT_BACKGROUND = new SceneSourcePair(SCENES.OVERLAY_UNIVERSAL_COUNTERS, "Bit Count Background");
        public static SceneSourcePair BIT_COUNT_BACKGROUND_ACTIVE = new SceneSourcePair(SCENES.OVERLAY_UNIVERSAL_COUNTERS, "Bit Count Background Active");
        public static SceneSourcePair TIP_COUNT = new SceneSourcePair(SCENES.OVERLAY_UNIVERSAL_COUNTERS, "Tip Count");
        public static SceneSourcePair TIP_COUNT_ACTIVE = new SceneSourcePair(SCENES.OVERLAY_UNIVERSAL_COUNTERS, "Tip Count Active");
        public static SceneSourcePair TIP_COUNT_BACKGROUND = new SceneSourcePair(SCENES.OVERLAY_UNIVERSAL_COUNTERS, "Tip Count Background");
        public static SceneSourcePair TIP_COUNT_BACKGROUND_ACTIVE = new SceneSourcePair(SCENES.OVERLAY_UNIVERSAL_COUNTERS, "Tip Count Background Active");
        public static SceneSourcePair SOUND_RING_INCREMENT = new SceneSourcePair(SCENES.OVERLAY_UNIVERSAL_COUNTERS, "Sound - Ring Increment");
        public static SceneSourcePair SOUND_MULTI_RING_GET = new SceneSourcePair(SCENES.OVERLAY_UNIVERSAL_COUNTERS, "Sound - Multi Ring Get");
        public static SceneSourcePair SOUND_SUB_INCREMENT = new SceneSourcePair(SCENES.OVERLAY_UNIVERSAL_COUNTERS, "Sound - Sub Increment");
        public static SceneSourcePair SUB_COUNT = new SceneSourcePair(SCENES.OVERLAY_UNIVERSAL_COUNTERS, "Sub Count");
        public static SceneSourcePair SUB_COUNT_ACTIVE = new SceneSourcePair(SCENES.OVERLAY_UNIVERSAL_COUNTERS, "Sub Count Active");
        public static SceneSourcePair SUB_COUNT_BACKGROUND = new SceneSourcePair(SCENES.OVERLAY_UNIVERSAL_COUNTERS, "Sub Count Background");
        public static SceneSourcePair SUB_COUNT_BACKGROUND_ACTIVE = new SceneSourcePair(SCENES.OVERLAY_UNIVERSAL_COUNTERS, "Sub Count Background Active");
        public static SceneSourcePair HEART_FULL = new SceneSourcePair(SCENES.OVERLAY_UNIVERSAL_COUNTERS, "Heart # Full");
        public static SceneSourcePair SOUND_FOLLOWER_DECREMENT = new SceneSourcePair(SCENES.OVERLAY_UNIVERSAL_COUNTERS, "Sound - Follower Decrement");
        public static SceneSourcePair SOUND_FOLLOWER_INCREMENT = new SceneSourcePair(SCENES.OVERLAY_UNIVERSAL_COUNTERS, "Sound - Follower Increment");
    }

    public class SceneSourcePair
    {
        public SceneSourcePair(string scene, string source)
        {
            Scene = scene;
            Source = source;
        }

        public string Scene { get; set; }
        public string Source { get; set; }
    }


    // *************************************************************************************************************************************
    // ********************************************* COUNTER *****************************************************************************
    public class Counter
    {
        private int _previousCount = 0;
        protected IInlineInvokeProxy _cph;
        public int InitialCount { get; set; } = 0;

        public int PreviousCount
        {
            get
            {
                return _cph.GetGlobalVar<int>(PreviousCountName, true);
            }

            set
            {
                _previousCount = value;
                _cph.SetGlobalVar(PreviousCountName, value, true);
            }
        }

        public int CurrentCount { get; set; } = 0;
        public string CurrentCountName { get; set; }
        public string PreviousCountName { get; set; }
        public int LoopSpeed { get; set; } = 0;
        public int EndLoopSpeed { get; set; } = 0;
        public SceneSourcePair SoundSource { get; set; } = null;
        public List<SceneSourcePair> InactiveSources { get; set; } = new List<SceneSourcePair>();
        public List<SceneSourcePair> ActiveSources { get; set; } = new List<SceneSourcePair>();
        public List<SceneSourcePair> CounterSources { get; set; } = new List<SceneSourcePair>();

        public Counter(string currentCountName, string previousCountName, List<SceneSourcePair> activeSources, List<SceneSourcePair> inactiveSources, List<SceneSourcePair> counterSources, int loopSpeed, IInlineInvokeProxy cph, int endLoopSpeed = -1)
        {
            CurrentCountName = currentCountName;
            ActiveSources = activeSources ?? new List<SceneSourcePair>();
            InactiveSources = inactiveSources ?? new List<SceneSourcePair>();
            CounterSources = counterSources ?? new List<SceneSourcePair>();
            LoopSpeed = loopSpeed;
            PreviousCountName = previousCountName;
            _cph = cph;
            EndLoopSpeed = endLoopSpeed == -1 ? LoopSpeed : endLoopSpeed;
            Initialize();
        }

        public void Initialize()
        {
            // Don't forget to actually get the values before running this code. 
            if (!_cph.TryGetArg<int>(CurrentCountName, out int currentCountResult))
            {
                _cph.LogError($"TryGetArg for {CurrentCountName} failed.");
                PrintToCounterSource(0);
                return;
            }

            InitialCount = PreviousCount;
            CurrentCount = currentCountResult;
        }

        public virtual void GetValues()
        {
            if (!_cph.TryGetArg<int>(CurrentCountName, out int currentCountResult))
            {
                _cph.LogError($"TryGetArg for {CurrentCountName} failed.");
                return;
            }

            CurrentCount += currentCountResult;
        }

        protected virtual void PrintToCounterSource(int number)
        {
            number = Convert(number);
            foreach (SceneSourcePair pair in CounterSources)
            {
                _cph.ObsSetGdiText(pair.Scene, pair.Source, number.ToString());
            }
        }

        protected virtual void PrintToCounterSource(int position, bool visible)
        {
            return;
        }

        protected virtual void SetUpdatingState(bool active)
        {
            foreach (SceneSourcePair pair in ActiveSources)
            {
                _cph.ObsSetSourceVisibility(pair.Scene, pair.Source, active);
            }

            foreach (SceneSourcePair pair in InactiveSources)
            {
                _cph.ObsSetSourceVisibility(pair.Scene, pair.Source, !active);
            }
        }

        protected virtual void PrepareUpdate()
        {
            if (PreviousCount >= CurrentCount)
            {
                PrintToCounterSource(CurrentCount);
            }

            SetUpdatingState(true);
            PrepareSound();
        }

        protected virtual void Update()
        {
            for (int i = Convert(PreviousCount); i <= Convert(CurrentCount); i++)
            {
                PrintToCounterSource(i);
                PlaySound();
                bool isLastIteration = i >= Convert(CurrentCount);
                _cph.Wait(isLastIteration ? EndLoopSpeed : LoopSpeed);
            }
        }

        protected virtual void FinishUpdate()
        {
            SetUpdatingState(false);
            PreviousCount = CurrentCount;
            FinishSound();
        }

        public virtual void UpdatePackage()
        {
            PrepareUpdate();
            Update();
            FinishUpdate();
        }

        protected virtual void PrepareSound()
        {
            return;
        }

        protected virtual void FinishSound()
        {
            return;
        }

        protected virtual void PlaySound()
        {
            return;
        }

        public virtual int Convert(int count)
        {
            return count;
        }

        public void Execute()
        {
            GetValues();
            UpdatePackage();
        }
    }

    public class TipCounter : Counter
    {
        private SceneSourcePair soundSource = new SceneSourcePair(SOURCES.SOUND_RING_INCREMENT.Scene, SOURCES.SOUND_RING_INCREMENT.Source);
        private SceneSourcePair soundLoopSource = new SceneSourcePair(SOURCES.SOUND_MULTI_RING_GET.Scene, SOURCES.SOUND_MULTI_RING_GET.Source);
        public static List<SceneSourcePair> ACTIVE_SOURCES = new List<SceneSourcePair>
{
    SOURCES.TIP_COUNT_ACTIVE,
    SOURCES.TIP_COUNT_BACKGROUND_ACTIVE
};
        public static List<SceneSourcePair> INACTIVE_SOURCES = new List<SceneSourcePair>
{
    SOURCES.TIP_COUNT,
    SOURCES.TIP_COUNT_BACKGROUND
};
        public static List<SceneSourcePair> COUNTER_SOURCES = new List<SceneSourcePair>
{
    SOURCES.TIP_COUNT,
    SOURCES.TIP_COUNT_ACTIVE
};
        public TipCounter(IInlineInvokeProxy cph) : base("tipAmount", "previousTips", ACTIVE_SOURCES, INACTIVE_SOURCES, COUNTER_SOURCES, 1, cph)
        {
        }

        public override int Convert(int count)
        {
            return count * 5;
        }

        protected override void PrepareSound()
        {
            _cph.ObsSetSourceVisibility(soundSource.Scene, soundSource.Source, true);
            _cph.ObsSetSourceVisibility(soundLoopSource.Scene, soundLoopSource.Source, true);
            _cph.ObsMediaRestart(soundLoopSource.Scene, soundLoopSource.Source);
        }

        protected override void FinishSound()
        {
            _cph.ObsMediaStop(soundLoopSource.Scene, soundLoopSource.Source);
            _cph.ObsMediaRestart(soundSource.Scene, soundSource.Source);
            _cph.Wait(500);
            _cph.ObsSetSourceVisibility(soundLoopSource.Scene, soundLoopSource.Source, false);
            _cph.ObsSetSourceVisibility(soundSource.Scene, soundSource.Source, false);
        }

        protected override void PlaySound()
        {
        }
    }

    public class BitCounter : Counter
    {
        private SceneSourcePair soundSource = new SceneSourcePair(SOURCES.SOUND_RING_INCREMENT.Scene, SOURCES.SOUND_RING_INCREMENT.Source);
        private SceneSourcePair soundLoopSource = new SceneSourcePair(SOURCES.SOUND_MULTI_RING_GET.Scene, SOURCES.SOUND_MULTI_RING_GET.Source);
        public static List<SceneSourcePair> ACTIVE_SOURCES = new List<SceneSourcePair>
{
    SOURCES.BIT_COUNT_ACTIVE,
    SOURCES.BIT_COUNT_BACKGROUND_ACTIVE
};
        public static List<SceneSourcePair> INACTIVE_SOURCES = new List<SceneSourcePair>
{
    SOURCES.BIT_COUNT,
    SOURCES.BIT_COUNT_BACKGROUND
};
        public static List<SceneSourcePair> COUNTER_SOURCES = new List<SceneSourcePair>
{
    SOURCES.BIT_COUNT,
    SOURCES.BIT_COUNT_ACTIVE
};
        public BitCounter(IInlineInvokeProxy cph) : base("bits", "previousBits", ACTIVE_SOURCES, INACTIVE_SOURCES, COUNTER_SOURCES, 1, cph)
        {
        }

        public override int Convert(int count)
        {
            return count / 2;
        }

        protected override void PrepareSound()
        {
            _cph.ObsSetSourceVisibility(soundSource.Scene, soundSource.Source, true);
            _cph.ObsSetSourceVisibility(soundLoopSource.Scene, soundLoopSource.Source, true);
            _cph.ObsMediaRestart(soundLoopSource.Scene, soundLoopSource.Source);
        }

        protected override void FinishSound()
        {
            _cph.ObsMediaStop(soundLoopSource.Scene, soundLoopSource.Source);
            _cph.ObsMediaRestart(soundSource.Scene, soundSource.Source);
            _cph.Wait(500);
            _cph.ObsSetSourceVisibility(soundLoopSource.Scene, soundLoopSource.Source, false);
            _cph.ObsSetSourceVisibility(soundSource.Scene, soundSource.Source, false);
        }

        protected override void PlaySound()
        {
        }
    }

    public class StarCounter : Counter
    {
        private SceneSourcePair soundSource = new SceneSourcePair(SOURCES.SOUND_SUB_INCREMENT.Scene, SOURCES.SOUND_SUB_INCREMENT.Source);
        public static List<SceneSourcePair> ACTIVE_SOURCES = new List<SceneSourcePair>
{
    SOURCES.SUB_COUNT_ACTIVE,
    SOURCES.SUB_COUNT_BACKGROUND_ACTIVE
};
        public static List<SceneSourcePair> INACTIVE_SOURCES = new List<SceneSourcePair>
{
    SOURCES.SUB_COUNT,
    SOURCES.SUB_COUNT_BACKGROUND
};
        public static List<SceneSourcePair> COUNTER_SOURCES = new List<SceneSourcePair>
{
    SOURCES.SUB_COUNT,
    SOURCES.SUB_COUNT_ACTIVE
};
        public StarCounter(IInlineInvokeProxy cph) : base("subscriberCount", "savedSubscriberCount", ACTIVE_SOURCES, INACTIVE_SOURCES, COUNTER_SOURCES, 500, cph, 2000)
        {
        }

        protected override void PrepareSound()
        {
            _cph.ObsSetSourceVisibility(soundSource.Scene, soundSource.Source, true);
        }

        protected override void FinishSound()
        {
            _cph.ObsSetSourceVisibility(soundSource.Scene, soundSource.Source, false);
        }

        protected override void PlaySound()
        {
            _cph.ObsMediaRestart(soundSource.Scene, soundSource.Source);
        }

        public override void GetValues()
        {
            if (!_cph.TryGetArg<int>(CurrentCountName, out int currentCountResult))
            {
                _cph.LogError($"TryGetArg for {CurrentCountName} failed.");
                return;
            }

            CurrentCount = currentCountResult;
        }
    }

    public class LoopableCounter : Counter
    {
        public SceneSourcePair SoundIncrement { get; set; }
        public SceneSourcePair SoundDecrement { get; set; }
        public int Mod { get; set; } = 0;
        public SceneSourcePair ItemSource { get; set; }

        public LoopableCounter(string currentCountName, string previousCountName, List<SceneSourcePair> activeSources, List<SceneSourcePair> inactiveSources, List<SceneSourcePair> counterSources, SceneSourcePair itemSource, SceneSourcePair soundIncrement, SceneSourcePair soundDecrement, int loopSpeed, int mod, IInlineInvokeProxy cph) : base(currentCountName, previousCountName, activeSources, inactiveSources, counterSources, loopSpeed, cph)
        {
            ItemSource = itemSource;
            SoundIncrement = soundIncrement;
            SoundDecrement = soundDecrement;
            Mod = mod;
        }

        public override void GetValues()
        {
            if (!_cph.TryGetArg<int>(CurrentCountName, out int currentCountResult))
            {
                _cph.LogError($"TryGetArg for {CurrentCountName} failed.");
                return;
            }

            CurrentCount = currentCountResult;
        }

        public List<int> GetAddSequence(int lastCount, int newCount)
        {
            var result = new List<int>();
            int addAmount = newCount - lastCount;
            for (int i = 1; i <= addAmount; i++)
            {
                int position = (lastCount + i) % Mod;
                result.Add(position);
            }

            return result;
        }

        public List<int> GetRemoveSequence(int lastCount, int newCount)
        {
            var result = new List<int>();
            int removeAmount = lastCount - newCount;
            for (int i = 0; i < removeAmount; i++)
            {
                int position = (lastCount - i) % Mod;
                result.Add(position);
            }

            return result;
        }

        protected virtual void PrintToItemSource(int position, bool visible)
        {
            _cph.LogInfo(ItemSource.Scene);
            _cph.LogInfo(ItemSource.Source);
            _cph.LogInfo(ItemSource.Source.Replace("#", position.ToString()));
            _cph.ObsSetSourceVisibility(ItemSource.Scene, ItemSource.Source.Replace("#", position.ToString()), visible);
        }

        protected override void PrintToCounterSource(int position, bool visible)
        {
            _cph.LogInfo(ItemSource.Scene);
            _cph.LogInfo(ItemSource.Source);
            _cph.LogInfo(ItemSource.Source.Replace("#", position.ToString()));
            _cph.ObsSetSourceVisibility(ItemSource.Scene, ItemSource.Source.Replace("#", position.ToString()), visible);
        }

        protected override void PrepareUpdate()
        {
            if (PreviousCount >= CurrentCount)
            {
                Refresh();
            }

            SetUpdatingState(true);
            PrepareSound();
        }

        public bool Refresh()
        {
            GetValues();
            int heartCount = CurrentCount % Mod == 0 ? Mod : CurrentCount % Mod;
            for (int i = 1; i <= Mod; i++)
            {
                int position = i % Mod;
                if (i > 0 && i <= heartCount)
                {
                    PrintToItemSource(position, true);
                }
                else
                {
                    PrintToItemSource(position, false);
                }
            }

            return true;
        }

        public bool Clear()
        {
            for (int i = 0; i < Mod; i++)
            {
                PrintToItemSource(i, false);
            }

            return true;
        }

        public bool Fill()
        {
            for (int i = 0; i < Mod; i++)
            {
                PrintToItemSource(i, true);
            }

            return true;
        }

        protected override void PrepareSound()
        {
            _cph.ObsSetSourceVisibility(SoundIncrement.Scene, SoundIncrement.Source, true);
            _cph.ObsSetSourceVisibility(SoundDecrement.Scene, SoundDecrement.Source, true);
        }

        protected override void FinishSound()
        {
            _cph.ObsSetSourceVisibility(SoundIncrement.Scene, SoundIncrement.Source, false);
            _cph.ObsSetSourceVisibility(SoundDecrement.Scene, SoundDecrement.Source, false);
        }

        protected override void Update()
        {
            var removeSequence = GetRemoveSequence(PreviousCount, CurrentCount);
            foreach (int position in removeSequence)
            {
                if (position == 0)
                {
                    // We've started a new row so we need to clear all the hearts of this current row. 
                    Fill();
                }

                PrintToItemSource(position, false);
                _cph.ObsMediaRestart(SoundDecrement.Scene, SoundDecrement.Source);
                _cph.Wait(LoopSpeed);
            }

            var addSequence = GetAddSequence(PreviousCount, CurrentCount);
            foreach (int position in addSequence)
            {
                if (position == 1)
                {
                    // We've started a new row so we need to clear all the hearts of this current row. 
                    Clear();
                }

                PrintToItemSource(position, true);
                _cph.ObsMediaRestart(SoundIncrement.Scene, SoundIncrement.Source);
                _cph.Wait(LoopSpeed);
            }
        }
    }
}
