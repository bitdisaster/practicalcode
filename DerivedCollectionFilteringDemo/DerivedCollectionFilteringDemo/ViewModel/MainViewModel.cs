using DerivedCollectionFilteringDemo.Model;
using ReactiveUI;
using System;
using System.Reactive.Linq;

namespace DerivedCollectionFilteringDemo.ViewModel
{
    public class MainViewModel : ReactiveObject
    {
        private readonly DataSerivce _dataService;
        private ReactiveList<TodoItem> _rootList;
        private IReactiveDerivedList<TodoItem> _items;
        private int _count;


        private bool _showTodoOnly;

        public bool ShowTodoOnly
        {
            get { return _showTodoOnly; }
            set { this.RaiseAndSetIfChanged(ref _showTodoOnly, value); }
        }


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

            Items = _rootList.CreateDerivedCollection(x => x, x => !ShowTodoOnly || x.Done == false, (x, y) => x.DueDate.CompareTo(y.DueDate));

            this.ObservableForProperty(x => x.ShowTodoOnly)
                .Subscribe(_ =>
              {
                  Items = _rootList.CreateDerivedCollection(x => x, x => !ShowTodoOnly || x.Done == false, (x, y) => x.DueDate.CompareTo(y.DueDate));
              });

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
