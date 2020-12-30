using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajorProject
{
    public class GameWeaponRifle : GameWeapon
    {

        public GameWeaponRifle()
        {
            attackCooldown = 1;
            itemType = "Rifle";
        }

    }
}
