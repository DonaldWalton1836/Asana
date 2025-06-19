using Asana.Library.Models;
using Asana.Library.Services;
using System;

namespace Asana
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var toDoSvc = ToDoServiceProxy.Current;
            int choiceInt;
            do
            {
                Console.WriteLine("\nChoose a menu option:");
                Console.WriteLine("1. Create a ToDo");
                Console.WriteLine("2. List all ToDos");
                Console.WriteLine("3. List all outstanding ToDos");
                Console.WriteLine("4. Delete a ToDo");
                Console.WriteLine("5. Update a ToDo");
                Console.WriteLine("6. Create a Project");
                Console.WriteLine("7. Delete a Project");
                Console.WriteLine("8. Update a Project");
                Console.WriteLine("9. List all Projects");
                Console.WriteLine("10. List all ToDos in a Project");
                Console.WriteLine("11. Exit");

                var choice = Console.ReadLine() ?? "11";

                if (int.TryParse(choice, out choiceInt))
                {
                    switch (choiceInt)
                    {
                        case 1:
                            Console.Write("Name: ");
                            var name = Console.ReadLine();
                            Console.Write("Description: ");
                            var description = Console.ReadLine();
                            Console.Write("Priority: ");
                            var priority = Console.ReadLine();
                            Console.Write("Project ID: ");
                            int pid = int.Parse(Console.ReadLine());

                            var todo = new ToDo
                            {
                                Id = 0,
                                Name = name,
                                Description = description,
                                Priority = 0,
                                IsCompleted = false,
                                ProjectId = pid
                            };

                            toDoSvc.AddOrUpdate(todo);
                            var proj = toDoSvc.GetAllProjects().Find(p => p.Id == pid);
                            if (proj != null)
                            {
                                proj.ToDoIds.Add(todo.Id);
                                toDoSvc.UpdateProjectCompletion(proj);
                            }
                            break;

                        case 2:
                            toDoSvc.DisplayToDos(true);
                            break;

                        case 3:
                            toDoSvc.DisplayToDos();
                            break;

                        case 4:
                            toDoSvc.DisplayToDos(true);
                            Console.Write("ToDo to Delete: ");
                            var toDoChoice4 = int.Parse(Console.ReadLine() ?? "0");

                            var reference = toDoSvc.GetById(toDoChoice4);
                            toDoSvc.DeleteToDo(reference);
                            break;

                        case 5:
                            toDoSvc.DisplayToDos(true);
                            Console.Write("ToDo to Update: ");
                            var toDoChoice5 = int.Parse(Console.ReadLine() ?? "0");
                            var updateReference = toDoSvc.GetById(toDoChoice5);

                            if (updateReference != null)
                            {
                                Console.Write("Name: ");
                                updateReference.Name = Console.ReadLine();

                                Console.Write("Description: ");
                                updateReference.Description = Console.ReadLine();

                                Console.Write("Priority (1=Low, 2=Medium, 3=High): ");
                                var priorityInput = Console.ReadLine();
                                if (int.TryParse(priorityInput, out int parsedPriority))
                                {
                                    updateReference.Priority = parsedPriority;
                                }
                                else
                                {
                                    Console.WriteLine("Invalid input. Priority not updated.");
                                }

                                Console.Write("Is Completed (true/false): ");
                                var completedInput = Console.ReadLine();
                                updateReference.IsCompleted = bool.TryParse(completedInput, out bool completed)
                                    ? completed
                                    : false;
                            }

                            toDoSvc.AddOrUpdate(updateReference);
                            break;

                        case 6:
                            Console.Write("Project Name: ");
                            var pname = Console.ReadLine();
                            Console.Write("Project Description: ");
                            var pdesc = Console.ReadLine();
                            toDoSvc.AddProject(pname, pdesc);
                            break;

                        case 7:
                            Console.Write("Project ID to delete: ");
                            int delId = int.Parse(Console.ReadLine());
                            toDoSvc.DeleteProject(delId);
                            break;

                        case 8:
                            Console.Write("Project ID to update: ");
                            int updId = int.Parse(Console.ReadLine());
                            Console.Write("New Name: ");
                            var newName = Console.ReadLine();
                            Console.Write("New Description: ");
                            var newDesc = Console.ReadLine();
                            toDoSvc.UpdateProject(new Project { Id = updId, Name = newName, Description = newDesc });
                            break;

                        case 9:
                            foreach (var p in toDoSvc.GetAllProjects())
                            {
                                Console.WriteLine($"{p.Id}: {p.Name} - {p.CompletePercent}% complete");
                            }
                            break;

                        case 10:
                            Console.Write("Enter Project ID: ");
                            int pidView = int.Parse(Console.ReadLine());
                            var todos = toDoSvc.GetToDosByProject(pidView);
                            foreach (var t in todos)
                            {
                                Console.WriteLine($"{t.Id}: {t.Name} | Done: {t.IsCompleted}");
                            }
                            break;

                        case 11:
                            break;

                        default:
                            Console.WriteLine("ERROR: Unknown menu selection");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine($"ERROR: '{choice}' is not a valid menu selection");
                }

            } while (choiceInt != 11);
        }
    }
}