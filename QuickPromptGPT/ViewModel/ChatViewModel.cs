using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace QuickPromptGPT.ViewModel
{
    public class ChatViewModel : ObservableObject
    {
        private GPTModel _selectedModel;

        public GPTModel SelectedModel
        {
            get => _selectedModel;
            set => SetProperty(ref _selectedModel, value);
        }



        public ChatViewModel()
        {
        }

    }



}
