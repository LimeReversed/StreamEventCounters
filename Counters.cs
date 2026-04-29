using System;
using System.Collections.Generic;
using Streamer.bot.Plugin.Interface;

namespace Counters
{
    public class CounterManager
    {
        public static List<SceneSourcePair> TIP_ACTIVE_SOURCES = new List<SceneSourcePair>
        {
            SOURCES.TIP_COUNT_ACTIVE,
            SOURCES.TIP_COUNT_BACKGROUND_ACTIVE
        };
        public static List<SceneSourcePair> TIP_INACTIVE_SOURCES = new List<SceneSourcePair>
        {
            SOURCES.TIP_COUNT,
            SOURCES.TIP_COUNT_BACKGROUND
        };
        public static List<SceneSourcePair> TIP_COUNTER_SOURCES = new List<SceneSourcePair>
        {
            SOURCES.TIP_COUNT,
            SOURCES.TIP_COUNT_ACTIVE
        };

        public static List<SceneSourcePair> BIT_ACTIVE_SOURCES = new List<SceneSourcePair>
        {
            SOURCES.BIT_COUNT_ACTIVE,
            SOURCES.BIT_COUNT_BACKGROUND_ACTIVE
        };
        public static List<SceneSourcePair> BIT_INACTIVE_SOURCES = new List<SceneSourcePair>
        {
            SOURCES.BIT_COUNT,
            SOURCES.BIT_COUNT_BACKGROUND
        };
        public static List<SceneSourcePair> BIT_COUNTER_SOURCES = new List<SceneSourcePair>
        {
            SOURCES.BIT_COUNT,
            SOURCES.BIT_COUNT_ACTIVE
        };

        public static List<SceneSourcePair> SUB_ACTIVE_SOURCES = new List<SceneSourcePair>
        {
            SOURCES.SUB_COUNT_ACTIVE,
            SOURCES.SUB_COUNT_BACKGROUND_ACTIVE
        };
        public static List<SceneSourcePair> SUB_INACTIVE_SOURCES = new List<SceneSourcePair>
        {
            SOURCES.SUB_COUNT,
            SOURCES.SUB_COUNT_BACKGROUND
        };
        public static List<SceneSourcePair> SUB_COUNTER_SOURCES = new List<SceneSourcePair>
        {
            SOURCES.SUB_COUNT,
            SOURCES.SUB_COUNT_ACTIVE
        };

        
        public static List<SceneSourcePair> FOLLOW_ACTIVE_SOURCES = new List<SceneSourcePair>();

        public static List<SceneSourcePair> FOLLOW_INACTIVE_SOURCES = new List<SceneSourcePair>();

        public static List<SceneSourcePair> FOLLOW_COUNTER_SOURCES = new List<SceneSourcePair>();



        public static Counter tipCounter = null;
        public static Counter tipCounterTest = null;
        public static Counter bitCounter = null;
        public static Counter bitCounterTest = null;
        public static Counter subCounter = null;
        public static CounterLoopable followCounter = null;

