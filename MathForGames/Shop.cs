using System;
using System.Collections.Generic;
using System.Text;

namespace MathForGames
{
    class Shop
    {
        
        static int _gold;
        static item[] _inventory;

        public Shop()
        {
            _gold = 2000;
            _inventory = new item[3];
        }

        public Shop(item[] items)
        {
            _gold = 30;

            _inventory = items;
        }

        public static bool Sell(Player player, int Stock, int playerInventory)
        {
            item ItemBuy = _inventory[Stock];

            if (player.Buy(ItemBuy, playerInventory))
            {
                _gold += ItemBuy.cost;
                return true;
            }
            return false;
        }

        
    }
}
