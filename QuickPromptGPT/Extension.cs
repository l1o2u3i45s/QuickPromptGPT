using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickPromptGPT
{
    internal static class Extension
    {
        public static OpenAI_API.Models.Model ToOpenAIModel(this GPTModel gptModel)
        {

            return gptModel switch
            {
                GPTModel.GPT3_5 => OpenAI_API.Models.Model.DefaultChatModel,
                GPTModel.GPT4 => OpenAI_API.Models.Model.GPT4,
                GPTModel.GPT4_Turbo => OpenAI_API.Models.Model.GPT4_Turbo,
                _ => throw new NotImplementedException()
            };

        }
    }
}
