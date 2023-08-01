using OpenTK;
using OpenTK.Graphics;
using StorybrewCommon.Mapset;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Storyboarding.Util;
using StorybrewCommon.Subtitles;
using StorybrewCommon.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StorybrewScripts
{
    public class Lyrics : StoryboardObjectGenerator
    {
        FontGenerator fontGen;
        StoryboardLayer lyricLayer;
        StoryboardLayer backLayer;
        float scale = 0.15f;
        public override void Generate()
        {
            lyricLayer = GetLayer("Lyric");

            backLayer = GetLayer("Lyric Background");

            fontGen = LoadFont("sb/f/l", new FontDescription()
            {
                FontPath = "fonts/PretendardJP-Light.otf",
                FontSize = 152,
                Color = Color4.White,
                Padding = Vector2.Zero,
                FontStyle = System.Drawing.FontStyle.Regular,
                TrimTransparency = true,
                EffectsOnly = false,
                Debug = false
            });

            generatePerLine("天使の国へ戻った", 46666, 51833, 320, 350);
            generatePerLine("誰かに呼ばれた気がし", 52333, 56666, 320, 350);
            generatePerLine("たの明日からは私の番", 56999, 61999, 320, 350);
            generatePerLine("愛を届けにゆくの", 62333, 69999, 320, 350);
            generatePerLine("きれいなお花畑の真ん中", 70666, 76999, 320, 350);
            generatePerLine("なんてここは素晴らしいのかしら", 77499, 80999, 320, 350);
            generatePerLine("素敵な愛で世界をいっぱいにして", 81166, 86166, 320, 350);
            generatePerLine("きれいな羽根で", 86333, 89166, 320, 350);
            generateScaleOut("空をとぶ天使になるの", 89333, 94999, 320, 240);


            generateScaleOut("空をとぶ天使になるの", 121665, 124665, 320, 240);


            generatePerLine("きれいなお花畑の真ん中", 348430, 354763, 320, 350);
            generatePerLine("なんてここは素晴らしいのかしら",354930 , 358430, 320, 350);
            generatePerLine("愛を届けにゆくのきれいなお", 358430, 364763, 320, 350);
            generatePerLine("きれいな羽根で空をとぶ天使になるの", 364763, 370096, 320, 350);



        }

        public void generatePerLine(string lyric, int startTime, int endTime, int positionX, int positionY)
        {
            var texture = fontGen.GetTexture(lyric);
            var position = new Vector2(positionX - texture.BaseWidth * 0.5f * scale, positionY) + texture.OffsetFor(OsbOrigin.Centre) * scale;
            position.Y = positionY;

            if (!texture.IsEmpty)
            {
                var sprite = lyricLayer.CreateSprite(texture.Path, OsbOrigin.Centre, position);
                sprite.Scale(startTime, scale);
                sprite.Fade(OsbEasing.OutExpo, startTime - 250, startTime + 500, 0, 1); 
                sprite.Fade(OsbEasing.InExpo, endTime - 750, endTime, 1, 0);

                var lineWidth = texture.BaseWidth * scale;

                backLayer = GetLayer("Lyric Background");

                var backBox = backLayer.CreateSprite("sb/p.png");

                backBox.Color(startTime, Color4.Black);
                backBox.Move(startTime - 250, new Vector2(positionX, positionY));
                backBox.ScaleVec(OsbEasing.OutExpo, startTime - 250, startTime + 500, 0, 200 * scale, lineWidth, 200 * scale);
                backBox.Fade(startTime - 250, 0.7);
                backBox.Fade(OsbEasing.InExpo, endTime - 750, endTime, 0.7, 0);
            }
        }

        public void generateScaleOut(String lyric, int startTime, int endTime, int positionX, int positionY)
        {
            // 92499 - Start of scaling up

            var texture = fontGen.GetTexture(lyric);
            var position = new Vector2(positionX - texture.BaseWidth * 0.5f * scale, positionY) + texture.OffsetFor(OsbOrigin.Centre) * scale;
            position.Y = positionY;

            var sprite = lyricLayer.CreateSprite(texture.Path, OsbOrigin.Centre, position);
            sprite.Scale(startTime, scale);
            sprite.Scale(OsbEasing.InSine, startTime, endTime, scale, scale * 2);
            sprite.Fade(OsbEasing.OutExpo, startTime - 250, startTime + 500, 0, 1);
            sprite.Fade(OsbEasing.InSine, endTime - 2000, endTime, 1, 0);

            var lineWidth = texture.BaseWidth * scale;

            // backLayer = GetLayer("Lyric Background");

            // var backBox = backLayer.CreateSprite("sb/p.png");

            // backBox.Color(startTime, Color4.Black);
            // backBox.Move(startTime - 250, new Vector2(positionX, positionY));
            // backBox.ScaleVec(OsbEasing.OutExpo, startTime - 250, startTime + 500, 0, 200 * scale, lineWidth, 200 * scale);
            // backBox.ScaleVec(OsbEasing.InSine, 92499, 94999, lineWidth, 200 * scale, lineWidth * 3, 600 * scale);
            // // backBox.ScaleVec(OsbEasing.OutExpo, endTime, endTime + fadeDuration, lineWidth, 40, 0, 40);
            // // backBox.MoveY(OsbEasing.OutExpo, startTime, Math.Round(startTime + ((endTime - startTime) / 1.7), 0), positionY + 25, positionY);
            // backBox.Fade(startTime - 250, 0.7);
            // backBox.Fade(OsbEasing.InSine, 92499, 94999, 0.7, 0);
        }
    }
}