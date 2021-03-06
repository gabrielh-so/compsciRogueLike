using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajorProject
{
    public abstract class GameAbility : GameItem
    {

        public GameAbility()
        {

        }

        // generic use ability // not implemented/needed in the final game
        public override void Use(GamePlayer user)
        {
            throw new NotImplementedException();
        }

    }
}
