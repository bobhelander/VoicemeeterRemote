using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Voicemeeter
{
    internal static class RemoteWrapper
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
    }
}
