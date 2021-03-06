using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

using static MajorProject.PlayerPreferences; // no longhand reference to difficulty enum needed

namespace MajorProject
{
    public class GameMenuScreen : Screen
    {

        // all the components that make up this UI screen

        public Image TitleImage;
        public Label TitleLabel;
        public Button ReturnToMenuButton;
        public Button SaveGameButton;
        public Button OptionsButton;
        public Button DifficultyButton;
        public Label DifficultyLabel;
        public Button BackToGameButton;

        SoundEffect ButtonHover;
        SoundEffect ButtonPress;

        difficultyLevel difficulty;

        bool returnToGame;


        public GameMenuScreen()
        {

        }

        public override void LoadContent()
        {

            // load the content of each element
            // assign the function delegates for when the items are used

            content = new ContentManager(ScreenManager.Instance.Content.ServiceProvider, "Content");

            ButtonHover = content.Load<SoundEffect>("Audio/Sound/UI/Button/FocusSound");
            ButtonPress = content.Load<SoundEffect>("Audio/Sound/UI/Button/SelectSound");

            ReturnToMenuButton.OnActivate = new UiElement.onActivate(ReturnToMenu);
            ReturnToMenuButton.OnHover = new UiElement.onHover(TriggerButtonHoverSound);

            SaveGameButton.OnActivate = new UiElement.onActivate(SaveGame);
            SaveGameButton.OnHover = new UiElement.onHover(TriggerButtonHoverSound);

            OptionsButton.OnActivate = new UiElement.onActivate(OpenOptions);
            OptionsButton.OnHover = new UiElement.onHover(TriggerButtonHoverSound);

            DifficultyButton.OnActivate = new UiElement.onActivate(ChangeDifficulty);
            DifficultyButton.OnHover = new UiElement.onHover(TriggerButtonHoverSound);

            BackToGameButton.OnActivate = new UiElement.onActivate(BackToGame);
            BackToGameButton.OnHover = new UiElement.onHover(TriggerButtonHoverSound);

            TitleLabel.LoadContent();

            ReturnToMenuButton.LoadContent();
            SaveGameButton.LoadContent();
            OptionsButton.LoadContent();
            DifficultyButton.LoadContent();
            BackToGameButton.LoadContent();

            // gets the difficulty from the user preferences class and initialises the difficulty toggle
            difficulty = Instance.difficulty;
            DifficultyLabel = new Label();
            DifficultyLabel.Text = difficulty.ToString();
            DifficultyLabel.Position = new Vector2(DifficultyButton.Position.X, DifficultyButton.Position.Y + 25);
            DifficultyLabel.LoadContent();
        }

        public override void UnloadContent()
        {
            // unload all the components

            TitleLabel.UnloadContent();

            ReturnToMenuButton.UnloadContent();
            SaveGameButton.UnloadContent();
            OptionsButton.UnloadContent();
            DifficultyButton.UnloadContent();
            BackToGameButton.UnloadContent();

            DifficultyLabel.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            // updates all the buttons
            ReturnToMenuButton.Update(gameTime);
            SaveGameButton.Update(gameTime);
            OptionsButton.Update(gameTime);
            DifficultyButton.Update(gameTime);
            BackToGameButton.Update(gameTime);

            // detects when it is save to return to menu - prevents multiple threads applying operations on the same object
            if (returnToGame)
            {
                if (!ScreenManager.Instance.IsTransitioning) {

                    if (((GameScreen)(ScreenManager.Instance.oldScreen)).saveOperationMut.WaitOne(1))
                    {
                        ScreenManager.Instance.LoadPreservedScreen();

                        ((GameScreen)(ScreenManager.Instance.oldScreen)).saveOperationMut.ReleaseMutex();
                    }

                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // draw all the components

            ReturnToMenuButton.Draw(spriteBatch);
            SaveGameButton.Draw(spriteBatch);
            OptionsButton.Draw(spriteBatch);
            DifficultyButton.Draw(spriteBatch);
            BackToGameButton.Draw(spriteBatch);

            TitleLabel.Draw(spriteBatch);
            DifficultyLabel.Draw(spriteBatch);
        }

        void ReturnToMenu(UiElement triggerElement)
        {
            // not going to return to the currently paused game, so don't need the screen backup anymore
            ScreenManager.Instance.UnloadPreservedScreen();
            // load the new menu screen
            ScreenManager.Instance.ChangeScreens("MainMenuScreen");
        }

        void SaveGame(UiElement triggerElement)
        {
            GameScreen gs = (GameScreen)ScreenManager.Instance.oldScreen; // the only way to access this menu is if oldscreen contains a game screen

            gs.SaveGame();
        }

        void OpenOptions(UiElement triggerElement)
        {
            ScreenManager.Instance.ChangeScreens("GameMenuOptionScreen");
        }

        // increases the difficulty enum (loops back if too big)
        void ChangeDifficulty(UiElement triggerElement)
        {
            difficulty = (difficultyLevel)((((int)difficulty) + 1) % 4);
            DifficultyLabel.Text = difficulty.ToString();

            Instance.difficulty = difficulty;
        }

        // pretty self explainatory
        void TriggerButtonHoverSound(UiElement triggeredObject)
        {
            AudioManager.Instance.PlaySoundInstance(ButtonHover.CreateInstance(), triggeredObject.Name);
        }

        void BackToGame(UiElement triggerElement)
        {
            AudioManager.Instance.PlaySoundInstance(ButtonPress.CreateInstance(), triggerElement.Name);

            // signals the update function should return to the game (does all the threadsafe checking there)
            returnToGame = true;

        }
    }
}
