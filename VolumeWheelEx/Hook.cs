using System;
using System.Runtime.InteropServices;

static class Hook {
    private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
    private static IntPtr hhook = IntPtr.Zero;
    private static LowLevelKeyboardProc _proc = hookProc;
    
    const int WH_MOUSE_LL    = 14;
    const int WM_MOUSEWHEEL  = 0x020A;
    const int WM_MBUTTONDOWN = 0x0207;
    const byte VK_ALT        = 0x12;

    public static void SetHook() {
        IntPtr hInstance = LoadLibrary("User32");
        hhook = SetWindowsHookEx(WH_MOUSE_LL, _proc, hInstance, 0);
    }

    public static void UnHook() {
        UnhookWindowsHookEx(hhook);
    }

    static short wData;
    static IntPtr stopMsg = (IntPtr)1;

    public static IntPtr hookProc(int code, IntPtr wParam, IntPtr lParam) {
        if((code >= 0) && (GetKeyState(VK_ALT)<0)) {

            if((long)wParam == WM_MOUSEWHEEL) {
                wData = Marshal.ReadInt16(lParam + 10);
                if(wData > 0) {
                    Program.VolumeUp();
                } else if(wData < 0) {
                    Program.VolumeDown();
                }
                return stopMsg;
            } else if((long)wParam == WM_MBUTTONDOWN) {
                Program.Mute();
                return stopMsg;
            }
            
        }
        return IntPtr.Zero;
    }

    [DllImport("user32.dll")]
    static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc callback, IntPtr hInstance, uint threadId);

    [DllImport("user32.dll")]
    static extern bool UnhookWindowsHookEx(IntPtr hInstance);

    [DllImport("kernel32.dll")]
    static extern IntPtr LoadLibrary(string lpFileName);

    [DllImport("user32.dll")]
    static extern short GetKeyState(byte virtualKeyCode);
}
