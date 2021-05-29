using PhZx.ViewModels;
using ReactiveUI;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;

namespace PhZx.Views
{
    /// <summary>
    /// Interaction logic for CompletedView.xaml
    /// </summary>
    public partial class CompletedView : UserControl, IViewFor<CompletedViewModel>
    {
        public CompletedView()
        {
            InitializeComponent();
            this.WhenActivated((d) =>
            {
                this.WhenAnyValue(x => x.ViewModel).BindTo(this, x => x.DataContext).DisposeWith(d);
                this.BindCommand(ViewModel, vm => vm.GoBackCommand, v => v.GoBackBtn).DisposeWith(d);
            });
        }

        public CompletedViewModel ViewModel
        {
            get { return (CompletedViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(CompletedViewModel), typeof(CompletedView), new PropertyMetadata(null));

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (CompletedViewModel)value;
        }
    }
}
