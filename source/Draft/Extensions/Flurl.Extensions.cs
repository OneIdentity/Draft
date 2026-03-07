using Flurl;
using Flurl.Http;
using System;
using System.Threading.Tasks;

namespace Draft
{
    internal static class FlurlExtensions
    {

        public static Url ToUrl(this Uri This)
        {
            return This.ToString();
        }

        public static IFlurlRequest Conditionally(this IFlurlRequest This, bool predicate, Func<IFlurlRequest, IFlurlRequest> action)
        {
            return predicate ? action(This) : This;
        }

        public static IFlurlClient Conditionally(this Url This, bool predicate, Func<Url, IFlurlClient> action)
        {
            return predicate ? action(This) : new FlurlClient(This);
        }

        public static Task<IFlurlResponse> Conditionally(this Url This, bool predicate, object data, Func<Url, object, Task<IFlurlResponse>> ifTrue, Func<Url, object, Task<IFlurlResponse>> ifFalse)
        {
            return predicate ? ifTrue(This, data) : ifFalse(This, data);
        }

    }
}
