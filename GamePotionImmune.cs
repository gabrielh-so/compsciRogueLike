using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace MajorProject
{
    public class GamePotionImmune : GamePotion
    {

        public GamePotionImmune()
        {
            full = true;
            type = this.GetType();
            itemType = "Immune";
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Refill()
        {
            full = true;
            itemType = "Immune";
        }

        public override void Use(GamePlayer user)
        {
            if (full)
            {
                // gives 7.5 seconds of damage immunit
                user.hitCooldown = true;
                user.currentHitDelay = user.maxHitDelay - 7.5;

                full = false;
                itemType = "Empty";
            }
        }
    }
}
