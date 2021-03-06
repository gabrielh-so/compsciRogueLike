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
        //Random rand;

        // all the different images that are

        GameImage bossImage;
        GameImage bossChargeImage;
        GameImage bossDeadImage;


        // values that determine when to fire
        double maxFireInterval = 1;
        double currentFireInterval = 0;

        // how many projctiles are fired each charge
        int ProjectilesPerCharge;


        // string arrays for each image

        string[] bossAnimation =
        {
            "Boss1"
        };

        string[] chargeAnimation =
        {
            "BossCharge"
        };

        string[] deadAnimation =
        {
            "Dead"
        };




        public GameBoss()
        {
            // initialise all the image objects
            bossImage = new GameImage();
            bossChargeImage = new GameImage();
            bossDeadImage = new GameImage();
            type = GetType();
        }

        public override void LoadContent(ref ResourcePack resources)
        {


            base.LoadContent(ref resources);

            // prevents already dead enemies from becoming alive on reload
            if (!alive)
                WasAlive = false;

            rand = new Random();

            // not actually used - this enemy doesn't move
            speed = 50;

            // sets bounding/position information
            BoundingBox.Size = new Point(150, 150);
            bossImage.position = position.ToPoint();
            BoundingBox.Location = (position - BoundingBox.Size.ToVector2() / 2).ToPoint();

            bossDeadImage.position = position.ToPoint();

            // set health values (overrwitten by GameWorld.LoadRoomContents)
            maxHealth = 1500;
            health = maxHealth;

            ProjectilesPerCharge = 15;

            // loads and sets values for all images
            bossImage.LoadContent(ref Resources, bossAnimation);

            bossImage.animated = false;
            bossImage.centered = true;
            bossImage.SpriteSize = new Point(150, 150);

            bossDeadImage.LoadContent(ref Resources, deadAnimation);

            bossDeadImage.animated = false;
            bossDeadImage.centered = true;
            bossDeadImage.SpriteSize = new Point(150, 150);
            bossDeadImage.alpha = 0.5f;


            bossChargeImage.position = position.ToPoint();
            bossChargeImage.LoadContent(ref Resources, chargeAnimation);

            bossChargeImage.animated = false;
            bossChargeImage.centered = true;
            bossChargeImage.SpriteSize = new Point(150, 150);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // only need to update if alive
            if (alive)
            {

                // goblins keep their distance from the player and fire slingshots


                if (target != null)
                {

                    // character doesn't move, so don't need movement code

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

                    // updates

                    currentFireInterval += gameTime.ElapsedGameTime.TotalSeconds;
                    if (currentFireInterval >= maxFireInterval)
                    {

                        // generate projectiles, assign velocity based on angle of rotation

                        // finds the change in angle per projectile for even spread
                        double interpolation = (Math.PI * 2) / ProjectilesPerCharge;

                        // randomsies the original offset for angles, so player can't hide in one place indefinetly
                        double offset = rand.NextDouble() * Math.PI;
                        for (double i = Math.PI * 2; i >= 0; i -= interpolation)
                        {

                            // creates orojectile object and assigns values
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

                        // resets count until next firing

                        currentFireInterval = 0;


                    }

                    //position += velocity;
                }

            }

            // updates bounding box/image position
            BoundingBox.Location = (position - BoundingBox.Size.ToVector2() / 2).ToPoint();
            bossImage.position = position.ToPoint();

            // if alive, update the image
            if (alive)
                bossImage.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            // if alive, draw the character based on their status
            // otherwise, draw their grave
            if (alive)
            {
                if (maxFireInterval - currentFireInterval < 0.5)
                {
                    bossChargeImage.Draw(spriteBatch);
                }
                else bossImage.Draw(spriteBatch);
            }
            else bossDeadImage.Draw(spriteBatch);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();

            // unload image content

            bossImage.UnloadContent();
            bossChargeImage.UnloadContent();
            bossDeadImage.UnloadContent();
        }

        public override void OnDeath()
        {
            // creates a soundinstance, and generates random ID to prevent collisions in dictionary
            AudioManager.Instance.PlaySoundInstance(Resources.AudioPack["Boss_Death"].CreateInstance(), "Boss_Death" + rand.NextDouble().ToString());
        }

    }
}
