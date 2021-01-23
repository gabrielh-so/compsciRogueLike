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
        public Screen oldScreen;

        // if any future employer is reading this please don't think i usually code like this. It's just it's 2:25AM and I just want to get this done
        bool LoadingToPreserve = false;
        bool ScreenPreserved = false;
        bool LoadingFromPreserve = false;
        bool CurrentScreenWasPreserved = false;


        private static ScreenManager instance;
        [XmlIgnore]
        public Vector2 Dimensions { set; get; }
        [XmlIgnore]
        public ContentManager Content { private set; get; }
        XmlManager<Screen> xmlGameScreenManager;
            
        public Screen currentScreen, newScreen;
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
            CurrentScreenWasPreserved = false;
        }

        public void ChangeScreens(string screenName, bool preserveScreen)
        {
            if (preserveScreen)
            {
                if (!CurrentScreenWasPreserved) if (oldScreen != null) oldScreen.UnloadContent();
                oldScreen = currentScreen;
                LoadingToPreserve = true;
            }
            newScreen = (Screen)Activator.CreateInstance(Type.GetType("MajorProject." + screenName));
            SetTransitionValues();
            CurrentScreenWasPreserved = false;
        }

        public void LoadPreservedScreen()
        {
            if (oldScreen == null) return;
            LoadingFromPreserve = true;
            newScreen = oldScreen;
            SetTransitionValues();
        }

        public void UnloadPreservedScreen()
        {
            if (ScreenPreserved) return;
            oldScreen.UnloadContent();
            oldScreen = null;
        }



        void Transition(GameTime gameTime)
        {
            Image.Update(gameTime);
            if (Image.Alpha >= 1.0f)
            {
                if (!LoadingToPreserve)
                    currentScreen.UnloadContent();
                else
                    ScreenPreserved = true;

                currentScreen = newScreen;
                xmlGameScreenManager.type = currentScreen.Type;
                if (!LoadingFromPreserve)
                    if (File.Exists(currentScreen.XmlPath))
                        currentScreen = xmlGameScreenManager.Load(currentScreen.XmlPath);

                if (!LoadingFromPreserve) currentScreen.LoadContent();
                else
                {
                    CurrentScreenWasPreserved = true;
                    ScreenPreserved = false;
                }
                
                LoadingFromPreserve = false;

                LoadingToPreserve = false;
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
            if (IsTransitioning)
                Transition(gameTime);

            currentScreen.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            currentScreen.Draw(spriteBatch);
            if (IsTransitioning)
                Image.Draw(spriteBatch);
        }
    }
}
