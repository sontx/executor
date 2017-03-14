using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Collections;

namespace Sontx.Utils.Executor
{
    public sealed class ArgumentDeserializer : IEnumerable<object>
    {
        private IList<JObject> objectArguments = null;
        private readonly string[] commandLineArguments = null;

        public bool DeleteTempFileOnDeserialized { get; set; } = true;

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
                DeserializeArguments(tempFile);
            }
            catch
            {
                return false;
            }

            if (DeleteTempFileOnDeserialized)
                File.Delete(tempFile);

            return true;
        }

        public T GetArgument<T>(int index)
        {
            return objectArguments[index].ToObject<ObjectWrapper<T>>().Value;
        }

        public T GetArgument<T>(string key)
        {
            JObject argument = objectArguments.Single((arg) => { return arg.ToObject<ObjectWrapper<object>>().Key == key; });
            return argument.ToObject<ObjectWrapper<T>>().Value;
        }

        public bool Contains(string key)
        {
            return objectArguments.Any((arg) => { return arg.ToObject<ObjectWrapper<object>>().Key == key; });
        }

        private void DeserializeArguments(string tempFile)
        {
            string json = File.ReadAllText(tempFile);
            objectArguments = JsonConvert.DeserializeObject<IList<JObject>>(json);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<object> GetEnumerator()
        {
            foreach (var jObject in objectArguments)
            {
                yield return jObject.ToObject<ObjectWrapper<object>>().Value;
            }
        }
    }
}