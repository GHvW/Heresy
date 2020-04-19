using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Heresy.Test {

    public class NullableReferenceExtensionsTest {
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
              isTotaled switch
              {
                  true => null,
                  false => new Car()
              };
        }

        [Fact]
        public void MapTest() {

            Person? person = null;

            Car? car = person.Map(p => new Car(p));

            Assert.Null(car);

            Person? person2 = new Person("Garrett", "Van");

            Car? car2 = person2.Map(p => new Car(p));

            Assert.Equal("Garrett", car2?.Driver?.FirstName);
            Assert.Equal("Van", car2?.Driver?.LastName);
        }

        [Fact]
        public void FlatMapTest() {
            Person? person = null;

            Car? car = person.FlatMap(p => {
                return new Car(p).Wreck(false);
            });

            Assert.Null(car);

            Car? car2 = person.FlatMap(p => {
                return new Car(p).Wreck(true);
            });

            Assert.Null(car2);

            Person? person2 = new Person("Garrett", "Van");

            Car? car3 = person2.FlatMap(p => {
                return new Car(p).Wreck(true);
            });

            Assert.Null(car3);

            Car? car4 = person2.FlatMap(p => {
                return new Car(p).Wreck(false);
            });

            Assert.NotNull(car4);
        }
    }

    public class NullableValueExtensionsTest {

        [Fact]
        public void Map_Test() {

            int? item = 10;

            var res = item.Map(x => x * 10);

            int? nullItem = null;

            var nullRes = nullItem.Map(x => x * 100);

            Assert.Equal(100, res);
            Assert.Null(nullRes);
        }
    }
}
