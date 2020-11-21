using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using static MajorProject.InputManager;

namespace MajorProject
{

    public class GameScreen : Screen
    {

        Vector2 ScreenSize;

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

        public ResourcePack PlayerResources;
        public ResourcePack GoblinResources;

        public EnvironmentResourcePack EnvironmentResources;

        public GameScreen()
        {
            ScreenSize = ScreenManager.Instance.Dimensions;

            GameWorld =  new World();

            GameWorld.room_count = 4;

            GameWorld.level_cell_width = 14;
            GameWorld.level_cell_height = 14;


            GameWorld.room_min_cell = new Vector2(4, 4);
            GameWorld.room_max_cell = new Vector2(8, 8);

            Player = new GamePlayer();

            cameraTransformationMatrix = Matrix.Identity;
        }

        public void ConstructWorld()
        {
            EnvironmentResources.LoadContent(LevelNames[LevelIndex]);

            GameWorld.LoadContent(ref EnvironmentResources);
            GameWorld.GenerateWorld();
            GameWorld.RenderTexture();

            AudioManager.Instance.PlaySoundInstance(EnvironmentResources.AudioPack["Music"].CreateInstance(), "Music", true);
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

        public void LoadPlayer()
        {
            PlayerResources.LoadContent();
            Player.LoadContent(ref PlayerResources);
            Player.Map = GameWorld.Map;
            SetPlayerToEntrance();
        }

        public override void LoadContent()
        {
            ConstructWorld();

            LoadPlayer();
        }

        public override void UnloadContent()
        {
            Player.UnloadContent();

            GameWorld.UnloadContent();

            PlayerResources.UnloadContent();
            GoblinResources.UnloadContent();
            EnvironmentResources.UnloadContent();
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

            Player.Update(gameTime);


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






            spriteBatch.End();

            spriteBatch.Begin();


            // probably draw ui elements here

        }
    }
}
