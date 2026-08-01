using System.ComponentModel;
using System.Windows;
using 撈金魚.FileManager;
using 撈金魚.Updater;

namespace 撈金魚.UserInterface
{
    public partial class UpdateSettingsWindow : Window
    {
        private readonly AllSettings user_settings;
        private ManifestEntry found_update;

        public UpdateSettingsWindow(AllSettings user_settings)
        {
            InitializeComponent();
            this.user_settings = user_settings;
            auto_update.IsChecked = user_settings.AutoUpdate;
            current_version_text.Text = "目前版本：" + AppVersion.Current;
        }

        private void ClosingAction(object sender, CancelEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }

        private void AutoUpdateChanged(object sender, RoutedEventArgs e)
        {
            user_settings.AutoUpdate = auto_update.IsChecked ?? false;
        }

        private void CheckUpdateButtonClick(object sender, RoutedEventArgs e)
        {
            status_text.Text = "檢查中...";
            update_choice_panel.Visibility = Visibility.Collapsed;
            found_update = UpdateManager.CheckForUpdate();
            if (found_update == null)
            {
                status_text.Text = "已是最新版本";
            }
            else
            {
                status_text.Text = $"發現新版本 {found_update.Version}";
                update_choice_panel.Visibility = Visibility.Visible;
            }
        }

        private void UpdateNowButtonClick(object sender, RoutedEventArgs e)
        {
            if (found_update == null)
                return;

            //mirror MainWindow's ClosingAction so settings are persisted before the app is replaced
            UserSettings.Save(user_settings);
            UpdateManager.DownloadAndInstall(found_update.Url, true);
            System.Environment.Exit(0);
        }

        private void UpdateOnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            if (found_update == null)
                return;

            UpdateManager.SchedulePendingUpdate(found_update.Version, found_update.Url);
            status_text.Text = $"將在關閉程式時更新至 {found_update.Version}";
            update_choice_panel.Visibility = Visibility.Collapsed;
        }

        private void SkipUpdateButtonClick(object sender, RoutedEventArgs e)
        {
            found_update = null;
            UpdateManager.ClearPendingUpdate();
            status_text.Text = "已略過此版本";
            update_choice_panel.Visibility = Visibility.Collapsed;
        }
    }
}
