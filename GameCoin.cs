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

        public int value;

        static Random rand = new Random();

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

            type = this.GetType();

        }

        public override void LoadContent(ref ResourcePack resources)
        {
            base.LoadContent(ref resources);

            BoundingBox.Size = new Point(25, 25);

            value = 100;

            radius = 25;

            image = new GameImage();

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
            base.UnloadContent();

            Resources = null;

            image.UnloadContent();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            image.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            image.position = position.ToPoint();
            image.Update(gameTime);
        }

        public override void Use(GamePlayer user)
        {

            user.money += value;

            AudioManager.Instance.PlaySoundInstance(Resources.AudioPack["Coin_Pickup"].CreateInstance(), "CoimPickup" + rand.NextDouble().ToString());
        }


    }
}
