using System.Linq;
using Misc.Extensions;

namespace Misc.Tests.Extensions
{
    public class ObjectExtensionsTester
    {
        [Test]
        public void Yield_returns_collection_of_object_type()
        {
            1.Yield().Should().AllBeOfType<int>();
            "".Yield().Should().AllBeOfType<string>();
        }

        [Test]
        public void Yield_returns_collection_with_single_item()
        {
            1.Yield().Count().ShouldBeEquivalentTo(1);
            new[] {1, 2, 3}.Yield().Count().ShouldBeEquivalentTo(1);
        }
    }
}
