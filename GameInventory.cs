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
        int itemSpace = 3;

        static Random rand = new Random();

        public GameItem[] itemList;

        public GameAbility[] abilityList;

        public int SelectedItemSlot = 1;

        Dictionary<Keys, int> numberKeyMap;


        public bool ContainsWeapon;

        public ResourcePack Resources;


        public void Update(GameTime gameTime)
        {

            foreach (Keys k in numberKeyMap.Keys)
            {
                if (0 < numberKeyMap[k] && numberKeyMap[k] <= itemSpace)
                    if (InputManager.Instance.KeyPressed(k)) SelectedItemSlot = numberKeyMap[k] - 1;
            }

        }

        // need to check that player can fight enemies in a room before entering
        public void UpdateWeaponStatus()
        {
            foreach (GameItem i in itemList) {
                if (i != null)
                    if (i.type.BaseType == typeof(GameWeapon))
                    {
                        ContainsWeapon = true;
                        return;
                    }
            }
            ContainsWeapon = false;
            return;
        }

        public GameInventory()
        {
            // maps the number keys to actual indexes
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

            //generate item list
            itemList = new GameItem[itemSpace];
        }

        public bool RemoveItem(GamePlayer user, int index)
        {
            // can't remove an item that doesn't exist
            if (itemList[index] == null) return false;


            // update the item's position
            itemList[index].SetPosition(user.position.X, user.position.Y);

            // add a slide to the item
            Random rand = new Random();
            double movementAngle = rand.NextDouble() * Math.PI * 2;
            itemList[index].velocity = new Vector2((float)Math.Sin(movementAngle), (float)Math.Cos(movementAngle)) * 1.5f;

            // hand the item to the gameworld
            ((GameScreen)ScreenManager.Instance.currentScreen).AddItem(itemList[index]);

            itemList[index] = null;

            // update flag for weapons present in the inventory
            UpdateWeaponStatus();

            return true;
        }

        public bool AddItem(GamePlayer user, GameItem i)
        {
            // give pickup sound instance to sound manager
            AudioManager.Instance.PlaySoundInstance(Resources.AudioPack["Item_Pickup"].CreateInstance(), "ItemPickUp" + rand.NextDouble().ToString());

            // if an item's already in the selected, boot it
            if (itemList[SelectedItemSlot] != null)
                RemoveItem(user, SelectedItemSlot);

            // actually assign the item to the slot
            itemList[SelectedItemSlot] = i;


            // update flag for weapons present in the inventory
            UpdateWeaponStatus();

            return true;
        }

        /*
        public bool AddAbility(GameAbility a)
        {
            abilityList[SelectedItemSlot] = a;


            return true;
        }
        */

        public void UseItem(GamePlayer user)
        {
            // checks that there is a usable item in the selected slot
            if (itemList[SelectedItemSlot] != null)
            {
                // use the item
                itemList[SelectedItemSlot].Use(user);
            }
        }

        public void LoadContent(ResourcePack resources)
        {
            // hook up resources object refernce
            Resources = resources;

            // init variables
            bool inMenu = false;

            // for when a save gae is loaded, the inventory will be initialised in the game menu
            if (ScreenManager.Instance.currentScreen.GetType() == typeof(GameMenuScreen))
                inMenu = true;

            if (inMenu)
            {
                // if the user is currently in a menu, load from the correct screen, not the current screen
                foreach (GameItem i in itemList)
                {
                    if (i != null)
                        i.LoadContent(ref ((GameScreen)ScreenManager.Instance.oldScreen).LootResources);
                }
                return;
            }
            foreach (GameItem i in itemList)
            {
                if (i != null)
                    i.LoadContent(ref ((GameScreen)ScreenManager.Instance.currentScreen).LootResources);
            }
        }

        public void UnLoadContent()
        {
            // unload all the items and unhook the resources
            Resources = null;
            foreach (GameItem i in itemList)
            {
                if (i != null)
                    i.UnloadContent();
            }
        }

    }
}
