using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MajorProject
{
    public class GameSlime : GameEnemy
    {

        GameImage slimeImage;
        GameImage slimeAttackImage;


        string[] walkAnimation =
        {
            "Slime1",
            "Slime2",
            "Slime3"
        };




        public GameSlime()
        {

            slimeImage = new GameImage();
            type = GetType();
            speed = 25;


            maxHealth = 150;
            health = maxHealth;
        }

        public override void LoadContent(ref ResourcePack resources)
        {
            base.LoadContent(ref resources);

            slimeImage.LoadContent(ref Resources, walkAnimation);
            slimeImage.animated = true;
            slimeImage.centered = true;
            slimeImage.SpriteSize = new Point(25, 25);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (alive)
            {

                if (target != null)
                {

                    Vector2 directionVector = (target.BoundingBox.Location - BoundingBox.Location).ToVector2();

                    // check magnitude is bigger than 0 before normallizing
                    if (Math.Abs(directionVector.X) + Math.Abs(directionVector.Y) > 0)
                        directionVector.Normalize();

                    velocity.X = (float)(directionVector.X * speed * (float)gameTime.ElapsedGameTime.TotalSeconds);
                    velocity.Y = (float)(directionVector.Y * speed * (float)gameTime.ElapsedGameTime.TotalSeconds);
                }
                else velocity = Vector2.Zero;


                position += velocity;
            }



            slimeImage.position = position.ToPoint();
            slimeImage.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            slimeImage.Draw(spriteBatch);
        }




        public override void UnloadContent()
        {
            base.UnloadContent();

            slimeImage.UnloadContent();
        }

    }
}
