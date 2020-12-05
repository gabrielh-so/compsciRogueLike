using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajorProject
{
    public class GameEnemy : GameCharacter
    {
        GameCharacter target;


        public void SetTarget(GameCharacter target)
        {

        }
        public void RemoveTarget(GameCharacter target)
        {

        }

        public GameEnemy()
        {

        }

        public override void UnloadContent()
        {
            base.UnloadContent();

            target = null;
        }

    }
}
