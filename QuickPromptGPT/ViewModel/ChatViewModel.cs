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

        private ObservableCollection<DisplayConversation> _displayConversations = new ObservableCollection<DisplayConversation>();

        public ObservableCollection<DisplayConversation> DisplayConversations
        {
            get => _displayConversations;
            set => SetProperty(ref _displayConversations, value);
        }

        private DisplayConversation _selectedConversation;

        public DisplayConversation SelectedConversation
        {
            get => _selectedConversation;
            set => SetProperty(ref _selectedConversation, value);
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
            SelectedConversation.Messages.Add(new DisplayChatMessage()
            {
                TextContent = _currentMessage.TextContent
            });


            DisplayChatMessage reponse = new DisplayChatMessage();

            SelectedConversation.Messages.Add(reponse);
            await foreach (var answer in _gptService.Send(_currentMessage.TextContent))
            {
                reponse.TextContent += answer;
            }

            CurrentMessage.TextContent = "";
        }

    }



}
