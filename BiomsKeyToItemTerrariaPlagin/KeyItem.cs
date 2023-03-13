using System.Collections.Generic;
using Terraria.ID;
using TShockAPI.Handlers.NetModules;

public class KeyItem
{
    public int Id { get; set; }
    public int IdItemChest { get; set; }
    public int Count { get; set; }
    public int Index { get; set; }

}

//ItemID.JungleKey, ItemID.CorruptionKey, ItemID.CrimsonKey, ItemID.HallowedKey, ItemID.FrozenKey, 4714

public class KeyItemsList
{
    private readonly List<KeyItem> _keyItems = new List<KeyItem>();

    public KeyItemsList(Dictionary<int, int> keys)
    {
        foreach (var key in keys.Keys)
        {
            _keyItems.Add(new KeyItem { Id = key });
        }
    }
    public List<KeyItem> GetKeyItems()
    {
        return _keyItems;
    }
}
