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

            var curried1 = ((Func<int, int, int>) Adder.Add).Curry()(10);
            var curried2 = adder.Curry()(10);

            var result1 = curried1(5);
            var result2 = curried2(5);

            Assert.Equal(15, result1);
            Assert.Equal(15, result2);
        }

        [Fact]
        public void Curry2Test() {

            Func<int, int, int> adder = (a, b) => a + b;

            var curried1 = new Func<int, int, int>(Adder.Add).Curry();
            var curried2 = adder.Curry();

            Assert.Equal(15, curried1(5)(10));
            Assert.Equal(15, curried1(10)(5));
        }

        [Fact]
        public void Tupled_AndThen_Test() {

            Func<int, int, int> adder = (a, b) => a + b;

            var fn1 = 
                new Func<int, int, int>(Adder.Add)
                    .Tupled()
                    .AndThen((x) => x * 10);

            var fn2 = adder.Tupled().AndThen((x) => x * 10);

            Assert.Equal(150, fn1((5, 10)));
            Assert.Equal(150, fn2((10, 5)));
        }
    }
}
