using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MajorProject
{
    public class Button : UiElement
    {
        
        public Image StandardImage;
        public Image PressedImage;
        public Image HoverImage;

        public Image ActiveImage;
        public Rectangle Container;

        bool wasHovered;
        bool wasClicked;


        public bool mouseIsHovering()
        {

            return Container.Contains(InputManager.Instance.GetMousePosition());
        }

        public override void Update(GameTime gameTime)
        {
            if (mouseIsHovering())
            {
                if (InputManager.Instance.MousePressed())
                {
                    if (!wasClicked)
                    {
                        ActiveImage = PressedImage;
                        wasClicked = true;
                    }
                } else if (InputManager.Instance.MouseReleased())
                {
                    if (wasClicked)
                    {
                        OnActivate?.Invoke(this);
                        wasClicked = false;
                        wasHovered = false;
                    }
                }
                if (!wasHovered)
                {
                    ActiveImage = HoverImage;
                    OnHover?.Invoke(this);
                }
                wasHovered = true;
            } else
            {
                if (wasHovered || ActiveImage == null)
                {
                    ActiveImage = StandardImage;
                    wasHovered = false;
                    wasClicked = false;
                    OnStopHover?.Invoke(this);
                }
            }
        }

        public Button()
        {

        }

        public bool PointIntersects(Point p)
        {
            return Container.Contains(p);
        }

        public override void LoadContent()
        {
            StandardImage.LoadContent();
            HoverImage.LoadContent();
            PressedImage.LoadContent();

            StandardImage.Position = Container.Location.ToVector2();
            HoverImage.Position = Container.Location.ToVector2();
            PressedImage.Position = Container.Location.ToVector2();

            PressedImage.Position.Y += 5;

            Container.Width = StandardImage.Texture.Width;
            Container.Height = StandardImage.Texture.Height;

            Position = StandardImage.Position;
        }

        public override void UnloadContent()
        {
            StandardImage.UnloadContent();
            HoverImage.UnloadContent();
            PressedImage.UnloadContent();
            ActiveImage.UnloadContent();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (ActiveImage != null) ActiveImage.Draw(spriteBatch);
        }
    }
}
