using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace Sontx.Utils.Executor.Protocol
{
    internal abstract class StreamObjectTransmitter : IObjectTransmitter
    {
        private StreamReader reader;
        private StreamWriter writer;

        protected void Initialize(Stream stream)
        {
            reader = new StreamReader(stream);
            writer = new StreamWriter(stream);
            writer.AutoFlush = true;
        }

        #region Implements IObjectTransmitter

        public T Receive<T>()
        {
            string json = reader.ReadLine();
            return JsonConvert.DeserializeObject<ObjectWrapper<T>>(json).Value;
        }

        public void Send(object obj)
        {
            var json = JsonConvert.SerializeObject(new ObjectWrapper<object>() { Value = obj });
            writer.WriteLine(json);
        }

        public Task SendAsync(object obj)
        {
            return Task.Run(() =>
            {
                Send(obj);
            });
        }

        public Task<T> ReceiveAsync<T>()
        {
            return Task.Run(() =>
            {
                return Receive<T>();
            });
        }

        public abstract Task PrepareAsync();

        public abstract void Dispose();

        #endregion Implements IObjectTransmitter
    }
}