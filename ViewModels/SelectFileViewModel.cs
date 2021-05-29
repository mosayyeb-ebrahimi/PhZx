using PhZx.Helpers;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace PhZx.ViewModels
{
    public class SelectFileViewModel : ReactiveObject, IRoutableViewModel, ILoadingViewModel
    {
        #region Properties
        public string UrlPathSegment => "/files";
        public IScreen HostScreen { get; private set; }
        public ReactiveCommand<Unit, Unit> ClearRecentCommand { get; private set; }
        [Reactive] public bool IsLoading { get; set; }
        [Reactive] public string LoadingText { get; set; }
        [Reactive] public ObservableCollection<RecentItemViewModel> RecentItems { get; set; }
        #endregion

        #region Constructors
        public SelectFileViewModel(IScreen screen)
        {
            HostScreen = screen;
            ClearRecentCommand = ReactiveCommand.Create(() =>
            {
                RecentItems.Clear();
                ((AppBootstrapper)HostScreen).Settings.RecentCompressed.Clear();
            });
            GenerateItems(((AppBootstrapper)HostScreen).Settings.RecentCompressed);
        }



        private void GenerateItems(List<string> recentitems)
        {
            RecentItems = new();
            foreach (var path in recentitems)
            {
                if (Directory.Exists(path))
                {
                    RecentItems.Add(new()
                    {
                        Name = Path.GetFileName(path),
                        Path = path,
                        CreateTime = File.GetCreationTime(path).ToShortDateString()
                    });

                }
            }
        }


        #endregion

        #region Methods
        public async Task Navigate(string[] names)
        {
            IsLoading = true;
            ((AppBootstrapper)HostScreen).CanEdit = false;
            ObservableCollection<PhotoViewModel> photos = new();
            var dir = DirectoryHelper.CreateThumbDirectory();
            await Task.Run(() =>
            {
                var i = 1;
                var txt = App.Current.FindResource("CreatingThumb");
                foreach (var name in names)
                {
                    LoadingText = $"{txt} {i}/{names.Length}";
                    var fi = new System.IO.FileInfo(name);
                    using (Image image = Image.FromFile(name))
                    {
                        using var thumb = NetVips.Image.Thumbnail(name, 300, 300);
                        thumb.WriteToFile($"{dir}\\{fi.Name}-thumb.jpg");
                        var photo = new PhotoViewModel
                        {
                            Name = fi.Name,
                            Path = name,
                            Size = fi.Length.ToSize(SizeUnits.MB),
                            ThumbPath = $"{dir}\\{fi.Name}-thumb.jpg",
                            Height = image.Height,
                            Width = image.Width,
                            Quality = 70,
                        };
                        photos.Add(photo);
                    }
                    i++;
                }
            });
            IsLoading = false;
            ((AppBootstrapper)HostScreen).CanEdit = true;
            await HostScreen.Router.NavigateAndReset.Execute(new MainViewModel(HostScreen, photos));
        }

        public static bool IsValidData(string[] names)
        {
            bool isValid = true;
            string[] formats = { ".jpeg", ".png", ".jpg" };
            foreach (var name in names)
            {
                var ext = Path.GetExtension(name).ToLower();
                if (!formats.Contains(ext))
                {
                    isValid = false;
                    break;
                }
            }
            return isValid;
        }
        #endregion
    }
}
