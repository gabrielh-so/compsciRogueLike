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

        public Image myImage;
        public Image myOtherImage;

        public SliderScreen()
        {

        }

        public override void LoadContent()
        {
            //mySlider.OnActivate = new UiElement.onActivate(LoadMenu);
            mySlider.OnActivateF = new UiElement.onActivateF(MoveImage);
            mySlider.LoadContent();
            myImage.LoadContent();
            myOtherImage.LoadContent();
            
        }

        public override void UnloadContent()
        {
            mySlider.UnloadContent();
            myImage.UnloadContent();
            myOtherImage.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            mySlider.Update(gameTime);
            myImage.Update(gameTime);
            myOtherImage.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            mySlider.Draw(spriteBatch);
            myImage.Draw(spriteBatch);
            myOtherImage.Draw(spriteBatch);
        }

        void LoadMenu()
        {
            //update the options with it
            bool t = false;
            t = !t;
        }

        private static float map(float value, float fromLow, float fromHigh, float toLow, float toHigh)
        {
            return (value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
        }

        void MoveImage(UiElement triggerElement, float value)
        {
            myOtherImage.Position.X = map(value, 0, 1, 0, ScreenManager.Instance.Dimensions.X);
        }
    }
}
