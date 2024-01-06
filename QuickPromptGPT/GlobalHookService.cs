using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;
namespace QuickPromptGPT
{



    public class GlobalHookService
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private static LowLevelKeyboardProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;

        private Dictionary<Keys, Action> _keyActions = new Dictionary<Keys, Action>();

        public GlobalHookService()
        {
            _keyActions.Add(Keys.G, () => Console.WriteLine("a"));
        }

        public void SetHook()
        {
            _hookID = SetWindowsHookEx(WH_KEYBOARD_LL, _proc,
                GetModuleHandle(null), 0);
        }

        public void ReleaseHook()
        {
            UnhookWindowsHookEx(_hookID);
        }

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                if ((Control.ModifierKeys & Keys.Control) != 0 && vkCode == (int)Keys.G)
                {
                    //Console.WriteLine("a");
                    // Ctrl + G was pressed

                    var tmpClipboard = System.Windows.Clipboard.GetDataObject();

                    System.Windows.Clipboard.Clear();

                    // I think a small delay will be more safe.
                    // You could remove it, but be careful.

                    // Send Ctrl+C, which is "copy"
                   SendKeys.SendWait("^c");

                    // Same as above. But this is more important.
                    // In some softwares like Word, the mouse double click will not select the word you clicked immediately.
                    // If you remove it, you will not get the text you selected.

                    if (System.Windows.Clipboard.ContainsText())
                    {
                        string text = System.Windows.Clipboard.GetText();

                        // Your code

                    }
                }
            }

            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
    }
}
