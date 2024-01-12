using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OpenAI_API.Chat;
using QuickPromptGPT.Model;
using QuickPromptGPT.Service;

namespace QuickPromptGPT.ViewModel
{
    public class GPTChatWindowViewModel : ObservableObject
    {
        private readonly IGPTService _gptService;

        private ObservableCollection<DisplayChatMessage> _chatMessages = new ObservableCollection<DisplayChatMessage>();

        public ObservableCollection<DisplayChatMessage> ChatMessages
        {
            get => _chatMessages;
            set => SetProperty(ref _chatMessages, value);
        }

        private DisplayChatMessage _currentMessage = new DisplayChatMessage();

        public DisplayChatMessage CurrentMessage
        {
            get => _currentMessage;
            set => SetProperty(ref _currentMessage, value);
        }


        public ICommand SendMessageCommand { get; set; }

        public GPTChatWindowViewModel(IGPTService gptService)
        {
            _gptService = gptService;
            SendMessageCommand = new AsyncRelayCommand(SendGPT);
        }


        public async Task SendGPT()
        {
            ChatMessages.Add(CurrentMessage);
            CurrentMessage = new DisplayChatMessage();

            var response = await _gptService.Send(_currentMessage.TextContent);

            ChatMessages.Add(new DisplayChatMessage() { TextContent = response });
        }
    }
}
