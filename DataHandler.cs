using System;
using System.Collections.Generic;
using Streamer.bot.Plugin.Interface;

namespace Counters
{
    public class DataHandler
    {
        protected int _previousCount = 0;
        protected IInlineInvokeProxy _cph;

        protected int Count { get; set; }

        protected int PreviousCount
        {
            get
            {
                return _previousCount;
            }

            set
            {
                _previousCount = value;
                _cph.SetGlobalVar(PreviousCountArg, value, true);
            }
        }

        public int InitialCount { get; protected set; } = 0;

        /// <summary>
        /// Returns the converted count value.
        /// </summary>
        public int LastCountOutput
        {
            get { return Convert(_previousCount); }
        }

        /// <summary>
        /// Returns the converted count value.
        /// </summary>
        public int CountOutput {
            get { return Convert(Count); }
        }

        /// <summary>
        /// The name of the argument that gathers the new total of a count. 
        /// </summary>
        public string RefreshCountArg { get; set; }

        /// <summary>
        /// The name of the argument that gathers the previously saved count. 
        /// </summary>
        protected string PreviousCountArg { get; set; }
        public List<SceneSourcePair> InactiveSources { get; set; }
        public List<SceneSourcePair> ActiveSources { get; set; }
        public List<SceneSourcePair> CounterSources { get; set; }
        public Func<int, int> Convert { get; set; }

        public DataHandler(IInlineInvokeProxy cph, string refreshCountArg, string previousCountArg, List<SceneSourcePair> inactiveSources, List<SceneSourcePair> activeSources, List<SceneSourcePair> counterSources, Func<int, int> convert)
        {
            _cph = cph;
            RefreshCountArg = refreshCountArg;
            PreviousCountArg = previousCountArg;
            InactiveSources = inactiveSources ?? new List<SceneSourcePair>();
            ActiveSources = activeSources ?? new List<SceneSourcePair>();
            CounterSources = counterSources ?? new List<SceneSourcePair>();
            Convert = convert ?? ((number) => number);
            Initialize();
        }

        public virtual void Initialize()
        {
            PreviousCount = _cph.GetGlobalVar<int>(PreviousCountArg, true);
            InitialCount = PreviousCount;
            Count = PreviousCount;
        }

        public virtual void UpdateCount(string argName)
        {
            if (!_cph.TryGetArg<int>(argName, out int currentCountResult))
            {
                _cph.LogError($"TryGetArg for {argName} failed.");
                return;
            }

            PreviousCount = Count;
            Count = currentCountResult;
        }

        public virtual void IncrementCount(string argName)
        {
            if (!_cph.TryGetArg<int>(argName, out int currentCountResult))
            {
                _cph.LogError($"TryGetArg for {argName} failed.");
                return;
            }

            PreviousCount = Count;
            Count = Count + currentCountResult;
        }

        public virtual void UpdateCount(int newCount)
        {
            PreviousCount = Count;
            Count = newCount;
        }

        public virtual void IncrementCount(int amount)
        {
            PreviousCount = Count;
            Count = Count + amount;
        }
    }

    public class DataHandlerWithItem : DataHandler
    {
        public SceneSourcePair ItemSource { get; set; }
        public DataHandlerWithItem(IInlineInvokeProxy cph, string currentCountName, string previousCountName, List<SceneSourcePair> inactiveSources, List<SceneSourcePair> activeSources, List<SceneSourcePair> counterSources, SceneSourcePair itemSource, Func<int, int> convert, Func<int, int, int> updateCount) : base(cph, currentCountName, previousCountName, inactiveSources, activeSources, counterSources, convert)
        {
            ItemSource = itemSource;
        }
    }
}
