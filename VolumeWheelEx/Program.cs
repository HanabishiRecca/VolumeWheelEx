using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;

static class Program {
    static void Main() {
        bool b;
        Mutex m = new Mutex(true, "VolumeWheelEx", out b);

        if(!b) {
            var thisProc = Process.GetCurrentProcess();
            var prcs = Process.GetProcessesByName(thisProc.ProcessName);
            for(int i = 0; i < prcs.Length; i++) {
                if(prcs[i].Id != thisProc.Id) {
                    prcs[i].Kill();
                }
            }
            
            return;
        }

        Hook.SetHook();
        System.Windows.Forms.Application.Run();
        Hook.UnHook();
    }

    public static void VolumeUp() {
        SendKey(VK_VOLUME_UP);
    }

    public static void VolumeDown() {
        SendKey(VK_VOLUME_DOWN);
    }

    public static void Mute() {
        SendKey(VK_VOLUME_MUTE);
    }

    static void SendKey(byte key) {
        keybd_event(key, 0x45, 1, IntPtr.Zero);
        keybd_event(key, 0x45, 1 | 2, IntPtr.Zero);
    }

    const byte VK_VOLUME_MUTE = 0xAD;
    const byte VK_VOLUME_DOWN = 0xAE;
    const byte VK_VOLUME_UP   = 0xAF;

    [DllImport("user32.dll")]
    static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, IntPtr dwExtraInfo);

}
