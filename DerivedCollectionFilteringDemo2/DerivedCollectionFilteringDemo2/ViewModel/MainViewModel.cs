using DerivedCollectionFilteringDemo2.Model;
using ReactiveUI;
using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace DerivedCollectionFilteringDemo2.ViewModel
{
    public class MainViewModel : ReactiveObject
    {
        private readonly DataService _dataService;
        private ReactiveList<TodoItemViewModel> _rootList;
        private IReactiveDerivedList<TodoItemViewModel> _items;
        private int _count;


        private bool _showTodoOnly;

        public bool ShowTodoOnly
        {
            get { return _showTodoOnly; }
            set { this.RaiseAndSetIfChanged(ref _showTodoOnly, value); }
        }


        private string _filterText;

        public string FilterText
        {
            get { return _filterText; }
            set { this.RaiseAndSetIfChanged(ref _filterText, value); }
        }


        public Subject<Predicate<TodoItem>> FilterObservable { get; set; } = new Subject<Predicate<TodoItem>>();

        public IReactiveDerivedList<TodoItemViewModel> Items
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
            _dataService = new DataService();
            _rootList = new ReactiveList<TodoItemViewModel>();
            _rootList.ChangeTrackingEnabled = true;

            Items = _rootList.CreateDerivedCollection(x => x, x => !x.IsFilteredOut, (x, y) => x.Item.DueDate.CompareTo(y.Item.DueDate));

            this.ObservableForProperty(x => x.ShowTodoOnly)
                .Subscribe(_ =>
                {
                    UpdateFilter();
                });

            this.ObservableForProperty(x => x.FilterText)
                 .Do(x =>
                 {
                     if (string.IsNullOrEmpty(x.Value))
                     {
                         UpdateFilter();
                     }
                 })
                .Where(x => !string.IsNullOrEmpty(x.Value))
                .Throttle(TimeSpan.FromSeconds(1))
                .Subscribe(_ =>
                {
                    UpdateFilter();
                });

            // Subscribe to the count changed observable so we can update our Count property.
            Items.CountChanged.Subscribe(x => Count = x);


            _dataService.Listen()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(x =>
                {
                    _rootList.Add(new TodoItemViewModel(x, FilterObservable));
                });


            Load(10);
        }

        private void UpdateFilter()
        {
            if (ShowTodoOnly)
            {
                if (string.IsNullOrEmpty(FilterText))
                {
                    FilterObservable.OnNext(x => x.Done == false);
                }
                else
                {
                    FilterObservable.OnNext(x => x.Done == false || !x.Title.Contains(FilterText));
                }
            }
            else
            {
                if (string.IsNullOrEmpty(FilterText))
                {
                    FilterObservable.OnNext(x => false);
                }
                else
                {
                    FilterObservable.OnNext(x => !x.Title.Contains(FilterText));
                }
            }
        }

        public void Load(int desiredNumber)
        {
            _dataService.Load(desiredNumber);
        }
    }
}
