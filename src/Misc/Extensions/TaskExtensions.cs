using System;
using System.Threading.Tasks;

namespace Misc.Extensions
{
    public static class TaskExtensions
    {
        /// <summary>
        ///     Awaits a task, invoking one of two actions based on if the execution of the task throws
        ///     an exception of the specified type.
        /// </summary>
        /// <typeparam name="TException">
        ///     The type of exception that, if thrown, will cause the <paramref name="onError"/> delegate to be
        ///     executed.
        /// </typeparam>
        /// <param name="task">The task to execute.</param>
        /// <param name="onSuccess">
        ///     The delegate that represents the code to execute if the task completes without throwing and exception
        ///     of type <typeparamref name="TException"/>.
        /// </param>
        /// <param name="onError">
        ///     The delegate that represents the code to execute if and eception of type
        ///     <typeparamref name="TException"/> is thrown during execution.
        /// </param>
        /// <returns>The result of the awaited task.</returns>
        public static async Task TryAsync<TException>(this Task task, Action onSuccess, Action<TException> onError)
            where TException : Exception
        {
            try
            {
                await task;
            }
            catch (TException e)
            {
                onError.Invoke(e);
                return;
            }

            onSuccess.Invoke();
        }

        /// <summary>
        ///     Awaits a task, invoking one of two functions based on whether or not the execution of the task throws
        ///     an exception of the specified type.
        /// </summary>
        /// <typeparam name="TResult">The result type of the task.</typeparam>
        /// <typeparam name="TException">
        ///     The type of exception that, if thrown, will cause the <paramref name="onError"/> delegate to be
        ///     executed.
        /// </typeparam>
        /// <param name="task">The task to execute.</param>
        /// <param name="onSuccess">
        ///     The delegate that represents the code to execute if the task completes without throwing and exception
        ///     of type <typeparamref name="TException"/>.
        /// </param>
        /// <param name="onError">
        ///     The delegate that represents the code to execute if and eception of type
        ///     <typeparamref name="TException"/> is thrown during execution.
        /// </param>
        /// <returns>The result of the awaited task.</returns>
        public static async Task<TResult> TryAsync<TResult, TException>(this Task<TResult> task,
            Action<TResult> onSuccess, Action<TException> onError)
            where TException : Exception
        {
            TResult result;

            try
            {
                result = await task;
            }
            catch (TException e)
            {
                onError.Invoke(e);
                return default(TResult);
            }
            
            onSuccess.Invoke(result);

            return result;
        }
    }
}
