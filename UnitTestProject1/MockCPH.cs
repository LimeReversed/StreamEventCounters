using Streamer.bot.Common.Events;
using Streamer.bot.Plugin.Interface;
using Streamer.bot.Plugin.Interface.Enums;
using Streamer.bot.Plugin.Interface.Model;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Security.Policy;
using Twitch.Common.Models.Api;

namespace Test_Chamber
{
    public class Source: IEquatable<Source>
    {
        public virtual string Value { get; set; }
        public bool visible = false;

        public Source(string value, bool visible)
        {
            Value = value;
            this.visible = visible;
        }

        public bool Equals(Source other)
        {
            if (other == null)
                return false;
            if (other.Value == null && Value == null && other.visible == visible)
                return true;
            return visible == other.visible && Value.Equals(other.Value);
        }
    }

    public class MockCPH : IInlineInvokeProxy
    {
        public readonly Dictionary<string, object> _args = new Dictionary<string, object>();
        public readonly Dictionary<string, object> _globalVars = new Dictionary<string, object>();
        public readonly Dictionary<string, Source> _sources = new Dictionary<string, Source>();
        public static string subscriberCount = "subscriberCount";
        public static string lastSubscriberCount = "lastSubscriberCount";
        public static string followerCount = "followerCount";
        public static string lastFollowerCount = "lastFollowerCount";
        public static string lastTips = "lastTips";
        public static string lastBits = "lastBits";
        public static string bits = "bits";
        public static string tipAmount = "tipAmount";


        public void SetArg(string name, object value)
        {
            _args[name] = value;
        }

        public void ClearArgs()
        {
            _args.Clear();
        }

        // Required for interface.
        public void SetGlobalVar(string varName, object value, bool persisted = true)
        {
            _globalVars[varName] = value;
            Console.WriteLine($"[SetGlobalVar] {varName} = {value}");
        }

        public bool TryGetArg<T>(string argName, out T value)
        {
            if (_args.TryGetValue(argName, out var rawValue))
            {
                if (rawValue is T typedValue)
                {
                    value = typedValue;
                    Console.WriteLine($"[TryGetArg] {argName} = {value}");
                    return true;
                }

                try
                {
                    value = (T)Convert.ChangeType(rawValue, typeof(T));
                    Console.WriteLine($"[TryGetArg] {argName} = {value}");
                    return true;
                }
                catch
                {
                    // fall through
                }
            }

            Console.WriteLine($"[TryGetArg FAILED] {argName}");
            value = default;
            return false;
        }

        public void ObsSetSourceVisibility(string scene, string source, bool visible, int connection = 0)
        {
            Console.WriteLine($"[OBS Visibility] Scene='{scene}' Source='{source}' Visible={visible}");
            if (_sources.ContainsKey(source))
            {
                _sources[source].visible = visible;
            }
            else
            {
                _sources[source] = new Source(null, visible);
            }

        }

        public void ObsSetGdiText(string scene, string source, string text, int connection = 0)
        {
            Console.WriteLine($"[OBS Text] Scene='{scene}' Source='{source}' Text='{text}'");
            if (_sources.ContainsKey(source))
            {
                _sources[source].Value = text;
            }
            else
            {
                _sources[source] = new Source(text, true);
            }
        }

        public void ObsMediaRestart(string scene, string source, int connection = 0)
        {
            Console.WriteLine($"[OBS Media Restart] Scene='{scene}' Source='{source}'");
        }

        public void ObsMediaStop(string scene, string source, int connection = 0)
        {
            Console.WriteLine($"[OBS Media Stop] Scene='{scene}' Source='{source}'");
        }

        public void Wait(int milliseconds)
        {
            Console.WriteLine($"[Wait] {milliseconds}ms");
            // Optional:
            // System.Threading.Thread.Sleep(milliseconds);
        }

        public void LogInfo(string logLine)
        {
            Console.WriteLine($"[INFO] {logLine}");
        }

        public void LogError(string logLine)
        {
            Console.WriteLine($"[ERROR] {logLine}");
        }

        public T GetGlobalVar<T>(string varName, bool persisted = true)
        {
            if (_globalVars.TryGetValue(varName, out var rawValue))
            {
                if (rawValue is T typedValue)
                    return typedValue;

                try
                {
                    return (T)Convert.ChangeType(rawValue, typeof(T));
                }
                catch
                {
                    // fall through
                }
            }

            return default;
        }

        public string TwitchClientId => throw new NotImplementedException();

        public string TwitchOAuthToken => throw new NotImplementedException();

        public bool ActionExists(string actionName)
        {
            throw new NotImplementedException();
        }

        public bool AddGroup(string groupName)
        {
            throw new NotImplementedException();
        }

        public int AddQuoteForKick(string userId, string quote, bool captureGame = false)
        {
            throw new NotImplementedException();
        }

        public int AddQuoteForTrovo(string userId, string quote, bool captureGame = false)
        {
            throw new NotImplementedException();
        }

        public int AddQuoteForTwitch(string userId, string quote, bool captureGame = false)
        {
            throw new NotImplementedException();
        }

        public int AddQuoteForYouTube(string userId, string quote)
        {
            throw new NotImplementedException();
        }

        public void AddToCredits(string section, string value, bool json = true)
        {
            throw new NotImplementedException();
        }

        public bool AddUserIdToGroup(string userId, Platform platform, string groupName)
        {
            throw new NotImplementedException();
        }

        public bool AddUserToGroup(string userName, Platform platform, string groupName)
        {
            throw new NotImplementedException();
        }

        public int Between(int min, int max)
        {
            throw new NotImplementedException();
        }

        public int BroadcastUdp(int port, object data)
        {
            throw new NotImplementedException();
        }

        public void ClearNonPersistedGlobals()
        {
            throw new NotImplementedException();
        }

        public void ClearNonPersistedUserGlobals()
        {
            throw new NotImplementedException();
        }

        public bool ClearUsersFromGroup(string groupName)
        {
            throw new NotImplementedException();
        }

        public void CommandAddToAllUserCooldowns(string id, int seconds)
        {
            throw new NotImplementedException();
        }

        public void CommandAddToGlobalCooldown(string id, int seconds)
        {
            throw new NotImplementedException();
        }

        public void CommandAddToUserCooldown(string id, int userId, int seconds)
        {
            throw new NotImplementedException();
        }

        public void CommandAddToUserCooldown(string id, string userId, Platform platform, int seconds)
        {
            throw new NotImplementedException();
        }

        public int CommandGetCounter(string commandId)
        {
            throw new NotImplementedException();
        }

        public CommandCounter CommandGetUserCounter(string userLogin, Platform platform, string commandId)
        {
            throw new NotImplementedException();
        }

        public CommandCounter CommandGetUserCounterById(string userId, Platform platform, string commandId)
        {
            throw new NotImplementedException();
        }

        public void CommandRemoveAllUserCooldowns(string id)
        {
            throw new NotImplementedException();
        }

        public void CommandRemoveGlobalCooldown(string id)
        {
            throw new NotImplementedException();
        }

        public void CommandRemoveUserCooldown(string id, int userId)
        {
            throw new NotImplementedException();
        }

        public void CommandRemoveUserCooldown(string id, string userId, Platform platform)
        {
            throw new NotImplementedException();
        }

        public void CommandResetAllUserCooldowns(string id)
        {
            throw new NotImplementedException();
        }

        public void CommandResetAllUserCounters(string commandId)
        {
            throw new NotImplementedException();
        }

        public void CommandResetCounter(string commandId)
        {
            throw new NotImplementedException();
        }

        public void CommandResetGlobalCooldown(string id)
        {
            throw new NotImplementedException();
        }

        public void CommandResetUserCooldown(string id, int userId)
        {
            throw new NotImplementedException();
        }

        public void CommandResetUserCooldown(string id, string userId, Platform platform)
        {
            throw new NotImplementedException();
        }

        public void CommandResetUserCounter(string commandId, string userId, Platform platform)
        {
            throw new NotImplementedException();
        }

