using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Heresy.Test {

    public class EitherTests {

        public static (IEither<string, int>, IEither<string, int>) Setup() => (new Either<string, int>.Left("Hello World!"), new Either<string, int>.Right(10));

        public class Person {
            public int Id { get; set; }
            public string? Name { get; set; }
        }

        public class GameScore {
            public int Id { get; set; }
            public int Score { get; set; }
            public int GamerId { get; set; }
            public string? Game { get; set; }
        }

        public class Prize {
            public int Id { get; set; }
            public string? Name { get; set; }
            public int Score { get; set; }
        }

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

            var rightResult = right.UnwrapOrHandle(x => x.Length * 100);
            var leftResult = left.UnwrapOrHandle(x => x.Length * 100);

            Assert.Equal(1200, leftResult);
            Assert.Equal(10, rightResult);
        }

        [Fact]
        public void Fold_Test() {

            var (left, right) = Setup();

            var rightResult =
                right.Fold(
                    s => s.Length,
                    r => r + 100);


            var leftResult =
                left.Fold(
                    s => s.Length - 10,
                    r => r - 10);

            Assert.Equal(110, rightResult);
            Assert.Equal(2, leftResult);
        }

        [Fact]
        public void Cond_Test() {

            var right = Either<string, int>.Cond(true, 10, "Hello World!");
            var left = Either<string, int>.Cond(false, 10, "Hello World!");

            Assert.Equal(new Either<string, int>.Left("Hello World!"), left);
            Assert.Equal(new Either<string, int>.Right(10), right);
        }

        [Fact]
        public void Map_Test() {

            var (left, right) = Setup();

            var leftResult = left.Select(x => x * 10);

            var rightResult = right.Select(x => x * 100);

            Assert.Equal(new Either<string, int>.Left("Hello World!"), leftResult);
            Assert.Equal(new Either<string, int>.Right(1000), rightResult);
        }

        [Fact]
        public void Map_Left_Test() {

            var (left, right) = Setup();

            var leftResult = left.MapLeft(x => $"Either {x}");

            var rightResult = right.MapLeft(x => $"Either {x}");

            Assert.Equal(new Either<string, int>.Left("Either Hello World!"), leftResult);
            Assert.Equal(new Either<string, int>.Right(10), rightResult);
        }

        [Fact]
        public void Bind_Test() {

            var (left, right) = Setup();

            var leftResult = left.SelectMany(x => new Either<string, int>.Right(x * 10));

            var rightResult = right.SelectMany(x => new Either<string, int>.Right(x * 100));

            var leftResult2 = left.SelectMany(x => new Either<string, int>.Left("Oops"));

            var rightResult2 = right.SelectMany(x => new Either<string, int>.Left("Oops"));

            Assert.Equal(new Either<string, int>.Left("Hello World!"), leftResult);
            Assert.Equal(new Either<string, int>.Right(1000), rightResult);
            Assert.Equal(new Either<string, int>.Left("Hello World!"), leftResult2);
            Assert.Equal(new Either<string, int>.Left("Oops"), rightResult2);
        }

        [Fact]
        public void Query_Test() {

            var result = (from x in new Either<string, int>.Right(10)
                          from y in new Either<string, int>.Right(5)
                          select x + y);

            var result2 = (from x in new Either<string, int>.Right(10)
                           from y in new Either<string, int>.Right(5)
                           from z in new Either<string, int>.Right(9)
                           select x + y + z);

            Assert.Equal(new Either<string, int>.Right(15), result);
            Assert.Equal(new Either<string, int>.Right(24), result2);
        }

        [Fact]
        public void Query_Failures_Test() {

            var firstFail = (from x in new Either<string, int>.Left("Fail, and fail fast")
                             from y in new Either<string, int>.Right(5)
                             from z in new Either<string, int>.Right(9)
                             select x + y);

            var secondFail = (from x in new Either<string, int>.Right(10)
                              from y in new Either<string, int>.Left("y is a no good")
                              from z in new Either<string, int>.Right(9)
                              select x + y + z);

            var thirdFail = (from x in new Either<string, int>.Right(10)
                             from y in new Either<string, int>.Right(5)
                             from z in new Either<string, int>.Left("so, so close")
                             select x + y + z);

            Assert.Equal(new Either<string, int>.Left("Fail, and fail fast"), firstFail);
            Assert.Equal(new Either<string, int>.Left("y is a no good"), secondFail);
            Assert.Equal(new Either<string, int>.Left("so, so close"), thirdFail);
        }

        [Fact]
        public void Join_Success_Test() {
            var person = new Either<string, Person>.Right(new Person() { Id = 1, Name = "Dude" });
            var gameScore = new Either<string, GameScore>.Right(new GameScore() { Id = 10, Score = 100, GamerId = 1, Game = "Warcraft 2" });
            var prize = new Either<string, Prize>.Right(new Prize() { Id = 20, Name = "Super Awesome 100 Prize", Score = 100 });

            //var result = (from x in new Either<string, Person>.Right(person)
            //              join y in new Either<string, GameScore>.Right(gameScore) on x.Id equals y.GamerId
            //              join z in new Either<string, Prize>.Right(prize) on y.Score equals z.Score
            //              select (x.Name, y.Game, y.Score, z.Name));

            //var result = 
            //    person
            //        .Select(p => {
            //            return gameScore
            //                .Select(g => (g, p))
            //        })

        }

        [Fact]
        public void Join_Query_Success_Test() {
            var person = new Person() { Id = 1, Name = "Dude" };
            var gameScore = new GameScore() { Id = 10, Score = 100, GamerId = 1, Game = "Warcraft 2" };
            var prize = new Prize() { Id = 20, Name = "Super Awesome 100 Prize", Score = 100 };

            var result = (from x in new Either<string, Person>.Right(person)
                          join y in new Either<string, GameScore>.Right(gameScore) on x.Id equals y.GamerId
                          join z in new Either<string, Prize>.Right(prize) on y.Score equals z.Score
                          select (x.Name, y.Game, y.Score, z.Name));

            Assert.Equal(new Either<string, (string, string, int, string)>.Right(("Dude", "Warcraft 2", 100, "Super Awesome 100 Prize")), result);
        }

        [Fact]
        public void Join_Query_Fail_Test() {
            var person = new { Id = 1, Name = "Dude" };
            var gameScore = new { Id = 10, Score = 100, GamerId = 1, Game = "Warcraft 2" };
            var prize = new { Id = 20, Name = "Super Awesome 100 Prize", Score = 100 };
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

            var leftResult = left.Or(new Either<string, int>.Right(200));
            var leftResult2 = left.Or(new Either<string, int>.Left("Oops"));

            var rightResult = right.Or(new Either<string, int>.Right(200));
            var rightResult2 = right.Or(new Either<string, int>.Left("Oops"));

            Assert.Equal(new Either<string, int>.Right(200), leftResult);
            Assert.Equal(new Either<string, int>.Left("Oops"), leftResult2);
            Assert.Equal(new Either<string, int>.Right(10), rightResult);
            Assert.Equal(new Either<string, int>.Right(10), rightResult2);
        }

        [Fact]
        public void OrElse_Test() {

            var (left, right) = Setup();

            var leftResult = left.OrElse(x => new Either<string, int>.Right(x.Length * 100));
            var leftResult2 = left.OrElse(x => new Either<string, int>.Left($"Oops: {x}"));

            var rightResult = right.OrElse(x => new Either<string, int>.Right(x.Length * 200));
            var rightResult2 = right.OrElse(x => new Either<string, int>.Left($"Oops {x}"));

            Assert.Equal(new Either<string, int>.Right(1200), leftResult);
            Assert.Equal(new Either<string, int>.Left("Oops: Hello World!"), leftResult2);
            Assert.Equal(new Either<string, int>.Right(10), rightResult);
            Assert.Equal(new Either<string, int>.Right(10), rightResult2);
        }

        [Fact]
        public void And_Test() {

            var (left, right) = Setup();

            var leftResult = left.And(new Either<string, int>.Right(200));
            var leftResult2 = left.And(new Either<string, int>.Left("Oops"));

            var rightResult = right.And(new Either<string, int>.Right(200));
            var rightResult2 = right.And(new Either<string, int>.Left("Oops"));

            Assert.Equal(new Either<string, int>.Left("Hello World!"), leftResult);
            Assert.Equal(new Either<string, int>.Left("Hello World!"), leftResult2);
            Assert.Equal(new Either<string, int>.Right(200), rightResult);
            Assert.Equal(new Either<string, int>.Left("Oops"), rightResult2);
        }

        //[Fact]
        //public async Task Async_OrElse_Test() {

        //    var (left, right) = Setup();

        //    var leftResult = await left.OrElse(async x => new Either<string, int>.Right(await Task.FromResult(x.Length * 100)));
        //    var leftResult2 = await left.OrElse(async x => new Either<string, int>.Left(await Task.FromResult($"Oops: {x}")));

        //    var rightResult = await right.OrElse(async x => new Either<string, int>.Right(await Task.FromResult(x.Length * 200)));
        //    var rightResult2 = await right.OrElse(async x => new Either<string, int>.Left(await Task.FromResult($"Oops {x}")));

        //    Assert.Equal(new Either<string, int>.Right(1200), leftResult);
        //    Assert.Equal(new Either<string, int>.Left("Oops: Hello World!"), leftResult2);
        //    Assert.Equal(new Either<string, int>.Right(10), rightResult);
        //    Assert.Equal(new Either<string, int>.Right(10), rightResult2);
        //}
    }

    public class ToEitherExtensionsTests {

        [Fact]
        public void Right_Test() {

            var right = 10.Right<string, int>();

            Assert.Equal(new Either<string, int>.Right(10), right);
        }

        [Fact]
        public void Left_Test() {

            var left = "Hello World!".Left<string, int>();

            Assert.Equal(new Either<string, int>.Left("Hello World!"), left);
        }
    }
}
