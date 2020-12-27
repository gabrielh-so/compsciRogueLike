using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System.Runtime.Serialization.Formatters.Binary;

using static MajorProject.InputManager;

namespace MajorProject
{

    public class GameScreen : Screen
    {

        Random rand;

        string saveFileName = "SaveFile.bin";

        Vector2 ScreenSize;

        

        public enum RoomType
        {
            Loot,
            Combat,
            Boss,
            Shop
        }

        string[] LevelNames =
        {
            "TestArea",
            "Dungeon",
            "Volcano",
            "Jungle",
            "Temple"
        };

        // incremented each time the player passes through a door
        int LevelIndex = 0;

        Matrix cameraTransformationMatrix;

        World GameWorld;

        GamePlayer Player;

        public HUD headsUpDisplay;

        List<List<GameEnemy>> Enemies;
        List<GameProjectile> EnemyProjectiles;
        List<GameProjectile> PlayerProjectiles;

        List<GameItem> WorldItems;

        public ResourcePack PlayerResources;
        public ResourcePack SlimeResources;
        public ResourcePack FlyerResources;
        public ResourcePack BossResources;
        public ResourcePack GoblinResources;

        public ResourcePack ProjectileResources;

        public ResourcePack HUDResources;

        public EnvironmentResourcePack EnvironmentResources;

        public ResourcePack CoinResources;

        public GameScreen()
        {
            rand = new Random();

            ScreenSize = ScreenManager.Instance.Dimensions;

            GameWorld =  new World();

            GameWorld.room_count = 8;

            GameWorld.level_cell_width = 25;
            GameWorld.level_cell_height = 25;


            GameWorld.room_min_cell = new Vector2(4, 4);
            GameWorld.room_max_cell = new Vector2(6, 6);

            Player = new GamePlayer();

            Enemies = new List<List<GameEnemy>>();

            PlayerProjectiles = new List<GameProjectile>();
            EnemyProjectiles = new List<GameProjectile>();

            WorldItems = new List<GameItem>();

            cameraTransformationMatrix = Matrix.Identity;
        }

        public void ConstructWorld()
        {
            EnvironmentResources.LoadContent(LevelNames[LevelIndex]);

            GameWorld.LoadContent(ref EnvironmentResources);
            GameWorld.GenerateWorld();
            GameWorld.RenderTexture();

            //AudioManager.Instance.PlaySoundInstance(EnvironmentResources.AudioPack["Music"].CreateInstance(), "Music", true);
            AudioManager.Instance.PlayMusic(EnvironmentResources.AudioPack["Music"].Name);
        }

        public void RegenerateWorld()
        {
            GameWorld.UnloadContent();
            EnvironmentResources.UnloadContent();

            ConstructWorld();
        }

        public void SetPlayerToEntrance()
        {
            Player.position.X = (GameWorld.entryIndex.X + 0.5f) * World.tilePixelWidth;
            Player.position.Y = (GameWorld.entryIndex.Y + 0.5f) * World.tilePixelHeight;
        }


        public void AddProjectile(GameProjectile p)
        {
            p.Map = GameWorld.Map;
            p.LoadContent(ref ProjectileResources);
            if (p.target == typeof(GamePlayer))
            {
                EnemyProjectiles.Add(p);
            }
            else
                PlayerProjectiles.Add(p);
        }

        public void AddItem(GameItem i)
        {
            i.removeable = false;
            WorldItems.Add(i);
        }



        public void LoadPlayer()
        {
            Player.tileWidth = World.tilePixelWidth;
            Player.LoadContent(ref PlayerResources);
            Player.Map = GameWorld.Map;
            SetPlayerToEntrance();
        }

