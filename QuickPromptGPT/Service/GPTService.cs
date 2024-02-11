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

        IAsyncEnumerable<string> Send(Conversation conversation);

        Task<Conversation> CreateConversation(GPTModel model);

    }

    public class GPTService : IGPTService
    {

        private readonly IOpenAIAPI _openAiapi;


        public GPTService(IOpenAIAPI openAiapi)
        {
            _openAiapi = openAiapi;
        }

        public async Task Init(string tokenkey)
        {
            _openAiapi.Auth = tokenkey;
        }

        public async IAsyncEnumerable<string> Send(Conversation conversation)
        {
            await foreach (var res in conversation.StreamResponseEnumerableFromChatbotAsync())
            {
                yield return res;
            }
        }

        public async Task<Conversation> CreateConversation(GPTModel model)
        {
            ChatRequest request = new ChatRequest()
            {
                Model = model.ToOpenAIModel()
            };
             return _openAiapi.Chat.CreateConversation(request);
        }

    }

   
}
