using Microsoft.Win32;
using PhZx.ViewModels;
using ReactiveUI;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;

namespace PhZx.Views
{
    /// <summary>
    /// Interaction logic for SelectFileView.xaml
    /// </summary>
    public partial class SelectFileView : UserControl, IViewFor<SelectFileViewModel>
    {
        public SelectFileView()
        {
            InitializeComponent();
            this.WhenActivated((d) =>
            {
                this.WhenAnyValue(x => x.ViewModel).BindTo(this, x => x.DataContext).DisposeWith(d);
            });
        }



        public SelectFileViewModel ViewModel
        {
            get { return (SelectFileViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(SelectFileViewModel), typeof(SelectFileView), new PropertyMetadata(null));

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (SelectFileViewModel)value;
        }

        private void Rectangle_DragOver(object sender, DragEventArgs e)
        {
            bool isValid;
            if (e.Data.GetDataPresent(DataFormats.FileDrop, true))
            {
                string[] names = e.Data.GetData(DataFormats.FileDrop, true) as string[];
                isValid = SelectFileViewModel.IsValidData(names);
            }
            else
                isValid = false;

            e.Effects = isValid ? DragDropEffects.Copy : DragDropEffects.None;
            e.Handled = true;
        }

        private async void Rectangle_Drop(object sender, DragEventArgs e)
        {
            string[] files = e.Data.GetData(DataFormats.FileDrop, true) as string[];
            await ViewModel.Navigate(files);
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var fd = new OpenFileDialog
            {
                Filter = "Images|*.jpg;*.jpeg;*.png",
                Multiselect = true,
                Title = "Select Images"
            };
            if (fd.ShowDialog() == true)
            {
                await ViewModel.Navigate(fd.FileNames);
            }
        }

    }
}
