using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajorProject
{
    public abstract class GameCharacter : GameEntity
    {
        // this is a character

        public int health;

        public int speed = 250;

        public bool alive;

        public int[,] Map;

        public int projectileDamage;
        public int meleeDamage;

        public virtual void TakeDamage(int damage)
        {
            // all damage should be routed through damage functions, in case environment / difficulty modifiers affect the value

            health -= damage;
        }




        public override void Update(GameTime gameTime)
        {

            if (health <= 0)
            {
                alive = false;
            }

            BoundingBox.Location = position.ToPoint();

        }
        void OnDeath()
        {

        }

    }
}
