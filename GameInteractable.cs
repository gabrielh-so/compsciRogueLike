using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajorProject
{
    public class GameInteractable : GameEntity
    {
        public bool IsHovering;

        public virtual void Use(int LevelIndex, GamePlayer user) { }
        public override void Update(GameTime gameTime)
        {
            //base.Update(gameTime);
        }

        public virtual void Hovering() { }

    }
}
