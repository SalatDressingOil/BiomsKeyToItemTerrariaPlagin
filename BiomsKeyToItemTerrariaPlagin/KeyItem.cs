using System.Collections.Generic;
using Terraria.ID;
using TShockAPI.Handlers.NetModules;
using System.Collections.Generic;

public class KeyItem
{
    public Dictionary<int, KeyItemObj> dict;
    public KeyItem()
    {
        dict = new Dictionary<int, KeyItemObj>()
        {
            {ItemID.JungleKey, new KeyItemObj(ItemID.PiranhaGun, 0, -1)},
            {ItemID.CorruptionKey, new KeyItemObj(ItemID.ScourgeoftheCorruptor, 1, -1)},
            {ItemID.CrimsonKey, new KeyItemObj(ItemID.VampireKnives, 0, -1)},
            {ItemID.HallowedKey, new KeyItemObj(ItemID.RainbowGun, 0, -1)},
            {ItemID.FrozenKey, new KeyItemObj(ItemID.StaffoftheFrostHydra, 0, -1)},
            {4714, new KeyItemObj(4607, 0, -1)} // id пустынного ключа и посоха пустынного тигра которых нет в ItemID
        };
    }

}

public class KeyItemObj
{
    public int itemID;
    public int countKey;
    public int indexKey;

    public KeyItemObj(int itemID, int countKey, int indexKey)
    {
        this.itemID = itemID;
        this.countKey = countKey;
        this.indexKey = indexKey;
    }
}
