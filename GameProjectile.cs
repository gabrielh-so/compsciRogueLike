using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajorProject
{
    public class GameProjectile : GameEntity
    {

        Vector2 direction;

        double velocity;

        double lifeSpan;

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

        public GameProjectile()
        {
            SpriteSize = new Point(25, 25);
            image.centered = true;
        }

        public override void Update(GameTime gameTime)
        {
            image.Update(gameTime);
        }

        public override void LoadContent(ref ResourcePack resources)
        {
            base.LoadContent(ref resources);

            string[] textureNames = new string[3];
            for (int i = 0; i < 3; i++)
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


    }
}
