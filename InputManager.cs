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

        Dictionary<ActionType, Keys> ActionKeyDict;

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
            ActionKeyDict = PlayerPreferences.Instance.ActionKeyDict;
        }

        public void Update()
        {
            prevKeyState = currentKeyState;
            prevMouseState = currentMouseState;
            if (!ScreenManager.Instance.IsTransitioning)
            {
                currentKeyState = Keyboard.GetState();
                currentMouseState = Mouse.GetState();
            }
        }

        public List<Keys> GetChangedKeys()
        {
            List<Keys> changedKeys = new List<Keys>();
            Keys[] pressedKeys = currentKeyState.GetPressedKeys();
            foreach (Keys k in pressedKeys)
            {
                if (prevKeyState.IsKeyUp(k)) changedKeys.Add(k);
            }
            return changedKeys;
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
