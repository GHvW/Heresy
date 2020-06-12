using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Xunit;

namespace Heresy.Test {

    public class NullableReferenceExtensionsTest {
        class PointC {
            public int X { get; }
            public int Y { get; }

            public PointC(int x, int y) {
                this.X = x;
                this.Y = y;
            }
        }
        struct PointS {
            public int X { get; }
            public int Y { get; }

            public PointS(int x, int y) {
                this.X = x;
                this.Y = y;
            }
        }

        class Person {
            public string FirstName { get; }
            public string LastName { get; }

            public Person(string first, string last) {
                FirstName = first;
                LastName = last;
            }
        }

        class Car {
            public Person? Driver { get; set; }

            public Car() { }
            public Car(Person driver) {
                Driver = driver;
            }

            public Car? Wreck(bool isTotaled) =>
              isTotaled switch {
                  true => null,
                  false => new Car()
              };
        }

        [Fact]
        public void SelectTest() {

            Person? person = null;

            Car? car = person.Select(p => new Car(p));

            Assert.Null(car);

            Person? person2 = new Person("Garrett", "Van");

            Car? car2 = person2.Select(p => new Car(p));

            Assert.Equal("Garrett", car2?.Driver?.FirstName);
            Assert.Equal("Van", car2?.Driver?.LastName);
        }

        [Fact]
        public void SelectManyTest() {
            Person? person = null;

            Car? car = person.SelectMany(p => {
                return new Car(p).Wreck(false);
            });

            Assert.Null(car);

            Car? car2 = person.SelectMany(p => {
                return new Car(p).Wreck(true);
            });

            Assert.Null(car2);

            Person? person2 = new Person("Garrett", "Van");

            Car? car3 = person2.SelectMany(p => {
                return new Car(p).Wreck(true);
            });

            Assert.Null(car3);

            Car? car4 = person2.SelectMany(p => {
                return new Car(p).Wreck(false);
            });

            Assert.NotNull(car4);
        }

        [Fact]
        public void Query_Test() {
            PointC? p1 = new PointC(1, 2);
            PointC? p2 = null;
            PointC? p3 = new PointC(4, 5);
            PointC? p4 = new PointC(10, 20);

            var result1 = (from x in p2
                           from y in p1
                           select new { First = x, Second = y });

            var result2 = (from x in p1
                           from y in p3
                           from z in p4
                           select new { First = x, Second = y, Third = z });

            var result3 = (from x in p1
                           from y in p3
                           from z in p2
                           select new { First = x, Second = y, Third = z });

            Assert.Null(result1);
            Assert.Null(result3);
            Assert.Equal(1, result2.First.X);
            Assert.Equal(4, result2.Second.X);
            Assert.Equal(10, result2.Third.X);
        }
    }

    public class NullableValueExtensionsTest {

        [Fact]
        public void Select_Test() {

            int? item = 10;

            var res = item.Select(x => x * 10);

            int? nullItem = null;

            var nullRes = nullItem.Select(x => x * 100);

            Assert.Equal(100, res);
            Assert.Null(nullRes);
        }
    }
}
