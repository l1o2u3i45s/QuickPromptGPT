using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private string _name;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public ObservableCollection<DisplayChatMessage> Messages { get; set; } = new ObservableCollection<DisplayChatMessage>();
    }
}
