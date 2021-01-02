using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Serialization;

namespace MajorProject
{
    public abstract class GameCharacter : GameEntity
    {
        // this is a character
        // (yeah thanks gab)

        Random rand;



        public List<GameProjectile> projectiles;

        public GameCharacter()
        {
            meleeDamage = 10;
            projectileDamage = 5;
            rand = new Random();
            projectiles = new List<GameProjectile>();
            maxHealth = 100;
            alive = true;
            health = maxHealth;
            lastHealth = maxHealth;
        }

        public int currentRoom = -1;

        public int maxHealth;
        public int health;
        public int lastHealth;

        public double speed = 250;

        public int radius; // radius of a circle hitbox! can be used with circles or

        public bool alive;

        [XmlIgnore]
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
            TakeDamage(p.damage);
        }


        public override void Update(GameTime gameTime)
        {
            for (int i = projectiles.Count - 1; i > -1; i--)
            {
                projectiles[i].Update(gameTime);
                if (projectiles[i].removeable)
                {
                    projectiles[i].UnloadContent();
                    projectiles.RemoveAt(i);
                }
            }

            if (health < lastHealth)
            {
                GameProjectile p = new GameProjectile();

                double i = rand.NextDouble() * Math.PI * 2;

                p.position = new Vector2(position.X, position.Y);
                p.target = typeof(GamePlayer);
                p.totalLifeSpan = 5;
                p.velocity = new Vector2((float)Math.Sin(i), (float)Math.Cos(i));
                p.speed = 150;
                p.damageType = GameProjectile.DamageType.Blood;
                p.damage = 0;
                p.hit = true;
                p.BoundingBox.Location = position.ToPoint();
                p.BoundingBox.Size = new Point(25, 25);


                // now *that's* punk
                ((GameScreen)ScreenManager.Instance.currentScreen).AddProjectile(p);
            }

            if (health <= 0)
            {
                alive = false;
            }

            BoundingBox.X = (int)position.X - BoundingBox.Size.X / 2;
            BoundingBox.Y = (int)position.Y - BoundingBox.Size.Y / 2;

            lastHealth = health;

        }


        public virtual void onCollision(GameCharacter e)
        {

        }

        void OnDeath()
        {

        }

    }
}
