using System;
using System.Threading.Tasks;
using Flamtap.Extensions;
using FluentAssertions;
using NUnit.Framework;

namespace Flamtap.Tests.Extensions
{
    [TestFixture]
    public class TaskExtensionsTester
    {
        [Test]
        public async Task Success_delegate_should_execute_if_task_does_not_throw_exception()
        {
            var result = false;

            await Task.CompletedTask.TryAsync<Exception>(() => result = true, e => result = false);

            result.Should().BeTrue();
        }

        [Test]
        public async Task Success_delegate_should_execute_if_typed_task_does_not_throw_exception()
        {
            var result = false;

            int value = await Task.Run(() => -1).TryAsync<int, Exception>(
                onSuccess: r =>
                {
                    result = true;
                },
                onError: e =>
                {
                    result = false;
                });

            value.ShouldBeEquivalentTo(-1);
            result.Should().BeTrue();
        }

        [Test]
        public async Task Error_delegate_should_execute_if_task_throws_exception()
        {
            var result = true;

            var task = Task.Run(() => throw new Exception());

            await task.TryAsync<Exception>(() => result = true, e => result = false);

            result.Should().BeFalse();
        }

        [Test]
        public async Task Error_delegate_should_execute_if_typed_task_throws_exception()
        {
            var result = true;

            int value = await Task.Run(() =>
            {
                var i = 1;

                if (i == 1)
                    throw new Exception();

                return i;
            }).TryAsync<int, Exception>(i => result = true, e => result = false);

            value.ShouldBeEquivalentTo(0);
            result.Should().BeFalse();
        }

        [Test]
        public async Task Error_delegate_should_execute_if_thrown_exception_is_subtype_of_exception_type1()
        {
            var result = true;

            var task = Task.Run(() => throw new InvalidOperationException());

            await task.TryAsync<Exception>(() => result = true, e => result = false);

            result.Should().BeFalse();
        }

        [Test]
        public async Task Error_delegate_should_execute_if_thrown_exception_is_subtype_of_exception_type2()
        {
            var result = true;

            int value = await Task.Run(() =>
            {
                var i = 1;

                if (i == 1)
                    throw new InvalidOperationException();

                return i;
            }).TryAsync<int, Exception>(i => result = true, e => result = false);

            value.ShouldBeEquivalentTo(0);
            result.Should().BeFalse();
        }

        [Test]
        public void Exception_should_be_thrown_if_execution_throws_a_different_exception_than_specified1()
        {
            Func<Task> func = async () =>
            {
                var task = Task.Run(() => throw new DummyException());

                await task.TryAsync<InvalidOperationException>(() => { }, e => { });
            };

            func.ShouldThrowExactly<DummyException>();
        }

        [Test]
        public void Exception_should_be_thrown_if_execution_throws_a_different_exception_than_specified2()
        {
            Func<Task> func = async () =>
            {
                await Task.Run(() =>
                {
                    var i = 1;

                    if (i == 1)
                        throw new DummyException();

                    return i;
                }).TryAsync<int, InvalidOperationException>(i => { }, e => { });
            };

            func.ShouldThrowExactly<DummyException>();
        }

        private class DummyException : Exception
        {
        }
    }
}
