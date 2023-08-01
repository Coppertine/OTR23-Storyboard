using OpenTK;
using OpenTK.Graphics;
using StorybrewCommon.Mapset;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Storyboarding.Util;
using StorybrewCommon.Subtitles;
using StorybrewCommon.Util;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace StorybrewScripts
{
    public class CollabNames : StoryboardObjectGenerator
    {
        FontGenerator fontGen;
        StoryboardLayer lyricLayer;

        float scale = 0.45f;
        public override void Generate()
        {
            lyricLayer = GetLayer("Lyric");
            List<CollabPart> CollabParts = ReadCollabFile("collabList.csv");

            fontGen = LoadFont("sb/f/c", new FontDescription()
            {
                FontPath = "fonts/Nexa-Heavy.ttf",
                FontSize = 55,
                Color = Color4.White,
                Padding = Vector2.Zero,
                FontStyle = System.Drawing.FontStyle.Regular,
                TrimTransparency = false,
                EffectsOnly = false,
                Debug = false
            },
            new FontShadow()
            {
                Thickness = 5,
                Color = Color4.Black
            });

            foreach (CollabPart part in CollabParts)
            {
                generatePerLine(part.name, (int)part.startTime, (int)part.endTime, 600, 420, false);
            }
        }

        public void generatePerLine(String lyric, int startTime, int endTime, int positionX, int positionY, bool inverse)
        {
            int xOffset = 25;
            xOffset = inverse ? xOffset * -1 : xOffset;

            var texture = fontGen.GetTexture(lyric);
            var position = new Vector2(positionX - texture.BaseWidth * scale * 0.5f, positionY) + texture.OffsetFor(OsbOrigin.Centre) * (float)scale;

            position.Y = positionY;

            var sprite = lyricLayer.CreateSprite(texture.Path, OsbOrigin.Centre, position);
            sprite.Scale(startTime, scale);
            sprite.Fade(OsbEasing.OutExpo, startTime, startTime + 750, 0, 1);
            sprite.Fade(OsbEasing.InExpo, endTime - 750, endTime, 1, 0);
            sprite.MoveX(OsbEasing.OutExpo, startTime, startTime + 750, positionX - xOffset, positionX);

            sprite.MoveX(OsbEasing.InExpo, endTime-750, endTime, positionX, positionX + xOffset);

            var lineWidth = texture.BaseWidth * scale;
        }

        public List<CollabPart> ReadCollabFile(string filePath)
        {
            List<CollabPart> parts = new List<CollabPart>();
            using (StreamReader reader = new StreamReader(OpenProjectFile(filePath)))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] values = line.Split(',');

                    parts.Add(new CollabPart
                    {
                        startTime = int.Parse(values[0]),
                        endTime = int.Parse(values[1]),
                        name = values[2]
                    });
                }
                return parts;
            }
        }

        public class CollabPart
        {
            public double startTime { get; set; }
            public double endTime { get; set; }
            public string name { get; set; }
        }
    }
}
