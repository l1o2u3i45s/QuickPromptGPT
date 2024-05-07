using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QuickPromptGPT.Service;
using QuickPromptGPT.Windows;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace QuickPromptGPT.ViewModel
{
    public partial class SettingViewModel : ObservableObject
    {
        public ICommand ApplyCommand => new AsyncRelayCommand(ApplyAction);
        public ICommand ReleaseCommand => new AsyncRelayCommand(ReleaseAction);

        public ICommand SaveCommand => new AsyncRelayCommand(Save);

        private readonly GlobalHookService _globalHookService;
        private readonly CacheService _cacheService;
        private readonly IGPTService _gptService;

        private readonly GPTChatWindow _gptChatWindow;
        private readonly GPTChatWindowViewModel _gptChatWindowViewModel;


        private string _tokenKey ;

        public string TokenKey
        {
            get => _tokenKey;
            set
            {
                SetProperty(ref _tokenKey, value);
                _ = ApplyAction();
            }
        }

        

        public SettingViewModel(GlobalHookService globalHookService, IGPTService gptService,
       GPTChatWindow gptChatWindow, GPTChatWindowViewModel gptChatWindowViewModel, CacheService cacheService)
        {
            _globalHookService = globalHookService;
            _gptService = gptService;
            _cacheService = cacheService;
            TokenKey = _cacheService.GetKey();


            _ = SetupHotKey();
            _ = _gptService.Init(_tokenKey);

            _gptChatWindow = gptChatWindow;
            _gptChatWindowViewModel = gptChatWindowViewModel;
        }

        [RelayCommand]
        private async Task Save()
        {
            _cacheService.SetKey(_tokenKey);
            _cacheService.Save();
        }

        private async Task ApplyAction()
        {
            await _gptService.Init(_tokenKey);
        }
        private async Task ReleaseAction()
        {
            _globalHookService.ReleaseHook();
        }

        private async Task SetupHotKey()
        {
            _globalHookService.SetHook();
            _globalHookService.AddKeyAction(Keys.G, TriggerGPT);
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
