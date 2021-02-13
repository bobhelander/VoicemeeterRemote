using Microsoft.Win32;
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Voicemeeter;

namespace VoiceMeeter
{
    public static class Remote
    {
        // Don't keep loading the DLL
        private static IntPtr? handle;

        #region Parameters
        
        /// <summary>
        /// Gets a text value
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static string GetTextParameter(string parameter)
        {
            var buffer = new StringBuilder(255);
            var code = RemoteWrapper.InternalGetParameterW(parameter, buffer);
            if (code == 0)
                return buffer.ToString();

            return string.Empty;
        }

        /// <summary>
        /// Set a text value
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static void SetTextParameter(string parameter, string value)
        {
            TestResult(RemoteWrapper.InternalSetParameterW(parameter, value));
        }

        /// <summary>
        /// Get a named parameter
        /// </summary>
        /// <param name="parameter">Parameter name</param>
        /// <returns>float value</returns>
        public static float GetParameter(string parameter)
        {
            float value = 0;
            TestResult(RemoteWrapper.GetParameter(parameter, ref value));
            return value;
        }

        /// <summary>
        /// Set a named parameter
        /// </summary>
        /// <param name="parameter">Parameter name</param>
        /// <param name="value">float value</param>
        public static void SetParameter(string parameter, float value)
        {
            TestResult(RemoteWrapper.SetParameter(parameter, value));
        }

        #endregion

        #region Commands

        /// <summary>
        /// Start the VoiceMeeter program
        /// </summary>
        /// <param name="voicemeterType"></param>
        public static void Start(RunVoicemeeterParam voicemeterType)
        {
            switch (RemoteWrapper.InternalRunVoicemeeter((int)voicemeterType))
            {
                case 0: return;
                case -1: throw new Exception("Not installed");
                default: throw new Exception("Unknown");
            }
        }

        /// <summary>
        /// Shutdown the VoiceMeeter program
        /// </summary>
        /// <param name="voicemeeterType">The Voicemeeter program to run</param>
        public static void Shutdown()
        {
            TestResult(RemoteWrapper.SetParameter(VoicemeeterCommand.Shutdown, 1));
        }

        /// <summary>
        /// Restart the audio engine
        /// </summary>
        public static void Restart()
        {
            TestResult(RemoteWrapper.SetParameter(VoicemeeterCommand.Restart, 1));
        }

        /// <summary>
        /// Shows the running Voicemeeter application if minimized.
        /// </summary>
        public static void Show()
        {
            TestResult(RemoteWrapper.SetParameter(VoicemeeterCommand.Show, 1));
        }

        /// <summary>
        /// Return if the parameters have changed since the last time this method was called.
        /// </summary>
        public static int IsParametersDirty()
        {
            try
            {
                return RemoteWrapper.IsParametersDirty();
            }
            catch(Exception ex)
            {
                // TODO: Figure out the Memory Exception when calling the API
                ;
            }
            return 0;
        }

        /// <summary>
        /// Eject Cassette 
        /// </summary>
        public static void Eject()
        {
            TestResult(RemoteWrapper.SetParameter(VoicemeeterCommand.Eject, 1));
        }

        /// <summary>
        /// Load a configuation file name
        /// </summary>
        /// <param name="configurationFileName">Full path to file</param>
        public static void Load(string configurationFileName)
        {
            SetTextParameter(VoicemeeterCommand.Load, configurationFileName);
        }

        /// <summary>
        /// Save a configuration to the given file name
        /// </summary>
        /// <param name="configurationFileName">Full path to file</param>
        public static void Save(string configurationFileName)
        {
            SetTextParameter(VoicemeeterCommand.Load, configurationFileName);
        }

        #endregion

        #region Rx

        public static float GetLevel(LevelType type, int channel)
        {
            float value = 0;
            TestLevelResult(RemoteWrapper.GetLevel((int)type, channel, ref value));
            return value;
        }

        private static void TestLevelResult(int result)
        {
            // 0: OK(no error).
            // -1: error
            // -2: no server.
            // -3: no level available
            // -4: out of range
            switch (result)
            {
                case 0: return;
                case -1: throw new Exception("Error");
                case -2: throw new Exception("Not Connected");
                case -3: return;
                case -4: throw new ArgumentException("Channel out of range");
                default: throw new Exception("Unknown");
            }
        }

        #endregion

        /// <summary>
        /// Logs into the Voicemeeter application.  Starts the given application (Voicemeeter, Bananna, Potato) if it is not already runnning.
        /// </summary>
        /// <param name="voicemeeterType">The Voicemeeter program to run</param>
        /// <returns>IDisposable class to dispose when finished with the remote.</returns>
        public static async Task<IDisposable> Initialize(RunVoicemeeterParam voicemeeterType)
        {
            if (handle.HasValue == false)
            {
                // Find current version from the registry
                const string key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
                const string uninstKey = "VB:Voicemeeter {17359A74-1236-5467}";
                var voicemeeter = Registry.GetValue($"{key}\\{uninstKey}", "UninstallString", null);
                if (voicemeeter == null)
                {
                    throw new Exception("Voicemeeter not installed");
                }

                handle = Wrapper.LoadLibrary(
                    System.IO.Path.Combine(System.IO.Path.GetDirectoryName(voicemeeter.ToString()), "VoicemeeterRemote.dll"));
            }

            var startVoiceMeeter = voicemeeterType != RunVoicemeeterParam.None;

            if (await Login(voicemeeterType, startVoiceMeeter).ConfigureAwait(false))
            {
                return new VoicemeeterClient();
            }

            return null;
        }

        public static async Task<bool> Login(RunVoicemeeterParam voicemeeterType, bool retry = true)
        {
            switch ((LoginResponse)RemoteWrapper.LoginVoicemeeter())
            {
                case LoginResponse.OK:
                case LoginResponse.AlreadyLoggedIn:
                    return true;

                case LoginResponse.VoicemeeterNotRunning:
                    if (retry)
                    {
                        // Run voicemeeter program 
                        Start(voicemeeterType);

                        await Task.Delay(2000).ConfigureAwait(false);
                        return await Login(voicemeeterType, false).ConfigureAwait(false);
                    }
                    break;
            }
            return false;
        }

        private static void TestResult(int result)
        {
            //0: OK(no error).
            //-1: error
            //-2: no server.
            //-3: unknown parameter
            //-5: structure mismatch
            switch (result)
            {
                case 0: return;
                case -1: throw new Exception("Parameter Error");
                case -2: throw new Exception("Not Connected");
                case -3: throw new ArgumentException("Parameter not found");
                case -5: throw new Exception("Structure mismatch");
                default: throw new Exception("Unknown");
            }
        }
    }
}