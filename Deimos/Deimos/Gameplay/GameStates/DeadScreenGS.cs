using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    class DeadScreenGS : GameStateObj
    {
        public override GameStates GameState
        {
            get { return GameStates.DeadScreen; }
        }

        public override void PreSet()
        {
            DisplayFacade.ScreenElementManager.IsMouseVisible = true;
            int width = GeneralFacade.Game.GraphicsDevice.Viewport.Width;
            int height = GeneralFacade.Game.GraphicsDevice.Viewport.Height;

            Texture2D bg = new Texture2D(GeneralFacade.Game.GraphicsDevice, 1, 1);
            bg.SetData(new Color[] { Color.Black });
            DisplayFacade.ScreenElementManager.AddImage(
                "DeadScreenBackground",
                0,
                0,
                width,
                height,
                0,
                bg,
                null,
                null,
                null
            );

            DisplayFacade.ScreenElementManager.AddText(
                "DeadScreenTitle",
                (int)((width - DisplayFacade.TitleFont.MeasureString("You have been killed.").X) / 2),
                150,
                1,
                DisplayFacade.TitleFont,
                "You have been killed.",
                Color.White
            );
            DisplayFacade.ScreenElementManager.AddText(
                "DeadScreenSubTitle",
                (int)((width - DisplayFacade.TableFont.MeasureString("Please choose which class to respawn with.").X) / 2),
                190,
                1,
                DisplayFacade.TableFont,
                "Please choose which class to respawn with.",
                Color.LightGray
            );

            int buttonWidth = (int)(DisplayFacade.ButtonsImages["DeadScreenGood"].Width / 2);
            int offset = ((width / 3) - buttonWidth) / 2;
            DisplayFacade.ScreenElementManager.AddImage(
                "DeadScreenGood",
                offset,
                430,
                0.5f,
                0.5f,
                1,
                DisplayFacade.ButtonsImages["DeadScreenGood"],
                delegate(ScreenElement el, DeimosGame game)
                {
                    GameplayFacade.ThisPlayer.Class = Player.Spec.Soldier;
                    GeneralFacade.GameStateManager.Set(new RespawnGS(GameplayFacade.ThisPlayer.NextInstance, false));
                    GeneralFacade.GameStateManager.Set(new PlayingGS());
                },
                delegate(ScreenElement el, DeimosGame game)
                {
                    ScreenImage t = DisplayFacade.ScreenElementManager.GetImage("DeadScreenGood");
                    t.Image = DisplayFacade.ButtonsImages["DeadScreenGoodHover"];
                },
                delegate(ScreenElement el, DeimosGame game)
                {
                    ScreenImage t = DisplayFacade.ScreenElementManager.GetImage("DeadScreenGood");
                    t.Image = DisplayFacade.ButtonsImages["DeadScreenGood"];
                }
            );
            DisplayFacade.ScreenElementManager.AddImage(
                "DeadScreenBad",
                (width / 3) + offset,
                430,
                0.5f,
                0.5f,
                1,
                DisplayFacade.ButtonsImages["DeadScreenBad"],
                delegate(ScreenElement el, DeimosGame game)
                {
                    GameplayFacade.ThisPlayer.Class = Player.Spec.Agent;
                    GeneralFacade.GameStateManager.Set(new RespawnGS(GameplayFacade.ThisPlayer.NextInstance, false));
                    GeneralFacade.GameStateManager.Set(new PlayingGS());
                },
                delegate(ScreenElement el, DeimosGame game)
                {
                    ScreenImage t = DisplayFacade.ScreenElementManager.GetImage("DeadScreenBad");
                    t.Image = DisplayFacade.ButtonsImages["DeadScreenBadHover"];
                },
                delegate(ScreenElement el, DeimosGame game)
                {
                    ScreenImage t = DisplayFacade.ScreenElementManager.GetImage("DeadScreenBad");
                    t.Image = DisplayFacade.ButtonsImages["DeadScreenBad"];
                }
            );
            DisplayFacade.ScreenElementManager.AddImage(
                "DeadScreenUgly",
                (width / 3) * 2 + offset,
                430,
                0.5f,
                0.5f,
                1,
                DisplayFacade.ButtonsImages["DeadScreenUgly"],
                delegate(ScreenElement el, DeimosGame game)
                {
                    GameplayFacade.ThisPlayer.Class = Player.Spec.Overwatch;
                    GeneralFacade.GameStateManager.Set(new RespawnGS(GameplayFacade.ThisPlayer.NextInstance, false));
                    GeneralFacade.GameStateManager.Set(new PlayingGS());
                },
                delegate(ScreenElement el, DeimosGame game)
                {
                    ScreenImage t = DisplayFacade.ScreenElementManager.GetImage("DeadScreenUgly");
                    t.Image = DisplayFacade.ButtonsImages["DeadScreenUglyHover"];
                },
                delegate(ScreenElement el, DeimosGame game)
                {
                    ScreenImage t = DisplayFacade.ScreenElementManager.GetImage("DeadScreenUgly");
                    t.Image = DisplayFacade.ButtonsImages["DeadScreenUgly"];
                }
            );
        }

        public override void PostUnset()
        {
            DisplayFacade.ScreenElementManager.RemoveImage("DeadScreenBackground");
            DisplayFacade.ScreenElementManager.RemoveText("DeadScreenTitle");
            DisplayFacade.ScreenElementManager.RemoveText("DeadScreenSubTitle");
            DisplayFacade.ScreenElementManager.RemoveImage("DeadScreenGood");
            DisplayFacade.ScreenElementManager.RemoveImage("DeadScreenBad");
            DisplayFacade.ScreenElementManager.RemoveImage("DeadScreenUgly");
        }
    }
}
