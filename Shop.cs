using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MajorProject
{
    public class Shop : GameInteractable
    {
        public bool IsPurchased;

        public int ItemPrice;

        public GameItem item;

        static Random rand = new Random();

        string[] OpenAnimation;

        public Shop()
        {
            type = this.GetType();

        }

        public override void Use(int LevelIndex, GamePlayer user)
        {
            // determine powerful weapon based on level number and difficulty
            if (user.money < ItemPrice) return;

            // checks that it hasn't been purchased already
            if (!IsPurchased)
            {

                user.money -= ItemPrice;

                IsPurchased = true;

                double movementAngle;
                movementAngle = rand.NextDouble() * Math.PI * 2;
                item.velocity = new Vector2((float)Math.Sin(movementAngle), (float)Math.Cos(movementAngle));

                ((GameScreen)ScreenManager.Instance.currentScreen).AddItem(item);

                item = null;
            }
        }

        public override void LoadContent(ref ResourcePack resources)
        {
            Resources = resources;

            BoundingBox.Size = new Vector2(50, 50).ToPoint();
            BoundingBox.Location = (position - BoundingBox.Size.ToVector2() / 2).ToPoint();

            removeable = false;

            if (item != null)
                item.LoadContent(ref Resources);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            if (item != null)
                item.UnloadContent();
            Resources = null;
        }

        public override void Update(GameTime gameTime)
        {
            IsHovering = false;
            BoundingBox.Location = (position - BoundingBox.Size.ToVector2() / 2).ToPoint();
        }

        public override void Hovering()
        {
            IsHovering = true;
        }

        public void NewItem(int price)
        {
            ItemPrice = price;

            // more likely to sell potions than weapons
            if (rand.NextDouble() < 0.75)
            {
                double potionRoll = rand.NextDouble();
                if (potionRoll < 0.25)
                {
                    item = new GamePotionHealth();
                } else if (potionRoll < 0.5)
                {
                    item = new GamePotionImmune();
                } else if (potionRoll < 0.75)
                {
                    item = new GamePotionSpeed();
                }
                else
                {
                    item = new GamePotionRecharge();
                }
            } else
            {
                double weaponRoll = rand.NextDouble();
                if (weaponRoll < 0.25)
                {
                    item = new GameWeaponSword();
                }
                else if (weaponRoll < 0.5)
                {
                    item = new GameWeaponSpear();
                }
                else if (weaponRoll < 0.75)
                {
                    item = new GameWeaponSlingShot();
                }
                else
                {
                    item = new GameWeaponRifle();
                }
                item.OnGround = true;
                ((GameWeapon)item).SetWeaponDamage((int)(PlayerPreferences.Instance.weaponDamages[0][item.itemType] * (1 + (rand.NextDouble()) / 5)));
                
            }

            item.position = position;
            item.LoadContent(ref Resources);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (item != null)
                item.Draw(spriteBatch);

            if (IsHovering && !IsPurchased)
            {
                spriteBatch.DrawString(Resources.FontPack["coders_crux" + "_" + PlayerPreferences.Instance.fontSize.ToString()], "Press 'E' to purchase for " + ItemPrice + "G.", position, color: Color.Blue); ;
            }
        }
    }
}
