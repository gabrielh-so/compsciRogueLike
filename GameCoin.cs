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
            base.LoadContent(ref Resources);

            type = typeof(GameCoin);

            BoundingBox.Size = new Point(25, 25);

            value = 100;

            radius = 25;

            image = new GameImage();

            image.LoadContent(ref Resources, CoinAnimation);
            image.animated = true;
            image.centered = true;
            image.SpriteSize = new Point(25, 25);

            image.position = position.ToPoint();
            BoundingBox.Location = position.ToPoint();

            removeable = false;

        }

        public override void LoadContent(ref ResourcePack resources)
        {
            base.LoadContent(ref resources);

            image.LoadContent(ref resources, CoinAnimation);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();

            image.UnloadContent();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            image.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            // account for friction - allows launched coins to stop at some point
            if (velocity.LengthSquared() > 0) velocity *= 0.8f;

            position += velocity;
            BoundingBox.Location = position.ToPoint();
            BoundingBox.X -= BoundingBox.Width / 2;
            BoundingBox.Y -= BoundingBox.Height / 2;

            image.position = position.ToPoint();
            image.Update(gameTime);
        }

        public override void Use(GamePlayer user)
        {

            user.money += value;
        }


    }
}
