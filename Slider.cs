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
        public Vector2 Position;
        public Image baseImage;
        public Image sliderImage;

        public float sliderPosition;

        public Rectangle MainContainer;
        public Rectangle SliderContainer;

        bool wasClicked;

        public Slider()
        {

        }

        public bool mouseIsHovering()
        {
            return MainContainer.Contains(InputManager.Instance.GetMousePosition());
        }
        public bool mouseIsHoveringSlider()
        {
            return SliderContainer.Contains(InputManager.Instance.GetMousePosition());
        }

        public bool PointIntersects(Point p)
        {
            return MainContainer.Contains(p);
        }

        public bool PointIntersectsSlider(Point p)
        {
            return SliderContainer.Contains(p);
        }

        public override void LoadContent()
        {
            sliderImage.LoadContent();
            baseImage.LoadContent();
            baseImage.Position = Position;
            MainContainer = new Rectangle(Position.ToPoint(), baseImage.Texture.Bounds.Size);
            SliderContainer.Size = new Point(sliderImage.Texture.Width, sliderImage.Texture.Height);
            SetSliderPosition(sliderPosition);
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

            if (InputManager.Instance.KeyPressed(Keys.F))
                SetSliderPosition(0.5f);
            if (InputManager.Instance.KeyPressed(Keys.D))
                SetSliderPosition(0.25f);
            if (InputManager.Instance.KeyPressed(Keys.G))
                SetSliderPosition(0.75f);

            if (InputManager.Instance.MouseReleased())
            {
                wasClicked = false;
            }
            if (wasClicked && InputManager.Instance.MouseMoved())
            {
                Point mousePos = InputManager.Instance.GetMousePosition();
                float halfTexture = sliderImage.Texture.Width / 2;
                SetSliderPosition(map(mousePos.X - halfTexture, Position.X - halfTexture, Position.X + baseImage.Texture.Width - halfTexture, 0, 1));
            }
            if (sliderPosition == 0)
            {
                bool u = true;
            }
            if (sliderPosition == 1)
            {
                bool u = true;
            }
        }

        public void SetSliderPosition(float amount)
        {
            amount = Math.Max(0, Math.Min(1, amount));
            sliderImage.Position = Position;
            sliderImage.Position.X -= sliderImage.Texture.Width / 2; // move so that the cursor is on the start of the line instead of the edge being at the start
            sliderImage.Position.Y -= sliderImage.Texture.Height; // move so that cursor points on line instead of below ___^___ or something
            sliderImage.Position.X += baseImage.Texture.Width * amount;
            SliderContainer.Location = sliderImage.Position.ToPoint();
            sliderPosition = amount;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            baseImage.Draw(spriteBatch);
            sliderImage.Draw(spriteBatch);
        }
        private static float map(float value, float fromLow, float fromHigh, float toLow, float toHigh)
        {
            return (value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
        }



    }
}
