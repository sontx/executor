using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace SonTx.Utils.Executor
{
    public sealed class ProcessExecutor
    {
        public string ExecutableFilePath { get; set; }

        private IList<ObjectWrapper<object>> objectWrapperArguments = new List<ObjectWrapper<object>>();

        public ProcessExecutor(string executableFilePath)
        {
            this.ExecutableFilePath = executableFilePath;
        }

        public ProcessExecutor()
        {
        }

        public void Add(object argument)
        {
            Add(null, argument);
        }

        public void Add(string key, object argument)
        {
            if (string.IsNullOrEmpty(key))
                key = GenerateKey();
            else
                CheckDuplicateKey(key);
            objectWrapperArguments.Add(new ObjectWrapper<object>() { Value = argument, Key = key });
        }

        public void Execute()
        {
            var tempFile = SerializeArgumentsToFile();
            Process.Start(ExecutableFilePath, "\"" + tempFile + "\"");
        }

        private void CheckDuplicateKey(string key)
        {
            if (objectWrapperArguments.Any((arg) => { return arg.Key == key; }))
                throw new ArgumentException("Key is Duplicated.");
        }

        private string GenerateKey()
        {
            return Guid.NewGuid().ToString();
        }

        private string SerializeArgumentsToFile()
        {
            string json = JsonConvert.SerializeObject(objectWrapperArguments);
            string tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, json);
            return tempFile;
        }
    }
}