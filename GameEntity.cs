using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MajorProject
{
    public abstract class GameEntity
    {
        public Type type;

        public Vector2 position;
        public Vector2 velocity;

        public Rectangle BoundingBox;
        public Point SpriteSize;

        // reference to a resource pack // all textures/sounds can be accessed from here
        public ResourcePack Resources;


        public GameEntity()
        {
            position = new Vector2();
            velocity = new Vector2();

            
        }

        public void SetPosition(float x, float y)
        {
            position.X = x;
            position.Y = y;
        }

        // content within the pack is already loaded, so just assign
        public virtual void LoadContent(ref ResourcePack resources)
        {
            Resources = resources;
        }

        public virtual void UnloadContent()
        {
            // unlink resources thingy
            Resources = null;
        }

        public abstract void Update(GameTime gameTime); // all game entities must have an update loop of some sort - otherwise they're probably pointless

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            
        }


    }
}
