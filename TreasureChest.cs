using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MajorProject
{
    public class TreasureChest : GameInteractable
    {
        public bool IsOpen;

        public GameImage image;

        Random rand;

        string[] OpenAnimation;

        public TreasureChest()
        {
            rand = new Random();

            type = this.GetType();

            image = new GameImage();
        }

        public override void Use(int LevelIndex, GamePlayer user)
        {
            // determine powerful weapon based on level number and difficulty

            if (!IsOpen)
            {
                IsOpen = true;

                image.animated = true;

                double movementAngle;

                for (int i = rand.Next(2, 5); i > -1; i--)
                {
                    GameCoin c = new GameCoin();
                    c.value = 10;
                    c.SetPosition(position.X, position.Y);
                    movementAngle = rand.NextDouble() * Math.PI * 2;
                    c.velocity = new Vector2((float)Math.Sin(movementAngle), (float)Math.Cos(movementAngle));

                    ((GameScreen)ScreenManager.Instance.currentScreen).AddItem(c);
                }



                ///////////////////////
                GameWeapon w;

                double weaponRoll = rand.NextDouble();
                if (weaponRoll < 0.25)
                {
                    w = new GameWeaponSword();
                    w.SetWeaponDamage(PlayerPreferences.Instance.weaponDamages[LevelIndex]["Sword"]);
                }
                else if (weaponRoll < 0.5)
                {
                    w = new GameWeaponSpear();
                    w.SetWeaponDamage(PlayerPreferences.Instance.weaponDamages[LevelIndex]["Spear"]);
                }
                else if (weaponRoll < 0.75)
                {
                    w = new GameWeaponSlingShot();
                    w.SetWeaponDamage(PlayerPreferences.Instance.weaponDamages[LevelIndex]["Slingshot"]);
                }
                else
                {
                    w = new GameWeaponRifle();
                    w.SetWeaponDamage(PlayerPreferences.Instance.weaponDamages[LevelIndex]["Rifle"]);
                }

                w.SetWeaponDamage((int)(w.Damage * (1 + (rand.NextDouble()  ) / 5)));

                movementAngle = rand.NextDouble() * Math.PI * 2;
                w.velocity = new Vector2((float)Math.Sin(movementAngle), (float)Math.Cos(movementAngle)) * 1.5f;
                w.OnGround = true;
                w.SetPosition(position.X, position.Y);
                ((GameScreen)ScreenManager.Instance.currentScreen).AddItem(w);

            }
        }

        public override void LoadContent(ref ResourcePack resources)
        {
            Resources = resources;


            BoundingBox.Size = new Point(100, 100);

            image.NoLoop = true;
            image.centered = true;
            image.SpriteSize = new Point(100, 100);

            image.position = position.ToPoint();
            BoundingBox.Location = (position - BoundingBox.Size.ToVector2() / 2).ToPoint();

            removeable = false;

            OpenAnimation = new string[5]
            {
                "Chest1",
                "Chest2",
                "Chest3",
                "Chest4",
                "Chest5"
            };

            image.LoadContent(ref Resources, OpenAnimation);
            image.animated = false;
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            image.UnloadContent();
            Resources = null;
        }

        public override void Update(GameTime gameTime)
        {
            IsHovering = false;
            BoundingBox.Location = (position - BoundingBox.Size.ToVector2() / 2).ToPoint();
            image.Update(gameTime);
        }

        public override void Hovering()
        {
            IsHovering = true;
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            image.position = position.ToPoint();
            image.Draw(spriteBatch);

            if (IsHovering)
            {
                spriteBatch.DrawString(Resources.FontPack["coders_crux" + "_" + PlayerPreferences.Instance.fontSize.ToString()], "Press 'E' to interact.", position, color: Color.Blue);
            }
        }
    }
}
