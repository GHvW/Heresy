using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Heresy {

    public static class TaskExtensions {

        // TODO : add other methods that tak cancellation tokens? 

        // *Functor* Map
        public static async Task<B> Select<A, B>(this Task<A> it, Func<A, B> transform, CancellationToken token = default) =>
            transform(await it);

        // *Monad* Bind
        // maybe im not completely crazy https://devblogs.microsoft.com/pfxteam/tasks-monads-and-linq/
        public static async Task<B> SelectMany<A, B>(this Task<A> it, Func<A, CancellationToken, Task<B>> transform, CancellationToken token = default) =>
            await transform(await it, token);

        public static async Task<C> SelectMany<A, B, C>(this Task<A> it, Func<A, Task<B>> transform, Func<A, B, C> selector) {
            var a = await it;
            var b = await transform(a);
            return selector(a, b);
        }

        // TODO  - class and struct return
        //public static async Task<D?> Join<A, B, C, D>(this Task<A> it, 
        //                                                  Task<B> other, 
        //                                                  Func<A, C> getItKey, 
        //                                                  Func<B, C> getOtherKey,
        //                                                  Func<A, B, D> mapper) where D : struct {
        //    await Task.WhenAll(it, other); // does this let them complete such that .Result is ok?
        //    var itKey = getItKey(it.Result);
        //    var otherKey = getOtherKey(other.Result);

        //    return EqualityComparer<C>.Default.Equals(itKey, otherKey)
        //        ? mapper(it.Result, other.Result)
        //        : default;
        //}

        // TODO : Combine? Aggregate?
    }

    public static class TaskReturn {

        // *Monad*
        // TODO : consequences of an extension method on everything?
        public static Task<A> Return<A>(this A it) => Task.FromResult(it);
    }
}
