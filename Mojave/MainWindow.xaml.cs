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
        private NotifyIcon notify;

        public MainWindow()
        {
            InitializeComponent();
            MojaveThread.IsBackground = true;
            notifyIcon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Reflection.Assembly.GetExecutingAssembly().Location);
            notifyIcon.BalloonTipClosed += (_s, _e) =>
            {
                notifyIcon.Visible = false;
            };
        }

        private void OnWindowLoad(object sender, RoutedEventArgs e)
        {
            try
            {
                ContextMenu menu = new ContextMenu();
                notify = new NotifyIcon
                {
                    Visible = true,
                    ContextMenu = menu,
                    Text = "Mojave",
                    Icon = new System.Drawing.Icon("resources/mojave_sun.ico")
                };

                MenuItem activateMenu = new MenuItem();
                _ = menu.MenuItems.Add(item: activateMenu);
                activateMenu.Text = "Activate";
                activateMenu.Click += delegate (object c, EventArgs args)
                { StartProcess(); };
                
                MenuItem showMenu = new MenuItem();
                _ = menu.MenuItems.Add(item: showMenu);
                showMenu.Text = "Show window";
                showMenu.Click += delegate (object c, EventArgs args)
                { WindowState = WindowState.Maximized; };

                _ = menu.MenuItems.Add("-");

                MenuItem exitMenu = new MenuItem();
                _ = menu.MenuItems.Add(item: exitMenu);
                exitMenu.Text = "Exit";
                exitMenu.Click += delegate (object c, EventArgs args)
                { Terminate(); };

            }
            catch (Exception error)
            {
                _ = System.Windows.MessageBox.Show(error.Message, "An unhandled exception occurred.");
            }

            WindowState = WindowState.Minimized;
            StartProcess();
        }

        private void OnClose(object sender, RoutedEventArgs e)
            => WindowState = WindowState.Minimized;

        private void Github(object sender, RoutedEventArgs e)
            => System.Diagnostics.Process.Start("explorer.exe", "https://github.com/tdh8316/Mojave");

        public void OnDrag(object sender, System.Windows.Input.MouseEventArgs e)
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

        private void StartProcess(object sender = null, RoutedEventArgs e = null)
        {
            notifyIcon.Visible = true;

            if (!MojaveThread.IsAlive)
            {
                if (isStarted)
                {
                    System.Windows.MessageBox.Show(
                        "Error while starting new thread.",
                        "Failed to activate Mojave due to it is already activated.\n" +
                        "Please restart Mojave."
                    );
                    return;
                }

                try
                {
                    MojaveThread.Start();
                    OnOffLabel.Content = "Running";
                    StartButton.IsEnabled = false;
                }
                catch (Exception error)
                {
                    _ = System.Windows.MessageBox.Show(error.Message, "An unhandled exception occurred.");
                    Terminate();
                }
                isStarted = true;
                notifyIcon.ShowBalloonTip(
                    1500,
                    "Mojave is running!",
                    "You can deactivate Mojave in the system tray.",
                    ToolTipIcon.Info
                );
            }

            else
            {
                notifyIcon.ShowBalloonTip(
                    1500,
                    "Mojave is already running!",
                    "You can deactivate Mojave in the system tray.",
                    ToolTipIcon.Error
                );
            }
        }

        private void Terminate()
        {
            if (MojaveThread.IsAlive)
            {
                MojaveThread.Interrupt();
            }
            notify.Dispose();
            Close();
        }
    }
}
