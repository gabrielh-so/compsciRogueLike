using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MajorProject
{
    public class MiniMap
    {
        public Point position;
        public Point SpriteSize;
        public Point TargetPosition;
        public Point ViewSize;

        public ResourcePack Resources;

        Texture2D MiniMapTexture;

        public int[,] Map;

        int tileSize = 2;

        public void Update(GameTime gameTime)
        {

        }

        public void RenderTexture()
        {

            Vector2 dimensions = new Vector2();

            dimensions.X = tileSize * Map.GetLength(0);

            dimensions.Y = tileSize * Map.GetLength(1);

            RenderTarget2D renderTarget = new RenderTarget2D(ScreenManager.Instance.GraphicsDevice,
                (int)dimensions.X, (int)dimensions.Y);

            ScreenManager.Instance.GraphicsDevice.SetRenderTarget(renderTarget);
            ScreenManager.Instance.GraphicsDevice.Clear(Color.Transparent);

            // doesn't apply anti-aliasing
            ScreenManager.Instance.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

            for (int y = 0; y < Map.GetLength(0); y++)
            {
                for (int x = 0; x < Map.GetLength(1); x++)
                {
                    int originPointX = tileSize * x;
                    int originPointY = tileSize * y;

                    if (Map[y, x] > 0) // can't draw nothing
                        ScreenManager.Instance.SpriteBatch.Draw(Resources.TexturePack[Map[y, x].ToString()], destinationRectangle: new Rectangle(originPointX, originPointY, tileSize, tileSize));

                }
            }

            ScreenManager.Instance.SpriteBatch.End();

            MiniMapTexture = renderTarget;


            ScreenManager.Instance.GraphicsDevice.SetRenderTarget(null); // reset rendertarget to default - main display rendertarget

        }

        public void LoadContent(ref ResourcePack resources)
        {
            Resources = resources;
        }

        public void UnloadContent()
        {
            Resources = null;
            MiniMapTexture.Dispose();
        }

        public MiniMap()
        {
            tileSize = 2;
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            // restart spritebatch to apply anti-aliasing settings
            spriteBatch.End();

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            Point TargetOffset = new Point();
            TargetOffset.X = TargetPosition.X / 25;
            TargetOffset.Y = TargetPosition.Y / 25;

            TargetOffset.X -= ViewSize.X / 2;
            TargetOffset.Y -= ViewSize.Y / 2;
            spriteBatch.Draw(MiniMapTexture, sourceRectangle: new Rectangle(TargetOffset, ViewSize), destinationRectangle: new Rectangle(position, SpriteSize));

            // restart spritebatch using normal settings

            spriteBatch.End();

            spriteBatch.Begin();
        }



    }
}
