using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Serialization;

namespace MajorProject
{
    public class GameProjectile : GameEntity
    {
        // all the values the game projectile needs to use
        public bool hit;

        public double speed;

        public double totalLifeSpan;
        public double currentLifeSpan;

        [XmlIgnore]
        public int[,] Map;
        

        GameImage image;

        static Random rand = new Random();


        string[] textureNames = new string[3];

        // enum of all the different projectile types - some cause damage
        public enum DamageType
        {
            Kinetic,
            Fire,
            Ice,
            Toxic,
            SwordSwipe,
            Bullet,
            Blood
        }



        public DamageType damageType;
        public Type target;
        public int radius;
        public int damage;

        public GameProjectile()
        {
            // initialise the values - some are set in the function the projectile is created in
            image = new GameImage();
            type = this.GetType();
            currentLifeSpan = 0;
            totalLifeSpan = 2;
        }

        // sets the velocity of the projectile - normalises if not 0
        public void SetVelocity(Vector2 p)
        {
            velocity = p;
            if (velocity.X * velocity.X + velocity.Y * velocity.Y > 0)
            {
                velocity.Normalize();
            }
        }

        public override void Update(GameTime gameTime)
        {
            // update the image
            image.Update(gameTime);

            // add to total life span
            currentLifeSpan += gameTime.ElapsedGameTime.TotalSeconds;

            
            if (damageType != DamageType.Blood)
            {
                // apply unchanged velocity to the blood
                position.X += (float)(velocity.X * speed * gameTime.ElapsedGameTime.TotalSeconds);
                position.Y += (float)(velocity.Y * speed * gameTime.ElapsedGameTime.TotalSeconds);
            } else
            {
                // blood isn't propelled. only apply friction
                velocity *= 0.96f;
                position.X += (float)(velocity.X * speed * gameTime.ElapsedGameTime.TotalSeconds);
                position.Y += (float)(velocity.Y * speed * gameTime.ElapsedGameTime.TotalSeconds);
            }

            // check they aren't too old
            if (currentLifeSpan >= totalLifeSpan || (hit && damageType == DamageType.Bullet))
            {
                removeable = true;
            }

            // check they haven't hit wall
            if (damageType != DamageType.SwordSwipe)
                if (Map[(int)position.Y / World.tilePixelWidth, (int)position.X / World.tilePixelWidth] != (int)World.cellType.floor)
                {
                    removeable = true;
                    AudioManager.Instance.PlaySoundInstance(Resources.AudioPack["Wall_Impact"].CreateInstance(), "WallImpact" + rand.NextDouble().ToString());
                }


        }


        public override void LoadContent(ref ResourcePack resources)
        {
            // initialise all the position values

            base.LoadContent(ref resources);

            SpriteSize = new Point(25, 25);
            image.centered = true;
            radius = 12;
            image.NoLoop = true;

            image.SpriteSize = SpriteSize;

            // put all the frame information in the array
            for (int i = 1; i < 4; i++)
            {
                textureNames[i-1] = damageType.ToString() + i.ToString();
            }

            image.LoadContent(ref Resources, textureNames);
            image.animated = true;
        }

        public override void UnloadContent()
        {
            // unload the images

            image.UnloadContent();

            base.UnloadContent();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // draw the image
            image.position = position.ToPoint();
            image.Draw(spriteBatch);
        }

        public void OnCollision()
        {
            AudioManager.Instance.PlaySoundInstance(Resources.AudioPack["Character_Impact"].CreateInstance(), "CharacterImpact" + rand.NextDouble().ToString());
        }


    }
}
