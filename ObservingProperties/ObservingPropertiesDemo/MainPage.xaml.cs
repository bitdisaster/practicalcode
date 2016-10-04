using ObservingPropertiesDemo.ViewModel;
using ReactiveUI;
using System;
using System.Diagnostics;
using System.Reactive.Linq;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ObservingPropertiesDemo
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


            // The traditional way
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;

            // Simply subscribe to value changes of Foo.
            ViewModel.WhenAnyValue(x => x.Foo)
                .Subscribe(foo =>
            {
                // Do what you need to do with MainViewModel.Foo
                Debug.WriteLine($"Foo is: {foo}");
            });

            // Maybe we are only interested in 
            ViewModel.WhenAnyValue(x => x.Foo)
                .Where(x => x == 14)
                .Subscribe(foo =>
            {
                // Do what you need to do with MainViewModel.Foo
                Debug.WriteLine($"Foo is: {foo}");
            });


            // We can even listen to multiple properties and combine them
            ViewModel.WhenAny(x => x.Foo, x => x.Bar, (f, b) => new Tuple<int, string>(f.Value, b.Value))
                .Subscribe(values =>
                   {
                       Debug.WriteLine($"Values are: Foo:  {values.Item1}, Bar: {values.Item2}");
                   });




            ViewModel.Bar = "Hello";

            for (int i = 0; i < 41; i++)
            {
                ViewModel.Foo = i;
            }
        }


        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.Foo))
            {
                // Do what you need to do with MainViewModel.Foo
            }
        }
    }
}
