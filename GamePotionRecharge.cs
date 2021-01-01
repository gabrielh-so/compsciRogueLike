using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace MajorProject
{
    public class GamePotionRecharge : GamePotion
    {
        public GamePotionRecharge()
        {
            full = true;
            type = this.GetType();
            itemType = "Recharge";
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Refill()
        {
            full = true;
            itemType = "Recharge";
        }

        public override void Use(GamePlayer user)
        {
            if (full)
            {
                
                foreach (GameItem i in user.inventory.itemList)
                {
                    if (i != null)
                        if (i.GetType().BaseType == typeof(GamePotion))
                        {
                            if (i.GetType() != typeof(GamePotionRecharge))
                            {
                                ((GamePotion)i).Refill();
                            }
                        }
                }

                full = false;
                itemType = "Empty";
            }
        }

    }
}
