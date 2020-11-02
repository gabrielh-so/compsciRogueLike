using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MajorProject
{
    class GameScreen : Screen
    {
        World GameWorld;

        GameEntity PlayerEntity;


        public GameScreen()
        {
            GameWorld =  new World();

            GameWorld.room_count = 15;

            GameWorld.level_cell_width = 50;
            GameWorld.level_cell_height = 50;


            GameWorld.room_min_cell = new Vector2(4, 4);
            GameWorld.room_max_cell = new Vector2(8, 8);



            GameWorld.GenerateWorld();
            GameWorld.RenderTexture();

        }

        public override void LoadContent()
        {

        }

        public override void UnloadContent()
        {

        }

        public override void Update(GameTime gameTime)
        {
            // update each entity




            // detect collisions and trigger oncollide functions




            if (InputManager.Instance.KeyPressed(Keys.Escape))
                InputManager.Instance.QuitSignaled = true;

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            GameWorld.Draw(spriteBatch);
        }
    }
}
