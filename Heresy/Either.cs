using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Heresy {

    public interface IEither<A, B>
        where A : notnull
        where B : notnull {

        public B UnwrapOr(B b);

        public B UnwrapOrHandle(Func<A, B> handler);

        // *Functor* Map
        public IEither<A, Out> Select<Out>(Func<B, Out> transform) where Out : notnull;

        public IEither<Out, B> MapLeft<Out>(Func<A, Out> transform) where Out : notnull;

        // *Monad* Bind
        public IEither<A, Out> SelectMany<Out>(Func<B, IEither<A, Out>> transform) where Out : notnull;
        public IEither<A, Out> SelectMany<C, Out>(Func<B, IEither<A, C>> transform, Func<B, C, Out> projection) 
            where C : notnull 
            where Out : notnull;

        public IEither<A, B> Or(IEither<A, B> r);

        public IEither<A, B> And(IEither<A, B> r);

        public IEither<A, B> OrElse(Func<A, IEither<A, B>> fn);

        public bool IsLeft(); // make get?

        public bool IsRight(); // make get?

        public Out Fold<Out>(Func<A, Out> lefnFn, Func<B, Out> rightFn);

        public IEither<A, E> Join<C, D, E>(IEither<A, C> other, Func<B, D> getThisKey, Func<C, D> getOtherKey, Func<B, C, E> mapper)
            where C : notnull
            where D : notnull
            where E : notnull;

        //public Task<IEither<A, Out>> Map<Out>(Func<B, Task<Out>> transform, CancellationToken token) where Out : notnull;
        //public Task<IEither<A, B>> OrElse(Func<A, Task<IEither<A, B>>> fn, CancellationToken token);

        //public Out Fold<Out>(Action<Out> lefnAction, Action<Out> rightAction);
    }

    public static class Either<A, B>
        where A : notnull
        where B : notnull {

        public class Left : IEither<A, B>, IEquatable<Left> {

            private readonly A data;

            public Left(A data) {
                this.data = data;
            }

            // *Monad* Bind
            public IEither<A, Out> SelectMany<Out>(Func<B, IEither<A, Out>> transform) where Out : notnull =>
                new Either<A, Out>.Left(this.data);

            public bool IsLeft() => true;

            public bool IsRight() => false;

            // *Functor* Map
            public IEither<A, Out> Select<Out>(Func<B, Out> transform) where Out : notnull =>
                new Either<A, Out>.Left(this.data);

            public IEither<Out, B> MapLeft<Out>(Func<A, Out> transform) where Out : notnull =>
                new Either<Out, B>.Left(transform(this.data));

            public B UnwrapOr(B b) => b;

            public B UnwrapOrHandle(Func<A, B> handle) => handle(this.data);

            public Out Fold<Out>(Func<A, Out> leftFn, Func<B, Out> rightFn) => leftFn(this.data);

            public IEither<A, B> Or(IEither<A, B> r) => r;

            public IEither<A, B> And(IEither<A, B> r) => new Either<A, B>.Left(this.data);

            public IEither<A, B> OrElse(Func<A, IEither<A, B>> fn) => fn(this.data);

            public bool Equals(Left other) => data.Equals(other.data);

            public IEither<A, Out> SelectMany<C, Out>(Func<B, IEither<A, C>> transform, Func<B, C, Out> projection)
                where C : notnull
                where Out : notnull {

                //throw new NotImplementedException();
                return new Either<A, Out>.Left(this.data);
            }

            public IEither<A, E> Join<C, D, E>(IEither<A, C> other, Func<B, D> getThisKey, Func<C, D> getOtherKey, Func<B, C, E> mapper)
                where C : notnull
                where D : notnull
                where E : notnull {

                return new Either<A, E>.Left(this.data);
            }

            // TODO : make these extensions on Task<IEither<A, B>>?
            //public Task<IEither<A, B>> OrElse(Func<A, Task<IEither<A, B>>> fn, CancellationToken token = default) => fn(this.data);

            //public Task<IEither<A, Out>> Map<Out>(Func<B, Task<Out>> transform, CancellationToken token = default) where Out : notnull {
            //    IEither<A, Out> left = new Either<A, Out>.Left(this.data);
            //    return Task.FromResult(left);
            //}

        }

        public class Right : IEither<A, B>, IEquatable<Right> {

            private readonly B data;

            public Right(B data) {
                this.data = data;
            }

            // *Monad* Bind
            public IEither<A, Out> SelectMany<Out>(Func<B, IEither<A, Out>> transform) where Out : notnull => transform(this.data);

            public bool IsLeft() => false;

            public bool IsRight() => true;

            // *Functor* Map
            public IEither<A, Out> Select<Out>(Func<B, Out> transform) where Out : notnull => new Either<A, Out>.Right(transform(this.data));


            public IEither<Out, B> MapLeft<Out>(Func<A, Out> transform) where Out : notnull => new Either<Out, B>.Right(this.data);

            public B UnwrapOr(B b) => this.data;

            //public B Unwrap() => this.data;

            public B UnwrapOrHandle(Func<A, B> handle) => this.data;

            public Out Fold<Out>(Func<A, Out> lefnFn, Func<B, Out> rightFn) => rightFn(this.data);

            public IEither<A, B> Or(IEither<A, B> r) => new Either<A, B>.Right(this.data);

            public IEither<A, B> And(IEither<A, B> r) =>
                r.Fold<IEither<A, B>>(
                    left => new Either<A, B>.Left(left),
                    right => new Either<A, B>.Right(right));

            public IEither<A, B> OrElse(Func<A, IEither<A, B>> fn) => new Either<A, B>.Right(this.data);

            public bool Equals(Either<A, B>.Right other) => this.data.Equals(other.data);

            public IEither<A, Out> SelectMany<C, Out>(Func<B, IEither<A, C>> transform, Func<B, C, Out> projection)
                where C : notnull
                where Out : notnull {
                //throw new NotImplementedException();

                return transform(this.data)
                        .SelectMany(result => 
                            projection(this.data, result)
                                .Return<A, Out>());
            }

            public IEither<A, E> Join<C, D, E>(IEither<A, C> other, Func<B, D> getThisKey, Func<C, D> getOtherKey, Func<B, C, E> mapper)
                where C : notnull
                where D : notnull
                where E : notnull {
                //throw new NotImplementedException();
                return other.Select(c => {
                    var thisKey = getThisKey(this.data);
                    var otherKey = getOtherKey(c);

                    return EqualityComparer<D>.Default.Equals(thisKey, otherKey)
                        ? mapper(this.data, c)
                        : default;
                });
            }


            // TODO : add this as extension on Task<IEither> ?
            //public Task<IEither<A, B>> OrElse(Func<A, Task<IEither<A, B>>> fn, CancellationToken token = default) {
            //    IEither<A, B> right = new Either<A, B>.Right(this.data);
            //    return Task.FromResult(right);
            //} 

            ////// TODO : Check on this one, Task is a monad, deal with it differently?
            //public async Task<IEither<A, Out>> Map<Out>(Func<B, Task<Out>> transform, CancellationToken token = default) where Out : notnull =>
            //    new Either<A, Out>.Right(await transform(this.data));
        }

        //public static IEither<A, B> Left(A data) => new Left_(data);

        //public static IEither<A, B> Right(B data) => new Right_(data);


        // TODO: look into how do do this without a cast
        public static IEither<A, B> Cond(bool condition, B right, A left) =>
            condition
                ? (IEither<A, B>) new Either<A, B>.Right(right)
                : new Either<A, B>.Left(left);

        //public static IEither<A, B> Do(IEither<A, B> first)
    }

    public static class ToEitherExtensions {

        public static IEither<A, B> Right<A, B>(this B it) 
            where A : notnull 
            where B : notnull 
            => new Either<A, B>.Right(it);

        public static IEither<A, B> Left<A, B>(this A it) 
            where A : notnull 
            where B : notnull 
            => new Either<A, B>.Left(it);

        // *Monad* return
        public static IEither<A, B> Return<A, B>(this B it)
            where A : notnull
            where B : notnull
            => new Either<A, B>.Right(it);
    }
}
