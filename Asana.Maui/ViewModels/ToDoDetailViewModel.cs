﻿using Asana.Library.Models;
using Asana.Library.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asana.Maui.ViewModels
{
    public class ToDoDetailViewModel
    {
        public ToDoDetailViewModel() {
            Model = new ToDo();
        }

        public ToDoDetailViewModel(int id)
        {
            Model = ToDoServiceProxy.Current.GetById(id) ?? new ToDo();
        }

        public ToDo? Model { get ; set; }

        public List<int> Priorities
        {
            get
            {
                return new List<int> { 0, 1, 2, 3, 4 };
            }
        }

        public int SelectedPriority { 
            get
            {
                return Model?.Priority ?? 4;
            }
            set
            {
                if (Model != null && Model.Priority != value)
                {
                    Model.Priority = value;
                }
            }
        }

        public void AddOrUpdateToDo()
        {
            ToDoServiceProxy.Current.AddOrUpdate(Model);
        }

        //This is option 1 to fix the UX issue with Priority
        public string PriorityDisplay
        {
            set
            {
                if(Model == null)
                {
                    return;
                }

                if (!int.TryParse(value, out int p))
                {
                    Model.Priority = -9999;
                }
                else
                {
                    Model.Priority = p;
                }
            }

            get
            {
                return Model?.Priority?.ToString() ?? string.Empty;
            }
        }


    }
}
