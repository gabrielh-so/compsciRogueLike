﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace MajorProject
{
    public class MainMenuOptionScreen : Screen
    {
        public Slider SoundVolumeSlider;
        public Slider MusicVolumeSlider;
        public Slider MasterVolumeSlider;

        public Button BackButton;
        public Button ResetButton;

        /*
        public KeyToggleButton walk_upToggle;
        public KeyToggleButton walk_rightToggle;
        public KeyToggleButton walk_downToggle;
        public KeyToggleButton walk_leftToggle;
        public KeyToggleButton pick_upToggle;
        public KeyToggleButton open_inventoryToggle;
        public KeyToggleButton use_potionToggle;
        */

        public List<Label> LabelList;

        public List<KeyToggleButton> ToggleButtonList;

        public Label optionDescriptor;

        SoundEffect ButtonHover;

        /*
        public Label walk_upLabel;
        public Label walk_rightLabel;
        public Label walk_downLabel;
        public Label walk_leftLabel;
        public Label pick_upLabel;
        public Label open_inventoryLabel;
        public Label use_potionLabel;
        */

        const string defaultDescriptionText = "Hover over an option for a description";

        public override void LoadContent()
        {
            content = new ContentManager(ScreenManager.Instance.Content.ServiceProvider, "Content");

            ButtonHover = content.Load<SoundEffect>("Audio/Sound/UI/Button/FocusSound");

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

            optionDescriptor.Text = defaultDescriptionText;
            optionDescriptor.LoadContent();

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
            content.Unload();
            content.Dispose();

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
            SoundVolumeSlider.Update(gameTime);
            MusicVolumeSlider.Update(gameTime);
            MasterVolumeSlider.Update(gameTime);

            foreach (KeyToggleButton b in ToggleButtonList)
            {
                b.Update(gameTime);
            }

            BackButton.Update(gameTime);
            ResetButton.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            SoundVolumeSlider.Draw(spriteBatch);
            MusicVolumeSlider.Draw(spriteBatch);
            MasterVolumeSlider.Draw(spriteBatch);

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
        }

        void ApplyToggleButtonDelegates(KeyToggleButton b)
        {
            b.OnHover = new UiElement.onHover(ToggleButtonHoverFunction);
            b.OnStopHover = new UiElement.onHover(ResetDescription);
        }

        void ToggleButtonHoverFunction(UiElement triggeredObject)
        {
            TriggerButtonHoverSound(triggeredObject);
            ShowDescription(triggeredObject);
        }

        void ShowDescription(UiElement triggeredObject)
        {
            optionDescriptor.Text = triggeredObject.Description;
        }

        void ResetDescription(UiElement triggeredObject)
        {
            optionDescriptor.Text = defaultDescriptionText;
        }

        void TriggerButtonHoverSound(UiElement triggeredObject)
        {
            AudioManager.Instance.PlaySoundInstance(ButtonHover.CreateInstance(), triggeredObject.Name);
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

            foreach (KeyToggleButton b in ToggleButtonList)
            {
                b.UpdateLabelText();
            }
            
        }

        void ResetVolumeToDefault(UiElement triggeredObject)
        {

        }

        void BackToMenu(UiElement triggeredObject)
        {
            ScreenManager.Instance.ChangeScreens("MainMenuScreen");
        }
    }
}
