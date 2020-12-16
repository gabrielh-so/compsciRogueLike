using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajorProject
{
    public class GameInventory
    {

        List<GameAbility> abilities;
        List<GameWeapon> weapons;
        List<GamePotion> potions;
        



        public GameInventory()
        {
            abilities = new List<GameAbility>();
            weapons = new List<GameWeapon>();
            potions = new List<GamePotion>();
        }

    }
}
