using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajorProject
{
    public class GameEnemy : GameCharacter
    {
        public GameCharacter target;

        public int speed = 100;

        public void SetTarget(GameCharacter t)
        {
            target = t;
        }
        public void RemoveTarget()
        {
            target = null;
        }

        public GameEnemy()
        {

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            /*

            if (target != null)
            {

                Vector2 directionVector = (target.BoundingBox.Location - BoundingBox.Location).ToVector2();

                directionVector.Normalize();

                velocity = directionVector * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else velocity = Vector2.Zero;

            */


            position += velocity;

        }

        public override void UnloadContent()
        {
            base.UnloadContent();

            target = null;
        }

    }
}
