using Microsoft.Extensions.DependencyInjection;
using PixiEditor.Helpers;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace PixiEditor.Models.Controllers.Commands
{
    [MarkupExtensionReturnType(typeof(Binding))]
    public class ShortcutBinding : MarkupExtension
    {
        public string Name { get; set; }

        public ShortcutBinding() { }

        public ShortcutBinding(string name) => Name = name;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (DesignerProperties.GetIsInDesignMode(serviceProvider.GetService<IProvideValueTarget>().TargetObject as DependencyObject))
            {
                var attribute = DesignCommandHelpers.GetCommandAttribute(Name);
                return new KeyCombination(attribute.Key, attribute.Modifiers).ToString();
            }

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