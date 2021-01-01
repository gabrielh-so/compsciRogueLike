using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace MajorProject
{
    public class GamePotionHealth : GamePotion
    {
        public GamePotionHealth()
        {
            full = true;
            type = this.GetType();
            itemType = "Health";
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Refill()
        {
            full = true;
            itemType = "Health";
        }

        public override void Use(GamePlayer user)
        {
            if (full)
            {
                user.health = (int)Math.Min(user.health + (user.maxHealth * 0.75), user.maxHealth);
                full = false;
                itemType = "Empty";
            }
        }
    }
}
