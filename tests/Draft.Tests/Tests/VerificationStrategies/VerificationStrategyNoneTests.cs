using Draft.Endpoints;
using FluentAssertions;
using Flurl.Http.Testing;
using Xunit;

namespace Draft.Tests.VerificationStrategies
{
    public class VerificationStrategyNoneTests : BaseVerificationStrategyTests
    {

        protected override Uri[] Uris
        {
            get { return new[] { Uri1, Uri2, Uri3, Uri4, Uri5 }; }
        }

        protected override EndpointVerificationStrategy VerificationStrategy
        {
            get { return new VerificationStrategyNone(); }
        }

        [Fact]
        public void ShouldVerifyAndBuildWithoutException()
        {
            using (var http = new HttpTest())
            {
                http.RespondWith("etcd 1.2.3")
                    .RespondWith("etcd 1.2.3")
                    .RespondWith("etcd 1.2.4")
                    .RespondWith("etcd 1.2.4")
                    .RespondWith("etcd 1.2.4");

                Assert.Null(Record.Exception(BuildAndVerifyAction));
            }
        }

        [Fact]
        public async Task ShouldMarkAllAsOnline()
        {
            var results = await CreateSut().VerifyAndBuild(Uris);

            results.Should().NotBeNull();
            results.AllEndpoints
                   .Should()
                   .HaveSameCount(Uris);

            foreach (var endpoint in results.AllEndpoints)
            {
                endpoint.Availability.Should()
                        .Be(EndpointAvailability.Online);
            }
        }

    }
}
