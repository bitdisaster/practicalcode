using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SearchDemo.Model
{
    public class SearchService
    {
        string[] _lookup;
        public Task<Tuple<string, List<string>>> Search(string searchTerm)
        {
            return Task<List<string>>.Run(async () =>
           {
               // pretend the search take long
               await Task.Delay(3000);

               _lookup = _lookup ?? await Load();

               var ret = _lookup.Where(x => x.Contains(searchTerm)).ToList();
               return new Tuple<string, List<string>>(searchTerm, ret);
           });
        }

        private async Task<string[]> Load()
        {
            Uri dataUri = new Uri("ms-appx:///Assets/Dictionary.txt", UriKind.Absolute);
            Windows.Storage.StorageFile file = await Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(dataUri);
            string words = await Windows.Storage.FileIO.ReadTextAsync(file);

            return words.Split("\r\n".ToCharArray());
        }
    }
}
