using System;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace DerivedCollectionDemo1.Model
{
    public class DataSerivce
    {
        private int _numberOfLoadedItems = 0;

        private Subject<TodoItem> _itemObservable = new Subject<TodoItem>();

        public IObservable<TodoItem> Listen()
        {
            return _itemObservable;
        }

        public void Load(int number)
        {
            Task.Run(() =>
            {
                Random rand = new Random();
                int toLoad = _numberOfLoadedItems + number;
                for (int i = _numberOfLoadedItems; i < toLoad; i++)
                {
                    _itemObservable.OnNext(new TodoItem { Title = $"Item {i}", DueDate = DateTime.Now.AddMinutes(rand.Next(0, 1000)), Done = rand.Next(0, 100) % 3 == 0 });
                    _numberOfLoadedItems++;
                }
            });
        }
    }
}
