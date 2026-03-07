using System.Diagnostics.CodeAnalysis;

using FluentAssertions.Execution;

using Flurl.Http;

namespace Draft.Tests.Assertions
{
    [ExcludeFromCodeCoverage]
    public class HttpTestFluentAssertions : BaseFluentAssertions
    {

        public HttpTestFluentAssertions(IReadOnlyList<FlurlCall> calls) : base(calls) { }

        public HttpCallFluentAssertions HaveCalled(string urlPattern, string because = "", params object[] reasonArgs)
        {
            var matchingCalls = FilterCalls(x => MatchesPattern(x.HttpRequestMessage.RequestUri?.AbsoluteUri ?? string.Empty, urlPattern));
            AssertionChain.GetOrCreate()
                .ForCondition(matchingCalls.Any())
                .BecauseOf(because, reasonArgs)
                .FailWith("Expected {context:IEtcdClient} to have called {0}{reason}, but did not find any calls.", urlPattern);

            return new HttpCallFluentAssertions(matchingCalls);
        }

        public HttpCallFluentAssertions NotHaveCalled(string urlPattern, string because = "", params object[] reasonArgs)
        {
            var matchingCalls = FilterCalls(x => MatchesPattern(x.HttpRequestMessage.RequestUri?.AbsoluteUri ?? string.Empty, urlPattern));
            AssertionChain.GetOrCreate()
                .ForCondition(!matchingCalls.Any())
                .BecauseOf(because, reasonArgs)
                .FailWith("Expected {context:IEtcdClient} to not have called {0}{reason}, but found {1} calls.", urlPattern, matchingCalls.Count);

            return new HttpCallFluentAssertions(matchingCalls);
        }

    }
}
