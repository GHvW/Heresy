using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Heresy {

    public static class TaskExtensions {

        // should all of these take cancellation tokens?

        // *Functor*
        public static async Task<B> Map<A, B>(this Task<A> it, Func<A, B> transform, CancellationToken token = default) =>
            transform(await it);

        // *Monad*
        // maybe im not completely crazy https://devblogs.microsoft.com/pfxteam/tasks-monads-and-linq/
        public static async Task<B> Bind<A, B>(this Task<A> it, Func<A, CancellationToken, Task<B>> transform, CancellationToken token = default) =>
            await transform(await it, token);

        //public static async Task<A> Or<A>(this Task<A> it, Task<A> other, CancellationToken token = default) {
        //    var winningTask = await Task.WhenAny(it, other);
        //    return await winningTask;
        //}

        //public static async Task<A> And<A>(this Task<A> it, Task<A> other, CancellationToken token = default) {
        //    //var tasks = new Task<A>[] { it, other };
        //    await Task.WhenAll(it, other);
        //    return await other;
        //}
    }

    public static class TaskReturn {

        // *Monad*
        // TODO : consequences of an extension method on everything?
        public static Task<A> Return<A>(this A it) => Task.FromResult(it);
    }
}
