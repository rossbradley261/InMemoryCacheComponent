namespace InMemoryCacheComponent.cs.Entities;

public class FinbourneCacheEntity
{
    // object to be serialized and stored as a string on the cache 
	private string _value;
    public DateTime LastUsedDate { get; set; }
    public string ItemValue { get { UseItemValue(); return _value; } set { UseItemValue(); _value = value; } }

    public FinbourneCacheEntity(string value)
    {
        ItemValue = value;
    }

    public void Update(string value)
    {
        ItemValue = value;
    }

    private void UseItemValue()
    {
        LastUsedDate = DateTime.Now;
    }
}
