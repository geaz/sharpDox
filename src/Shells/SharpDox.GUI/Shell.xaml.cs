using SharpDox.Build;
using SharpDox.GUI.Controls.ConfigGrid;
using SharpDox.GUI.ViewModels;
using SharpDox.Sdk.Config;
using SharpDox.Sdk.Exporter;
using SharpDox.Sdk.Local;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;

namespace SharpDox.GUI
{
    public partial class Shell
    {
        public event Action OnClose;

        public Shell(IConfigController configController, IExporter[] allExporters, ILocalController localController, BuildController buildController)
        {
            var guiStrings = localController.GetLocalStrings<SDGuiStrings>();
            DataContext = new ShellViewModel(guiStrings, configController, buildController, ExecuteOnClose);
            Strings = guiStrings;
            Style = (Style)FindResource("SharpDoxWindowStyle");

            Closing += (sender, args) => ExecuteOnClose();
            SourceInitialized += OnWindowSourceInitialized;

            InitializeComponent();
            svBody.Content = new ConfigGridControl(configController, allExporters, localController, buildController);
        }

        public override void OnApplyTemplate()
        {
            var minimizeButton = GetTemplateChild("MinimizeButton") as MenuItem;
            if (minimizeButton != null)
                minimizeButton.Click += (s, a) => { WindowState = WindowState.Minimized; };

            var restoreButton = GetTemplateChild("RestoreButton") as MenuItem;
            if (restoreButton != null)
                restoreButton.Click += (s, a) => SwitchWindowState();

            var closeButton = GetTemplateChild("CloseButton") as MenuItem;
            if (closeButton != null)
                closeButton.Click += (s, a) => Close();

            base.OnApplyTemplate();
        }

        private void ExecuteOnClose()
        {
            if (OnClose != null) OnClose();
            Application.Current.Shutdown();
        }

        private void SwitchWindowState()
        {
            switch (WindowState)
            {
                case WindowState.Normal:
                    WindowState = WindowState.Maximized;
                    break;
                case WindowState.Maximized:
                    WindowState = WindowState.Normal;
                    break;
            }
        }

        private void OnWindowSourceInitialized(object sender, EventArgs e)
        {
            var mWindowHandle = (new WindowInteropHelper(this)).Handle;
            HwndSource.FromHwnd(mWindowHandle).AddHook(WindowProc);
        }

        private static IntPtr WindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case 0x0024:
                    WmGetMinMaxInfo(hwnd, lParam);
                    break;
            }
            return IntPtr.Zero;
        }

        private static void WmGetMinMaxInfo(IntPtr hwnd, IntPtr lParam)
        {
            POINT lMousePosition;
            GetCursorPos(out lMousePosition);

            var lPrimaryScreen = MonitorFromPoint(new POINT(0, 0), MonitorOptions.MONITOR_DEFAULTTOPRIMARY);
            var lPrimaryScreenInfo = new MONITORINFO();
            if (GetMonitorInfo(lPrimaryScreen, lPrimaryScreenInfo) == false)
            {
                return;
            }

            var lCurrentScreen = MonitorFromPoint(lMousePosition, MonitorOptions.MONITOR_DEFAULTTONEAREST);
            var lMmi = (MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(MINMAXINFO));

            if (lPrimaryScreen.Equals(lCurrentScreen))
            {
                lMmi.ptMaxPosition.X = lPrimaryScreenInfo.rcWork.Left;
                lMmi.ptMaxPosition.Y = lPrimaryScreenInfo.rcWork.Top;
                lMmi.ptMaxSize.X = lPrimaryScreenInfo.rcWork.Right - lPrimaryScreenInfo.rcWork.Left;
                lMmi.ptMaxSize.Y = lPrimaryScreenInfo.rcWork.Bottom - lPrimaryScreenInfo.rcWork.Top;
            }
            else
            {
                lMmi.ptMaxPosition.X = lPrimaryScreenInfo.rcMonitor.Left;
                lMmi.ptMaxPosition.Y = lPrimaryScreenInfo.rcMonitor.Top;
                lMmi.ptMaxSize.X = lPrimaryScreenInfo.rcMonitor.Right - lPrimaryScreenInfo.rcMonitor.Left;
                lMmi.ptMaxSize.Y = lPrimaryScreenInfo.rcMonitor.Bottom - lPrimaryScreenInfo.rcMonitor.Top;
            }

            Marshal.StructureToPtr(lMmi, lParam, true);
        }

        #region DLLImports
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr MonitorFromPoint(POINT pt, MonitorOptions dwFlags);

        enum MonitorOptions : uint
        {
            MONITOR_DEFAULTTONULL = 0x00000000,
            MONITOR_DEFAULTTOPRIMARY = 0x00000001,
            MONITOR_DEFAULTTONEAREST = 0x00000002
        }

        [DllImport("user32.dll")]
        static extern bool GetMonitorInfo(IntPtr hMonitor, MONITORINFO lpmi);

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public POINT(int x, int y)
            {
                X = x;
                Y = y;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MINMAXINFO
        {
            public POINT ptReserved;
            public POINT ptMaxSize;
            public POINT ptMaxPosition;
            public POINT ptMinTrackSize;
            public POINT ptMaxTrackSize;
        };

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class MONITORINFO
        {
            public int cbSize = Marshal.SizeOf(typeof(MONITORINFO));
            public RECT rcMonitor = new RECT();
            public RECT rcWork = new RECT();
            public int dwFlags = 0;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left, Top, Right, Bottom;

            public RECT(int left, int top, int right, int bottom)
            {
                Left = left;
                Top = top;
                Right = right;
                Bottom = bottom;
            }
        }
        #endregion

        public SDGuiStrings Strings { get; private set; }
        public bool IsGui { get { return true; } }
    }
}