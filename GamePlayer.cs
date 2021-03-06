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

        // money counter
        public int money;

        // timing cooldown values
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

        // stores the amount of time played - only increased when player update is called
        public double SecondsPlayed = 0;

        public GamePlayer()
        {

            // initialises the player object with default values

            inventory = new GameInventory();
            playerImage = new GameImage();
            money = 0;
            BaseSpeed = 200;
            speed = BaseSpeed;
            maxHitDelay = 1;
            currentHitDelay = 0;
            maxHealth = 100;
            health = 100;
            lastHealth = 100;
        }

        // all the frames in the walk animation
        string[] walkAnimation =
        {
            "Player_Forward"
        };

        // takes damage from projectile if not currently in hit cooldown
        public override void ProjectileCollision(GameProjectile p)
        {
            if (!hitCooldown)
                TakeDamage(p.damage);
        }

        // applies damage passed to the function (also plays takedamage sound)
        public override void TakeDamage(int damage)
        {
            damage = (int)(damage * PlayerPreferences.playerDamageScalars[PlayerPreferences.Instance.difficulty]);
            base.TakeDamage(damage);

            AudioManager.Instance.PlaySoundInstance(Resources.AudioPack["Player_Damage"].CreateInstance(), "PlayerDamage");
        }

        public override void LoadContent(ref ResourcePack resources)
        {
            // loads and inits all images and extra player objects (like inventory)
            base.LoadContent(ref resources);

            inventory.LoadContent(Resources);

            playerImage.LoadContent(ref Resources, walkAnimation);
            playerImage.animated = false;
            playerImage.centered = true;
            playerImage.SpriteSize = new Point(25, 25);

            BoundingBox.Size = SpriteSize;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            
            // adds time elapsed to seconds played
            SecondsPlayed += gameTime.ElapsedGameTime.TotalSeconds;

            // updates the speed boost checker (checks if it's time to stop boosting speed)
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

            // updates hit cooldown (if time has elapsed, hit cooldown no longer applies and the player can take damage)
            if (hitCooldown)
            {
                currentHitDelay += gameTime.ElapsedGameTime.TotalSeconds;
                if (currentHitDelay >= maxHitDelay)
                {
                    hitCooldown = false;
                    currentHitDelay = 0;
                }
            }

            // updates the attack cooldown (if timer has expired player can attack again)
            if (attackCooldown)
            {
                currentAttackDelay += gameTime.ElapsedGameTime.TotalSeconds;
                if (currentAttackDelay >= maxAttackDelay)
                {
                    attackCooldown = false;
                    currentAttackDelay = 0;
                }
            }

            // updates the player's inventory
            inventory.Update(gameTime);

            // checks if the use button has been clicked
            if (InputManager.Instance.MousePressed())
            {
                inventory.UseItem(this);
            }

            // calculates player movement

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
                // if current velocity ends up in a wall, remove that velocity and check again - prevents getting caught on wall
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


        }

        // draws the player image
        public override void Draw(SpriteBatch spriteBatch)
        {
            playerImage.Draw(spriteBatch);
        }

        /// <summary>
        /// log code I don't need because it's not really in a development phase
        /// </summary>

        /*
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
        */

        // function called when player collides with another character
        // includes taking damage
        public override void onCollision(GameCharacter e)
        {
            if (!hitCooldown)
            {
                TakeDamage(e.meleeDamage);
                hitCooldown = true;
            }
        }

        // adds an item to the player's inventory
        public bool AddItem(GameItem i)
        {
            return inventory.AddItem(this, i);
        }

        /*
        public bool AddAbility(GameAbility a)
        {
            return inventory.AddAbility(a);
        }
        */


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

            // unhook resources and unload image

            base.UnloadContent();

            inventory.UnLoadContent();
        }

        // function called on the player's death
        public override void OnDeath()
        {
            AudioManager.Instance.PlaySoundInstance(Resources.AudioPack["Player_Death"].CreateInstance(), "PlayerDeath");

        }

    }
}
