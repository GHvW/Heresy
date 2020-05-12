using System;

namespace Heresy {
    public static class NullableReferenceExtensions {

        public static U? Select<T, U>(this T? x, Func<T, U> fn)
          where T : class
          where U : class {

            return x switch
            {
                null => null,
                T it => fn(it)
            };

        }

        public static U? SelectMany<T, U>(this T? x, Func<T, U?> fn)
            where T : class
            where U : class {

            return x switch
            {
                null => null,
                T it => fn(it)
            };
        }
    }

    public static class NullableValueExtensions {

        public static B? Select<A, B>(this A? x, Func<A, B> fn)
            where A : struct
            where B : struct {

            return x switch {
                null => null,
                A it => fn(it)
            };
        }

        public static U? SelectMany<T, U>(this T? x, Func<T, U?> fn)
            where T : struct
            where U : struct {

            return x switch {
                null => null,
                T it => fn(it)
            };
        }
    }

    public static class NullableValueToReferenceExtensions {

        public static B? Select<A, B>(this A? x, Func<A, B> fn)
            where A : struct
            where B : class {

            return x switch {
                null => null,
                A it => fn(it)
            };
        }

        public static U? SelectMany<T, U>(this T? x, Func<T, U?> fn)
            where T : struct
            where U : class {

            return x switch {
                null => null,
                T it => fn(it)
            };
        }
    }

    public static class NullableReferenceToValueExtensions {

        public static B? Select<A, B>(this A? x, Func<A, B> fn)
            where A : class
            where B : struct {

            return x switch {
                null => null,
                A it => fn(it)
            };
        }

        public static U? SelectMany<T, U>(this T? x, Func<T, U?> fn)
            where T : class
            where U : struct {

            return x switch {
                null => null,
                T it => fn(it)
            };
        }
    }
}
