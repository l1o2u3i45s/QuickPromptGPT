using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;
namespace QuickPromptGPT
{

    public delegate Task SnippetStringCallback(string copyString);
    public class GlobalHookService
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private static LowLevelKeyboardProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;

        private static Dictionary<Keys, SnippetStringCallback> _keyActions = new Dictionary<Keys, SnippetStringCallback>();
        private static List<Keys> _pressedKeys = new List<Keys>();

        public GlobalHookService()
        {
        }

        public void AddKeyAction(Keys key, SnippetStringCallback action)
        {
            _keyActions.Add(key, action);
            _pressedKeys.Add(key);
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

                bool isPressedControl = (Control.ModifierKeys & Keys.Control) != 0;

                bool foundKey = false;
                Keys currentPressKey = Keys.A;

                foreach (var key in _pressedKeys)
                {
                    if (vkCode == (int)key)
                    {
                        currentPressKey = key;
                        foundKey = true;
                    }
                }
                if (isPressedControl && foundKey)
                {
                    System.Windows.Clipboard.Clear();

                    // I think a small delay will be more safe.
                    // You could remove it, but be careful.
                    Thread.Sleep(50);

                    // Send Ctrl+C, which is "copy"
                    SendKeys.SendWait("^c");


                    Thread.Sleep(50);
                    // Same as above. But this is more important.
                    // In some softwares like Word, the mouse double click will not select the word you clicked immediately.
                    // If you remove it, cyou will not get the text you selected.

                    if (System.Windows.Clipboard.ContainsText())
                    {
                        string text = System.Windows.Clipboard.GetText();

                        if(_keyActions.ContainsKey(currentPressKey))
                            _keyActions[currentPressKey]?.Invoke(text);

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
