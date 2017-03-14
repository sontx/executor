using System.Collections.Generic;

namespace Sontx.Utils.Executor
{
    internal class ExecutingSession<T>
    {
        public string SessionKey { get; set; }
        public IList<T> Arguments { get; set; }
    }
}