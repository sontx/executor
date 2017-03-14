using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sontx.Utils.Executor.Example
{
    internal class Program
    {
        private class Person
        {
            public string Name { get; set; }
            public int Age { get; set; }

            public override string ToString()
            {
                return string.Format("Name: {0}; Age: {1}", Name, Age);
            }
        }

        private class Dog
        {
            public string CoatColor { get; set; }
            public string Type { get; set; }

            public override string ToString()
            {
                return string.Format("CoatColor: {0}; Type: {1}", CoatColor, Type);
            }
        }

        private static void Main(string[] args)
        {
            ArgumentDeserializer deserializer = new ArgumentDeserializer();
            if (deserializer.Deserialize())
            {
                Console.WriteLine("I'm called");

                var person1 = deserializer.GetArgument<Person>("person1");
                var person2 = deserializer.GetArgument<Person>(1);
                var myDog = deserializer.GetArgument<Dog>("myDog");
                var myString = deserializer.GetArgument<string>(3);
                var myCode = deserializer.GetArgument<int>(4);

                Console.WriteLine(person1);
                Console.WriteLine(person2);
                Console.WriteLine(myDog);
                Console.WriteLine(myString);
                Console.WriteLine(myCode);
            }
            else
            {
                Console.WriteLine("I'm calling");

                ProcessExecutor executor = new ProcessExecutor("Example.exe");// execute itself
                executor.Add("person1", new Person() { Name = "tran xuan son", Age = 23 });
                executor.Add(new Person() { Name = "tran xuan soan", Age = 24 });
                executor.Add("myDog", new Dog() { CoatColor = "black", Type = "i don't know" });
                executor.Add("some string");
                executor.Add(3393);
                executor.Execute();
            }
            Console.Read();
        }
    }
}