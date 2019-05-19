using System;

namespace Mojave
{
    class MainThread
    {
        private static int latestIndex = 0;

        private static int GetBackgroundIndexFromTime(int t)
        {
            if (t > 24) return 16;

            switch (t)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                    return 16;
                case 5: return 1;
                case 6: return 2;
                case 7: return 3;
                case 8:
                case 9:
                    return 4;
                case 10: return 5;
                case 11: return 6;
                case 12:
                case 13:
                    return 7;
                case 14: return 8;
                case 15:
                case 16:
                    return 9;
                case 17: return 10;
                case 18: return 11;
                case 19: return 12;
                case 20: return 13;
                case 21: return 14;
                case 22:
                case 23:
                case 24:
                    return 15;
            }

            return 1;
        }


        public static void WallpaperChangeScheduler()
        {
            for (; ; )
            {
                int index = GetBackgroundIndexFromTime(int.Parse(DateTime.Now.ToString("HH")));
                if (index != latestIndex)
                {
                    Wallpaper.SetWallpaper(path: System.IO.Directory.GetCurrentDirectory() + "/Resources/" + index.ToString() + ".jpg");
                }

                else
                {
                    latestIndex = index;
                }

                try
                {
                    System.Threading.Thread.Sleep(60000);
                }
                catch (System.Threading.ThreadInterruptedException)
                {
                    return;
                }
                catch (Exception error)
                {
                    _ = System.Windows.MessageBox.Show(error.Message, "An unhandled exception occurred.");
                    return;
                }
            }
        }
    }
}