        public void CommandResetUsersCounters(string userId, Platform platform, bool persisted)
        {
            throw new NotImplementedException();
        }

        public void CommandSetGlobalCooldownDuration(string id, int seconds)
        {
            throw new NotImplementedException();
        }

        public void CommandSetUserCooldownDuration(string id, int seconds)
        {
            throw new NotImplementedException();
        }

        public ClipData CreateClip(string title = null, int duration = 30)
        {
            throw new NotImplementedException();
        }

        public StreamMarker CreateStreamMarker(string description)
        {
            throw new NotImplementedException();
        }

        public void DeckItemSetBackground(string deckId, string itemId, string color = null, string url = null, string fileId = null, int? state = null)
        {
            throw new NotImplementedException();
        }

        public void DeckItemSetIcon(string deckId, string itemId, string color = null, string name = null, string url = null, string fileId = null, int? state = null)
        {
            throw new NotImplementedException();
        }

        public void DeckItemSetState(string deckId, string itemId, int state)
        {
            throw new NotImplementedException();
        }

        public void DeckItemSetTitle(string deckId, string itemId, string title, string color = null, int? state = null)
        {
            throw new NotImplementedException();
        }

        public void DeckItemSetValue(string deckId, string itemId, int value, int? state = null)
        {
            throw new NotImplementedException();
        }

        public void DeckItemToggleState(string deckId, string itemId)
        {
            throw new NotImplementedException();
        }

        public void DeckNotify(string deckId, string title, string description, string color = null, string soundId = null)
        {
            throw new NotImplementedException();
        }

        public void DeckPageNext(string deckId)
        {
            throw new NotImplementedException();
        }

        public void DeckPagePrev(string deckId)
        {
            throw new NotImplementedException();
        }

        public void DeckPageSet(string deckId, int page)
        {
            throw new NotImplementedException();
        }

        public bool DeleteGroup(string groupName)
        {
            throw new NotImplementedException();
        }

        public bool DeleteQuote(int quoteId)
        {
            throw new NotImplementedException();
        }

        public void DisableAction(string actionName)
        {
            throw new NotImplementedException();
        }

        public void DisableActionById(string actionId)
        {
            throw new NotImplementedException();
        }

        public void DisableCommand(string id)
        {
            throw new NotImplementedException();
        }

        public void DisableReward(string rewardId)
        {
            throw new NotImplementedException();
        }

        public void DisableTimer(string timerName)
        {
            throw new NotImplementedException();
        }

        public void DisableTimerById(string timerId)
        {
            throw new NotImplementedException();
        }

        public string DiscordPostTextToWebhook(string webhookUrl, string content, string username = null, string avatarUrl = null, bool textToSpeech = false)
        {
            throw new NotImplementedException();
        }

        public void EnableAction(string actionName)
        {
            throw new NotImplementedException();
        }

        public void EnableActionById(string actionId)
        {
            throw new NotImplementedException();
        }

        public void EnableCommand(string id)
        {
            throw new NotImplementedException();
        }

        public void EnableReward(string rewardId)
        {
            throw new NotImplementedException();
        }

        public void EnableTimer(string timerName)
        {
            throw new NotImplementedException();
        }

        public void EnableTimerById(string timerId)
        {
            throw new NotImplementedException();
        }

        public string EscapeString(string text)
        {
            throw new NotImplementedException();
        }

        public bool ExecuteMethod(string executeCode, string methodName)
        {
            throw new NotImplementedException();
        }

        public List<ActionData> GetActions()
        {
            throw new NotImplementedException();
        }

        public List<ClipData> GetAllClips(bool? isFeatured = null)
        {
            throw new NotImplementedException();
        }

        public List<Cheermote> GetCheermotes()
        {
            throw new NotImplementedException();
        }

        public List<ClipData> GetClips(int count, bool? isFeatured = null)
        {
            throw new NotImplementedException();
        }

        public List<ClipData> GetClipsForGame(int gameId, bool? isFeatured = null)
        {
            throw new NotImplementedException();
        }

        public List<ClipData> GetClipsForGame(int gameId, int count, bool? isFeatured = null)
        {
            throw new NotImplementedException();
        }

        public List<ClipData> GetClipsForGame(int gameId, DateTime start, DateTime end, bool? isFeatured = null)
        {
            throw new NotImplementedException();
        }

        public List<ClipData> GetClipsForGame(int gameId, DateTime start, DateTime end, int count, bool? isFeatured = null)
        {
            throw new NotImplementedException();
        }

        public List<ClipData> GetClipsForGame(int gameId, TimeSpan duration, bool? isFeatured = null)
        {
            throw new NotImplementedException();
        }

        public List<ClipData> GetClipsForGame(int gameId, TimeSpan duration, int count, bool? isFeatured = null)
        {
            throw new NotImplementedException();
        }

        public List<ClipData> GetClipsForUser(int userId)
        {
            throw new NotImplementedException();
        }

        public List<ClipData> GetClipsForUser(int userId, int count)
        {
            throw new NotImplementedException();
        }

        public List<ClipData> GetClipsForUser(int userId, DateTime start, DateTime end)
        {
            throw new NotImplementedException();
        }

        public List<ClipData> GetClipsForUser(int userId, DateTime start, DateTime end, int count)
        {
            throw new NotImplementedException();
        }

        public List<ClipData> GetClipsForUser(int userId, TimeSpan duration)
        {
            throw new NotImplementedException();
        }

        public List<ClipData> GetClipsForUser(int userId, TimeSpan duration, int count)
        {
            throw new NotImplementedException();
        }

        public List<ClipData> GetClipsForUser(string username, bool? isFeatured = null)
        {
            throw new NotImplementedException();
        }

        public List<ClipData> GetClipsForUser(string userName, int count, bool? isFeatured = null)
        {
            throw new NotImplementedException();
        }

        public List<ClipData> GetClipsForUser(string username, DateTime start, DateTime end, bool? isFeatured = null)
        {
            throw new NotImplementedException();
        }

        public List<ClipData> GetClipsForUser(string username, DateTime start, DateTime end, int count, bool? isFeatured = null)
        {
            throw new NotImplementedException();
        }

        public List<ClipData> GetClipsForUser(string username, TimeSpan duration, bool? isFeatured = null)
        {
            throw new NotImplementedException();
        }

        public List<ClipData> GetClipsForUser(string username, TimeSpan duration, int count, bool? isFeatured = null)
        {
            throw new NotImplementedException();
        }

        public List<ClipData> GetClipsForUserById(string userId, bool? isFeatured = null)
        {
            throw new NotImplementedException();
        }

        public List<ClipData> GetClipsForUserById(string userId, int count, bool? isFeatured = null)
        {
            throw new NotImplementedException();
        }

        public List<ClipData> GetClipsForUserById(string userId, DateTime start, DateTime end, bool? isFeatured = null)
        {
            throw new NotImplementedException();
        }

        public List<ClipData> GetClipsForUserById(string userId, DateTime start, DateTime end, int count, bool? isFeatured = null)
        {
            throw new NotImplementedException();
        }

        public List<ClipData> GetClipsForUserById(string userId, TimeSpan duration, bool? isFeatured = null)
        {
            throw new NotImplementedException();
        }

        public List<ClipData> GetClipsForUserById(string userId, TimeSpan duration, int count, bool? isFeatured = null)
        {
            throw new NotImplementedException();
        }

        public List<CommandData> GetCommands()
        {
            throw new NotImplementedException();
        }

        public EventType GetEventType()
        {
            throw new NotImplementedException();
        }

        public List<GlobalVariableValue> GetGlobalVarValues(bool persisted = true)
        {
            throw new NotImplementedException();
        }

        public List<string> GetGroups()
        {
            throw new NotImplementedException();
        }

        public List<UserVariableValue<T>> GetKickUsersVar<T>(string varName, bool persisted = true)
        {
            throw new NotImplementedException();
        }

        public T GetKickUserVar<T>(string userName, string varName, bool persisted = true)
        {
            throw new NotImplementedException();
        }

        public T GetKickUserVarById<T>(string userId, string varName, bool persisted = true)
        {
            throw new NotImplementedException();
        }

