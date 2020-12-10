using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MajorProject
{
    public class HUD
    {

        // woah! new class time(TM)

        // this will store a gameplayer entity and read the inventory off of it, and display the results

        // located at the bottom of the screen - covers 1/(4-5)th of it


        GamePlayer player;

        public HUD()
        {

        }




        void SetPlayer(GamePlayer p)
        {
            player = p;
        }

        void UnSetPlayer()
        {
            player = null;
        }

        public void Update()
        {

        }






    }
}