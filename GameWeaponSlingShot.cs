﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MajorProject
{
    public class GameWeaponSlingShot : GameWeapon
    {
        int Damage = 50;
        public GameWeaponSlingShot()
        {
            attackCooldown = 1;
            type = this.GetType();
            itemType = "Slingshot";
            Description = "Slingshot.\nWeapon. Fires low-damage projectiles at a\nfast rate.\nDamage: " + Damage + "\nCooldown: " + attackCooldown + "s";
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

                p.position = new Vector2(position.X, position.Y);
                p.target = typeof(GameEnemy);
                p.totalLifeSpan = 10; // should survive until a collision with enemy or wall
                p.SetVelocity(mouseDirection); // the projectile should move
                p.speed = 250;
                p.damageType = GameProjectile.DamageType.Kinetic;
                p.damage = Damage;

                p.BoundingBox.Location = position.ToPoint();
                p.BoundingBox.Size = new Point(25, 25);

                

                ((GameScreen)ScreenManager.Instance.currentScreen).AddProjectile(p);

                base.Use(user);
            }



        }

    }
}
