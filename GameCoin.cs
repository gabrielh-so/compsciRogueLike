using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajorProject
{
    public class GameCoin : GameItem
    {

        public GameImage image;

        // value of the coin
        public int value;

        static Random rand = new Random();

        // coin animation will be the same for all coins, so define it within the class body
        string[] CoinAnimation = new string[5]
        {
            "Coin1",
            "Coin2",
            "Coin3",
            "Coin4",
            "Coin5"

        };

        public GameCoin()
        {
            // like all items, it has a type object

            type = this.GetType();

        }

        public override void LoadContent(ref ResourcePack resources)
        {
            base.LoadContent(ref resources);

            // update bounding box information
            BoundingBox.Size = new Point(25, 25);

            // give a value
            value = 100;

            // give a value for the circular hitbox - 25 is standard
            radius = 25;

            image = new GameImage();

            // gives default image values
            image.animated = true;
            image.centered = true;
            image.SpriteSize = new Point(25, 25);

            image.position = position.ToPoint();
            BoundingBox.Location = position.ToPoint();

            removeable = false;

            image.LoadContent(ref resources, CoinAnimation);
        }

        public override void UnloadContent()
        {
            // unload images and unhook Resources reference from object

            base.UnloadContent();

            Resources = null;

            image.UnloadContent();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // just draw the image
            image.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            // update the image, and the base function
            base.Update(gameTime);

            image.position = position.ToPoint();
            image.Update(gameTime);
        }

        public override void Use(GamePlayer user)
        {
            // add value to user's money total, and add coin pickup effect to audio manager
            user.money += value;

            AudioManager.Instance.PlaySoundInstance(Resources.AudioPack["Coin_Pickup"].CreateInstance(), "CoimPickup" + rand.NextDouble().ToString());
        }


    }
}
