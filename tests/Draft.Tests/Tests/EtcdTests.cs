using Xunit;

namespace Draft.Tests
{
    public class EtcdTests
    {

        [Fact]
        public void ShouldThrowArgumentExceptionOnRelativeUri()
        {
            Action action = () => Etcd.ClientFor(new Uri(Fixtures.RelativeEtcdUrl, UriKind.Relative));
            var exception = Record.Exception(action);
            Assert.IsType<ArgumentException>(exception);
        }

    }
}
