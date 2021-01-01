﻿using Todo.Shared.Enums;

namespace Todo.Shared.Dto
{
    public class TodoItemDto
    {
        public string ListId { get; set; }
        public string ItemId { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
        public TodoItemPriority Priority { get; set; }
    }
}
