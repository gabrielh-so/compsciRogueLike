using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace MajorProject
{
    public class SplashScreen : Screen
    {
        public Image Image;
        double timeToShow = 7500;
        double totalUpTime;

        public override void LoadContent()
        {
            // load the image and initialise the values
            base.LoadContent();
            totalUpTime = 0;

            AudioManager.Instance.PlayMusic("Audio/Sound/UI/Music/Music");

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

            // if timer has finished or keyboard is pressed and the screenmanager isn't already transitioning, change screens
            if ((totalUpTime > timeToShow || InputManager.Instance.KeyDown(Keys.Enter)) && !ScreenManager.Instance.IsTransitioning)
            {
                ScreenManager.Instance.ChangeScreens("MainMenuScreen");
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // just draws the image onto the screen
            Image.Draw(spriteBatch);
        }
    }
}
