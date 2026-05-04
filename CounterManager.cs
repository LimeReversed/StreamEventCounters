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
            var tipDataHandler = new DataHandler(CPH, "tipAmount", "lastTips", TIP_INACTIVE_SOURCES, TIP_ACTIVE_SOURCES, TIP_COUNTER_SOURCES, (nr) => nr * 5);
            var tipIncrementSound = new SoundPlayerLoop(CPH, SOURCES.SOUND_RING_INCREMENT, SOURCES.SOUND_MULTI_RING_GET, 500);
            tipCounter = new Counter(CPH, tipDataHandler, 1, tipIncrementSound);
            
            var tipDataHandlerTest = new DataHandler(CPH, "message", "lastTips", TIP_INACTIVE_SOURCES, TIP_ACTIVE_SOURCES, TIP_COUNTER_SOURCES, (nr) => nr * 5);
            tipCounterTest = new Counter(CPH, tipDataHandlerTest, 1, tipIncrementSound);

            var bitDataHandler = new DataHandler(CPH, "bits", "lastBits", BIT_INACTIVE_SOURCES, BIT_ACTIVE_SOURCES, BIT_COUNTER_SOURCES, (nr) => nr / 2);
            var bitIncrementSound = new SoundPlayerLoop(CPH, SOURCES.SOUND_RING_INCREMENT, SOURCES.SOUND_MULTI_RING_GET, 500);
            bitCounter = new Counter(CPH, bitDataHandler, 1, bitIncrementSound);
            
            var bitDataHandlerTest = new DataHandler(CPH, "message", "lastBits", BIT_INACTIVE_SOURCES, BIT_ACTIVE_SOURCES, BIT_COUNTER_SOURCES, (nr) => nr / 2);
            bitCounterTest = new Counter(CPH, bitDataHandlerTest, 1, bitIncrementSound);

            var subDataHandler = new DataHandler(CPH, "subscriberCount", "lastSubscriberCount", SUB_INACTIVE_SOURCES, SUB_ACTIVE_SOURCES, SUB_COUNTER_SOURCES, null);
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
}
