using DerivedCollectionDemo1.Model;
using ReactiveUI;
using System;
using System.Reactive.Linq;

namespace DerivedCollectionDemo1.ViewModel
{
    public class MainViewModel : ReactiveObject
    {
        private readonly DataSerivce _dataService;
        private ReactiveList<TodoItem> _rootList;
        private IReactiveDerivedList<TodoItem> _items;
        private int _count;


        public IReactiveDerivedList<TodoItem> Items
        {
            get { return _items; }
            set { this.RaiseAndSetIfChanged(ref _items, value); }
        }


        public int Count
        {
            get { return _count; }
            set { this.RaiseAndSetIfChanged(ref _count, value); }
        }


        public MainViewModel()
        {
            _dataService = new DataSerivce();
            _rootList = new ReactiveList<TodoItem>();

            Items = _rootList.CreateDerivedCollection(x => x, x => true, (x, y) => x.DueDate.CompareTo(y.DueDate));

            // Subscribe to the count changed observable so we can update our Count property.
            Items.CountChanged.Subscribe(x => Count = x);


            _dataService.Listen()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(x =>
                {
                    _rootList.Add(x);
                });


            Load(10);
        }

        public void Load(int desiredNumber)
        {
            _dataService.Load(desiredNumber);
        }
    }
}
