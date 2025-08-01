﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asana.Library.Models
{
    public class ToDo
    {
        public ToDo()
        {
            Id = 0;
            IsCompleted = false;
        }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? Priority { get; set; }
        public bool IsCompleted { get; set; } = false;


        public int ProjectId { get; set; }

        public DateTime DueDate { get; set; }



        public int Id { get; set; }

        public override string ToString()
        {
            return $"[{Id}] {Name} - {Description}";
        }
    }

    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double CompletePercent { get; set; }
        public List<int> ToDoIds { get; set; } = new List<int>();
    }

}
