using MergeDemo.Model;
using ReactiveUI;
using System;
using System.Reactive.Linq;

namespace MergeDemo.ViewModel
{
    public class MainViewModel : ReactiveObject
    {
        private readonly DataService _dataService;
        private ReactiveList<Message> _rootList;
        private IReactiveDerivedList<Message> _items;


        public IReactiveDerivedList<Message> Items
        {
            get { return _items; }
            set { this.RaiseAndSetIfChanged(ref _items, value); }
        }


        public MainViewModel()
        {
            _dataService = new DataService();
            _rootList = new ReactiveList<Message>();

            Items = _rootList.CreateDerivedCollection(x => x, x => true, (x, y) => -1 * x.Date.CompareTo(y.Date));

            _dataService.Loaded()
                .Merge(_dataService.Listen())
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(x =>
                {
                    _rootList.Add(x);
                });
        }

        public void Load()
        {
            _dataService.Load(5);
            _dataService.Connect();
        }
    }
}
