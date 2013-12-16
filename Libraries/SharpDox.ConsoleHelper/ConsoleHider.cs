using System;
using System.Runtime.InteropServices;

namespace SharpDox.ConsoleHelper
{
    public static class ConsoleHider
    {
        /// <summary>
        /// Function to get a handle of the console window.
        /// </summary>
        /// <returns>Handle of the console window.</returns>
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();

        /// <summary>
        /// Hide or Show a Window.
        /// </summary>
        /// <param name="hWnd">Windowhandle</param>
        /// <param name="nCmdShow">0 - Hides the window, 5 - Shows the window</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        public static void ShowConsoleWindow()
        {
            var handle = GetConsoleWindow();
            ShowWindow(handle, 5);
        }

        public static void HideConsoleWindow()
        {
            var handle = GetConsoleWindow();
            ShowWindow(handle, 0);
        }
    }
}
