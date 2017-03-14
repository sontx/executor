using Newtonsoft.Json;
using Sontx.Utils.Executor.Protocol;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Sontx.Utils.Executor
{
    public sealed class ProcessExecutor
    {
        private IObjectTransmitter _transmitter = null;

        public IObjectTransmitter Transmitter
        {
            get
            {
                if (_transmitter == null)
                {
                    ValidSessionKey();
                    _transmitter = new PipelineServerTransmitter(SessionKey);
                }
                return _transmitter;
            }
        }

        public string ExecutableFilePath { get; set; }

        public string SessionKey
        {
            get { return executingSession.SessionKey; }
            set { executingSession.SessionKey = value; }
        }

        private ExecutingSession<ObjectWrapper<object>> executingSession;

        public ProcessExecutor(string executableFilePath)
            : this()
        {
            this.ExecutableFilePath = executableFilePath;
        }

        public ProcessExecutor()
        {
            executingSession = new ExecutingSession<ObjectWrapper<object>>();
            executingSession.Arguments = new List<ObjectWrapper<object>>();
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
            executingSession.Arguments.Add(new ObjectWrapper<object>() { Value = argument, Key = key });
        }

        public void RemoveAt(int index)
        {
            executingSession.Arguments.RemoveAt(index);
        }

        public bool Remove(object argument)
        {
            var argumentObject = executingSession.Arguments.Single((obj) => { return obj.Value == argument; });
            return argumentObject != null ? executingSession.Arguments.Remove(argumentObject) : false;
        }

        public bool RemoveByKey(string key)
        {
            var argumentObject = executingSession.Arguments.Single((obj) => { return obj.Key == key; });
            return argumentObject != null ? executingSession.Arguments.Remove(argumentObject) : false;
        }

        public void Execute()
        {
            var process = CreateProcess();
            process.Dispose();
        }

        public Task ExecuteAndWaitAsync(int milliseconds = -1)
        {
            return Task.Run(() =>
            {
                using (var process = CreateProcess())
                {
                    if (milliseconds < 0)
                        process.WaitForExit();
                    else
                        process.WaitForExit(milliseconds);
                }
            });
        }

        private Process CreateProcess()
        {
            var tempFile = SerializeExecutingSessionToFile();
            return Process.Start(ExecutableFilePath, "\"" + tempFile + "\"");
        }

        private void CheckDuplicateKey(string key)
        {
            if (executingSession.Arguments.Any((arg) => { return arg.Key == key; }))
                throw new ArgumentException("Key is Duplicated.");
        }

        private string GenerateKey()
        {
            return Guid.NewGuid().ToString();
        }

        private void ValidSessionKey()
        {
            if (string.IsNullOrEmpty(executingSession.SessionKey))
                executingSession.SessionKey = GenerateKey();
        }

        private string SerializeExecutingSessionToFile()
        {
            ValidSessionKey();
            string json = JsonConvert.SerializeObject(executingSession);
            string tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, json);
            return tempFile;
        }
    }
}