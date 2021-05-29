using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Diagnostics;
using System.Reactive;

namespace PhZx.ViewModels
{
    public class CompletedViewModel : ReactiveObject, IRoutableViewModel
    {

        #region Properties
        public string UrlPathSegment => "/completed";
        public IScreen HostScreen { get; private set; }
        [Reactive] public string Path { get; set; }
        public ReactiveCommand<Unit, Unit> GoBackCommand { get; set; }
        public ReactiveCommand<Unit, Unit> OpenInExplorerCommand { get; set; }
        #endregion

        #region Constructors
        public CompletedViewModel(IScreen screen, string path)
        {
            HostScreen = screen;
            Path = path;
            GoBackCommand = ReactiveCommand.Create(GoBack);
            OpenInExplorerCommand = ReactiveCommand.Create(OpenExplorer);
        }
        #endregion

        #region Methods
        private void GoBack()
        {
            HostScreen.Router.NavigateAndReset.Execute(new SelectFileViewModel(HostScreen));
        }

        private void OpenExplorer()
        {
            var proc = new Process
            {
                StartInfo = new()
                {
                    UseShellExecute = true,
                    FileName = Path,
                }
            };
            proc.Start();
        }
        #endregion
    }
}
