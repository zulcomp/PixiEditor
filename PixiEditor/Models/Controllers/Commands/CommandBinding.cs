using Microsoft.Extensions.DependencyInjection;
using PixiEditor.Helpers;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using System.Xaml;

namespace PixiEditor.Models.Controllers.Commands
{
    [MarkupExtensionReturnType(typeof(ICommand))]
    public class CommandBinding : MarkupExtension
    {
        public string Name { get; set; }

        public CommandBinding() { }

        public CommandBinding(string name) => Name = name;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (DesignerProperties.GetIsInDesignMode(serviceProvider.GetService<IProvideValueTarget>().TargetObject as DependencyObject))
            {
                DesignCommandHelpers.GetCommandAttribute(Name);
                return new ProvidedICommand(null);
            }

            return GenerateICommand(Name);
        }

        public static ICommand GenerateICommand(string name) => GenerateICommand(CommandController.Current.Commands[name]);

        public static ICommand GenerateICommand(Command command) => new ProvidedICommand(command);

        private class ProvidedICommand : ICommand
        {
            public event EventHandler CanExecuteChanged
            {
                add => CommandManager.RequerySuggested += value;
                remove => CommandManager.RequerySuggested -= value;
            }

            private readonly Command _command;

            public ProvidedICommand(Command command)
            {
                _command = command;
            }

            public bool CanExecute(object parameter) => _command.CanExecute();

            public void Execute(object parameter) => _command.Execute();
        }
    }
}
