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
            type = GetType();
        }

        public override void LoadContent(ref ResourcePack resources)
        {
            base.LoadContent(ref resources);

            goblinImage.LoadContent(ref Resources, walkAnimation);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

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
