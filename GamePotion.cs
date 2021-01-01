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

            itemType = "Empty";

            type = typeof(GamePotion);
            image = new GameImage();

            BoundingBox.Size = new Point(25, 25);

        }
        public override void LoadContent(ref ResourcePack resources)
        {
            base.LoadContent(ref resources);

            radius = 25;


            image.animated = true;
            image.centered = true;
            image.SpriteSize = new Point(25, 25);

            image.position = position.ToPoint();
            BoundingBox.Location = position.ToPoint();

            removeable = false;

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
