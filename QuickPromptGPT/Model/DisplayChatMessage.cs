using System;
using System.Collections.Generic;
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
}
