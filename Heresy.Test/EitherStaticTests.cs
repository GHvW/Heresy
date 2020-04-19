using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Heresy.Test {

    public class EitherStaticTests {

        public static (IEither<string, int>, IEither<string, int>) Setup() => (Either<string, int>.Right(10), Either<string, int>.Left("Hello World!"));

        [Fact]
        public void GetOrElse_Test() {

            var (left, right) = Setup();

            var rightResult = right.UnwrapOr(123456789);
            var leftResult = left.UnwrapOr(123456789);

            Assert.Equal(123456789, leftResult);
            Assert.Equal(10, rightResult);
        }

        [Fact]
        public void Match_Test() {
            //var right = Either<string, int>.Right(10);

            //var left = Either<string, int>.Left("Hello World!");
            var (left, right) = Setup();

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

        [Fact]
        public void Map_Test() {

            var (left, right) = Setup();

            var leftResult = left.Map(x => x * 100);

            var rightResult = right.Map(x => x * 100);
            

            //Assert.Equal(Either<string, int>.Left)
        }
    }
}