        void LoadRoomContents()
        {

            for (int i = 0; i < GameWorld.room_count; i++)
            {

                Vector2 RoomPosition = GameWorld.generation_RoomIndexPositions[i];
                Enemies.Add(new List<GameEnemy>());

                switch (GameWorld.roomTypes[i])
                {
                    case RoomType.Combat:
                        for (int j = 0; j < GameWorld.generation_RoomIndexDimensions[i].X; j++)
                        {




                            GameEnemy e = new GameEnemy();

                            int enemyIndex = rand.Next(0, 3);

                            switch (enemyIndex)
                            {
                                case 0:
                                    e = new GameGoblin();
                                    e.LoadContent(ref GoblinResources);
                                    break;
                                case 1:
                                    e = new GameSlime();
                                    e.LoadContent(ref SlimeResources);
                                    break;
                                case 2:
                                    e = new GameFlyer();
                                    e.LoadContent(ref FlyerResources);
                                    break;
                            }

                            e.Map = GameWorld.Map;

                            e.tileWidth = World.tilePixelWidth;

                            e.SetPosition(RoomPosition.X * World.tilePixelWidth + World.tilePixelWidth * j, RoomPosition.Y * World.tilePixelWidth + rand.Next(0, (int)(World.tilePixelHeight * GameWorld.generation_RoomIndexDimensions[i].Y)));

                            e.currentRoom = i;

                            e.alive = true;
                            e.BoundingBox = new Rectangle();
                            e.BoundingBox.Size = new Point(25, 25);

                            Enemies[i].Add(e);

                        }
                        break;

                }


                
            }
        }

        void LoadItemResources()
        {
            CoinResources.LoadContent();
            ProjectileResources.LoadContent();
        }

        void LoadCharacterResources()
        {
            PlayerResources.LoadContent();
            GoblinResources.LoadContent();
            FlyerResources.LoadContent();
            SlimeResources.LoadContent();
            //BossResources.LoadContent();
        }

        public void AssignRooms()
        {

            // calculate room proportion based on level

            //// level 1 - 1 shop - 4 combat - 2 loot - 1 boss
            ///
            ////just assigning this for all levels when programming - tuning by level can wait until it works
            ///

            GameWorld.roomTypes.Add(RoomType.Boss);
            GameWorld.roomTypes.Add(RoomType.Loot);
            GameWorld.roomTypes.Add(RoomType.Loot);
            GameWorld.roomTypes.Add(RoomType.Combat);
            GameWorld.roomTypes.Add(RoomType.Combat);
            GameWorld.roomTypes.Add(RoomType.Combat);
            GameWorld.roomTypes.Add(RoomType.Combat);
            GameWorld.roomTypes.Add(RoomType.Shop);


        }

        public void PlaceLoot()
        {
            // give player a slingshot to start with - low damage

            GameWeaponSword s = new GameWeaponSword();



            // place items at the ends of corridors sometimes

            foreach (Vector2 position in GameWorld.CorridorEndings)
            {
                // place item here
                Vector2 coinPosition = new Vector2();
                coinPosition.X = (position.X) * World.tilePixelWidth * 2 + World.tilePixelWidth;
                coinPosition.Y = (position.Y) * World.tilePixelWidth * 2 + World.tilePixelWidth;

                GameCoin c = new GameCoin();
                c.LoadContent(ref CoinResources);
                c.value = 10;
                c.SetPosition(coinPosition.X + World.tilePixelWidth / 2, coinPosition.Y + World.tilePixelWidth / 2);

                WorldItems.Add(c);
            }



            Player.AddItem(s);

        }

        public void LoadHUD()
        {
            headsUpDisplay = new HUD();
            HUDResources.LoadContent();
            headsUpDisplay.LoadContent(HUDResources);
            headsUpDisplay.SetPlayer(Player);
        }

        public override void LoadContent()
        {
            if (PlayerPreferences.Instance.LoadSavedGame)
            {
                if (LoadGame()) return;
            }

            ConstructWorld();

            LoadCharacterResources();

            LoadPlayer();

            LoadHUD();

            LoadItemResources();

            AssignRooms();

            LoadRoomContents();

            PlaceLoot();

            headsUpDisplay.GenerateMiniMap(GameWorld.Map);
        }

