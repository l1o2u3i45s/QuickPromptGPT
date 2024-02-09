using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QuickPromptGPT.Service;
using QuickPromptGPT.Windows;

namespace QuickPromptGPT.ViewModel
{
    public class SettingVIewModel : ObservableObject
    {
        public ICommand ApplyCommand => new AsyncRelayCommand(ApplyAction);
        public ICommand ReleaseCommand => new AsyncRelayCommand(ReleaseAction);

        private readonly GlobalHookService _globalHookService;
        private readonly IGPTService _gptService;

        private readonly GPTChatWindow _gptChatWindow;
        private readonly GPTChatWindowViewModel _gptChatWindowViewModel;


        private string _tokenKey = "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";

        public string TokenKey
        {
            get => _tokenKey; set => SetProperty(ref _tokenKey, value);
        }

        public SettingVIewModel(GlobalHookService globalHookService, IGPTService gptService,
       GPTChatWindow gptChatWindow, GPTChatWindowViewModel gptChatWindowViewModel)
        {
            _globalHookService = globalHookService;
            _gptService = gptService;

            _globalHookService.SetHook();
            _globalHookService.AddKeyAction(Keys.G, TriggerGPT);

            _ = _gptService.Init(_tokenKey);
            _ = _gptService.CreateConversation();

            _gptChatWindow = gptChatWindow;
            _gptChatWindowViewModel = gptChatWindowViewModel;
        }

        private async Task ApplyAction()
        {
            await _gptService.Init(_tokenKey);
        }
        private async Task ReleaseAction()
        {
            _globalHookService.ReleaseHook();
        }

        private async Task TriggerGPT(string copyMessage)
        {

            _gptChatWindowViewModel.CurrentMessage.TextContent = copyMessage;

            _gptChatWindow.Topmost = true;
            _gptChatWindow.Show();
            _gptChatWindow.Activate();

            await _gptChatWindowViewModel.SendGPT();
        }
    }
}