        public QuoteData GetQuote(int quoteId)
        {
            throw new NotImplementedException();
        }

        public int GetQuoteCount()
        {
            throw new NotImplementedException();
        }

        public EventSource GetSource()
        {
            throw new NotImplementedException();
        }

        public List<TeamInfo> GetTeamInfo(int userId)
        {
            throw new NotImplementedException();
        }

        public List<TeamInfo> GetTeamInfo(string username)
        {
            throw new NotImplementedException();
        }

        public List<TeamInfo> GetTeamInfoById(string userId)
        {
            throw new NotImplementedException();
        }

        public List<TeamInfo> GetTeamInfoByLogin(string userLogin)
        {
            throw new NotImplementedException();
        }

        public bool GetTimerState(string timerId)
        {
            throw new NotImplementedException();
        }

        public List<UserVariableValue<T>> GetTrovoUsersVar<T>(string varName, bool persisted = true)
        {
            throw new NotImplementedException();
        }

        public T GetTrovoUserVar<T>(string userName, string varName, bool persisted = true)
        {
            throw new NotImplementedException();
        }

        public T GetTrovoUserVarById<T>(string userId, string varName, bool persisted = true)
        {
            throw new NotImplementedException();
        }

        public List<UserVariableValue<T>> GetTwitchUsersVar<T>(string varName, bool persisted = true)
        {
            throw new NotImplementedException();
        }

        public T GetTwitchUserVar<T>(string userName, string varName, bool persisted = true)
        {
            throw new NotImplementedException();
        }

        public T GetTwitchUserVarById<T>(string userId, string varName, bool persisted = true)
        {
            throw new NotImplementedException();
        }

        public T GetUserVar<T>(string userName, string varName, bool persisted = true)
        {
            throw new NotImplementedException();
        }

        public string GetVersion()
        {
            throw new NotImplementedException();
        }

        public List<UserVariableValue<T>> GetYouTubeUsersVar<T>(string varName, bool persisted = true)
        {
            throw new NotImplementedException();
        }

        public T GetYouTubeUserVar<T>(string userName, string varName, bool persisted = true)
        {
            throw new NotImplementedException();
        }

        public T GetYouTubeUserVarById<T>(string userId, string varName, bool persisted = true)
        {
            throw new NotImplementedException();
        }

        public bool GroupExists(string groupName)
        {
            throw new NotImplementedException();
        }

        public void IncrementAllKickUsersVar(string varName, object value, bool persisted = true)
        {
            throw new NotImplementedException();
        }

        public void IncrementAllTrovoUsersVar(string varName, object value, bool persisted = true)
        {
            throw new NotImplementedException();
        }

        public void IncrementAllTwitchUsersVar(string varName, object value, bool persisted = true)
        {
            throw new NotImplementedException();
        }

        public void IncrementAllYouTubeUsersVar(string varName, object value, bool persisted = true)
        {
            throw new NotImplementedException();
        }

        public void IncrementKickUsersVarById(List<string> userIds, string varName, object value, bool persisted = true)
        {
            throw new NotImplementedException();
        }

        public void IncrementOrCreateKickUsersVarById(List<string> userIds, string varName, object value, bool persisted = true)
        {
            throw new NotImplementedException();
        }

        public void IncrementOrCreateTrovoUsersVarById(List<string> userIds, string varName, object value, bool persisted = true)
        {
            throw new NotImplementedException();
        }

        public void IncrementOrCreateTwitchUsersVarById(List<string> userIds, string varName, object value, bool persisted = true)
        {
            throw new NotImplementedException();
        }

        public void IncrementOrCreateYouTubeUsersVarById(List<string> userIds, string varName, object value, bool persisted = true)
        {
            throw new NotImplementedException();
        }

        public void IncrementTrovoUsersVarById(List<string> userIds, string varName, object value, bool persisted = true)
        {
            throw new NotImplementedException();
        }

        public void IncrementTwitchUsersVarById(List<string> userIds, string varName, object value, bool persisted = true)
        {
            throw new NotImplementedException();
        }

        public void IncrementYouTubeUsersVarById(List<string> userIds, string varName, object value, bool persisted = true)
        {
            throw new NotImplementedException();
        }

        public void KeyboardPress(string keyPress)
        {
            throw new NotImplementedException();
        }

        public bool KickBanUser(string userName, string reason = null, bool bot = false)
        {
            throw new NotImplementedException();
        }

        public KickUserInfo KickGetBot()
        {
            throw new NotImplementedException();
        }

        public KickUserInfo KickGetBroadcaster()
        {
            throw new NotImplementedException();
        }

        public void KickReplyToMessage(string message, string replyId, bool useBot = true, bool fallback = true)
        {
            throw new NotImplementedException();
        }

        public KickCategory KickSetCategory(string categoryName)
        {
            throw new NotImplementedException();
        }

        public bool KickSetTitle(string title)
        {
            throw new NotImplementedException();
        }

        public bool KickTimeoutUser(string username, int duration, string reason = null, bool bot = false)
        {
            throw new NotImplementedException();
        }

        public bool KickUnbanUser(string userName, bool bot = false)
        {
            throw new NotImplementedException();
        }

        public void LogDebug(string logLine)
        {
            throw new NotImplementedException();
        }

        public void LogVerbose(string logLine)
        {
            throw new NotImplementedException();
        }

        public void LogWarn(string logLine)
        {
            throw new NotImplementedException();
        }

        public void LumiaSendCommand(string command)
        {
            throw new NotImplementedException();
        }

        public void LumiaSetToDefault()
        {
            throw new NotImplementedException();
        }

        public bool MeldStudioConnect(int connectionIdx = -1)
        {
            throw new NotImplementedException();
        }

        public void MeldStudioDisconnect(int connectionIdx = -1)
        {
            throw new NotImplementedException();
        }

        public int MeldStudioGetConnectionByName(string name)
        {
            throw new NotImplementedException();
        }

        public bool MeldStudioHideLayerByName(string sceneName, string layerName, int connectionIdx = -1)
        {
            throw new NotImplementedException();
        }

        public bool MeldStudioIsConnected(int connectionIdx = -1)
        {
            throw new NotImplementedException();
        }

        public bool MeldStudioShowLayerByName(string sceneName, string layerName, int connectionIdx = -1)
        {
            throw new NotImplementedException();
        }

        public bool MeldStudioShowScene(string sceneId, int connectionIdx = -1)
        {
            throw new NotImplementedException();
        }

        public bool MeldStudioShowSceneByName(string sceneName, int connectionIdx = -1)
        {
            throw new NotImplementedException();
        }

        public void MidiSendControlChange(Guid deviceId, int channel, int controller, int value)
        {
            throw new NotImplementedException();
        }

        public void MidiSendControlChangeByName(string name, int channel, int controller, int value)
        {
            throw new NotImplementedException();
        }

        public void MidiSendNoteOn(Guid deviceId, int channel, int note, int velocity, int duration = 0, bool sendNoteOff = false)
        {
            throw new NotImplementedException();
        }

        public void MidiSendNoteOnByName(string name, int channel, int note, int velocity, int duration = 0, bool sendNoteOff = false)
        {
            throw new NotImplementedException();
        }

        public void MidiSendRaw(Guid deviceId, int command, int channel, int data1, int data2)
        {
            throw new NotImplementedException();
        }

        public void MidiSendRawByName(string name, int command, int channel, int data1, int data2)
        {
            throw new NotImplementedException();
        }

        public double NextDouble()
        {
            throw new NotImplementedException();
        }

        public bool ObsConnect(int connection = 0)
        {
            throw new NotImplementedException();
        }

        public long ObsConvertColorHex(string colorHex)
        {
            throw new NotImplementedException();
        }

        public long ObsConvertRgb(int a, int r, int g, int b)
        {
            throw new NotImplementedException();
        }

