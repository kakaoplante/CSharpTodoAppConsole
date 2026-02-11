using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text.Json;

namespace todoApp;

class Program
{
    static void Main(string[] args)
    {

        string filePath = Path.Combine(AppContext.BaseDirectory, "todos.json");

        // Loads todos via method. 
        List<Todo>? todosList = LoadTodos(filePath);

        string? userInput;

        while (true)
        {
            //Display todoAPp
            DisplayInterface(todosList);

            //Show menu
            DisplayMenu();

        }

        //User interface:


        void DisplayInterface(List<Todo> todoList)
        {
            Console.Clear();
            System.Console.WriteLine("                   ");
            System.Console.WriteLine("     ToDo APP      ");
            System.Console.WriteLine("                   ");
            int count = 1;
            foreach (Todo item in todosList)
            {
                string done = item.IsDone ? "Done" : "Not Done";
                System.Console.WriteLine($"{count++}. {item.TodoItem} - {done}");
            }
            System.Console.WriteLine("");
        }

        void DisplayMenu()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            System.Console.WriteLine("MENU:");
            System.Console.WriteLine("[1] Add todo.  [2] Edit Todo.  [3] Mark as done,   [4] Delele todo.");
            Console.ResetColor();
            userInput = Console.ReadLine();

            if (userInput == "1")
            {
                DisplayInterface(todosList);

                System.Console.WriteLine("Please enter what ToDo:");
                userInput = Console.ReadLine();
                if (userInput == null || userInput == "")
                {
                    InvalidCommand("Non Chosen.");
                }
                else
                {
                    AddTodo(userInput);
                }

            }
            else if (userInput == "2")
            {
                DisplayInterface(todosList);

                System.Console.WriteLine("Please type the number of the todo to edit.");
                userInput = Console.ReadLine();
                int choice;
                if (int.TryParse(userInput, out choice))
                {
                    if (choice <= todosList.Count && choice > 0)
                    {
                        DisplayInterface(todosList);
                        EditTodo(todosList[choice - 1]);
                    }
                    else
                    {
                        InvalidCommand("Non Chosen.");
                    }

                }
                else
                {
                    InvalidCommand("Non Chosen.");

                }

            }
            else if (userInput == "3")
            {

                DisplayInterface(todosList);

                System.Console.WriteLine("Which Todo to mark as done?");
                userInput = Console.ReadLine();
                int choice;
                if (int.TryParse(userInput, out choice))
                {
                    if (choice <= todosList.Count && choice > 0)
                    {
                        MarkAsDone(todosList[choice - 1]);
                    }
                    else
                    {
                        InvalidCommand("Non Chosen.");
                    }
                }
                else
                {
                    InvalidCommand("Non Chosen.");
                }
            }
            else if (userInput == "4")
            {
                DisplayInterface(todosList);

                System.Console.WriteLine("Which Todo to delete?");
                userInput = Console.ReadLine();
                int choice;
                if (int.TryParse(userInput, out choice))
                {
                    if (choice <= todosList.Count && choice > 0)
                    {
                        todosList.RemoveAt(choice - 1);
                        SaveTodos(filePath, todosList);
                    }
                    else
                    {
                        InvalidCommand("Non Chosen.");
                    }
                }
            }
            else
            {
                InvalidCommand("Non Chosen.");
            }
        }

        void InvalidCommand(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine(msg);
            Thread.Sleep(1000);
            Console.ResetColor();
        }


        // Todo Interaction
        void AddTodo(string item)
        {
            Todo newTodo = new Todo() { TodoItem = item, IsDone = false };

            todosList.Add(newTodo);
            SaveTodos(filePath, todosList);
        }

        void EditTodo(Todo todo)
        {
            System.Console.WriteLine($"Original item: {todo.TodoItem}");
            System.Console.Write("Please enter edit: ");
            string? newItem = Console.ReadLine();
            if (newItem == null || newItem == "")
            {
                System.Console.WriteLine("No input. Non added");
            }
            else
            {
                todo.TodoItem = newItem;
                SaveTodos(filePath, todosList);

            }
        }

        void MarkAsDone(Todo todo)
        {
            todo.IsDone = true;
            SaveTodos(filePath, todosList);


        }


        //Database
        //Opretter og tjekker for liste database
        List<Todo> LoadTodos(string path)
        {
            if (!File.Exists(path))
            {
                File.WriteAllText(path, "[]");
                return new List<Todo>();
            }

            var json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<List<Todo>>(json)
                   ?? new List<Todo>();
        }

        // Gemmer listen
        void SaveTodos(string filePath, List<Todo> todos)
        {
            string json = JsonSerializer.Serialize(todos, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(filePath, json);
        }

    }


}



