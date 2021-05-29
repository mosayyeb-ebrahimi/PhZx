using PhZx.Models;
using PhZx.Views;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using System.Linq;

namespace PhZx.ViewModels
{
    public class AppBootstrapper : ReactiveObject, IScreen
    {
        public string AppName => "PhZx";
        public RoutingState Router { get; private set; }
        [Reactive] public bool CanEdit { get; set; } = true;
        public Config Settings { get; set; }
        public AppBootstrapper()
        {
            Router = new RoutingState();
            Settings = new();
            Settings.Lang = Properties.App.Default.lang;
            Settings.Theme = Properties.App.Default.theme;
            if (Properties.App.Default.recent != null)
                Settings.RecentCompressed = Properties.App.Default.recent.Cast<string>().ToList();
            else
                Settings.RecentCompressed = new();

            RegisterParts(Locator.CurrentMutable);
            Router.Navigate.Execute(new SelectFileViewModel(this));
        }

        private void RegisterParts(IMutableDependencyResolver dependencyResolver)
        {
            dependencyResolver.RegisterConstant(this, typeof(IScreen));
            dependencyResolver.Register(() => new MainView(), typeof(IViewFor<MainViewModel>));
            dependencyResolver.Register(() => new SelectFileView(), typeof(IViewFor<SelectFileViewModel>));
            dependencyResolver.Register(() => new CompletedView(), typeof(IViewFor<CompletedViewModel>));
        }
    }
}

