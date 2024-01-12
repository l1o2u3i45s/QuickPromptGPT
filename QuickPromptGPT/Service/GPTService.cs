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
    public interface IGPTService
    {
        Task Init(string tokenkey);

        Task<string> Send(string message);

        Task CreateConversation();
    }

    public class GPTService : IGPTService
    {

        private readonly IOpenAIAPI _openAiapi;

        private Conversation _currentConversation;

        public GPTService(IOpenAIAPI openAiapi)
        {
            _openAiapi = openAiapi;
        }

        public async Task Init(string tokenkey)
        {
            _openAiapi.Auth = tokenkey;
        }

        public async Task<string> Send(string message)
        {
            ChatMessage chat = new ChatMessage();
            chat.TextContent = message;
            _currentConversation.AppendMessage(chat);

            string response = string.Empty;

            await foreach (var res in _currentConversation.StreamResponseEnumerableFromChatbotAsync())
            {
                Console.Write(res);
            }

            //await _currentConversation.StreamResponseFromChatbotAsync(res =>
            //{
            //    response += res;
            //});

            return response;
        }

        public async Task CreateConversation()
        {
            _currentConversation = _openAiapi.Chat.CreateConversation();
        }

    }
}
