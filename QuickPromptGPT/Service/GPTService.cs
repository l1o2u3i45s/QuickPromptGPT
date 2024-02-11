using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using OpenAI_API;
using OpenAI_API.Chat;
using QuickPromptGPT.Model;

namespace QuickPromptGPT.Service
{
    public interface IGPTService
    {
        Task Init(string tokenkey);

        IAsyncEnumerable<string> Send(string message);

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

        public async IAsyncEnumerable<string> Send(string message)
        {
            ChatMessage chat = new ChatMessage();
            chat.TextContent = message;
            _currentConversation.AppendMessage(chat);

            await foreach (var res in _currentConversation.StreamResponseEnumerableFromChatbotAsync())
            {
                yield return res;
            }
        }

        public async Task CreateConversation()
        {
            _currentConversation = _openAiapi.Chat.CreateConversation();
        }

    }
}
