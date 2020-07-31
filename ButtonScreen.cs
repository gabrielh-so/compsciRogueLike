using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace MajorProject
{
    public class ButtonScreen : Screen
    {
        public Button myButton;

        public ButtonScreen()
        {
            
        }

        public override void LoadContent()
        {
            myButton.OnActivate = new UiElement.onActivate(LoadMenu);
            myButton.LoadContent();
        }

        public override void UnloadContent()
        {
            myButton.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            myButton.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            myButton.Draw(spriteBatch);
        }

        void LoadMenu()
        {
            ScreenManager.Instance.ChangeScreens("SplashScreen");
        }
    }
}
