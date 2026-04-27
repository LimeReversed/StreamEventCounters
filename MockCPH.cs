using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Counters
{
    public class Source
    {
        public object value = null;
        public bool visible;
    }

    /// <summary>
    /// Mock interface matching the methods your code uses from CPH / _cph.
    /// This lets you test locally without Streamer.bot.
    /// </summary>
    public interface IInlineInvokeProxy
    {
        bool TryGetArg<T>(string name, out T value);

        T GetGlobalVar<T>(string name, bool persisted = true);

        void SetGlobalVar(string name, object value, bool persisted = true);

        void LogInfo(string message);

        void LogError(string message);

        void Wait(int milliseconds);

        void ObsSetGdiText(string scene, string source, string text);

        void ObsSetSourceVisibility(string scene, string source, bool visible);

        void ObsMediaRestart(string scene, string source);

        void ObsMediaStop(string scene, string source);
    }

    /// <summary>
    /// Simple local mock implementation for testing.
    /// Replace CPH with: new MockCPH()
    /// </summary>
    public class MockCPH : IInlineInvokeProxy
    {
        public readonly Dictionary<string, object> _args = new Dictionary<string, object>();
        public readonly Dictionary<string, object> _globalVars = new Dictionary<string, object>();
        public readonly Dictionary<string, Source> _sources = new Dictionary<string, Source>();
        public static string subscriberCount = "subscriberCount";
        public static string savedSubscriberCount = "savedSubscriberCount";
        public static string followerCount = "followerCount";
        public static string savedFollowerCount = "savedFollowerCount";
        public static string previousTips = "previousTips";
        public static string previousBits = "previousBits";

        // -----------------------------
        // Helper methods for testing
        // -----------------------------

        public void SetArg(string name, object value)
        {
            _args[name] = value;
        }

        public void ClearArgs()
        {
            _args.Clear();
        }

        // -----------------------------
        // Required interface methods
        // -----------------------------

        public bool TryGetArg<T>(string name, out T value)
        {
            if (_args.TryGetValue(name, out var rawValue))
            {
                if (rawValue is T typedValue)
                {
                    value = typedValue;
                    Console.WriteLine($"[TryGetArg] {name} = {value}");
                    return true;
                }

                try
                {
                    value = (T)Convert.ChangeType(rawValue, typeof(T));
                    Console.WriteLine($"[TryGetArg] {name} = {value}");
                    return true;
                }
                catch
                {
                    // fall through
                }
            }

            Console.WriteLine($"[TryGetArg FAILED] {name}");
            value = default;
            return false;
        }

        public T GetGlobalVar<T>(string name, bool persisted = true)
        {
            if (_globalVars.TryGetValue(name, out var rawValue))
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

        public void SetGlobalVar(string name, object value, bool persisted = true)
        {
            _globalVars[name] = value;
            Console.WriteLine($"[SetGlobalVar] {name} = {value}");
        }

        public void LogInfo(string message)
        {
            Console.WriteLine($"[INFO] {message}");
        }

        public void LogError(string message)
        {
            Console.WriteLine($"[ERROR] {message}");
        }

        public void Wait(int milliseconds)
        {
            Console.WriteLine($"[Wait] {milliseconds}ms");
            // Optional:
            // System.Threading.Thread.Sleep(milliseconds);
        }

        public void ObsSetGdiText(string scene, string source, string text)
        {
            Console.WriteLine($"[OBS Text] Scene='{scene}' Source='{source}' Text='{text}'");
            if (_sources.ContainsKey(source))
            {
                _sources[source].value = text;
            }
            else 
            {
                _sources[source] = new Source { value = text };
            }
        }

        public void ObsSetSourceVisibility(string scene, string source, bool visible)
        {
            Console.WriteLine($"[OBS Visibility] Scene='{scene}' Source='{source}' Visible={visible}");
            if (_sources.ContainsKey(source))
            {
                _sources[source].visible = visible;
            }
            else
            {
                _sources[source] = new Source { visible = visible };
            }

        }

        public void ObsMediaRestart(string scene, string source)
        {
            Console.WriteLine($"[OBS Media Restart] Scene='{scene}' Source='{source}'");
        }

        public void ObsMediaStop(string scene, string source)
        {
            Console.WriteLine($"[OBS Media Stop] Scene='{scene}' Source='{source}'");
        }
    }
}
