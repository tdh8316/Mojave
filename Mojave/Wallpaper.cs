using System;
using System.Runtime.InteropServices;

namespace Mojave
{
    public class Wallpaper
    {

        [DllImport("user32.dll")]
        private static extern bool SystemParametersInfo(uint uiAction, uint uiParam, string pvParam, uint fWinIni);

        public static readonly UInt32 SPI_SETDESKWALLPAPER = 0x14;
        public static readonly UInt32 SPI_GETDESKWALLPAPER = 0x0073;
        public static readonly UInt32 SPIF_UPDATEINIFILE = 0x01;
        public static readonly UInt32 SPIF_SENDWININICHANGE = 0x02;

        static public void SetWallpaper(string path)
        {
            SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, path, SPIF_UPDATEINIFILE);
        }

    }
}
