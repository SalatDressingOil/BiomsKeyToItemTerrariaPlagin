using System;
using System.Collections.Generic;
using System.Linq;
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
        public override string Description => "Добавляет команду которая выдаёт оружие из биомного сундука в обмен на ключ этого сундука ";
        public override Version Version => new Version(1, 0, 0);

        public BiomsKeyToItemTerrariaPlagin(Main game) : base(game)
        {
        }

        public override void Initialize()
        {
            Commands.ChatCommands.Add(new Command(BiomKeyToItemBiomChest, "key")
            {
                AllowServer = false,
            });
            Commands.ChatCommands.Add(new Command(keyd, "keyd")
            {
                AllowServer = false,
            });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {

            }
            base.Dispose(disposing);
        }
        private void keyd(CommandArgs args)
        {

            TSPlayer tsPlayer = args.Player;
            Player player = Main.player[tsPlayer.Index];
            player.inventory[0].stack--;
            args.Player.SendSuccessMessage(Main.ServerSideCharacter.ToString());
            NetMessage.SendData((int)PacketTypes.PlayerSlot, -1, -1, null, tsPlayer.Index, 0, tsPlayer.TPlayer.inventory[0].prefix);
        }

        public Dictionary<int, int> keys = new Dictionary<int, int>()// Словарь для хранения id ключей и соответствующих предметов
        {
            {ItemID.JungleKey, ItemID.PiranhaGun},
            {ItemID.CorruptionKey, ItemID.ScourgeoftheCorruptor},
            {ItemID.CrimsonKey, ItemID.VampireKnives},
            {ItemID.HallowedKey, ItemID.RainbowGun},
            {ItemID.FrozenKey, ItemID.StaffoftheFrostHydra},
            {4714, 4607} // id пустынного ключа и посоха пустынного тигра которых нет в ItemID
        };
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
                // Подсчет количества каждого ключа в инвентаре игрока и выдача предметов
                foreach (int key in keys.Keys)
                {
                    int countKey = 0;
                    int indexKey = -1;
                    int CountNoneType = 0;
                    for (int i = 0; i < player.inventory.Length-9; i++)// -9 значит, что мы не учитываем слоты боеприпасов и слот в курсоре
                    {
                        tsPlayer.SendSuccessMessage($"[i:{player.inventory[i].type}]");
                        if (player.inventory[i].type == key)
                        {
                            countKey += player.inventory[i].stack;
                            if (indexKey == -1)
                                indexKey = i;
                        }
                        else if (player.inventory[i].type == ItemID.None)
                        {
                            CountNoneType++; //пустые слоты
                        }
                    }
                    if (countKey > 0)
                    {
                        if (CountNoneType > 0)
                        {
                            tsPlayer.GiveItem(keys[key], 1);
                            tsPlayer.SendSuccessMessage($"Получен предмет.");
                            if (player.inventory[indexKey].stack > 1)
                            {
                                player.inventory[indexKey].stack--;
                            }
                            else
                            {
                                player.inventory[indexKey] = new Item();
                            }
                            NetMessage.SendData((int)PacketTypes.PlayerSlot, -1, -1, null, tsPlayer.Index, indexKey, tsPlayer.TPlayer.inventory[indexKey].prefix);
                            //NetMessage.SendData((int)PacketTypes.PlayerSlot, tsPlayer.Index, -1, null, tsPlayer.Index, 0, player.inventory[indexKey].prefix);
                            break;
                        }
                        else
                        {
                            tsPlayer.SendErrorMessage("У вас недостаточно свободных слотов в инвентаре для получения предмета.");
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Выводим сообщение об ошибке игроку
                args.Player.SendErrorMessage($"Произошла ошибка: {ex.Message}");
            }
        }

    }
}

