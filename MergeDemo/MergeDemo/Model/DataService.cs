using System;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace MergeDemo.Model
{
    public class DataService
    {
        private int _numberOfLoadedItems = 0;

        private Subject<Message> _loadedObservable = new Subject<Message>();
        private Subject<Message> _socketObservable = new Subject<Message>();

        public IObservable<Message> Loaded()
        {
            return _loadedObservable;
        }

        public IObservable<Message> Listen()
        {
            return _socketObservable;
        }


        public void Load(int number)
        {
            Task.Run(async () =>
            {
                for (int i = 0; i < 3; i++)
                {
                    Random rand = new Random();
                    int toLoad = _numberOfLoadedItems + number;
                    for (int j = _numberOfLoadedItems; j < toLoad; j++)
                    {
                        _loadedObservable.OnNext(new Message { Text = $"Message {j} from disk.", Date = DateTime.Now.AddMinutes(-rand.Next(0, 1000)) });
                        _numberOfLoadedItems++;
                    }
                    await Task.Delay(2000);
                }
            });
        }

        public void Connect()
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    _socketObservable.OnNext(new Message { Text = $"Message from socket.", Date = DateTime.Now });
                    await Task.Delay(3000);
                }
            });
        }
    }
}
