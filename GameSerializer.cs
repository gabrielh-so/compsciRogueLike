using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using Newtonsoft.Json;
using System.Runtime.Serialization.Formatters.Binary;

namespace MajorProject
{
    class GameSerializer
    {


        static string saveFileName = "SaveFile.bin";

        /*
        public static void SerializeGame(GameScreen gameScreen)
        {


            using (FileStream sw = new FileStream(saveFileName, FileMode.OpenOrCreate))
            {
                BinaryFormatter bs = new BinaryFormatter();

                List<string> savefiledata = new List<string>();


                savefiledata.Add(JsonConvert.SerializeObject(gameScreen.GameWorld));





                // issue - serialiser can't tell the difference between classes that inherit from a base class that it is deserializing from
                // solution - create a new list for each type of weapon, item

                GameBoss b = new GameBoss();

                // first - characters
                

                List<List<GameFlyer>> flyers = new List<List<GameFlyer>>();
                for (int i = 0; i < gameScreen.Enemies.Count; i++) flyers.Add(new List<GameFlyer>());

                List<List<GameGoblin>> goblins = new List<List<GameGoblin>>();
                for (int i = 0; i < gameScreen.Enemies.Count; i++) goblins.Add(new List<GameGoblin>());

                List<List<GameSlime>> slimes = new List<List<GameSlime>>();
                for (int i = 0; i < gameScreen.Enemies.Count; i++) slimes.Add(new List<GameSlime>());


                for (int i = 0; i < gameScreen.Enemies.Count; i++)
                {
                    for (int j = 0; j < gameScreen.Enemies[i].Count; j++)
                    {
                        if (gameScreen.Enemies[i][j].type == typeof(GameFlyer))
                        {
                            flyers[i].Add((GameFlyer)gameScreen.Enemies[i][j]);

                        } else if (gameScreen.Enemies[i][j].type == typeof(GameGoblin))
                        {
                            goblins[i].Add((GameGoblin)gameScreen.Enemies[i][j]);
                        }
                        else if (gameScreen.Enemies[i][j].type == typeof(GameSlime))
                        {
                            slimes[i].Add((GameSlime)gameScreen.Enemies[i][j]);
                        }
                        else
                        {
                            b = (GameBoss)gameScreen.Enemies[i][j];
                        }
                    }
                }

                ///////////////////////
                ///game items
                ///////////////////////

                List<GameCoin> coins = new List<GameCoin>();

                List<GameWeaponRifle> rifles = new List<GameWeaponRifle>();
                List<GameWeaponSlingShot> slingshot = new List<GameWeaponSlingShot>();
                List<GameWeaponSpear> spear = new List<GameWeaponSpear>();
                List<GameWeaponSword> sword = new List<GameWeaponSword>();

                List<GamePotionHealth> health = new List<GamePotionHealth>();
                List<GamePotionImmune> immune = new List<GamePotionImmune>();
                List<GamePotionSpeed> speed = new List<GamePotionSpeed>();
                List<GamePotionRecharge> recharge = new List<GamePotionRecharge>();



                for (int i = 0; i < gameScreen.WorldItems.Count; i++)
                {
                    if (gameScreen.WorldItems[i].type == typeof(GameCoin))
                    {
                        coins.Add((GameCoin)gameScreen.WorldItems[i]);

                    }
                    else if (gameScreen.WorldItems[i].type == typeof(GameWeaponRifle))
                    {
                        rifles.Add((GameWeaponRifle)gameScreen.WorldItems[i]);
                    }
                    else if (gameScreen.WorldItems[i].type == typeof(GameWeaponSlingShot))
                    {
                        slingshot.Add((GameWeaponSlingShot)gameScreen.WorldItems[i]);
                    }
                    else if (gameScreen.WorldItems[i].type == typeof(GameWeaponSpear))
                    {
                        spear.Add((GameWeaponSpear)gameScreen.WorldItems[i]);
                    }
                    else if (gameScreen.WorldItems[i].type == typeof(GameWeaponSword))
                    {
                        sword.Add((GameWeaponSword)gameScreen.WorldItems[i]);
                    }
                    else if (gameScreen.WorldItems[i].type == typeof(GamePotionHealth))
                    {
                        health.Add((GamePotionHealth)gameScreen.WorldItems[i]);
                    }
                    else if (gameScreen.WorldItems[i].type == typeof(GamePotionImmune))
                    {
                        immune.Add((GamePotionImmune)gameScreen.WorldItems[i]);
                    }
                    else if (gameScreen.WorldItems[i].type == typeof(GamePotionSpeed))
                    {
                        speed.Add((GamePotionSpeed)gameScreen.WorldItems[i]);
                    }
                    else if (gameScreen.WorldItems[i].type == typeof(GamePotionRecharge))
                    {
                        recharge.Add((GamePotionRecharge)gameScreen.WorldItems[i]);
                    }
                    else
                    {
                        bool z = true;
                    }
                }









                    ///////////////////////////
                    ///world interactables
                    //////////////////////////

                List<TreasureChest> treasure = new List<TreasureChest>();
                List<Shop> shop = new List<Shop>();

                List<Type> shopItemType = new List<Type>();
                List<int> shopItemIndex = new List<int>();

                ExitInteractable e = new ExitInteractable();

                for (int i = 0; i < gameScreen.WorldInteractables.Count; i++)
                {
                    if (gameScreen.WorldInteractables[i].type == typeof(TreasureChest))
                    {
                        treasure.Add((TreasureChest)gameScreen.WorldInteractables[i]);

                    }
                    else if (gameScreen.WorldInteractables[i].type == typeof(Shop))
                    {

                        if ((shop)gameScreen.WorldInteractables[i].type == typeof(GameCoin))
                        {
                            coins.Add((GameCoin)gameScreen.WorldItems[i]);

                        }
                        else if (gameScreen.WorldItems[i].type == typeof(GameWeaponRifle))
                        {
                            rifles.Add((GameWeaponRifle)gameScreen.WorldItems[i]);
                        }
                        else if (gameScreen.WorldItems[i].type == typeof(GameWeaponSlingShot))
                        {
                            slingshot.Add((GameWeaponSlingShot)gameScreen.WorldItems[i]);
                        }
                        else if (gameScreen.WorldItems[i].type == typeof(GameWeaponSpear))
                        {
                            spear.Add((GameWeaponSpear)gameScreen.WorldItems[i]);
                        }
                        else if (gameScreen.WorldItems[i].type == typeof(GameWeaponSword))
                        {
                            sword.Add((GameWeaponSword)gameScreen.WorldItems[i]);
                        }
                        else if (gameScreen.WorldItems[i].type == typeof(GamePotionHealth))
                        {
                            health.Add((GamePotionHealth)gameScreen.WorldItems[i]);
                        }
                        else if (gameScreen.WorldItems[i].type == typeof(GamePotionImmune))
                        {
                            immune.Add((GamePotionImmune)gameScreen.WorldItems[i]);
                        }
                        else if (gameScreen.WorldItems[i].type == typeof(GamePotionSpeed))
                        {
                            speed.Add((GamePotionSpeed)gameScreen.WorldItems[i]);
                        }
                        else if (gameScreen.WorldItems[i].type == typeof(GamePotionRecharge))
                        {
                            recharge.Add((GamePotionRecharge)gameScreen.WorldItems[i]);
                        }


                        shop.Add((Shop)gameScreen.WorldInteractables[i]);
                    }
                    else if (gameScreen.WorldInteractables[i].type == typeof(ExitInteractable))
                    {
                        e = (ExitInteractable)gameScreen.WorldInteractables[i];
                    }
                }





                savefiledata.Add(JsonConvert.SerializeObject(b));
                savefiledata.Add(JsonConvert.SerializeObject(goblins));
                savefiledata.Add(JsonConvert.SerializeObject(flyers));
                savefiledata.Add(JsonConvert.SerializeObject(slimes));
                savefiledata.Add(JsonConvert.SerializeObject(gameScreen.PlayerProjectiles));
                savefiledata.Add(JsonConvert.SerializeObject(gameScreen.EnemyProjectiles));


                savefiledata.Add(JsonConvert.SerializeObject(rifles));
                savefiledata.Add(JsonConvert.SerializeObject(slingshot));
                savefiledata.Add(JsonConvert.SerializeObject(spear));
                savefiledata.Add(JsonConvert.SerializeObject(sword));

                savefiledata.Add(JsonConvert.SerializeObject(health));
                savefiledata.Add(JsonConvert.SerializeObject(immune));
                savefiledata.Add(JsonConvert.SerializeObject(speed));
                savefiledata.Add(JsonConvert.SerializeObject(recharge));



                bs.Serialize(sw, savefiledata.ToArray());

            }



        }

        public static void DeSerializeGame(GameScreen gameScreen)
        {

            using (StreamReader sr = new StreamReader(saveFileName))
            {
                BinaryFormatter bs = new BinaryFormatter();
                string[] savefiledata = (string[])bs.Deserialize(sr.BaseStream);




            }
        }
        */

