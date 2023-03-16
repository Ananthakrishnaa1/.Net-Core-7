using Newtonsoft.Json.Linq;

namespace System.Linq
{
    internal class FetchResult<T>
    {
        public JToken? result;

        public FetchResult(JToken? result)
        {
            this.result = result;
        }
    }
}