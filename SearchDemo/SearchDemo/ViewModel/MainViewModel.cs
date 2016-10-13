using ReactiveUI;
using SearchDemo.Model;
using System;
using System.Diagnostics;
using System.Reactive.Linq;

namespace SearchDemo.ViewModel
{
    public class MainViewModel : ReactiveObject
    {
        private readonly SearchService _searchService;

        private string _searchTerm;
        private ReactiveList<string> _results;
        private bool _isSearching;

        public string SearchTerm
        {
            get { return _searchTerm; }
            set { this.RaiseAndSetIfChanged(ref _searchTerm, value); }
        }

        public ReactiveList<string> Results
        {
            get { return _results; }
            set { this.RaiseAndSetIfChanged(ref _results, value); }
        }

        public bool IsSearching
        {
            get { return _isSearching; }
            set { this.RaiseAndSetIfChanged(ref _isSearching, value); }
        }

        public MainViewModel()
        {
            _searchService = new SearchService();
            Results = new ReactiveList<string>();

            // Snippet 2
            //this.WhenAnyValue(x => x.SearchTerm)
            //   .Subscribe(async searchTerm =>
            //   {
            //       Results.Clear();

            //       if (!string.IsNullOrEmpty(searchTerm))
            //       {
            //           IsSearching = true;

            //           Debug.WriteLine($"Searching for: {searchTerm}");
            //           var results = await _searchService.Search(searchTerm);

            //           // We might have triggered multiple searches. But we only want the results that match the current search term.
            //           if (results?.Item1 == SearchTerm)
            //           {
            //               Results.AddRange(results.Item2);
            //               IsSearching = false;
            //           }
            //       }

            //       IsSearching = false;
            //   });

            // Snippet 3
            //this.WhenAnyValue(x => x.SearchTerm)
            //    .Throttle(TimeSpan.FromSeconds(1.5), RxApp.MainThreadScheduler)
            //    .Subscribe(async searchTerm =>
            //    {
            //        Results.Clear();
            //        if (!string.IsNullOrEmpty(searchTerm))
            //        {
            //            IsSearching = true;

            //            Debug.WriteLine($"Searching for: {searchTerm}");
            //            var results = await _searchService.Search(searchTerm);

            //            if (results?.Item1 == SearchTerm)
            //            {
            //                Results.AddRange(results.Item2);
            //                IsSearching = false;
            //            }
            //        }

            //        IsSearching = false;
            //    });

            // Snippet 4
            this.WhenAnyValue(x => x.SearchTerm)
                .Do(x =>
                {
                    if (string.IsNullOrEmpty(x))
                    {
                        Results.Clear();
                        IsSearching = false;
                    }
                })
                .Where(x => !string.IsNullOrEmpty(x))
                .Throttle(TimeSpan.FromSeconds(1.5), RxApp.MainThreadScheduler)
                .Subscribe(async searchTerm =>
                {
                    Results.Clear();
                    if (!string.IsNullOrEmpty(searchTerm))
                    {
                        IsSearching = true;

                        Debug.WriteLine($"Searching for: {searchTerm}");
                        var results = await _searchService.Search(searchTerm);

                        if (results?.Item1 == SearchTerm)
                        {
                            Results.AddRange(results.Item2);
                            IsSearching = false;
                        }
                    }

                    IsSearching = false;
                });
        }
    }
}
