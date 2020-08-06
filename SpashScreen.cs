using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MajorProject
{
    public class SplashScreen : Screen
    {
        public Image Image;
        double timeToShow = 7500;
        double totalUpTime;

        public override void LoadContent()
        {
            base.LoadContent();
            totalUpTime = 0;

            Image.LoadContent();
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            Image.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Image.Update(gameTime);

            totalUpTime += gameTime.ElapsedGameTime.TotalMilliseconds;

            if ((totalUpTime > timeToShow || InputManager.Instance.KeyDown(Keys.Enter)) && !ScreenManager.Instance.IsTransitioning)
            {
                ScreenManager.Instance.ChangeScreens("MainMenuScreen");
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Image.Draw(spriteBatch);
        }
    }
}
