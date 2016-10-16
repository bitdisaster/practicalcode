using System;

namespace DerivedCollectionDemo1.Model
{
    public class TodoItem
    {
        public string Title { get; set; }

        public DateTime DueDate { get; set; }

        public bool? Done { get; set; }
    }
}
