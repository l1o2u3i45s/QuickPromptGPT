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
                GPTModel.DALL_E => OpenAI_API.Models.Model.DALLE3,
                _ => throw new NotImplementedException()
            };

        }
    }
}
