using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Heresy {

    public interface IEither<A, B>
        where A : notnull
        where B : notnull {

        public B UnwrapOr(B b);

        //public B Unwrap();

        public B UnwrapOrHandle(Func<A, B> handler);

        public IEither<A, Out> Map<Out>(Func<B, Out> transform) where Out : notnull;

        //public Task<IEither<A, Out>> Map<Out>(Func<B, Task<Out>> transform) where Out : notnull;

        public IEither<Out, B> MapLeft<Out>(Func<A, Out> transform) where Out : notnull;

        public IEither<A, Out> Bind<Out>(Func<B, IEither<A, Out>> transform) where Out : notnull;

        public IEither<A, B> Or(IEither<A, B> r);

        public IEither<A, B> OrElse(Func<A, IEither<A, B>> fn);

        //public Task<IEither<A, B>> OrElse(Func<A, Task<IEither<A, B>>> fn);

        public bool IsLeft(); // make get?

        public bool IsRight(); // make get?

        public Out Fold<Out>(Func<A, Out> lefnFn, Func<B, Out> rightFn);

        //public Out Fold<Out>(Action<Out> lefnAction, Action<Out> rightAction);
    }

    public static class Either<A, B>
        where A : notnull
        where B : notnull {

        private class Left_ : IEither<A, B>, IEquatable<Left_> {

            private readonly A data;

            public Left_(A data) {
                this.data = data;
            }

            public IEither<A, Out> Bind<Out>(Func<B, IEither<A, Out>> transform) where Out : notnull =>
                Either<A, Out>.Left(this.data);

            public bool IsLeft() => true;

            public bool IsRight() => false;

            public IEither<A, Out> Map<Out>(Func<B, Out> transform) where Out : notnull =>
                Either<A, Out>.Left(this.data);

            //public Task<IEither<A, Out>> Map<Out>(Func<B, Task<Out>> transform) where Out : notnull =>
            //    Task.FromResult(Either<A, Out>.Left(this.data));

            public IEither<Out, B> MapLeft<Out>(Func<A, Out> transform) where Out : notnull =>
                Either<Out, B>.Left(transform(this.data));

            public B UnwrapOr(B b) => b;

            //public B Unwrap() => throw new Exception();

            public B UnwrapOrHandle(Func<A, B> handle) => handle(this.data);

            public Out Fold<Out>(Func<A, Out> leftFn, Func<B, Out> rightFn) => leftFn(this.data);

            public IEither<A, B> Or(IEither<A, B> r) => r;

            public IEither<A, B> OrElse(Func<A, IEither<A, B>> fn) => fn(this.data);

            public bool Equals(Left_ other) => data.Equals(other.data);

            //public Task<IEither<A, B>> OrElse(Func<A, Task<IEither<A, B>>> fn) => fn(this.data);
        }

        private class Right_ : IEither<A, B>, IEquatable<Right_> {

            private readonly B data;

            public Right_(B data) {
                this.data = data;
            }

            public IEither<A, Out> Bind<Out>(Func<B, IEither<A, Out>> transform) where Out : notnull => transform(this.data);

            public bool IsLeft() => false;

            public bool IsRight() => true;

            public IEither<A, Out> Map<Out>(Func<B, Out> transform) where Out : notnull => Either<A, Out>.Right(transform(this.data));

            // TODO : Check on this one
            //public async Task<IEither<A, Out>> Map<Out>(Func<B, Task<Out>> transform) where Out : notnull =>
            //    Either<A, Out>.Right(await transform(this.data));

            public IEither<Out, B> MapLeft<Out>(Func<A, Out> transform) where Out : notnull => Either<Out, B>.Right(this.data);

            public B UnwrapOr(B b) => this.data;

            //public B Unwrap() => this.data;

            public B UnwrapOrHandle(Func<A, B> handle) => this.data;

            public Out Fold<Out>(Func<A, Out> lefnFn, Func<B, Out> rightFn) => rightFn(this.data);

            public IEither<A, B> Or(IEither<A, B> r) => Either<A, B>.Right(this.data);

            public IEither<A, B> OrElse(Func<A, IEither<A, B>> fn) => Either<A, B>.Right(this.data);

            public Task<IEither<A, B>> OrElse(Func<A, Task<IEither<A, B>>> fn) =>
                Task.FromResult(Either<A, B>.Right(this.data));

            public bool Equals(Either<A, B>.Right_ other) => this.data.Equals(other.data);
        }

        public static IEither<A, B> Left(A data) => new Left_(data);

        public static IEither<A, B> Right(B data) => new Right_(data);

        public static IEither<A, B> Cond(bool condition, B right, A left) =>
            condition
                ? Either<A, B>.Right(right)
                : Either<A, B>.Left(left);

        //public static IEither<A, B> Do(IEither<A, B> first)
    }

    public static class ToEitherExtensions {

        public static IEither<A, B> Right<A, B>(this B it) 
            where A : notnull 
            where B : notnull 
            => Either<A, B>.Right(it);

        public static IEither<A, B> Left<A, B>(this A it) 
            where A : notnull 
            where B : notnull 
            => Either<A, B>.Left(it);
    }
}
