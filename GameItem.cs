using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajorProject
{
    public abstract class GameItem : GameEntity
    {
        public string itemType;

        public string Description;

        public int radius;

        public bool OnGround;

        public virtual void Use(GamePlayer user)
        {

        }


        public GameItem()
        {

        }

        public virtual void WriteDescription()
        {
            return;
        }

        public override void Update(GameTime gameTime)
        {
            // account for friction - allows launched coins to stop at some point
            if (velocity.LengthSquared() > 0)
            {
                velocity *= 0.95f;

                position += velocity;
                BoundingBox.Location = position.ToPoint();
                BoundingBox.X -= BoundingBox.Width / 2;
                BoundingBox.Y -= BoundingBox.Height / 2;

            }

        }

        

    }
}
