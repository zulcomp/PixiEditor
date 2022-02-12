using System;
using System.Windows.Data;
using System.Windows.Markup;

namespace PixiEditor.Models.Controllers.Commands
{
    [MarkupExtensionReturnType(typeof(string))]
    public class ShortcutBinding : MarkupExtension
    {
        public string Name { get; set; }

        public ShortcutBinding() { }

        public ShortcutBinding(string name) => Name = name;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            Command command = CommandController.Current?.Commands[Name];

            return new Binding
            {
                Source = command,
                Path = new("Shortcut"),
                Mode = BindingMode.OneWay,
                StringFormat = ""
            }.ProvideValue(serviceProvider);
        }

        public static Binding GetBinding(Command command) => new Binding
        {
            Source = command,
            Path = new("Shortcut"),
            Mode = BindingMode.OneWay,
            StringFormat = ""
        };
    }
}