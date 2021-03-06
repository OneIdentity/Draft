﻿using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using Flurl;
using Flurl.Http.Testing;

using Xunit;

namespace Draft.Tests.Keys
{
    public class QueueRequestTests
    {

        [Fact]
        public async Task ShouldCallTheCorrectUrlByAwaitingImmediately()
        {
            using (var http = new HttpTest())
            {
                http.RespondWithJson(Fixtures.Queue.DefaultResponse);

                await Etcd.ClientFor(Fixtures.EtcdUrl.ToUri())
                    .Atomic
                    .Enqueue(Fixtures.Queue.Path)
                    .WithValue(Fixtures.Queue.DefaultValue);

                http.Should()
                    .HaveCalled(
                        Fixtures.EtcdUrl
                            .AppendPathSegments(Constants.Etcd.Path_Keys, Fixtures.Queue.Path)
                    )
                    .WithVerb(HttpMethod.Post)
                    .WithRequestBody(Fixtures.Queue.DefaultRequest())
                    .Times(1);
            }
        }

        [Fact]
        public async Task ShouldCallTheCorrectUrlWithTtlOption()
        {
            using (var http = new HttpTest())
            {
                http.RespondWithJson(Fixtures.Queue.DefaultResponse);

                await Etcd.ClientFor(Fixtures.EtcdUrl.ToUri())
                    .Atomic
                    .Enqueue(Fixtures.Queue.Path)
                    .WithValue(Fixtures.Queue.DefaultValue)
                    .WithTimeToLive(Fixtures.Queue.DefaultTtl);

                http.Should()
                    .HaveCalled(
                        Fixtures.EtcdUrl
                            .AppendPathSegments(Constants.Etcd.Path_Keys, Fixtures.Queue.Path)
                    )
                    .WithVerb(HttpMethod.Post)
                    .WithRequestBody(Fixtures.Queue.TtlRequest())
                    .Times(1);
            }
        }

        [Fact]
        public async Task ShouldCallTheCorrectUrlWithTtlOptionUsingTimeSpan()
        {
            var expectedTtlValue = TimeSpan.FromMinutes(3) + TimeSpan.FromSeconds(16);
            using (var http = new HttpTest())
            {
                http.RespondWithJson(Fixtures.Queue.DefaultResponse);

                var result = await Etcd.ClientFor(Fixtures.EtcdUrl.ToUri())
                    .Atomic
                    .Enqueue(Fixtures.Queue.Path)
                    .WithValue(Fixtures.Queue.DefaultValue)
                    .WithTimeToLive(expectedTtlValue);

                http.Should()
                    .HaveCalled(
                        Fixtures.EtcdUrl
                            .AppendPathSegments(Constants.Etcd.Path_Keys, Fixtures.Queue.Path)
                    )
                    .WithVerb(HttpMethod.Post)
                    .WithRequestBody(Fixtures.Key.TtlTimeSpanRequest(Fixtures.Queue.DefaultValue, expectedTtlValue))
                    .Times(1);
            }
        }

    }
}
