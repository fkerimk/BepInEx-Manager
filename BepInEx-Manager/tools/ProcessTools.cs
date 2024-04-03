using System.Diagnostics;
using System.Runtime.InteropServices;

namespace BepInExManager.Tools;

internal static class ProcessTools {

    [DllImport("kernel32.dll")]
    private static extern IntPtr GetConsoleWindow();

    [DllImport("user32.dll")]
    static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

    [DllImport("ntdll.dll")]
    private static extern int NtQueryInformationProcess(IntPtr processHandle, int processInformationClass, ref ProcessBasicInformation processInformation, int processInformationLength, out int returnLength);

    [StructLayout(LayoutKind.Sequential)]
    private struct ProcessBasicInformation {

        public IntPtr Reserved1;
        public IntPtr PebBaseAddress;
        public IntPtr Reserved2_0;
        public IntPtr Reserved2_1;
        public IntPtr UniqueProcessId;
        public IntPtr InheritedFromUniqueProcessId;

        public int Size {

            get { return Marshal.SizeOf(typeof(ProcessBasicInformation)); }
        }
    }

    public static Process GetParentProcess(Process child) {

        var pbi = new ProcessBasicInformation();
        int status = NtQueryInformationProcess(child.Handle, 0, ref pbi, pbi.Size, out _);

        if (status != 0)
            return null;

        try {

            return Process.GetProcessById(pbi.InheritedFromUniqueProcessId.ToInt32());

        } catch (ArgumentException) {

            return null;
        }
    }

    public static void GetProcessId (out uint pid) {

        IntPtr ptr = GetConsoleWindow();
        GetWindowThreadProcessId(ptr, out pid);
    }

    public static bool IsInExplorer () {

        GetProcessId(out uint pid);

        using (Process process = Process.GetProcessById((int)pid)) {

            using (Process parentProcess = GetParentProcess(process)) {

                if (parentProcess != null) {

                    if (parentProcess.ProcessName.EqualsAnyOfIgnoreCase("explorer", "devenv"))
                        return true;

                    else
                        return false;
                }
            }
        }

        return false;
    }
}