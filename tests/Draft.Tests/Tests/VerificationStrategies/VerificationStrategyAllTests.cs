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
    public class VerificationStrategyAllTests : BaseVerificationStrategyTests
    {

        protected override Uri[] Uris
        {
            get { return new[] { Uri1, Uri2, Uri3, Uri4, Uri5 }; }
        }

        protected override EndpointVerificationStrategy VerificationStrategy
        {
            get { return new VerificationStrategyAll(); }
        }


        [Fact]
        public void ShouldThrowExceptionWhenSomeEndpointsAreOffline()
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

                action.Should().Throw<InvalidHostException>();
            }
        }

        [Fact]
        public async Task ShouldSucceedIfAllEndpointsAreOnline()
        {
            using (var http = new HttpTest())
            {
                http.RespondWith("etc 1.2.3");
                http.RespondWith("etc 1.2.3");
                http.RespondWith("etc 1.2.3");
                http.RespondWith("etc 1.2.3");
                http.RespondWith("etc 1.2.3");
                http.RespondWith("etc 1.2.3");

                var epool = await CreateSut(VerificationStrategy)
                    .VerifyAndBuild(Uris);

                epool.Should().NotBeNull();

                epool.OnlineEndpoints.Should().HaveSameCount(Uris);

                for (var i = 0; i < Uris.Length; i++)
                {
                    var endpoint = epool.OnlineEndpoints[i];
                    endpoint.Uri
                            .Should()
                            .BeSameAs(Uris[i]);
                }
            }
        }
    }
}
