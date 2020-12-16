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
        double speed;

        public double totalLifeSpan;
        public double currentLifeSpan;

        GameImage image;

        public Point SpriteSize;

        public enum DamageType
        {
            Kinetic,
            Fire,
            Ice,
            Toxic
        }

        public DamageType damageType;
        public Type target;
        public int radius;
        public int damage;

        public GameProjectile()
        {
            totalLifeSpan = 2;
            currentLifeSpan = 0;
            SpriteSize = new Point(25, 25);
            image.centered = true;
            radius = 12;
        }

        public override void Update(GameTime gameTime)
        {
            image.Update(gameTime);

            currentLifeSpan += gameTime.ElapsedGameTime.TotalSeconds;
        }

        public override void LoadContent(ref ResourcePack resources)
        {
            base.LoadContent(ref resources);

            string[] textureNames = new string[3];
            for (int i = 1; i < 4; i++)
            {
                textureNames[i] = damageType.ToString() + i.ToString();
            }

            image.LoadContent(ref Resources, textureNames);
        }

        public override void UnloadContent()
        {
            image.UnloadContent();

            base.UnloadContent();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            image.Draw(spriteBatch);
        }


    }
}
