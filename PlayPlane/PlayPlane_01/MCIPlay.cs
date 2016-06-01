using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace PlayPlane_01
{
    class MCIPlay
    {
        public static uint SND_ASYNC = 0x0001;
        public static uint SND_FILENAME = 0x00020000;
        [DllImport("winmm.dll")]
        public static extern uint mciSendString(string lpstrCommand,
        string lpstrReturnString, uint uReturnLength, uint hWndCallback);

        private string musicPath;

        public MCIPlay(string musicPath)
        {
            this.musicPath = musicPath;
        }

        public void Play()
        {
            mciSendString(@"close temp_alias", null, 0, 0);
            mciSendString(@"open "+musicPath+" alias temp_alias", null, 0, 0);
            mciSendString("play temp_alias ", null, 0, 0);
        }
    }
}
