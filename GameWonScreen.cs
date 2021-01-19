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
    public class GameWonScreen : Screen
    {

        public Label timeLabel;

        public GameWonScreen()
        {

        }

        public override void LoadContent()
        {
            int totalseconds = (int)Math.Floor(  ((GameScreen)ScreenManager.Instance.oldScreen).Player.SecondsPlayed   );
            int seconds = totalseconds % 60;
            int minutes = (totalseconds - seconds) / 60;
            string text = "Total time: " + minutes + " minutes " + seconds + " seconds.\nPress Enter to continue.";


            timeLabel.LoadContent();
            timeLabel.Text = text;
        }

        public override void UnloadContent()
        {
            timeLabel.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (InputManager.Instance.KeyPressed(Keys.Enter))
            {
                ScreenManager.Instance.LoadPreservedScreen();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            timeLabel.Draw(spriteBatch);
        }


    }
}
