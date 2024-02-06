using HotelsManager.Attributes.Base;

namespace HotelsManager.Attributes
{
    public class HttpPutAttribute : HttpAttribute
    {
        public HttpPutAttribute(string routing) : base(HttpMethod.Put, routing) {}

        public HttpPutAttribute() : base(HttpMethod.Put, null) {}
    }
}