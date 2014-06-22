using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    class ErrorScreenGS : GameStateObj
    {
        public override GameStates GameState
        {
            get { return GameStates.ErrorScreen; }
        }

        string ErrorMessage;

        public ErrorScreenGS(string error)
            : base()
        {
            ErrorMessage = error;
        }

        public override void PreSet()
        {
            GeneralFacade.SceneManager.SetScene<SceneStartMenu>();


            DisplayFacade.ScreenElementManager.IsMouseVisible = true;
            float coeffX = (float)GeneralFacade.Game.GraphicsDevice.Viewport.Width / (float)DisplayFacade.BackgroundMenu.Width;
            float coeffY = (float)GeneralFacade.Game.GraphicsDevice.Viewport.Height / (float)DisplayFacade.BackgroundMenu.Height;
            int imageWidth = DisplayFacade.MenuImages["StartMenuPlay"].Width;

            DisplayFacade.ScreenElementManager.AddImage(
                "ErrorScreenMenuBackground",
                0,
                0,
                coeffX,
                coeffY,
                0,
                DisplayFacade.BackgroundMenu,
                null, null, null
            );

            DisplayFacade.ScreenElementManager.AddText(
                "ErrorScreenMenuTitle",
                (int)((540 + 20) * coeffX),
                100,
                1,
                DisplayFacade.TitleFont,
                "Error!",
                Color.White
            );

            DisplayFacade.ScreenElementManager.AddText(
                "ErrorScreenMenuMessage",
                (int)((540 + 20) * coeffX),
                200,
                1,
                DisplayFacade.TableFont,
                ErrorMessage,
                Color.White
            );

            DisplayFacade.ScreenElementManager.AddImage(
                "ErrorScreenMenuMain",
                (int)((510 - imageWidth) * coeffX),
                390,
                coeffX,
                coeffY,
                1,
                DisplayFacade.MenuImages["PauseMenuMain"],
                delegate(ScreenElement el, DeimosGame game)
                {
                    GeneralFacade.GameStateManager.Set(new StartMenuGS());
                },
                delegate(ScreenElement el, DeimosGame game)
                {
                    ScreenImage t = DisplayFacade.ScreenElementManager.GetImage("ErrorScreenMenuMain");
                    t.Image = DisplayFacade.MenuImages["PauseMenuMainHover"];
                },
                delegate(ScreenElement el, DeimosGame game)
                {
                    ScreenImage t = DisplayFacade.ScreenElementManager.GetImage("ErrorScreenMenuMain");
                    t.Image = DisplayFacade.MenuImages["PauseMenuMain"];
                }
            );
        }

        public override void PostUnset()
        {
            DisplayFacade.ScreenElementManager.RemoveImage("ErrorScreenMenuBackground");
            DisplayFacade.ScreenElementManager.RemoveText("ErrorScreenMenuTitle");
            DisplayFacade.ScreenElementManager.RemoveText("ErrorScreenMenuMessage");
            DisplayFacade.ScreenElementManager.RemoveImage("ErrorScreenMenuMain");
        }
    }
}
