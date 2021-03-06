using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

using static MajorProject.PlayerPreferences; // no longhand reference to difficulty enum needed

namespace MajorProject
{
    public class GameMenuOptionScreen : Screen
    {

        //flag to tell butons when to listen for hover
        bool ButtonIsListening;

        // all the components that make up this UI screen
        public Image KeyInputAlert;
        public Label KeyInputAlertText;

        public Label SoundVolumeLabel;
        public Label MusicVolumeLabel;
        public Label MasterVolumeLabel;

        public Slider SoundVolumeSlider;
        public Slider MusicVolumeSlider;
        public Slider MasterVolumeSlider;

        public Button BackButton;
        public Button ResetButton;

        public Button FontButton;
        public Label FontSizeLabel;

        /*
        public KeyToggleButton walk_upToggle;
        public KeyToggleButton walk_rightToggle;
        public KeyToggleButton walk_downToggle;
        public KeyToggleButton walk_leftToggle;
        public KeyToggleButton pick_upToggle;
        public KeyToggleButton open_inventoryToggle;
        public KeyToggleButton use_potionToggle;
        */

        // list of labels that label the key toggle buttons
        public List<Label> LabelList;

        // list of all the key toggle buttons
        public List<KeyToggleButton> ToggleButtonList;

        public Label optionDescriptor;

        // generic sound effect for button rollover
        SoundEffect ButtonHover;

        fontSizeLevel fontSize;

        /*
        public Label walk_upLabel;
        public Label walk_rightLabel;
        public Label walk_downLabel;
        public Label walk_leftLabel;
        public Label pick_upLabel;
        public Label open_inventoryLabel;
        public Label interactLabel;
        */

        const string defaultDescriptionText = "Hover over an option for a description";
        const string AlertText = "Press any available key to reassign value. Press ESC to cancel.";

        public override void LoadContent()
        {
            // load the content of each element
            // assign the function delegates for when the items are used

            content = new ContentManager(ScreenManager.Instance.Content.ServiceProvider, "Content");

            ButtonHover = content.Load<SoundEffect>("Audio/Sound/UI/Button/FocusSound");

            BackButton.LoadContent();
            BackButton.OnActivate = new UiElement.onActivate(BackToMenu);
            BackButton.OnHover = new UiElement.onHover(TriggerButtonHoverSound);

            ResetButton.LoadContent();
            ResetButton.OnActivate = new UiElement.onActivate(ResetKeysToDefault);
            ResetButton.OnHover = new UiElement.onHover(TriggerButtonHoverSound);

            FontButton.LoadContent();
            FontButton.OnActivate = new UiElement.onActivate(ChangeFontSize);
            FontButton.OnHover = new UiElement.onHover(TriggerButtonHoverSound);

            SoundVolumeLabel.LoadContent();
            MusicVolumeLabel.LoadContent();
            MasterVolumeLabel.LoadContent();

            SoundVolumeSlider.OnActivateF = new UiElement.onActivateF(ChangeSoundVolume);
            MusicVolumeSlider.OnActivateF = new UiElement.onActivateF(ChangeMusicVolume);
            MasterVolumeSlider.OnActivateF = new UiElement.onActivateF(ChangeMasterVolume);

            SoundVolumeSlider.LoadContent();
            MusicVolumeSlider.LoadContent();
            MasterVolumeSlider.LoadContent();

            SoundVolumeSlider.SetSliderPosition(AudioManager.Instance.SoundVolume, true);
            MusicVolumeSlider.SetSliderPosition(AudioManager.Instance.MusicVolume, true);
            MasterVolumeSlider.SetSliderPosition(AudioManager.Instance.MasterVolume, true);

            optionDescriptor.Text = defaultDescriptionText;
            optionDescriptor.LoadContent();

            // also initialise and assign values to all the labels

            fontSize = Instance.fontSize;
            FontSizeLabel = new Label();
            FontSizeLabel.Text = fontSize.ToString();
            FontSizeLabel.Position = new Vector2(FontButton.Position.X, FontButton.Position.Y + 25);
            FontSizeLabel.LoadContent();

            KeyInputAlert.LoadContent();
            KeyInputAlert.Scale = new Vector2(ScreenManager.Instance.GraphicsDevice.Viewport.Width / KeyInputAlert.Texture.Width, 1);
            KeyInputAlertText.Text = AlertText;
            KeyInputAlertText.LoadContent();

            foreach (Label l in LabelList)
            {
                l.LoadContent();
            }

            foreach (KeyToggleButton b in ToggleButtonList)
            {
                ApplyToggleButtonDelegates(b);
                b.LoadContent();
            }

            base.LoadContent();
        }

