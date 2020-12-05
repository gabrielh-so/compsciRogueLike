using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MajorProject
{
    public class GameFlyer : GameEnemy
    {

        GameImage flyerImage;
        GameImage flyerAttackImage;


        string[] walkAnimation =
        {
            "Flyer1",
            "Flyer2",
            "Flyer3"
        };




        public GameFlyer()
        {

            type = GetType();
        }

        public override void LoadContent(ref ResourcePack resources)
        {
            base.LoadContent(ref resources);

            flyerImage.LoadContent(ref Resources, walkAnimation);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            flyerImage.position = position.ToPoint();
            flyerImage.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            flyerImage.Draw(spriteBatch);
        }

    }
}
