// // See https://aka.ms/new-console-template for more information
// Console.WriteLine("Hello, World!");

using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.IO.Pipes;


namespace todoApp{
    class TodoApp{
        static Boolean run= true;
        static TodoListTool myTask ;

        static TodoApp(){
        myTask = new TodoListTool();
        }
        static void Main(String[] args){
            myTask = new TodoListTool();
            if(myTask is null){
                return;
            }
            //check if args is given or not
            if(args.Length > 0 ){
                string arg = String.Join(" ",args);
                if(OptionHandler(arg.ToLower()) != 0){
                    ErrorHandler.InvalidCommand("Unknown command. Use 'help' to see available commands.");
                }
            }else{
                _Logo();
                while(run){
                    Console.Write("Todo://> ");
                    string[] command = Console.ReadLine()!.Split(" ",2);
                    if (command != null ){
                        if (command.Length ==2){
                            string arg = command[0];
                            string parse = command[1];
                            Functionality(arg,parse);
                        }
                        else{
                            // PrintArgs(command);
                            Functionality(command[0]);
                        }
                        
                    }
                }
                // Console.WriteLine("no argument pass");
            }

            /*    
            // static void PrintArgs(string[] args){
            //     for(int idx =0;idx <args.Length;idx++){
            //         Console.Write(args[idx]);
            //     }
            // }
                
            // }
            
            // TodoListTool myTask = new TodoListTool();
            // // CreateTable table = new CreateTable();
            // // table.RowLines();
            // // m.ShowTask();
            // DocumentationHelp();
            */
        }  
        
        static int OptionHandler(string arg){
            if (arg == "-a" || arg== "-all"){
                myTask.ShowTasks();
                return 0;
            }
            if (arg == "-c" || arg== "-complete"){
                myTask.ShowTasks("Complete",3);
                return 0;
            }
            if (arg == "-p" || arg== "-padding"){
                myTask.ShowTasks("Padding",3);
                return 0;
            }
            if (arg == "-v"|| arg=="--version"){
                _version();
            }
            return -1;
        }
        
        static void Functionality(string arg,string parse ="None" ){
            
            switch (arg.ToLower()){
                case "add":
                    AddTask(parse);
                    break;

                case "list":
                    PrintTaskTable(parse);
                    break;

                case "markdone":
                    MarkCompleteTask(parse); 
                    break;

                case "delete":
                    // Console.WriteLine("this is not noweoifnksndfkjfdkaf;");
                    RemoveTaskFromList(parse);
                    break;
                
                case "edit":
                    EditTask(parse);
                    break;

                case "quit":
                    Console.WriteLine("Quiting App...");
                    run = false;
                    break;
                case "help":
                    _DocumentationHelp();
                    break;
                default:
                    ErrorHandler.InvalidCommand("Unknown command. Use 'help' to see available commands.");
                    // Console.WriteLine("Unknown command. Use 'help' to see available commands.");
                    break;
            }
        }

        private static void EditTask(string parse){
            string[] taskDetail = parse.Split(" ",2);
            if (taskDetail.Length != 2){
                ErrorHandler.InvalidCommand("Edit command must have two args taskId and newTask");
            }
            else{
                if (taskDetail[0].Length == 5)
                if( myTask.EditTaskData(taskDetail[0],taskDetail[1],1)){
                    Console.WriteLine($"task, {taskDetail[0]} updated successfully!");
                }
                else{
                    ErrorHandler.InvalidInput("no TaskID found!");
                }
            }
        }

        private static void RemoveTaskFromList(string taskId){
            if (taskId.Length == 5)
                if( myTask.RemoveTask(taskId)){
                    Console.WriteLine($"task, {taskId} removed successfully!");
                }
                else{
                    ErrorHandler.InvalidInput("no TaskID found!");
                }
        }

        private static void PrintTaskTable(string parse= "None"){
            if (parse !=  "None"){
                if(parse =="-c" || parse == "--complete"){
                    myTask.ShowTasks("Complete",3);
                }
                if (parse =="-p" || parse == "--pending"){
                    myTask.ShowTasks("Pending",3);
                }
            }else{
                myTask.ShowTasks();
            }
        }
        private static void MarkCompleteTask(string taskId = "None"){
            if (taskId.Length == 5  ){
                myTask.TaskComplete(taskId);
            }else{
                ErrorHandler.InvalidCommand("Please Enter valid TaskID");
            }
        }

        private static void AddTask(string parse = "None"){
            
            if (parse !=  "None"){
                string[] tasks = parse.Split(',');
                for(int i =0;i<tasks.Length;i++){
                    myTask.AddTask(tasks[i]);
                }
                Console.WriteLine("Task Added successfully!");
            }
            else{
                ErrorHandler.InvalidCommand("Task cannot be empty.");
            }
        }

        private static void _Logo(){
            Console.WriteLine(@$"
============================================================================
  _____     __    ____     ___
 |_   _|  / _ \  |  _ \   / _ \ 
   | |   | | | | | | | | | | | |
   | |   | |_| | | |_| | | |_| |
   |_|    \___/  |____/   \___/ 
                              
===============================TODO TERMINAL=================================");
        }
        private static void _version(){
            Console.WriteLine(@$"App: TODO
                version : beta UnRelease 0.1
                by:AlphaAkhil 
                url: github.com/AlphaAkhil"
                );
        }
        private static void _DocumentationHelp(){
            Console.WriteLine(@"
Usage: todo [OPTIONS] [COMMAND] [ARGS...]

A command-line tool to manage your daily tasks efficiently.

Commands:
  add [task]           Add a new task to the list.
  list                 List all pending tasks.
  markdone [id]            Mark a task as completed.
  delete [id]          Delete a task by its ID.
  edit [id] [task]     Edit the content of a task.
  help                 Show this help message.

Options:
  -a, --all            Show all tasks (completed + pending)
  -c, --completed      Show only completed tasks
  -p, --pending        Show only pending tasks
  -v, --version        Show application version
  -h, --help           Show this help message

Exit Status:
  Returns 0 if command executed successfully.
  Non-zero status indicates an error or invalid command usage.

Examples:
  todo add Buy groceries
  todo list --all
  todo done 1b4a7
"
                );
            }
        }
    }
