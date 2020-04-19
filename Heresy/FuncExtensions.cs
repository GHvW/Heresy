using System;
using System.Collections.Generic;
using System.Text;

namespace Heresy {
    public static class FuncExtensions {

        //public static Func<A, Func<B, C>> Curry<A, B, C>(this Func<A, B, C> fn) => (a) => (b) => fn(a, b);

        public static Func<B, C> Curry<A, B, C>(this Func<A, B, C> fn, A a) => (b) => fn(a, b);

        public static Func<B, Func<C, D>> Curry<A, B, C, D>(this Func<A, B, C, D> fn, A a) => (b) => (c) => fn(a, b, c);
    }
}
