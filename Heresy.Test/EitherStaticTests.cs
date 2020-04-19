using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Heresy.Test {
    public class EitherStaticTests {

        [Fact]
        public void Match_Test() {
            var right = Either<string, int>.Right(10);

            var left = Either<string, int>.Left("Hello World!");

            var rightResult =
                right.Match(
                    s => s.Length,
                    r => r + 100);


            var leftResult =
                left.Match(
                    s => s.Length,
                    r => r + 100);

            Assert.Equal(110, rightResult);
            Assert.Equal(12, leftResult);
        }
    }
}