        public static void SerializeGame(GameScreen gameScreen)
        {
            List<string> savefiledata = new List<string>();


            List<List<Type>> enemyTypes = new List<List<Type>>();
            for (int i = 0; i < gameScreen.Enemies.Count; i++) enemyTypes.Add(new List<Type>());
            List<List<string>> serializedEnemies = new List<List<string>>();
            for (int i = 0; i < gameScreen.Enemies.Count; i++) serializedEnemies.Add(new List<string>());


            List<string> serializedPlayerProjectiles = new List<string>();
            List<string> serializedEnemyProjectiles = new List<string>();

            List<Type> itemTypes = new List<Type>();
            List<string> serializedItems = new List<string>();

            List<Type> interactableTypes = new List<Type>();
            List<string> serializedInteractables = new List<string>();

            List<Type> shopTypes = new List<Type>();
            List<string> shopItems = new List<string>();

            List<Type> PlayerInventoryTypes = new List<Type>();
            List<string> serialisedPlayerInventoryItems = new List<string>();
            string serializedPlayer;

            string serializedGameWorld;




            for (int i = 0; i < gameScreen.Enemies.Count; i++)
            {
                for (int j = 0; j < gameScreen.Enemies[i].Count; j++)
                {
                    enemyTypes[i].Add(gameScreen.Enemies[i][j].type);
                    serializedEnemies[i].Add(JsonConvert.SerializeObject( gameScreen.Enemies[i][j], gameScreen.Enemies[i][j].type, null));
                }
            }

            foreach (GameProjectile p in gameScreen.PlayerProjectiles)
                serializedPlayerProjectiles.Add(JsonConvert.SerializeObject(p));

            foreach (GameProjectile p in gameScreen.EnemyProjectiles)
                serializedEnemyProjectiles.Add(JsonConvert.SerializeObject(p));

            foreach (GameItem i in gameScreen.WorldItems)
            {
                itemTypes.Add(i.type);
                serializedItems.Add(JsonConvert.SerializeObject(i, i.type, null));
            }

            for (int i = 0; i < gameScreen.WorldInteractables.Count; i++)
            {

                interactableTypes.Add(gameScreen.WorldInteractables[i].type);

                if (gameScreen.WorldInteractables[i].type == typeof(Shop))
                {
                    Shop s = (Shop)gameScreen.WorldInteractables[i];
                    shopTypes.Add(s.item.type);
                    shopItems.Add(JsonConvert.SerializeObject(s.item, s.item.type, null));
                    s.item = null;
                }

                serializedInteractables.Add(JsonConvert.SerializeObject(gameScreen.WorldInteractables[i], gameScreen.WorldInteractables[i].type, null));
            }

            for (int i = 0; i < gameScreen.Player.inventory.itemList.Length; i++)
            {
                if (gameScreen.Player.inventory.itemList[i] != null)
                {
                    PlayerInventoryTypes.Add(gameScreen.Player.inventory.itemList[i].type);
                    serialisedPlayerInventoryItems.Add(JsonConvert.SerializeObject(gameScreen.Player.inventory.itemList[i], gameScreen.Player.inventory.itemList[i].type, null));
                    gameScreen.Player.inventory.itemList[i] = null;
                }
            }

            serializedPlayer = JsonConvert.SerializeObject(gameScreen.Player);

            serializedGameWorld = JsonConvert.SerializeObject(gameScreen.GameWorld);


            savefiledata.Add(JsonConvert.SerializeObject(enemyTypes));
            savefiledata.Add(JsonConvert.SerializeObject(serializedEnemies));
            savefiledata.Add(JsonConvert.SerializeObject(serializedPlayerProjectiles));
            savefiledata.Add(JsonConvert.SerializeObject(serializedEnemyProjectiles));
            savefiledata.Add(JsonConvert.SerializeObject(itemTypes));
            savefiledata.Add(JsonConvert.SerializeObject(serializedItems));
            savefiledata.Add(JsonConvert.SerializeObject(interactableTypes));
            savefiledata.Add(JsonConvert.SerializeObject(serializedInteractables));
            savefiledata.Add(JsonConvert.SerializeObject(shopTypes));
            savefiledata.Add(JsonConvert.SerializeObject(shopItems));
            savefiledata.Add(JsonConvert.SerializeObject(PlayerInventoryTypes));
            savefiledata.Add(JsonConvert.SerializeObject(serialisedPlayerInventoryItems));
            savefiledata.Add(JsonConvert.SerializeObject(serializedPlayer));
            savefiledata.Add(JsonConvert.SerializeObject(serializedGameWorld));

            using (FileStream fs = new FileStream(saveFileName, FileMode.OpenOrCreate))
            {
                BinaryFormatter bs = new BinaryFormatter();
                bs.Serialize(fs, savefiledata.ToArray());
                
            }

            gameScreen.ReloadSerialisedContent();


        }

        


