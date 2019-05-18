using System;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace Mojave
{
    public partial class MainWindow : Window
    {
        private Point startPos;
        private readonly NotifyIcon notifyIcon = new NotifyIcon();
        private readonly Thread MojaveThread = new Thread(new ThreadStart(MainThread.WallpaperChangeScheduler));
        private bool isStarted = false;
        public NotifyIcon notify;

        public MainWindow()
        {
            MojaveThread.IsBackground = true;
            InitializeComponent();
            notifyIcon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Reflection.Assembly.GetExecutingAssembly().Location);
            notifyIcon.BalloonTipClosed += (_s, _e) => notifyIcon.Visible = false;
        }

        private void OnWindowLoad(object sender, RoutedEventArgs e)
        {
            try
            {
                ContextMenu menu = new ContextMenu();
                notify = new NotifyIcon
                {
                    // TODO: notify.Icon
                    Visible = true,
                    ContextMenu = menu,
                    Text = "Mojave",
                    Icon = System.Drawing.SystemIcons.Application
                };

                MenuItem exitMenu = new MenuItem();
                menu.MenuItems.Add(exitMenu);
                exitMenu.Index = 0;
                exitMenu.Text = "Exit";
                exitMenu.Click += delegate (object c, EventArgs args)
                {
                    MojaveThread.Interrupt();
                    notify.Dispose();
                    Close();
                };
            }
            catch (Exception)
            {

            }
        }

        private void OnClose(object sender, RoutedEventArgs e)
            => WindowState = WindowState.Minimized;

        public void System_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (WindowState == WindowState.Maximized && Math.Abs(startPos.Y - e.GetPosition(null).Y) > 2)
                {
                    Point point = PointToScreen(e.GetPosition(null));

                    WindowState = WindowState.Normal;

                    Left = point.X - (ActualWidth / 2);
                    Top = point.Y - (ActualHeight / 2);
                }
                DragMove();
            }
        }

        private void StartProcess(object sender, RoutedEventArgs e)
        {
            notifyIcon.Visible = true;

            if (!MojaveThread.IsAlive)
            {
                if (isStarted)
                {
                    System.Windows.MessageBox.Show(
                        "Couldn't start the main thread due to it has been started.\n" +
                        "Please restart Mojave.",
                        "Error while starting new thread.");
                    return;
                }
                notifyIcon.ShowBalloonTip(
                    3000,
                    "Mojave is running!",
                    "A background thread is started.\n",
                    ToolTipIcon.Info
                );
                MojaveThread.Start();
                isStarted = true;
            }

            else
            {
                notifyIcon.ShowBalloonTip(
                    3000,
                    "Mojave is already running!",
                    "So the Mojave process did not start.",
                    ToolTipIcon.Error
                );
            }
        }
    }
}
