using System;

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
            var clientProcess = new ClientProcess();
            if (clientProcess.Deserialize())
            {
                Console.WriteLine("I'm called");

                // receive arguments from caller process
                var deserializer = clientProcess.Deserializer;

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

                // communicate with caller process
                clientProcess.Transmitter.PrepareAsync().Wait();
                var message = clientProcess.Transmitter.Receive<string>();
                Console.WriteLine(message);
            }
            else
            {
                Console.WriteLine("I'm calling");

                ProcessExecutor executor = new ProcessExecutor("Example.exe");// execute itself

                // pass arguments while calling
                executor.Add("person1", new Person() { Name = "tran xuan son", Age = 23 });
                executor.Add(new Person() { Name = "tran xuan soan", Age = 24 });
                executor.Add("myDog", new Dog() { CoatColor = "black", Type = "i don't know" });
                executor.Add("some string");
                executor.Add(3393);
                executor.Execute();

                // communicate with process that is called
                executor.Transmitter.PrepareAsync().Wait();
                executor.Transmitter.Send("ok, i am fine");
            }
            Console.Read();
        }
    }
}