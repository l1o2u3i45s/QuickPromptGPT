using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using OpenAI_API;
using OpenAI_API.Chat;

namespace QuickPromptGPT.Service
{
    public class GPTService
    {

        private readonly OpenAIAPI _openAiapi;

        private Conversation _currentConversation;

        public GPTService(OpenAIAPI openAiapi)
        {
            _openAiapi = openAiapi;
        }

        public async Task Init(string tokenkey)
        {
            _openAiapi.Auth = tokenkey;
        }

        public async Task<List<string>> Send(string message)
        {
            ChatMessage chat = new ChatMessage();
            chat.TextContent = message;
            _currentConversation.AppendMessage(chat);

            var responses = new List<string>();
           
            await _currentConversation.StreamResponseFromChatbotAsync(res =>
            {
                responses.Add(res);
            });

            return responses;
        }

        public async Task CreateConversation()
        {
            _currentConversation = _openAiapi.Chat.CreateConversation();
        }

    }
}
