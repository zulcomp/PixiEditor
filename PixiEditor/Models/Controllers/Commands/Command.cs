using PixiEditor.Helpers;
using System;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;

#pragma warning disable SA1402 // File may only contain a single type
namespace PixiEditor.Models.Controllers.Commands
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public abstract class Command : NotifyableObject
    {
        private KeyCombination shortcut;

        private string DebuggerDisplay
        {
            get
            {
                StringBuilder builder = new($"{Display} ({Name})");

                if (Shortcut != KeyCombination.None)
                {
                    builder.Append(' ');
                    builder.Append(Shortcut);
                }

                return builder.ToString();
            }
        }

        public string Name { get; init; }

        public string Display { get; init; }

        public KeyCombination Shortcut
        {
            get => shortcut;
            set
            {
                KeyCombination old = shortcut;
                if (SetProperty(ref shortcut, value))
                {
                    ShortcutChanged?.Invoke(this, new CommandShortcutChangedEventArgs() { OldShortcut = old, NewShortcut = value });
                }
            }
        }

        public event CommandShortcutChanged ShortcutChanged;

        public ICommand ICommand { get; init; }

        protected Command(string name, string display, KeyCombination shortcut, ICommand command)
        {
            Name = name;
            Display = display;
            Shortcut = shortcut;
            ICommand = command;
        }

        public abstract void Execute();

        public abstract bool CanExecute();

        public override string ToString() => Display;
    }

    public delegate void CommandShortcutChanged(Command command, CommandShortcutChangedEventArgs args);

    public class CommandShortcutChangedEventArgs : EventArgs
    {
        public KeyCombination OldShortcut { get; init; }

        public KeyCombination NewShortcut { get; init; }
    }

    public class BasicCommand : Command
    {
        public object Parameter { get; init; }

        public BasicCommand(string name, string display, object parameter, ICommand command, KeyCombination shortcut = default)
            : base(name, display, shortcut, command)
        {
            Parameter = parameter;
        }

        public override void Execute()
        {
            if (CanExecute())
            {
                ICommand.Execute(Parameter);
            }
        }

        public override bool CanExecute() => ICommand.CanExecute(Parameter);
    }

    public class FactoryCommand : Command
    {
        public Func<Command, object> ParameterFactory { get; init; }

        public FactoryCommand(string name, string display, Func<Command, object> parameterFactory, ICommand command, KeyCombination shortcut = default)
            : base(name, display, shortcut, command)
        {
            ParameterFactory = parameterFactory;
        }

        public FactoryCommand(string name, string display, Func<object> parameterFactory, ICommand command, KeyCombination shortcut = default)
            : this(name, display, _ => parameterFactory(), command, shortcut)
        {
        }

        public override void Execute()
        {
            object parameter = ParameterFactory(this);

            if (ICommand.CanExecute(parameter))
            {
                ICommand.Execute(parameter);
            }
        }

        public override bool CanExecute() => ICommand.CanExecute(ParameterFactory(this));
    }
}
