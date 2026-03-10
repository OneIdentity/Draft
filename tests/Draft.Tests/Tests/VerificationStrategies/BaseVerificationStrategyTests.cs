using Draft.Endpoints;

namespace Draft.Tests.VerificationStrategies
{
    public abstract class BaseVerificationStrategyTests
    {

        protected static readonly Uri Uri1 = new Uri("http://localhost:1");

        protected static readonly Uri Uri2 = new Uri("http://localhost:2");

        protected static readonly Uri Uri3 = new Uri("http://localhost:3");

        protected static readonly Uri Uri4 = new Uri("http://localhost:4");

        protected static readonly Uri Uri5 = new Uri("http://localhost:5");

        protected Action BuildAndVerifyAction
        {
            get
            {
                return async () =>
                {
                    await CreateSut()
                        .VerifyAndBuild(Uris);
                };
            }
        }

        protected abstract Uri[] Uris { get; }

        protected abstract EndpointVerificationStrategy VerificationStrategy { get; }

        protected EndpointPool.Builder CreateSut(EndpointVerificationStrategy? strategy = null)
        {
            return EndpointPool.Build()
                               .WithVerificationStrategy(strategy ?? VerificationStrategy);
        }

    }
}
