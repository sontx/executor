using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Sontx.Utils.Executor
{
    public sealed class ArgumentDeserializer : IEnumerable<object>
    {
        private ExecutingSession<JObject> executingSession = null;

        private readonly string[] commandLineArguments = null;

        public string SessionKey { get { return executingSession.SessionKey; } }

        public bool DeleteTempFileOnDeserialized { get; set; } = true;

        public bool IsDeserialized { get; private set; } = false;

        public ArgumentDeserializer(string[] arguments)
        {
            this.commandLineArguments = arguments;
        }

        public ArgumentDeserializer()
        {
            this.commandLineArguments = Environment.GetCommandLineArgs();
        }

        public bool Deserialize()
        {
            if (commandLineArguments.Length != 2)
                return false;

            string tempFile = commandLineArguments[1];
            if (!File.Exists(tempFile))
                return false;

            try
            {
                DeserializeExecutingSession(tempFile);
            }
            catch
            {
                return false;
            }

            if (DeleteTempFileOnDeserialized)
                File.Delete(tempFile);

            IsDeserialized = true;

            return true;
        }

        public T GetArgument<T>(int index)
        {
            return executingSession.Arguments[index].ToObject<ObjectWrapper<T>>().Value;
        }

        public T GetArgument<T>(string key)
        {
            JObject argument = executingSession.Arguments.Single((arg) => { return arg.ToObject<ObjectWrapper<object>>().Key == key; });
            return argument.ToObject<ObjectWrapper<T>>().Value;
        }

        public bool Contains(string key)
        {
            return executingSession.Arguments.Any((arg) => { return arg.ToObject<ObjectWrapper<object>>().Key == key; });
        }

        private void DeserializeExecutingSession(string tempFile)
        {
            string json = File.ReadAllText(tempFile);
            executingSession = JsonConvert.DeserializeObject<ExecutingSession<JObject>>(json);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<object> GetEnumerator()
        {
            foreach (var jObject in executingSession.Arguments)
            {
                yield return jObject.ToObject<ObjectWrapper<object>>().Value;
            }
        }
    }
}