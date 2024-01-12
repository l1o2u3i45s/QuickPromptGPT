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
using QuickPromptGPT.Service;

namespace QuickPromptGPT.ViewModel
{
    public class GPTChatWindowViewModel : ObservableObject
    {
        private readonly IGPTService _gptService;

        private ObservableCollection<ChatMessage> _chatMessages = new ObservableCollection<ChatMessage>();

        public ObservableCollection<ChatMessage> ChatMessages
        {
            get => _chatMessages;
            set => SetProperty(ref _chatMessages, value);
        }

        private ChatMessage _currentMessage = new ChatMessage();

        private string _currentMessageInput;

        public string CurrentMessageInput
        {
            get => _currentMessageInput;
            set
            {
                SetProperty(ref _currentMessageInput, value);
                _currentMessage.TextContent = value;
            }
        }


        public ICommand SendMessageCommand { get; set; }

        public GPTChatWindowViewModel(IGPTService gptService)
        {
            _gptService = gptService;
            SendMessageCommand = new AsyncRelayCommand(SendGPT);
        }


        public async Task SendGPT()
        {
            ChatMessage message = _currentMessage;
            var response = await _gptService.Send(message.TextContent);


            ChatMessage responseMessage = new ChatMessage();
            // responseMessage.TextContent = response;

        }


    }
}
