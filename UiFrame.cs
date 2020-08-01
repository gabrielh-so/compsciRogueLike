using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MajorProject
{
    public class UiFrame
    {
        Rectangle Container;
        public bool IsActive;
        public bool IsDisplayed;
        public List<UiElement> uiElements;

        public UiFrame()
        {

        }

        public void LoadContent()
        {

            foreach (UiElement uie in uiElements)
                uie.LoadContent();
        }

        public void UnloadContent()
        {
            foreach (UiElement uie in uiElements)
            {
                uie.UnloadContent();
            }
        }

        public void Update(GameTime gameTime)
        {
            if (IsActive && Container.Contains(InputManager.Instance.GetMousePosition()))
                foreach (UiElement uie in uiElements)
                {
                    uie.Update(gameTime);
                }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsDisplayed)
                foreach (UiElement uie in uiElements)
                {
                    uie.Draw(spriteBatch);
                }
        }
    }
}
