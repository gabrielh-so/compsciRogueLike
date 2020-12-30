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
        public string itemType;

        public int radius;

        public bool OnGround;

        public abstract void Use(GamePlayer user);


        public GameItem()
        {

        }

        public override void Update(GameTime gameTime)
        {

        }

        

    }
}