        public static void InitializeAll(IInlineInvokeProxy CPH)
        {
            var tipDataHandler = new DataHandler(CPH, "tipAmount", "lastTips", TIP_INACTIVE_SOURCES, TIP_ACTIVE_SOURCES, TIP_COUNTER_SOURCES, (nr) => nr * 5, (current, updated) => current + updated);
            var tipIncrementSound = new SoundPlayerLoop(CPH, SOURCES.SOUND_RING_INCREMENT, SOURCES.SOUND_MULTI_RING_GET, 500);
            tipCounter = new Counter(CPH, tipDataHandler, 1, tipIncrementSound);
            
            var tipDataHandlerTest = new DataHandler(CPH, "message", "lastTips", TIP_INACTIVE_SOURCES, TIP_ACTIVE_SOURCES, TIP_COUNTER_SOURCES, (nr) => nr * 5, (current, updated) => current + updated);
            tipCounterTest = new Counter(CPH, tipDataHandlerTest, 1, tipIncrementSound);

            var bitDataHandler = new DataHandler(CPH, "bits", "lastBits", BIT_INACTIVE_SOURCES, BIT_ACTIVE_SOURCES, BIT_COUNTER_SOURCES, (nr) => nr / 2, (current, updated) => current + updated);
            var bitIncrementSound = new SoundPlayerLoop(CPH, SOURCES.SOUND_RING_INCREMENT, SOURCES.SOUND_MULTI_RING_GET, 500);
            bitCounter = new Counter(CPH, bitDataHandler, 1, bitIncrementSound);
            
            var bitDataHandlerTest = new DataHandler(CPH, "message", "lastBits", BIT_INACTIVE_SOURCES, BIT_ACTIVE_SOURCES, BIT_COUNTER_SOURCES, (nr) => nr / 2, (current, updated) => current + updated);
            bitCounterTest = new Counter(CPH, bitDataHandlerTest, 1, bitIncrementSound);

            var subDataHandler = new DataHandler(CPH, "subscriberCount", "lastSubscriberCount", SUB_INACTIVE_SOURCES, SUB_ACTIVE_SOURCES, SUB_COUNTER_SOURCES, null, null);
            var subIncrementSound = new SoundPlayerBasic(CPH, SOURCES.SOUND_SUB_INCREMENT, 2000);
            subCounter = new Counter(CPH, subDataHandler, 500, subIncrementSound);

            var followDataHandler = new DataHandlerWithItem(CPH, "followerCount", "lastFollowerCount", null, null, null, SOURCES.HEART_FULL, null, null);
            var followerIncrementSound = new SoundPlayerBasic(CPH, SOURCES.SOUND_FOLLOWER_INCREMENT, 400);
            var followerDecrementSound = new SoundPlayerBasic(CPH, SOURCES.SOUND_FOLLOWER_DECREMENT, 400);
            followCounter = new CounterLoopable(CPH, followDataHandler, 400, followerIncrementSound, followerDecrementSound, 10);
        }
    }

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

    public abstract class SoundPlayer
    {
        protected IInlineInvokeProxy _cph = null;
        public SceneSourcePair Source { get; set; } = null;
        public int SourceLength { get; set; } = 0;
        public SoundPlayer(IInlineInvokeProxy cph, SceneSourcePair source, int sourceLength)
        {
            Source = source;
            SourceLength = sourceLength;
            _cph = cph;
        }
        public abstract void Prepare();
        public abstract void Play();
        public abstract void Finish();
    }

    public class SoundPlayerBasic : SoundPlayer
    {
        public SoundPlayerBasic(IInlineInvokeProxy cph, SceneSourcePair source, int sourceLength) : base(cph, source, sourceLength)
        {
        }

        public override void Prepare()
        {
            _cph.ObsSetSourceVisibility(Source.Scene, Source.Source, true);
        }
        public override void Play()
        {
            _cph.ObsMediaRestart(Source.Scene, Source.Source);
        }

        public override void Finish()
        {
            _cph.ObsSetSourceVisibility(Source.Scene, Source.Source, false);
        }
    }

    public class SoundPlayerLoop : SoundPlayer
    {
        public SceneSourcePair LoopSource { get; set; } = null;
        public SoundPlayerLoop(IInlineInvokeProxy cph, SceneSourcePair source, SceneSourcePair loopSource, int sourceLength) : base(cph, source, sourceLength)
        {
            LoopSource = loopSource;
        }

        public override void Prepare()
        {
            _cph.ObsSetSourceVisibility(Source.Scene, Source.Source, true);
            _cph.ObsSetSourceVisibility(LoopSource.Scene, LoopSource.Source, true);
            _cph.ObsMediaRestart(LoopSource.Scene, LoopSource.Source);
        }
        public override void Play()
        {
        }

        public override void Finish()
        {
            _cph.ObsMediaStop(LoopSource.Scene, LoopSource.Source);
            _cph.ObsMediaRestart(Source.Scene, Source.Source);
            _cph.Wait(SourceLength);
            _cph.ObsSetSourceVisibility(LoopSource.Scene, LoopSource.Source, false);
            _cph.ObsSetSourceVisibility(Source.Scene, Source.Source, false);
        }
    }

