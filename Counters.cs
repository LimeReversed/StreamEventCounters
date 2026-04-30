using System;
using System.Collections.Generic;
using Streamer.bot.Plugin.Interface;

namespace Counters
{
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

        public bool ResetToLast()
        {
            int heartCount = DataHandler.PreviousCount % Mod == 0 ? Mod : DataHandler.PreviousCount % Mod;
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
