using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Heresy.Test {

    public class TaskExtensionsTests {

        [Fact]
        public async Task Select_Query_Test() {

            var result = (from x in Task.FromResult(10)
                          from y in Task.FromResult(100)
                          select x + y);

            Assert.Equal(110, await result);
        }

        
        //[Fact]
        //public async Task Join_Test() {
        //    var person = new { Id = 1, Name = "Dude" };
        //    var gameScore = new { Id = 10, Score = 100, GamerId = 1, Game = "Warcraft 2" };
        //    var prize = new { Id = 20, Name = "Super Awesome 100 Prize", Score = 100 };

        //    var result = (from x in Task.FromResult(person)
        //                  join y in Task.FromResult(gameScore) on x.Id equals y.GamerId
        //                  join z in Task.FromResult(prize) on y.Score equals z.Score
        //                  select (x.Name, y.Game, y.Score, z.Name));

        //    Assert.Equal(("Dude", "Warcraft 2", 100, "Super Awesome 100 Prize"), await result);
        //}

        //[Fact]
        //public async Task Join_Async_Test() {
        //    var person = new { Id = 1, Name = "Dude" };
        //    var gameScore = new { Id = 10, Score = 100, GamerId = 1, Game = "Warcraft 2" };
        //    var prize = new { Id = 20, Name = "Super Awesome 100 Prize", Score = 100 };

        //    var result = (from x in Task.FromResult(person)
        //                  join y in Task.FromResult(gameScore) on x.Id equals y.GamerId
        //                  join z in Task.FromResult(prize) on y.Score equals z.Score
        //                  select (x.Name, y.Game, y.Score, z.Name));

        //    Assert.Equal(("Dude", "Warcraft 2", 100, "Super Awesome 100 Prize"), await result);
        //}
    }
}
