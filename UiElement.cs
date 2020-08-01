using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using System.Xml.Serialization;

namespace MajorProject
{
    public abstract class UiElement
    {

        public Vector2 Position;
        public string Description;

        public UiElement()
        {

        }

        public virtual void LoadContent()
        {

        }

        public virtual void UnloadContent()
        {
            
        }

        public virtual void Update(GameTime gameTime)
        {
            
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            
        }

        public delegate void onActivate(UiElement triggeredObject);
        [XmlIgnore]
        public onActivate OnActivate;

        public delegate void onActivateF(UiElement triggeredObject, float value);
        [XmlIgnore]
        public onActivateF OnActivateF;

        public delegate void onHover(UiElement triggeredObject);
        [XmlIgnore]
        public onHover OnHover;

        public delegate void onStopHover(UiElement triggeredObject);
        [XmlIgnore]
        public onHover OnStopHover;
    }
}
