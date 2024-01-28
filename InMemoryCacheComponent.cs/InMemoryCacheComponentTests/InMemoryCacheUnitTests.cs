using InMemoryCacheComponent.cs.Entities;

namespace InMemoryCacheComponentTests
{
	[TestClass]
	public class InMemoryCacheUnitTests
	{
		[TestMethod]
		public void Add_BasicData_NewCacheItemAdded()
		{
			// create cache with 1 item and retrieve it
			var cache = FinbourneCache.GetInstance(1);
			var key1 = cache.AddItem("1");
			var retrieveItem = cache.GetItem(key1);

			Assert.AreEqual("1", retrieveItem);

			FinbourneCache.RemoveCache();
		}

		[TestMethod]
		public void Add_PersonObject_NewCacheItemAdded()
		{
			// create cache with 1 item and retrieve it
			var cache = FinbourneCache.GetInstance(1);

			var person = CreatePerson(30, "Bob");
			var personSerialized = System.Text.Json.JsonSerializer.Serialize(person);

			var personKey = cache.AddItem(personSerialized);
			var retrieveItem = cache.GetItem(personKey);

			Assert.AreEqual(personSerialized, retrieveItem);

			FinbourneCache.RemoveCache();
		}

		[TestMethod]
		public void Add_CarObject_NewCacheItemAdded()
		{
			// create cache with 1 item and retrieve it
			var cache = FinbourneCache.GetInstance(1);

			var car = CreateCar(2015, "Red");
			var carSerialized = System.Text.Json.JsonSerializer.Serialize(car);

			var carKey = cache.AddItem(carSerialized);
			var retrieveItem = cache.GetItem(carKey);

			Assert.AreEqual(carSerialized, retrieveItem);

			FinbourneCache.RemoveCache();
		}

		[TestMethod]
		public void Add_MultipleObjects_NewCacheItemsAdded()
		{
			// create cache with 1 item and retrieve it
			var cache = FinbourneCache.GetInstance(3);

			var car1 = CreateCar(2015, "Red");
			var car2 = CreateCar(2019, "Blue");
			var person1 = CreatePerson(35, "Jim");
			var car1Serialized = System.Text.Json.JsonSerializer.Serialize(car1);
			var car2Serialized = System.Text.Json.JsonSerializer.Serialize(car2);
			var person1Serialized = System.Text.Json.JsonSerializer.Serialize(person1);

			var car1Key = cache.AddItem(car1Serialized);
			var car2Key = cache.AddItem(car2Serialized);
			var person1Key = cache.AddItem(person1Serialized);
			var retrieveCar1 = cache.GetItem(car1Key);
			var retrieveCar2 = cache.GetItem(car2Key);
			var retrievePerson1 = cache.GetItem(person1Key);

			Assert.AreEqual(car1Serialized, retrieveCar1);
			Assert.AreEqual(car2Serialized, retrieveCar2);
			Assert.AreEqual(person1Serialized, retrievePerson1);

			FinbourneCache.RemoveCache();
		}

		[TestMethod]
		public void Add_LimitHit_LeastRecentlyUsedItemRemoved()
		{
			// create cache with items 1-3
			var cache = FinbourneCache.GetInstance(1);
			var key1 = cache.AddItem("1");
			var key2 = cache.AddItem("2");
			var key3 = cache.AddItem("3");

			// touch all cache items expect key2
			cache.GetItem(key1);
			cache.GetItem(key3);

			// add new item 
			var key4 = cache.AddItem("4");

			// attempt to retrieve key2, it should be removed therefore the returned value should be string.empty
			var retrieveItem = cache.GetItem(key2);

			Assert.AreEqual(string.Empty, retrieveItem);

			FinbourneCache.RemoveCache();
		}

		[TestMethod]
		public void GetItem_ItemRemoved_GetDateWhenItemWasRemoved()
		{
			// create cache with items 1-3
			var cache = FinbourneCache.GetInstance(1);
			var key1 = cache.AddItem("1");
			cache.AddItem("2");

			// attempt to retrieve key1, it should be removed therefore the returned value should be string.empty
			var retrieveItem = cache.GetItem(key1);
			var removedItemDate = new DateTime?();
			if (string.IsNullOrWhiteSpace(retrieveItem))
				removedItemDate = cache.GetRemovedItemDate(key1);

			Assert.IsNotNull(removedItemDate);

			FinbourneCache.RemoveCache();
		}

		[TestMethod]
		public void Singleton_AttemptMultipleInstances_OnlyOneAllowed()
		{
			// create cache with 1 item and retrieve it
			var cache = FinbourneCache.GetInstance(1);
			cache.AddItem("1");

			// attempt to create another instance 
			var cache2 = FinbourneCache.GetInstance(5);
			cache.AddItem("2");
			Assert.AreEqual(cache.GetAllItems()?.Count, cache2.GetAllItems()?.Count);

			FinbourneCache.RemoveCache();
		}

		private Car CreateCar(int carYear, string carColour)
		{
			return new Car()
			{
				Year = carYear,
				Colour = carColour
			};
		}

		private Person CreatePerson(int personAge, string personName)
		{
			return new Person()
			{
				Age = personAge,
				Name = personName
			};
		}
	}
}