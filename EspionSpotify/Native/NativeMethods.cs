using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace EspionSpotify.Native
{
    internal static class NativeMethods
    {
        private const uint WM_APPCOMMAND = 0x0319;
        private const uint APPCOMMAND_MEDIA_PLAY_PAUSE = 0xE0000;
        private const uint APPCOMMAND_MEDIA_NEXT = 0xB0000;

        internal static void PreventSleep()
        {
            SetThreadExecutionState(ExecutionState.EsContinuous | ExecutionState.EsSystemRequired);
        }

        internal static void AllowSleep()
        {
            SetThreadExecutionState(ExecutionState.EsContinuous);
        }

        internal static void SendKeyPessNextMedia(IntPtr process)
        {
            Task.Run(() =>
            {
                SendMessage(process, WM_APPCOMMAND, IntPtr.Zero, new IntPtr(APPCOMMAND_MEDIA_NEXT));
            });
        }

        internal static void SendKeyPessPauseMedia(IntPtr process)
        {
            Task.Run(() =>
            {
                    SendMessage(process, WM_APPCOMMAND, IntPtr.Zero, new IntPtr(APPCOMMAND_MEDIA_PLAY_PAUSE));
            });
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern ExecutionState SetThreadExecutionState(ExecutionState esFlags);

        
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        internal static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        
        [FlagsAttribute]
        private enum ExecutionState : uint
        {
            EsAwaymodeRequired = 0x00000040,
            EsContinuous = 0x80000000,
            EsDisplayRequired = 0x00000002,
            EsSystemRequired = 0x00000001
        }
    }
}