        public override void UnloadContent()
        {
            headsUpDisplay.UnSetPlayer();
            headsUpDisplay.UnloadContent();

            Player.UnloadContent();

            GameWorld.UnloadContent();

            PlayerResources.UnloadContent();
            GoblinResources.UnloadContent();
            SlimeResources.UnloadContent();
            FlyerResources.UnloadContent();
            //BossResources.UnloadContent();
            EnvironmentResources.UnloadContent();
        }

        void CreateCollision(GameEntity a, GameEntity b)
        {

        }

        public override void Update(GameTime gameTime)
        {


            if (InputManager.Instance.KeyPressed(Keys.Escape))
            {
                ScreenManager.Instance.ChangeScreens("GameMenuScreen", true);
            }


            if (InputManager.Instance.KeyPressed(Keys.R))
            {
                LevelIndex++;
                LevelIndex %= LevelNames.Length;
                AudioManager.Instance.StopSoundInstance("Music", true);
                RegenerateWorld();
                SetPlayerToEntrance();
            }

            // update each entity

            if (Player.alive)
                Player.Update(gameTime);

            for (int i = 0; i < GameWorld.room_count; i++)
            {
                for (int j = 0; j < Enemies[i].Count; j++)
                {
                    Enemies[i][j].Update(gameTime);
                }
            }

            for (int i = 0; i < WorldItems.Count; i++)
            {
                WorldItems[i].Update(gameTime);
            }


            // compare the player only to enemies it currently shares a room with

            // find out which room is the player in, if any
            int playerRoomIndex = -1;
            int tileWidth = World.tilePixelWidth;
            for (int i = 0; i < GameWorld.room_count; i++)
            {
                // check x and y coordinates
                if (Player.position.X < GameWorld.generation_RoomIndexPositions[i].X * tileWidth || Player.position.X > (GameWorld.generation_RoomIndexPositions[i].X + GameWorld.generation_RoomIndexDimensions[i].X) * tileWidth) continue;
                if (Player.position.Y < GameWorld.generation_RoomIndexPositions[i].Y * tileWidth || Player.position.Y > (GameWorld.generation_RoomIndexPositions[i].Y + GameWorld.generation_RoomIndexDimensions[i].Y) * tileWidth) continue;

                playerRoomIndex = i;

            }
            
            // checks that player has not moved to/from rooms
            if (Player.currentRoom != playerRoomIndex)
            {
                if (Player.currentRoom > -1)
                {
                    // player has left a room - remove the player target from all enemies in the room
                    foreach (GameEnemy e in Enemies[Player.currentRoom])
                    {
                        e.RemoveTarget();
                    }

                }
                if (playerRoomIndex > -1)
                {
                    // player has entered a(nother) room - set the player as the target for all enemies in the room

                    foreach (GameEnemy e in Enemies[playerRoomIndex])
                    {
                        e.SetTarget(Player);
                    }
                }


                Player.currentRoom = playerRoomIndex;
            }



            if (Player.currentRoom > -1)
            {
                // player is in a room, compare collision with all enemies

                for (int i = 0; i < Enemies[playerRoomIndex].Count; i++)
                {
                    if (Enemies[playerRoomIndex][i].BoundingBox.Contains(Player.BoundingBox))
                    {
                        // uh oh!
                        // player has walked within the graps of an enemy
                        // may need to do some damage

                        Player.onCollision(Enemies[playerRoomIndex][i]);
                        Enemies[playerRoomIndex][i].onCollision(Player);


                    }
                }



            }







            // regardless, compare with all non-enemy entities

            // go backwards through projectile array
            for (int i = EnemyProjectiles.Count - 1; i > -1; i--)
            {
                // temporary variables to set edges for testing
                float testX = EnemyProjectiles[i].position.X;
                float testY = EnemyProjectiles[i].position.Y;

                // which edge is closest?
                if (EnemyProjectiles[i].position.X < Player.BoundingBox.X) testX = Player.BoundingBox.X;      // test left edge
                else if (EnemyProjectiles[i].position.X > Player.BoundingBox.X + Player.BoundingBox.Width) testX = Player.BoundingBox.X + Player.BoundingBox.Width;   // right edge
                if (EnemyProjectiles[i].position.Y < Player.BoundingBox.Y) testY = Player.BoundingBox.Y;      // top edge
                else if (EnemyProjectiles[i].position.Y > Player.BoundingBox.Y + Player.BoundingBox.Height) testY = Player.BoundingBox.Y + Player.BoundingBox.Height;   // bottom edge

                // get distance from closest edges
                double distX = EnemyProjectiles[i].position.X - testX;
                double distY = EnemyProjectiles[i].position.Y - testY;
                double distance = Math.Sqrt((distX * distX) + (distY * distY));

                // collision if the distance is less than the radius
                if (distance <= EnemyProjectiles[i].radius)
                {
                    // give projectile to player for damage
                    if (!Player.hitCooldown)
                        Player.ProjectileCollision(EnemyProjectiles[i]);

                    //destroy the projectile
                    EnemyProjectiles[i].removeable = true;
                }
            }

            for (int i = EnemyProjectiles.Count - 1; i > -1; i--)
            {
                if (EnemyProjectiles[i].removeable)
                {
                    EnemyProjectiles.RemoveAt(i);
                }
            }







            foreach (List<GameEnemy> el in Enemies)
            {
                foreach (GameEnemy e in el)
                {
                    // go backwards through projectile array
                    for (int i = PlayerProjectiles.Count - 1; i > -1; i--)
                    {
                        // temporary variables to set edges for testing
                        float testX = PlayerProjectiles[i].position.X;
                        float testY = PlayerProjectiles[i].position.Y;

                        // which edge is closest?
                        if (PlayerProjectiles[i].position.X < e.BoundingBox.X) testX = e.BoundingBox.X;      // test left edge
                        else if (PlayerProjectiles[i].position.X > e.BoundingBox.X + e.BoundingBox.Width) testX = Player.BoundingBox.X + Player.BoundingBox.Width;   // right edge
                        if (PlayerProjectiles[i].position.Y < e.BoundingBox.Y) testY = e.BoundingBox.Y;      // top edge
                        else if (PlayerProjectiles[i].position.Y > e.BoundingBox.Y + e.BoundingBox.Height) testY = Player.BoundingBox.Y + Player.BoundingBox.Height;   // bottom edge

                        // get distance from closest edges
                        double distX = PlayerProjectiles[i].position.X - testX;
                        double distY = PlayerProjectiles[i].position.Y - testY;
                        double distance = Math.Sqrt((distX * distX) + (distY * distY));

                        // collision if the distance is less than the radius
                        if (distance <= PlayerProjectiles[i].radius)
                        {
                            // give projectile to player for damage
                            e.ProjectileCollision(PlayerProjectiles[i]);

                            //destroy the projectile
                            PlayerProjectiles[i].removeable = true;
                        }
                    }

                    for (int i = PlayerProjectiles.Count - 1; i > -1; i--)
                    {
                        if (PlayerProjectiles[i].removeable)
                        {
                            PlayerProjectiles.RemoveAt(i);
                        }
                    }
                }
            }

            for (int i = PlayerProjectiles.Count - 1; i > -1; i--)
            {
                if (PlayerProjectiles[i].removeable)
                {
                    PlayerProjectiles.RemoveAt(i);
                }
            }





















            // go backwards through WorldItems array
            for (int i = WorldItems.Count - 1; i > -1; i--)
            {
                // temporary variables to set edges for testing
                float testX = WorldItems[i].position.X;
                float testY = WorldItems[i].position.Y;

                // which edge is closest?
                if (WorldItems[i].position.X < Player.BoundingBox.X) testX = Player.BoundingBox.X;      // test left edge
                else if (WorldItems[i].position.X > Player.BoundingBox.X + Player.BoundingBox.Width) testX = Player.BoundingBox.X + Player.BoundingBox.Width;   // right edge
                if (WorldItems[i].position.Y < Player.BoundingBox.Y) testY = Player.BoundingBox.Y;      // top edge
                else if (WorldItems[i].position.Y > Player.BoundingBox.Y + Player.BoundingBox.Height) testY = Player.BoundingBox.Y + Player.BoundingBox.Height;   // bottom edge

                // get distance from closest edges
                double distX = WorldItems[i].position.X - testX;
                double distY = WorldItems[i].position.Y - testY;
                double distance = Math.Sqrt((distX * distX) + (distY * distY));

                // collision if the distance is less than the radius
                if (distance <= WorldItems[i].radius)
                {

                    // is item pickupable, and if so has the player signaled to pick up?
                    if (WorldItems[i].type != typeof(GameCoin))
                    {
                        Player.AddItem(WorldItems[i]);
                        WorldItems[i].removeable = true;
                    }
                    else
                    {
                        // item is a coin, add money to player balance then unload
                        WorldItems[i].Use(Player);
                        WorldItems[i].UnloadContent();
                        WorldItems[i].removeable = true;
                    }


                }
            }

            for (int i = WorldItems.Count - 1; i > -1; i--)
            {
                if (WorldItems[i].removeable)
                {
                    WorldItems.RemoveAt(i);
                }
            }




            headsUpDisplay.Update(gameTime);




            // detect collisions between entities and trigger oncollide functions









            // update player viewmatrix using player location
            cameraTransformationMatrix.Translation = new Vector3((-Player.position.X) + ScreenSize.X / 2, (-Player.position.Y) + ScreenSize.Y / 2, 0);
        }


