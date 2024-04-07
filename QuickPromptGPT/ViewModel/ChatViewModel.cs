using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using OpenAI_API.Chat;
using QuickPromptGPT.Model;
using QuickPromptGPT.Model.Messenge;
using QuickPromptGPT.Service;

namespace QuickPromptGPT.ViewModel
{
    public class ChatViewModel : ObservableObject
    {
        private GPTModel _selectedModel = GPTModel.GPT3_5;

        public GPTModel SelectedModel
        {
            get => _selectedModel;
            set => SetProperty(ref _selectedModel, value);
        }

        private DisplayChatMessage _currentMessage = new DisplayChatMessage(false);

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
        public ICommand AddConversationCommand { get; set; }

        private readonly IGPTService _gptService;
        private readonly IConversationService _conversationService;

        public ChatViewModel(IGPTService gptService, IConversationService conversationService)
        {
            _gptService = gptService;
            _conversationService = conversationService;

            _ = InitData();

            SendMessageCommand = new AsyncRelayCommand(SendMessage);
            AddConversationCommand = new AsyncRelayCommand(AddConversation);
        }

        private async Task InitData()
        {
            var hisConversation = await _conversationService.GetConversations();
            DisplayConversations = new ObservableCollection<DisplayConversation>(hisConversation);

            if (DisplayConversations.Any() == false)
            {
                var newConversation = new DisplayConversation(await _gptService.CreateConversation(SelectedModel));
                newConversation.Summary = "New Conversation";
                DisplayConversations.Add(newConversation);
            }

            SelectedConversation = DisplayConversations.FirstOrDefault();
        }

        private async Task AddConversation()
        {
            var newConversation = new DisplayConversation(await _gptService.CreateConversation(SelectedModel));
            newConversation.Summary = "New Conversation";
            DisplayConversations.Add(newConversation);
            SelectedConversation = DisplayConversations.LastOrDefault();
        }

      
        private async Task SendMessage()
        {
          
            SelectedConversation.AppendMessage(_currentMessage.TextContent,false);
            SelectedConversation.CurrentConversation.Model = SelectedModel.ToOpenAIModel();
            WeakReferenceMessenger.Default.Send(new ChatViewScrollMessenge());
            CurrentMessage.TextContent = "";

            DisplayChatMessage response = new DisplayChatMessage(true);
            SelectedConversation.AppendMessage(response);
            await foreach (var answer in _gptService.Send(SelectedConversation.CurrentConversation))
            {
                response.TextContent += answer;
                WeakReferenceMessenger.Default.Send(new ChatViewScrollMessenge());
            }
        }

    }



}
