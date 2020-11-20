using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;


namespace MajorProject
{
    
    public class ResourcePack
    {
        [XmlIgnore]
        public Dictionary<string, Texture2D> TexturePack;
        [XmlIgnore]
        public Dictionary<string, SoundEffect> AudioPack;

        [XmlIgnore]
        public Dictionary<string, string> TextureFileNames;
        [XmlIgnore]
        public Dictionary<string, string> AudioFileNames;

        public List<string> TextureNameAndPath;
        public List<string> AudioNameAndPath;

        public string BaseAudioPath;
        public string BaseTexturePath;

        protected ContentManager content;


        public ResourcePack()
        {
            TexturePack = new Dictionary<string, Texture2D>();
            AudioPack = new Dictionary<string, SoundEffect>();
        }

        public virtual void LoadContent()
        {
            if (content != null) content.Unload();
            content = new ContentManager(ScreenManager.Instance.Content.ServiceProvider, "Content");

            for (int i = 0; i < TextureNameAndPath.Count - 1; i += 2)
            {
                TexturePack.Add(TextureNameAndPath[i], content.Load<Texture2D>(BaseTexturePath + TextureNameAndPath[i + 1]));
            }
            for (int i = 0; i < AudioPack.Count - 1; i += 2)
            {
                AudioPack.Add(TextureNameAndPath[i], content.Load<SoundEffect>(BaseAudioPath + AudioNameAndPath[i + 1]));
            }

        }

        public void UnloadContent()
        {
            content.Unload();
            content.Dispose();

            // unload content tied to texture objects
            foreach (Texture2D t in TexturePack.Values)
            {
                t.Dispose();
            }
            foreach (SoundEffect t in AudioPack.Values)
            {
                t.Dispose();
            }
            TexturePack.Clear();
            AudioPack.Clear();

        }



    }
}
