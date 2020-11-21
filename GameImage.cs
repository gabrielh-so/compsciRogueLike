using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MajorProject
{
    public class GameImage
    {

        /// <summary>
        /// this is a lightweight image class - use it for showing ingame sprites.
        /// We cannot use the Image class because it's feature rich, but uses energy (memory (not sure what i was thinking)) like a mammoth-flipper
        /// </summary>

        ResourcePack Resources;
        
        public bool staticImage; // can the image animate

        string[] textureNames;

        public bool resetAnimation;
        public bool animated; // is the image currently animating?
        public bool blinking; // is the image blinking in and out?

        public double frameLength; // total length of time per frame (seconds)
        public double frameTime; // time since last frame change

        public bool centered;

        public GameImage()
        {

        }

        public virtual void LoadContent(ref ResourcePack resources, string[] texturenames)
        {
            texturenames.CopyTo(textureNames, 0);
            if (texturenames.Length > 1)
            {
                animated = true;
                frameLength = 100;
                frameTime = 0;
            }
            else staticImage = true;
        }
        public virtual void LoadContent(ref ResourcePack resources, int framelength, string[] texturenames)
        {
            frameLength = framelength;
            LoadContent(ref resources, texturenames);
        }

        public virtual void UnloadContent()
        {
            Resources = null;
        }

        public virtual void Update(GameTime gameTime)
        {
            if (!staticImage & animated)
            {
                frameTime += gameTime.ElapsedGameTime.TotalSeconds;
                if (frameTime > frameLength)
                {

                }
            }
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
