using System.IO.Pipes;
using System.Threading.Tasks;

namespace Sontx.Utils.Executor.Protocol
{
    internal class PipelineServerTransmitter : StreamObjectTransmitter
    {
        private NamedPipeServerStream serverStream;

        public string Key { get; set; }

        public PipelineServerTransmitter(string key)
        {
            this.Key = key;
        }

        public override Task PrepareAsync()
        {
            return Task.Run(() =>
            {
                serverStream = new NamedPipeServerStream(Key, PipeDirection.InOut, 1, PipeTransmissionMode.Byte);
                serverStream.WaitForConnection();
                Initialize(serverStream);
            });
        }

        public override void Dispose()
        {
            if (serverStream != null)
            {
                serverStream.Close();
                serverStream = null;
            }
        }
    }
}