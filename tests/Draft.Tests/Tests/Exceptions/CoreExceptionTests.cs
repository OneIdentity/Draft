using System.Net;
using System.Net.Sockets;
using Draft.Exceptions;
using Draft.Responses;
using FluentAssertions;
using Flurl.Http;
using Flurl.Http.Testing;
using Xunit;
using KeyNotFoundException = Draft.Exceptions.KeyNotFoundException;

namespace Draft.Tests.Exceptions
{
    public class CoreExceptionTests
    {
        private static readonly Func<Task<IEtcdVersion>> CallFixture = async () => await Etcd.ClientFor(Fixtures.EtcdUrl.ToUri()).GetVersion();

        private static HttpTestSetup NewErrorCodeFixture(HttpTest httpTest, int? etcdErrorCode = null, HttpStatusCode status = HttpStatusCode.BadRequest)
        {
            return httpTest
                .RespondWithJson(status, Fixtures.CreateErrorMessage(etcdErrorCode));
        }

        [Fact]
        public async Task ShouldParseErrorCodeFromHttpStatusIfMissingFromBody()
        {
            using (var http = new HttpTest())
            {
                NewErrorCodeFixture(http, status: HttpStatusCode.Conflict);

                await Assert.ThrowsAsync<ExistingPeerAddressException>(CallFixture);
            }
        }

        [Fact]
        public async Task ShouldParseErrorCodeFromMessageBody()
        {
            using (var http = new HttpTest())
            {
                NewErrorCodeFixture(http, Constants.Etcd.ErrorCode_Unknown);

                await Assert.ThrowsAsync<UnknownErrorException>(CallFixture);
            }
        }

        [Fact]
        public async Task ShouldThrowEtcdTimeoutException()
        {
            using (var http = new HttpTest())
            {
                http.SimulateTimeout();

                var exception = await Record.ExceptionAsync(CallFixture);
                Assert.IsType<EtcdTimeoutException>(exception)
                    .IsTimeout.Should().BeTrue();
            }
        }

        [Fact]
        public async Task ShouldThrowInvalidRequestException()
        {
            using (var http = new HttpTest())
            {
                http.RespondWith(HttpStatusCode.NotFound, HttpStatusCode.NotFound.ToString());

                var exception = await Record.ExceptionAsync(CallFixture);
                Assert.IsType<InvalidRequestException>(exception)
                    .IsInvalidRequest.Should().BeTrue();
            }
        }

        #region Exception Type Tests

        [Fact]
        public async Task ShouldThrowClientInternalException()
        {
            using (var http = new HttpTest())
            {
                NewErrorCodeFixture(http, Constants.Etcd.ErrorCode_ClientInternal);

                var exception = await Record.ExceptionAsync(CallFixture);
                Assert.IsType<ClientInternalException>(exception)
                    .IsClientInternal.Should().BeTrue();
            }
        }

        [Fact]
        public async Task ShouldThrowDirectoryNotEmptyException()
        {
            using (var http = new HttpTest())
            {
                NewErrorCodeFixture(http, Constants.Etcd.ErrorCode_DirectoryNotEmpty);

                var exception = await Record.ExceptionAsync(CallFixture);
                Assert.IsType<DirectoryNotEmptyException>(exception)
                    .IsDirectoryNotEmpty.Should().BeTrue();
            }
        }

        [Fact]
        public async Task ShouldThrowEventIndexClearedException()
        {
            using (var http = new HttpTest())
            {
                NewErrorCodeFixture(http, Constants.Etcd.ErrorCode_EventIndexCleared);

                var exception = await Record.ExceptionAsync(CallFixture);
                Assert.IsType<EventIndexClearedException>(exception)
                    .IsEventIndexCleared.Should().BeTrue();
            }
        }

        [Fact]
        public async Task ShouldThrowExistingPeerAddressException()
        {
            using (var http = new HttpTest())
            {
                NewErrorCodeFixture(http, Constants.Etcd.ErrorCode_ExistingPeerAddress);

                var exception = await Record.ExceptionAsync(CallFixture);
                Assert.IsType<ExistingPeerAddressException>(exception)
                    .IsExistingPeerAddress.Should().BeTrue();
            }
        }

        [Fact]
        public async Task ShouldThrowIndexNotANumberException()
        {
            using (var http = new HttpTest())
            {
                NewErrorCodeFixture(http, Constants.Etcd.ErrorCode_IndexNotANumber);

                var exception = await Record.ExceptionAsync(CallFixture);
                Assert.IsType<IndexNotANumberException>(exception)
                    .IsIndexNotANumber.Should().BeTrue();
            }
        }

        [Fact]
        public async Task ShouldThrowIndexOrValueRequiredException()
        {
            using (var http = new HttpTest())
            {
                NewErrorCodeFixture(http, Constants.Etcd.ErrorCode_IndexOrValueRequired);

                var exception = await Record.ExceptionAsync(CallFixture);
                Assert.IsType<IndexOrValueRequiredException>(exception)
                    .IsIndexOrValueRequired.Should().BeTrue();
            }
        }

