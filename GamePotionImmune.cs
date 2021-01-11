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
        float ImmuneTime;

        public GamePotionImmune()
        {
            full = true;
            type = this.GetType();
            itemType = "Immune";
            ImmuneTime = 7.5f;

            Description = "Immunity potion.\nRefillable consumable. A potion that\ntemporarily shields player from all damage.\nImmune time: " + ImmuneTime + "s.";
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
                user.currentHitDelay = user.maxHitDelay - ImmuneTime;

                full = false;
                itemType = "Empty";
            }
        }
    }
}
