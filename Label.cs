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
    public class Label : UiElement
    {
        ContentManager content;
        SpriteFont font;
        public string FontName;
        public string Text;

        public Label()
        {
            Text = string.Empty;
        }

        public override void LoadContent()
        {
            content = new ContentManager(ScreenManager.Instance.Content.ServiceProvider, "Content");

            FontName = "Fonts/coders_crux";

            font = content.Load<SpriteFont>(FontName);

            base.LoadContent();
        }

        public override void UnloadContent()
        {
            content.Unload();
            content.Dispose();
            
            base.UnloadContent();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            ScreenManager.Instance.SpriteBatch.DrawString(font, Text, Position, Color.White);
            base.Draw(spriteBatch);
        }
    }
}
