using System;
using System.Collections.Generic;
using System.Text;

namespace Heresy {

    public interface EitherA<A, B>
        where A : notnull
        where B : notnull {

        public EitherA<A, Out> Map<Out>(Func<B, Out> transform) where Out : notnull;

        public B UnwrapOr(B b);

        //public B Unwrap();

        public B GetOrHandle(Func<A, B> handler);

        public EitherA<Out, B> MapA<Out>(Func<A, Out> transform) where Out : notnull;

        public EitherA<A, Out> Bind<Out>(Func<B, IEither<A, Out>> transform) where Out : notnull;

        public EitherA<A, Out> Or<Out>(IEither<A, Out> r) where Out : notnull;

    }

    public class Left<A, B> : EitherA<A, B>
        where A : notnull
        where B : notnull {
        public EitherA<A, Out> Bind<Out>(Func<B, IEither<A, Out>> transform) where Out : notnull {
            throw new NotImplementedException();
        }

        public B GetOrHandle(Func<A, B> handler) {
            throw new NotImplementedException();
        }

        public EitherA<A, Out> Map<Out>(Func<B, Out> transform) where Out : notnull {
            throw new NotFiniteNumberException();
        }

        public EitherA<Out, B> MapA<Out>(Func<A, Out> transform) where Out : notnull {
            throw new NotImplementedException();
        }

        public EitherA<A, Out> Or<Out>(IEither<A, Out> r) where Out : notnull {
            throw new NotImplementedException();
        }

        public B UnwrapOr(B b) {
            throw new NotImplementedException();
        }
    }

    public class Right<A, B> : EitherA<A, B>
        where A : notnull
        where B : notnull {
        public EitherA<A, Out> Bind<Out>(Func<B, IEither<A, Out>> transform) where Out : notnull {
            throw new NotImplementedException();
        }

        public B GetOrHandle(Func<A, B> handler) {
            throw new NotImplementedException();
        }

        public EitherA<A, Out> Map<Out>(Func<B, Out> transform) where Out : notnull {
            throw new NotImplementedException();
        }

        public EitherA<Out, B> MapA<Out>(Func<A, Out> transform) where Out : notnull {
            throw new NotImplementedException();
        }

        public EitherA<A, Out> Or<Out>(IEither<A, Out> r) where Out : notnull {
            throw new NotImplementedException();
        }

        public B UnwrapOr(B b) {
            throw new NotImplementedException();
        }
    }
}
