using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using static MajorProject.InputManager;
using static MajorProject.PlayerPreferences;

namespace MajorProject
{

    public class GameScreen : Screen
    {

        // this class is a doosy


        // all the values to execute save functionality

        // sepeate thread for executing save function
        [XmlIgnore]
        public Thread SerialiserThread;
        static string saveFileName = "SaveFile.bin";
        public bool hasSaved;
        public Mutex saveOperationMut = new Mutex();

        // a flag to show some system wants to advance the level forward
        public bool signalLevelChange;

        Random rand;

        Vector2 ScreenSize;

        float secondsPlayed;

        public void SignalLevelChange()
        {
            signalLevelChange = true;
        }

        // all the different types of rooms that can be placed
        public enum RoomType
        {
            Loot,
            Combat,
            Boss,
            Shop
        }

        // the base shop prices for each world
        readonly int[] shopPrices = { 1000, 2000, 3000, 4000, 5000 };

        // the names of all the different levels
        string[] LevelNames =
        {
            "TestArea",
            "Dungeon",
            "Volcano",
            "Jungle",
            "Temple"
        };

        // incremented each time the player passes through a door
        public int LevelIndex;

        Matrix cameraTransformationMatrix;

        [XmlIgnore]
        public World GameWorld;

        public GamePlayer Player;

        public HUD headsUpDisplay;


        // lists of all the game entities in the world

        [XmlIgnore]
        public List<List<GameEnemy>> Enemies;
        public List<GameProjectile> EnemyProjectiles;
        public List<GameProjectile> PlayerProjectiles;

        public List<GameItem> WorldItems;

        public List<GameInteractable> WorldInteractables;


        // all the character resource backs

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
            // initialising the values for generating worlds
            rand = new Random();

            ScreenSize = ScreenManager.Instance.Dimensions;

            GameWorld = new World();

            GameWorld.room_count = 8;

            GameWorld.level_cell_width = 25;
            GameWorld.level_cell_height = 25;


            GameWorld.room_min_cell = new Vector2(4, 4);
            GameWorld.room_max_cell = new Vector2(6, 6);


            // creating the player object
            Player = new GamePlayer();

            // lists of the enemies in each room - some are empty
            Enemies = new List<List<GameEnemy>>();

            // lists of player and enemy projectiles respectively
            PlayerProjectiles = new List<GameProjectile>();
            EnemyProjectiles = new List<GameProjectile>();

            // all the world interactables
            WorldInteractables = new List<GameInteractable>();

            // all the other world items
            WorldItems = new List<GameItem>();

            
            cameraTransformationMatrix = Matrix.Identity;

            
        }

        public void ConstructWorld()
        {
            // reloads enviroment resources based on level index
            // reloads gameworld
            // generates new world
            // renders the texture of the world

            EnvironmentResources.LoadContent(LevelNames[LevelIndex]);

            GameWorld.LoadContent(ref EnvironmentResources);
            GameWorld.GenerateWorld();
            GameWorld.RenderTexture();

            // resets the music
            AudioManager.Instance.PlayMusic(EnvironmentResources.AudioPack["Music"].Name);
        }

        public void RegenerateWorld()
        {

            // unloads the gameworld content
            // unloads environment content
            // unloads all the different types of game entities
            // clears all the gameentities from the lists
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



            // regenerates world

            ConstructWorld();

            // generates the minimap texture
            headsUpDisplay.GenerateMiniMap(GameWorld.Map);

            // adds low-level items at the end of some corridoors
            PlaceLoot();

            // adds enemies and contents to rooms
            LoadRoomContents();

            AudioManager.Instance.PlayMusic(EnvironmentResources.AudioPack["Music"].Name);
        }

        // sets the player's position to the entrance
        public void SetPlayerToEntrance()
        {
            Player.position.X = (GameWorld.entryIndex.X + 0.5f) * World.tilePixelWidth;
            Player.position.Y = (GameWorld.entryIndex.Y + 0.5f) * World.tilePixelHeight;
        }

        // given a projectile, determines type and adds it to either projectile array
        public void AddProjectile(GameProjectile p)
        {
            // hands the projectile the gamemap to use
            p.Map = GameWorld.Map;
            p.LoadContent(ref ProjectileResources);
            if (p.target == typeof(GamePlayer))
            {
                EnemyProjectiles.Add(p);
            }
            else
                PlayerProjectiles.Add(p);
        }

