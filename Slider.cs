using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace MajorProject
{
    public class Slider : UiElement
    {
        public Image baseImage;
        public Image sliderImage;

        public float sliderPosition;

        public Rectangle MainContainer;
        public Rectangle SliderContainer;

        bool wasClicked;

        public Slider()
        {

        }

        // returns true if mouse is hovering over any of the slider
        public bool mouseIsHovering()
        {
            return MainContainer.Contains(InputManager.Instance.GetMousePosition());
        }

        // returns true if mouse is hovering over the slider part of the slider
        public bool mouseIsHoveringSlider()
        {
            return SliderContainer.Contains(InputManager.Instance.GetMousePosition());
        }

        // same as before, but just from some generic point
        public bool PointIntersects(Point p)
        {
            return MainContainer.Contains(p);
        }

        // same as above
        public bool PointIntersectsSlider(Point p)
        {
            return SliderContainer.Contains(p);
        }

        public override void LoadContent()
        {
            // starts the positions
            sliderImage.LoadContent();
            baseImage.LoadContent();
            baseImage.Position = Position;
            MainContainer = new Rectangle(Position.ToPoint(), baseImage.Texture.Bounds.Size);
            SliderContainer.Size = new Point(sliderImage.Texture.Width, sliderImage.Texture.Height);
            SetSliderPosition(sliderPosition, true);
        }

        public override void UnloadContent()
        {
            baseImage.UnloadContent();
            sliderImage.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            baseImage.Update(gameTime);
            sliderImage.Update(gameTime);

            // determines if the slider has been selected
            if (mouseIsHoveringSlider())
            {
                if (InputManager.Instance.MousePressed())
                {
                    if (!wasClicked)
                    {
                        wasClicked = true;
                    }
                }
            }

            if (InputManager.Instance.MouseReleased())
            {
                wasClicked = false;
            }

            // moves with the mouse (x axis) if clicked and the mouse has been moved
            if (wasClicked && InputManager.Instance.MouseMoved())
            {
                Point mousePos = InputManager.Instance.GetMousePosition();
                float halfTexture = sliderImage.Texture.Width / 2;
                SetSliderPosition(map(mousePos.X - halfTexture, Position.X - halfTexture, Position.X + baseImage.Texture.Width - halfTexture, 0, 1));
            }
            /*
            if (sliderPosition == 0)
            {
                bool u = true;
            }
            if (sliderPosition == 1)
            {
                bool u = true;
            }
            */
        }

        public void SetSliderPosition(float amount, bool setup)
        {
            // updates the position of the slider to whatever amount it is
            if (setup) sliderPosition -= 1.5f;
            amount = Math.Max(0, Math.Min(1, amount));
            if (amount == sliderPosition) return;
            sliderImage.Position = Position;
            sliderImage.Position.X -= sliderImage.Texture.Width / 2; // move so that the cursor is on the start of the line instead of the edge being at the start
            sliderImage.Position.Y -= sliderImage.Texture.Height; // move so that cursor points on line instead of below ___^___ or something
            sliderImage.Position.X += baseImage.Texture.Width * amount; // move the cursor along the line by the specified amount
            SliderContainer.Location = sliderImage.Position.ToPoint(); // move the hitbox along with it
            sliderPosition = amount; // update the stated amount
            if (!setup)
                OnActivateF(this, amount);
        }

        // generic slider function, sets the setup bool to false
        public void SetSliderPosition(float amount)
        {
            SetSliderPosition(amount, false);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            baseImage.Draw(spriteBatch);
            sliderImage.Draw(spriteBatch);
        }

        // classical map function - learnt it in physics

        private static float map(float value, float fromLow, float fromHigh, float toLow, float toHigh)
        {
            return (value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
        }



    }
}
