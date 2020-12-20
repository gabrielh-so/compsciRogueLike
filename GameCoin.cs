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
            image = new GameImage();
            value = 100;

            radius = 25;

            image = new GameImage();

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
            image.Update(gameTime);
        }


    }
}
