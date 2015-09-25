using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using NovaDll;

namespace Step_Creator
{

    public class Principal : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        WMPLib.WindowsMediaPlayer player;
        Texture2D fillTexture;
        Rectangle[] indicadores = new Rectangle[5]
        {
            new Rectangle(0, 0, 50, 50),
            new Rectangle(60, 0, 50, 50), 
            new Rectangle(120, 0, 50, 50),
            new Rectangle(180, 0, 50, 50),
            new Rectangle(240, 0, 50, 50)
        };
        Boolean[] canDrawStep = new Boolean[5]
        {
            false,
            false,
            false,
            false,
            false
        };
        public struct Step
        {
            public Double time;
            public int note;
        }
        List<Step> times = new List<Step>();
        Microsoft.Xna.Framework.Input.Keys[] controles = new Microsoft.Xna.Framework.Input.Keys[5] 
        {
            Microsoft.Xna.Framework.Input.Keys.A,
            Microsoft.Xna.Framework.Input.Keys.S,
            Microsoft.Xna.Framework.Input.Keys.J,
            Microsoft.Xna.Framework.Input.Keys.K,
            Microsoft.Xna.Framework.Input.Keys.L
        };
        KeyboardState keyboard, oldKeyboard;
        String musicPath = "";
        Microsoft.Xna.Framework.Input.Keys closeKey = Microsoft.Xna.Framework.Input.Keys.Escape;
        Microsoft.Xna.Framework.Input.Keys slowKey = Microsoft.Xna.Framework.Input.Keys.D1;
        Microsoft.Xna.Framework.Input.Keys fastKey = Microsoft.Xna.Framework.Input.Keys.D2;
        Microsoft.Xna.Framework.Input.Keys normalSpeedKey = Microsoft.Xna.Framework.Input.Keys.Back;
        float mediaPlayerSpeed = 1;


        public Principal()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        private void SelecionarMusica()
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "*.mp3|*.mp3";
            op.FileName = String.Empty;
            op.ShowDialog();
            if (!String.IsNullOrEmpty(op.FileName))
            {
                musicPath = op.FileName;
            }
            player.URL = musicPath;
            player.controls.play();
        }

        private void PopularLista(KeyboardState kb, KeyboardState okb)
        {
            Double currentTime = Math.Floor(player.controls.currentPosition * 100) / 100;

            for (int i = 0; i < 5; i++)
            {
                if (kb.IsKeyDown(controles[i]) && okb.IsKeyUp(controles[i]))
                {
                    Step step = new Step();
                    step.time = currentTime;
                    step.note = i;
                    times.Add(step);
                    canDrawStep[i] = true;
                }
                else
                {
                    canDrawStep[i] = false;
                }
            }
        }

        private void SaveMusic()
        {
            FolderBrowserDialog folder = new FolderBrowserDialog();
            folder.SelectedPath = String.Empty;
            folder.ShowDialog();
            if (!String.IsNullOrEmpty(folder.SelectedPath))
            {
                Save(folder.SelectedPath);
            }
            Exit();
        }

        private void Save(String directory)
        {
            File.Copy(musicPath, directory + "\\music.mp3");
            StreamWriter sw = new StreamWriter(directory + "\\steps.script");
            for (int i = times.Count - 1; i >= 0; i--)
            {
                sw.WriteLine(Convert.ToString(times[i].time) + "|" + Convert.ToString(times[i].note));
            }
            sw.Close();
        }

        private void EndMusicListener(KeyboardState kb)
        {
            if (kb.IsKeyDown(closeKey) || player.playState == WMPLib.WMPPlayState.wmppsStopped)
            {
                SaveMusic();
            }
        }

        private void DrawSteps(SpriteBatch render)
        {
            for (int i = 0; i < 5; i++)
            {
                if (canDrawStep[i])
                {
                    render.Draw(fillTexture, indicadores[i], Color.White);
                }
            }
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Nova_Functions.SetViewport(GraphicsDevice);
            player = new WMPLib.WindowsMediaPlayer();
            player.settings.autoStart = false;
            player.settings.rate = 0.8;
            fillTexture = Nova_Functions.GetFillTexture(Color.Blue);
            SelecionarMusica();
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            keyboard = Keyboard.GetState();
            if (player.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                PopularLista(keyboard, oldKeyboard);
            }
            EndMusicListener(keyboard);
            oldKeyboard = keyboard;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            spriteBatch.Begin(0, BlendState.NonPremultiplied);
            DrawSteps(spriteBatch);
            spriteBatch.End();
        }
    }
}
