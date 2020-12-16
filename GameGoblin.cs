using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajorProject
{
    public class GameGoblin : GameEnemy
    {

        GameImage goblinImage;
        GameImage goblinAttackImage;


        string[] walkAnimation =
        {
            "Goblin1",
            "Goblin2",
            "Goblin3"
        };




        public GameGoblin()
        {

            goblinImage = new GameImage();
            type = GetType();
        }

        public override void LoadContent(ref ResourcePack resources)
        {
            base.LoadContent(ref resources);

            goblinImage.LoadContent(ref Resources, walkAnimation);

            goblinImage.animated = true;
            goblinImage.centered = true;
            goblinImage.SpriteSize = new Point(25, 25);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);


            // goblins keep their distance from the player and fire slingshots


            if (target != null)
            {

            }








            goblinImage.position = position.ToPoint();
            goblinImage.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            goblinImage.Draw(spriteBatch);
        }
    }
}
