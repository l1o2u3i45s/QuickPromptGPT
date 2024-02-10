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
    public class ChatViewModel : ObservableObject
    {
        private GPTModel _selectedModel;

        public GPTModel SelectedModel
        {
            get => _selectedModel;
            set => SetProperty(ref _selectedModel, value);
        }

        private DisplayChatMessage _currentMessage = new DisplayChatMessage();

        public DisplayChatMessage CurrentMessage
        {
            get => _currentMessage;
            set => SetProperty(ref _currentMessage, value);
        }

        private ObservableCollection<DisplayChatMessage> _chatMessages = new ObservableCollection<DisplayChatMessage>();

        public ObservableCollection<DisplayChatMessage> ChatMessages
        {
            get => _chatMessages;
            set => SetProperty(ref _chatMessages, value);
        }

        public ICommand SendMessageCommand { get; set; }


        private readonly IGPTService _gptService;

        public ChatViewModel(IGPTService gptService)
        {
            _gptService = gptService;


            SendMessageCommand = new AsyncRelayCommand(SendMessage);
        }


        private async Task SendMessage()
        {
            ChatMessages.Add(new DisplayChatMessage()
            {
                TextContent = _currentMessage.TextContent
            });


            DisplayChatMessage reponse = new DisplayChatMessage();

            ChatMessages.Add(reponse);
            await foreach (var answer in _gptService.Send(_currentMessage.TextContent))
            {
                reponse.TextContent += answer;
            }

            CurrentMessage.TextContent = "";
        }

    }



}
