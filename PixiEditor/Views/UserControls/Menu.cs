using PixiEditor.Models.Controllers.Commands;
using System.Windows;
using System.Windows.Controls;

namespace PixiEditor.Views.UserControls
{
    internal class Menu : System.Windows.Controls.Menu
    {
        public static readonly DependencyProperty CommandNameProperty =
            DependencyProperty.RegisterAttached(
                "CommandName",
                typeof(string),
                typeof(Menu),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, CommandChanged)
            );

        public static string GetCommandName(UIElement target) => (string)target.GetValue(CommandNameProperty);

        public static void SetCommandName(UIElement target, string value) => target.SetValue(CommandNameProperty, value);

        public static void CommandChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is not string value || sender is not MenuItem item)
            {
                return;
            }

            var command = CommandController.Current.Commands[value];

            item.Command = command.ICommand;
            item.SetBinding(MenuItem.InputGestureTextProperty, ShortcutBinding.GetBinding(command));
        }
    }
}
