using PixiEditor.Models.Controllers.Commands;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PixiEditor.Views.UserControls
{
    /// <summary>
    /// Interaction logic for ShortcutBox.xaml
    /// </summary>
    public partial class ShortcutBox : UserControl
    {
        public static readonly DependencyProperty HotkeyProperty =
                DependencyProperty.Register(nameof(Shortcut), typeof(KeyCombination),
                    typeof(ShortcutBox),
                    new FrameworkPropertyMetadata(
                        default(KeyCombination),
                        FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public KeyCombination Shortcut
        {
            get => (KeyCombination)GetValue(HotkeyProperty);
            set => SetValue(HotkeyProperty, value);
        }

        public ShortcutBox()
        {
            InitializeComponent();
        }

        private void HotkeyTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;

            var modifiers = Keyboard.Modifiers;
            var key = e.Key;

            if (key == Key.System)
            {
                key = e.SystemKey;
            }

            switch (key)
            {
                case Key.LeftCtrl:
                case Key.RightCtrl:
                case Key.LeftAlt:
                case Key.RightAlt:
                case Key.LeftShift:
                case Key.RightShift:
                case Key.LWin:
                case Key.RWin:
                case Key.Clear:
                case Key.OemClear:
                case Key.Apps:
                    return;
            }

            // Update the value
            Shortcut = new KeyCombination(key, modifiers);
        }
    }
}
