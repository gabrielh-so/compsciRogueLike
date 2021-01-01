using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using static MajorProject.InputManager;

namespace MajorProject
{

    public class GameScreen : Screen
    {

        static string saveFileName = "SaveFile.bin";

        public bool signalLevelChange;

        Random rand;

        Vector2 ScreenSize;

        public void SignalLevelChange()
        {
            signalLevelChange = true;
        }

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

        [XmlIgnore]
        public World GameWorld;

        public GamePlayer Player;

        public HUD headsUpDisplay;

        [XmlIgnore]
        public List<List<GameEnemy>> Enemies;
        public List<GameProjectile> EnemyProjectiles;
        public List<GameProjectile> PlayerProjectiles;

        public List<GameItem> WorldItems;

        public List<GameInteractable> WorldInteractables;

        public ResourcePack PlayerResources;
        public ResourcePack SlimeResources;
        public ResourcePack FlyerResources;
        public ResourcePack BossResources;
        public ResourcePack GoblinResources;

        public ResourcePack ProjectileResources;

        public ResourcePack HUDResources;

        public ResourcePack EnvironmentResources;

        public ResourcePack LootResources;

        public GameScreen()
        {
            rand = new Random();

            ScreenSize = ScreenManager.Instance.Dimensions;

            GameWorld = new World();

            GameWorld.room_count = 8;

            GameWorld.level_cell_width = 25;
            GameWorld.level_cell_height = 25;


            GameWorld.room_min_cell = new Vector2(4, 4);
            GameWorld.room_max_cell = new Vector2(6, 6);

            Player = new GamePlayer();

            Enemies = new List<List<GameEnemy>>();

            PlayerProjectiles = new List<GameProjectile>();
            EnemyProjectiles = new List<GameProjectile>();

            WorldInteractables = new List<GameInteractable>();

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



            foreach (List<GameEnemy> el in Enemies)
            {
                foreach (GameEnemy e in el)
                    e.UnloadContent();
                el.Clear();
            }
            foreach (GameProjectile p in EnemyProjectiles)
                p.UnloadContent();

            foreach (GameProjectile p in PlayerProjectiles)
                p.UnloadContent();
            foreach (GameItem p in WorldItems)
                p.UnloadContent();

            foreach (GameInteractable p in WorldInteractables)
                p.UnloadContent();

            Enemies.Clear();
            EnemyProjectiles.Clear();
            PlayerProjectiles.Clear();
            WorldItems.Clear();
            WorldInteractables.Clear();

            ConstructWorld();

            headsUpDisplay.GenerateMiniMap(GameWorld.Map);

            PlaceLoot();

            LoadRoomContents();
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
            i.OnGround = true;
            i.LoadContent(ref LootResources);
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
                Vector2 RoomCentre = GameWorld.generation_RoomIndexPositions[i] + GameWorld.generation_RoomIndexDimensions[i] / 2;
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

                    case RoomType.Loot:

                        TreasureChest t = new TreasureChest();

                        t.position = RoomCentre * World.tilePixelWidth;
                        t.LoadContent(ref LootResources);
                        WorldInteractables.Add(t);

                        break;
                    case RoomType.Shop:
                        for (int j = 0; j < 3; j++)
                        {
                            Shop s = new Shop();

                            s.position = (RoomCentre * World.tilePixelWidth) - new Vector2(World.tilePixelWidth, World.tilePixelWidth) + new Vector2(World.tilePixelWidth, World.tilePixelWidth) * j;
                            s.NewItem(100 + 50 * j);
                            s.LoadContent(ref LootResources);
                            WorldInteractables.Add(s);
                        }
                        break;
                    case RoomType.Boss:
                        GameBoss b = new GameBoss();

                        b.position = RoomCentre * World.tilePixelWidth;
                        b.LoadContent(ref BossResources);

                        Enemies[i].Add(b);

                        ExitInteractable exit = new ExitInteractable();
                        exit.position = RoomCentre * World.tilePixelWidth;
                        exit.LoadContent(ref EnvironmentResources);

                        WorldInteractables.Add(exit);
                        break;
                }



            }
        }

        void LoadItemResources()
        {
            LootResources.LoadContent();
            ProjectileResources.LoadContent();
        }

        void LoadCharacterResources()
        {
            PlayerResources.LoadContent();
            GoblinResources.LoadContent();
            FlyerResources.LoadContent();
            SlimeResources.LoadContent();
            BossResources.LoadContent();
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
            GameWorld.roomTypes.Add(RoomType.Combat);
            GameWorld.roomTypes.Add(RoomType.Combat);
            GameWorld.roomTypes.Add(RoomType.Shop);
            GameWorld.roomTypes.Add(RoomType.Combat);
            GameWorld.roomTypes.Add(RoomType.Loot);
            GameWorld.roomTypes.Add(RoomType.Combat);


        }

        public void PlaceLoot()
        {
            // give player a slingshot to start with - low damage

            //GameWeaponSword s = new GameWeaponSword();



            // place items at the ends of corridors sometimes

            foreach (Vector2 position in GameWorld.CorridorEndings)
            {
                // place item here

                if (rand.NextDouble() <= 1)
                {

                    Vector2 itemPosition = new Vector2();
                    itemPosition.X = position.X * World.tilePixelWidth * 2 + World.tilePixelWidth;
                    itemPosition.Y = position.Y * World.tilePixelWidth * 2 + World.tilePixelWidth;

                    if (rand.NextDouble() < 0.2)
                    {
                        GameCoin c = new GameCoin();
                        c.LoadContent(ref LootResources);
                        c.value = 10;
                        c.SetPosition(itemPosition.X + World.tilePixelWidth / 2, itemPosition.Y + World.tilePixelWidth / 2);

                        c.OnGround = true;
                        WorldItems.Add(c);
                    }
                    else
                    {
                        if (rand.NextDouble() < 0.5)
                        {
                            GameWeapon w;

                            double weaponRoll = rand.NextDouble();
                            if (weaponRoll < 0.25)
                            {
                                w = new GameWeaponSword();
                            }
                            else if (weaponRoll < 0.5)
                            {
                                w = new GameWeaponSpear();
                            }
                            else if (weaponRoll < 0.75)
                            {
                                w = new GameWeaponSlingShot();
                            }
                            else
                            {
                                w = new GameWeaponRifle();
                            }
                            w.OnGround = true;
                            w.SetPosition(itemPosition.X + World.tilePixelWidth / 2, itemPosition.Y + World.tilePixelWidth / 2);
                            w.LoadContent(ref LootResources);
                            WorldItems.Add(w);

                        }
                        else
                        {
                            GamePotion p;

                            double potionRoll = rand.NextDouble();
                            if (potionRoll < 0.25)
                            {
                                p = new GamePotionHealth();
                            }
                            else if (potionRoll < 0.5)
                            {
                                p = new GamePotionImmune();
                            }
                            else //if (weaponRoll < 0.75)
                            {
                                p = new GamePotionSpeed();
                            }
                            /*
                            else // only want to buy potion recharges
                            {
                                p = new GamePotionRecharge();
                            }
                            */
                            p.OnGround = true;
                            p.SetPosition(itemPosition.X + World.tilePixelWidth / 2, itemPosition.Y + World.tilePixelWidth / 2);
                            p.LoadContent(ref LootResources);
                            WorldItems.Add(p);
                        }

                    }
                }
            }



            //Player.AddItem(s);

        }

        public void LoadHUD()
        {
            headsUpDisplay = new HUD();
            HUDResources.LoadContent();
            headsUpDisplay.LoadContent(HUDResources);
            headsUpDisplay.GenerateMiniMap(GameWorld.Map);
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

            SetCharacterAndProjectileMaps();
        }

        void SetCharacterAndProjectileMaps()
        {

            Player.Map = GameWorld.Map;

            foreach (List<GameEnemy> el in Enemies)
                foreach (GameEnemy e in el)
                    e.Map = GameWorld.Map;
            foreach (GameProjectile p in EnemyProjectiles)
                p.Map = GameWorld.Map;

            foreach (GameProjectile p in PlayerProjectiles)
                p.Map = GameWorld.Map;

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
            BossResources.UnloadContent();
            EnvironmentResources.UnloadContent();
        }

        void ChangeToNextLevel()
        {
            LevelIndex++;
            GameWorld.LevelIndex++;
            AudioManager.Instance.StopSoundInstance("Music", true);
            RegenerateWorld();
            SetCharacterAndProjectileMaps();
            SetPlayerToEntrance();
        }

        public override void Update(GameTime gameTime)
        {
            if (InputManager.Instance.KeyPressed(Keys.R))
            {
                Enemies[0][0].alive = false;
            }

            if (InputManager.Instance.KeyPressed(Keys.Escape))
            {
                ScreenManager.Instance.ChangeScreens("GameMenuScreen", true);
            }



            // update each entity


            foreach (GameInteractable i in WorldInteractables)
            {
                i.Update(gameTime);
            }

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

            foreach (GameProjectile p in EnemyProjectiles)
            {
                p.Update(gameTime);
            }
            foreach (GameProjectile p in PlayerProjectiles)
            {
                p.Update(gameTime);
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
                    if (Enemies[playerRoomIndex][i].alive)
                        if (Enemies[playerRoomIndex][i].BoundingBox.Contains(Player.BoundingBox))
                        {
                            // uh oh!
                            // player has walked within the graps of an enemy
                            // may need to do some damage

                            Player.onCollision(Enemies[playerRoomIndex][i]);
                            Enemies[playerRoomIndex][i].onCollision(Player);


                        }
                }

                for (int i = 0; i < WorldInteractables.Count; i++)
                {
                    if (WorldInteractables[i].BoundingBox.Contains(Player.BoundingBox))
                    {
                        if (InputManager.Instance.KeyPressed(Keys.E))
                        {
                            WorldInteractables[i].Use(LevelIndex, Player);
                        }
                        WorldInteractables[i].Hovering();
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


            if (Player.currentRoom > -1)
                foreach (List<GameEnemy> el in Enemies)
                {
                    foreach (GameEnemy e in el)
                    {
                        if (e.alive)
                        {
                            // go backwards through projectile array
                            for (int i = PlayerProjectiles.Count - 1; i > -1; i--)
                            {
                                // temporary variables to set edges for testing
                                float testX = PlayerProjectiles[i].position.X;
                                float testY = PlayerProjectiles[i].position.Y;

                                // which edge is closest?
                                if (PlayerProjectiles[i].position.X < e.BoundingBox.X) testX = e.BoundingBox.X;      // test left edge
                                else if (PlayerProjectiles[i].position.X > e.BoundingBox.X + e.BoundingBox.Width) testX = e.BoundingBox.X + e.BoundingBox.Width;   // right edge
                                if (PlayerProjectiles[i].position.Y < e.BoundingBox.Y) testY = e.BoundingBox.Y;      // top edge
                                else if (PlayerProjectiles[i].position.Y > e.BoundingBox.Y + e.BoundingBox.Height) testY = e.BoundingBox.Y + e.BoundingBox.Height;   // bottom edge

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
                }
                else
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
                        if (InputManager.Instance.ActionKeyPressed(ActionType.pick_up))
                        {
                            Player.AddItem(WorldItems[i]);
                            WorldItems[i].OnGround = false;
                            WorldItems[i].removeable = true;
                        }
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

            if (signalLevelChange)
            {
                signalLevelChange = false;
                ChangeToNextLevel();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {




            spriteBatch.End();

            // the camera produces a view matrix that can be applied to any sprite batch
            spriteBatch.Begin(transformMatrix: cameraTransformationMatrix);

            // ... draw sprites here ...

            GameWorld.Draw(spriteBatch);

            foreach (GameInteractable i in WorldInteractables)
            {
                i.Draw(spriteBatch);
            }

            foreach (List<GameEnemy> el in Enemies)
            {
                foreach (GameEnemy e in el)
                {
                    e.Draw(spriteBatch);
                }
            }


            foreach (GameProjectile p in PlayerProjectiles)
            {
                p.Draw(spriteBatch);
            }


            foreach (GameItem i in WorldItems)
            {
                i.Draw(spriteBatch);
            }

            Player.Draw(spriteBatch);

            foreach (GameProjectile p in EnemyProjectiles)
            {
                p.Draw(spriteBatch);
            }


            spriteBatch.End();

            spriteBatch.Begin();


            // probably draw ui elements here

            headsUpDisplay.Draw(spriteBatch);

        }

        public void SaveGame()
        {

            Player.UnloadContent();

            foreach (List<GameEnemy> el in Enemies)
                foreach (GameEnemy e in el)
                    e.UnloadContent();
            foreach (GameProjectile e in EnemyProjectiles)
                e.UnloadContent();
            foreach (GameProjectile e in PlayerProjectiles)
                e.UnloadContent();
            foreach (GameItem e in WorldItems)
                e.UnloadContent();
            foreach (GameInteractable e in WorldInteractables)
                e.UnloadContent();

            GameSerializer.SerializeGame(this);


            Player.LoadContent(ref PlayerResources);

            foreach (List<GameEnemy> el in Enemies)
                foreach (GameEnemy e in el)
                {
                    if (e.type == typeof(GameGoblin))
                    {
                        e.LoadContent(ref GoblinResources);
                    } else if (e.type == typeof(GameFlyer))
                    {
                        e.LoadContent(ref FlyerResources);
                    }
                    else if (e.type == typeof(GameSlime))
                    {
                        e.LoadContent(ref SlimeResources);
                    }
                    else
                    {
                        e.LoadContent(ref BossResources);
                    }
                }
            foreach (GameProjectile e in EnemyProjectiles)
                e.LoadContent(ref ProjectileResources);
            foreach (GameProjectile e in PlayerProjectiles)
                e.LoadContent(ref ProjectileResources);
            foreach (GameItem e in WorldItems)
                e.LoadContent(ref LootResources);
            foreach (GameInteractable e in WorldInteractables)
            {
                if (e.type == typeof(ExitInteractable))
                    e.LoadContent(ref EnvironmentResources);
                else e.LoadContent(ref LootResources);
            }
        }


        public bool LoadGame()
        {
            if (File.Exists(saveFileName))
            {
                GameSerializer.DeSerializeGame(this);

                ///don't forget to reload content
                ///

                LevelIndex = GameWorld.LevelIndex;

                LoadCharacterResources();
                LoadItemResources();
                EnvironmentResources.LoadContent(LevelNames[LevelIndex]);

                GameWorld.LoadContent(ref EnvironmentResources);
                GameWorld.RenderTexture();
                GameWorld.GenerateNewRandom();

                Player.LoadContent(ref PlayerResources);

                foreach (List<GameEnemy> el in Enemies)
                    foreach (GameEnemy e in el)
                    {
                        if (e.type == typeof(GameGoblin))
                        {
                            e.LoadContent(ref GoblinResources);
                        }
                        else if (e.type == typeof(GameFlyer))
                        {
                            e.LoadContent(ref FlyerResources);
                        }
                        else if (e.type == typeof(GameSlime))
                        {
                            e.LoadContent(ref SlimeResources);
                        }
                        else
                        {
                            e.LoadContent(ref BossResources);
                        }
                    }
                foreach (GameProjectile e in EnemyProjectiles)
                    e.LoadContent(ref ProjectileResources);
                foreach (GameProjectile e in PlayerProjectiles)
                    e.LoadContent(ref ProjectileResources);
                foreach (GameItem e in WorldItems)
                    e.LoadContent(ref LootResources);
                foreach (GameInteractable e in WorldInteractables)
                {
                    if (e.type == typeof(ExitInteractable))
                        e.LoadContent(ref EnvironmentResources);
                    else e.LoadContent(ref LootResources);
                }

                LoadHUD();

                return true;
            }

            return false;
        }
    }
}
