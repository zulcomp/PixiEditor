using PixiEditor.Helpers;
using PixiEditor.Models.Controllers.Commands;
using PixiEditor.Models.DataHolders;
using PixiEditor.Views.UserControls;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace PixiEditor.ViewModels
{
    public class CommandControlViewModel : ViewModelBase
    {
        private readonly CommandController _commandController;
        private readonly CommandControl _control;

        private string searchTerm;
        private int selectedCommand;

        public string SearchTerm
        {
            get => searchTerm;
            set
            {
                if (SetProperty(ref searchTerm, value))
                {
                    UpdateSearchResults();
                }
            }
        }

        public ObservableCollection<Command> SearchResults { get; }

        public int SelectedCommand { get => selectedCommand; set => SetProperty(ref selectedCommand, value); }

        public RelayCommand SearchBoxKeyDownCommand { get; set; }

        public RelayCommand ExecuteCommand { get; set; }

        public CommandControlViewModel(CommandControl control, CommandController commandController)
        {
            SearchBoxKeyDownCommand = new(SearchBoxKeyDown);
            ExecuteCommand = new(ExeCommand);
            _control = control;
            _commandController = commandController ?? throw new ArgumentNullException(nameof(commandController));
            SearchResults = new();
            SearchTerm = string.Empty;
        }

        public void UpdateSearchResults()
        {
            if (string.IsNullOrWhiteSpace(SearchTerm))
            {
                SelectedCommand = -1;
            }

            SearchResults.Clear();

            if (SearchTerm.StartsWith('#'))
            {
                if (SKColor.TryParse(SearchTerm, out SKColor color))
                {
                    var command = new FactoryCommand(
                        "",
                        $"Set primary color to {color.ToString().ToUpper()}",
                        _ => color,
                        _commandController.Commands["PixiEditor.Colors.SelectColor"].ICommand);

                    SearchResults.Add(command);

                    EnsureIndex();
                    return;
                }
            }

            HandleCommandSearch();
            AddRecentlyOpened();
            EnsureIndex();

            void EnsureIndex()
            {
                if (SearchResults.Count > 0 && SelectedCommand == -1)
                {
                    SelectedCommand = 0;
                }

                SelectedCommand = Math.Min(SearchResults.Count - 1, SelectedCommand);
            }
        }

        private void ExeCommand(object paramter)
        {
            var command = paramter as Command;

            command.Execute();
            _control.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void SearchBoxKeyDown(object parameter)
        {
            var args = parameter as KeyEventArgs;

            if (_commandController.Commands["PixiEditor.Commands.ToggleSearchWindow"].Shortcut == new KeyCombination(args.Key, Keyboard.Modifiers))
            {
                _control.Visibility = System.Windows.Visibility.Collapsed;
            }

            if (SearchResults.Count == 0)
            {
                return;
            }

            args.Handled = HandleTextBoxKey(args.Key);
        }

        private bool HandleTextBoxKey(Key key)
        {
            int index = SelectedCommand;

            if (key == Key.Down)
            {
                index++;
            }
            else if (key == Key.Up)
            {
                index--;
            }
            else if (key == Key.Enter && SearchResults.Count > 0)
            {
                ExeCommand(SearchResults[Math.Max(0, SelectedCommand)]);
                return false;
            }
            else
            {
                return false;
            }

            if (index >= SearchResults.Count)
            {
                index = 0;
            }
            else if (index < 0)
            {
                index = SearchResults.Count - 1;
            }

            SelectedCommand = index;

            return true;
        }

        private void HandleCommandSearch()
        {
            if (string.IsNullOrWhiteSpace(SearchTerm))
            {
                return;
            }

            foreach (var command in _commandController.Commands)
            {
                if (command.Display.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
                {
                    SearchResults.Add(command);
                }

                if (SearchResults.Count > 8)
                {
                    break;
                }
            }
        }

        private void AddRecentlyOpened()
        {
            IEnumerable<RecentlyOpenedDocument> documents = ViewModelMain.Current.FileSubViewModel.RecentlyOpened.Where(x => !x.Corrupt);

            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                documents = documents.Where(x => x.FilePath.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase));
            }

            foreach (var document in documents)
            {
                SearchResults.Add(new BasicCommand(
                    "",
                    $"Open '{document.FullFileName}'",
                    document,
                    _commandController.Commands["PixiEditor.File.OpenRecent"].ICommand));
            }
        }
    }
}
