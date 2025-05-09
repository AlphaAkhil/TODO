using System;

namespace todoApp{
    class ErrorHandler{
        
        public static void InvalidCommand(string command)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[ERROR] Invalid command: '{command}'. Use 'help' to see available commands.");
            Console.ResetColor();
        }

        public static void InvalidInput(string detail)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[ERROR] Invalid input: {detail}");
            Console.ResetColor();
        }

    }
}