        public bool ObsCreateRecordChapter(string chapterName, int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void ObsDisconnect(int connection = 0)
        {
            throw new NotImplementedException();
        }

        public int ObsGetConnectionByName(string name)
        {
            throw new NotImplementedException();
        }

        public string ObsGetCurrentScene(int connection = 0)
        {
            throw new NotImplementedException();
        }

        public List<string> ObsGetGroupSources(string scene, string groupName, int connection = 0)
        {
            throw new NotImplementedException();
        }

        public string ObsGetSceneItemProperties(string scene, string source, int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void ObsHideFilter(string scene, string filterName, int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void ObsHideFilter(string scene, string source, string filterName, int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void ObsHideGroupsSources(string scene, string groupName, int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void ObsHideScenesFilters(string scene, int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void ObsHideSceneSources(string scene, int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void ObsHideSource(string scene, string source, int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void ObsHideSourcesFilters(string scene, string source, int connection = 0)
        {
            throw new NotImplementedException();
        }

        public bool ObsIsConnected(int connection = 0)
        {
            throw new NotImplementedException();
        }

        public bool ObsIsFilterEnabled(string scene, string filterName, int connection = 0)
        {
            throw new NotImplementedException();
        }

        public bool ObsIsFilterEnabled(string scene, string source, string filterName, int connection = 0)
        {
            throw new NotImplementedException();
        }

        public bool ObsIsRecording(int connection = 0)
        {
            throw new NotImplementedException();
        }

        public bool ObsIsSourceVisible(string scene, string source, int connection = 0)
        {
            throw new NotImplementedException();
        }

        public bool ObsIsStreaming(int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void ObsMediaNext(string scene, string source, int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void ObsMediaPause(string scene, string source, int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void ObsMediaPlay(string scene, string source, int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void ObsMediaPrevious(string scene, string source, int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void ObsPauseRecording(int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void ObsReplayBufferSave(int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void ObsReplayBufferStart(int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void ObsReplayBufferStop(int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void ObsResumeRecording(int connection = 0)
        {
            throw new NotImplementedException();
        }

        public string ObsSendBatchRaw(string data, bool haltOnFailure = false, int executionType = 0, int connection = 0)
        {
            throw new NotImplementedException();
        }

        public string ObsSendRaw(string requestType, string data, int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void ObsSetBrowserSource(string scene, string source, string url, int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void ObsSetColorSourceColor(string scene, string source, int a, int r, int g, int b, int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void ObsSetColorSourceColor(string scene, string source, string hexColor, int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void ObsSetColorSourceRandomColor(string scene, string source, int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void ObsSetFilterState(string scene, string filterName, int state, int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void ObsSetFilterState(string scene, string source, string filterName, int state, int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void ObsSetImageSourceFile(string scene, string source, string file, int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void ObsSetMediaSourceFile(string scene, string source, string file, int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void ObsSetMediaState(string scene, string source, int state, int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void ObsSetRandomFilterState(string scene, int state, int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void ObsSetRandomFilterState(string scene, string source, int state, int connection = 0)
        {
            throw new NotImplementedException();
        }

        public string ObsSetRandomGroupSourceVisible(string scene, string groupName, int connection = 0)
        {
            throw new NotImplementedException();
        }

        public string ObsSetRandomSceneSourceVisible(string scene, int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void ObsSetReplayBufferState(int state, int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void ObsSetScene(string sceneName, int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void ObsSetSourceMuteState(string scene, string source, int state, int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void ObsSetSourceVisibilityState(string scene, string source, int state, int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void ObsShowFilter(string scene, string filterName, int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void ObsShowFilter(string scene, string source, string filterName, int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void ObsShowSource(string scene, string source, int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void ObsSourceMute(string scene, string source, int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void ObsSourceMuteToggle(string scene, string source, int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void ObsSourceUnMute(string scene, string source, int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void ObsStartRecording(int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void ObsStartStreaming(int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void ObsStopRecording(int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void ObsStopStreaming(int connection = 0)
        {
            throw new NotImplementedException();
        }

        public bool ObsTakeScreenshot(string source, string path, int quality = -1, int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void ObsToggleFilter(string scene, string filterName, int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void ObsToggleFilter(string scene, string source, string filterName, int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void PauseActionQueue(string name)
        {
            throw new NotImplementedException();
        }

        public void PauseAllActionQueues()
        {
            throw new NotImplementedException();
        }

        public void PauseReward(string rewardId)
        {
            throw new NotImplementedException();
        }

        public double PlaySound(string fileName, float volume = 1, bool finishBeforeContinuing = false, string name = null, bool useFileName = true)
        {
            throw new NotImplementedException();
        }

        public double PlaySoundFromFolder(string path, float volume = 1, bool recursive = false, bool finishBeforeContinuing = false, string name = null, bool useFileName = true)
        {
            throw new NotImplementedException();
        }

        public void PronounClearForUserlogin(string userLogin)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, object> PronounLookup(string userLogin)
        {
            throw new NotImplementedException();
        }

        public bool RegisterCustomTrigger(string triggerName, string eventName, string[] categories)
        {
            throw new NotImplementedException();
        }

        public bool RemoveUserFromGroup(string userName, Platform platform, string groupName)
        {
            throw new NotImplementedException();
        }

        public bool RemoveUserIdFromGroup(string userId, Platform platform, string groupName)
        {
            throw new NotImplementedException();
        }

        public void ResetCredits()
        {
            throw new NotImplementedException();
        }

        public void ResetFirstWords()
        {
            throw new NotImplementedException();
        }

        public void ResumeActionQueue(string name, bool clear = false)
        {
            throw new NotImplementedException();
        }

        public void ResumeAllActionQueues(bool clear = false)
        {
            throw new NotImplementedException();
        }

        public bool RunAction(string actionName, bool runImmediately = true)
        {
            throw new NotImplementedException();
        }

        public bool RunActionById(string actionId, bool runImmediately = true)
        {
            throw new NotImplementedException();
        }

        public void SendAction(string action, bool useBot = true, bool fallback = true)
        {
            throw new NotImplementedException();
        }

        public void SendKickMessage(string message, bool useBot = true, bool fallback = true)
        {
            throw new NotImplementedException();
        }

        public void SendMessage(string message, bool useBot = true, bool fallback = true)
        {
            throw new NotImplementedException();
        }

        public void SendTrovoMessage(string message, bool useBot = true, bool fallback = true)
        {
            throw new NotImplementedException();
        }

        public bool SendWhisper(string userName, string message, bool bot = true)
        {
            throw new NotImplementedException();
        }

        public void SendYouTubeMessage(string message, bool useBot = true, bool fallback = true, string broadcastId = null)
        {
            throw new NotImplementedException();
        }

        public void SendYouTubeMessageToLatestMonitored(string message, bool useBot = true, bool fallback = true)
        {
            throw new NotImplementedException();
        }

        public void SetArgument(string variableName, object value)
        {
            throw new NotImplementedException();
        }

        public GameInfo SetChannelGame(string game)
        {
            throw new NotImplementedException();
        }

        public bool SetChannelGameById(string gameId)
        {
            throw new NotImplementedException();
        }

        public bool SetChannelTitle(string title)
        {
            throw new NotImplementedException();
        }

        public void SetKickUsersVarById(List<string> userIds, string varName, object value, bool persisted = true)
        {
            throw new NotImplementedException();
        }

        public void SetKickUserVar(string userName, string varName, object value, bool persisted = true)
        {
            throw new NotImplementedException();
        }

        public void SetKickUserVarById(string userId, string varName, object value, bool persisted = true)
        {
            throw new NotImplementedException();
        }

        public void SetTimerInterval(string timerId, int interval)
        {
            throw new NotImplementedException();
        }

        public void SetTrovoUsersVarById(List<string> userIds, string varName, object value, bool persisted = true)
        {
            throw new NotImplementedException();
        }

        public void SetTrovoUserVar(string userName, string varName, object value, bool persisted = true)
        {
            throw new NotImplementedException();
        }

        public void SetTrovoUserVarById(string userId, string varName, object value, bool persisted = true)
        {
            throw new NotImplementedException();
        }

        public void SetTwitchUsersVarById(List<string> userIds, string varName, object value, bool persisted = true)
        {
            throw new NotImplementedException();
        }

        public void SetTwitchUserVar(string userName, string varName, object value, bool persisted = true)
        {
            throw new NotImplementedException();
        }

        public void SetTwitchUserVarById(string userId, string varName, object value, bool persisted = true)
        {
            throw new NotImplementedException();
        }

        public void SetUserVar(string userName, string varName, object value, bool persisted = true)
        {
            throw new NotImplementedException();
        }

        public void SetYouTubeUsersVarById(List<string> userIds, string varName, object value, bool persisted = true)
        {
            throw new NotImplementedException();
        }

        public void SetYouTubeUserVar(string userName, string varName, object value, bool persisted = true)
        {
            throw new NotImplementedException();
        }

        public void SetYouTubeUserVarById(string userId, string varName, object value, bool persisted = true)
        {
            throw new NotImplementedException();
        }

        public void ShowToastNotification(string title, string message, string attribution = null, string iconPath = null)
        {
            throw new NotImplementedException();
        }

        public void ShowToastNotification(string id, string title, string message, string attribution = null, string iconPath = null)
        {
            throw new NotImplementedException();
        }

        public bool SlobsConnect(int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void SlobsDisconnect(int connection = 0)
        {
            throw new NotImplementedException();
        }

        public string SlobsGetCurrentScene(int connection = 0)
        {
            throw new NotImplementedException();
        }

        public List<string> SlobsGetGroupSources(string scene, string groupName, int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void SlobsHideFilter(string scene, string filterName, int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void SlobsHideFilter(string scene, string source, string filterName, int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void SlobsHideGroupsSources(string scene, string groupName, StreamlabsDesktopOutputType output = StreamlabsDesktopOutputType.Both, int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void SlobsHideSource(string scene, string source, StreamlabsDesktopOutputType output = StreamlabsDesktopOutputType.Both, int connection = 0)
        {
            throw new NotImplementedException();
        }

        public bool SlobsIsConnected(int connection = 0)
        {
            throw new NotImplementedException();
        }

        public bool SlobsIsFilterEnabled(string scene, string filterName, int connection = 0)
        {
            throw new NotImplementedException();
        }

        public bool SlobsIsFilterEnabled(string scene, string source, string filterName, int connection = 0)
        {
            throw new NotImplementedException();
        }

        public bool SlobsIsRecording(int connection = 0)
        {
            throw new NotImplementedException();
        }

        public bool SlobsIsSourceVisible(string scene, string source, StreamlabsDesktopOutputType output = StreamlabsDesktopOutputType.Both, int connection = 0)
        {
            throw new NotImplementedException();
        }

        public bool SlobsIsStreaming(int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void SlobsPauseRecording(int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void SlobsResumeRecording(int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void SlobsSetBrowserSource(string scene, string source, string url, int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void SlobsSetFilterState(string scene, string filterName, int state, int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void SlobsSetFilterState(string scene, string source, string filterName, int state, int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void SlobsSetGdiText(string scene, string source, string text, int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void SlobsSetRandomFilterState(string scene, int state, int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void SlobsSetRandomFilterState(string scene, string source, int state, int connection = 0)
        {
            throw new NotImplementedException();
        }

        public string SlobsSetRandomGroupSourceVisible(string scene, string groupName, StreamlabsDesktopOutputType output = StreamlabsDesktopOutputType.Both, int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void SlobsSetScene(string sceneName, int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void SlobsSetSourceMuteState(string scene, string source, int state, int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void SlobsSetSourceVisibility(string scene, string source, bool visible, StreamlabsDesktopOutputType output = StreamlabsDesktopOutputType.Both, int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void SlobsSetSourceVisibilityState(string scene, string source, int state, StreamlabsDesktopOutputType output = StreamlabsDesktopOutputType.Both, int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void SlobsShowFilter(string scene, string filterName, int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void SlobsShowFilter(string scene, string source, string filterName, int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void SlobsShowSource(string scene, string source, StreamlabsDesktopOutputType output = StreamlabsDesktopOutputType.Both, int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void SlobsSourceMute(string scene, string source, string filterName, int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void SlobsSourceMuteToggle(string scene, string source, string filterName, int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void SlobsSourceUnMute(string scene, string source, string filterName, int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void SlobsStartRecording(int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void SlobsStartStreaming(int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void SlobsStopRecording(int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void SlobsStopStreaming(int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void SlobsToggleFilter(string scene, string filterName, int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void SlobsToggleFilter(string scene, string source, string filterName, int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void StopAllSoundPlayback()
        {
            throw new NotImplementedException();
        }

        public void StopSoundPlayback(string soundName)
        {
            throw new NotImplementedException();
        }

        public void StreamDeckSetBackgroundColor(string buttonId, string color)
        {
            throw new NotImplementedException();
        }

        public void StreamDeckSetBackgroundColor(string buttonId, string color, int state)
        {
            throw new NotImplementedException();
        }

        public void StreamDeckSetBackgroundLocal(string buttonId, string imageFile)
        {
            throw new NotImplementedException();
        }

        public void StreamDeckSetBackgroundLocal(string buttonId, string imageFile, string color)
        {
            throw new NotImplementedException();
        }

        public void StreamDeckSetBackgroundLocal(string buttonId, string imageFile, int state)
        {
            throw new NotImplementedException();
        }

        public void StreamDeckSetBackgroundLocal(string buttonId, string imageFile, string color, int state)
        {
            throw new NotImplementedException();
        }

        public void StreamDeckSetBackgroundUrl(string buttonId, string imageUrl)
        {
            throw new NotImplementedException();
        }

        public void StreamDeckSetBackgroundUrl(string buttonId, string imageUrl, string color)
        {
            throw new NotImplementedException();
        }

        public void StreamDeckSetBackgroundUrl(string buttonId, string imageUrl, int state)
        {
            throw new NotImplementedException();
        }

        public void StreamDeckSetBackgroundUrl(string buttonId, string imageUrl, string color, int state)
        {
            throw new NotImplementedException();
        }

        public void StreamDeckSetState(string buttonId, int state)
        {
            throw new NotImplementedException();
        }

        public void StreamDeckSetTitle(string buttonId, string title)
        {
            throw new NotImplementedException();
        }

        public void StreamDeckSetTitle(string buttonId, string title, int state)
        {
            throw new NotImplementedException();
        }

        public void StreamDeckSetValue(string buttonId, string value)
        {
            throw new NotImplementedException();
        }

        public void StreamDeckShowAlert(string buttonId)
        {
            throw new NotImplementedException();
        }

        public void StreamDeckShowOk(string buttonId)
        {
            throw new NotImplementedException();
        }

        public void StreamDeckToggleState(string buttonId)
        {
            throw new NotImplementedException();
        }

        public bool ThrowingSystemActivateTriggerByName(string triggerName)
        {
            throw new NotImplementedException();
        }

        public bool ThrowingSystemThrowItemByName(string itemName, double delay = 0.05, int amount = 1)
        {
            throw new NotImplementedException();
        }

        public void TriggerCodeEvent(string eventName, bool useArgs = true)
        {
            throw new NotImplementedException();
        }

        public void TriggerCodeEvent(string eventName, Dictionary<string, object> args)
        {
            throw new NotImplementedException();
        }

        public void TriggerCodeEvent(string eventName, string json)
        {
            throw new NotImplementedException();
        }

        public void TriggerEvent(string eventName, bool useArgs = true)
        {
            throw new NotImplementedException();
        }

        public bool TrovoBanUser(string userName, bool bot = false)
        {
            throw new NotImplementedException();
        }

        public TrovoUserInfo TrovoGetBot()
        {
            throw new NotImplementedException();
        }

        public TrovoUserInfo TrovoGetBroadcaster()
        {
            throw new NotImplementedException();
        }

        public bool TrovoSetTitle(string title)
        {
            throw new NotImplementedException();
        }

        public bool TrovoTimeoutUser(string userName, int duration, bool bot = false)
        {
            throw new NotImplementedException();
        }

        public bool TrovoUnbanUser(string userName, bool bot = false)
        {
            throw new NotImplementedException();
        }

        public bool TryGetArg(string argName, out object value)
        {
            throw new NotImplementedException();
        }

        public int TtsSpeak(string voiceAlias, string message, bool badWordFilter = false)
        {
            throw new NotImplementedException();
        }

        public bool TwitchAddBlockedTerm(string term)
        {
            throw new NotImplementedException();
        }

        public bool TwitchAddChannelTag(string tag)
        {
            throw new NotImplementedException();
        }

        public bool TwitchAddModerator(string userName)
        {
            throw new NotImplementedException();
        }

        public bool TwitchAddVip(string userName)
        {
            throw new NotImplementedException();
        }

        public void TwitchAnnounce(string message, bool bot = false, string color = null, bool fallback = false)
        {
            throw new NotImplementedException();
        }

        public bool TwitchApproveAutoHeldMessage(string messageId)
        {
            throw new NotImplementedException();
        }

        public bool TwitchAssignGuestStarSlot(string userLogin, int slot)
        {
            throw new NotImplementedException();
        }

        public bool TwitchBanUser(string userName, string reason = null, bool bot = false)
        {
            throw new NotImplementedException();
        }

        public bool TwitchBlockUser(string userLogin, TwitchBlockContext context = TwitchBlockContext.None, TwitchBlockReason reason = TwitchBlockReason.None)
        {
            throw new NotImplementedException();
        }

        public bool TwitchBlockUserById(string userId, TwitchBlockContext context = TwitchBlockContext.None, TwitchBlockReason reason = TwitchBlockReason.None)
        {
            throw new NotImplementedException();
        }

        public bool TwitchCancelRaid()
        {
            throw new NotImplementedException();
        }

        public bool TwitchClearChannelTags()
        {
            throw new NotImplementedException();
        }

        public bool TwitchClearChatMessages(bool bot = true)
        {
            throw new NotImplementedException();
        }

        public GuestSession TwitchCreateGuestStarSession()
        {
            throw new NotImplementedException();
        }

        public bool TwitchDeleteChatMessage(string messageId, bool bot = true)
        {
            throw new NotImplementedException();
        }

        public bool TwitchDeleteGuestStarInvite(string userLogin)
        {
            throw new NotImplementedException();
        }

        public bool TwitchDeleteGuestStarSlot(string userLogin, int slot)
        {
            throw new NotImplementedException();
        }

        public bool TwitchDenyAutoHeldMessage(string messageId)
        {
            throw new NotImplementedException();
        }

        public void TwitchEmoteOnly(bool enabled = true)
        {
            throw new NotImplementedException();
        }

        public GuestSession TwitchEndGuestStarSession()
        {
            throw new NotImplementedException();
        }

        public void TwitchFollowMode(bool enabled = true, int duration = 0)
        {
            throw new NotImplementedException();
        }

        public long TwitchGetBitsDonatedByUserId(string userId)
        {
            throw new NotImplementedException();
        }

        public List<string> TwitchGetBlockedTerms()
        {
            throw new NotImplementedException();
        }

        public TwitchUserInfo TwitchGetBot()
        {
            throw new NotImplementedException();
        }

        public TwitchUserInfo TwitchGetBroadcaster()
        {
            throw new NotImplementedException();
        }

        public GuestStarSettings TwitchGetChannelGuestStarSettings()
        {
            throw new NotImplementedException();
        }

        public long TwitchGetChannelPointsUsedByUserId(string userId)
        {
            throw new NotImplementedException();
        }

        public ClipDownloadData TwitchGetClipDownloadUrls(string clipId)
        {
            throw new NotImplementedException();
        }

        public TwitchUserInfoEx TwitchGetExtendedUserInfoById(string userId)
        {
            throw new NotImplementedException();
        }

        public TwitchUserInfoEx TwitchGetExtendedUserInfoByLogin(string userLogin)
        {
            throw new NotImplementedException();
        }

        public List<GuestStarInvite> TwitchGetGuestStarInvites()
        {
            throw new NotImplementedException();
        }

        public GuestSession TwitchGetGuestStarSession()
        {
            throw new NotImplementedException();
        }

        public int TwitchGetPrerollFreeTime()
        {
            throw new NotImplementedException();
        }

        public TwitchRewardCounter TwitchGetRewardCounter(string rewardId, bool persisted = true)
        {
            throw new NotImplementedException();
        }

        public List<TwitchReward> TwitchGetRewards()
        {
            throw new NotImplementedException();
        }

        public TwitchRewardCounter TwitchGetRewardUserCounter(string userLogin, string rewardId, bool persisted = true)
        {
            throw new NotImplementedException();
        }

        public TwitchRewardCounter TwitchGetRewardUserCounterById(string userId, string rewardId, bool persisted = true)
        {
            throw new NotImplementedException();
        }

        public List<TwitchRewardCounter> TwitchGetRewardUserCounters(string rewardId, bool persisted = true)
        {
            throw new NotImplementedException();
        }

        public TwitchUserInfo TwitchGetUserInfoById(string userId)
        {
            throw new NotImplementedException();
        }

        public TwitchUserInfo TwitchGetUserInfoByLogin(string userLogin)
        {
            throw new NotImplementedException();
        }

        public bool TwitchIsUserSubscribed(string userId, out string tier)
        {
            throw new NotImplementedException();
        }

        public void TwitchPollArchive(string pollId)
        {
            throw new NotImplementedException();
        }

        public bool TwitchPollCreate(string title, List<string> choices, int duration, int channelPointsPerVote = 0)
        {
            throw new NotImplementedException();
        }

        public void TwitchPollTerminate(string pollId)
        {
            throw new NotImplementedException();
        }

        public void TwitchPredictionCancel(string predictionId)
        {
            throw new NotImplementedException();
        }

        public string TwitchPredictionCreate(string title, string firstOption, string secondOption, int duration)
        {
            throw new NotImplementedException();
        }

        public string TwitchPredictionCreate(string title, List<string> options, int duration)
        {
            throw new NotImplementedException();
        }

        public void TwitchPredictionLock(string predictionId)
        {
            throw new NotImplementedException();
        }

        public void TwitchPredictionResolve(string predictionId, string winningId)
        {
            throw new NotImplementedException();
        }

        public bool TwitchRedemptionCancel(string rewardId, string redemptionId)
        {
            throw new NotImplementedException();
        }

        public bool TwitchRedemptionFulfill(string rewardId, string redemptionId)
        {
            throw new NotImplementedException();
        }

        public bool TwitchRemoveBlockedTerm(string term)
        {
            throw new NotImplementedException();
        }

        public bool TwitchRemoveChannelTag(string tag)
        {
            throw new NotImplementedException();
        }

        public bool TwitchRemoveModerator(string userName)
        {
            throw new NotImplementedException();
        }

        public bool TwitchRemoveVip(string userName)
        {
            throw new NotImplementedException();
        }

        public void TwitchReplyToMessage(string message, string replyId, bool useBot = true, bool fallback = true)
        {
            throw new NotImplementedException();
        }

        public void TwitchResetRewardCounter(string rewardId)
        {
            throw new NotImplementedException();
        }

        public void TwitchResetRewardUserCounters(string rewardId)
        {
            throw new NotImplementedException();
        }

        public void TwitchResetUserRewardCounter(string rewardId, string userId)
        {
            throw new NotImplementedException();
        }

        public void TwitchResetUserRewardCounters(string userId, bool persisted)
        {
            throw new NotImplementedException();
        }

        public void TwitchRewardGroupDisable(string groupName)
        {
            throw new NotImplementedException();
        }

        public void TwitchRewardGroupEnable(string groupName)
        {
            throw new NotImplementedException();
        }

        public void TwitchRewardGroupPause(string groupName)
        {
            throw new NotImplementedException();
        }

        public void TwitchRewardGroupToggleEnable(string groupName)
        {
            throw new NotImplementedException();
        }

        public void TwitchRewardGroupTogglePause(string groupName)
        {
            throw new NotImplementedException();
        }

        public void TwitchRewardGroupUnPause(string groupName)
        {
            throw new NotImplementedException();
        }

        public bool TwitchRunCommercial(int duration)
        {
            throw new NotImplementedException();
        }

        public bool TwitchSendGuestStarInvite(string userLogin)
        {
            throw new NotImplementedException();
        }

        public bool TwitchSendShoutoutById(string userId)
        {
            throw new NotImplementedException();
        }

        public bool TwitchSendShoutoutByLogin(string userLogin)
        {
            throw new NotImplementedException();
        }

        public bool TwitchSetChannelTags(List<string> tags)
        {
            throw new NotImplementedException();
        }

        public bool TwitchShieldModeOff()
        {
            throw new NotImplementedException();
        }

        public bool TwitchShieldModeOn()
        {
            throw new NotImplementedException();
        }

        public void TwitchSlowMode(bool enabled = true, int duration = 0)
        {
            throw new NotImplementedException();
        }

        public bool TwitchStartRaidById(string userId)
        {
            throw new NotImplementedException();
        }

        public bool TwitchStartRaidByName(string userName)
        {
            throw new NotImplementedException();
        }

        public void TwitchSubscriberOnly(bool enabled = true)
        {
            throw new NotImplementedException();
        }

        public bool TwitchTimeoutUser(string username, int duration, string reason = null, bool bot = false)
        {
            throw new NotImplementedException();
        }

        public bool TwitchUnbanUser(string userName, bool bot = false)
        {
            throw new NotImplementedException();
        }

        public bool TwitchUnblockUser(string userLogin)
        {
            throw new NotImplementedException();
        }

        public bool TwitchUnblockUserById(string userId)
        {
            throw new NotImplementedException();
        }

        public bool TwitchUpdateChannelGuestStarSettings(bool? isModeratorSendLiveEnabled = null, int? slotCount = null, bool? isBrowserSourceAudioEnabled = null, string groupLayout = null, bool? regeneratgeBrowserSource = null)
        {
            throw new NotImplementedException();
        }

        public bool TwitchUpdateGuestStarSlot(int sourceSlot, int destinationSlot)
        {
            throw new NotImplementedException();
        }

        public bool TwitchUpdateGuestStarSlotSettings(int slotId, bool? isAudioEnabled = null, bool? isVideoEnabled = null, bool? isLive = null, int? volume = null)
        {
            throw new NotImplementedException();
        }

        public bool TwitchWarnUser(string userName, string reason)
        {
            throw new NotImplementedException();
        }

        public void UnPauseReward(string rewardId)
        {
            throw new NotImplementedException();
        }

        public void UnsetAllUsersVar(string varName, bool persisted = true)
        {
            throw new NotImplementedException();
        }

        public void UnsetGlobalVar(string varName, bool persisted = true)
        {
            throw new NotImplementedException();
        }

        public void UnsetKickUser(string userName, bool persisted = true)
        {
            throw new NotImplementedException();
        }

        public void UnsetKickUserById(string userId, bool persisted = true)
        {
            throw new NotImplementedException();
        }

        public void UnsetKickUserVar(string userName, string varName, bool persisted = true)
        {
            throw new NotImplementedException();
        }

        public void UnsetKickUserVarById(string userId, string varName, bool persisted = true)
        {
            throw new NotImplementedException();
        }

        public void UnsetTrovoUser(string userName, bool persisted = true)
        {
            throw new NotImplementedException();
        }

        public void UnsetTrovoUserById(string userId, bool persisted = true)
        {
            throw new NotImplementedException();
        }

        public void UnsetTrovoUserVar(string userName, string varName, bool persisted = true)
        {
            throw new NotImplementedException();
        }

        public void UnsetTrovoUserVarById(string userId, string varName, bool persisted = true)
        {
            throw new NotImplementedException();
        }

        public void UnsetTwitchUser(string userName, bool persisted = true)
        {
            throw new NotImplementedException();
        }

        public void UnsetTwitchUserById(string userId, bool persisted = true)
        {
            throw new NotImplementedException();
        }

        public void UnsetTwitchUserVar(string userName, string varName, bool persisted = true)
        {
            throw new NotImplementedException();
        }

        public void UnsetTwitchUserVarById(string userId, string varName, bool persisted = true)
        {
            throw new NotImplementedException();
        }

        public void UnsetUser(string userName, bool persisted = true)
        {
            throw new NotImplementedException();
        }

        public void UnsetUserVar(string userName, string varName, bool persisted = true)
        {
            throw new NotImplementedException();
        }

        public void UnsetYouTubeUser(string userName, bool persisted = true)
        {
            throw new NotImplementedException();
        }

        public void UnsetYouTubeUserById(string userId, bool persisted = true)
        {
            throw new NotImplementedException();
        }

        public void UnsetYouTubeUserVar(string userName, string varName, bool persisted = true)
        {
            throw new NotImplementedException();
        }

        public void UnsetYouTubeUserVarById(string userId, string varName, bool persisted = true)
        {
            throw new NotImplementedException();
        }

        public bool UpdateReward(string rewardId, string title = null, string prompt = null, int? cost = null, string backroundColor = null)
        {
            throw new NotImplementedException();
        }

        public bool UpdateRewardBackgroundColor(string rewardId, string backgroundColor)
        {
            throw new NotImplementedException();
        }

        public void UpdateRewardCooldown(string rewardId, long cooldown, bool additive = false)
        {
            throw new NotImplementedException();
        }

        public void UpdateRewardCost(string rewardId, int cost, bool additive = false)
        {
            throw new NotImplementedException();
        }

        public void UpdateRewardMaxPerStream(string rewardId, long redeems, bool additive = false)
        {
            throw new NotImplementedException();
        }

        public void UpdateRewardMaxPerUserPerStream(string rewardId, long redeems, bool additive = false)
        {
            throw new NotImplementedException();
        }

        public bool UpdateRewardPrompt(string rewardId, string prompt)
        {
            throw new NotImplementedException();
        }

        public bool UpdateRewardTitle(string rewardId, string title)
        {
            throw new NotImplementedException();
        }

        public string UrlEncode(string text)
        {
            throw new NotImplementedException();
        }

        public bool UserIdInGroup(string userId, Platform platform, string groupName)
        {
            throw new NotImplementedException();
        }

        public bool UserInGroup(string userName, Platform platform, string groupName)
        {
            throw new NotImplementedException();
        }

        public List<GroupUser> UsersInGroup(string groupName)
        {
            throw new NotImplementedException();
        }

        public void VoiceModBackgroundEffectOff()
        {
            throw new NotImplementedException();
        }

        public void VoiceModBackgroundEffectOn()
        {
            throw new NotImplementedException();
        }

        public void VoiceModCensorOff()
        {
            throw new NotImplementedException();
        }

        public void VoiceModCensorOn()
        {
            throw new NotImplementedException();
        }

        public bool VoiceModGetBackgroundEffectStatus()
        {
            throw new NotImplementedException();
        }

        public string VoiceModGetCurrentVoice()
        {
            throw new NotImplementedException();
        }

        public bool VoiceModGetHearMyselfStatus()
        {
            throw new NotImplementedException();
        }

        public bool VoiceModGetVoiceChangerStatus()
        {
            throw new NotImplementedException();
        }

        public void VoiceModHearMyVoiceOff()
        {
            throw new NotImplementedException();
        }

        public void VoiceModHearMyVoiceOn()
        {
            throw new NotImplementedException();
        }

        public void VoiceModSelectVoice(string voiceId)
        {
            throw new NotImplementedException();
        }

        public void VoiceModVoiceChangerOff()
        {
            throw new NotImplementedException();
        }

        public void VoiceModVoiceChangerOn()
        {
            throw new NotImplementedException();
        }

        public bool VTubeStudioActivateExpression(string expressionFile, float fadeTime = 0.25F)
        {
            throw new NotImplementedException();
        }

        public bool VTubeStudioColorTintAll(string hexColor, double mixWithSceneLighting = 0)
        {
            throw new NotImplementedException();
        }

        public bool VTubeStudioColorTintByNameContains(string hexColor, double mixWithSceneLighting, List<string> filterValues)
        {
            throw new NotImplementedException();
        }

        public bool VTubeStudioColorTintByNames(string hexColor, double mixWithSceneLighting, List<string> filterValues)
        {
            throw new NotImplementedException();
        }

        public bool VTubeStudioColorTintByNumber(string hexColor, double mixWithSceneLighting, List<int> artMeshNumbers)
        {
            throw new NotImplementedException();
        }

        public bool VTubeStudioColorTintByTagContains(string hexColor, double mixWithSceneLighting, List<string> filterValues)
        {
            throw new NotImplementedException();
        }

        public bool VTubeStudioColorTintByTags(string hexColor, double mixWithSceneLighting, List<string> filterValues)
        {
            throw new NotImplementedException();
        }

        public bool VTubeStudioDeactivateExpression(string expressionFile)
        {
            throw new NotImplementedException();
        }

        public VTSModelPosition VTubeStudioGetModelPosition()
        {
            throw new NotImplementedException();
        }

        public bool VTubeStudioLoadModelById(string modelId)
        {
            throw new NotImplementedException();
        }

        public bool VTubeStudioLoadModelByName(string modelName)
        {
            throw new NotImplementedException();
        }

        public bool VTubeStudioMoveModel(double seconds, bool relative, double? posX = null, double? posY = null, double? rotation = null, double? size = null)
        {
            throw new NotImplementedException();
        }

        public bool VTubeStudioRandomColorTint()
        {
            throw new NotImplementedException();
        }

        public bool VTubeStudioResetAllColorTints()
        {
            throw new NotImplementedException();
        }

        public string VTubeStudioSendRawRequest(string requestType, string data)
        {
            throw new NotImplementedException();
        }

        public bool VTubeStudioTriggerHotkeyById(string hotkeyId)
        {
            throw new NotImplementedException();
        }

        public bool VTubeStudioTriggerHotkeyByName(string hotkeyName)
        {
            throw new NotImplementedException();
        }

        public string WaveLinkGetInputIdentifier(string inputName)
        {
            throw new NotImplementedException();
        }

        public string WaveLinkGetMicrophoneIdentifier(string microphoneName)
        {
            throw new NotImplementedException();
        }

        public void WaveLinkInputFilterBypassBypassed(string inputIdentifier, string mixer)
        {
            throw new NotImplementedException();
        }

        public void WaveLinkInputFilterBypassEnabled(string inputIdentifier, string mixer)
        {
            throw new NotImplementedException();
        }

        public void WaveLinkInputFilterBypassToggle(string inputIdentifier, string mixer)
        {
            throw new NotImplementedException();
        }

        public void WaveLinkInputFilterDisable(string inputIdentifier, string filterIdentifier)
        {
            throw new NotImplementedException();
        }

        public void WaveLinkInputFilterEnable(string inputIdentifier, string filterIdentifier)
        {
            throw new NotImplementedException();
        }

        public void WaveLinkInputFilterToggle(string inputIdentifier, string filterIdentifier)
        {
            throw new NotImplementedException();
        }

        public string WaveLinkInputGetFilterIdentifier(string inputIdentifier, string filterName)
        {
            throw new NotImplementedException();
        }

        public bool WaveLinkInputGetFilterState(string inputIdentifier, string filterIdentifier)
        {
            throw new NotImplementedException();
        }

        public long WaveLinkInputGetVolume(string inputIdentifier, string mixer)
        {
            throw new NotImplementedException();
        }

        public void WaveLinkInputMute(string identifier, string mixer)
        {
            throw new NotImplementedException();
        }

        public void WaveLinkInputSetVolume(string inputIdentifier, string mixer, int volume)
        {
            throw new NotImplementedException();
        }

        public void WaveLinkInputToggleMute(string identifier, string mixer)
        {
            throw new NotImplementedException();
        }

        public void WaveLinkInputUnmute(string identifier, string mixer)
        {
            throw new NotImplementedException();
        }

        public double WaveLinkMicrophoneGetVolume(string microphoneIdentifier)
        {
            throw new NotImplementedException();
        }

        public void WaveLinkMicrophoneMute(string microphoneIdentifier)
        {
            throw new NotImplementedException();
        }

        public void WaveLinkMicrophoneSetVolume(string microphoneIdentifier, double volume)
        {
            throw new NotImplementedException();
        }

        public void WaveLinkMicrophoneToggleMute(string microphoneIdentifier)
        {
            throw new NotImplementedException();
        }

        public void WaveLinkMicrophoneUnmute(string microphoneIdentifier)
        {
            throw new NotImplementedException();
        }

        public void WaveLinkOutputMute(string mixer)
        {
            throw new NotImplementedException();
        }

        public void WaveLinkOutputToggleMute(string mixer)
        {
            throw new NotImplementedException();
        }

        public void WaveLinkOutputUnmute(string mixer)
        {
            throw new NotImplementedException();
        }

        public void WaveLinkSetOutputVolume(string mixer, int volume)
        {
            throw new NotImplementedException();
        }

        public void WebsocketBroadcastJson(string data)
        {
            throw new NotImplementedException();
        }

        public void WebsocketBroadcastString(string data)
        {
            throw new NotImplementedException();
        }

        public void WebsocketConnect(int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void WebsocketCustomServerBroadcast(string data, string sessionId, int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void WebsocketCustomServerCloseAllSessions(int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void WebsocketCustomServerCloseSession(string sessionId, int connection = 0)
        {
            throw new NotImplementedException();
        }

        public int WebsocketCustomServerGetConnectionByName(string name)
        {
            throw new NotImplementedException();
        }

        public bool WebsocketCustomServerIsListening(int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void WebsocketCustomServerStart(int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void WebsocketCustomServerStop(int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void WebsocketDisconnect(int connection = 0)
        {
            throw new NotImplementedException();
        }

        public bool WebsocketIsConnected(int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void WebsocketSend(string data, int connection = 0)
        {
            throw new NotImplementedException();
        }

        public void WebsocketSend(byte[] data, int connection = 0)
        {
            throw new NotImplementedException();
        }

        public bool YouTubeAddTags(List<string> tags, string broadcastId = null)
        {
            throw new NotImplementedException();
        }

        public bool YouTubeBanUserById(string userId, string broadcastId = null)
        {
            throw new NotImplementedException();
        }

        public bool YouTubeBanUserByName(string userName, string broadcastId = null)
        {
            throw new NotImplementedException();
        }

        public bool YouTubeClearTags(string broadcastId = null)
        {
            throw new NotImplementedException();
        }

        public YouTubeUserInfo YouTubeGetBot()
        {
            throw new NotImplementedException();
        }

        public YouTubeUserInfo YouTubeGetBroadcaster()
        {
            throw new NotImplementedException();
        }

        public YouTubeBroadcastInfo YouTubeGetLatestMonitoredBroadcast()
        {
            throw new NotImplementedException();
        }

        public List<YouTubeBroadcastInfo> YouTubeGetMonitoredBroadcasts()
        {
            throw new NotImplementedException();
        }

        public bool YouTubeRemoveTags(List<string> tags, string broadcastId = null)
        {
            throw new NotImplementedException();
        }

        public bool YouTubeSetCategory(string categoryName, string broadcastId = null)
        {
            throw new NotImplementedException();
        }

        public bool YouTubeSetDescription(string description, string broadcastId = null)
        {
            throw new NotImplementedException();
        }

        public bool YouTubeSetMetaData(string title, string description, string privacy, string categoryName, string broadcastId = null)
        {
            throw new NotImplementedException();
        }

        public bool YouTubeSetPrivacy(string privacy, string broadcastId = null)
        {
            throw new NotImplementedException();
        }

        public bool YouTubeSetTitle(string title, string broadcastId = null)
        {
            throw new NotImplementedException();
        }

        public bool YouTubeTimeoutUserById(string userId, int duration, string broadcastId = null)
        {
            throw new NotImplementedException();
        }

        public bool YouTubeTimeoutUserByName(string userName, int duration, string broadcastId = null)
        {
            throw new NotImplementedException();
        }
    }
}
