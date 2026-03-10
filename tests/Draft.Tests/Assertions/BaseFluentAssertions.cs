using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

using Flurl.Http;

namespace Draft.Tests.Assertions
{
    [ExcludeFromCodeCoverage]
    public abstract class BaseFluentAssertions
    {

        protected BaseFluentAssertions(IReadOnlyList<FlurlCall> calls)
        {
            Calls = calls;
        }

        protected IReadOnlyList<FlurlCall> Calls { get; private set; }

        protected List<FlurlCall> FilterCalls(Func<FlurlCall, bool> filter)
        {
            return Calls.Where(filter).ToList();
        }

        protected static bool MatchesPattern(string toCheck, string pattern)
        {
            var regex = Regex.Escape(pattern).Replace("\\*", "(.*)");
            return Regex.IsMatch(toCheck, regex);
        }

        protected string? FirstRequestAbsoluteUri
        {
            get { return Calls.Select(x => x.HttpRequestMessage.RequestUri?.AbsoluteUri).FirstOrDefault(); }
        }

    }
}