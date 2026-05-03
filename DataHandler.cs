using System;
using System.Collections.Generic;
using Streamer.bot.Plugin.Interface;

namespace Counters
{
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
                return _cph.GetGlobalVar<int>(PreviousCountArg, true);

            }

            set
            {
                _previousCount = value;
                _cph.SetGlobalVar(PreviousCountArg, value, true);
            }
        }
        public int CurrentCount {
            get { return Convert(_currentCount); }
            set { _currentCount = value; }
        }

        public string RefreshCountArg { get; set; }
        public string PreviousCountArg { get; set; }
        public List<SceneSourcePair> InactiveSources { get; set; }
        public List<SceneSourcePair> ActiveSources { get; set; }
        public List<SceneSourcePair> CounterSources { get; set; }
        public Func<int, int> Convert { get; set; }
        public Func<int, int, int> UpdateCount { get; set; }

        public DataHandler(IInlineInvokeProxy cph, string refreshCountArg, string previousCountArg, List<SceneSourcePair> inactiveSources, List<SceneSourcePair> activeSources, List<SceneSourcePair> counterSources, Func<int, int> convert, Func<int, int, int> updateCount)
        {
            _cph = cph;
            RefreshCountArg = refreshCountArg;
            PreviousCountArg = previousCountArg;
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
            UpdateData(PreviousCountArg);
            InitialCount = PreviousCount;
        }

        public virtual void UpdateData(string argName)
        {
            if (!_cph.TryGetArg<int>(argName, out int currentCountResult))
            {
                _cph.LogError($"TryGetArg for {argName} failed.");
                return;
            }

            CurrentCount = UpdateCount(_currentCount, currentCountResult);
        }

        public virtual void UpdateData(int newCount)
        {
            CurrentCount = UpdateCount(_currentCount, newCount);
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
}
