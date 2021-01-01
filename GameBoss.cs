using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MajorProject
{
    public class GameBoss : GameEnemy
    {
        Random rand;

        GameImage bossImage;


        int targetDistance = 25;

        double maxFireInterval = 2;
        double currentFireInterval = 0;

        int ProjectilesPerCharge;



        string[] bossAnimation =
        {
            "Boss1",
            "BossCharge"
        };




        public GameBoss()
        {

            bossImage = new GameImage();
            type = GetType();
        }

        public override void LoadContent(ref ResourcePack resources)
        {
            base.LoadContent(ref resources);


            rand = new Random();

            speed = 50;

            targetDistance = 250;

            BoundingBox.Size = new Point(150, 150);
            bossImage.position = position.ToPoint();
            BoundingBox.Location = (position - BoundingBox.Size.ToVector2() / 2).ToPoint();

            maxHealth = 1500;
            health = maxHealth;

            ProjectilesPerCharge = 15;

            bossImage.LoadContent(ref Resources, bossAnimation);

            bossImage.animated = false;
            bossImage.centered = true;
            bossImage.SpriteSize = new Point(150, 150);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (alive)
            {

                // goblins keep their distance from the player and fire slingshots


                if (target != null)
                {

                    /*
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

                        // update hitbox
                        BoundingBox.Location = position.ToPoint();
                    }
                    */


                    currentFireInterval += gameTime.ElapsedGameTime.TotalSeconds;
                    if (currentFireInterval >= maxFireInterval)
                    {
                        double interpolation = (Math.PI * 2) / ProjectilesPerCharge;
                        double offset = rand.NextDouble() * Math.PI;
                        for (double i = Math.PI * 2; i >= 0; i -= interpolation)
                        {
                            GameProjectile p = new GameProjectile();
                            p.position = new Vector2(position.X, position.Y);
                            p.target = typeof(GamePlayer);
                            p.totalLifeSpan = 3;
                            p.velocity = new Vector2((float)Math.Sin(i + offset), (float)Math.Cos(i + offset));
                            p.speed = 150;
                            p.damageType = GameProjectile.DamageType.Fire;
                            p.damage = 5;

                            p.BoundingBox.Location = position.ToPoint();
                            p.BoundingBox.Size = new Point(25, 25);


                            // now *that's* punk
                            ((GameScreen)ScreenManager.Instance.currentScreen).AddProjectile(p);
                        }


                        currentFireInterval = 0;


                    }

                    //position += velocity;
                }

            }








            bossImage.position = position.ToPoint();
            bossImage.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            bossImage.Draw(spriteBatch);
        }

    }
}
