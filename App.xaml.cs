using HandyControl.Data;
using HandyControl.Themes;
using HandyControl.Tools;
using System.Windows;

namespace PhZx
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        internal void UpdateSkin(SkinType skin)
        {
            SharedResourceDictionary.SharedDictionaries.Clear();
            ResourceHelper.GetTheme("AppTheme", Resources).Skin = skin;
            Current.MainWindow?.OnApplyTemplate();
        }
    }

}
