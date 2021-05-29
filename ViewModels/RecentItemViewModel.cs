using ReactiveUI;
using System.Diagnostics;
using System.Reactive;

namespace PhZx.ViewModels
{
    public class RecentItemViewModel : ReactiveObject
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string CreateTime { get; set; }
        public ReactiveCommand<Unit, Unit> OpenCommand { get; private set; }

        public RecentItemViewModel()
        {
            OpenCommand = ReactiveCommand.Create(() =>
            {
                var proc = new Process
                {
                    StartInfo = new(Path)
                    {
                        UseShellExecute = true,
                    }
                };
                proc.Start();
            });
        }


    }
}
