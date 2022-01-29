using PixiEditor.Models.Controllers.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace PixiEditor.ViewModels.SubViewModels.UserPreferences.Settings
{
    public class KeybindSettings : SettingsGroup
    {
        private readonly CommandController _commandController;

        private string searchTerm;

        public string SearchTerm
        {
            get => searchTerm;
            set
            {
                SetProperty(ref searchTerm, value);
                UpdateTextSearchResults();
            }
        }

        public KeyCombination SearchShortcut { get; set; }

        public ObservableCollection<Command> Commands { get; set; }

        public KeybindSettings()
        {
            _commandController = CommandController.Current;
            Commands = new();
            UpdateTextSearchResults();
        }

        private void UpdateTextSearchResults()
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                UpdateSearchResults(x => x);
                return;
            }

            UpdateSearchResults(x => x.Where(x => x.Display.Contains(searchTerm)));
        }

        private void UpdateSearchResults(Func<IEnumerable<Command>, IEnumerable<Command>> commands)
        {
            Commands.Clear();

            foreach (var command in commands(_commandController.Commands.Where(x => string.IsNullOrWhiteSpace(x.Display))))
            {
                Commands.Add(command);
            }
        }
    }
}
