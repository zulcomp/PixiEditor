using System;

namespace PixiEditor.Models.Controllers.Commands
{

    [Serializable]
    public class CommandNotFoundException : Exception
    {
        public string Name { get; }

        public CommandNotFoundException(string name) : this(name, GetMessage(name)) { }

        public CommandNotFoundException(string name, string message) : base(message) => Name = name;

        public CommandNotFoundException(string name, Exception inner) : this(name, GetMessage(name), inner) { }

        public CommandNotFoundException(string name, string message, Exception inner) : base(message, inner) => Name = name;

        protected CommandNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }

        private static string GetMessage(string name) => $"The command with the name '{name}' could not be found";
    }
}
