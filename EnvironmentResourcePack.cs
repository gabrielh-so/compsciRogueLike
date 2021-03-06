using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace MajorProject
{
    public class EnvironmentResourcePack : ResourcePack
    {
        // same as resource pack, but inserts the name of the level in the path
        public void LoadContent(string EnvironmentName)
        {

            if (content != null) content.Unload();
            content = new ContentManager(ScreenManager.Instance.Content.ServiceProvider, "Content");

            for (int i = 0; i < TextureNameAndPath.Count - 1; i += 2)
            {
                TexturePack.Add(TextureNameAndPath[i], content.Load<Texture2D>(BaseTexturePath + EnvironmentName + "/" + TextureNameAndPath[i + 1]));
            }
            for (int i = 0; i < AudioNameAndPath.Count - 1; i += 2)
            {
                AudioPack.Add(AudioNameAndPath[i], content.Load<SoundEffect>(BaseAudioPath + EnvironmentName + "/" + AudioNameAndPath[i + 1]));
            }
            for (int i = 0; i < FontNameAndPath.Count - 1; i += 2)
            {
                FontPack.Add(FontNameAndPath[i], content.Load<SpriteFont>(BaseFontPath + FontNameAndPath[i + 1]));
            }
        }
    }
}
