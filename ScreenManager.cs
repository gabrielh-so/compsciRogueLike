using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MajorProject
{
    public class ScreenManager
    {
        [XmlIgnore]
        Screen oldScreen;


        private static ScreenManager instance;
        [XmlIgnore]
        public Vector2 Dimensions { set; get; }
        [XmlIgnore]
        public ContentManager Content { private set; get; }
        XmlManager<Screen> xmlGameScreenManager;
            
        Screen currentScreen, newScreen;
        [XmlIgnore]
        public GraphicsDevice GraphicsDevice;
        [XmlIgnore]
        public SpriteBatch SpriteBatch;

        public Image Image;
        [XmlIgnore]
        public bool IsTransitioning { get; private set; }

        enum transitionStyles
        {
            FadeOut,
            Wipe,
            GrowAndShrink
        }

        transitionStyles CurrentTransitionStyle;

        public static ScreenManager Instance
        {
            get
            {
                if (instance == null)
                {
                    // no instance of screen manager has been created yet - get instance from xml file
                    XmlManager<ScreenManager> xml = new XmlManager<ScreenManager>();
                    instance = xml.Load("Load/ScreenManager.xml");
                }

                return instance;
            }
        }

        void SetTransitionValues()
        {
            Image.IsActive = true;
            Image.FadeEffect.Increase = true;
            Image.Alpha = 0.0f;
            IsTransitioning = true;
        }

        public void ChangeScreens(string screenName)
        {
            newScreen = (Screen)Activator.CreateInstance(Type.GetType("MajorProject." + screenName));
            SetTransitionValues();
        }

        public void ChangeScreens(string screenName, bool useOldScreen, bool retainCurrentScreen)
        {
            if (retainCurrentScreen)
            {
                Screen temp = oldScreen;
                oldScreen = currentScreen;
                if (useOldScreen) currentScreen = temp;
                else
                {
                    temp.UnloadContent();
                    ChangeScreens(screenName);
                    return;
                }
                SetTransitionValues();
                return;
            }
            if (useOldScreen)
            {
                currentScreen = oldScreen;
                SetTransitionValues();
                return;
            }
            ChangeScreens(screenName);

        }

        void Transition(GameTime gameTime)
        {
            Image.Update(gameTime);
            if (Image.Alpha >= 1.0f)
            {
                currentScreen.UnloadContent();
                currentScreen = newScreen;
                xmlGameScreenManager.type = currentScreen.Type;
                if (File.Exists(currentScreen.XmlPath))
                    currentScreen = xmlGameScreenManager.Load(currentScreen.XmlPath);
                currentScreen.LoadContent();
            }
            else if (Image.Alpha <= 0.0f)
            {
                Image.IsActive = false;
                IsTransitioning = false;
            }
        }

        ScreenManager()
        {
            currentScreen = new SplashScreen();
            xmlGameScreenManager = new XmlManager<Screen>();
            xmlGameScreenManager.type = currentScreen.Type;
            currentScreen = xmlGameScreenManager.Load("Load/SplashScreen.xml");
        }

        public void LoadContent(ContentManager Content)
        {
            this.Content = new ContentManager(Content.ServiceProvider, "Content");
            currentScreen.LoadContent();
            Image.LoadContent();
        }

        public void UnloadContent()
        {
            currentScreen.UnloadContent();
            Image.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            currentScreen.Update(gameTime);

            if (IsTransitioning)
                Transition(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            currentScreen.Draw(spriteBatch);
            if (IsTransitioning)
                Image.Draw(spriteBatch);
        }
    }
}
