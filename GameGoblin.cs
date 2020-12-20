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




        double maxFireInterval = 2;
        double currentFireInterval = 0;




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
                currentFireInterval += gameTime.ElapsedGameTime.TotalSeconds;
                if (currentFireInterval >= maxFireInterval)
                {
                    GameProjectile p = new GameProjectile();
                    p.position = new Vector2(position.X, position.Y);
                    p.target = typeof(GamePlayer);
                    p.totalLifeSpan = 3;
                    p.SetVelocity(target.position - position);
                    p.speed = 2;
                    p.damageType = GameProjectile.DamageType.Fire;
                    p.damage = 5;

                    p.BoundingBox.Location = position.ToPoint();
                    p.BoundingBox.Size = new Point(25, 25);

                    projectiles.Add(p);

                    // now *that's* punk
                    ((GameScreen)ScreenManager.Instance.currentScreen).AddProjectile(p);


                    currentFireInterval = 0;


                }
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
