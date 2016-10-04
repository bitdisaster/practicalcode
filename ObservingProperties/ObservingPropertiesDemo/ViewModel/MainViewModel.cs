using System.ComponentModel;

namespace ObservingPropertiesDemo.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private int _foo;
        private string _bar;

        public int Foo
        {
            get { return _foo; }
            set { _foo = value; RaisePropertyChanged(nameof(Foo)); }
        }

        public string Bar
        {
            get { return _bar; }
            set { _bar = value; RaisePropertyChanged(nameof(Bar)); }

        }

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
