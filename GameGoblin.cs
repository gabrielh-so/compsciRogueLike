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
        GameImage goblinDeadImage;


        int targetDistance = 25;

        double maxFireInterval = 2;
        double currentFireInterval = 0;




        string[] walkAnimation =
        {
            "Goblin1",
            "Goblin2",
            "Goblin3"
        };

        string[] deathAnimation =
        {
            "Dead"
        };




        public GameGoblin()
        {

            goblinImage = new GameImage();
            goblinDeadImage = new GameImage();
            type = GetType();

        }

        public override void LoadContent(ref ResourcePack resources)
        {
            base.LoadContent(ref resources);

            if (!alive)
                WasAlive = false;

            speed = 50;

            targetDistance = 250;
            maxHealth = 100;
            health = maxHealth;

            goblinImage.LoadContent(ref Resources, walkAnimation);

            goblinImage.animated = true;
            goblinImage.centered = true;
            goblinImage.SpriteSize = new Point(25, 25);

            goblinDeadImage.LoadContent(ref Resources, deathAnimation);

            goblinDeadImage.animated = true;
            goblinDeadImage.centered = true;
            goblinDeadImage.SpriteSize = new Point(25, 25);
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

                    // goblins can move to spaces unchecked by the player (like being backed up into a wall, so checks need to be made for this entity too!)

                    goblinImage.animated = true;

                    Vector2 newPosition = position;

                    Vector2 directionVector = (target.BoundingBox.Location - BoundingBox.Location).ToVector2();

                    // if goblin is too close, move away from player


                    // check if player is closer or nearer to target radius
                    if (Math.Pow(target.position.X - position.X, 2) + Math.Pow(target.position.Y - position.Y, 2) < targetDistance * targetDistance)
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

                    // switch statement is so so so much more readable my god
                    switch (Map[(int)newPosition.Y / tileWidth, (int)newPosition.X / tileWidth])
                    {
                        case 2: // wall
                        case 3: // door
                            // do nothing because they must not leave the room bounds
                            break;
                        default:
                            position = newPosition;

                            // update boxes
                            BoundingBox.Location = position.ToPoint();

                            // update images
                            goblinImage.position = position.ToPoint();
                            goblinDeadImage.position = position.ToPoint();
                            break;
                    }


                    // check if it is time to fire yet
                    currentFireInterval += gameTime.ElapsedGameTime.TotalSeconds;
                    if (currentFireInterval >= maxFireInterval)
                    {
                        // if so, fire
                        GameProjectile p = new GameProjectile();
                        p.position = new Vector2(position.X, position.Y);
                        p.target = typeof(GamePlayer);
                        p.totalLifeSpan = 3;
                        p.SetVelocity(target.position - position);
                        p.speed = 200;
                        p.damageType = GameProjectile.DamageType.Fire;
                        p.damage = 5;

                        p.BoundingBox.Location = position.ToPoint();
                        p.BoundingBox.Size = new Point(25, 25);


                        // now *that's* punk
                        ((GameScreen)ScreenManager.Instance.currentScreen).AddProjectile(p);


                        currentFireInterval = 0;


                    }

                    //position += velocity;
                }
                else
                {
                    // goblin isn't moving any more, so show standing stil image
                    goblinImage.RestartAnimation();
                    goblinImage.animated = false;
                }

            }

            // update positions and images
            goblinImage.position = position.ToPoint();
            goblinDeadImage.position = position.ToPoint();
            if (alive)
                goblinImage.Update(gameTime);
            else
                goblinDeadImage.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            // if alive, draw normal goblin
            // else, draw dead image
            if (alive)
                goblinImage.Draw(spriteBatch);
            else goblinDeadImage.Draw(spriteBatch);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();

            // unload image data
            goblinImage.UnloadContent();
            goblinDeadImage.UnloadContent();
        }

        public override void OnDeath()
        {
            // play death sound from resource pack
            AudioManager.Instance.PlaySoundInstance(Resources.AudioPack["Goblin_Death"].CreateInstance(), "GoblinDeath" + rand.NextDouble().ToString());
        }
    }
}
