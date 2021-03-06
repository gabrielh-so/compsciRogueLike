using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MajorProject
{
    public class ExitInteractable : GameInteractable
    {
        public GameImage image;

        string[] OpenAnimation;

        public ExitInteractable()
        {
            // initialises the image, position and bounding information

            type = this.GetType();

            image = new GameImage();

            BoundingBox.Size = new Point(50, 50);

            image.NoLoop = true;
            image.centered = true;
            image.SpriteSize = new Point(50, 50);

            image.position = position.ToPoint();
            BoundingBox.Location = position.ToPoint();

            removeable = false;
        }

        public override void Use(int LevelIndex, GamePlayer user)
        {
            // determine powerful weapon based on level number and difficulty

            // checks not already transitioning
            if (!ScreenManager.Instance.IsTransitioning)
                // checks boss is dead (will always be in first room)
                if (!((GameScreen)ScreenManager.Instance.currentScreen).Enemies[0][0].alive)
                    // flag that the level should be changed
                    ((GameScreen)ScreenManager.Instance.currentScreen).SignalLevelChange();
        }

        public override void LoadContent(ref ResourcePack resources)
        {
            // assigns the reference to the resources object
            Resources = resources;

            // get the animation string - only one so a static image
            OpenAnimation = new string[1]
            {
                "Exit_Texture"
            };

            // load image with frame name
            image.LoadContent(ref Resources, OpenAnimation);
            image.animated = false;
        }

        public override void UnloadContent()
        {
            // unloads base, the image and unhooks resources
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
            // update image positions
            image.position = position.ToPoint();
            image.Draw(spriteBatch);

            // if the boss enemy is dead, display the instruction text
            if (IsHovering && !((GameScreen)ScreenManager.Instance.currentScreen).Enemies[0][0].alive)
            {
                spriteBatch.DrawString(Resources.FontPack["coders_crux" + "_" + PlayerPreferences.Instance.fontSize.ToString()], "Press 'E' to interact.", position, color: Color.Blue);
            }
        }

    }
}
