using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DavidFidge.TestInfrastructure
{
    [TestClass]
    public abstract class BaseTest
    {
        [TestInitialize]
        public virtual void Setup()
        {
        }

        [TestCleanup]
        public virtual void TearDown()
        {
        }
    }
}