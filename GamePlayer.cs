using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

using System.Runtime.Serialization.Formatters.Binary;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

// shorthand for the actiontype
using static MajorProject.InputManager;


namespace MajorProject
{
    [Serializable]
    public class GamePlayer : GameCharacter
    {
        public GameInventory inventory;

        GameImage playerImage;

        public int money;

        public bool hitCooldown;
        public double maxHitDelay;
        public double currentHitDelay;

        public bool attackCooldown;
        public double currentAttackDelay;
        public double maxAttackDelay;

        double BaseSpeed;
        bool boostCooldown;
        double maxBoostDelay;
        double currentBoostDelay;

        public GamePlayer()
        {
            inventory = new GameInventory();
            playerImage = new GameImage();
            money = 0;
            BaseSpeed = 200;
            speed = BaseSpeed;
            maxHitDelay = 1;
            currentHitDelay = 0;
        }

        string[] walkAnimation =
        {
            "Player_Forward"
        };

        public override void ProjectileCollision(GameProjectile p)
        {
            if (!hitCooldown)
                TakeDamage(p.damage);
        }

        public override void LoadContent(ref ResourcePack resources)
        {
            base.LoadContent(ref resources);

            inventory.LoadContent();

            playerImage.LoadContent(ref Resources, walkAnimation);
            playerImage.animated = false;
            playerImage.centered = true;
            playerImage.SpriteSize = new Point(25, 25);

            BoundingBox.Size = SpriteSize;
        }

        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);

            if (boostCooldown)
            {
                currentBoostDelay += gameTime.ElapsedGameTime.TotalSeconds;
                if (currentBoostDelay >= maxBoostDelay)
                {
                    boostCooldown = false;
                    currentBoostDelay = 0;

                    speed = BaseSpeed;
                }
            }

            if (hitCooldown)
            {
                currentHitDelay += gameTime.ElapsedGameTime.TotalSeconds;
                if (currentHitDelay >= maxHitDelay)
                {
                    hitCooldown = false;
                    currentHitDelay = 0;
                }
            }

            if (attackCooldown)
            {
                currentAttackDelay += gameTime.ElapsedGameTime.TotalSeconds;
                if (currentAttackDelay >= maxAttackDelay)
                {
                    attackCooldown = false;
                    currentAttackDelay = 0;
                }
            }


            velocity = Vector2.Zero;

            Vector2 newPosition = position;

            if (InputManager.Instance.ActionKeyDown(ActionType.walk_right))
            {
                velocity.X += 1;
                //recordValues(playerlocation.X.ToString(), playerlocation.Y.ToString());
            }
            if (InputManager.Instance.ActionKeyDown(ActionType.walk_left))
            {
                velocity.X -= 1;
                //recordValues(position.X.ToString(), position.Y.ToString());
            }
            if (InputManager.Instance.ActionKeyDown(ActionType.walk_down))
            {
                velocity.Y += 1;
                //recordValues(playerlocation.X.ToString(), playerlocation.Y.ToString());
            }
            if (InputManager.Instance.ActionKeyDown(ActionType.walk_up))
            {
                velocity.Y -= 1;
                //recordValues(playerlocation.X.ToString(), playerlocation.Y.ToString());
            }

            // normalise velocity, apply scalar values and add to position // normalise returns NaN if magnitude is 0
            if ((Math.Abs(velocity.X) + Math.Abs(velocity.Y)) > 0)
            {
                velocity.Normalize();
                velocity *= (float)(speed * (gameTime.ElapsedGameTime.TotalSeconds));
                newPosition += velocity;
            }

            // check that there's no collision with a wall
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            // this is a bodge
            // please forgive me god of code

            /////////////
            
            bool recalculatePosition = false;
            if (Map[(int)newPosition.Y / tileWidth, (int)newPosition.X / tileWidth] == (int)World.cellType.wall)
            {
                recalculatePosition = true;
            }
            if ( !((GameScreen)ScreenManager.Instance.currentScreen).IsRoomDead(currentRoom) && Map[(int)newPosition.Y / tileWidth, (int)newPosition.X / tileWidth] == (int)World.cellType.door)
            {
                recalculatePosition = true;
            }

            if (!recalculatePosition)
            {
                position = newPosition;

                // update boxes
                BoundingBox.Location = position.ToPoint();


                playerImage.position = position.ToPoint();
            }
            else
            {
                recalculatePosition = false;
                // issue - colliding with walls stops *all* movement, so take out colliding velocity
                if ((int)newPosition.Y / tileWidth != (int)position.Y / tileWidth) velocity.Y = 0; //colides with something above or below, so remove vertical component of velocity
                else velocity.X = 0;
                newPosition = position + velocity;

                if (Map[(int)newPosition.Y / tileWidth, (int)newPosition.X / tileWidth] == (int)World.cellType.wall)
                {
                    recalculatePosition = true;
                }
                if (!((GameScreen)ScreenManager.Instance.currentScreen).IsRoomDead(currentRoom) && Map[(int)newPosition.Y / tileWidth, (int)newPosition.X / tileWidth] == (int)World.cellType.door)
                {
                    recalculatePosition = true;
                }

                if (!recalculatePosition)
                {
                    position = newPosition;

                    // update boxes
                    BoundingBox.Location = position.ToPoint();


                    playerImage.position = position.ToPoint();
                }
            }


            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            inventory.Update(gameTime);

            if (InputManager.Instance.MousePressed())
            {
                inventory.UseItem(this);
            }

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            playerImage.Draw(spriteBatch);
        }


        void recordValues(string value1) // throwaway methods for testing without second screen
        {
            using (StreamWriter sw = new StreamWriter("output.txt"))
            {
                sw.WriteLine("value 1: " + value1);
            }
        }
        void recordValues(string value1, string value2) // for the love of god please don't use these they write a new line every frame
        {
            using (StreamWriter sw = new StreamWriter("output.txt"))
            {
                sw.WriteLine("value 1: " + value1 + " | value 2 :" + value2);
            }
        }

        public override void onCollision(GameCharacter e)
        {
            if (!hitCooldown)
            {
                TakeDamage(e.meleeDamage);
                hitCooldown = true;
            }
        }

        public bool AddItem(GameItem i)
        {
            return inventory.AddItem(this, i);
        }

        public bool AddAbility(GameAbility a)
        {
            return inventory.AddAbility(a);
        }

        public void AddAttackCooldown(double cooldownLength)
        {
            attackCooldown = true;
            currentAttackDelay = 0;
            maxAttackDelay = cooldownLength;
        }

        public void Boost(double duration, int newSpeed)
        {
            speed = newSpeed;
            maxBoostDelay = duration;
            boostCooldown = true;
        }

        public override void UnloadContent()
        {
            base.UnloadContent();

            inventory.UnLoadContent();
        }

    }
}
