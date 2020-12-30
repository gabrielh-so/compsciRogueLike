using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajorProject
{
    public class GameWeaponSlingShot : GameWeapon
    {
        public GameWeaponSlingShot()
        {
            attackCooldown = 1;
            itemType = "Slingshot";
        }

    }
}
