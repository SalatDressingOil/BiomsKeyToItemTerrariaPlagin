using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using TerrariaApi.Server;
using TShockAPI;
using static Terraria.GameContent.Animations.Actions;


namespace YourPluginNamespace
{
    [ApiVersion(2, 1)]
    public class BiomsKeyToItemTerrariaPlagin : TerrariaPlugin
    {
        public override string Name => "BiomsKeyToItemTerrariaPlagin";
        public override string Author => "Синий Ёж";
        public override string Description => "Добавляет команду которая выдаёт оружие из биомного сундука в обмен на ключ этого сундука";
        public override Version Version => new Version(1, 0, 0);

        public BiomsKeyToItemTerrariaPlagin(Main game) : base(game)
        {
        }

        public override void Initialize()
        {
            Commands.ChatCommands.Add(new Command(BiomKeyToItemBiomChest, "key")
            {
                AllowServer = false,
                HelpText = "Заменяет ключи биомов в инвентаре на оружие из сундука биомов, работает только после убийства плантеры"
            });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {

            }
            base.Dispose(disposing);
        }
        private void BiomKeyToItemBiomChest(CommandArgs args)
        {
            try
            {
                // Получаем игрока
                TSPlayer tsPlayer = args.Player;
                Player player = Main.player[tsPlayer.Index];
                if (!NPC.downedPlantBoss)
                {
                    tsPlayer.SendErrorMessage("Плантера ещё не была побеждена!");
                    return;
                }
                if (player.dead)
                {
                    tsPlayer.SendErrorMessage("Вы мертвы!");
                    return;
                }
                int CountNoneType = 0;
                bool flag = false;
                KeyItem KeyItem = new KeyItem();
                for (int i = 0; i < player.inventory.Length - 9; i++)// -9 значит, что мы не учитываем слоты боеприпасов и слот в курсоре
                {
                    foreach (int key in KeyItem.dict.Keys)
                    {
                        if (player.inventory[i].type == key)
                        {
                            flag = true;
                            KeyItem.dict[key].countKey += player.inventory[i].stack;
                            if (KeyItem.dict[key].indexKey == -1)
                                KeyItem.dict[key].indexKey = i;
                        }
                    }
                    if (player.inventory[i].type == ItemID.None)
                    {
                        CountNoneType++;
                    }
                }
                if (!flag)
                {
                    tsPlayer.SendErrorMessage($"Ключи биомов не найдены.");
                    return;
                }
                string Out = "";
                foreach (int key in KeyItem.dict.Keys)
                {
                    //tsPlayer.SendSuccessMessage($"{CountNoneType}");
                    if (CountNoneType > 0)
                    {
                        if (KeyItem.dict[key].indexKey != -1)
                        {
                            flag = true;
                            tsPlayer.GiveItem(KeyItem.dict[key].itemID, 1);
                            Out += $"[i:{KeyItem.dict[key].itemID}]";
                            if (player.inventory[KeyItem.dict[key].indexKey].stack > 1)
                            {
                                player.inventory[KeyItem.dict[key].indexKey].stack--;
                            }
                            else
                            {
                                player.inventory[KeyItem.dict[key].indexKey] = new Item();
                            }
                            NetMessage.SendData((int)PacketTypes.PlayerSlot, -1, -1, null, tsPlayer.Index, KeyItem.dict[key].indexKey, tsPlayer.TPlayer.inventory[KeyItem.dict[key].indexKey].prefix);
                            CountNoneType--;
                        }
                    }
                    else
                    {
                        tsPlayer.SendErrorMessage("Недостаточно свободных слотов в инвентаре для получения всех предметов.");
                        break;
                    }
                }
                if (Out != "")
                {
                    tsPlayer.SendSuccessMessage($"Полученные предметы:{Out}");
                }
            }
            catch (Exception ex)
            {
                args.Player.SendErrorMessage($"Произошла ошибка: {ex.Message}");
            }
        }

    }
}

