﻿using PixiEditor.Models.Enums;
using PixiEditor.Views;
using System.Drawing.Imaging;
using System.Windows;

namespace PixiEditor.Models.Dialogs
{
    public class ExportFileDialog : CustomDialog
    {
        FileType _chosenFormat;

        private int fileHeight;

        private string filePath;

        private int fileWidth;

        public ExportFileDialog(Size fileDimensions)
        {
            FileHeight = (int)fileDimensions.Height;
            FileWidth = (int)fileDimensions.Width;
        }

        public int FileWidth
        {
            get => fileWidth;
            set
            {
                if (fileWidth != value)
                {
                    fileWidth = value;
                    RaisePropertyChanged("Width");
                }
            }
        }

        public int FileHeight
        {
            get => fileHeight;
            set
            {
                if (fileHeight != value)
                {
                    fileHeight = value;
                    RaisePropertyChanged("FileHeight");
                }
            }
        }

        public string FilePath
        {
            get => filePath;
            set
            {
                if (filePath != value)
                {
                    filePath = value;
                    RaisePropertyChanged("FilePath");
                }
            }
        }

        public FileType ChosenFormat
        {
            get => _chosenFormat;
            set
            {
                if (_chosenFormat != value)
                {
                    _chosenFormat = value;
                    RaisePropertyChanged(nameof(ChosenFormat));
                }
            }
        }

        public override bool ShowDialog()
        {
            ExportFilePopup popup = new ExportFilePopup
            {
                SaveWidth = FileWidth,
                SaveHeight = FileHeight
            };
            popup.ShowDialog();
            if (popup.DialogResult == true)
            {
                FileWidth = popup.SaveWidth;
                FileHeight = popup.SaveHeight;
                FilePath = popup.SavePath;
                ChosenFormat = popup.SaveFormat;
            }

            return (bool)popup.DialogResult;
        }
    }
}
