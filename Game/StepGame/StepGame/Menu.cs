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
            Loader.LoadFonts();
            LoadMusics();
            ActivateMusic();
            scoreFont = (SpriteFont)Nova_DataBase.GetResource("FONT_MENU");
        }

        SpriteFont scoreFont;

        KeyboardState oldKb;

        public enum Sense{down, up};

        private void SwitchMusic(Sense sense)
        {
            if (sense == Sense.down)
            {
                if (currentIndex < musicas.Count-1)
                {
                    currentIndex++;
                }
            }
            else
            {
                if (currentIndex > 0)
                {
                    currentIndex--;
                }
            }
            ActivateMusic();
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
            KeyboardState newKb = Keyboard.GetState();

            if (newKb.IsKeyDown(Keys.Down) && oldKb.IsKeyUp(Keys.Down))
            {
                SwitchMusic(Sense.down);
            }

            if (newKb.IsKeyDown(Keys.Up) && oldKb.IsKeyUp(Keys.Up))
            {
                SwitchMusic(Sense.up);
            }

            if (newKb.IsKeyDown(Keys.Enter) && oldKb.IsKeyUp(Keys.Enter))
            {
                sf = new StepFloor(musicas[currentIndex].musicPath, background);
            }

            oldKb = newKb;
        }

        private void LoadMusics()
        {
            DirectoryInfo di = new DirectoryInfo(MUSIC_PATH);
            foreach (DirectoryInfo folder in di.GetDirectories())
            {
                Musica musica = new Musica();

                if (!Directory.Exists(Nova_Importer.Content.RootDirectory + "\\" + folder.Name))
                {
                    Directory.CreateDirectory(Nova_Importer.Content.RootDirectory + "\\" + folder.Name);
                }


                if (!File.Exists(Nova_Importer.Content.RootDirectory + "\\" + folder.Name + "\\background.xnb"))
                {
                    Nova_Importer.LoadExternResource(folder.FullName + "\\" + IMAGE_BACKGROUND_NAME, true, "BACKGROUND");
                    File.Copy(Nova_Importer.Content.RootDirectory + "\\Externs\\BACKGROUND.xnb", Nova_Importer.Content.RootDirectory + "\\" + folder.Name + "\\background.xnb");
                }
                Nova_Importer.LoadResource("BACKGROUND", folder.Name + "\\BACKGROUND");
                musica.background = (Texture2D)Nova_DataBase.GetResource("BACKGROUND");


                
                if (!File.Exists(Nova_Importer.Content.RootDirectory + "\\" + folder.Name + "\\cover.xnb"))
                {
                    Nova_Importer.LoadExternResource(folder.FullName + "\\" + IMAGE_COVER_NAME, true, "COVER");
                    File.Copy(Nova_Importer.Content.RootDirectory + "\\Externs\\COVER.xnb", Nova_Importer.Content.RootDirectory + "\\" + folder.Name + "\\cover.xnb");
                }
                Nova_Importer.LoadResource("COVER", folder.Name + "\\COVER");
                musica.musicImage = (Texture2D)Nova_DataBase.GetResource("COVER");

                musica.musicPath = folder.FullName;
                musicas.Add(musica);
            }
        }

        private void DrawImages(SpriteBatch render)
        {
            render.Draw(background, Nova_Functions.ReturnScreenRectangle(), Color.White);
            render.Draw(musicImage, Nova_Functions.CreateRectangle(Nova_Functions.GetCenterVector(Nova_Functions.CreateRectangle(Vector2.Zero, 400, 200), Nova_Functions.ReturnScreenRectangle()), 400, 200), Color.White);
        }

        public void Update(GameTime gameTime)
        {
            SwitchMusicControl();
            if (sf != null)
            {
                sf.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch render)
        {
            DrawImages(render);
            render.DrawString(scoreFont,musicas[currentIndex].musicPath.Split('\\')[musicas[currentIndex].musicPath.Split('\\').Length-1], new Vector2(0, 0), Color.White);
            if (sf != null)
            {
                sf.Draw(render);
            }
        }
    }
}
