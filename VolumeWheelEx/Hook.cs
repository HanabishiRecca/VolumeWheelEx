using System;
using System.Runtime.InteropServices;

static class Hook {
    
    private static IntPtr hHook = IntPtr.Zero;

    public static void SetHook() {
        hHook = WinAPI.SetWindowsHookEx(WinAPI.WH_MOUSE_LL, Intercept, WinAPI.LoadLibrary("User32"), 0);
    }

    public static void UnHook() {
        WinAPI.UnhookWindowsHookEx(hHook);
    }

    static short wData;
    static IntPtr interceptMsg = (IntPtr)1;

    public static IntPtr Intercept(int code, int wParam, IntPtr lParam) {
        if((code >= 0) && (WinAPI.GetKeyState(WinAPI.VK_ALT) < 0)) {

            if(wParam == WinAPI.WM_MOUSEWHEEL) {
                wData = Marshal.ReadInt16(lParam + 10);
                if(wData > 0) {
                    Program.VolumeUp();
                } else if(wData < 0) {
                    Program.VolumeDown();
                }
                return interceptMsg;
            } else if(wParam == WinAPI.WM_MBUTTONDOWN) {
                Program.Mute();
                return interceptMsg;
            }
            
        }
        return WinAPI.CallNextHookEx(IntPtr.Zero, code, wParam, lParam);
    }
    
}
