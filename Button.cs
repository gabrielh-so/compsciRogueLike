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
        /// <summary>
        /// all the different looks of buttons at different stages of being pressed
        /// images will be moved to active image to be drawn
        /// </summary>
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
            // different states of button will produce different outputs

            if (mouseIsHovering())
            {
                // mouse is hovering, so detect if mouse is pressed
                if (InputManager.Instance.MousePressed())
                {
                    // mouse is pressed
                    if (!wasClicked)
                    {
                        // mouse wasn't pressed before, so change the image
                        ActiveImage = PressedImage;
                        wasClicked = true;
                    }
                } else if (InputManager.Instance.MouseReleased())
                {
                    // mouse is released
                    if (wasClicked)
                    {
                        // mouse was clicked but now isn't, so trigger the activation delegate
                        OnActivate?.Invoke(this);
                        wasClicked = false;
                        wasHovered = false;
                    }
                }
                if (!wasHovered)
                {
                    // mouse wasn't hovered over before, so change the image
                    ActiveImage = HoverImage;
                    OnHover?.Invoke(this);
                }
                wasHovered = true;
            } else
            {
                // if mouse was on before, change the active image to the standard button
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
            // load all the images, and set the positions of those images
            // also shift the pressed button image down a bit to make it seem actually pressed

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
            // unload all the images

            StandardImage.UnloadContent();
            HoverImage.UnloadContent();
            PressedImage.UnloadContent();
            ActiveImage.UnloadContent();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // if there is an image to be drawn, draw the image
            if (ActiveImage != null)
                ActiveImage.Draw(spriteBatch);
        }
    }
}
