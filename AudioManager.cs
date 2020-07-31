using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajorProject
{
    public class AudioManager
    {

        private static AudioManager instance;

        public static AudioManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new AudioManager();

                return instance;
            }
        }



        public AudioManager()
        {

        }



    }
}
