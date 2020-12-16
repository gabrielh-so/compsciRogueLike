using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MajorProject
{
    public class InputManager
    {
        KeyboardState currentKeyState, prevKeyState;
        MouseState currentMouseState, prevMouseState;
        public bool QuitSignaled;

        bool ChangedKeysCalculated;
        List<Keys> changedKeys;

        public enum ActionType
        {
            walk_up,
            walk_right,
            walk_down,
            walk_left,
            shoot,
            pick_up,
            open_inventory,
            use_potion
            // aiming using mouse and selecting weapons using the number keys is unchangable
        }

        public Dictionary<ActionType, Keys> ActionKeyDict;

        private static InputManager instance;

        public static InputManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new InputManager();

                return instance;
            }
        }

        public InputManager()
        {
            UpdateKeyDictionary();
            changedKeys = new List<Keys>();
        }

        public void UpdateKeyDictionary()
        {
            ActionKeyDict = PlayerPreferences.Instance.ActionKeyDict;
        }

        public void Update()
        {
            ChangedKeysCalculated = false;
            prevKeyState = currentKeyState;
            prevMouseState = currentMouseState;
            if (!ScreenManager.Instance.IsTransitioning)
            {
                currentKeyState = Keyboard.GetState();
                currentMouseState = Mouse.GetState();
            }
        }

        public bool ActionKeyDown(ActionType action)
        {
            return KeyDown(ActionKeyDict[action]);
        }

        public bool ActionKeyPressed(ActionType action)
        {
            return KeyPressed(ActionKeyDict[action]);
        }




        public List<Keys> GetChangedKeys()
        {
            if (ChangedKeysCalculated) return changedKeys;
            changedKeys = new List<Keys>();
            Keys[] pressedKeys = currentKeyState.GetPressedKeys();
            foreach (Keys k in pressedKeys)
            {
                if (prevKeyState.IsKeyUp(k)) changedKeys.Add(k);
            }
            ChangedKeysCalculated = true;
            return changedKeys;
        }

        public int GetChangedKeysCount()
        {
            if (ChangedKeysCalculated) return changedKeys.Count;
            return GetChangedKeys().Count;
        }

        public Point GetMousePosition()
        {
            return currentMouseState.Position;
        }

        public bool MouseMoved()
        {
            return currentMouseState.Position != prevMouseState.Position;
        }

        public bool MousePressed()
        {
            return (currentMouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released);
        }

        public bool MouseDown()
        {
            return currentMouseState.LeftButton == ButtonState.Pressed;
        }
        public bool MouseUp()
        {
            return currentMouseState.LeftButton == ButtonState.Released;
        }

        public bool MouseReleased()
        {
            return (prevMouseState.LeftButton == ButtonState.Pressed && currentMouseState.LeftButton == ButtonState.Released);
        }

        public bool KeyPressed(params Keys[] keys)
        {
            foreach (Keys key in keys)
            {
                if (currentKeyState.IsKeyDown(key) && prevKeyState.IsKeyUp(key))
                    return true;
            }
            return false;
        }

        public bool KeyReleased(params Keys[] keys)
        {
            foreach (Keys key in keys)
            {
                if (currentKeyState.IsKeyUp(key) && prevKeyState.IsKeyDown(key))
                    return true;
            }
            return false;
        }

        public bool KeyDown(params Keys[] keys)
        {
            foreach (Keys key in keys)
            {
                if (currentKeyState.IsKeyDown(key))
                    return true;
            }
            return false;
        }
    }
}
