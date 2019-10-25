using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voicemeeter
{
    public enum RunVoicemeeterParam
    {
        None = 0,
        Voicemeeter = 1, 
        VoicemeeterBanana = 2,
        VoicemeeterPotato = 3,
    };

    public enum LevelType
    {
        PreFaderInput = 0,
        PostFaderInput = 1,
        PostMuteInput = 2,
        Output = 3
    };

    public enum LoginResponse
    {
        AlreadyLoggedIn = -2,
        NoClient = -1,
        OK = 0,
        VoicemeeterNotRunning = 1,
    }

    public static class InputChannel
    {
        public const int Strip1Left = 0;
        public const int Strip1Right = 1;
        public const int Strip2Left = 2;
        public const int Strip2Right = 3;
        public const int Strip3Left = 4;
        public const int Strip3Right = 5;
        public const int VAIOLeft = 6;
        public const int VAIORight = 7;
        public const int AUXLeft = 8;
        public const int AUXRight = 9;
    }

    public static class OutputChannel
    {
        public const int A1Left = 0;
        public const int A1Right = 1;
        public const int A2Left = 2;
        public const int A2Right = 3;
        public const int A3Left = 4;
        public const int A3Right = 5;
        public const int Bus1Left = 6;
        public const int Bus1Right = 7;
        public const int Bus2Left = 8;
        public const int Bus2Right = 9;
    }

    public static class VoicemeeterCommand
    {
        public static string Shutdown = "Command.Shutdown";
        public static string Show = "Command.Show";
        public static string Restart = "Command.Restart";
        public static string Eject = "Command.Eject";
        public static string Reset = "Command.Reset";
        public static string Save = "Command.Save";
        public static string Load = "Command.Load";
    }
}
