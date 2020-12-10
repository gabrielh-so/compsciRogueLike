using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

// shorthand for the actiontype
using static MajorProject.InputManager;


namespace MajorProject
{
    class GamePlayer : GameCharacter
    {

        GameImage playerImage;

        public GamePlayer()
        {
            playerImage = new GameImage();
        }

        string[] walkAnimation =
        {
            "Player_Forward"
        };

        public override void LoadContent(ref ResourcePack resources)
        {
            base.LoadContent(ref resources);

            playerImage.LoadContent(ref Resources, walkAnimation);
            playerImage.animated = false;
            playerImage.centered = true;
            playerImage.SpriteSize = new Point(25, 25);
        }

        public override void Update(GameTime gameTime)
        {
            velocity = Vector2.Zero;

            if (InputManager.Instance.ActionKeyDown(ActionType.walk_right))
            {
                velocity.X += 1;
                //recordValues(playerlocation.X.ToString(), playerlocation.Y.ToString());
            }
            if (InputManager.Instance.ActionKeyDown(ActionType.walk_left))
            {
                velocity.X -= 1;
                //recordValues(position.X.ToString(), position.Y.ToString());
            }
            if (InputManager.Instance.ActionKeyDown(ActionType.walk_down))
            {
                velocity.Y += 1;
                //recordValues(playerlocation.X.ToString(), playerlocation.Y.ToString());
            }
            if (InputManager.Instance.ActionKeyDown(ActionType.walk_up))
            {
                velocity.Y -= 1;
                //recordValues(playerlocation.X.ToString(), playerlocation.Y.ToString());
            }

            // normalise velocity, apply scalar values and add to position // normalise returns NaN if magnitude is 0
            if ((velocity.X * velocity.X + velocity.Y * velocity.Y) > 0)
            {
                velocity.Normalize();
                velocity *= (float)(speed * (gameTime.ElapsedGameTime.TotalSeconds));
                position += velocity;
            }

            // update boxes
            BoundingBox.Location = position.ToPoint();


            playerImage.position = position.ToPoint();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            playerImage.Draw(spriteBatch);
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


    }
}
