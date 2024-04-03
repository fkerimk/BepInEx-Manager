using System.Diagnostics;
using System.Runtime.InteropServices;

namespace BepInExManager;

using Tools;

internal static class Program {

    [DllImport("kernel32.dll")]
    static extern IntPtr GetConsoleWindow();

    [DllImport("user32.dll")]
    static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    [STAThread]
    static void Main (params string[] args) {

        if (ProcessTools.IsInExplorer()) {

            var handle = GetConsoleWindow();

            ShowWindow(handle, 0);

            Thread.Sleep(1);

            ApplicationConfiguration.Initialize();
            Application.Run(new Home());

        } else {

            Console.WriteLine(":)");
        }
    }
}