        // given an item, adds the item to the worlditem list after setting appropriate values
        public void AddItem(GameItem i)
        {
            i.removeable = false;
            i.OnGround = true;
            i.LoadContent(ref LootResources);
            WorldItems.Add(i);
        }


        // initialise player values
        public void LoadPlayer()
        {
            Player.tileWidth = World.tilePixelWidth;
            Player.LoadContent(ref PlayerResources);
            Player.Map = GameWorld.Map;
            SetPlayerToEntrance();
        }

        // iterates over an enemy list to check if any are alive
        public bool IsRoomDead(int room)
        {
            if (room >= GameWorld.room_count || room < 0) return true; // anything that isn't a room doesn't have enemies in it
            foreach (GameEnemy e in Enemies[room]) if (e.alive)
            {
                return false;
            }
            return true;
        }

        // iterates over each room in the gameworld, and adds content to that room based off type
        void LoadRoomContents()
        {

            for (int i = 0; i < GameWorld.room_count; i++)
            {
                // protects against not enough rooms being generated - rarely happens, but enough to warrant protection
                if (GameWorld.generation_RoomIndexPositions.Count <= i) continue;

                // set relative position values
                Vector2 RoomPosition = GameWorld.generation_RoomIndexPositions[i];
                Vector2 RoomCentre = GameWorld.generation_RoomIndexPositions[i] + GameWorld.generation_RoomIndexDimensions[i] / 2;
                Enemies.Add(new List<GameEnemy>());

                // adds different stuff based off the room types
                switch (GameWorld.roomTypes[i])
                {
                    case RoomType.Combat:
                        for (int j = 0; j < GameWorld.generation_RoomIndexDimensions[i].X; j++)
                        {


                            // creates a generic enemy object, and assigns it new values

                            GameEnemy e = new GameEnemy();

                            int enemyIndex = rand.Next(0, 3);

                            switch (enemyIndex)
                            {
                                case 0:
                                    e = new GameGoblin();
                                    e.LoadContent(ref GoblinResources);
                                    e.SetHealth(PlayerPreferences.Instance.enemyHealth[LevelIndex]["Goblin"]);
                                    break;
                                case 1:
                                    e = new GameSlime();
                                    e.LoadContent(ref SlimeResources);
                                    e.SetHealth(PlayerPreferences.Instance.enemyHealth[LevelIndex]["Slime"]);
                                    break;
                                case 2:
                                    e = new GameFlyer();
                                    e.LoadContent(ref FlyerResources);
                                    e.SetHealth(PlayerPreferences.Instance.enemyHealth[LevelIndex]["Flyer"]);
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

                        // adds a tresure chest in the centre of the room
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
                            
                            s.LoadContent(ref LootResources);
                            s.NewItem(shopPrices[LevelIndex] + (int)(shopPrices[0] * (float)j * 1/3));
                            WorldInteractables.Add(s);
                        }
                        break;
                    case RoomType.Boss:

                        // adds a boss character in the centre
                        // also adds an exit

                        GameBoss b = new GameBoss();

                        b.position = RoomCentre * World.tilePixelWidth;
                        b.LoadContent(ref BossResources);

                        b.SetHealth(PlayerPreferences.Instance.enemyHealth[LevelIndex]["Boss"]);

                        Enemies[i].Add(b);

                        ExitInteractable exit = new ExitInteractable();
                        exit.position = RoomCentre * World.tilePixelWidth;
                        exit.LoadContent(ref EnvironmentResources);

                        WorldInteractables.Add(exit);
                        break;
                }



            }
        }

        // loads the resouces used by projectiles and items
        void LoadItemResources()
        {
            LootResources.LoadContent();
            ProjectileResources.LoadContent();
        }

        // loads the resources used by characters
        void LoadCharacterResources()
        {
            PlayerResources.LoadContent();
            GoblinResources.LoadContent();
            FlyerResources.LoadContent();
            SlimeResources.LoadContent();
            BossResources.LoadContent();
        }

        // assigns an enum value to the list of rooms
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
                                w.SetWeaponDamage(PlayerPreferences.Instance.weaponDamages[LevelIndex]["Sword"]);
                            }
                            else if (weaponRoll < 0.5)
                            {
                                w = new GameWeaponSpear();
                                w.SetWeaponDamage(PlayerPreferences.Instance.weaponDamages[LevelIndex]["Spear"]);
                            }
                            else if (weaponRoll < 0.75)
                            {
                                w = new GameWeaponSlingShot();
                                w.SetWeaponDamage(PlayerPreferences.Instance.weaponDamages[LevelIndex]["Slingshot"]);
                            }
                            else
                            {
                                w = new GameWeaponRifle();
                                w.SetWeaponDamage(PlayerPreferences.Instance.weaponDamages[LevelIndex]["Rifle"]);
                            }

