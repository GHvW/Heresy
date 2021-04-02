using System;
using System.Collections.Generic;
using System.Text;

namespace Heresy {
    public static class FuncExtensions {

        public static Func<A, C> AndThen<A, B, C>(this Func<A, B> f, Func<B, C> g) => 
            (x) => g(f(x));


        public static Func<A, C> Compose<A, B, C>(this Func<B, C> f, Func<A, B> g) => 
            (x) => f(g(x));


        public static Func<(A, B), C> Tupled<A, B, C>(this Func<A, B, C> fn) {
            return data => {
                var (a, b) = data;
                return fn(a, b);
            };
        }


        public static Func<A, Func<B, C>> Curry<A, B, C>(this Func<A, B, C> fn) => 
            (a) => (b) => fn(a, b);


        public static Func<A, Func<B, Func<C, D>>> Curry<A, B, C, D>(this Func<A, B, C, D> fn) => 
            (a) => (b) => (c) => fn(a, b, c);
    }
}
