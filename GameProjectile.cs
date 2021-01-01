using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Serialization;

namespace MajorProject
{
    public class GameProjectile : GameEntity
    {
        public double speed;

        public double totalLifeSpan;
        public double currentLifeSpan;

        [XmlIgnore]
        public int[,] Map;
        

        GameImage image;


        string[] textureNames = new string[3];

        public enum DamageType
        {
            Kinetic,
            Fire,
            Ice,
            Toxic,
            SwordSwipe,
            Bullet
        }

        public DamageType damageType;
        public Type target;
        public int radius;
        public int damage;

        public GameProjectile()
        {
            image = new GameImage();
            type = this.GetType();
            currentLifeSpan = 0;
            totalLifeSpan = 2;
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

            position.X += (float)(velocity.X * speed * gameTime.ElapsedGameTime.TotalSeconds);
            position.Y += (float)(velocity.Y * speed * gameTime.ElapsedGameTime.TotalSeconds);

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

            SpriteSize = new Point(25, 25);
            image.centered = true;
            radius = 12;

            image.SpriteSize = SpriteSize;

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
