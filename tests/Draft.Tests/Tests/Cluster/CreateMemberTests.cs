using System.Net;
using Draft.Exceptions;
using FluentAssertions;
using Flurl;
using Flurl.Http.Testing;
using Xunit;

namespace Draft.Tests.Cluster
{
    public class CreateMemberTests
    {

        [Fact]
        public async Task ShouldCallTheCorrectUrlByAwaitingImmediately()
        {
            using (var http = new HttpTest())
            {
                http.RespondWithJson(Fixtures.Cluster.CreateResponse(Fixtures.EtcdUrl.ToUri()));

                var member = await Etcd.ClientFor(Fixtures.EtcdUrl.ToUri())
                                       .Cluster
                                       .CreateMember()
                                       .WithPeerUri(Fixtures.EtcdUrl.ToUri());

                http.Should()
                    .HaveCalled(
                        Fixtures.EtcdUrl
                                .AppendPathSegment(Constants.Etcd.Path_Members)
                    )
                    .WithVerb(HttpMethod.Post)
                    .WithContentType(Constants.Http.ContentType_ApplicationJson)
                    .Times(1);

                member.Should().NotBeNull();

                member.PeerUrls.Should()
                      .NotBeEmpty()
                      .And
                      .ContainSingle(x => Fixtures.EtcdUrl.ToUri().Equals(x));
            }
        }

        [Fact]
        public async Task ShouldThrowExistingPeerAddressExceptionOnDuplicatePeerAddress()
        {
            using (var http = new HttpTest())
            {
                http.RespondWithJson(HttpStatusCode.Conflict, Fixtures.CreateErrorMessage(Constants.Etcd.ErrorCode_ExistingPeerAddress));

                Func<Task> action = async () =>
                {
                    await Etcd.ClientFor(Fixtures.EtcdUrl.ToUri())
                              .Cluster
                              .CreateMember()
                              .WithPeerUri(Fixtures.EtcdUrl.ToUri());
                };

                var exception = await Record.ExceptionAsync(action);
                Assert.IsType<ExistingPeerAddressException>(exception)
                      .IsExistingPeerAddress.Should().BeTrue();
            }
        }

    }
}
