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

        // the information that the label needs in order to be drawn
        // this class is as light as possible
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

        // takes in coordinate values and updates the position vector
        public void SetPosition(int x, int y)
        {
            Position.X = x;
            Position.Y = y;
        }

        // unhooks the resource reference
        public void UnloadContent()
        {
            Resources = null;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // actually calls the function to draw the label text
            spriteBatch.DrawString(Resources.FontPack[FontName + "_" + PlayerPreferences.Instance.fontSize.ToString()], Text, Position, FontColor);
        }

    }
}
