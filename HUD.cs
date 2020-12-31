using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MajorProject
{
    public class HUD
    {

        // woah! new class time(TM)

        // this will store a gameplayer entity and read the inventory off of it, and display the results

        // located at the bottom of the screen - covers 1/(4-5)th of it

        // it will also display a mini-map of the level!

        MiniMap miniMap;

        GamePlayer player;

        ResourcePack HUDResources;

        GameLabel playerAttackCooldownLabel;
        GameLabel MoneyLabel;


        GameImage mainBar;
        GameImage healthBar;

        GameImage damageOverlay;
        double damageOverlayFadeTime = 2;

        GameImage lowHealthOverlay;

        int previousHealth;


        int maxHealthBarSize = 300;


        public HUD()
        {
            mainBar = new GameImage();
            healthBar = new GameImage();
            damageOverlay = new GameImage();
            lowHealthOverlay = new GameImage();

            playerAttackCooldownLabel = new GameLabel();
            MoneyLabel = new GameLabel();

            miniMap = new MiniMap();
        }




        public void SetPlayer(GamePlayer p)
        {
            player = p;
            previousHealth = p.health;

            double healthRatio = (double)player.health / player.maxHealth;

            healthBar.SpriteSize.X = (int)(healthRatio * maxHealthBarSize);

            damageOverlay.alpha = 0;

            lowHealthOverlay.alpha = 0;
        }

        public void UnSetPlayer()
        {
            player = null;
        }

        public void GenerateMiniMap(int[,] map)
        {
            miniMap.Map = map;

            miniMap.RenderTexture();
            // assigns the minimap texture a new value based on what the map details are
        }

        public void Update(GameTime gameTime)
        {

            if (player != null)
            {
                // roight so you 'ave a gander at the ol' playah, yeah?
                // then you get wot he has, and then put it on the screen

                // update the displayed inventory

                // player handles dropping stuff


                // for now, just display the main bar and the player's health

                // find the ratio of current player health to max health

                if (previousHealth != player.health)
                {

                    double healthRatio = (double)player.health / player.maxHealth;

                    healthBar.SpriteSize.X = (int)(healthRatio * maxHealthBarSize);

                    // the player has taken damage since last frame, so show the damage overlay
                    if (previousHealth > player.health)
                        damageOverlay.alpha = 1;


                    // update the low health overlay
                    if (player.health < player.maxHealth / 2)
                    {
                        lowHealthOverlay.alpha = 1 - (player.health / ((float)player.maxHealth / 2));
                    }
                    else lowHealthOverlay.alpha = 0;

                } else if (damageOverlay.alpha > 0)
                {

                    damageOverlay.alpha -= (float)(gameTime.ElapsedGameTime.TotalSeconds / damageOverlayFadeTime);
                }

                previousHealth = player.health;


                if (player.attackCooldown)
                {

                    playerAttackCooldownLabel.Text = (player.maxAttackDelay - player.currentAttackDelay).ToString();
                }

                miniMap.TargetPosition = player.position.ToPoint();

                MoneyLabel.Text = player.money.ToString() + "G";
            }



        }

        public void Draw(SpriteBatch spriteBatch)
        {
            mainBar.Draw(spriteBatch);
            healthBar.Draw(spriteBatch);

            int selectedSlot = player.inventory.SelectedItemSlot;
            int offset = 0;
            foreach (GameItem i in player.inventory.itemList)
            {
                if (i != null)
                {
                    spriteBatch.Draw(i.Resources.TexturePack[i.itemType], destinationRectangle: new Rectangle(new Point(450 + 100 * offset, 575), new Point(50, 50)));
                }
                if (offset == selectedSlot)
                    spriteBatch.Draw(HUDResources.TexturePack["5"], destinationRectangle: new Rectangle(new Point(450 + 100 * offset, 575), new Point(50, 50)), color: Color.White * 0.25f);

                offset++;
            }

            if (player.attackCooldown)
            {
                playerAttackCooldownLabel.Draw(spriteBatch);
            }

            MoneyLabel.Draw(spriteBatch);

            damageOverlay.Draw(spriteBatch);

            lowHealthOverlay.Draw(spriteBatch);

            miniMap.Draw(spriteBatch);
        }

        public void UnloadContent()
        {
            HUDResources = null;

            mainBar.UnloadContent();
            healthBar.UnloadContent();
            damageOverlay.UnloadContent();
            lowHealthOverlay.UnloadContent();
            miniMap.UnloadContent();

            playerAttackCooldownLabel.UnloadContent();

            MoneyLabel.UnloadContent();
        }

        public void LoadContent(ResourcePack resources)
        {
            HUDResources = resources;

            mainBar.position = new Point(50, 550);
            mainBar.SpriteSize = new Point(1100, 100);

            healthBar.position = new Point(100, 575);
            healthBar.SpriteSize = new Point(300, 50);

            damageOverlay.position = new Point(0, 0);
            damageOverlay.SpriteSize = new Point(1200, 720);

            lowHealthOverlay.position = new Point(0, 0);
            lowHealthOverlay.SpriteSize = new Point(1200, 720);

            miniMap.position = new Point(1000, 575);
            miniMap.SpriteSize = new Point(95, 95);
            miniMap.ViewSize = new Point(1500/25, 1500/25);

            playerAttackCooldownLabel.SetPosition(50, 550);
            playerAttackCooldownLabel.FontName = "coders_crux";

            MoneyLabel.SetPosition(450, 550);
            MoneyLabel.FontName = "coders_crux";
            MoneyLabel.FontColor = Color.Yellow;

            miniMap.LoadContent(ref HUDResources);

            mainBar.LoadContent(ref HUDResources, new string[1]{ "HUDTexture" });
            healthBar.LoadContent(ref HUDResources, new string[1] { "HealthBarTexture" });
            damageOverlay.LoadContent(ref HUDResources, new string[1] { "DamageTexture" });
            lowHealthOverlay.LoadContent(ref HUDResources, new string[1] { "DamageTexture" });
            playerAttackCooldownLabel.LoadContent(ref HUDResources);
            MoneyLabel.LoadContent(ref HUDResources);

        }






    }
}