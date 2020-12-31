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

        public bool NoLoop;
        public bool completed; // is the animation completed?

        public double frameLength; // total length of time per frame (seconds)
        public double frameTime; // time since last frame change

        public bool centered;

        public int textureIndex;

        public Point position;
        public Point SpriteSize;

        public float alpha;


        void MoveOrigin(Point newPosition)
        {
            position = newPosition;
        }

        public void RestartAnimation()
        {
            textureIndex = 0;
            frameTime = 0;
        }


        public GameImage()
        {
            NoLoop = false;
            position = new Point();
            alpha = 1;
        }

        public virtual void LoadContent(ref ResourcePack resources, string[] texturenames)
        {
            Resources = resources;
            textureNames = texturenames;
            if (texturenames.Length > 1)
            {
                animated = true;
                frameLength = 0.2;
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
            if (!staticImage && animated && !completed)
            {
                frameTime += gameTime.ElapsedGameTime.TotalSeconds;
                if (frameTime > frameLength)
                {
                    textureIndex++;
                    if (textureIndex == textureNames.Length)
                    {
                        if (NoLoop)
                        {
                            completed = true;

                            textureIndex--;
                        }
                        else
                        {
                            textureIndex = 0;
                        }
                    }
                    frameTime -= frameLength;
                }
            }
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (alpha > 0)
            {
                if (staticImage) textureIndex = 0;
                Point origin = new Point();
                if (centered)
                {
                    origin.X = position.X - SpriteSize.X / 2;
                    origin.Y = position.Y - SpriteSize.Y / 2;
                }
                else
                {
                    origin = position;
                }

                spriteBatch.Draw(Resources.TexturePack[textureNames[textureIndex]], destinationRectangle: new Rectangle(origin, SpriteSize), color: Color.White * alpha);

            }

        }
        public virtual void Draw(SpriteBatch spriteBatch, int textureIndex)
        {
            if (alpha > 0)
            {
                Point origin = new Point();
                if (centered)
                {
                    origin.X = position.X - SpriteSize.X / 2;
                    origin.Y = position.Y - SpriteSize.Y / 2;
                }
                else
                {
                    origin = position;
                }

                spriteBatch.Draw(Resources.TexturePack[textureNames[textureIndex]], destinationRectangle: new Rectangle(origin, SpriteSize), color: Color.White * alpha);

            }

        }
    }
}
