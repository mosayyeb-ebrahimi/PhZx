using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Diagnostics;
using System.Reactive;

namespace PhZx.ViewModels
{
    public class PhotoViewModel : ReactiveObject
    {
        #region Properties
        public string Name { get; set; }
        public string Path { get; set; }
        public string Size { get; set; }
        public string ThumbPath { get; set; }
        [Reactive] public bool Resize { get; set; }
        [Reactive] public bool Rename { get; set; }
        [Reactive] public int Quality { get; set; }
        [Reactive] public int Height { get; set; }
        [Reactive] public int Width { get; set; }
        public ReactiveCommand<Unit, Unit> ShowCommand { get; set; }
        #endregion

        #region Constructors
        public PhotoViewModel()
        {
            ShowCommand = ReactiveCommand.Create(Show);
        }
        #endregion

        #region Methods
        private void Show()
        {
            var proc = new Process
            {
                StartInfo = new(Path)
                {
                    UseShellExecute = true,
                }
            };
            proc.Start();
        }
        #endregion
    }
}