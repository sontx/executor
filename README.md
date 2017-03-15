#Executor Utils
You can pass multiple arguments that can be complex objects to another program while executing that program from an executable file.
#Install
To install Executor, run the following command in the [Package Manager Console](https://docs.nuget.org/docs/start-here/using-the-package-manager-console)

    Install-Package Sontx.Utils.Executor 
#Usage

Just want to pass some arguments!
---------------------------------

**From caller:**

    var executor = new ProcessExecutor("another.exe");
    executor.Add("person1", new Person() { Name = "tran xuan son", Age = 23 });
    executor.Add(new Person() { Name = "tran xuan soan", Age = 24 });
    executor.Add("myDog", new Dog() { CoatColor = "black", Type = "i don't know" });
    executor.Add("some string");
    executor.Add(3393);
    executor.Execute();
**From another program:**

    var deserializer = new ArgumentDeserializer();
    if (deserializer.Deserialize())
    {
        var person1 = deserializer.GetArgument<Person>("person1");
        var person2 = deserializer.GetArgument<Person>(1);
        var myDog = deserializer.GetArgument<Dog>("myDog");
        var myString = deserializer.GetArgument<string>(3);
        var myCode = deserializer.GetArgument<int>(4);
    }

And communicate with another
----------------------------
**From caller:**

    var executor = new ProcessExecutor("another.exe");
    executor.Add("something", "your arguments");
    executor.Execute();
    
    executor.Transmitter.PrepareAsync().Wait();
    executor.Transmitter.Send("I'm busy, what do you want to tell me!");
**From another program:**
        
    var clientProcess = new ClientProcess();
    if (clientProcess.Deserialize())
    {
        var deserializer = clientProcess.Deserializer;
        var something = deserializer.GetArgument<string>("something");
        
        clientProcess.Transmitter.PrepareAsync().Wait();
        var message = clientProcess.Transmitter.Receive<string>();
        clientProcess.Transmitter.Send("Ahihi, I love you ;)");
    }
