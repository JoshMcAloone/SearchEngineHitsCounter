using Autofac.Extras.Moq;

namespace SearchEngineHitCounterTests
{
    public class TestBase
    {
        protected AutoMock GetAutoMock()
        {
            var mock = AutoMock.GetLoose();
            return mock;
        }
    }
}
