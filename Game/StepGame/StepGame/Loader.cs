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
    public static class Loader
    {
        public static void LoadNotes()
        {
            List<Texture2D> texture;

            for (int i = 1; i <= 5; i++)
            {
                texture = new List<Texture2D>();
                texture = Nova_Importer.GetTextureList(@"Notes_Sprite\"+i.ToString(), 4);
                Nova_DataBase.AddResource("NOTE_" + i.ToString(), texture);

                Nova_Importer.LoadResource("NOTE_OFF_" + i.ToString(), @"Notes_Sprite\" + i.ToString() + @"\5");
                Nova_Importer.LoadResource("NOTE_HIT_" + i.ToString(), @"Notes_Sprite\" + i.ToString() + @"\6");
            }

           
        }

        public static void LoadFonts()
        {
            Nova_Importer.LoadResource("FONT_SCORE", @"Fonts\ScoreFont");
            Nova_Importer.LoadResource("FONT_MENU", @"Fonts\TitleFont");
            Nova_Importer.LoadResource("MENU_BACKGROUND", @"Back\\cover");
            Nova_Importer.LoadResource("SCORE_BACKGROUND", @"Back\\scoreBackground");

        }
    }
}
