using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MajorProject
{
    public class GameImage
    {

        /// <summary>
        /// this is a lightweight image class - use it for showing ingame sprites.
        /// We cannot use the Image class because it's feature rich, but uses energy like a mammoth-flipper
        /// </summary>

        Texture2D texture;


        public GameImage()
        {

        }

        public virtual void LoadContent(Texture2D t)
        {
            texture = t;
        }

        public virtual void UnloadContent()
        {
            texture.Dispose();
        }

        public virtual void Update(GameTime gameTime)
        {

        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
