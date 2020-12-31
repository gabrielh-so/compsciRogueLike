using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MajorProject
{
    public class GamePotion : GameItem
    {
        public bool full;

        protected GameImage image;

        public virtual void Refill()
        {
            full = true;
        }

        public GamePotion()
        {
            full = true;

            type = typeof(GamePotion);

            BoundingBox.Size = new Point(25, 25);

            radius = 25;

            image = new GameImage();
            
            image.animated = true;
            image.centered = true;
            image.SpriteSize = new Point(25, 25);

            image.position = position.ToPoint();
            BoundingBox.Location = position.ToPoint();

            removeable = false;

            itemType = "Empty";
        }
        public override void LoadContent(ref ResourcePack resources)
        {
            base.LoadContent(ref resources);

            string[] potionImages = new string[2]
            {
                itemType,
                "Empty"
            };

            image.LoadContent(ref resources, potionImages);
        }



        public override void Use(GamePlayer user)
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            image.position = position.ToPoint();

            if (full)
            {
                image.Draw(spriteBatch, 0);
            }
            else
            {
                image.Draw(spriteBatch, 1);
            }

        }


    }
}
