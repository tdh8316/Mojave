using System.Runtime.InteropServices;

namespace Mojave
{
    public class Wallpaper
    {

        [DllImport("user32.dll")]
        private static extern bool SystemParametersInfo(uint uiAction, uint uiParam, string pvParam, uint fWinIni);

        public static readonly uint SPI_SETDESKWALLPAPER = 0x14;
        public static readonly uint SPI_GETDESKWALLPAPER = 0x0073;
        public static readonly uint SPIF_UPDATEINIFILE = 0x01;
        public static readonly uint SPIF_SENDWININICHANGE = 0x02;

        public static void SetWallpaper(string path) => _ = SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, path, SPIF_UPDATEINIFILE);

    }
}
