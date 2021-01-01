﻿using Todo.Shared.Enums;

namespace Todo.Models
{
    public class TodoItem
    {
        public string Name { get; set; }
        public bool Status { get; set; }
        public TodoItemPriority Priority { get; set; }
    }
}
