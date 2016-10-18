using DerivedCollectionFilteringDemo2.Model;
using ReactiveUI;
using System;

namespace DerivedCollectionFilteringDemo2.ViewModel
{
    public class TodoItemViewModel : ReactiveObject
    {
        private bool _isFilteredOut;


        public TodoItem Item { get; set; }


        public bool IsFilteredOut
        {
            get { return _isFilteredOut; }
            set { this.RaiseAndSetIfChanged(ref _isFilteredOut, value); }
        }


        public TodoItemViewModel(TodoItem item, IObservable<Predicate<TodoItem>> filter)
        {
            Item = item;

            filter.Subscribe(x =>
            {
                IsFilteredOut = x(Item);
            });
        }
    }

}