        [Fact]
        public async Task ShouldThrowIndexValueMutexException()
        {
            using (var http = new HttpTest())
            {
                NewErrorCodeFixture(http, Constants.Etcd.ErrorCode_IndexValueMutex);

                var exception = await Record.ExceptionAsync(CallFixture);
                Assert.IsType<IndexValueMutexException>(exception)
                    .IsIndexValueMutex.Should().BeTrue();
            }
        }

        [Fact]
        public async Task ShouldThrowInvalidActiveSizeException()
        {
            using (var http = new HttpTest())
            {
                NewErrorCodeFixture(http, Constants.Etcd.ErrorCode_InvalidActiveSize);

                var exception = await Record.ExceptionAsync(CallFixture);
                Assert.IsType<InvalidActiveSizeException>(exception)
                    .IsInvalidActiveSize.Should().BeTrue();
            }
        }

        [Fact]
        public async Task ShouldThrowInvalidFieldException()
        {
            using (var http = new HttpTest())
            {
                NewErrorCodeFixture(http, Constants.Etcd.ErrorCode_InvalidField);

                var exception = await Record.ExceptionAsync(CallFixture);
                Assert.IsType<InvalidFieldException>(exception)
                    .IsInvalidField.Should().BeTrue();
            }
        }

        [Fact]
        public async Task ShouldThrowInvalidFormException()
        {
            using (var http = new HttpTest())
            {
                NewErrorCodeFixture(http, Constants.Etcd.ErrorCode_InvalidForm);

                var exception = await Record.ExceptionAsync(CallFixture);
                Assert.IsType<InvalidFormException>(exception)
                    .IsInvalidForm.Should().BeTrue();
            }
        }

        [Fact]
        public async Task ShouldThrowInvalidRemoveDelayException()
        {
            using (var http = new HttpTest())
            {
                NewErrorCodeFixture(http, Constants.Etcd.ErrorCode_InvalidRemoveDelay);

                var exception = await Record.ExceptionAsync(CallFixture);
                Assert.IsType<InvalidRemoveDelayException>(exception)
                    .IsInvalidRemoveDelay.Should().BeTrue();
            }
        }

        [Fact]
        public async Task ShouldThrowKeyIsPreservedException()
        {
            using (var http = new HttpTest())
            {
                NewErrorCodeFixture(http, Constants.Etcd.ErrorCode_KeyIsPreserved);

                var exception = await Record.ExceptionAsync(CallFixture);
                Assert.IsType<KeyIsPreservedException>(exception)
                    .IsKeyIsPreserved.Should().BeTrue();
            }
        }

        [Fact]
        public async Task ShouldThrowKeyNotFoundException()
        {
            using (var http = new HttpTest())
            {
                NewErrorCodeFixture(http, Constants.Etcd.ErrorCode_KeyNotFound);

                var exception = await Record.ExceptionAsync(CallFixture);
                Assert.IsType<KeyNotFoundException>(exception)
                    .IsKeyNotFound.Should().BeTrue();
            }
        }

        [Fact]
        public async Task ShouldThrowLeaderElectException()
        {
            using (var http = new HttpTest())
            {
                NewErrorCodeFixture(http, Constants.Etcd.ErrorCode_LeaderElect);

                var exception = await Record.ExceptionAsync(CallFixture);
                Assert.IsType<LeaderElectException>(exception)
                    .IsLeaderElect.Should().BeTrue();
            }
        }

        [Fact]
        public async Task ShouldThrowNameRequiredException()
        {
            using (var http = new HttpTest())
            {
                NewErrorCodeFixture(http, Constants.Etcd.ErrorCode_NameRequired);

                var exception = await Record.ExceptionAsync(CallFixture);
                Assert.IsType<NameRequiredException>(exception)
                    .IsNameRequired.Should().BeTrue();
            }
        }

        [Fact]
        public async Task ShouldThrowNodeExistsException()
        {
            using (var http = new HttpTest())
            {
                NewErrorCodeFixture(http, Constants.Etcd.ErrorCode_NodeExists);

                var exception = await Record.ExceptionAsync(CallFixture);
                Assert.IsType<NodeExistsException>(exception)
                    .IsNodeExists.Should().BeTrue();
            }
        }

        [Fact]
        public async Task ShouldThrowNoMorePeerException()
        {
            using (var http = new HttpTest())
            {
                NewErrorCodeFixture(http, Constants.Etcd.ErrorCode_NoMorePeer);

                var exception = await Record.ExceptionAsync(CallFixture);
                Assert.IsType<NoMorePeerException>(exception)
                    .IsNoMorePeer.Should().BeTrue();
            }
        }

        [Fact]
        public async Task ShouldThrowNotADirectoryException()
        {
            using (var http = new HttpTest())
            {
                NewErrorCodeFixture(http, Constants.Etcd.ErrorCode_NotDirectory);

                var exception = await Record.ExceptionAsync(CallFixture);
                Assert.IsType<NotADirectoryException>(exception)
                    .IsNotDirectory.Should().BeTrue();
            }
        }

