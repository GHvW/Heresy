using System;
using System.Collections.Generic;
using System.Text;

namespace Heresy {

    public interface IEither<A, B> {
        public IEither<A, Result> Map<Result>(Func<B, Result> transform);
        public IEither<Result, B> MapLeft<Result>(Func<A, Result> transform);
    }

    public static class Either<A, B> {
        private class _Left : IEither<A, B> {

            private readonly A data;

            public _Left(A data) {
                this.data = data;
            }

            public IEither<A, Result> Map<Result>(Func<B, Result> transform) => (IEither<A, Result>) this;

            public IEither<Result, B> MapLeft<Result>(Func<A, Result> transform) => Either<Result, B>.Left(transform(this.data));
        }

        private class _Right : IEither<A, B> {

            private readonly B data;

            public _Right(B data) {
                this.data = data;
            }

            public IEither<A, Result> Map<Result>(Func<B, Result> transform) => Either<A, Result>.Right(transform(this.data));

            public IEither<Result, B> MapLeft<Result>(Func<A, Result> transform) => (IEither<Result, B>) this;
        }

        public static IEither<A, B> Left(A data) => new _Left(data);

        public static IEither<A, B> Right(B data) => new _Right(data);
    }
}
