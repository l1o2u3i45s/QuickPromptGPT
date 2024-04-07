using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace QuickPromptGPT.Model
{
    public class ChatFileInfo : ObservableObject
    {
        private string _filePath;

        public string FilePath
        {
            get => _filePath;
            set => SetProperty(ref _filePath, value);
        }

        private bool _isSelectedFile;

        public bool IsSelectedFile
        {
            get => _isSelectedFile;
            set => SetProperty(ref _isSelectedFile, value);
        }
    }
}
