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
            // initialises all the values for the object
            full = true;

            itemType = "Empty";

            type = typeof(GamePotion);
            image = new GameImage();

            BoundingBox.Size = new Point(25, 25);

        }
        public override void LoadContent(ref ResourcePack resources)
        {
            // load image content
            // sets bounding box values

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


        // function called when player uses item
        public override void Use(GamePlayer user)
        {
            AudioManager.Instance.PlaySoundInstance(Resources.AudioPack["Potion_Drink"].CreateInstance(), "PotionDrink");
        }

        // operations that allow the strength of the potion to be changed
        public virtual void SetValue(float newValue)
        {

        }


        public virtual void MultiplyValue(float newScalar)
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // update the image position
            image.position = position.ToPoint();

            // draws the image based on whether the potion has been consumed yet
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
