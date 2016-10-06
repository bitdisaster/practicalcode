using System;

namespace ReactiveListDemo.Model
{
    public class TodoItem
    {
        public string Title { get; set; }

        public DateTime DueDate { get; set; }

        public bool? Done { get; set; }
    }
}
