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
        public static readonly DependencyProperty ShortcutProperty =
                DependencyProperty.Register(nameof(Shortcut), typeof(KeyCombination),
                    typeof(ShortcutBox),
                    new FrameworkPropertyMetadata(
                        default(KeyCombination),
                        FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                        ShortcutChanged));

        public KeyCombination Shortcut
        {
            get => (KeyCombination)GetValue(ShortcutProperty);
            set => SetValue(ShortcutProperty, value);
        }

        public ShortcutBox()
        {
            InitializeComponent();
        }

        private static void ShortcutChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            (sender as ShortcutBox).textBox.Text = args.NewValue.ToString();
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;

            textBox.Text = new KeyCombination(Key.None, Keyboard.Modifiers).ToString();
        }

        private void TextBox_PreviewKeyUp(object sender, KeyEventArgs e)
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
                    textBox.Text = new KeyCombination(Key.None, modifiers).ToString();
                    return;
            }

            Shortcut = new KeyCombination(key, modifiers);
            FocusManager.SetFocusedElement(FocusManager.GetFocusScope(textBox), null);
            Keyboard.ClearFocus();
        }
    }
}
