using System;
using System.Runtime.InteropServices;

public static class WinAPI {

    //Hook type
    public const int WH_MOUSE_LL = 14;

    //Message codes
    public const uint WM_MOUSEWHEEL     = 0x020A;
    public const uint WM_MBUTTONDOWN    = 0x0207;
    public const uint WM_QUIT           = 0x0012;

    //Keys
    public const byte VK_VOLUME_MUTE    = 0xAD;
    public const byte VK_VOLUME_DOWN    = 0xAE;
    public const byte VK_VOLUME_UP      = 0xAF;
    public const byte VK_ALT            = 0x12;

    //WinAPI

    public delegate IntPtr onHook(int nCode, int wParam, IntPtr lParam);

    [DllImport("user32.dll")]
    public static extern IntPtr SetWindowsHookEx(int idHook, onHook callback, IntPtr hInstance, uint threadId);

    [DllImport("user32.dll")]
    public static extern bool UnhookWindowsHookEx(IntPtr hInstance);

    [DllImport("kernel32.dll")]
    public static extern IntPtr LoadLibrary(string lpFileName);

    [DllImport("user32.dll")]
    public static extern short GetKeyState(byte virtualKeyCode);

    [DllImport("user32.dll")]
    public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, IntPtr dwExtraInfo);

    [DllImport("user32.dll")]
    public static extern sbyte GetMessage(IntPtr lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);

    [DllImport("user32.dll")]
    public static extern bool PostThreadMessage(uint threadId, uint msg, UIntPtr wParam, IntPtr lParam);
}
