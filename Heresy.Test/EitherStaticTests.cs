using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Heresy.Test {

    public class EitherStaticTests {

        public static (IEither<string, int>, IEither<string, int>) Setup() => (Either<string, int>.Left("Hello World!"), Either<string, int>.Right(10));

        [Fact]
        public void GetOrElse_Test() {

            var (left, right) = Setup();

            var rightResult = right.UnwrapOr(123456789);
            var leftResult = left.UnwrapOr(123456789);

            Assert.Equal(123456789, leftResult);
            Assert.Equal(10, rightResult);
        }

        [Fact]
        public void GetOrHandle_Test() {

            var (left, right) = Setup();

            var rightResult = right.GetOrHandle(x => x.Length * 100);
            var leftResult = left.GetOrHandle(x => x.Length * 100);

            Assert.Equal(1200, leftResult);
            Assert.Equal(10, rightResult);
        }

        [Fact]
        public void Match_Test() {

            var (left, right) = Setup();

            var rightResult =
                right.Match(
                    s => s.Length,
                    r => r + 100);


            var leftResult =
                left.Match(
                    s => s.Length - 10,
                    r => r - 10);

            Assert.Equal(110, rightResult);
            Assert.Equal(2, leftResult);
        }

        [Fact]
        public void Cond_Test() {

            var right = Either<string, int>.Cond(true, 10, "Hello World!");
            var left = Either<string, int>.Cond(false, 10, "Hello World!");

            Assert.Equal(Either<string, int>.Left("Hello World!"), left);
            Assert.Equal(Either<string, int>.Right(10), right);
        }

        [Fact]
        public void Map_Test() {

            var (left, right) = Setup();

            var leftResult = left.Map(x => x * 10);

            var rightResult = right.Map(x => x * 100);

            Assert.Equal(Either<string, int>.Left("Hello World!"), leftResult);
            Assert.Equal(Either<string, int>.Right(1000), rightResult);
        }

        [Fact]
        public void Map_Left_Test() {

            var (left, right) = Setup();

            var leftResult = left.MapLeft(x => $"Either {x}");

            var rightResult = right.MapLeft(x => $"Either {x}");

            Assert.Equal(Either<string, int>.Left("Either Hello World!"), leftResult);
            Assert.Equal(Either<string, int>.Right(10), rightResult);
        }

        [Fact]
        public void Bind_Test() {

            var (left, right) = Setup();

            var leftResult = left.Bind(x => Either<string, int>.Right(x * 10));

            var rightResult = right.Bind(x => Either<string, int>.Right(x * 100));

            var leftResult2 = left.Bind(x => Either<string, int>.Left("Oops"));

            var rightResult2 = right.Bind(x => Either<string, int>.Left("Oops"));

            Assert.Equal(Either<string, int>.Left("Hello World!"), leftResult);
            Assert.Equal(Either<string, int>.Right(1000), rightResult);
            Assert.Equal(Either<string, int>.Left("Hello World!"), leftResult2);
            Assert.Equal(Either<string, int>.Left("Oops"), rightResult2);
        }

        [Fact]
        public void IsLeft_Test() {

            var (left, right) = Setup();

            var leftResult = left.IsLeft();

            var rightResult = right.IsLeft();

            Assert.True(leftResult);
            Assert.False(rightResult);
        }

        [Fact]
        public void IsRight_Test() {

            var (left, right) = Setup();

            var leftResult = left.IsRight();

            var rightResult = right.IsRight();

            Assert.False(leftResult);
            Assert.True(rightResult);
        }

        [Fact]
        public void Or_Test() {

            var (left, right) = Setup();

            var leftResult = left.Or(Either<string, int>.Right(200));
            var leftResult2 = left.Or(Either<string, int>.Left("Oops")); 

            var rightResult = right.Or(Either<string, int>.Right(200));
            var rightResult2 = right.Or(Either<string, int>.Left("Oops"));

            Assert.Equal(Either<string, int>.Right(200), leftResult);
            Assert.Equal(Either<string, int>.Left("Oops"), leftResult2);
            Assert.Equal(Either<string, int>.Right(10), rightResult);
            Assert.Equal(Either<string, int>.Right(10), rightResult2);
        }

        [Fact]
        public void OrElse_Test() {

            var (left, right) = Setup();

            var leftResult = left.OrElse(x => Either<string, int>.Right(x.Length * 100));
            var leftResult2 = left.OrElse(x => Either<string, int>.Left($"Oops: {x}")); 

            var rightResult = right.OrElse(x => Either<string, int>.Right(x.Length * 200));
            var rightResult2 = right.OrElse(x => Either<string, int>.Left($"Oops {x}"));

            Assert.Equal(Either<string, int>.Right(1200), leftResult);
            Assert.Equal(Either<string, int>.Left("Oops: Hello World!"), leftResult2);
            Assert.Equal(Either<string, int>.Right(10), rightResult);
            Assert.Equal(Either<string, int>.Right(10), rightResult2);
        }

        [Fact]
        public async Task Async_OrElse_Test() {

            var (left, right) = Setup();

            var leftResult = await left.OrElse(async x => Either<string, int>.Right(await Task.FromResult(x.Length * 100)));
            var leftResult2 = await left.OrElse(async x => Either<string, int>.Left(await Task.FromResult($"Oops: {x}")));

            var rightResult = await right.OrElse(async x => Either<string, int>.Right(await Task.FromResult(x.Length * 200)));
            var rightResult2 = await right.OrElse(async x => Either<string, int>.Left(await Task.FromResult($"Oops {x}")));

            Assert.Equal(Either<string, int>.Right(1200), leftResult);
            Assert.Equal(Either<string, int>.Left("Oops: Hello World!"), leftResult2);
            Assert.Equal(Either<string, int>.Right(10), rightResult);
            Assert.Equal(Either<string, int>.Right(10), rightResult2);
        }
    }
}
