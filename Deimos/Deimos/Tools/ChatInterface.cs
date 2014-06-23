using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    class ChatInterface
    {
        public bool InputChat = false;
        private List<string> ChatInputs = new List<string>();
        private Keys[] OldInputs = { Keys.Y };
        private string CurrentInput = "";
        private float CountDown = 0;
        const int NumberLinesChat = 10;
        const int CharsPerLine = 100;
        const int MarginLeft = 20;
        const int MarginBottom = 100;

        public ChatInterface()
        {
            AddChatInput("Manu: Coucou!");
            AddChatInput("Erenus: La forme?");
            AddChatInput("Manu: Impeccable! Et en plus je sais dire plus d'un mot!");
        }

        private bool GetChar(Keys input, out char key)
        {
            KeyboardState ks = Keyboard.GetState();
            bool shift = ks.IsKeyDown(Keys.LeftShift) || ks.IsKeyDown(Keys.RightShift);

            switch (input)
            {
                //Alphabet keys
                case Keys.A: if (shift) { key = 'A'; } else { key = 'a'; } return true;
                case Keys.B: if (shift) { key = 'B'; } else { key = 'b'; } return true;
                case Keys.C: if (shift) { key = 'C'; } else { key = 'c'; } return true;
                case Keys.D: if (shift) { key = 'D'; } else { key = 'd'; } return true;
                case Keys.E: if (shift) { key = 'E'; } else { key = 'e'; } return true;
                case Keys.F: if (shift) { key = 'F'; } else { key = 'f'; } return true;
                case Keys.G: if (shift) { key = 'G'; } else { key = 'g'; } return true;
                case Keys.H: if (shift) { key = 'H'; } else { key = 'h'; } return true;
                case Keys.I: if (shift) { key = 'I'; } else { key = 'i'; } return true;
                case Keys.J: if (shift) { key = 'J'; } else { key = 'j'; } return true;
                case Keys.K: if (shift) { key = 'K'; } else { key = 'k'; } return true;
                case Keys.L: if (shift) { key = 'L'; } else { key = 'l'; } return true;
                case Keys.M: if (shift) { key = 'M'; } else { key = 'm'; } return true;
                case Keys.N: if (shift) { key = 'N'; } else { key = 'n'; } return true;
                case Keys.O: if (shift) { key = 'O'; } else { key = 'o'; } return true;
                case Keys.P: if (shift) { key = 'P'; } else { key = 'p'; } return true;
                case Keys.Q: if (shift) { key = 'Q'; } else { key = 'q'; } return true;
                case Keys.R: if (shift) { key = 'R'; } else { key = 'r'; } return true;
                case Keys.S: if (shift) { key = 'S'; } else { key = 's'; } return true;
                case Keys.T: if (shift) { key = 'T'; } else { key = 't'; } return true;
                case Keys.U: if (shift) { key = 'U'; } else { key = 'u'; } return true;
                case Keys.V: if (shift) { key = 'V'; } else { key = 'v'; } return true;
                case Keys.W: if (shift) { key = 'W'; } else { key = 'w'; } return true;
                case Keys.X: if (shift) { key = 'X'; } else { key = 'x'; } return true;
                case Keys.Y: if (shift) { key = 'Y'; } else { key = 'y'; } return true;
                case Keys.Z: if (shift) { key = 'Z'; } else { key = 'z'; } return true;

                //Decimal keys
                case Keys.D0: if (shift) { key = ')'; } else { key = '0'; } return true;
                case Keys.D1: if (shift) { key = '!'; } else { key = '1'; } return true;
                case Keys.D2: if (shift) { key = '@'; } else { key = '2'; } return true;
                case Keys.D3: if (shift) { key = '#'; } else { key = '3'; } return true;
                case Keys.D4: if (shift) { key = '$'; } else { key = '4'; } return true;
                case Keys.D5: if (shift) { key = '%'; } else { key = '5'; } return true;
                case Keys.D6: if (shift) { key = '^'; } else { key = '6'; } return true;
                case Keys.D7: if (shift) { key = '&'; } else { key = '7'; } return true;
                case Keys.D8: if (shift) { key = '*'; } else { key = '8'; } return true;
                case Keys.D9: if (shift) { key = '('; } else { key = '9'; } return true;

                //Decimal numpad keys
                case Keys.NumPad0: key = '0'; return true;
                case Keys.NumPad1: key = '1'; return true;
                case Keys.NumPad2: key = '2'; return true;
                case Keys.NumPad3: key = '3'; return true;
                case Keys.NumPad4: key = '4'; return true;
                case Keys.NumPad5: key = '5'; return true;
                case Keys.NumPad6: key = '6'; return true;
                case Keys.NumPad7: key = '7'; return true;
                case Keys.NumPad8: key = '8'; return true;
                case Keys.NumPad9: key = '9'; return true;

                //Special keys
                case Keys.OemTilde: if (shift) { key = '~'; } else { key = '`'; } return true;
                case Keys.OemSemicolon: if (shift) { key = ':'; } else { key = ';'; } return true;
                case Keys.OemQuotes: if (shift) { key = '"'; } else { key = '\''; } return true;
                case Keys.OemQuestion: if (shift) { key = '?'; } else { key = '/'; } return true;
                case Keys.OemPlus: if (shift) { key = '+'; } else { key = '='; } return true;
                case Keys.OemPipe: if (shift) { key = '|'; } else { key = '\\'; } return true;
                case Keys.OemPeriod: if (shift) { key = '>'; } else { key = '.'; } return true;
                case Keys.OemOpenBrackets: if (shift) { key = '{'; } else { key = '['; } return true;
                case Keys.OemCloseBrackets: if (shift) { key = '}'; } else { key = ']'; } return true;
                case Keys.OemMinus: if (shift) { key = '_'; } else { key = '-'; } return true;
                case Keys.OemComma: if (shift) { key = '<'; } else { key = ','; } return true;
                case Keys.Space: key = ' '; return true;
            }

            key = (char)0;
            return false;
        }

        public void AddChatInput(string input)
        {
            CountDown = 5;
            ChatInputs.Add(input);
        }

        public void HandleInput(GameTime gameTime)
        {
            if (CountDown > 0)
            {
                CountDown -= (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
            }

            if (!InputChat)
            {
                return;
            }

            KeyboardState ks = Keyboard.GetState();
            Keys[] keysPressed = ks.GetPressedKeys();

            foreach (var k in keysPressed)
            {
                if (CurrentInput.Count() >= CharsPerLine)
                {
                    continue; // In case there is a return key
                }

                if (OldInputs.Contains(k))
                {
                    continue; // Makes it non-spammable
                }
                char thisChar;
                if (GetChar(k, out thisChar))
                {
                    CurrentInput += thisChar;
                }

                if (k == Keys.Back && CurrentInput.Count() > 0)
                {
                    CurrentInput = CurrentInput.Remove(CurrentInput.Count() - 1);
                }

                if (ks.IsKeyDown(Keys.Enter))
                {
                    if (CurrentInput != "")
                    {
                        // Do the needed stuff to send the chat message here with
                        // the variable CurrentInput
                        AddChatInput(CurrentInput); // testing
                    }
                    

                    OldInputs = new Keys[] { Keys.Y };
                    CurrentInput = "";
                    InputChat = false;
                    return;
                }
            }

            OldInputs = keysPressed;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!InputChat && CountDown <= 0)
            {
                return;
            }

            spriteBatch.Begin();

            int width = GeneralFacade.Game.GraphicsDevice.Viewport.Width;
            int height = GeneralFacade.Game.GraphicsDevice.Viewport.Height;
            int fontWidth = (int)DisplayFacade.ChatFont.MeasureString("a").X;
            int fontHeight = (int)DisplayFacade.ChatFont.MeasureString("jL").Y;


            spriteBatch.Draw(
                DisplayFacade.ScreenElementManager.DummyTexture,
                new Rectangle(
                    MarginLeft, 
                    height - MarginBottom - (fontHeight * NumberLinesChat), 
                    fontWidth * CharsPerLine, 
                    fontHeight * NumberLinesChat
                ), 
                new Color(0, 0, 0, 0.5f)
            );


            for (int i = NumberLinesChat; i > 0; i--)
            {
                if (ChatInputs.Count() - i < 0)
                {
                    continue;
                }
                string t = ChatInputs[ChatInputs.Count() - i];
                spriteBatch.DrawString(
                    DisplayFacade.ChatFont,
                    HelperFacade.Helpers.Truncate(t, CharsPerLine - 3, "..."),
                    new Vector2(MarginLeft, height - MarginBottom - (fontHeight * i)),
                    Color.White
                );
            }

            if (InputChat)
            {
                spriteBatch.Draw(
                    DisplayFacade.ScreenElementManager.DummyTexture,
                    new Rectangle(
                       MarginLeft,
                       height - MarginBottom,
                       fontWidth * CharsPerLine, 
                       fontHeight + 6
                    ),
                    new Color(0, 0, 0, 0.9f)
                );
                spriteBatch.DrawString(
                    DisplayFacade.ChatFont,
                    CurrentInput,
                    new Vector2(MarginLeft, height - MarginBottom + 3),
                    Color.White
                );
            }

            spriteBatch.End();
        }
    }
}
