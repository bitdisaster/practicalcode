using System;

namespace DerivedCollectionFilteringDemo.Model
{
    public class TodoItem
    {
        public string Title { get; set; }

        public DateTime DueDate { get; set; }

        public bool? Done { get; set; }
    }
}
