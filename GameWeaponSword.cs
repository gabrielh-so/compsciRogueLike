﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MajorProject
{
    public class GameWeaponSword : GameWeapon
    {
        int range = 50;

        public GameWeaponSword()
        {
            attackCooldown = 1;
            type = this.GetType();
            itemType = "Sword";
            WriteDescription();

        }

        public override void WriteDescription()
        {
            Description = "Sword.\nWeapon. Medium damage with small area of\neffect at a fast rate.\nDamage: " + Damage + "\nCooldown: " + attackCooldown + "s";
        }

        public override void Use(GamePlayer user)
        {
            position = user.position;
            // checks player hasn't already swung a weapon
            if (!user.attackCooldown)
            {

                // work out the direction the user is pointing
                // displace the short-life projectile a distance away in that direction to get the damage area for a sword hit


                // get mouse location relative to player (subrtact half the screen dimensions)
                Point mousePosition = InputManager.Instance.GetMousePosition();
                Vector2 mouseDirection = new Vector2((int)(mousePosition.X - ScreenManager.Instance.Dimensions.X / 2), (int)(mousePosition.Y - ScreenManager.Instance.Dimensions.Y / 2));


                if (mousePosition.X * mousePosition.X + mousePosition.Y * mousePosition.Y != 0)
                    mouseDirection.Normalize();



                GameProjectile p = new GameProjectile();

                p.position = new Vector2(position.X + mouseDirection.X * range, position.Y + mouseDirection.Y * range);
                p.target = typeof(GameEnemy);
                p.totalLifeSpan = 0.6;
                p.SetVelocity(new Vector2()); // the projectile shouldn't move anywhere
                p.speed = 0;
                p.damageType = GameProjectile.DamageType.SwordSwipe;
                p.damage = 75;

                p.BoundingBox.Location = position.ToPoint();
                p.BoundingBox.Size = new Point(25, 25);

                ((GameScreen)ScreenManager.Instance.currentScreen).AddProjectile(p);

                base.Use(user);
            }



        }

    }
}
