using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MajorProject
{
    public class GameFlyer : GameEnemy
    {

        GameImage flyerImage;
        GameImage flyerAttackImage;


        string[] walkAnimation =
        {
            "Flyer1",
            "Flyer2",
            "Flyer3"
        };




        public GameFlyer()
        {
            flyerImage = new GameImage();
            type = GetType();
        }

        public override void LoadContent(ref ResourcePack resources)
        {
            base.LoadContent(ref resources);

            speed = 50;

            maxHealth = 50;
            health = maxHealth;
            BoundingBox.Size = new Point(25, 25);

            flyerImage.LoadContent(ref Resources, walkAnimation);
            flyerImage.animated = true;
            flyerImage.centered = true;
            flyerImage.SpriteSize = new Point(25, 25);
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

            flyerImage.position = position.ToPoint();
            flyerImage.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            flyerImage.Draw(spriteBatch);
        }

    }
}
