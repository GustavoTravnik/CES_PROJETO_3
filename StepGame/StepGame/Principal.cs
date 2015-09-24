using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using NovaDll;

namespace Step_Game
{

    public class Principal : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch render;
        Menu menu;

        public Principal()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Nova_Functions.SetGraphics(graphics);
            Nova_Functions.SetGame(this);
           // Nova_Functions.ConfigureGraphics(false, true, false, SurfaceFormat.Color, DepthFormat.None);
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            render = new SpriteBatch(GraphicsDevice);
            Nova_Importer.SetContent(Content);
            Nova_Functions.SetViewport(GraphicsDevice);
            Nova_Functions.AdjustAspectRatio(this);
           // Nova_Functions.GetWindowsFormFrom(this.Window.Handle).Opacity = 0.05f;
            //Nova_Functions.GoToFullScreen();
            menu = new Menu();
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            menu.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            render.Begin(0, BlendState.NonPremultiplied);
            menu.Draw(render);
            render.End();
        }
    }
}
