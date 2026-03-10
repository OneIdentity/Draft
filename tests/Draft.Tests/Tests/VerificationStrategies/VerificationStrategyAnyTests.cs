using System.Net;
using System.Net.Sockets;
using System.Runtime.ExceptionServices;
using Draft.Endpoints;
using Draft.Exceptions;
using FluentAssertions;
using Flurl.Http.Testing;
using Xunit;

namespace Draft.Tests.VerificationStrategies
{
    public class VerificationStrategyAnyTests : BaseVerificationStrategyTests
    {

        protected override Uri[] Uris
        {
            get { return new[] { Uri1, Uri2, Uri3, Uri4, Uri5 }; }
        }

        protected override EndpointVerificationStrategy VerificationStrategy
        {
            get { return new VerificationStrategyAny(); }
        }


        [Fact]
        public void ShouldThrowExceptionWhenNoEndpointsAreOnline()
        {
            using (var http = new HttpTest())
            {
                http.SimulateException(new SocketException());

                Action action = () =>
                {
                    try
                    {
                        CreateSut(VerificationStrategy)
                            .VerifyAndBuild(Uris)
                            .Wait();
                    }
                    catch (AggregateException ae)
                    {
                        ExceptionDispatchInfo.Capture(ae.Flatten().InnerExceptions.First()).Throw();
                    }
                };

                Assert.Throws<InvalidHostException>(action);
            }
        }

        [Fact]
        public async Task ShouldSucceedIfSomeEndpointsAreOnline()
        {
            using (var http = new HttpTest())
            {
                http.SimulateException(new SocketException());
                http.RespondWith("etc 1 2 3");
                http.SimulateException(new SocketException());

                var epool = await CreateSut(VerificationStrategy)
                    .VerifyAndBuild(Uris);

                epool.Should().NotBeNull();

                epool.OnlineEndpoints.Should().HaveCount(1);
                var endpoint = epool.OnlineEndpoints.First();
                endpoint.Uri
                        .Should()
                        .BeSameAs(Uri2);
            }
        }

    }
}
