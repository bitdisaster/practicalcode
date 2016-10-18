using DerivedCollectionFilteringDemo2.ViewModel;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace DerivedCollectionFilteringDemo2
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainViewModel ViewModel { get; set; }

        public MainPage()
        {
            this.InitializeComponent();
            ViewModel = new MainViewModel();
            DataContext = ViewModel;
        }

        private void button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ViewModel.Load(3);
        }

        private void button_Click2(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ViewModel.Load(100);
        }
    }
}
