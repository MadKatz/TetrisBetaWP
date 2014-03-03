using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TetrisBetaWP
{
    class Sprite
    {
        private Texture2D textureImage;
        private Rectangle source_Rect;
        public Sprite(Texture2D texture, int source_x, int source_y, int width, int height)
        {
            textureImage = texture;
            source_Rect.X = source_x;
            source_Rect.Y = source_y;
            source_Rect.Width = width;
            source_Rect.Height = height;
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        public void Draw(SpriteBatch spriteBatch, int x, int y)
        {
            Rectangle destination_Rect = new Rectangle(x, y, source_Rect.Width, source_Rect.Height);
            spriteBatch.Draw(textureImage, destination_Rect, source_Rect, Color.White);
        }
    }
}
