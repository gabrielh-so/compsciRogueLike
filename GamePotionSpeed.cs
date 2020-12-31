using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace MajorProject
{
    public class GamePotionSpeed : GamePotion
    {
        public GamePotionSpeed()
        {
            full = true;
            itemType = "Speed";
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Refill()
        {
            full = true;
            itemType = "Speed";
        }

        public override void Use(GamePlayer user)
        {
            if (full)
            {
                user.Boost(7.5, 275);
                full = false;
                itemType = "Empty";
            }
        }
    }
}
