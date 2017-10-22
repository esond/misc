using System;
using System.Collections.Generic;
using Unity.Builder;
using Unity.Builder.Strategy;
using Unity.Resolution;

namespace Flamtap.Unity
{
    public class DecoratorBuildStrategy : BuilderStrategy
    {
        private readonly Dictionary<Type, List<Type>> _typeStacks;

        public DecoratorBuildStrategy(Dictionary<Type, List<Type>> typeStacks) => _typeStacks = typeStacks;

        public override void PreBuildUp(IBuilderContext context)
        {
            var key = context.OriginalBuildKey;

            if (!(key.Type.IsInterface && _typeStacks.ContainsKey(key.Type)))
                return;

            if (null != context.GetOverriddenResolver(key.Type))
                return;

            var stack = new Stack<Type>(
                _typeStacks[key.Type]
            );

            object value = null;

            foreach (var type in stack)
            {
                value = context.NewBuildUp(new NamedTypeBuildKey(type, key.Name));
                var overrides = new DependencyOverride(key.Type, value);
                context.AddResolverOverrides(overrides);
            }

            context.Existing = value;
            context.BuildComplete = true;
        }
    }
}
