using System;
using System.Threading;
using System.Diagnostics;

static class Program {

    static void Main() {

        bool isFirstInstance;
        var mutex = new Mutex(true, "VolumeWheelEx", out isFirstInstance);

        if(isFirstInstance) {

            try {
                Hook.SetHook();
                var exit = false;
                while(!exit) { try { exit = WinAPI.GetMessage(IntPtr.Zero, IntPtr.Zero, 0, 0) > 0; } catch(NullReferenceException) { /* Don't worry, GetMessage can sometimes returns NULL. */ } };
                Hook.UnHook();
            } catch(Exception e) {
                //LOG file directly on desktop
                System.IO.File.AppendAllText(Environment.GetEnvironmentVariable("userprofile") + "\\Desktop\\VolumeWheelEx.log", "[" + DateTime.Now.ToString() + "]\n" + e.Source + " -> " + e.ToString() + "\n" + e.StackTrace + "\n\n");
            } finally {
                mutex.ReleaseMutex();
            }

        } else {

            try {
                var thisProc = Process.GetCurrentProcess();
                var prcs = Process.GetProcessesByName(thisProc.ProcessName);
                for(int i = 0; i < prcs.Length; i++) {
                    if(prcs[i].Id != thisProc.Id) {
                        WinAPI.PostThreadMessage((uint)prcs[i].Threads[0].Id, WinAPI.WM_QUIT, UIntPtr.Zero, IntPtr.Zero);
                    }
                }
            } catch { }

        }
    }

    public static void VolumeUp() {
        SendKey(WinAPI.VK_VOLUME_UP);
    }

    public static void VolumeDown() {
        SendKey(WinAPI.VK_VOLUME_DOWN);
    }

    public static void Mute() {
        SendKey(WinAPI.VK_VOLUME_MUTE);
    }

    static void SendKey(byte key) {
        WinAPI.keybd_event(key, 0x45, 1, IntPtr.Zero);
        WinAPI.keybd_event(key, 0x45, 1 | 2, IntPtr.Zero);
    }

}
