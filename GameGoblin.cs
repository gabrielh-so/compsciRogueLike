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


        int targetDistance = 25;

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
            speed = 50;

            targetDistance = 250;

            goblinImage = new GameImage();
            type = GetType();

            maxHealth = 100;
            health = maxHealth;
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

            if (alive) {

                // goblins keep their distance from the player and fire slingshots


                if (target != null)
                {
                    // move towards player if too far away,
                    // move closer if too near

                    Vector2 newPosition = position;

                    Vector2 directionVector = (target.BoundingBox.Location - BoundingBox.Location).ToVector2();

                    // if goblin is too close, move away from player


                    // check if player is closer or nearer to target radius
                    if (Math.Pow(target.position.X - position.X, 2) + Math.Pow(target.position.Y - position.Y, 2) < targetDistance)
                    {
                        velocity = new Vector2(-1, -1);
                    }
                    else
                        velocity = new Vector2(1, 1);


                    // check magnitude is bigger than 0 before normallizing
                    if (directionVector.X * directionVector.X + directionVector.Y * directionVector.Y > 0)
                    {
                        directionVector.Normalize();
                    }

                    velocity.X *= (float)(directionVector.X * speed * (float)gameTime.ElapsedGameTime.TotalSeconds);
                    velocity.Y *= (float)(directionVector.Y * speed * (float)gameTime.ElapsedGameTime.TotalSeconds);



                    // check that the new location isn't in a wall or door

                    newPosition += velocity;

                    if (Map[(int)newPosition.Y / tileWidth, (int)newPosition.X / tileWidth] != (int)World.cellType.wall)
                    {
                        position = newPosition;

                        // update boxes
                        BoundingBox.Location = position.ToPoint();


                        goblinImage.position = position.ToPoint();
                    }


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

                    //position += velocity;
                }

                goblinImage.position = position.ToPoint();
                goblinImage.Update(gameTime);
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
