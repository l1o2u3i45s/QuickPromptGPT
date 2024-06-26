﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CommunityToolkit.Mvvm.Messaging;
using OpenAI_API.Chat;
using QuickPromptGPT.Model.Messenge;

namespace QuickPromptGPT.UserControl
{
    /// <summary>
    /// Interaction logic for ChatView_UC.xaml
    /// </summary>
    public partial class ChatView_UC : System.Windows.Controls.UserControl
    {
        public ChatView_UC()
        {
            InitializeComponent();
            WeakReferenceMessenger.Default.Register<ChatViewScrollMessenge>(this, (r, m) =>
            {

                Dispatcher.Invoke(()=>
                {
                    ChatScrollViewer.ScrollToBottom();
                });

            });
        }
    }
}
