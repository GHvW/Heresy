using System;
using System.Collections.Generic;
using System.Text;

namespace Heresy {

    public interface IEither<A, B>
        where A : notnull
        where B : notnull { 

        public IEither<A, Result> Map<Result>(Func<B, Result> transform) where Result : notnull;

        public B UnwrapOr(B b);

        //public B Unwrap();

        public Result GetOrHandle<Result>(Func<A, Result> handler, Func<B, Result> transform);

        public IEither<Result, B> MapLeft<Result>(Func<A, Result> transform) where Result : notnull;

        public IEither<A, Result> Bind<Result>(Func<B, IEither<A, Result>> transform) where Result : notnull;

        public IEither<A, Result> Or<Result>(IEither<A, Result> r) where Result : notnull;

        public IEither<A, Result> OrElse<Result>(Func<A, IEither<A, Result>> fn) where Result : notnull;

        public bool IsLeft(); // make get?

        public bool IsRight(); // make get?

        public Result Match<Result>(Func<A, Result> lefnFn, Func<B, Result> rightFn);

        //public Result Match<Result>(Action<Result> lefnAction, Action<Result> rightAction);
    }

    public static class Either<A, B>
        where A : notnull 
        where B : notnull { 

        private class Left_ : IEither<A, B>, IEquatable<Left_> {

            private readonly A data;

            public Left_(A data) {
                this.data = data;
            }

            public IEither<A, Result> Bind<Result>(Func<B, IEither<A, Result>> transform) where Result : notnull => (IEither<A, Result>) this;

            public bool IsLeft() => true;

            public bool IsRight() => false;

            public IEither<A, Result> Map<Result>(Func<B, Result> transform) where Result : notnull => (IEither<A, Result>) this;

            public IEither<Result, B> MapLeft<Result>(Func<A, Result> transform) where Result : notnull => Either<Result, B>.Left(transform(this.data));

            public B UnwrapOr(B b) => b;

            //public B Unwrap() => throw new Exception();

            public Result GetOrHandle<Result>(Func<A, Result> handle, Func<B, Result> transform) => handle(this.data);

            public Result Match<Result>(Func<A, Result> leftFn, Func<B, Result> rightFn) => leftFn(this.data);

            public IEither<A, Result> Or<Result>(IEither<A, Result> r) where Result : notnull => r;

            public IEither<A, Result> OrElse<Result>(Func<A, IEither<A, Result>> fn) where Result : notnull => fn(this.data);

            public bool Equals(Left_ other) {
                return data.Equals(other.data);
            }
        }

        private class Right_ : IEither<A, B>, IEquatable<Right_> {

            private readonly B data;

            public Right_(B data) {
                this.data = data;
            }

            public IEither<A, Result> Bind<Result>(Func<B, IEither<A, Result>> transform) where Result : notnull => transform(this.data);

            public bool IsLeft() => false;

            public bool IsRight() => true;

            public IEither<A, Result> Map<Result>(Func<B, Result> transform) where Result : notnull => Either<A, Result>.Right(transform(this.data));

            public IEither<Result, B> MapLeft<Result>(Func<A, Result> transform) where Result : notnull => (IEither<Result, B>) this;

            public B UnwrapOr(B b) => this.data;

            //public B Unwrap() => this.data;

            public Result GetOrHandle<Result>(Func<A, Result> handle, Func<B, Result> transform) => transform(this.data);

            public Result Match<Result>(Func<A, Result> lefnFn, Func<B, Result> rightFn) => rightFn(this.data);

            public IEither<A, Result> Or<Result>(IEither<A, Result> r) where Result : notnull => (IEither<A, Result>) this;

            public IEither<A, Result> OrElse<Result>(Func<A, IEither<A, Result>> fn) where Result : notnull => (IEither<A, Result>) this;

            public bool Equals(Either<A, B>.Right_ other) {
                return this.data.Equals(other.data);
            }
        }

        public static IEither<A, B> Left(A data) => new Left_(data);

        public static IEither<A, B> Right(B data) => new Right_(data);

        public static IEither<A, B> Cond<C>(bool condition, B right, A left) =>
            condition
                ? Either<A, B>.Right(right)
                : Either<A, B>.Left(left);
    }
}
