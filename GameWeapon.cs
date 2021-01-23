using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MajorProject
{
    public class GameWeapon : GameItem
    {
        public int Damage;

        public double attackCooldown;

        GameImage image; // image to be displayed when on the ground

        public string[] WeaponAnimation;

        public GameWeapon()
        {
            radius = 25;

            image = new GameImage();
        }

        public override void Use(GamePlayer user)
        {
            user.AddAttackCooldown(attackCooldown);
        }

        public override void Update(GameTime gameTime)
        {
            // account for friction - allows launched coins to stop at some point
            if (velocity.LengthSquared() > 0) velocity *= 0.8f;

            position += velocity;
            BoundingBox.Location = position.ToPoint();
            BoundingBox.X -= BoundingBox.Width / 2;
            BoundingBox.Y -= BoundingBox.Height / 2;

            image.position = position.ToPoint();
            image.Update(gameTime);
        }

        public override void LoadContent(ref ResourcePack resources)
        {
            Resources = resources;


            image.animated = true;
            image.centered = true;
            image.SpriteSize = new Point(25, 25);

            image.position = position.ToPoint();
            BoundingBox.Location = position.ToPoint();


            BoundingBox.Size = new Point(25, 25);

            removeable = false;


            WeaponAnimation = new string[1]
            {
                itemType
            };

            image.LoadContent(ref Resources, WeaponAnimation);

            image.animated = true;
            image.centered = true;
            image.SpriteSize = new Point(25, 25);
        }
        public override void UnloadContent()
        {
            Resources = null;
            image.UnloadContent();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (OnGround)
                image.Draw(spriteBatch);
        }

        public virtual void SetWeaponDamage(int newDamage)
        {
            Damage = newDamage;
            WriteDescription();
        }

        public virtual void SetWeaponCooldown(int newCooldown)
        {
            attackCooldown = newCooldown;
            WriteDescription();
        }
        

    }
}
