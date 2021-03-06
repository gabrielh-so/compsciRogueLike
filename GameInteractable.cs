using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajorProject
{
    public abstract class GameInteractable : GameEntity
    {
        // bool for if the player is hovering over the interactable
        public bool IsHovering;

        // all interactables must have a use case
        public abstract void Use(int LevelIndex, GamePlayer user);

        // update function to be called
        public override void Update(GameTime gameTime)
        {
            //base.Update(gameTime);
        }

        // function for when the player is hovering
        public virtual void Hovering() { }

    }
}
