using Flamtap.Unity;
using Microsoft.Practices.Unity;
using NUnit.Framework;

namespace Flamtap.UnitTests.Unity
{
    public class DecoratorContainerExtensionTests
    {
        public interface IContract
        {
        }

        public class Contract : IContract
        {
        }

        public class ContractDecorator : IContract
        {
            public IContract Base { get; set; }

            public ContractDecorator(IContract @base)
            {
                Base = @base;
            }
        }

        public interface IOtherContract
        {
        }

        public class OtherContract : IOtherContract
        {
        }

        public class MyClass
        {
            public IOtherContract OtherContract { get; set; }

            public MyClass(IOtherContract otherContract)
            {
                OtherContract = otherContract;
            }
        }

        [Test]
        public void HasNoEffectOnCreatingObjectsWithSingleRegistration()
        {
            IUnityContainer c = new UnityContainer().AddExtension(new DecoratorContainerExtension())
                        .RegisterType<IContract, ContractDecorator>()
                        .RegisterType<IContract, Contract>()
                        .RegisterType<IOtherContract, OtherContract>();

            MyClass o = c.Resolve<MyClass>();
            Assert.NotNull(o);
            Assert.NotNull(o.OtherContract);
            Assert.True(o.GetType() == typeof(MyClass));
            Assert.True(o.OtherContract.GetType() == typeof(OtherContract));
        }

        public void CreatesTheMostDependentType()
        {
            IUnityContainer c = new UnityContainer().AddExtension(new DecoratorContainerExtension())
                        .RegisterType<IContract, ContractDecorator>()
                        .RegisterType<IContract, Contract>();

            IContract o = c.Resolve<IContract>();
            Assert.NotNull(o);
            Assert.True(o.GetType() == typeof(ContractDecorator));
            Assert.True(((ContractDecorator) o).Base.GetType() == typeof(Contract));
        }

        public void CanRepeatCreatingTheMostDependentType()
        {
            IUnityContainer c = new UnityContainer().AddExtension(new DecoratorContainerExtension())
                        .RegisterType<IContract, ContractDecorator>()
                        .RegisterType<IContract, Contract>();

            IContract o = c.Resolve<IContract>();
            Assert.NotNull(o);
            Assert.True(o.GetType() == typeof(ContractDecorator));
            Assert.True(((ContractDecorator) o).Base.GetType() == typeof(Contract));

            o = c.Resolve<IContract>();
            Assert.NotNull(o);
            Assert.True(o.GetType() == typeof(ContractDecorator));
            Assert.True(((ContractDecorator) o).Base.GetType() == typeof(Contract));
        }
    }
}
