using Streamer.bot.Plugin.Interface;

namespace Counters
{
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
}
