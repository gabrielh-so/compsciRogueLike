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
    public class MainMenuScreen : Screen
    {

        public Image TitleImage;
        public Label TitleLabel;
        public Button NewGameButton;
        public Button ContinueGameButton;
        public Button OptionsButton;
        public Button DifficultyButton;
        public Label DifficultyLabel;
        public Button QuitButton;

        difficultyLevel difficulty;

        public MainMenuScreen()
        {

        }

        public override void LoadContent()
        {
            AudioManager.Instance.PlayMusic("Audio/Sound/UI/Music/Music");

            NewGameButton.OnActivate = new UiElement.onActivate(NewGame);
            ContinueGameButton.OnActivate = new UiElement.onActivate(ContinueGame);
            OptionsButton.OnActivate = new UiElement.onActivate(OpenOptions);
            DifficultyButton.OnActivate = new UiElement.onActivate(ChangeDifficulty);
            QuitButton.OnActivate = new UiElement.onActivate(Quit);

            TitleLabel.LoadContent();

            NewGameButton.LoadContent();
            ContinueGameButton.LoadContent();
            OptionsButton.LoadContent();
            DifficultyButton.LoadContent();
            QuitButton.LoadContent();

            difficulty = Instance.difficulty;
            DifficultyLabel = new Label();
            DifficultyLabel.Text = difficulty.ToString();
            DifficultyLabel.Position = new Vector2(DifficultyButton.Position.X, DifficultyButton.Position.Y + 25);
            DifficultyLabel.LoadContent();
        }

        public override void UnloadContent()
        {
            TitleLabel.UnloadContent();

            NewGameButton.UnloadContent();
            ContinueGameButton.UnloadContent();
            OptionsButton.UnloadContent();
            DifficultyButton.UnloadContent();
            QuitButton.UnloadContent();

            DifficultyLabel.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            NewGameButton.Update(gameTime);
            ContinueGameButton.Update(gameTime);
            OptionsButton.Update(gameTime);
            DifficultyButton.Update(gameTime);
            QuitButton.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            NewGameButton.Draw(spriteBatch);
            ContinueGameButton.Draw(spriteBatch);
            OptionsButton.Draw(spriteBatch);
            DifficultyButton.Draw(spriteBatch);
            QuitButton.Draw(spriteBatch);

            TitleLabel.Draw(spriteBatch);
            DifficultyLabel.Draw(spriteBatch);
        }

        void NewGame(UiElement triggerElement)
        {
            PlayerPreferences.Instance.LoadSavedGame = false;
            ScreenManager.Instance.ChangeScreens("GameScreen");
        }

        void ContinueGame(UiElement triggerElement)
        {
            PlayerPreferences.Instance.LoadSavedGame = true;
            ScreenManager.Instance.ChangeScreens("GameScreen");
        }

        void OpenOptions(UiElement triggerElement)
        {
            ScreenManager.Instance.ChangeScreens("MainMenuOptionScreen");
        }

        void ChangeDifficulty(UiElement triggerElement)
        {
            difficulty = (difficultyLevel)((((int)difficulty)+1)%4);
            DifficultyLabel.Text = difficulty.ToString();

            Instance.difficulty = difficulty;
        }

        void Quit(UiElement triggerElement)
        {
            InputManager.Instance.QuitSignaled = true;
        }
    }
}
