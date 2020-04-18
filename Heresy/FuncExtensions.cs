using System;
using System.Collections.Generic;
using System.Text;

namespace Heresy {
    public static class FuncExtensions {

        public static Func<B, C> Curry<A, B, C>(this Func<A, B, C> fn, A a) => (b) => fn(a, b);
    }
}