                            w.SetWeaponDamage( (int) (w.Damage * (1 + (rand.NextDouble() - 0.5)/5)) );

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

        // loads content in the heads up display, and passes values it needs to function
        public void LoadHUD()
        {
            headsUpDisplay = new HUD();
            HUDResources.LoadContent();
            headsUpDisplay.LoadContent(HUDResources);
            headsUpDisplay.GenerateMiniMap(GameWorld.Map);
            headsUpDisplay.SetPlayer(Player);
        }


        // loads all the level procedually
        public override void LoadContent()
        {
            // we don't want to regenerate the world if the class contains information already that's been loaded in from a save file
            if (PlayerPreferences.Instance.LoadSavedGame)
            {
                if (LoadGame()) return;

                // if load game doesn't return, the save file has probably been corrupted, so create a new world instead
            }

            // generates the tile map
            ConstructWorld();

            LoadCharacterResources();

            LoadPlayer();

            LoadHUD();

            LoadItemResources();

            AssignRooms();

            LoadRoomContents();

            PlaceLoot();

            SetCharacterAndProjectileMaps();

            AudioManager.Instance.PlayMusic(EnvironmentResources.AudioPack["Music"].Name);
        }

        // iterates over the player and projectiles, and assigns the gameworld map
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
        void ChangeToStartingLevel()
        {
            LevelIndex = -1;
            ChangeToNextLevel();

        }

        void ChangeToNextLevel()
        {
            /// check if the completed level was the last one
            LevelIndex++;

            // if the level index has moved beyond the level array, the game is beaten - display the game won screen
            if (LevelIndex == LevelNames.Length)
            {
                // show score splashscreen
                ScreenManager.Instance.ChangeScreens("GameWonScreen", true);

            }
           
            // regenerate world and loop back around if the end of all the levels
            LevelIndex %= LevelNames.Length;
            GameWorld.LevelIndex = LevelIndex;
            AudioManager.Instance.StopSoundInstance("Music", true);
            RegenerateWorld();
            SetCharacterAndProjectileMaps();
            SetPlayerToEntrance();

        }

