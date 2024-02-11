using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommunityToolkit.Mvvm.ComponentModel;
using OpenAI_API.Chat;

namespace QuickPromptGPT.Model
{
    public class DisplayChatMessage : ObservableObject
    {
        private string _textContent;

        public string TextContent
        {
            get => _textContent;
            set => SetProperty(ref _textContent, value);
        }

       
        public static implicit operator ChatMessage(DisplayChatMessage displayChatMessage)
        {
            ChatMessage chatMessage = new ChatMessage();
            chatMessage.TextContent = displayChatMessage.TextContent;
            return chatMessage;
        }
    }


    public class DisplayConversation : ObservableObject
    {
        private string _id;

        public string ID
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private string _name;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private Conversation _currentConversation;

        public Conversation CurrentConversation
        {
            get => _currentConversation;
            set => SetProperty(ref _currentConversation, value);
        }


        private ObservableCollection<DisplayChatMessage> _chatMessages = new ObservableCollection<DisplayChatMessage>();

        public ObservableCollection<DisplayChatMessage> ChatMessages
        {
            get => _chatMessages;
            set
            {
                SetProperty(ref _chatMessages, value);
            }
        }

        public DisplayConversation(Conversation currentConversation)
        {
            CurrentConversation = currentConversation;
        }


        public void AppendMessage(string message)
        {
            ChatMessage chat = new ChatMessage();
            chat.TextContent = message;
            _currentConversation.AppendMessage(chat);


            ChatMessages.Add(new DisplayChatMessage()
            {
                TextContent = message
            });
        }
    }
}
