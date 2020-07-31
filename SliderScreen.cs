using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MajorProject
{
    public class SliderScreen : Screen
    {
        public Slider mySlider;

        public SliderScreen()
        {

        }

        public override void LoadContent()
        {
            mySlider.OnActivate = new UiElement.onActivate(LoadMenu);
            mySlider.LoadContent();
        }

        public override void UnloadContent()
        {
            mySlider.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            mySlider.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            mySlider.Draw(spriteBatch);
        }

        void LoadMenu()
        {
            //update the options with it
            bool t = false;
            t = !t;
        }
    }
}
