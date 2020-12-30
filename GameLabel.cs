using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace MajorProject
{
    class GameLabel
    {

        ResourcePack Resources;
        public string Text;
        public string FontName;
        public Vector2 Position;
        public Color FontColor;


        public GameLabel()
        {
            Position = new Vector2();
            Text = "";
            FontColor = Color.Blue;
        }

        public void LoadContent(ref ResourcePack resources)
        {
            Resources = resources;
        }

        public void SetPosition(int x, int y)
        {
            Position.X = x;
            Position.Y = y;
        }

        public void UnloadContent()
        {
            Resources = null;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Resources.FontPack[FontName], Text, Position, FontColor);
        }

    }
}