    public class DataHandler
    {
        private int _previousCount = 0;
        private int _currentCount = 0;
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
        public int CurrentCount {
            get { return Convert(_currentCount); }
            set { _currentCount = value; }
        }

        public string CurrentCountName { get; set; }
        public string PreviousCountName { get; set; }
        public List<SceneSourcePair> InactiveSources { get; set; }
        public List<SceneSourcePair> ActiveSources { get; set; }
        public List<SceneSourcePair> CounterSources { get; set; }
        public Func<int, int> Convert { get; set; }
        public Func<int, int, int> UpdateCount { get; set; }

        public DataHandler(IInlineInvokeProxy cph, string currentCountName, string previousCountName, List<SceneSourcePair> inactiveSources, List<SceneSourcePair> activeSources, List<SceneSourcePair> counterSources, Func<int, int> convert, Func<int, int, int> updateCount)
        {
            _cph = cph;
            CurrentCountName = currentCountName;
            PreviousCountName = previousCountName;
            InactiveSources = inactiveSources ?? new List<SceneSourcePair>();
            ActiveSources = activeSources ?? new List<SceneSourcePair>();
            CounterSources = counterSources ?? new List<SceneSourcePair>();
            Convert = convert ?? ((number) => number);
            UpdateCount = updateCount ?? ((current, updated) => updated);
            Initialize();
        }

        public virtual void Initialize()
        {
            // Don't forget to actually get the values before running this code. 
            UpdateData();
            InitialCount = PreviousCount; // Expected to be already converted.
        }

        public virtual void UpdateData()
        {
            if (!_cph.TryGetArg<int>(CurrentCountName, out int currentCountResult))
            {
                _cph.LogError($"TryGetArg for {CurrentCountName} failed.");
                return;
            }

            CurrentCount = UpdateCount(_currentCount, currentCountResult);
        }
    }

    public class DataHandlerWithItem : DataHandler
    {
        public SceneSourcePair ItemSource { get; set; }
        public DataHandlerWithItem(IInlineInvokeProxy cph, string currentCountName, string previousCountName, List<SceneSourcePair> inactiveSources, List<SceneSourcePair> activeSources, List<SceneSourcePair> counterSources, SceneSourcePair itemSource, Func<int, int> convert, Func<int, int, int> updateCount) : base(cph, currentCountName, previousCountName, inactiveSources, activeSources, counterSources, convert, updateCount)
        {
            ItemSource = itemSource;
        }
    }

    // *************************************************************************************************************************************
    // ********************************************* COUNTER *****************************************************************************
    public class Counter
    {
        protected IInlineInvokeProxy _cph;
        public int LoopSpeed { get; set; } = 0;
        public SoundPlayer IncrementSound { get; set; } = null;
        public DataHandler DataHandler { get; set; } = null;

        public Counter(IInlineInvokeProxy cph, DataHandler dataHandler, int loopSpeed, SoundPlayer incrementSound)
        {
            _cph = cph;
            DataHandler = dataHandler;
            LoopSpeed = loopSpeed;
            IncrementSound = incrementSound;
        }

        protected virtual void WriteToSources(List<SceneSourcePair> sources, int number)
        {
            foreach (SceneSourcePair pair in sources)
            {
                _cph.ObsSetGdiText(pair.Scene, pair.Source, number.ToString());
            }
        }

        protected virtual void WriteToSource(SceneSourcePair source, bool visible, int position = -1)
        {
            return;
        }

        protected virtual void SetWriteState(bool active)
        {
            foreach (SceneSourcePair pair in DataHandler.ActiveSources)
            {
                _cph.ObsSetSourceVisibility(pair.Scene, pair.Source, active);
            }

            foreach (SceneSourcePair pair in DataHandler.InactiveSources)
            {
                _cph.ObsSetSourceVisibility(pair.Scene, pair.Source, !active);
            }
        }

        protected virtual void PrepareWrite()
        {
            if (DataHandler.PreviousCount >= DataHandler.CurrentCount)
            {
                WriteToSources(DataHandler.CounterSources, DataHandler.CurrentCount);
            }

            SetWriteState(true);
            IncrementSound.Prepare();
        }