        public static void DeSerializeGame(GameScreen gameScreen)
        {


            string[] savefiledata;


            using (FileStream fs = new FileStream(saveFileName, FileMode.Open))
            {
                BinaryFormatter bs = new BinaryFormatter();
                savefiledata = (string[])bs.Deserialize(fs);

            }

            List<List<Type>> enemyTypes = JsonConvert.DeserializeObject<List<List<Type>>>(savefiledata[0]);

            List<List<string>> serializedEnemies = JsonConvert.DeserializeObject<List<List<string>>>(savefiledata[1]);



            List<string> serializedPlayerProjectiles = JsonConvert.DeserializeObject<List<string>>(savefiledata[2]);
            List<string> serializedEnemyProjectiles = JsonConvert.DeserializeObject<List<string>>(savefiledata[3]);

            List<Type> itemTypes = JsonConvert.DeserializeObject<List<Type>>(savefiledata[4]);
            List<string> serializedItems = JsonConvert.DeserializeObject<List<string>>(savefiledata[5]);

            List<Type> interactableTypes = JsonConvert.DeserializeObject<List<Type>>(savefiledata[6]);
            List<string> serializedInteractables = JsonConvert.DeserializeObject<List<string>>(savefiledata[7]);

            List<Type> shopTypes = JsonConvert.DeserializeObject<List<Type>>(savefiledata[8]);
            List<string> shopItems = JsonConvert.DeserializeObject<List<string>>(savefiledata[9]);

            List<Type> PlayerInventoryTypes = JsonConvert.DeserializeObject<List<Type>>(savefiledata[10]);
            List<string> serialisedPlayerInventoryItems = JsonConvert.DeserializeObject<List<string>>(savefiledata[11]);
            string serializedPlayer = JsonConvert.DeserializeObject<string>(savefiledata[12]);

            string serializedGameWorld = JsonConvert.DeserializeObject<string>(savefiledata[13]);



            for (int i = 0; i < serializedEnemies.Count; i++)
            {
                gameScreen.Enemies.Add(new List<GameEnemy>());
                for (int j = 0; j < serializedEnemies[i].Count; j++)
                {
                    gameScreen.Enemies[i].Add((GameEnemy)JsonConvert.DeserializeObject(serializedEnemies[i][j], enemyTypes[i][j]));
                }
            }

            gameScreen.PlayerProjectiles = new List<GameProjectile>();
            for (int i = 0; i < serializedPlayerProjectiles.Count; i++)
            {
                gameScreen.PlayerProjectiles.Add((GameProjectile)JsonConvert.DeserializeObject(serializedPlayerProjectiles[i], typeof(GameProjectile)));
            }

            gameScreen.EnemyProjectiles = new List<GameProjectile>();
            for (int i = 0; i < serializedEnemyProjectiles.Count; i++)
            {
                gameScreen.EnemyProjectiles.Add((GameProjectile)JsonConvert.DeserializeObject(serializedEnemyProjectiles[i], typeof(GameProjectile)));
            }

            gameScreen.WorldItems = new List<GameItem>();
            for (int i = 0; i < serializedItems.Count; i++)
            {
                gameScreen.WorldItems.Add((GameItem)JsonConvert.DeserializeObject(serializedItems[i], itemTypes[i]));
            }

            int shopCount = 0;

            gameScreen.WorldInteractables = new List<GameInteractable>();
            for (int i = 0; i < interactableTypes.Count; i++)
            {
                if (interactableTypes[i] == typeof(Shop))
                {
                    Shop s = (Shop)JsonConvert.DeserializeObject(serializedInteractables[i], typeof(Shop));

                    s.item = (GameItem)JsonConvert.DeserializeObject(shopItems[shopCount], shopTypes[shopCount]);

                    gameScreen.WorldInteractables.Add(s);

                    shopCount++;
                }
                else
                    gameScreen.WorldInteractables.Add((GameInteractable)JsonConvert.DeserializeObject(serializedInteractables[i], interactableTypes[i]));
            }

            gameScreen.Player = (GamePlayer)JsonConvert.DeserializeObject(serializedPlayer, typeof(GamePlayer));

            for (int i = 0; i < serialisedPlayerInventoryItems.Count; i++)
            {
                gameScreen.Player.inventory.itemList[i] = (GameItem)JsonConvert.DeserializeObject(serialisedPlayerInventoryItems[i], PlayerInventoryTypes[i]);
            }

            gameScreen.GameWorld = (World)JsonConvert.DeserializeObject(serializedGameWorld, typeof(World));

            gameScreen.ReloadSerialisedContent();


        }



    }
}
