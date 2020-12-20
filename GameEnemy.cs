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

            

        }

        public override void UnloadContent()
        {
            base.UnloadContent();

            target = null;
        }

    }
}
