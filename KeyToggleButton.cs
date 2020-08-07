using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MajorProject
{
    public class KeyToggleButton : Button
    {
        public InputManager.ActionType toggleAction;
        public bool isListening;
        public Image ListeningImage;

        public Label ButtonLabel;

        public KeyToggleButton()
        {
            OnActivate = new onActivate(StartListening);
        }

        public override void LoadContent()
        {
            base.LoadContent();

            LoadLabel();
        }

        public void LoadLabel()
        {
            //StandardImage.Text = currentKey.ToString();
            //HoverImage.Text = currentKey.ToString();

            ButtonLabel = new Label();
            ButtonLabel.Position = Position;
            UpdateLabelText();
            ButtonLabel.LoadContent();
        }

        public void UpdateLabelText()
        {
            Keys currentKey = PlayerPreferences.Instance.ActionKeyDict[toggleAction];
            ButtonLabel.Text = currentKey.ToString();
        }

        public override void Update(GameTime gameTime)
        {
            if (InputManager.Instance.KeyPressed(Keys.Escape)) StopListening();

            if (isListening)
            {
                List<Keys> inputKeys;
                if ((inputKeys = InputManager.Instance.GetChangedKeys()).Count > 0)
                {
                    if (PlayerPreferences.Instance.UpdateKeyBinding(toggleAction, inputKeys[0]))
                    {
                        /*
                        StandardImage.Text = inputKeys[0].ToString();
                        HoverImage.Text = inputKeys[0].ToString();
                        */

                        ButtonLabel.Text = inputKeys[0].ToString();

                        //unload and reload image to display new character - should probably be a label or something
                        /*
                        StandardImage.RenderTexture();
                        HoverImage.RenderTexture();
                        PressedImage.RenderTexture();
                        ActiveImage = HoverImage;
                        */
                        StopListening();
                    }
                }

                //if (InputManager.Instance.MouseMoved()) isListening = false;

            }

            base.Update(gameTime);
        }

        public override void UnloadContent()
        {
            ButtonLabel.UnloadContent();
            base.UnloadContent();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            ButtonLabel.Draw(spriteBatch);
        }

        void StopListening()
        {
            isListening = false;
        }

        void StartListening(UiElement triggerObject)
        {
            isListening = true;
        }
    }
}
