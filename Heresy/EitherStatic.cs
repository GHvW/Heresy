using System;
using System.Collections.Generic;
using System.Text;

namespace Heresy {

    public interface IEither<A, B>  { 

        public IEither<A, Result> Map<Result>(Func<B, Result> transform);

        public Result GetOrElse<Result>(Result r, Func<B, Result> transform);

        public Result GetOrHandle<Result>(Func<A, Result> handler, Func<B, Result> transform);

        public IEither<Result, B> MapLeft<Result>(Func<A, Result> transform);

        public IEither<A, Result> Bind<Result>(Func<B, IEither<A, Result>> transform);

        public IEither<A, Result> Or<Result>(IEither<A, Result> r);

        public IEither<A, Result> OrElse<Result>(Func<A, IEither<A, Result>> fn);

        public bool IsLeft(); // make get?

        public bool IsRight(); // make get?

        public Result Match<Result>(Func<A, Result> lefnFn, Func<B, Result> rightFn);

        //public Result Match<Result>(Action<Result> lefnAction, Action<Result> rightAction);
    }

    public static class Either<A, B> {

        private class Left_ : IEither<A, B> {

            private readonly A data;

            public Left_(A data) {
                this.data = data;
            }

            public IEither<A, Result> Bind<Result>(Func<B, IEither<A, Result>> transform) => (IEither<A, Result>) this;

            public bool IsLeft() => true;

            public bool IsRight() => false;

            public IEither<A, Result> Map<Result>(Func<B, Result> transform) => (IEither<A, Result>) this;

            public IEither<Result, B> MapLeft<Result>(Func<A, Result> transform) => Either<Result, B>.Left(transform(this.data));

            public Result GetOrElse<Result>(Result r, Func<B, Result> transform) => r;

            public Result GetOrHandle<Result>(Func<A, Result> handle, Func<B, Result> transform) => handle(this.data);

            public Result Match<Result>(Func<A, Result> leftFn, Func<B, Result> rightFn) => leftFn(this.data);

            public IEither<A, Result> Or<Result>(IEither<A, Result> r) => r;

            public IEither<A, Result> OrElse<Result>(Func<A, IEither<A, Result>> fn) => fn(this.data);
        }

        private class Right_ : IEither<A, B> {

            private readonly B data;

            public Right_(B data) {
                this.data = data;
            }

            public IEither<A, Result> Bind<Result>(Func<B, IEither<A, Result>> transform) => transform(this.data);

            public bool IsLeft() => false;

            public bool IsRight() => true;

            public IEither<A, Result> Map<Result>(Func<B, Result> transform) => Either<A, Result>.Right(transform(this.data));

            public IEither<Result, B> MapLeft<Result>(Func<A, Result> transform) => (IEither<Result, B>) this;

            public Result GetOrElse<Result>(Result r, Func<B, Result> transform) => transform(this.data);

            public Result GetOrHandle<Result>(Func<A, Result> handle, Func<B, Result> transform) => transform(this.data);

            public Result Match<Result>(Func<A, Result> lefnFn, Func<B, Result> rightFn) => rightFn(this.data);

            public IEither<A, Result> Or<Result>(IEither<A, Result> r) => (IEither<A, Result>) this;

            public IEither<A, Result> OrElse<Result>(Func<A, IEither<A, Result>> fn) => (IEither<A, Result>) this;
        }

        public static IEither<A, B> Left(A data) => new Left_(data);

        public static IEither<A, B> Right(B data) => new Right_(data);

        public static IEither<A, B> Cond<C>(bool condition, B right, A left) =>
            condition
                ? Either<A, B>.Right(right)
                : Either<A, B>.Left(left);
    }
}
