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
using System.IO;

namespace Step_Game
{
    public class Menu
    {
        StepFloor sf;
        private static String MUSIC_PATH = Environment.CurrentDirectory + "\\Musics";
        private static String IMAGE_BACKGROUND_NAME = "background.png";
        private static String IMAGE_COVER_NAME = "cover.png";
        Texture2D background, musicImage;
        private int currentIndex = 0;
        public struct Musica
        {
            public Texture2D background;
            public Texture2D musicImage;
            public String musicPath;
        }
        private String musicPath;
        private List<Musica> musicas = new List<Musica>();

        public Menu()
        {
            sf = new StepFloor(MUSIC_PATH + "\\music");
        }

        KeyboardState oldKb;

        public enum Sense{down, up};

        private void SwitchMusic(Sense sense)
        {
            if (sense == Sense.down)
            {
                if (currentIndex < musicas.Count)
                {
                    currentIndex++;
                    ActivateMusic();
                }
            }
            else
            {
                if (currentIndex > 0)
                {
                    currentIndex--;
                    ActivateMusic();
                }
            }
        }

        private void ActivateMusic()
        {
            Musica m = musicas[currentIndex];
            background = m.background;
            musicImage = m.musicImage;
            musicPath = m.musicPath;

        }

        private void SwitchMusicControl()
        {
            KeyboarState newKb = Keyboard.GetState();

            if (newKb.IsKeyDown(Keys.Down) && oldKb.IsKeyDown(Keys.Down))
            {
                SwitchMusic(Sense.down);
            }

            if (newKb.IsKeyDown(Keys.Up) && oldKb.IsKeyDown(Keys.Up))
            {
                SwitchMusic(Sense.up);
            }

            oldKb = newKb;
        }

        private void LoadMusics()
        {
            DirectoryInfo di = new DirectoryInfo(MUSIC_PATH);
            foreach (DirectoryInfo folder in di.GetDirectories())
            {
                Nova_Importer.LoadExternalResource("BACKGROUND", folder + "\\" + IMAGE_BACKGROUND_NAME, false);
                background = (Texture2D)Nova_DataBase.GetResource("BACKGROUND");

                Nova_Importer.LoadExternalResource("COVER", folder + "\\" + IMAGE_COVER_NAME, false);
                musicImage = (Texture2D)Nova_DataBase.GetResource("COVER");

                String path = folder.FullName;

                Musica musica = new Musica();
                musica.background = background;
                musica.musicImage = musicImage;
                musica.musicPath = path;
            }
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
