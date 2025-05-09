/*
todo:
token number
remove;
help
replace;

*/


using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Security.Cryptography;
using System.Net.Http.Headers;
// using todoApp.CreateTable;

namespace todoApp{
    class TodoListTool{
        private string todoListFile;
        public TodoListTool( ){
            string csvFile =Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data.csv");
            //if file don't exist create a file
            if(!File.Exists(csvFile)){
                Console.WriteLine("File has been created");
                // File.Create(csvFile);
                string header = "#,TASK,DATE,STATUS,COMPLETE\n";
                File.WriteAllText(csvFile,header);
            }

            //assign path to todoListFile
            todoListFile = csvFile; 
        }
        
        //Get Token ID
        private string GetTokenNumber(){
            TokenGenerator taskId =  new TokenGenerator();
            string token = taskId.GenerateHexToken();
            return token;
        }

        //Add Task in files
        public void AddTask(string task){
            string taskHeader = $"{GetTokenNumber()},{task.Trim()},{DateTime.Now},Pending,-";
            using (StreamWriter sw = new StreamWriter(todoListFile, append: true)){
                sw.WriteLine(taskHeader);
            }; 
        }
        
        public void ShowTasks(string specificData = "None",int columnNum = 0 ){

            int[] padding = {6,32,22,11,22};
            CreateTable table = new CreateTable(5,padding);
            using(StreamReader rw = new StreamReader(todoListFile)){
                string? line;
                //header only
                if((line = rw.ReadLine())!=null){
                    table.PrintRowLines();
                    table.PrintRow(line.Split(","));
                }
                while((line  = rw.ReadLine()) != null){
                    // Console.WriteLine(line.Split(',')[coloumNum]);
                    string[] lineData = line.Split(",");
                    if (specificData == "None" || specificData == lineData[columnNum]){
                        table.PrintRowLines();
                        table.PrintRow(lineData);
                    }
                }
                table.PrintRowLines();
                
            }
        }
        // #pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

        public bool RemoveTask(string taskId){
            bool returnValue = false;
            
            string tempFile = Path.GetTempFileName();

            using (var sr = new StreamReader(todoListFile))
            using (var sw = new StreamWriter(tempFile,append:true)){
                
                string? line;
                //read all Line
                while ((line = sr.ReadLine()) != null)
                {
                    // Write all lines except the one to remove
                    if ( taskId != line.Split(',')[0]){
                        sw.WriteLine(line);
                        // Console.WriteLine($"{line} \n has been removed Successfull!");
                    }else{
                        // Console.WriteLine($"{} \n has been removed Successfull!");
                        returnValue = true;
                    }
                }
            };
            File.Delete(todoListFile);
            File.Move(tempFile, todoListFile);
            return returnValue;
        }   
        
        public void TaskComplete(string taskId){
            string[] oldValues = { "Pending", "-" }; // Match status and date fields
            string[] newValues = { "Complete", DateTime.Now.ToString() };
            if (EditTaskData(taskId, oldValues, newValues)){
                Console.Write("Marked task ");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write($"{taskId}");
                Console.ResetColor();
                Console.WriteLine(" Successfully!");
            }
        }

        public Boolean EditTaskData(string taskId, string newTask,int coloumIdx){
            bool returnStatus = false;

            string tempFile = Path.GetTempFileName();

            using (var sr = new StreamReader(todoListFile))
            using (var sw = new StreamWriter(tempFile, append: true))
            {
                string? line;
           
                while ((line = sr.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    string[] task = line.Split(',');

                    if (task[0].Trim() != taskId){
                        sw.WriteLine(line); // write unmodified
                    }
                    else{
                        string[] tmpTask = line.Split(',');
                        tmpTask[coloumIdx] = newTask;
                        sw.WriteLine(string.Join(',', tmpTask));
                        returnStatus = true;
                        // Console.WriteLine("Task updated successfully.");
                    }
                }
            }
            File.Delete(todoListFile);
            File.Move(tempFile, todoListFile);
            return returnStatus;
        }

        private Boolean EditTaskData(string taskId, string[] oldValues, string[] newValues){
            bool returnStatus = false;
            if (oldValues.Length != newValues.Length)
            {
                Console.WriteLine("Invalid input: mismatched value arrays.");
                return returnStatus;
            }

            string tempFile = Path.GetTempFileName();

            using (var sr = new StreamReader(todoListFile))
            using (var sw = new StreamWriter(tempFile, append: true))
            {
                string? line;
           
                while ((line = sr.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    string[] task = line.Split(',');

                    if (task[0].Trim() != taskId){
                        sw.WriteLine(line); // write unmodified
                    }
                    else{
                        for (int i = 0; i < task.Length; i++){
                            for (int j = 0; j < oldValues.Length; j++){
                                if (task[i] == oldValues[j])
                                    task[i] = newValues[j];
                                    returnStatus = true;
                            }
                        }
                        sw.WriteLine(string.Join(',', task));
                        // Console.WriteLine("Task updated successfully.");
                    }
                }
            }

            File.Delete(todoListFile);
            File.Move(tempFile, todoListFile);
            return returnStatus;
        }
    }
}