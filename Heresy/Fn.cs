using System;
using System.Collections.Generic;
using System.Text;

namespace Heresy {
    
    public static class Fn {
        public static Func<A, Func<B, C>> Curry<A, B, C>(Func<A, B, C> fn) => (A a) => (B b) => fn(a, b);
    }
}
