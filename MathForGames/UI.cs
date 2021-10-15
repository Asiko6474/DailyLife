using System;
using System.Collections.Generic;
using System.Text;

namespace MathForGames
{
    class UI : Actor
    {
        private string _text;
        private int _width;
        private int _height;
        

        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }

        public UI(float x, float y, string name, ConsoleColor color, int width, int height, string text = "") : base('\0', x, y, name, color)
        {
            _text = text;
            _width = width;
            _height = height;
        }

        public override void Draw()
        {
            int cursorPosX = (int)Position.x;
            int cursorPosY = (int)Position.y;

            Icon currentletter = new Icon { Color = Icon.Color };

            char[] textChars = Text.ToCharArray();

            for (int i = 0; i < textChars.Length; i++)
            {
                currentletter.Symbol = textChars[i];

                if (currentletter.Symbol == '\n')
                {
                    cursorPosX = (int)Position.x;
                    cursorPosY++;
                    continue;
                }
                Engine.Render(currentletter, new MathLibrary.Vector2 { x = cursorPosX, y = cursorPosY });

                cursorPosX++;

                if (cursorPosX - (int)Position.x > Width)
                {
                    cursorPosX = (int)Position.x;
                    cursorPosY++;
                }
                if (cursorPosY - (int)Position.y > Height)
                {
                    break;
                }

            }     
        }
    }
}
