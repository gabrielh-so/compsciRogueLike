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
        int Speed;

        public GamePotionSpeed()
        {
            // initialises the deafault values for the speed potion
            full = true;
            type = this.GetType();
            itemType = "Speed";

            Speed = 275;

            Description = "Speed potion.\nRefillable consumable. A potion that gives\nthe player a speed boost on use.\nSpeed Boost: " + Speed;

        }

        // overrides of base functions

        public override void SetValue(float newValue)
        {
            Speed = (int)newValue;
        }

        public override void MultiplyValue(float newScalar)
        {
            Speed = (int)(Speed * newScalar);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Refill()
        {
            // change the name of the potion and toggle the empty
            full = true;
            itemType = "Speed";
        }

        public override void Use(GamePlayer user)
        {
            if (full)
            {
                user.Boost(7.5, Speed);
                full = false;
                itemType = "Empty";
                base.Use(user);
            }
        }
    }
}
