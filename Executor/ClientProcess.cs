using Sontx.Utils.Executor.Protocol;

namespace Sontx.Utils.Executor
{
    public sealed class ClientProcess
    {
        private IObjectTransmitter _transmitter = null;

        public ArgumentDeserializer Deserializer { get; private set; }

        public IObjectTransmitter Transmitter
        {
            get
            {
                if (_transmitter == null && Deserializer.IsDeserialized)
                    _transmitter = new PipelineClientTransmitter(Deserializer.SessionKey);
                return _transmitter;
            }
        }

        public ClientProcess()
        {
            Deserializer = new ArgumentDeserializer();
        }

        public bool Deserialize()
        {
            return Deserializer.Deserialize();
        }
    }
}