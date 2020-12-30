using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajorProject
{
    public class GameProjectile : GameEntity
    {
        public double speed;

        public double totalLifeSpan;
        public double currentLifeSpan;

        public int[,] Map;
        

        GameImage image;

        public Point SpriteSize;


        string[] textureNames = new string[3];

        public enum DamageType
        {
            Kinetic,
            Fire,
            Ice,
            Toxic,
            SwordSwipe
        }

        public DamageType damageType;
        public Type target;
        public int radius;
        public int damage;

        public GameProjectile()
        {
            image = new GameImage();
            totalLifeSpan = 2;
            currentLifeSpan = 0;
            SpriteSize = new Point(25, 25);
            image.centered = true;
            radius = 12;
        }

        public void SetVelocity(Vector2 p)
        {
            velocity = p;
            if (velocity.X * velocity.X + velocity.Y * velocity.Y > 0)
            {
                velocity.Normalize();
            }
        }

        public override void Update(GameTime gameTime)
        {
            image.Update(gameTime);

            currentLifeSpan += gameTime.ElapsedGameTime.TotalSeconds;

            position.X += (float)(velocity.X * speed);
            position.Y += (float)(velocity.Y * speed);

            // check they aren't too old
            if (currentLifeSpan >= totalLifeSpan)
            {
                removeable = true;
            }

            // check they haven't hit wall
            if (damageType != DamageType.SwordSwipe)
                if (Map[(int)position.Y / World.tilePixelWidth, (int)position.X / World.tilePixelWidth] != (int)World.cellType.floor)
                {
                    removeable = true;
                }


        }

        public override void LoadContent(ref ResourcePack resources)
        {
            base.LoadContent(ref resources);

            image.SpriteSize = new Point(25, 25);

            for (int i = 1; i < 4; i++)
            {
                textureNames[i-1] = damageType.ToString() + i.ToString();
            }

            image.LoadContent(ref Resources, textureNames);
            image.animated = true;
        }

        public override void UnloadContent()
        {
            image.UnloadContent();

            base.UnloadContent();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            image.position = position.ToPoint();
            image.Draw(spriteBatch);
        }


    }
}
