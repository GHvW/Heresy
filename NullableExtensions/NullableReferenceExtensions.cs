﻿using System;

namespace Heresy {
  public static class NullableReferenceExtensions {

    public static U? Map<T, U>(this T? x, Func<T, U> fn)
      where T : class
      where U : class {

      return x switch {
        null => null,
        T it => fn(it)
      };

    }

    public static U? FlatMap<T, U>(this T? x, Func<T, U?> fn)
      where T : class
      where U : class {

      return x switch {
        null => null,
        T it => fn(it)
      };
    }
  }
}
