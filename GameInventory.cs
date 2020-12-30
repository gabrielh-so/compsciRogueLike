using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MajorProject
{
    public class GameInventory
    {
        int abilitySpace = 2;
        int itemSpace = 3;

        public GameItem[] itemList;

        public GameAbility[] abilityList;

        public int SelectedItemSlot = 1;

        Dictionary<Keys, int> numberKeyMap;



        public void Update(GameTime gameTime)
        {

            foreach (Keys k in numberKeyMap.Keys)
            {
                if (numberKeyMap[k] <= itemSpace)
                    if (InputManager.Instance.KeyPressed(k)) SelectedItemSlot = numberKeyMap[k];
            }

        }

        public GameInventory()
        {
            numberKeyMap = new Dictionary<Keys, int>() {
                { Keys.D1, 1 },
                { Keys.D2, 2 },
                { Keys.D3, 3 },
                { Keys.D4, 4 },
                { Keys.D5, 5 },
                { Keys.D6, 6 },
                { Keys.D7, 7 },
                { Keys.D8, 8 },
                { Keys.D9, 9 },
                { Keys.D0, 0 }
            };

            itemList = new GameItem[itemSpace];
            abilityList = new GameAbility[abilitySpace];
        }

        public bool RemoveItem(GamePlayer user, int index)
        {
            // can't remove an item that doesn't exist
            if (itemList[index] == null) return false;


            // update the item's position
            itemList[index].SetPosition(user.position.X, user.position.Y);
            ((GameScreen)ScreenManager.Instance.currentScreen).AddItem(itemList[index]);

            itemList[index] = null;

            return true;
        }

        public bool AddItem(GamePlayer user, GameItem i)
        {
            if (itemList[SelectedItemSlot] != null)
                RemoveItem(user, SelectedItemSlot);
            itemList[SelectedItemSlot] = i;


            return true;
        }

        public bool AddAbility(GameAbility a)
        {
            abilityList[SelectedItemSlot] = a;


            return true;
        }

        public void UseItem(GamePlayer user)
        {
            if (itemList[SelectedItemSlot] != null)
            {
                itemList[SelectedItemSlot].Use(user);
            }
        }

        public void UnLoadContent()
        {

        }

    }
}
