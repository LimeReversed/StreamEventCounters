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

    // ******************************************* Helper classes
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
        public SoundPlayer(SceneSourcePair source, int sourceLength, IInlineInvokeProxy cph)
        {
            Source = source;
            SourceLength = sourceLength;
            _cph = cph;
        }
        public abstract void PrepareSound();
        public abstract void FinishSound();
        public abstract void PlaySound();
    }

    public class SoundPlayerBasic : SoundPlayer
    {
        public SoundPlayerBasic(SceneSourcePair source, int sourceLength, IInlineInvokeProxy cph) : base(source, sourceLength, cph)
        {
        }

        public override void PrepareSound()
        {
            _cph.ObsSetSourceVisibility(Source.Scene, Source.Source, true);
        }
        public override void PlaySound()
        {
            _cph.ObsMediaRestart(Source.Scene, Source.Source);
        }

        public override void FinishSound()
        {
            _cph.ObsSetSourceVisibility(Source.Scene, Source.Source, false);
        }
    }

    public class SoundPlayerLoop : SoundPlayer
    {
        public SceneSourcePair LoopSource { get; set; } = null;
        public SoundPlayerLoop(SceneSourcePair source, SceneSourcePair loopSource, int sourceLength, IInlineInvokeProxy cph) : base(source, sourceLength, cph)
        {
            LoopSource = loopSource;
        }

        public override void PrepareSound()
        {
            _cph.ObsSetSourceVisibility(Source.Scene, Source.Source, true);
            _cph.ObsSetSourceVisibility(LoopSource.Scene, LoopSource.Source, true);
            _cph.ObsMediaRestart(LoopSource.Scene, LoopSource.Source);
        }
        public override void PlaySound()
        {
        }

        public override void FinishSound()
        {
            _cph.ObsMediaStop(LoopSource.Scene, LoopSource.Source);
            _cph.ObsMediaRestart(Source.Scene, Source.Source);
            _cph.Wait(SourceLength);
            _cph.ObsSetSourceVisibility(LoopSource.Scene, LoopSource.Source, false);
            _cph.ObsSetSourceVisibility(Source.Scene, Source.Source, false);
        }
    }

    public class  DataHandler
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
        public List<SceneSourcePair> InactiveSources { get; set; } = new List<SceneSourcePair>();
        public List<SceneSourcePair> ActiveSources { get; set; } = new List<SceneSourcePair>();
        public List<SceneSourcePair> CounterSources { get; set; } = new List<SceneSourcePair>();
        public Func<int, int> Convert { get; set; }

        public DataHandler(IInlineInvokeProxy cph, string currentCountName, string previousCountName, List<SceneSourcePair> inactiveSources, List<SceneSourcePair> activeSources, List<SceneSourcePair> counterSources, Func<int, int> convert)
        {
            _cph = cph;
            CurrentCountName = currentCountName;
            PreviousCountName = previousCountName;
            InactiveSources = inactiveSources;
            ActiveSources = activeSources;
            CounterSources = counterSources;
            Convert = convert ?? ((number) => number);
        }

        public virtual void Initialize()
        {
            // Don't forget to actually get the values before running this code. 
            if (!_cph.TryGetArg<int>(CurrentCountName, out int currentCountResult))
            {
                _cph.LogError($"TryGetArg for {CurrentCountName} failed.");
                return;
            }

            InitialCount = PreviousCount;
            CurrentCount = currentCountResult;
        }

        public virtual void UpdateData()
        {
            if (!_cph.TryGetArg<int>(CurrentCountName, out int currentCountResult))
            {
                _cph.LogError($"TryGetArg for {CurrentCountName} failed.");
                return;
            }

            CurrentCount = currentCountResult;
        }
    }

    public class DataHandlerIncremental : DataHandler
    {
        public DataHandlerIncremental(IInlineInvokeProxy cph, string currentCountName, string previousCountName, List<SceneSourcePair> inactiveSources, List<SceneSourcePair> activeSources, List<SceneSourcePair> counterSources, Func<int, int> convert) : base(cph, currentCountName, previousCountName, inactiveSources, activeSources, counterSources, convert)
        {
        }
        public override void UpdateData()
        {
            if (!_cph.TryGetArg<int>(CurrentCountName, out int currentCountResult))
            {
                _cph.LogError($"TryGetArg for {CurrentCountName} failed.");
                return;
            }
            CurrentCount += currentCountResult;
        }
    }

    // *************************************************************************************************************************************
    // ********************************************* COUNTER *****************************************************************************
    public class Counter
    {
        protected IInlineInvokeProxy _cph;
        public int LoopSpeed { get; set; } = 0;
        public SoundPlayer SoundPlayer { get; set; } = null;
        public DataHandler DataHandler { get; set; } = null;

        public Counter(IInlineInvokeProxy cph, DataHandler dataHandler, int loopSpeed, SoundPlayer soundPlayer)
        {
            _cph = cph;
            DataHandler = dataHandler;
            LoopSpeed = loopSpeed;
            SoundPlayer = soundPlayer;
        }        

        protected virtual void WriteToSources(List<SceneSourcePair> sources, int number)
        {
            number = DataHandler.Convert(number);
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
            SoundPlayer.PrepareSound();
        }

        protected virtual void ApplyWrite()
        {
            for (int i = DataHandler.Convert(DataHandler.PreviousCount); i <= DataHandler.Convert(DataHandler.CurrentCount); i++)
            {
                WriteToSources(DataHandler.CounterSources, i);
                SoundPlayer.PlaySound();
                bool isLastIteration = i >= DataHandler.Convert(DataHandler.CurrentCount);
                _cph.Wait(isLastIteration ? SoundPlayer.SourceLength : LoopSpeed);
            }
        }

        protected virtual void FinishWrite()
        {
            SetWriteState(false);
            DataHandler.PreviousCount = DataHandler.CurrentCount;
            SoundPlayer.FinishSound();
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

        protected override void WriteToSources(int position, bool visible)
        {
            _cph.ObsSetSourceVisibility(ItemSource.Scene, ItemSource.Source.Replace("#", position.ToString()), visible);
        }

        protected override void PrepareWrite()
        {
            if (PreviousCount >= CurrentCount)
            {
                Refresh();
            }

            SetWriteState(true);
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

        protected override void ApplyWrite()
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
