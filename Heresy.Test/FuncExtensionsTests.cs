using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Heresy.Test {
    public class FuncExtensionsTests {

        public static class Adder {

            public static int Add(int a, int b) => a + b;
        }


        [Fact]
        public void Curry1Test() {

            Func<int, int, int> adder = (a, b) => a + b;

            var curried1 = ((Func<int, int, int>) Adder.Add).Curry(10);
            var curried2 = adder.Curry(10);

            var result1 = curried1(5);
            var result2 = curried2(5);

            Assert.Equal(15, result1);
            Assert.Equal(15, result2);
        }

        [Fact]
        public void StaticCurryTest() {

            Func<int, int, int> adder = (a, b) => a + b;

            var curried1 = Fn.Curry<int, int, int>(Adder.Add);
            var curried2 = Fn.Curry(adder);

            Assert.Equal(15, curried1(5)(10));
            Assert.Equal(15, curried2(10)(5));
        }
    }
}