        protected virtual void ApplyWrite()
        {
            for (int i = DataHandler.PreviousCount; i <= DataHandler.CurrentCount; i++)
            {
                WriteToSources(DataHandler.CounterSources, i);
                IncrementSound.Play();
                bool isLastIteration = i >= DataHandler.CurrentCount;
                _cph.Wait(isLastIteration ? IncrementSound.SourceLength : LoopSpeed);
            }
        }

        protected virtual void FinishWrite()
        {
            SetWriteState(false);
            DataHandler.PreviousCount = DataHandler.CurrentCount;
            IncrementSound.Finish();
        }

        public virtual void WriteToOBS()
        {
            PrepareWrite();
            ApplyWrite();
            FinishWrite();
        }

        public void Execute()
        {
            DataHandler.UpdateData();
            WriteToOBS();
        }
    }

    public class CounterLoopable : Counter
    {
        public SoundPlayer DecrementSound { get; set; }
        public int Mod { get; set; } = 0;

        public CounterLoopable(IInlineInvokeProxy cph, DataHandlerWithItem dataHandler, int loopSpeed, SoundPlayer incrementSound, SoundPlayer decrementSound, int mod) : base(cph, dataHandler, loopSpeed, incrementSound)
        {
            DecrementSound = decrementSound;
            Mod = mod;
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

        protected override void WriteToSource(SceneSourcePair source, bool visible, int position = 0)
        {
            _cph.ObsSetSourceVisibility(source.Scene, source.Source.Replace("#", position.ToString()), visible);
        }

        public bool Refresh()
        {
            DataHandler.UpdateData();
            int heartCount = DataHandler.CurrentCount % Mod == 0 ? Mod : DataHandler.CurrentCount % Mod;
            for (int i = 1; i <= Mod; i++)
            {
                int position = i % Mod;
                if (i > 0 && i <= heartCount)
                {
                    WriteToSource((DataHandler as DataHandlerWithItem).ItemSource, true, position);
                }
                else
                {
                    WriteToSource((DataHandler as DataHandlerWithItem).ItemSource, false, position);
                }
            }

            return true;
        }

        public bool Clear()
        {
            for (int i = 0; i < Mod; i++)
            {
                WriteToSource((DataHandler as DataHandlerWithItem).ItemSource, false, i);
            }

            return true;
        }

        public bool Fill()
        {
            for (int i = 0; i < Mod; i++)
            {
                WriteToSource((DataHandler as DataHandlerWithItem).ItemSource, true, i);
            }

            return true;
        }

        protected override void PrepareWrite()
        {
            if (DataHandler.PreviousCount >= DataHandler.CurrentCount)
            {
                Refresh();
            }

            SetWriteState(true);
            IncrementSound.Prepare();
            DecrementSound.Prepare();
        }

        protected override void ApplyWrite()
        {
            var removeSequence = GetRemoveSequence(DataHandler.PreviousCount, DataHandler.CurrentCount);

            foreach (int position in removeSequence)
            {
                if (position == 0)
                {
                    // We've started a new row so we need to clear all the hearts of this current row. 
                    Fill();
                }

                bool last = position == removeSequence[removeSequence.Count - 1];
                WriteToSource((DataHandler as DataHandlerWithItem).ItemSource, false, position);
                DecrementSound.Play();
                _cph.Wait(last ? DecrementSound.SourceLength : LoopSpeed);
            }

            var addSequence = GetAddSequence(DataHandler.PreviousCount, DataHandler.CurrentCount);
            foreach (int position in addSequence)
            {
                if (position == 1)
                {
                    // We've started a new row so we need to clear all the hearts of this current row. 
                    Clear();
                }

                bool last = position == addSequence[addSequence.Count - 1];
                WriteToSource((DataHandler as DataHandlerWithItem).ItemSource, true, position);
                IncrementSound.Play();
                _cph.Wait(last ? IncrementSound.SourceLength : LoopSpeed);
            }
        }

        protected override void FinishWrite()
        {
            SetWriteState(false);
            DataHandler.PreviousCount = DataHandler.CurrentCount;
            IncrementSound.Finish();
            DecrementSound.Finish();
        }
    }
}