        public override void Update(GameTime gameTime)
        {
            // reset save flag (because main update has been run and game state has changed)
            if (hasSaved)
            {
                hasSaved = false;
            }

            if (InputManager.Instance.KeyPressed(Keys.R))
            {
                Enemies[0][0].alive = false;
            }

            // only want to quit if the player is alive
            if (Player.alive)
            {
                if (InputManager.Instance.KeyPressed(Keys.Escape))
                {
                    ScreenManager.Instance.ChangeScreens("GameMenuScreen", true);
                }
                foreach (GameInteractable i in WorldInteractables)
                {
                    i.Update(gameTime);
                }

                // update the player
                Player.Update(gameTime);

                for (int i = 0; i < GameWorld.room_count; i++)
                {
                    for (int j = 0; j < Enemies[i].Count; j++)
                    {
                        Enemies[i][j].Update(gameTime);
                    }
                }

                // update the world items
                for (int i = 0; i < WorldItems.Count; i++)
                {
                    WorldItems[i].Update(gameTime);
                }

                // updates all the projectiles
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

                    // iterates over all the world interactables, and check if the user's interacting with any of them
                    for (int i = 0; i < WorldInteractables.Count; i++)
                    {
                        if (WorldInteractables[i].BoundingBox.Contains(Player.BoundingBox))
                        {
                            if (InputManager.Instance.ActionKeyDown(ActionType.interact))
                            {
                                WorldInteractables[i].Use(LevelIndex, Player);
                            }
                            // if the player is hovering over the interactable, the interactable should display some prompt
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
                        {
                            Player.ProjectileCollision(EnemyProjectiles[i]);
                            EnemyProjectiles[i].OnCollision();
                        }

                        //destroy the projectile
                        EnemyProjectiles[i].removeable = true;
                    }
                }

                // removes any enemy projectiles that have signalled completion
                for (int i = EnemyProjectiles.Count - 1; i > -1; i--)
                {
                    if (EnemyProjectiles[i].removeable)
                    {
                        EnemyProjectiles.RemoveAt(i);
                    }
                }

                // if the player is in a room
                if (Player.currentRoom > -1)
                    foreach (List<GameEnemy> el in Enemies)
                    {
                        // iterate over every enemy in the room, and compare position with player projectiles
                        foreach (GameEnemy e in el)
                        {
                            if (e.alive)
                            {
                                // go backwards through projectile array
                                for (int i = PlayerProjectiles.Count - 1; i > -1; i--)
                                {
                                    if (!PlayerProjectiles[i].hit)
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
                                            PlayerProjectiles[i].hit = true;

                                            PlayerProjectiles[i].OnCollision();
                                        }
                                    }
                                }
                            }
                        }
                    }

                // remove any expired player projectiles
                for (int i = PlayerProjectiles.Count - 1; i > -1; i--)
                {
                    if (PlayerProjectiles[i].removeable)
                    {
                        PlayerProjectiles.RemoveAt(i);
                    }
                }

                bool playerIntersects = false;

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
                            if (!playerIntersects)
                            {
                                // player has not already intersected something - now they have
                                playerIntersects = true;

                                // send to the hud to display details of item

                                headsUpDisplay.ShowDetailsOfItem(WorldItems[i]);

                                if (InputManager.Instance.ActionKeyPressed(ActionType.pick_up))
                                {
                                    Player.AddItem(WorldItems[i]);
                                    WorldItems[i].OnGround = false;
                                    WorldItems[i].removeable = true;
                                }
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

                // removes any worlditems that have been flagged for removal
                for (int i = WorldItems.Count - 1; i > -1; i--)
                {
                    if (WorldItems[i].removeable)
                    {
                        WorldItems.RemoveAt(i);
                    }
                }

                // update the heads up display
                headsUpDisplay.Update(gameTime);
            }
            else
            {
                // player is dead, so take input to sacrifice an item in the inventory
                if (headsUpDisplay.UpdateSacrificeSelection())
                {
                    ChangeToStartingLevel();
                }
            }

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

            // goes through every game entity and draws it to the screen
            // only draws everything else if the player is alive
            if (Player.alive)
            {
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

                foreach (GameItem i in WorldItems)
                {
                    i.Draw(spriteBatch);
                }


                foreach (GameProjectile p in PlayerProjectiles)
                {
                    p.Draw(spriteBatch);
                }

                foreach (GameProjectile p in EnemyProjectiles)
                {
                    p.Draw(spriteBatch);
                }
            }

            // draws the player
            Player.Draw(spriteBatch);



            spriteBatch.End();

            spriteBatch.Begin();


            // probably draw ui elements here

            headsUpDisplay.Draw(spriteBatch);

        }

        public void SaveGame()
        {

            // checks if game has been saved in menu before
            if (!hasSaved)
            {
                hasSaved = true;

                // unloads all non-savable content before saving

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

                // creates a thread to save the game
                SerialiserThread = new Thread(() => GameSerializer.SerializeGame(this));
                SerialiserThread.Start();

            }

        }


        public bool LoadGame()
        {

            // checks there's a savefile
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

                AudioManager.Instance.PlayMusic(EnvironmentResources.AudioPack["Music"].Name);

                return true;
            }

            AudioManager.Instance.PlayMusic(EnvironmentResources.AudioPack["Music"].Name);

            return false;
        }

        public void ReloadSerialisedContent()
        {
            
            Player.LoadContent(ref PlayerResources);

            Player.currentRoom = -1;

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


        }
    }
}