        [Fact]
        public async Task ShouldThrowNotAFileException()
        {
            using (var http = new HttpTest())
            {
                NewErrorCodeFixture(http, Constants.Etcd.ErrorCode_NotFile);

                var exception = await Record.ExceptionAsync(CallFixture);
                Assert.IsType<NotAFileException>(exception)
                    .IsNotFile.Should().BeTrue();
            }
        }

        [Fact]
        public async Task ShouldThrowPreviousValueRequiredException()
        {
            using (var http = new HttpTest())
            {
                NewErrorCodeFixture(http, Constants.Etcd.ErrorCode_PreviousValueRequired);

                var exception = await Record.ExceptionAsync(CallFixture);
                Assert.IsType<PreviousValueRequiredException>(exception)
                    .IsPreviousValueRequired.Should().BeTrue();
            }
        }

        [Fact]
        public async Task ShouldThrowRaftInternalException()
        {
            using (var http = new HttpTest())
            {
                NewErrorCodeFixture(http, Constants.Etcd.ErrorCode_RaftInternal);

                var exception = await Record.ExceptionAsync(CallFixture);
                Assert.IsType<RaftInternalException>(exception)
                    .IsRaftInternal.Should().BeTrue();
            }
        }

        [Fact]
        public async Task ShouldThrowRootIsReadOnlyException()
        {
            using (var http = new HttpTest())
            {
                NewErrorCodeFixture(http, Constants.Etcd.ErrorCode_RootReadOnly);

                var exception = await Record.ExceptionAsync(CallFixture);
                Assert.IsType<RootIsReadOnlyException>(exception)
                    .IsRootReadOnly.Should().BeTrue();
            }
        }

        [Fact]
        public async Task ShouldThrowStandbyInternalException()
        {
            using (var http = new HttpTest())
            {
                NewErrorCodeFixture(http, Constants.Etcd.ErrorCode_StandbyInternal);

                var exception = await Record.ExceptionAsync(CallFixture);
                Assert.IsType<StandbyInternalException>(exception)
                    .IsStandbyInternal.Should().BeTrue();
            }
        }

        [Fact]
        public async Task ShouldThrowTestFailedException()
        {
            using (var http = new HttpTest())
            {
                NewErrorCodeFixture(http, Constants.Etcd.ErrorCode_TestFailed);

                var exception = await Record.ExceptionAsync(CallFixture);
                Assert.IsType<TestFailedException>(exception)
                    .IsTestFailed.Should().BeTrue();
            }
        }

        [Fact]
        public async Task ShouldThrowTimeoutNotANumberException()
        {
            using (var http = new HttpTest())
            {
                NewErrorCodeFixture(http, Constants.Etcd.ErrorCode_TimeoutNotANumber);

                var exception = await Record.ExceptionAsync(CallFixture);
                Assert.IsType<TimeoutNotANumberException>(exception)
                    .IsTimeoutNotANumber.Should().BeTrue();
            }
        }

        [Fact]
        public async Task ShouldThrowTtlNotANumberException()
        {
            using (var http = new HttpTest())
            {
                NewErrorCodeFixture(http, Constants.Etcd.ErrorCode_TtlNotANumber);

                var exception = await Record.ExceptionAsync(CallFixture);
                Assert.IsType<TtlNotANumberException>(exception)
                    .IsTtlNotANumber.Should().BeTrue();
            }
        }

        [Fact]
        public async Task ShouldThrowUnknownErrorException()
        {
            using (var http = new HttpTest())
            {
                NewErrorCodeFixture(http, Constants.Etcd.ErrorCode_Unknown);

                var exception = await Record.ExceptionAsync(CallFixture);
                Assert.IsType<UnknownErrorException>(exception)
                    .IsUnknown.Should().BeTrue();
            }
        }

        [Fact]
        public async Task ShouldThrowValueOrTtlRequiredException()
        {
            using (var http = new HttpTest())
            {
                NewErrorCodeFixture(http, Constants.Etcd.ErrorCode_ValueOrTtlRequired);

                var exception = await Record.ExceptionAsync(CallFixture);
                Assert.IsType<ValueOrTtlRequiredException>(exception)
                    .IsValueOrTtlRequired.Should().BeTrue();
            }
        }

        [Fact]
        public async Task ShouldThrowValueRequiredException()
        {
            using (var http = new HttpTest())
            {
                NewErrorCodeFixture(http, Constants.Etcd.ErrorCode_ValueRequired);

                var exception = await Record.ExceptionAsync(CallFixture);
                Assert.IsType<ValueRequiredException>(exception)
                    .IsValueRequired.Should().BeTrue();
            }
        }

        [Fact]
        public async Task ShouldThrowWatcherClearedException()
        {
            using (var http = new HttpTest())
            {
                NewErrorCodeFixture(http, Constants.Etcd.ErrorCode_WatcherCleared);

                var exception = await Record.ExceptionAsync(CallFixture);
                Assert.IsType<WatcherClearedException>(exception)
                    .IsWatcherCleared.Should().BeTrue();
            }
        }

        #endregion
    }
}
