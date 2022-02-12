using PixiEditor.Models.Controllers.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PixiEditor.Helpers
{
    /// <summary>
    /// Helps with debugging when using XAML
    /// </summary>
    public static class DesignCommandHelpers
    {
        private static IEnumerable<Commands.CommandAttribute> _commands;

        public static Commands.CommandAttribute GetCommandAttribute(string name)
        {
            if (_commands == null)
            {
                _commands = Assembly
                    .GetAssembly(typeof(CommandController))
                    .GetTypes()
                    .SelectMany(x => x.GetProperties())
                    .SelectMany(x => x.GetCustomAttributes<Commands.CommandAttribute>());
            }

            var command = _commands.SingleOrDefault(x => x.Name == name);

            if (command == null)
            {
                throw new CommandNotFoundException(name);
            }

            return command;
        }
    }
}
