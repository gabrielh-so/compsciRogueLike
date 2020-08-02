using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MajorProject
{
    public class MainMenuOptionScreen : Screen
    {
        public Slider SoundVolumeSlider;
        public Slider MusicVolumeSlider;
        public Slider MasterVolumeSlider;

        public Button BackButton;
        public Button ResetButton;

        public KeyToggleButton walk_upToggle;
        public KeyToggleButton walk_rightToggle;
        public KeyToggleButton walk_downToggle;
        public KeyToggleButton walk_leftToggle;
        public KeyToggleButton pick_upToggle;
        public KeyToggleButton open_inventoryToggle;
        public KeyToggleButton use_potionToggle;

        public Label optionDescriptor;

        public Label walk_upLabel;
        public Label walk_rightLabel;
        public Label walk_downLabel;
        public Label walk_leftLabel;
        public Label pick_upLabel;
        public Label open_inventoryLabel;
        public Label use_potionLabel;

        const string defaultDescriptionText = "Hover over an option for a description";

        public override void LoadContent()
        {
            BackButton.LoadContent();
            BackButton.OnActivate = new UiElement.onActivate(BackToMenu);

            ResetButton.LoadContent();
            ResetButton.OnActivate = new UiElement.onActivate(ResetKeysToDefault);

            SoundVolumeSlider.OnActivateF = new UiElement.onActivateF(ChangeSoundVolume);
            MusicVolumeSlider.OnActivateF = new UiElement.onActivateF(ChangeMusicVolume);
            MasterVolumeSlider.OnActivateF = new UiElement.onActivateF(ChangeMasterVolume);

            SoundVolumeSlider.LoadContent();
            MusicVolumeSlider.LoadContent();
            MasterVolumeSlider.LoadContent();

            SoundVolumeSlider.SetSliderPosition(AudioManager.Instance.SoundVolume, true);
            MusicVolumeSlider.SetSliderPosition(AudioManager.Instance.MusicVolume, true);
            MasterVolumeSlider.SetSliderPosition(AudioManager.Instance.MasterVolume, true);

            //optionDescriptor.Text = defaultDescriptionText;
            walk_upLabel.LoadContent();
            walk_rightLabel.LoadContent();
            walk_downLabel.LoadContent();
            walk_leftLabel.LoadContent();
            pick_upLabel.LoadContent();
            open_inventoryLabel.LoadContent();
            use_potionLabel.LoadContent();

            optionDescriptor.LoadContent();

            walk_upToggle.LoadContent();
            walk_upToggle.toggleAction = InputManager.ActionType.walk_up;
            walk_upToggle.OnHover = new UiElement.onHover(ShowDescription);
            walk_upToggle.OnStopHover = new UiElement.onHover(ResetDescription);

            walk_rightToggle.LoadContent();
            walk_rightToggle.toggleAction = InputManager.ActionType.walk_right;
            walk_rightToggle.OnHover = new UiElement.onHover(ShowDescription);
            walk_rightToggle.OnStopHover = new UiElement.onHover(ResetDescription);

            walk_downToggle.LoadContent();
            walk_downToggle.toggleAction = InputManager.ActionType.walk_down;
            walk_downToggle.OnHover = new UiElement.onHover(ShowDescription);
            walk_downToggle.OnStopHover = new UiElement.onHover(ResetDescription);

            walk_leftToggle.LoadContent();
            walk_leftToggle.toggleAction = InputManager.ActionType.walk_left;
            walk_leftToggle.OnHover = new UiElement.onHover(ShowDescription);
            walk_leftToggle.OnStopHover = new UiElement.onHover(ResetDescription);

            pick_upToggle.LoadContent();
            pick_upToggle.toggleAction = InputManager.ActionType.pick_up;
            pick_upToggle.OnHover = new UiElement.onHover(ShowDescription);
            pick_upToggle.OnStopHover = new UiElement.onHover(ResetDescription);

            open_inventoryToggle.LoadContent();
            open_inventoryToggle.toggleAction = InputManager.ActionType.open_inventory;
            open_inventoryToggle.OnHover = new UiElement.onHover(ShowDescription);
            open_inventoryToggle.OnStopHover = new UiElement.onHover(ResetDescription);

            use_potionToggle.LoadContent();
            use_potionToggle.toggleAction = InputManager.ActionType.use_potion;
            use_potionToggle.OnHover = new UiElement.onHover(ShowDescription);
            use_potionToggle.OnStopHover = new UiElement.onHover(ResetDescription);

            base.LoadContent();
        }

        public override void UnloadContent()
        {
            SoundVolumeSlider.UnloadContent();
            MusicVolumeSlider.UnloadContent();
            MasterVolumeSlider.UnloadContent();

            walk_upToggle.UnloadContent();
            walk_rightToggle.UnloadContent();
            walk_downToggle.UnloadContent();
            walk_leftToggle.UnloadContent();
            pick_upToggle.UnloadContent();
            open_inventoryToggle.UnloadContent();
            use_potionToggle.UnloadContent();

            optionDescriptor.UnloadContent();

            walk_upLabel.UnloadContent();
            walk_rightLabel.UnloadContent();
            walk_downLabel.UnloadContent();
            walk_leftLabel.UnloadContent();
            pick_upLabel.UnloadContent();
            open_inventoryLabel.UnloadContent();
            use_potionLabel.UnloadContent();

            BackButton.UnloadContent();
            ResetButton.UnloadContent();

            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            SoundVolumeSlider.Update(gameTime);
            MusicVolumeSlider.Update(gameTime);
            MasterVolumeSlider.Update(gameTime);


            walk_upToggle.Update(gameTime);
            walk_rightToggle.Update(gameTime);
            walk_downToggle.Update(gameTime);
            walk_leftToggle.Update(gameTime);
            pick_upToggle.Update(gameTime);
            open_inventoryToggle.Update(gameTime);
            use_potionToggle.Update(gameTime);

            BackButton.Update(gameTime);
            ResetButton.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            SoundVolumeSlider.Draw(spriteBatch);
            MusicVolumeSlider.Draw(spriteBatch);
            MasterVolumeSlider.Draw(spriteBatch);

            walk_upToggle.Draw(spriteBatch);
            walk_rightToggle.Draw(spriteBatch);
            walk_downToggle.Draw(spriteBatch);
            walk_leftToggle.Draw(spriteBatch);
            pick_upToggle.Draw(spriteBatch);
            open_inventoryToggle.Draw(spriteBatch);
            use_potionToggle.Draw(spriteBatch);

            walk_upLabel.Draw(spriteBatch);
            walk_rightLabel.Draw(spriteBatch);
            walk_downLabel.Draw(spriteBatch);
            walk_leftLabel.Draw(spriteBatch);
            pick_upLabel.Draw(spriteBatch);
            open_inventoryLabel.Draw(spriteBatch);
            use_potionLabel.Draw(spriteBatch);

            BackButton.Draw(spriteBatch);
            ResetButton.Draw(spriteBatch);

            optionDescriptor.Draw(spriteBatch);
        }

        void ShowDescription(UiElement triggeredObject)
        {
            optionDescriptor.Text = triggeredObject.Description;
        }

        void ResetDescription(UiElement triggeredObject)
        {
            optionDescriptor.Text = defaultDescriptionText;
        }

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

        void ResetKeysToDefault(UiElement triggeredObject)
        {
            PlayerPreferences.Instance.SetDefaultKeys();

            
            walk_upToggle.UpdateLabelText();
            walk_rightToggle.UpdateLabelText();
            walk_downToggle.UpdateLabelText();
            walk_leftToggle.UpdateLabelText();
            pick_upToggle.UpdateLabelText();
            open_inventoryToggle.UpdateLabelText();
            use_potionToggle.UpdateLabelText();
            
        }

        void ResetVolumeToDefault(UiElement triggeredObject)
        {
            PlayerPreferences.Instance.SetDefaultKeys();
        }

        void BackToMenu(UiElement triggeredObject)
        {
            ScreenManager.Instance.ChangeScreens("MainMenuScreen");
        }
    }
}
