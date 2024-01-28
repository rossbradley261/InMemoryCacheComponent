namespace InMemoryCacheComponent.cs.Entities;

public sealed class FinbourneCache
{
	private Dictionary<Guid, FinbourneCacheEntity> CacheItems;
	private Dictionary<Guid, DateTime> DeletedCacheItems = new Dictionary<Guid, DateTime>();
	private static int maxSize;
	private static FinbourneCache? _Instance;

	public static FinbourneCache GetInstance(int cacheMaxSize)
	{
		if (_Instance == null)
			_Instance = new FinbourneCache(cacheMaxSize);

		return _Instance;
	}

	private FinbourneCache(int cacheMaxSize)
    {
        CacheItems = new Dictionary<Guid, FinbourneCacheEntity>();
		maxSize = cacheMaxSize;
    }

	#region Get Methods 

	public string GetItem(Guid key)
    {
		var matchedItem = CacheItems.FirstOrDefault(x => x.Key == key);
		if (matchedItem.Value == null) return string.Empty;

		return matchedItem.Value.ItemValue;
    }

	public Dictionary<Guid, FinbourneCacheEntity>? GetAllItems()
	{
        return CacheItems;
	}

	public DateTime? GetRemovedItemDate(Guid key)
	{
		var matchedItem = DeletedCacheItems.FirstOrDefault(x => x.Key == key);
		if (matchedItem.Value == new DateTime()) return null;

		return matchedItem.Value;
	}

	public Dictionary<Guid, DateTime>? GetAllRemovedItems()
	{
		return DeletedCacheItems;
	}

	#endregion

	#region Add Methods 

	public Guid AddItem(string value)
    {
        if (IsFull())
            if (!RemoveLeastUsedItem()) return Guid.Empty;

		var newGuid = Guid.NewGuid();
        var newEntity = new FinbourneCacheEntity(value);

        CacheItems.Add(newGuid, newEntity);

        if(CacheItems.ContainsKey(newGuid)) return newGuid;
        return Guid.Empty;
    }

	#endregion

	#region General Functions 

	private bool IsFull()
    {
        return (CacheItems.Count >= maxSize);
    }

	private bool RemoveLeastUsedItem()
    {
        // order dictionary by last used date
        var leastUsedItem = CacheItems.OrderBy(x => x.Value.LastUsedDate).First();
        CacheItems.Remove(leastUsedItem.Key);

        // if item is confirmed to be removed, add it to the deleted dictionary
        var isRemoved = !(CacheItems.ContainsKey(leastUsedItem.Key));
        if (isRemoved)
            DeletedCacheItems.Add(leastUsedItem.Key, DateTime.Now);

        return isRemoved;
	}

	public static void RemoveCache()
	{
		_Instance = null;
	}

	#endregion
}
