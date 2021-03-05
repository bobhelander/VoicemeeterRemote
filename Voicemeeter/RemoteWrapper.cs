using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Voicemeeter
{
    internal static class RemoteWrapper
    {
        internal static int LoginVoicemeeter()
        {
            return Environment.Is64BitProcess
                ? RemoteWrapper64.LoginVoicemeeter()
                : RemoteWrapper32.LoginVoicemeeter();
        }
        
        internal static int Logout()
        {
            return Environment.Is64BitProcess
                ? RemoteWrapper64.Logout()
                : RemoteWrapper32.Logout();
        }

        internal static int InternalRunVoicemeeter(int voicemeterType)
        {
            return Environment.Is64BitProcess
                ? RemoteWrapper64.InternalRunVoicemeeter(voicemeterType)
                : RemoteWrapper32.InternalRunVoicemeeter(voicemeterType);
        }

        internal static int GetParameter(string szParamName, ref float value)
        {
            return Environment.Is64BitProcess
                ? RemoteWrapper64.GetParameter(szParamName, ref value)
                : RemoteWrapper32.GetParameter(szParamName, ref value);
        }

        internal static int SetParameter(string szParamName, float value)
        {
            return Environment.Is64BitProcess
                ? RemoteWrapper64.SetParameter(szParamName, value)
                : RemoteWrapper32.SetParameter(szParamName, value);
        }

        internal static int InternalGetParameterW(string szParamName, StringBuilder value)
        {

            return Environment.Is64BitProcess
                ? RemoteWrapper64.InternalGetParameterW(szParamName, value)
                : RemoteWrapper32.InternalGetParameterW(szParamName, value);
        }

        internal static int InternalSetParameterW(string szParamName, string value)
        {
            return Environment.Is64BitProcess
                ? RemoteWrapper64.InternalSetParameterW(szParamName, value)
                : RemoteWrapper32.InternalSetParameterW(szParamName, value);
        }

        internal static int IsParametersDirty()
        {
            return Environment.Is64BitProcess
                ? RemoteWrapper64.IsParametersDirty()
                : RemoteWrapper32.IsParametersDirty();
        }

        internal static int GetLevel(int nType, int nuChannel, ref float value)
        {
            return Environment.Is64BitProcess
                ? RemoteWrapper64.GetLevel(nType, nuChannel, ref value)
                : RemoteWrapper32.GetLevel(nType, nuChannel, ref value);

        }

        public static int SetParameters(string szParameters)
        {
            return Environment.Is64BitProcess
                ? RemoteWrapper64.SetParameters(szParameters)
                : RemoteWrapper32.SetParameters(szParameters);
        }
    }
    
    
    internal static class RemoteWrapper32
    {
        [DllImport("VoicemeeterRemote.dll", EntryPoint = "VBVMR_Login", CallingConvention = CallingConvention.StdCall)]
        internal static extern int LoginVoicemeeter();

        [DllImport("VoicemeeterRemote.dll", EntryPoint = "VBVMR_Logout")]
        internal static extern int Logout();

        [DllImport("VoicemeeterRemote.dll", EntryPoint = "VBVMR_RunVoicemeeter")]
        internal static extern int InternalRunVoicemeeter(int voicemeterType);

        // Get/Set Parameters return codes
        // returns 0: OK (no error).
        //        -1: error
        //        -2: no server.
        //        -3: unknown parameter
        //        -5: structure mismatch

        // long __stdcall VBVMR_GetParameterFloat(char * szParamName, float * pValue);
        [DllImport("VoicemeeterRemote.dll", EntryPoint = "VBVMR_GetParameterFloat")]
        internal static extern int GetParameter(string szParamName, ref float value);

        // long __stdcall VBVMR_SetParameterFloat(char * szParamName, float Value);
        [DllImport("VoicemeeterRemote.dll", EntryPoint = "VBVMR_SetParameterFloat")]
        internal static extern int SetParameter(string szParamName, float value);

        //long __stdcall VBVMR_GetParameterStringA(char* szParamName, char* szString);
        //long __stdcall VBVMR_GetParameterStringW(char* szParamName, unsigned short* wszString);
        [DllImport("VoicemeeterRemote.dll", EntryPoint = "VBVMR_GetParameterStringW", CallingConvention = CallingConvention.StdCall)]
        internal static extern int InternalGetParameterW(
            [MarshalAs(UnmanagedType.LPStr)] string szParamName,      // char*
            [MarshalAs(UnmanagedType.LPWStr)] StringBuilder value);   // unsigned short*

        // long __stdcall VBVMR_SetParameterStringA(char* szParamName, char* szString);
        // long __stdcall VBVMR_SetParameterStringW(char* szParamName, unsigned short* wszString);
        [DllImport("VoicemeeterRemote.dll", EntryPoint = "VBVMR_SetParameterStringW", CallingConvention = CallingConvention.StdCall)]
        internal static extern int InternalSetParameterW(
            [MarshalAs(UnmanagedType.LPStr)] string szParamName,      // char*
            [MarshalAs(UnmanagedType.LPWStr)] string value);          // unsigned short*

        // Check if parameters have changed.
        // Call this function periodically (typically every 10 or 20ms).
        // (this function must be called from one thread only)
        // returns:	 0: no new paramters.
        //  		 1: New parameters -> update your display.
        //			-1: error(unexpected)
        //			-2: no server.
        // long __stdcall VBVMR_IsParametersDirty(void);
        [DllImport("VoicemeeterRemote.dll", EntryPoint = "VBVMR_IsParametersDirty")]
        internal static extern int IsParametersDirty();

        // long __stdcall VBVMR_GetLevel(long nType, long nuChannel, float* pValue);
        [DllImport("VoicemeeterRemote.dll", EntryPoint = "VBVMR_GetLevel")]
        internal static extern int GetLevel(int nType, int nuChannel, ref float value);

        // long __stadcall VBVMR_SetParameters(char* szParameters)
        // long __stadcall VBVMR_SetParameters(unsigned short* szParameters)
        [DllImport("VoicemeeterRemote.dll", EntryPoint = "VBVMR_SetParametersW", CallingConvention = CallingConvention.StdCall)]
        public static extern int SetParameters([MarshalAs(UnmanagedType.LPWStr)] string szParameters);
    }


    internal static class RemoteWrapper64
    {
        [DllImport("VoicemeeterRemote64.dll", EntryPoint = "VBVMR_Login", CallingConvention = CallingConvention.StdCall)]
        internal static extern int LoginVoicemeeter();

        [DllImport("VoicemeeterRemote64.dll", EntryPoint = "VBVMR_Logout")]
        internal static extern int Logout();

        [DllImport("VoicemeeterRemote64.dll", EntryPoint = "VBVMR_RunVoicemeeter")]
        internal static extern int InternalRunVoicemeeter(int voicemeterType);

        // Get/Set Parameters return codes
        // returns 0: OK (no error).
        //        -1: error
        //        -2: no server.
        //        -3: unknown parameter
        //        -5: structure mismatch

        // long __stdcall VBVMR_GetParameterFloat(char * szParamName, float * pValue);
        [DllImport("VoicemeeterRemote64.dll", EntryPoint = "VBVMR_GetParameterFloat")]
        internal static extern int GetParameter(string szParamName, ref float value);

        // long __stdcall VBVMR_SetParameterFloat(char * szParamName, float Value);
        [DllImport("VoicemeeterRemote64.dll", EntryPoint = "VBVMR_SetParameterFloat")]
        internal static extern int SetParameter(string szParamName, float value);

        //long __stdcall VBVMR_GetParameterStringA(char* szParamName, char* szString);
        //long __stdcall VBVMR_GetParameterStringW(char* szParamName, unsigned short* wszString);
        [DllImport("VoicemeeterRemote64.dll", EntryPoint = "VBVMR_GetParameterStringW", CallingConvention = CallingConvention.StdCall)]
        internal static extern int InternalGetParameterW(
            [MarshalAs(UnmanagedType.LPStr)] string szParamName,      // char*
            [MarshalAs(UnmanagedType.LPWStr)] StringBuilder value);   // unsigned short*

        // long __stdcall VBVMR_SetParameterStringA(char* szParamName, char* szString);
        // long __stdcall VBVMR_SetParameterStringW(char* szParamName, unsigned short* wszString);
        [DllImport("VoicemeeterRemote64.dll", EntryPoint = "VBVMR_SetParameterStringW", CallingConvention = CallingConvention.StdCall)]
        internal static extern int InternalSetParameterW(
            [MarshalAs(UnmanagedType.LPStr)] string szParamName,      // char*
            [MarshalAs(UnmanagedType.LPWStr)] string value);          // unsigned short*

        // Check if parameters have changed.
        // Call this function periodically (typically every 10 or 20ms).
        // (this function must be called from one thread only)
        // returns:	 0: no new paramters.
        //  		 1: New parameters -> update your display.
        //			-1: error(unexpected)
        //			-2: no server.
        // long __stdcall VBVMR_IsParametersDirty(void);
        [DllImport("VoicemeeterRemote64.dll", EntryPoint = "VBVMR_IsParametersDirty")]
        internal static extern int IsParametersDirty();

        // long __stdcall VBVMR_GetLevel(long nType, long nuChannel, float* pValue);
        [DllImport("VoicemeeterRemote64.dll", EntryPoint = "VBVMR_GetLevel")]
        internal static extern int GetLevel(int nType, int nuChannel, ref float value);

        // long __stadcall VBVMR_SetParameters(char* szParameters)
        // long __stadcall VBVMR_SetParameters(unsigned short* szParameters)
        [DllImport("VoicemeeterRemote64.dll", EntryPoint = "VBVMR_SetParametersW", CallingConvention = CallingConvention.StdCall)]
        public static extern int SetParameters([MarshalAs(UnmanagedType.LPWStr)] string szParameters);
    }
}
