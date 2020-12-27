using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajorProject
{
    public abstract class GameWeapon : GameItem
    {
        public string weaponType;

        public double attackCooldown;

        public GameWeapon()
        {

        }

        public override void Use(GamePlayer user)
        {
            user.AddAttackCooldown(attackCooldown);
        }

    }
}
