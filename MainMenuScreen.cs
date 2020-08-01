using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
        public Button QuitButton;



        public MainMenuScreen()
        {

        }

        public override void LoadContent()
        {
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
        }

        public override void UnloadContent()
        {
            TitleLabel.UnloadContent();

            NewGameButton.UnloadContent();
            ContinueGameButton.UnloadContent();
            OptionsButton.UnloadContent();
            DifficultyButton.UnloadContent();
            QuitButton.UnloadContent();
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
        }

        void NewGame(UiElement triggerElement)
        {
            ScreenManager.Instance.ChangeScreens("SplashScreen");
        }

        void ContinueGame(UiElement triggerElement)
        {
            ScreenManager.Instance.ChangeScreens("MainMenuScreen");
        }

        void OpenOptions(UiElement triggerElement)
        {
            ScreenManager.Instance.ChangeScreens("MainMenuOptionScreen");
        }

        void ChangeDifficulty(UiElement triggerElement)
        {
            ScreenManager.Instance.ChangeScreens("MainMenuScreen");
        }

        void Quit(UiElement triggerElement)
        {
            InputManager.Instance.QuitSignaled = true;
        }
    }
}
