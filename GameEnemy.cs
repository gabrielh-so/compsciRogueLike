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
        public bool WasAlive;

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
            alive = true;
            WasAlive = true;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // enemies should drop coins on death
            // if enemy has died this frame (and only this frame), drop a coin
            if (WasAlive && !alive)
            {
                Random rand = new Random();
                GameCoin c = new GameCoin();
                c.value = 10;
                c.SetPosition(position.X, position.Y);
                double movementAngle = rand.NextDouble() * Math.PI * 2;
                c.velocity = new Vector2((float)Math.Sin(movementAngle), (float)Math.Cos(movementAngle));

                ((GameScreen)ScreenManager.Instance.currentScreen).AddItem(c);
            }

            // update the wasalive-otron
            WasAlive = alive;

            // update bounding box location
            BoundingBox.Location = position.ToPoint();
        }

        public override void UnloadContent()
        {
            // sets the target to null, removing any possible player references
            base.UnloadContent();

            target = null;
        }

        public override void OnDeath()
        {

        }

        public override void TakeDamage(int damage)
        {
            // takes damage and multiplies it based on difficulty value (scalar multiple can be edited in options)
            damage = (int)(damage * PlayerPreferences.enemyDamageScalars[PlayerPreferences.Instance.difficulty]);
            base.TakeDamage(damage);
        }

    }
}
