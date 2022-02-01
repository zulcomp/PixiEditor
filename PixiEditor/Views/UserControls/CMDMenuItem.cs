using PixiEditor.Models.Controllers.Commands;
using System.Windows;
using System.Windows.Controls;

namespace PixiEditor.Views.UserControls
{
    /// <summary>
    /// Interaction logic for CMDMenuItem.xaml
    /// </summary>
    public partial class CMDMenuItem : ContentControl
    {
        private readonly MenuItem item;
        private Command command;

        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register(nameof(Header), typeof(string), typeof(CMDMenuItem), new(null, HeaderUpdated));

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(nameof(Command), typeof(string), typeof(CMDMenuItem), new(null, CommandUpdated));

        public string Header
        {
            get => (string)GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }

        public string Command
        {
            get => (string)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public CMDMenuItem()
        {
            Content = item = new MenuItem();
            UpdateHeader();
        }

        private static void HeaderUpdated(DependencyObject obj, DependencyPropertyChangedEventArgs args) =>
            (obj as CMDMenuItem).UpdateHeader();

        private static void CommandUpdated(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var menuItem = obj as CMDMenuItem;

            if (menuItem.command != null)
            {
                menuItem.command.ShortcutChanged -= ShortcutUpdated;
            }

            menuItem.command = CommandController.Current.Commands[args.NewValue as string];

            menuItem.UpdateHeader();
            menuItem.item.InputGestureText = menuItem.command.Shortcut.ToString();
            menuItem.item.Command = CommandBinding.GenerateICommand(menuItem.command);

            void ShortcutUpdated(Command command, CommandShortcutChangedEventArgs args)
            {
                menuItem.item.InputGestureText = args.NewShortcut.ToString();
            }
        }

        private void UpdateHeader()
        {
            item.Header = Header ?? command?.Display;
        }
    }
}
