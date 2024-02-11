using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using QuickPromptGPT.Model;

namespace QuickPromptGPT.Service
{
    public interface IConversationService
    {
        Task<IEnumerable<DisplayConversation>> GetConversations();

        Task SaveConversation(IEnumerable<DisplayConversation> conversation);

    }

    public class ConversationService : IConversationService
    {
        private const string fileName = "conversations.json";

        public async Task<IEnumerable<DisplayConversation>> GetConversations()
        {
            using (StreamReader r = new StreamReader(fileName))
            {
                string json = r.ReadToEnd();
                List<DisplayConversation> items = JsonConvert.DeserializeObject<List<DisplayConversation>>(json);
                return items;
            }
        }

        public async Task SaveConversation(IEnumerable<DisplayConversation> conversation)
        {
            string json = JsonConvert.SerializeObject(conversation);
            File.WriteAllText(fileName, json);
        }
    }
}
