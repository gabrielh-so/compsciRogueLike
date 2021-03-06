using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace MajorProject
{
    public class FadeEffect : ImageEffect
    {
        // all the values
        public float FadeSpeed;
        public bool Increase;
        public bool Loop;
        public float StartingOpacity;

        public FadeEffect()
        {
            FadeSpeed = 1;
            Increase = false;
        }

        public override void LoadContent(ref Image Image)
        {
            base.LoadContent(ref Image);
            image.Alpha = StartingOpacity;
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // if image is active, update it
            if (image.IsActive && IsActive)
            {
                // change image alpha value based on direction of change
                if (!Increase)
                    image.Alpha -= FadeSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                else
                    image.Alpha += FadeSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                // hit lowest alpha, increase and stop if not looping
                if (image.Alpha < 0.0f)
                {
                    Increase = true;
                    image.Alpha = 0.0f;
                    if (!Loop) IsActive = false;
                }
                // hit highest alpha, decrease and stop if not looping
                else if (image.Alpha > 1.0f)
                {
                    Increase = false;
                    image.Alpha = 1.0f;
                    if (!Loop) IsActive = false;
                }
            }
            else
                // otherwise, give it full opacity
                image.Alpha = 1.0f;
        }
    }
}
