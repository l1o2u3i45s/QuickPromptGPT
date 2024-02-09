using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QuickPromptGPT.Service;
using QuickPromptGPT.ViewModel;
using QuickPromptGPT.Windows;

namespace QuickPromptGPT
{
    public  class MainViewModel : ObservableObject
    {
        private readonly SettingVIewModel _settingViewModel;

        public SettingVIewModel SettingViewModel => _settingViewModel;

        private readonly  ChatViewModel _chatViewModel;

        public ChatViewModel ChatViewModel => _chatViewModel;


        public MainViewModel(SettingVIewModel settingVIewModel, ChatViewModel chatViewModel)
        {
            _settingViewModel = settingVIewModel;
            _chatViewModel = chatViewModel;
        }

    }
}
