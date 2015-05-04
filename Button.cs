using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

namespace UGWProjCode
{
    class Button
    {
        // button attributes
        Texture2D texture;
        Vector2 position;
        Rectangle rectangle;
        Color color = new Color(255, 255, 255, 255);
        public Vector2 size;

        // initialize texture + size
        public Button(Texture2D newTexture, GraphicsDevice graphics)
        {
            texture = newTexture;
            size = new Vector2(graphics.Viewport.Width / 8, graphics.Viewport.Height / 30);
        }

        // check if mouse is interacting with button
        bool down;
        public bool isClicked;

        // update the buttons
        public void Update(MouseState mouse)
        {
            rectangle = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            Rectangle mRectangle = new Rectangle(mouse.X, mouse.Y, 1, 1);

            // mouse intersects rectangle changes button color slightly
            if (mRectangle.Intersects(rectangle))
            {
                if (color.A == 255) down = false;
                if (color.A == 0) down = true;
                if (down) color.A += 3;
                else color.A -= 3;

                // clicked button
                if (mouse.LeftButton == ButtonState.Pressed)
                {
                    isClicked = true;
                }
                else
                {
                    isClicked = false;
                }
            }
            else if (color.A < 255)
            {
                color.A += 3;
                isClicked = false;
            }
        }

        // position of button
        public void setPosition(Vector2 newPosition)
        {
            position = newPosition;
        }

        // draw button
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rectangle, color);
        }
    }
}
