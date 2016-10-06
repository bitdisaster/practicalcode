using ReactiveListDemo.Model;
using ReactiveUI;
using System;
using System.Reactive.Linq;

namespace ReactiveListDemo.ViewModels
{
    public class MainViewModel : ReactiveObject
    {
        private readonly DataSerivce _dataService;
        private ReactiveList<TodoItem> _items;
        private int _count;

        public ReactiveList<TodoItem> Items
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

            Items = new ReactiveList<TodoItem>();

            // Subscribe to the count changed observable so we can update our Count property.
            Items.CountChanged.Subscribe(x => Count = x);

            // Simply listen to each item as they come in
            _dataService.Listen()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(x =>
                {
                    using (Items.SuppressChangeNotifications())
                    {
                        Items.Add(x);
                    }
                });

            // Buffer incoming data and add them as ranges
            //_dataService.Listen()
            //    .Buffer(TimeSpan.FromSeconds(10), 50)
            //    .ObserveOn(RxApp.MainThreadScheduler)
            //    .Subscribe(x =>
            //{
            //    Items.AddRange(x);
            //});

            // Manually handle the suspending and resetting the list
            //_dataService.Listen()
            //    .Buffer(TimeSpan.FromSeconds(3), 500)
            //    .ObserveOn(RxApp.MainThreadScheduler)
            //    .Subscribe(x =>
            //    {
            //        using (Items.SuppressChangeNotifications())
            //        {
            //            foreach (var item in x)
            //            {
            //                Items.Add(item);
            //            }
            //        }
            //        if (x.Any())
            //        {
            //            Items.Reset();
            //        }
            //    });

            Load(10);
        }

        public void Load(int desiredNumber)
        {
            _dataService.Load(desiredNumber);
        }
    }
}