        public override void UnloadContent()
        {
            // unload all the components

            content.Unload();
            content.Dispose();

            KeyInputAlert.UnloadContent();

            SoundVolumeLabel.UnloadContent();
            MusicVolumeLabel.UnloadContent();
            MasterVolumeLabel.UnloadContent();

            SoundVolumeSlider.UnloadContent();
            MusicVolumeSlider.UnloadContent();
            MasterVolumeSlider.UnloadContent();

            optionDescriptor.UnloadContent();

            foreach (KeyToggleButton b in ToggleButtonList)
            {
                b.UnloadContent();
            }

            foreach (Label l in LabelList)
            {
                l.UnloadContent();
            }

            BackButton.UnloadContent();
            ResetButton.UnloadContent();

            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            // update the sliders
            SoundVolumeSlider.Update(gameTime);
            MusicVolumeSlider.Update(gameTime);
            MasterVolumeSlider.Update(gameTime);

            FontButton.Update(gameTime);

            // loop through all key toggle buttnos and update/activate listening flag if one is listening
            bool bListening = false;
            foreach (KeyToggleButton b in ToggleButtonList)
            {
                if (b.isListening)
                {
                    bListening = true;
                }
                b.Update(gameTime);
            }
            ButtonIsListening = bListening;

            BackButton.Update(gameTime);
            ResetButton.Update(gameTime);

            // if a button is listening, update the press button overlay
            if (ButtonIsListening)
            {
                KeyInputAlert.Update(gameTime);
            }

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // draw all the components
            SoundVolumeLabel.Draw(spriteBatch);
            MusicVolumeLabel.Draw(spriteBatch);
            MasterVolumeLabel.Draw(spriteBatch);


            SoundVolumeSlider.Draw(spriteBatch);
            MusicVolumeSlider.Draw(spriteBatch);
            MasterVolumeSlider.Draw(spriteBatch);

            FontButton.Draw(spriteBatch);
            FontSizeLabel.Draw(spriteBatch);

            optionDescriptor.Draw(spriteBatch);

            foreach (KeyToggleButton b in ToggleButtonList)
            {
                b.Draw(spriteBatch);
            }

            foreach (Label l in LabelList)
            {
                l.Draw(spriteBatch);
            }

            BackButton.Draw(spriteBatch);
            ResetButton.Draw(spriteBatch);

            // draw the press button overlay if a button is listening for input
            if (ButtonIsListening)
            {
                KeyInputAlert.Draw(spriteBatch);
                KeyInputAlertText.Draw(spriteBatch);
            }
        }

        // given a key toggle button, assign the delegates for proper function
        void ApplyToggleButtonDelegates(KeyToggleButton b)
        {
            b.OnHover = new UiElement.onHover(ToggleButtonHoverFunction);
            b.OnStopHover = new UiElement.onHover(ResetDescription);
        }

        // function called when a button is hovered over
        void ToggleButtonHoverFunction(UiElement triggeredObject)
        {
            TriggerButtonHoverSound(triggeredObject);
            ShowDescription(triggeredObject);
        }

        // updates the description label text
        void ShowDescription(UiElement triggeredObject)
        {
            optionDescriptor.Text = triggeredObject.Description;
        }

        // resets the description label text
        void ResetDescription(UiElement triggeredObject)
        {
            optionDescriptor.Text = defaultDescriptionText;
        }

        void TriggerButtonHoverSound(UiElement triggeredObject)
        {
            AudioManager.Instance.PlaySoundInstance(ButtonHover.CreateInstance(), triggeredObject.Name);
        }


        // all the sound functions
        void ChangeSoundVolume(UiElement triggeredObject, float value)
        {
            AudioManager.Instance.SoundVolume = value;
        }
        void ChangeMusicVolume(UiElement triggeredObject, float value)
        {
            AudioManager.Instance.MusicVolume = value;
        }
        void ChangeMasterVolume(UiElement triggeredObject, float value)
        {
            AudioManager.Instance.MasterVolume = value;
        }

        // the function that runs when the reset to defaults button is pressed
        void ResetKeysToDefault(UiElement triggeredObject)
        {
            PlayerPreferences.Instance.SetDefaultKeys();

            foreach (KeyToggleButton b in ToggleButtonList)
            {
                b.UpdateLabelText();
            }

        }

        // increments the font enum value, and reloads the components to apply changes
        void ChangeFontSize(UiElement triggeredElement)
        {
            fontSize = (fontSizeLevel)((((int)fontSize) + 1) % 3);
            FontSizeLabel.Text = fontSize.ToString();

            Instance.fontSize = fontSize;

            UnloadContent();
            LoadContent();
        }

        // function called when the back to menu button is pressed. returns to game menu
        void BackToMenu(UiElement triggeredObject)
        {
            ScreenManager.Instance.ChangeScreens("GameMenuScreen");
        }

        /*
        void ChangeFontSize(UiElement triggeredElement)
        {
            fontSize = (fontSizeLevel)((((int)fontSize) + 1) % 3);
            FontSizeLabel.Text = fontSize.ToString();

            Instance.fontSize = fontSize;

            LoadContent();
        }
        */
    }
}
