using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace MajorProject
{
    public class ImageEffect
    {
        protected Image image;
        public bool IsActive;

        public ImageEffect()
        {
            IsActive = false;
        }

        public virtual void LoadContent(ref Image Image)
        {
            this.image = Image; // this is a cyclic reference, so isn't best practice since it can lead to serialization issues
            // the c# standard xml serializer accounts for cyclic references, so ease of use is more important
        }

        public virtual void UnloadContent()
        {
        }

        public virtual void Update(GameTime gameTime)
        {

        }
    }
}
