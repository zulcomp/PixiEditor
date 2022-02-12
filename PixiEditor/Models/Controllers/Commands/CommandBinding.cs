using System;
using System.Windows.Input;
using System.Windows.Markup;

namespace PixiEditor.Models.Controllers.Commands
{
    [MarkupExtensionReturnType(typeof(ICommand))]
    public class CommandBinding : MarkupExtension
    {
        public string Name { get; set; }

        public CommandBinding() { }

        public CommandBinding(string name) => Name = name;

        public override object ProvideValue(IServiceProvider serviceProvider) => GenerateICommand(Name);

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
