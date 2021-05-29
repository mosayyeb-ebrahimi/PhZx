using HandyControl.Data;
using HandyControl.Tools;
using PhZx.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;

namespace PhZx
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : HandyControl.Controls.Window
    {
        public AppBootstrapper AppBootstrapper { get; private set; }
        public MainWindow()
        {
            InitializeComponent();
            this.Closing += MainWindow_Closing; ;
            AppBootstrapper = new AppBootstrapper();
            DataContext = AppBootstrapper;
            SetupSettings();

        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var settings = AppBootstrapper.Settings;
            Properties.App.Default.lang = settings.Lang;
            Properties.App.Default.theme = settings.Theme;
            Properties.App.Default.recent = new();
            Properties.App.Default.recent.AddRange(settings.RecentCompressed.ToArray());
            Properties.App.Default.Save();
        }

        private void SetupSettings()
        {
            var settings = AppBootstrapper.Settings;
            ((App)Application.Current).UpdateSkin((SkinType)settings.Theme);
            UpdateLang(settings.Lang);
            SettingBtn.Click += (s, e) => mPopup.IsOpen = true;

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            var cmb = sender as ComboBox;
            UpdateLang(cmb.SelectedIndex);
            mPopup.IsOpen = false;

        }
        private void UpdateLang(int lang)
        {
            string lcode = lang == 0 ? "en" : "fa";
            App.Current.Resources.MergedDictionaries[0].Source = new Uri($"Resources/Langs/Lang.{lcode}.xaml", UriKind.RelativeOrAbsolute);
            ConfigHelper.Instance.SetLang(lcode);
        }
        private void ThemeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            var cmb = sender as ComboBox;

            var theme = ((ComboBoxItem)cmb.SelectedItem).Tag;
            if (theme is SkinType type)
            {
                ((App)Application.Current).UpdateSkin(type);
            }

            mPopup.IsOpen = false;
        }
    }
}
