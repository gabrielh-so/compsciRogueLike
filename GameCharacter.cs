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
        // (yeah thanks gab)

        public GameCharacter()
        {
            maxHealth = 100;
            alive = true;
            health = maxHealth;
        }

        public int currentRoom = -1;

        public int maxHealth;
        public int health;

        public int speed = 250;

        public int radius; // radius of a circle hitbox! can be used with circles or

        public bool alive;

        public int[,] Map;
        public int tileWidth;

        public int projectileDamage;
        public int meleeDamage;

        public virtual void TakeDamage(int damage)
        {
            // all damage should be routed through damage functions, in case environment / difficulty modifiers affect the value

            health -= damage;
        }

        public virtual void ProjectileCollision(GameProjectile p)
        {
            
        }


        public override void Update(GameTime gameTime)
        {

            if (health <= 0)
            {
                alive = false;
            }

            BoundingBox.Location = position.ToPoint();

        }


        public void onCollision(GameEntity e)
        {

        }

        void OnDeath()
        {

        }

    }
}
