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

        public int radius;

        public abstract void Use(GamePlayer user);


        public GameItem()
        {

        }

        public override void Update(GameTime gameTime)
        {

        }

        

    }
}
