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
    public class Menu
    {
        StepFloor sf;
        private static String MUSIC_PATH = Environment.CurrentDirectory + "\\Musics";

        public Menu()
        {
            sf = new StepFloor(MUSIC_PATH + "\\music");
        }

        public void Update(GameTime gameTime)
        {
            if (sf != null)
            {
                sf.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch render)
        {
            if (sf != null)
            {
                sf.Draw(render);
            }
        }
    }
}
