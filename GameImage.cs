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
            // sets resources and texture names

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
            // alternate arguments for determining the length of each frame, not actually used, but nice to have
            frameLength = framelength;
            LoadContent(ref resources, texturenames);
        }

        public virtual void UnloadContent()
        {
            Resources = null;
        }

        public virtual void Update(GameTime gameTime)
        {
            // if a moving image
            if (!staticImage && animated && !completed)
            {
                // add time to the time to next frame
                frameTime += gameTime.ElapsedGameTime.TotalSeconds;
                if (frameTime > frameLength)
                {
                    // ifenough time has passed for next frame
                    textureIndex++;
                    if (textureIndex == textureNames.Length)
                    {
                        // frames have been looped through
                        if (NoLoop)
                        {
                            // the animation has completed. go back to final frame
                            completed = true;

                            textureIndex--;
                        }
                        else
                        {
                            // reset texture index
                            textureIndex = 0;
                        }
                    }

                    frameTime -= frameLength;
                }
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            // check that image is not transparent - don't bother drawing it otherwise!
            if (alpha > 0)
            {
                if (staticImage) textureIndex = 0;
                Point origin = new Point();

                // determine where image is drawn from bsaed on if it's centered or not
                if (centered)
                {
                    // origin is centre of image
                    origin.X = position.X - SpriteSize.X / 2;
                    origin.Y = position.Y - SpriteSize.Y / 2;
                }
                else
                {
                    // origin is top-left corner of image
                    origin = position;
                }

                // draw the image
                spriteBatch.Draw(Resources.TexturePack[textureNames[textureIndex]], destinationRectangle: new Rectangle(origin, SpriteSize), color: Color.White * alpha);

            }

        }

        // same as other Draw, just draw a specific frame
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
