using DerivedCollectionGroupingDemo.Model;
using ReactiveUI;
using System;

namespace DerivedCollectionGroupingDemo.ViewModel
{
    public class TodoItemViewModel : ReactiveObject
    {
        public TodoItem Item { get; set; }

        public bool? IsDone
        {
            get { return Item.Done; }
            set { Item.Done = value; this.RaisePropertyChanged(); }
        }


        public TodoItemViewModel(TodoItem item)
        {
            Item = item;
        }
    }
}
