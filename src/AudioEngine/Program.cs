// See https://aka.ms/new-console-template for more information
using System;

namespace AudioEngine
{
    class Program 
    {
        static void Main(string[] args) 
        {
            Console.WriteLine("HyperRoute Audio Engine v1.0");
            Console.WriteLine("Available commands: list-devices, list-sessions, route");
            
            if (args.Length > 0) 
            {
                Console.WriteLine($"Command received: {args[0]}");
            }
            else
            {
                Console.WriteLine("No command specified. Use: AudioEngine.exe [command]");
            }
        }
    }
}