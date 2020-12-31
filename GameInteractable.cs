using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajorProject
{
    public abstract class GameInteractable : GameEntity
    {
        public bool IsHovering;

        public abstract void Use(int LevelIndex, GamePlayer user);

        public abstract void Hovering();

    }
}
