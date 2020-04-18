using System;
using System.Collections.Generic;
using System.Text;

namespace Heresy {

    public interface IEither<A, B>
        where A : class
        where B : class {

        public IEither<A, Result> Map<Result>(Func<B, Result> transform) where Result : class;

        public IEither<Result, B> MapLeft<Result>(Func<A, Result> transform) where Result : class;

        public IEither<A, Result> Bind<Result>(Func<B, IEither<A, Result>> transform) where Result : class;

        public IEither<A, Result> Or<Result>(IEither<A, Result> r) where Result : class;

        public IEither<A, Result> OrEsle<Result>(Func<B, IEither<A, Result>> fn) where Result : class;

        public bool IsLeft(); // make get?

        public bool IsRight(); // make get?

        public B? Right();

        public A? Left();

        public Result Match<Result>(Func<A, Result> lefnFn, Func<B, Result> rightFn);

        //public Result Match<Result>(Action<Result> lefnAction, Action<Result> rightAction);
    }

    public static class Either<A, B>
        where A : class
        where B : class {

        private class Left_ : IEither<A, B> {

            private readonly A data;

            public Left_(A data) {
                this.data = data;
            }

            public IEither<A, Result> Bind<Result>(Func<B, IEither<A, Result>> transform) where Result : class => (IEither<A, Result>) this;

            public bool IsLeft() => true;

            public bool IsRight() => false;

            public IEither<A, Result> Map<Result>(Func<B, Result> transform) where Result : class => (IEither<A, Result>) this;

            public IEither<Result, B> MapLeft<Result>(Func<A, Result> transform) where Result : class => Either<Result, B>.Left(transform(this.data));

            public Result Match<Result>(Func<A, Result> leftFn, Func<B, Result> rightFn) => leftFn(this.data);

            public B? Right() => null;

            public A? Left() => this.data;

            public IEither<A, Result> Or<Result>(IEither<A, Result> r) where Result : class {
                throw new NotImplementedException();
            }

            public IEither<A, Result> OrEsle<Result>(Func<B, IEither<A, Result>> fn) where Result : class {
                throw new NotImplementedException();
            }
        }

        private class Right_ : IEither<A, B> {

            private readonly B data;

            public Right_(B data) {
                this.data = data;
            }

            public IEither<A, Result> Bind<Result>(Func<B, IEither<A, Result>> transform) where Result : class => transform(this.data);

            public bool IsLeft() => false;

            public bool IsRight() => true;

            public IEither<A, Result> Map<Result>(Func<B, Result> transform) where Result : class => Either<A, Result>.Right(transform(this.data));

            public IEither<Result, B> MapLeft<Result>(Func<A, Result> transform) where Result : class => (IEither<Result, B>) this;

            public Result Match<Result>(Func<A, Result> lefnFn, Func<B, Result> rightFn) => rightFn(this.data);

            public B? Right() => this.data;

            public A? Left() => null;

            public IEither<A, Result> Or<Result>(IEither<A, Result> r) where Result : class {
                throw new NotImplementedException();
            }

            public IEither<A, Result> OrEsle<Result>(Func<B, IEither<A, Result>> fn) where Result : class {
                throw new NotImplementedException();
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
