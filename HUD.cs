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


        GameImage minimap;


        GamePlayer player;

        ResourcePack HUDResources;


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

                if (previousHealth > player.health)
                {

                    double healthRatio = (double)player.health / player.maxHealth;

                    healthBar.SpriteSize.X = (int)(healthRatio * maxHealthBarSize);

                    // the player has taken damage since last frame, so show the damage overlay
                    damageOverlay.alpha = 1;


                    // update the low health overlay
                    if (player.health < player.maxHealth / 2)
                    {
                        lowHealthOverlay.alpha = 1 - (player.health / ((float)player.maxHealth / 2));
                    }

                } else if (damageOverlay.alpha > 0)
                {

                    damageOverlay.alpha -= (float)(gameTime.ElapsedGameTime.TotalSeconds / damageOverlayFadeTime);
                }

                previousHealth = player.health;


            }

            


        }

        public void Draw(SpriteBatch spriteBatch)
        {
            mainBar.Draw(spriteBatch);
            healthBar.Draw(spriteBatch);

            damageOverlay.Draw(spriteBatch);

            lowHealthOverlay.Draw(spriteBatch);
        }

        public void UnloadContent()
        {
            HUDResources = null;
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

            mainBar.LoadContent(ref HUDResources, new string[1]{ "HUDTexture" });
            healthBar.LoadContent(ref HUDResources, new string[1] { "HealthBarTexture" });
            damageOverlay.LoadContent(ref HUDResources, new string[1] { "DamageTexture" });
            lowHealthOverlay.LoadContent(ref HUDResources, new string[1] { "DamageTexture" });
        }






    }
}