using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MajorProject
{
    public class GameSlime : GameEnemy
    {

        GameImage slimeImage;
        GameImage slimeAttackImage;


        string[] walkAnimation =
        {
            "Slime1",
            "Slime2",
            "Slime3"
        };




        public GameSlime()
        {

            slimeImage = new GameImage();
            type = GetType();
        }

        public override void LoadContent(ref ResourcePack resources)
        {
            base.LoadContent(ref resources);

            slimeImage.LoadContent(ref Resources, walkAnimation);
            slimeImage.animated = true;
            slimeImage.centered = true;
            slimeImage.SpriteSize = new Point(25, 25);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            slimeImage.position = position.ToPoint();
            slimeImage.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            slimeImage.Draw(spriteBatch);
        }

    }
}
