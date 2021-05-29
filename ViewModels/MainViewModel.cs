using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Layout;
using PhZx.Helpers;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Compression;
using System.Reactive;
using System.Threading.Tasks;
using geom = iText.Kernel.Geom;

namespace PhZx.ViewModels
{
    public class MainViewModel : ReactiveObject, IRoutableViewModel, ILoadingViewModel
    {

        #region Properties
        public string UrlPathSegment => "/main";

        public IScreen HostScreen { get; private set; }
        public ObservableCollection<PhotoViewModel> Photos { get; set; }
        [Reactive] public PhotoViewModel SelectedPhoto { get; set; }
        [Reactive] public bool IsLoading { get; set; }
        [Reactive] public string LoadingText { get; set; }
        //GS
        [Reactive] public bool Rename { get; set; }
        [Reactive] public bool GenerateZipFile { get; set; } = true;
        [Reactive] public bool GeneratePdfFile { get; set; } = true;
        [Reactive] public string WorkName { get; set; }
        [Reactive] public string Location { get; set; }
        [Reactive] public string NewName { get; set; }
        // TDP - https://github.com/HandyOrg/HandyControl/issues/691
        [Reactive] public string NewNameError { get; set; }
        [Reactive] public bool NewNameHasError { get; set; }
        [Reactive] public string WorkNameError { get; set; }
        [Reactive] public bool WorkNameHasError { get; set; }
        public ReactiveCommand<Unit, Unit> CompressCommand { get; private set; }
        #endregion

        #region Constructors
        public MainViewModel(IScreen screen, ObservableCollection<PhotoViewModel> photos)
        {
            HostScreen = screen;
            Photos = photos;
            SelectedPhoto = photos[0];
            CompressCommand = ReactiveCommand.CreateFromTask(() => Task.Run(Compress));
        }

        #endregion

        #region Methods
        private void Compress()
        {
            if (!Validate()) return;

            IsLoading = true;
            ((AppBootstrapper)HostScreen).CanEdit = false;

            var dir = !string.IsNullOrWhiteSpace(Location) ?
                DirectoryHelper.CreateImageDirectory(Location, WorkName) : DirectoryHelper.CreateImageDirectory(WorkName);
            int c = 0;
            var cmpTxt = App.Current.FindResource("Compressed");
            foreach (var photo in Photos)
            {
                LoadingText = string.Format(cmpTxt + " {0}/{1}", Photos.Count, c);
                var pathToSave = Rename ? Path.Combine(dir, NewName + "_" + c + ".jpg") : Path.Combine(dir, photo.Name);

                if (photo.Resize)
                {
                    using var im = NetVips.Image.Thumbnail(photo.Path, photo.Width, height: photo.Height);
                    im.Jpegsave(pathToSave, photo.Quality, optimizeCoding: true, interlace: true, strip: true);
                }
                else
                {
                    using var im = NetVips.Image.NewFromFile(photo.Path);
                    im.Jpegsave(pathToSave, photo.Quality, optimizeCoding: true, interlace: true, strip: true);
                }
                c++;
            }

            if (GenerateZipFile) CreateZip(dir);

            if (GeneratePdfFile) CreatePdf(dir);
            IsLoading = false;
            ((AppBootstrapper)HostScreen).CanEdit = true;
            ((AppBootstrapper)HostScreen).Settings.RecentCompressed.Add(dir);
            App.Current.Dispatcher.Invoke(() => HostScreen.Router.NavigateAndReset.Execute(new CompletedViewModel(HostScreen, dir)));
        }

        private void CreateZip(string dir)
        {
            LoadingText = "Creating Zip Archive...";
            ZipFile.CreateFromDirectory(dir, dir + ".zip");
            File.Move(dir + ".zip", Path.Combine(dir, WorkName + ".zip"), true);
        }

        private void CreatePdf(string dir)
        {
            LoadingText = "Creating Pdf File";
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(Path.Combine(dir, WorkName + ".pdf")));

            pdfDoc.SetDefaultPageSize(geom.PageSize.A4);

            Document doc = new Document(pdfDoc);
            doc.SetMargins(0, 0, 0, 0);

            foreach (var photo in Photos)
            {
                ImageData imgData = ImageDataFactory.Create(photo.Path);
                var img = new iText.Layout.Element.Image(imgData);
                if (img.GetImageWidth() > img.GetImageHeight())
                {
                    img.SetRotationAngle(-1.5707963268);
                    img.SetWidth(geom.PageSize.A4.GetHeight());
                    img.SetHeight(geom.PageSize.A4.GetWidth());
                }
                else
                {
                    img.SetWidth(geom.PageSize.A4.GetWidth());
                    img.SetHeight(geom.PageSize.A4.GetHeight());
                }

                doc.Add(img);
            }
            doc.Close();
        }

        private bool Validate()
        {
            bool isValid = true;
            if (string.IsNullOrWhiteSpace(WorkName))
            {
                WorkNameHasError = true;
                WorkNameError = App.Current.TryFindResource("EmptyErr") as string;
                isValid = false;
            }
            if (Rename && string.IsNullOrWhiteSpace(NewName))
            {
                NewNameHasError = true;
                NewNameError = App.Current.TryFindResource("EmptyErr") as string;
                isValid = false;
            }
            return isValid;
        }
        #endregion
    }
}
