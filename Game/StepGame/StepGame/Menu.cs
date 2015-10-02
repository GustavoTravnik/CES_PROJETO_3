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
        private static String IMAGE_BACKGROUND_NAME = "background";
        private static String IMAGE_COVER_NAME = "cover";
        private static String MUSIC_FILE_NAME = "music";
        Texture2D background, musicImage, coverBackground, scoreBackground;
        private int currentIndex = 0;
        private Boolean isInGame = false;

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
            coverBackground = (Texture2D)Nova_DataBase.GetResource("MENU_BACKGROUND");
            scoreBackground = (Texture2D)Nova_DataBase.GetResource("SCORE_BACKGROUND");
        }

        SpriteFont scoreFont;

        private String totalNotesOk, totalNotes, porcentagem;
        private static String SCORE_FILE = "score.rd";

        KeyboardState oldKb;

        public enum Sense{down, up};

        private void LoadScore(String directory)
        {
            if (File.Exists(musicas[currentIndex].musicPath + "\\" + SCORE_FILE))
            {
                StreamReader sr = new StreamReader(musicas[currentIndex].musicPath + "\\" + SCORE_FILE);
                totalNotesOk = sr.ReadLine();
                totalNotes = sr.ReadLine();
                Double percent = Convert.ToDouble(totalNotesOk) / Convert.ToDouble(totalNotes) * 100;
                porcentagem = Nova_Functions.FormatNumber(percent, 2) + "%";
            }
        }

        private void SaveScore()
        {
            Boolean overwrite = false;
            if (File.Exists(musicas[currentIndex].musicPath + "\\" + SCORE_FILE))
            {
                StreamReader sr = new StreamReader(musicas[currentIndex].musicPath + "\\" + SCORE_FILE);
                totalNotesOk = sr.ReadLine();
                if (sf.notesOk > Convert.ToInt32(totalNotesOk))
                {
                    overwrite = true;
                }
            }
        }

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

            if (!isInGame)
            {
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
                    isInGame = true;
                    sf = new StepFloor(musicas[currentIndex].musicPath, background, scoreBackground);
                }
            }
            else
            {
                if (newKb.IsKeyDown(Keys.Escape) && oldKb.IsKeyUp(Keys.Escape))
                {
                    isInGame = false;
                    sf = null;
                    MediaPlayer.Stop();
                }
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
                    String extension = "";
                    if (File.Exists(folder.FullName + "\\" + IMAGE_BACKGROUND_NAME + ".jpg"))
                    {
                        extension = ".jpg";
                    }
                    else
                    {
                        extension = ".png";
                    }
                    Nova_Importer.LoadExternResource(folder.FullName + "\\" + IMAGE_BACKGROUND_NAME + extension, true, "BACKGROUND");
                    File.Copy(Nova_Importer.Content.RootDirectory + "\\Externs\\BACKGROUND.xnb", Nova_Importer.Content.RootDirectory + "\\" + folder.Name + "\\background.xnb");
                }
                Nova_Importer.LoadResource("BACKGROUND", folder.Name + "\\BACKGROUND");
                musica.background = (Texture2D)Nova_DataBase.GetResource("BACKGROUND");

                if (!File.Exists(Nova_Importer.Content.RootDirectory + "\\" + folder.Name + "\\cover.xnb"))
                {
                    String extension = "";
                    if (File.Exists(folder.FullName + "\\" + IMAGE_COVER_NAME + ".jpg"))
                    {
                        extension = ".jpg";
                    }
                    else
                    {
                        extension = ".png";
                    }
                    Nova_Importer.LoadExternResource(folder.FullName + "\\" + IMAGE_COVER_NAME + extension, true, "COVER");
                    File.Copy(Nova_Importer.Content.RootDirectory + "\\Externs\\COVER.xnb", Nova_Importer.Content.RootDirectory + "\\" + folder.Name + "\\cover.xnb");
                }
                Nova_Importer.LoadResource("COVER", folder.Name + "\\COVER");
                musica.musicImage = (Texture2D)Nova_DataBase.GetResource("COVER");

                if (!File.Exists(Nova_Importer.Content.RootDirectory + "\\" + folder.Name + "\\music.xnb"))
                {
                   Nova_Importer.LoadExternMusicResource (folder.FullName + "\\" + MUSIC_FILE_NAME +".mp3", true, "MUSIC");
                    File.Copy(Nova_Importer.Content.RootDirectory + "\\Externs\\MUSIC.xnb", Nova_Importer.Content.RootDirectory + "\\" + folder.Name + "\\music.xnb");
                    File.Copy(Nova_Importer.Content.RootDirectory + "\\Externs\\MUSIC.wma", Nova_Importer.Content.RootDirectory + "\\" + folder.Name + "\\music.wma");
                }
                

                musica.musicPath = folder.FullName;
                musicas.Add(musica);
            }
        }

        private void DrawImages(SpriteBatch render)
        {
            render.Draw(background, Nova_Functions.ReturnScreenRectangle(), Color.White);
            render.Draw(coverBackground, Nova_Functions.CreateRectangle(Nova_Functions.GetCenterVector(Nova_Functions.CreateRectangle(Vector2.Zero, 450, 250), Nova_Functions.ReturnScreenRectangle()), 450, 250), Color.White);
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
            render.Draw(scoreBackground, new Rectangle(0, 0, Nova_Functions.View.Width, 60), Color.White);
            render.DrawString(scoreFont,musicas[currentIndex].musicPath.Split('\\')[musicas[currentIndex].musicPath.Split('\\').Length-1], new Vector2(Nova_Functions.View.Width / 2 - scoreFont.MeasureString(musicas[currentIndex].musicPath.Split('\\')[musicas[currentIndex].musicPath.Split('\\').Length - 1]).X / 2, 0), Color.White);
            if (sf != null)
            {
                sf.Draw(render);
            }
        }
    }
}
