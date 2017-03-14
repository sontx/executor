using System.IO.Pipes;
using System.Threading.Tasks;

namespace Sontx.Utils.Executor.Protocol
{
    internal class PipelineClientTransmitter : StreamObjectTransmitter
    {
        private NamedPipeClientStream clientStream;

        public string Key { get; set; }

        public PipelineClientTransmitter(string key)
        {
            this.Key = key;
        }

        public override Task PrepareAsync()
        {
            return Task.Run(() =>
            {
                clientStream = new NamedPipeClientStream(Key);
                clientStream.Connect();
                Initialize(clientStream);
            });
        }

        public override void Dispose()
        {
            if (clientStream != null)
            {
                clientStream.Close();
                clientStream = null;
            }
        }
    }
}