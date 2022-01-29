using PixiEditor.Helpers;
using System;
using System.Windows;
using System.Windows.Input;

namespace PixiEditor.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();

            SourceInitialized += SettingsWindow_SourceInitialized;
        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CommandBinding_Executed_Close(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.CloseWindow(this);
        }

        private void SettingsWindow_SourceInitialized(object sender, EventArgs e)
        {
            this.AddMaxSizeHook();
        }
    }
}
