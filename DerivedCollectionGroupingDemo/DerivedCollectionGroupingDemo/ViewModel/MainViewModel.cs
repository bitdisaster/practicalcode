using DerivedCollectionGroupingDemo.Model;
using ReactiveUI;
using System;
using System.Reactive.Linq;

namespace DerivedCollectionGroupingDemo.ViewModel
{
    public class MainViewModel : ReactiveObject
    {
        private readonly DataService _dataService;
        private ReactiveList<TodoItemViewModel> _rootList;
        private ReactiveList<ItemGroup> _groups;
        private int _count;


        public ReactiveList<ItemGroup> Groups
        {
            get { return _groups; }
            set { this.RaiseAndSetIfChanged(ref _groups, value); }
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

            Groups = new ReactiveList<ItemGroup>();
            
            Groups.Add(new ItemGroup { Title = "Todo", Items = _rootList.CreateDerivedCollection(x => x, x => x.IsDone == false, (x, y) => x.Item.DueDate.CompareTo(y.Item.DueDate)) });
            Groups.Add(new ItemGroup { Title = "Completed Tasks", Items = _rootList.CreateDerivedCollection(x => x, x => x.IsDone == true, (x, y) => x.Item.DueDate.CompareTo(y.Item.DueDate)) });


            // Subscribe to the count changed observable so we can update our Count property.
            _rootList.CountChanged.Subscribe(x => Count = x);


            _dataService.Listen()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(x =>
                {
                    _rootList.Add(new TodoItemViewModel(x));
                });


            Load(10);
        }


        public void Load(int desiredNumber)
        {
            _dataService.Load(desiredNumber);
        }
    }
}
