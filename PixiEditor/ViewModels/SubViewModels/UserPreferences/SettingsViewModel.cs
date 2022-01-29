using PixiEditor.ViewModels.SubViewModels.UserPreferences.Settings;

namespace PixiEditor.ViewModels.SubViewModels.UserPreferences
{
    public class SettingsViewModel : SubViewModel<SettingsWindowViewModel>
    {
        public GeneralSettings General { get; set; } = new GeneralSettings();

        public FileSettings File { get; set; } = new FileSettings();

        public UpdateSettings Update { get; set; } = new UpdateSettings();

        public DiscordSettings Discord { get; set; } = new DiscordSettings();

        public KeybindSettings Keybinds { get; set; } = new KeybindSettings();

        public SettingsViewModel(SettingsWindowViewModel owner)
            : base(owner)
        {
        }
    }
}