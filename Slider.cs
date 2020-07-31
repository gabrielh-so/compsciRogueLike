using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
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
        float texWidth;

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

            sliderImage.Position = Position;
            baseImage.Position = Position;

            baseImage.LoadContent();
            sliderImage.LoadContent();
            texWidth = baseImage.Texture.Width/2;

            MainContainer.Width = baseImage.Texture.Width;
            MainContainer.Height = baseImage.Texture.Height;
            sliderImage.Position.Y -= sliderImage.Texture.Height;
            SliderContainer.Width = sliderImage.Texture.Width;
            SliderContainer.Height = sliderImage.Texture.Height;
            sliderImage.Position.X += (float)(sliderPosition-0.5) * baseImage.Texture.Width + texWidth;
            SliderContainer.Location += sliderImage.Position.ToPoint();
            MainContainer.Location += baseImage.Position.ToPoint();
            bool b = true;
        }

        public override void UnloadContent()
        {
            baseImage.UnloadContent();
            sliderImage.UnloadContent();
        }

        public virtual void Update(GameTime gameTime)
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


            if (InputManager.Instance.MouseReleased())
            {
                wasClicked = false;
            }
            if (wasClicked && InputManager.Instance.MouseMoved())
            {
                Point mousePos = InputManager.Instance.GetMousePosition();
                sliderImage.Position = new Vector2((float)Math.Min(baseImage.Texture.Width + Position.X, Math.Max(Position.X - sliderImage.Texture.Width / 2, mousePos.X-sliderImage.Texture.Width/2)), sliderImage.Position.Y);
                sliderPosition = map(sliderImage.Position.X + sliderImage.Texture.Width / 2, Position.X, baseImage.Texture.Width + Position.X, 0, 1);
                SliderContainer.X = (int)sliderImage.Position.X;
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
