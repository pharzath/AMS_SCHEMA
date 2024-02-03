using AMS.Model.Models;
using CommandDotNet;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

public class Program
{
	// this is the entry point of your application
	static int Main(string[] args)
	{
		// AppRunner<T> where T is the class defining your commands
		// You can use Program or create commands in another class
		return new AppRunner<Program>().Run(args);
	}

	// Add command with two positional arguments
	public void Add(int x, int y) => Console.WriteLine(x + y);

	// Subtract command with two positional arguments
	public void Subtract(int x, int y) => Console.WriteLine(x - y);
}