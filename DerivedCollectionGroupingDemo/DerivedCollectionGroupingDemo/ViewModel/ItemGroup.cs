using ReactiveUI;

namespace DerivedCollectionGroupingDemo.ViewModel
{
    public class ItemGroup : ReactiveObject
    {
        private IReactiveDerivedList<TodoItemViewModel> _items;
        private string _title;

        public string Title
        {
            get { return _title; }
            set { this.RaiseAndSetIfChanged(ref _title, value); }
        }


        public IReactiveDerivedList<TodoItemViewModel> Items
        {
            get { return _items; }
            set { this.RaiseAndSetIfChanged(ref _items, value); }
        }
    }
}
