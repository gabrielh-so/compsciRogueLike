using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MajorProject
{
    public abstract class GameEntity
    {

        public double positionX;
        public double positionY;

        



        public GameEntity()
        {

        }

        public virtual void LoadContent()
        {

        }

        public virtual void UnloadContent()
        {

        }

        public abstract void Update(GameTime gameTime); // all game entities must have an update loop of some sort - otherwise they're probably pointless

        public virtual void Draw()
        {

        }


    }
}
