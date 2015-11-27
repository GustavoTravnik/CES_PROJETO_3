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
using System.Diagnostics;

namespace Step_Game
{
    public class Menu
    {
        Boolean isServerOn = false;
        StepFloor sf;
        public static String playerName = Environment.UserName + new Random().Next(0, 99999).ToString();
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
        Nova_Network_Server server;
        public static Nova_Network_Client client;
        private void LoadAsServer()
        {
            Process.Start(Environment.CurrentDirectory + "\\server.exe");
        }

        private void ConnectAsClient()
        {
            client = new Nova_Network_Client();
            client.ConnectToServer(5457, "192.168.4.22", playerName);
            serverRunning = true;
        }

        public static Boolean serverRunning = false;

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

                if (newKb.IsKeyDown(Keys.F1) && oldKb.IsKeyUp(Keys.F1))
                {
                    LoadAsServer();
                }

                if (newKb.IsKeyDown(Keys.F2) && oldKb.IsKeyUp(Keys.F2))
                {
                    ConnectAsClient();
                }
            }
            else
            {
                if (newKb.IsKeyDown(Keys.Escape) && oldKb.IsKeyUp(Keys.Escape))
                {
                    isInGame = false;
                    SaveScoreOnline();
                    sf = null;
                    MediaPlayer.Stop();
                }
            }

            oldKb = newKb;
        }

        private void SaveScoreOnline()
        {
            try
            {
                if (isServerOn)
                {
                    Nova_FTP ftp = new Nova_FTP("yellowdesire.com", "u734915093", "finalfantasy9", false);
                    String name = new DirectoryInfo(musicas[currentIndex].musicPath).Name + ".txt";
                    StreamWriter sr = new StreamWriter(name);
                    sr.WriteLine(Environment.UserName);
                    sr.WriteLine(sf.notesOk.ToString() + "/" + ((int)sf.totalNotes).ToString());
                    sr.Close();
                    ftp.Upload(name);
                }
            }
            catch
            {
                isServerOn = false;
            }
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

        int oldIndex = -1;
        String currentScore= "";

        private String GetRecordByMusicName()
        {
            if (oldIndex != currentIndex && isServerOn)
            {
                currentScore = "";
                oldIndex = currentIndex;
                String name = new DirectoryInfo(musicas[currentIndex].musicPath).Name + ".txt";
                Nova_FTP ftp = new Nova_FTP("yellowdesire.com", "u734915093", "finalfantasy9", false);
                if (isServerOn)
                {
                    try
                    {
                    String[] files = ftp.GetFileList();
                    Boolean haveName = false;
                    foreach (String s in files)
                    {
                        if (s.Contains(name))
                        {
                            haveName = true;
                        }
                    }
                    if (!haveName)
                        return currentScore;

                  
                        if (File.Exists(name))
                            File.Delete(name);
                        ftp.Download(name, name);
                        StreamReader sr = new StreamReader(name);
                        currentScore = " - " + sr.ReadLine() + " Fez " + sr.ReadLine();
                        return currentScore;
                    }
                    catch
                    {
                        isServerOn = false;
                        return currentScore;
                    }
                }
                else
                    return currentScore;
            }
            else
                return currentScore;
          
        }

        public void Draw(SpriteBatch render)
        {
            DrawImages(render);
            render.Draw(scoreBackground, new Rectangle(0, 0, Nova_Functions.View.Width, 60), Color.White);
            render.DrawString(scoreFont,musicas[currentIndex].musicPath.Split('\\')[musicas[currentIndex].musicPath.Split('\\').Length-1] + GetRecordByMusicName(), new Vector2(Nova_Functions.View.Width / 2 - scoreFont.MeasureString(musicas[currentIndex].musicPath.Split('\\')[musicas[currentIndex].musicPath.Split('\\').Length - 1]).X / 2, 0), Color.White);
            if (sf != null)
            {
                sf.Draw(render);
            }
        }
    }
}
