using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using static MajorProject.PlayerPreferences; // no longhand reference to difficulty enum needed

namespace MajorProject
{
    public class GameMenuScreen : Screen
    {

        public Image TitleImage;
        public Label TitleLabel;
        public Button ReturnToMenuButton;
        public Button SaveGameButton;
        public Button OptionsButton;
        public Button DifficultyButton;
        public Label DifficultyLabel;
        public Button BackToGameButton;

        difficultyLevel difficulty;

        public GameMenuScreen()
        {

        }

        public override void LoadContent()
        {
            ReturnToMenuButton.OnActivate = new UiElement.onActivate(ReturnToMenu);
            SaveGameButton.OnActivate = new UiElement.onActivate(SaveGame);
            OptionsButton.OnActivate = new UiElement.onActivate(OpenOptions);
            DifficultyButton.OnActivate = new UiElement.onActivate(ChangeDifficulty);
            BackToGameButton.OnActivate = new UiElement.onActivate(BackToGame);

            TitleLabel.LoadContent();

            ReturnToMenuButton.LoadContent();
            SaveGameButton.LoadContent();
            OptionsButton.LoadContent();
            DifficultyButton.LoadContent();
            BackToGameButton.LoadContent();

            difficulty = Instance.difficulty;
            DifficultyLabel = new Label();
            DifficultyLabel.Text = difficulty.ToString();
            DifficultyLabel.Position = new Vector2(DifficultyButton.Position.X, DifficultyButton.Position.Y + 25);
            DifficultyLabel.LoadContent();
        }

        public override void UnloadContent()
        {
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
            ReturnToMenuButton.Update(gameTime);
            SaveGameButton.Update(gameTime);
            OptionsButton.Update(gameTime);
            DifficultyButton.Update(gameTime);
            BackToGameButton.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

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
            ScreenManager.Instance.UnloadPreservedScreen();
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

        void ChangeDifficulty(UiElement triggerElement)
        {
            difficulty = (difficultyLevel)((((int)difficulty) + 1) % 4);
            DifficultyLabel.Text = difficulty.ToString();

            Instance.difficulty = difficulty;
        }

        void BackToGame(UiElement triggerElement)
        {
            ScreenManager.Instance.LoadPreservedScreen();
        }
    }
}
