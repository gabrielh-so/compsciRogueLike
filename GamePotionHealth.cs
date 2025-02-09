﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace MajorProject
{
    public class GamePotionHealth : GamePotion
    {
        float healFraction;

        public GamePotionHealth()
        {
            // initialises the deafault values for the health potion

            full = true;
            type = this.GetType();
            itemType = "Health";
            healFraction = 0.75f;


            Description = "Health potion.\nRefillable consumable. A potion that refills\na portion of your health bar.\nHeals: " + healFraction * 100 + "% of player health.";
        }

        public override void Update(GameTime gameTime)
        {
            // update the base potion class
            base.Update(gameTime);
        }

        public override void Refill()
        {
            // change the name of the potion and toggle the empty
            full = true;
            itemType = "Health";
        }

        // overrides of base functions
        public override void SetValue(float newValue)
        {
            healFraction = newValue;
        }
        public override void MultiplyValue(float newScalar)
        {
            healFraction *= newScalar;
        }

        public override void Use(GamePlayer user)
        {
            if (full)
            {
                user.health = (int)Math.Min(user.health + (user.maxHealth * healFraction), user.maxHealth);
                full = false;
                itemType = "Empty";
                base.Use(user);
            }
        }
    }
}
