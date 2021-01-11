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

            WasAlive = true;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

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

            WasAlive = alive;
        }

        public override void UnloadContent()
        {
            base.UnloadContent();

            target = null;
        }

    }
}
