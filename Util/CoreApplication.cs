using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace MouseMover.Util;

public class CoreApplication
{
    private static readonly int _numKeyboardKeys = 26;
    private static readonly byte _keyAKeyboard = 0x41;
    private static readonly string _iniFilename = "config.ini";

    [DllImport("user32.dll")]
    private static extern bool SetCursorPos(int x, int y);

    [DllImport("user32.dll")]
    private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

    [DllImport("user32.dll")]
    private static extern int GetSystemMetrics(int nIndex);

    public static void Start(int? timeOut, bool simulateKeyPress, bool shutdown, int? shutdownTime)
    {
        int seed = Guid.NewGuid().GetHashCode();
        Random random = new Random(seed);
        int height;
        int width;
        byte key;
        // TODO: add timer on screen
        float runTimer = 0;

        (int heightMax, int widthMax) = ScreenMaxResolution();
        byte[] keys = FillKeyboardKeys();

        while (true)
        {
            height = random.Next(0, heightMax);
            width = random.Next(0, widthMax);
            if (simulateKeyPress)
            {
                key = keys[random.Next(0, keys.Length - 1)];
                SimulateKeyboardUse(key, 0x45, 0, 0);
            }
            ChangeCursorPosition(width, height);
            if (shutdown)
            {
                runTimer += (float)timeOut / 60;
                if (runTimer >= shutdownTime)
                {
                    // TODO: shutdown PC
                    break;
                }
            }
            Thread.Sleep(TimeSpan.FromSeconds(timeOut.Value));
        }
    }

    private static byte[] FillKeyboardKeys()
    {
        var keys = new byte[_numKeyboardKeys];
        for (int i = 0; i < _numKeyboardKeys; i++)
        {
            keys[i] = (byte)(_keyAKeyboard + i);
        }

        return keys;
    }

    private static void ChangeCursorPosition(int x, int y)
    {
        SetCursorPos(x, y);
    }

    private static void SimulateKeyboardUse(byte key, byte bScan, uint dwFlags, int dwExtraInfo)
    {
        keybd_event(key, bScan, dwFlags, dwExtraInfo);
    }

    private static string PathIniFile()
    {
        string path = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
        return Path.Combine(path, _iniFilename);
    }

    private static (int, int) ScreenMaxResolution()
    {
        const int SM_CYFULLSCREEN = 17;
        const int SM_CXFULLSCREEN = 16;

        int height = GetSystemMetrics(SM_CYFULLSCREEN);
        int width = GetSystemMetrics(SM_CXFULLSCREEN);

        return (height, width);
    }
}