        void recordValues(string value1) // throwaway methods for testing without second screen
        {
            using (StreamWriter sw = new StreamWriter("output.txt"))
            {
                sw.WriteLine("value 1: " + value1);
            }
        }
        void recordValues(string value1, string value2) // for the love of god please don't use these they write a new line every frame
        {
            using (StreamWriter sw = new StreamWriter("output.txt"))
            {
                sw.WriteLine("value 1: " + value1 + " | value 2 :" + value2);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {




            spriteBatch.End();

            // the camera produces a view matrix that can be applied to any sprite batch
            spriteBatch.Begin(transformMatrix: cameraTransformationMatrix);

            // ... draw sprites here ...

            GameWorld.Draw(spriteBatch);


            Player.Draw(spriteBatch);

            foreach (List<GameEnemy> el in Enemies)
            {
                foreach (GameEnemy e in el)
                {
                    e.Draw(spriteBatch);
                }
            }

            foreach (GameProjectile p in EnemyProjectiles)
            {
                p.Draw(spriteBatch);
            }


            foreach (GameItem i in WorldItems)
            {
                i.Draw(spriteBatch);
            }




            spriteBatch.End();

            spriteBatch.Begin();


            // probably draw ui elements here

            headsUpDisplay.Draw(spriteBatch);

        }

        public void SaveGame()
        {
            using (StreamWriter sw = new StreamWriter(saveFileName))
            {
                BinaryFormatter bs = new BinaryFormatter();
                bs.Serialize(sw.BaseStream, this);
            }
        }









        /// <summary>
        /// need to add item
        /// </summary>
        /// <returns></returns>


        /// need to add projectile
        /// 













        public bool LoadGame()
        {
            if (File.Exists(saveFileName))
            {

                using (StreamReader sr = new StreamReader(saveFileName))
                {
                    BinaryFormatter bs = new BinaryFormatter();
                    GameScreen gs = (GameScreen)bs.Deserialize(sr.BaseStream);

                    GameWorld = gs.GameWorld;
                    LevelIndex = gs.LevelIndex;
                    Player = gs.Player;
                    cameraTransformationMatrix = gs.cameraTransformationMatrix;

                    // and then any entity information

                    return true;

                }

                ///don't forget to reload content
                ///
            }

            return false;
        }
    }
}
