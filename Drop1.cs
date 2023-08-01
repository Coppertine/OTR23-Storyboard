using OpenTK;
using OpenTK.Graphics;
using StorybrewCommon.Mapset;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Storyboarding.Commands;
using StorybrewCommon.Storyboarding.CommandValues;
using StorybrewCommon.Storyboarding.Display;
using StorybrewCommon.Storyboarding.Util;
using StorybrewCommon.Subtitles;
using StorybrewCommon.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StorybrewScripts
{
    public class Drop1 : StoryboardObjectGenerator
    {
        // This would include both drops in the first section... gonna work in reverse tho.., since i have so few ideas on this section.
        public override void Generate()
        {
            FirstSection();
            SecondSection();
        }

        private void FirstSection()
        {


            // 2 x 6 grid of squares rapidly changing colors (simlar to a noise texture)
            // Three lines in the wooshing part
            OsbSprite grayOverlayBG = GetLayer("").CreateSprite("sb/p.png");
            grayOverlayBG.ScaleVec(100665, 853, 480);
            grayOverlayBG.Fade(100665, 1);
            grayOverlayBG.Fade(103332, 0);
            grayOverlayBG.Color(103332, Color4.Gray);


            int rowCount = 2;
            int widthCount = 20;
            int squareWidth = 20;

            FastNoiseLite noise = new FastNoiseLite();
            noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
            byte[] noiseData = new byte[(rowCount * widthCount) * 8];
            int index = 0;
            for (int i = 0; i < 8; i++)
            {
                for (int y = 0; y < rowCount; y++)
                {
                    for (int x = 0; x < widthCount; x++)
                    {
                        float f = ((noise.GetNoise(x, y + 12) + 1) / 2f);
                        noiseData[index++] = (byte)(f >= 1.0 ? 255 : (f <= 0.0 ? 0 : (int)Math.Floor(f * 256.0)));
                    }
                }
            }
            for (int y = 0; y < rowCount; y++)
            {
                for (int x = 0; x < widthCount; x++)
                {
                    var yPos = 240 - (y % 2 == 0 ? 15 : -15);

                    var xPos = 100 + (25 * x);
                    OsbSprite square = GetLayer("").CreateSprite("sb/p.png", OsbOrigin.Centre, new Vector2(xPos, yPos));
                    square.Scale(100665 + (25 * x), 100665 + (50 * x), 0, squareWidth);
                    var currColour = new Color4(noiseData[x + (1 * y)], noiseData[x + (1 * y)], noiseData[x + (1 * y)], 255);
                    int i = 1;
                    for (int t = 100665; t <= 101665; t += 100)
                    {
                        var newColor = new Color4(noiseData[(x + (1 * y)) * i], noiseData[(x + 1 + (1 * y)) * i], noiseData[(x + 2 + (1 * y)) * i], 255);
                        square.Color(OsbEasing.OutExpo, t, t + 100, currColour, newColor);
                        i++;
                        currColour = newColor;
                    }
                }
            }

            FontGenerator font = LoadFont("sb/f/n", new FontDescription
            {
                FontPath = "fonts/Nexa-Heavy.ttf",
                FontSize = 30,
                Color = Color4.White
            }, new FontShadow
            {
                Thickness = 5,
                Color = Color4.Black
            });
            Lyric threeLyric = GetLayer("").CreateLyric(this, font, "THREE", OsbOrigin.Centre, 1, new Vector2(-120, 240 - (480 / 3)));
            Lyric twoLyric = GetLayer("").CreateLyric(this, font, "TWO", OsbOrigin.Centre, 1, new Vector2(767, 240));
            Lyric oneLyric = GetLayer("").CreateLyric(this, font, "ONE", OsbOrigin.Centre, 1, new Vector2(-120, 240 + (480 / 3)));

            OsbSprite barTop = GetLayer("").CreateSprite("sb/p.png", OsbOrigin.CentreLeft, new Vector2(-107, 240 - (480 / 3)));
            OsbSprite barMid = GetLayer("").CreateSprite("sb/p.png", OsbOrigin.CentreRight, new Vector2(747, 240));
            OsbSprite barBottom = GetLayer("").CreateSprite("sb/p.png", OsbOrigin.CentreLeft, new Vector2(-107, 240 + (480 / 3)));

            int barHeight = 50;
            double scratch1 = 101665;
            double scratch2 = 101776;
            double scratch3 = 101887;
            double three = 101999;
            double two = 102332;
            double one = 102665;
            double endTime = 102999;
            OsbEasing easing = OsbEasing.OutExpo;
            barTop.ScaleVec(easing, scratch1, scratch2, new Vector2(0, barHeight), new Vector2(853, barHeight));
            barMid.ScaleVec(easing, scratch2, scratch3, new Vector2(0, barHeight), new Vector2(853, barHeight));
            barBottom.ScaleVec(easing, scratch3, three, new Vector2(0, barHeight), new Vector2(853, barHeight));

            barTop.Fade(scratch1, 1);
            barMid.Fade(scratch1, 1);
            barBottom.Fade(scratch1, 1);


            // Here are the lyrics part..

            OsbSprite barTopLeft = GetLayer("").CreateSprite("sb/p.png", OsbOrigin.CentreLeft, new Vector2(-107, 240 - (480 / 3)));
            OsbSprite barMidRight = GetLayer("").CreateSprite("sb/p.png", OsbOrigin.CentreRight, new Vector2(747, 240));
            OsbSprite barBottomLeft = GetLayer("").CreateSprite("sb/p.png", OsbOrigin.CentreLeft, new Vector2(-107, 240 + (480 / 3)));


            barTopLeft.Fade(three, 1);
            barMidRight.Fade(two, 1);
            barBottomLeft.Fade(one, 1);

            threeLyric.Move(easing, three, two, new Vector2(-120, 240 - (480 / 3) - 31), new Vector2(560, 240 - (480 / 3) - 31));
            twoLyric.Move(easing, two, one, new Vector2(767, 240 - 31), new Vector2(70, 240 - 31));
            oneLyric.Move(easing, one, endTime, new Vector2(-120, 240 + (480 / 3) - 31), new Vector2(435, 240 + (480 / 3) - 31));
            threeLyric.Fade(three, 1);
            twoLyric.Fade(two, 1);
            oneLyric.Fade(one, 1);

            barTop.MoveX(easing, three, two, -107, 650);
            barTop.ScaleVec(easing, three, two, new Vector2(853, barHeight), new Vector2(747 - 650, barHeight));
            barTopLeft.ScaleVec(easing, three, two, new Vector2(0, barHeight), new Vector2(580, barHeight));

            barMid.MoveX(easing, two, one, 747, 0);
            barMid.ScaleVec(easing, two, one, new Vector2(853, barHeight), new Vector2(107, barHeight));
            barMidRight.ScaleVec(easing, two, one, new Vector2(0, barHeight), new Vector2(605, barHeight));

            barBottom.MoveX(easing, one, endTime, -107, 500);
            barBottom.ScaleVec(easing, one, endTime, new Vector2(853, barHeight), new Vector2(747 - 500, barHeight));
            barBottomLeft.ScaleVec(easing, one, endTime, new Vector2(0, barHeight), new Vector2(480, barHeight));

            barTop.ScaleVec(easing, endTime, 103221, barTop.ScaleAt(endTime), new Vector2(747 - 560, (480 / 3)));
            barTop.MoveX(easing, endTime, 103221, barTop.PositionAt(endTime).X, 560);
            barTopLeft.ScaleVec(easing, endTime, 103221, barTopLeft.ScaleAt(endTime), new Vector2(560 + 107, (480 / 3)));

            barMid.ScaleVec(easing, endTime, 103221, barMid.ScaleAt(endTime), new Vector2(70 + 107, (480 / 3)));
            barMid.MoveX(easing, endTime, 103221, barMid.PositionAt(endTime).X, 70);
            barMidRight.ScaleVec(easing, endTime, 103221, barMidRight.ScaleAt(endTime), new Vector2(747 - 70, (480 / 3)));

            barBottom.ScaleVec(easing, endTime, 103221, barBottom.ScaleAt(endTime), new Vector2(747 - 560, (480 / 3)));
            barBottom.MoveX(easing, endTime, 103221, barBottom.PositionAt(endTime).X, 560);
            barBottomLeft.ScaleVec(easing, endTime, 103221, barBottomLeft.ScaleAt(endTime), new Vector2(560 + 107, (480 / 3)));


            barTop.Fade(103332, 0);
            barMid.Fade(103332, 0);
            barBottom.Fade(103332, 0);
            barTopLeft.Fade(103332, 0);
            barMidRight.Fade(103332, 0);
            barBottomLeft.Fade(103332, 0);

            threeLyric.Fade(103332, 0);
            twoLyric.Fade(103332, 0);
            oneLyric.Fade(103332, 0);

            GlitchSection();
        }

        OsbSprite bgR, bgG, bgB;
        void GlitchSection()
        {
            bgR = GetLayer("").CreateSprite("sb/r.jpg");
            bgG = GetLayer("").CreateSprite("sb/g.jpg");
            bgB = GetLayer("").CreateSprite("sb/b.jpg");
            bgR.Additive(103332);
            bgG.Additive(103332);
            bgB.Additive(103332);

            bgR.Fade(103332, 1);
            bgG.Fade(103332, 1);
            bgB.Fade(103332, 1);

            bgR.Fade(112499, 112665, 1, 0);
            bgG.Fade(112499, 112665, 1, 0);
            bgB.Fade(112499, 112665, 1, 0);

            bgR.Fade(113999, 1);
            bgG.Fade(113999, 1);
            bgB.Fade(113999, 1);


            bgR.Fade(121999, 0);
            bgG.Fade(121999, 0);
            bgB.Fade(121999, 0);
            bgR.Scale(103332, (480.0f / 1423) * 1.125f);
            bgG.Scale(103332, (480.0f / 1423) * 1.125f);
            bgB.Scale(103332, (480.0f / 1423) * 1.125f);

            rgbRandomMove(OsbEasing.OutExpo, 103332, 103582);
            rgbRandomMove(OsbEasing.OutExpo, 103582, 103832);
            rgbRandomMove(OsbEasing.OutExpo, 103832, 104165);
            rgbRandomMove(OsbEasing.OutExpo, 104165, 104665);
            rgbRandomMove(OsbEasing.OutExpo, 104665, 104915);
            rgbRandomMove(OsbEasing.OutExpo, 104915, 105165);
            rgbRandomMove(OsbEasing.OutExpo, 105165, 105999);
            rgbRandomMove(OsbEasing.OutExpo, 105999, 106249);
            rgbRandomMove(OsbEasing.OutExpo, 106249, 106499);
            rgbRandomMove(OsbEasing.OutExpo, 106499, 106832);
            rgbRandomMove(OsbEasing.OutExpo, 106832, 107332);
            rgbRandomMove(OsbEasing.OutExpo, 107332, 107582);
            rgbRandomMove(OsbEasing.OutExpo, 107582, 107832);
            rgbRandomMove(OsbEasing.OutCubic, 107832, 108665);

            rgbRandomMove(OsbEasing.OutCubic, 108665, 109332);
            rgbRandomMove(OsbEasing.OutCubic, 109332, 109832);
            rgbRandomMove(OsbEasing.OutCubic, 109832, 110332);
            rgbRandomMove(OsbEasing.OutCubic, 110332, 110665);
            rgbRandomMove(OsbEasing.OutCubic, 110665, 111332);

            rgbRandomMove(OsbEasing.OutExpo, 111332, 111999);
            rgbRandomMove(OsbEasing.OutExpo, 111999, 112332);
            rgbRandomMove(OsbEasing.OutExpo, 112332, 112665);

            rgbRandomMove(OsbEasing.OutExpo, 113999, 114249);
            rgbRandomMove(OsbEasing.OutExpo, 114249, 114499);
            rgbRandomMove(OsbEasing.OutExpo, 114499, 114832);
            rgbRandomMove(OsbEasing.OutExpo, 114832, 115332);
            rgbRandomMove(OsbEasing.OutExpo, 115332, 115582);
            rgbRandomMove(OsbEasing.OutExpo, 115582, 115832);
            rgbRandomMove(OsbEasing.OutExpo, 115832, 116665);
            rgbRandomMove(OsbEasing.OutExpo, 116665, 116999);
            rgbRandomMove(OsbEasing.OutExpo, 116999, 117249);
            rgbRandomMove(OsbEasing.OutExpo, 117249, 117499);
            rgbRandomMove(OsbEasing.OutExpo, 117499, 117665);
            rgbRandomMove(OsbEasing.OutExpo, 117665, 117832);
            rgbRandomMove(OsbEasing.OutExpo, 117832, 117999);
            rgbRandomMove(OsbEasing.OutCubic, 117999, 118332);
            rgbRandomMove(OsbEasing.OutCubic, 118332, 118665);
            rgbRandomMove(OsbEasing.OutCubic, 118332, 118665);
            rgbRandomMove(OsbEasing.OutCubic, 118665, 118999);
            rgbRandomMove(OsbEasing.OutCubic, 118999, 119332);
            rgbRandomMove(OsbEasing.OutExpo, 119332, 119665);
            rgbRandomMove(OsbEasing.OutExpo, 119665, 119999);
            rgbRandomMove(OsbEasing.OutExpo, 119999, 120499);
            rgbRandomMove(OsbEasing.OutExpo, 120499, 120832);
            rgbRandomMove(OsbEasing.OutExpo, 120832, 120999);
            rgbRandomMove(OsbEasing.OutExpo, 120999, 121332);
            rgbRandomMove(OsbEasing.OutExpo, 121332, 121999);


            var bg = GetLayer("").CreateSprite(Beatmap.BackgroundPath);
            bg.Scale(112665, 480.0f / 1423);
            bg.Fade(112665, 0.5);
            bg.Fade(113999, 0);

            bg.Fade(121999, 0.5);
            bg.Fade(124665, 0);
        }

        private void rgbRandomMove(OsbEasing easing, int start, int end)
        {
            bgR.Move(easing, start, end, new Vector2(320, 240), new Vector2(320 + Random(-10, 10), 240 + Random(-10, 10)));
            bgG.Move(easing, start, end, new Vector2(320, 240), new Vector2(320 + Random(-10, 10), 240 + Random(-10, 10)));
            bgB.Move(easing, start, end, new Vector2(320, 240), new Vector2(320 + Random(-10, 10), 240 + Random(-10, 10)));
        }

        // The reason why this isn't in Buildup class is that I want to use the same square for basicaly the entire sections.
        void SecondSection()
        {
            // Todo:
            // Small Square particles (clusters) from hitobjects of 129998 - 133998
            // 
            // Main Square on top (Rotated Math.PI / 4)
            // also.. no sprite pools.. for now..

            foreach (var hitobject in Beatmap.HitObjects)
            {
                if (hitobject is OsuSpinner)
                    continue;
                if (129998 <= hitobject.StartTime && hitobject.StartTime <= 133998)
                {
                    var pos = new Vector2(Random(-107, 747), Random(0, 480));
                    for (var i = 0; i <= Random(5, 15); i++)
                    {
                        var angle = Random(0, Math.PI * 2);
                        Vector2 startPosition = new Vector2(
                            (float)(pos.X + Math.Cos(angle) * Random(25, 100)),
                            (float)(pos.Y + Math.Sin(angle) * Random(25, 100))
                        );
                        var endTime = hitobject.StartTime +
                            (hitobject.StartTime < 132665 ?
                                Beatmap.GetTimingPointAt((int)hitobject.StartTime).BeatDuration :
                                Beatmap.GetTimingPointAt((int)hitobject.StartTime).BeatDuration / 2);
                        // Copy paste from Beggars...
                        var square = GetLayer("").CreateSprite("sb/p.png", OsbOrigin.Centre, new Vector2(0, 0));
                        square.Scale(hitobject.StartTime, 10);
                        square.Fade(hitobject.StartTime, hitobject.StartTime + 110, 0, 1);
                        square.Rotate(OsbEasing.InExpo, hitobject.StartTime, endTime, Random(0, Math.PI * 2), angle);
                        square.Move(OsbEasing.OutExpo, hitobject.StartTime, hitobject.StartTime + 20, hitobject.Position, startPosition);
                        square.Move(OsbEasing.InSine, hitobject.StartTime + 20, endTime + Beatmap.GetTimingPointAt((int)hitobject.StartTime).BeatDuration * 2, startPosition, new Vector2(320, 240));
                    }
                }
            }
            OsbSprite cover = GetLayer("").CreateSprite("sb/p.png");
            cover.Color(133665, Color4.Black);
            cover.ScaleVec(133665, new Vector2(847, 480));
            cover.Fade(133665, 1);
            cover.Fade(134832, 0);


            OsbSprite mainSquare = GetLayer("").CreateSprite("sb/p.png");
            mainSquare.Fade(124665, 129998, 0, 1);
            mainSquare.Rotate(124665, Math.PI / 4);
            mainSquare.Scale(124665, 129998, 200, 230);
            mainSquare.StartLoopGroup(129998, 16);
            mainSquare.Scale(OsbEasing.OutExpo, 0, Beatmap.GetTimingPointAt(129998).BeatDuration / 2, 250, 230);
            mainSquare.EndGroup();

            var scaleCurr = 250;
            for (double time = 132665; time <= 133998; time += Beatmap.GetTimingPointAt(129998).BeatDuration / 4)
            {
                mainSquare.Scale(OsbEasing.OutExpo, time, time + Beatmap.GetTimingPointAt(129998).BeatDuration / 4, scaleCurr + 70, scaleCurr + 50);
                scaleCurr += 50;
            }

            mainSquare.Rotate(132665, 133998, Math.PI / 4, (Math.PI / 4) * 7);

            mainSquare.Rotate(133998, Math.PI / 4);
            mainSquare.Scale(OsbEasing.OutExpo, 133998, 134248, 400, 220);

            // New drop...
            mainSquare.StartLoopGroup(135332, 128);
            mainSquare.Scale(OsbEasing.OutExpo, 0, Beatmap.GetTimingPointAt(135332).BeatDuration, 230, 220);
            mainSquare.EndGroup();

            // 8 bit right
            int rowCount = 2;
            int widthCount = 20;
            int squareWidth = 20;
            FastNoiseLite noise = new FastNoiseLite();
            noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
            byte[] noiseData = new byte[(rowCount * widthCount) * 8];
            int index = 0;
            for (int i = 0; i < 8; i++)
            {
                for (int y = 0; y < rowCount; y++)
                {
                    for (int x = 0; x < widthCount; x++)
                    {
                        float f = ((noise.GetNoise(x, y + 12) + 1) / 2f);
                        noiseData[index++] = (byte)(f >= 1.0 ? 255 : (f <= 0.0 ? 0 : (int)Math.Floor(f * 256.0)));
                    }
                }
            }



            // Right
            for (int y = 0; y < rowCount; y++)
            {
                for (int x = 0; x < widthCount; x++)
                {
                    var xPos = 595 - (y % 2 == 0 ? 15 : -15);

                    var yPos = 0 + (25 * x);
                    OsbSprite square = GetLayer("").CreateSprite("sb/p.png", OsbOrigin.Centre, new Vector2(xPos, yPos));
                    square.Scale(139332 + (1 * x), 139332 + (25 * x), 0, squareWidth);
                    square.Scale(140665 - (15 * x), 140665 - (1 * x), squareWidth, 0);
                    var currColour = new Color4(noiseData[x + (1 * y)], noiseData[x + (1 * y)], noiseData[x + (1 * y)], 255);
                    int i = 1;
                    for (int t = 139332; t <= 140332; t += 100)
                    {
                        var newColor = new Color4(noiseData[(x + (1 * y)) * i], noiseData[(x + 1 + (1 * y)) * i], noiseData[(x + 2 + (1 * y)) * i], 255);
                        square.Color(OsbEasing.OutExpo, t, t + 100, currColour, newColor);
                        i++;
                        currColour = newColor;
                    }
                }
            }

            // Left
            for (int y = 0; y < rowCount; y++)
            {
                for (int x = 0; x < widthCount; x++)
                {
                    var xPos = 25 - (y % 2 == 0 ? 15 : -15);

                    var yPos = 480 - (25 * x);
                    OsbSprite square = GetLayer("").CreateSprite("sb/p.png", OsbOrigin.Centre, new Vector2(xPos, yPos));
                    square.Scale(139665 + (1 * x), 139665 + (25 * x), 0, squareWidth);
                    square.Scale(140498 - (15 * x), 140498 - (1 * x), squareWidth, 0);

                    var currColour = new Color4(noiseData[x + (1 * y)], noiseData[x + (1 * y)], noiseData[x + (1 * y)], 255);
                    int i = 1;
                    for (int t = 139665; t <= 140332; t += 100)
                    {
                        var newColor = new Color4(noiseData[(x + (1 * y)) * i], noiseData[(x + 1 + (1 * y)) * i], noiseData[(x + 2 + (1 * y)) * i], 255);
                        square.Color(OsbEasing.OutExpo, t, t + 100, currColour, newColor);
                        i++;
                        currColour = newColor;
                    }
                }
            }

            mainSquare.Rotate(OsbEasing.OutExpo, 145665, 146332, Math.PI / 4, (Math.PI / 4) * 9);

            // Will need to cut the square into two rectangles, so mainSquare's gonna fade for a few moments.
            mainSquare.Fade(147498, 0);
            mainSquare.Fade(147665, 1);
            OsbSprite recLeft = GetLayer("").CreateSprite("sb/p.png", OsbOrigin.CentreRight);
            recLeft.Rotate(147498, Math.PI / 4);
            recLeft.Fade(147498, 1);
            recLeft.ScaleVec(147498, 147665, new Vector2(230 / 2, 230), new Vector2(220 / 2, 220));
            recLeft.Move(OsbEasing.OutExpo, 147498, 147665, new Vector2(320, 240), new Vector2(320 - 25, 240 + 25));

            OsbSprite recRight = GetLayer("").CreateSprite("sb/p.png", OsbOrigin.CentreLeft);
            recRight.Rotate(147498, Math.PI / 4);
            recRight.Fade(147498, 1);
            recRight.ScaleVec(147498, 147665, new Vector2(230 / 2, 230), new Vector2(220 / 2, 220));
            recRight.Move(OsbEasing.OutExpo, 147498, 147665, new Vector2(320, 240), new Vector2(320 + 25, 240 - 25));

            // RGB time
            double startTime = 149665;
            double endtime = 150665;
            double timeStep = (Beatmap.GetTimingPointAt((int)startTime).BeatDuration / 4) * 3;


            OsbSprite squareR = GetLayer("").CreateSprite("sb/p.png");
            OsbSprite squareG = GetLayer("").CreateSprite("sb/p.png");
            OsbSprite squareB = GetLayer("").CreateSprite("sb/p.png");

            squareR.Additive(startTime);
            squareG.Additive(startTime);
            squareB.Additive(startTime);

            squareR.Rotate(startTime, Math.PI / 4);
            squareG.Rotate(startTime, Math.PI / 4);
            squareB.Rotate(startTime, Math.PI / 4);

            squareR.Color(startTime, Color4.Red);
            squareG.Color(startTime, Color4.Green);
            squareB.Color(startTime, Color4.Blue);

            squareR.Fade(startTime, 1);
            squareR.Fade(151665, 0);
            squareG.Fade(startTime, 1);
            squareG.Fade(151665, 0);
            squareB.Fade(startTime, 1);
            squareB.Fade(151665, 0);

            squareR.StartLoopGroup(startTime, (int)(endtime - startTime) / (int)timeStep);
            squareR.Scale(OsbEasing.OutExpo, 0, timeStep, 230, 220);
            squareR.EndGroup();

            squareG.StartLoopGroup(startTime, (int)(endtime - startTime) / (int)timeStep);
            squareG.Scale(OsbEasing.OutExpo, 0, timeStep, 230, 220);
            squareG.EndGroup();

            squareB.StartLoopGroup(startTime, (int)(endtime - startTime) / (int)timeStep);
            squareB.Scale(OsbEasing.OutExpo, 0, timeStep, 230, 220);
            squareB.EndGroup();

            for (double time = startTime; time <= endtime; time += timeStep)
            {
                squareR.Move(OsbEasing.OutExpo, time, time + timeStep, new Vector2(320, 240), new Vector2(320 + Random(-40, 40), 240 + Random(-40, 40)));
                squareG.Move(OsbEasing.OutExpo, time, time + timeStep, new Vector2(320, 240), new Vector2(320 + Random(-40, 40), 240 + Random(-40, 40)));
                squareB.Move(OsbEasing.OutExpo, time, time + timeStep, new Vector2(320, 240), new Vector2(320 + Random(-40, 40), 240 + Random(-40, 40)));
            }

            squareR.Move(OsbEasing.InOutExpo, 150915, 151665, squareR.PositionAt((int)150915), new Vector2(320, 240));
            squareG.Move(OsbEasing.InOutExpo, 150915, 151665, squareG.PositionAt((int)150915), new Vector2(320, 240));
            squareB.Move(OsbEasing.InOutExpo, 150915, 151665, squareB.PositionAt((int)150915), new Vector2(320, 240));

            mainSquare.Fade(167332, 0);

            KikouHana();

        }
        void KikouHana()
        {
            List<int> movetimes = new List<int>() {
                169998,
                170248,
                170498,
                170665,
                170832,
                171165,
                171332,
                171498,
                171665,
                171832,
                171998
            };
            var movingbgs = new AnimatedValue<CommandPosition>();
            var rotatingbgs = new AnimatedValue<CommandDecimal>();
            var endtime = 172665;
            foreach (var movetime in movetimes)
            {
                movingbgs.Add(new MoveCommand(OsbEasing.OutQuad, movetime,
                    movetimes.IndexOf(movetime) == movetimes.Count - 1 ? endtime : movetimes[movetimes.IndexOf(movetime) + 1],
                    new Vector2(320, 240), new Vector2(320 + Random(-20, 20), 240 + Random(-20, 20))));
                rotatingbgs.Add(new RotateCommand(OsbEasing.OutQuad, movetime,
                    movetimes.IndexOf(movetime) == movetimes.Count - 1 ? endtime : movetimes[movetimes.IndexOf(movetime) + 1],
                    0, MathHelper.DegreesToRadians(Random(-10, 10))));
            }

            OsbSprite duoToneBG = GetLayer("").CreateSprite("sb/duotone.jpg");
            OsbSprite scanBG = GetLayer("").CreateSprite("sb/scan.jpg");
            OsbSprite invertBG = GetLayer("").CreateSprite("sb/invert.jpg");
            OsbSprite bwBG = GetLayer("").CreateSprite("sb/bw.jpg");



            duoToneBG.Scale(170248, (480.0f / 1151) * 1.25);
            scanBG.Scale(170082, (480.0f / 1151) * 1.25);
            invertBG.Scale(169998, (480.0f / 1423) * 1.25);
            bwBG.Scale(169998, (480.0f / 1423) * 1.25);
            bwBG.Fade(169998, 0);

            invertBG.Fade(169998, 1);
            invertBG.Move(169998, 170082, movingbgs.ValueAtTime(169998), movingbgs.ValueAtTime(170082));
            invertBG.Rotate(169998, 170082, rotatingbgs.ValueAtTime(169998), rotatingbgs.ValueAtTime(170082));
            invertBG.Fade(170082, 0);
            scanBG.Fade(170082, 1);
            scanBG.Move(170082, 170165, movingbgs.ValueAtTime(170082), movingbgs.ValueAtTime(170165));
            scanBG.Rotate(170082, 170165, rotatingbgs.ValueAtTime(170082), rotatingbgs.ValueAtTime(170165));
            scanBG.Fade(170165, 0);
            duoToneBG.Fade(170248, 1);
            duoToneBG.Move(170248, 170332, movingbgs.ValueAtTime(170248), movingbgs.ValueAtTime(170332));
            duoToneBG.Rotate(170248, 170332, rotatingbgs.ValueAtTime(170248), rotatingbgs.ValueAtTime(170332));
            duoToneBG.Fade(170332, 0);
            scanBG.Fade(170332, 1);
            scanBG.Move(170332, 170498, movingbgs.ValueAtTime(170332), movingbgs.ValueAtTime(170498));
            scanBG.Rotate(170332, 170498, rotatingbgs.ValueAtTime(170332), rotatingbgs.ValueAtTime(170498));
            scanBG.Fade(170498, 0);

            for (int time = 170498; time < 170665; time += 44)
            {
                invertBG.Fade(time, 1);
                invertBG.Move(time, time + 22, movingbgs.ValueAtTime(time), movingbgs.ValueAtTime(time + 22));
                invertBG.Rotate(time, time + 22, rotatingbgs.ValueAtTime(time), rotatingbgs.ValueAtTime(time + 22));
                invertBG.Fade(time + 22, 0);
                scanBG.Fade(time + 22, 1);
                scanBG.Move(time, time + 22, movingbgs.ValueAtTime(time), movingbgs.ValueAtTime(time + 22));
                scanBG.Rotate(time, time + 22, rotatingbgs.ValueAtTime(time), rotatingbgs.ValueAtTime(time + 22));
                scanBG.Fade(time + 44, 0);
            }

            duoToneBG.Fade(170665, 1);
            duoToneBG.Move(170665, 170748, movingbgs.ValueAtTime(170665), movingbgs.ValueAtTime(170748));
            duoToneBG.Rotate(170665, 170748, rotatingbgs.ValueAtTime(170665), rotatingbgs.ValueAtTime(170748));
            duoToneBG.Fade(170748, 0);
            scanBG.Fade(170748, 1);
            scanBG.Move(170748, 170832, movingbgs.ValueAtTime(170748), movingbgs.ValueAtTime(170832));
            scanBG.Rotate(170748, 170832, rotatingbgs.ValueAtTime(170748), rotatingbgs.ValueAtTime(170832));
            scanBG.Fade(170832, 0);
            invertBG.Fade(170832, 1);
            invertBG.Move(170832, 170915, movingbgs.ValueAtTime(170832), movingbgs.ValueAtTime(170915));
            invertBG.Rotate(170832, 170915, rotatingbgs.ValueAtTime(170832), rotatingbgs.ValueAtTime(170915));
            invertBG.Fade(170915, 0);
            duoToneBG.Fade(170915, 1);
            duoToneBG.Move(170915, 170998, movingbgs.ValueAtTime(170915), movingbgs.ValueAtTime(170998));
            duoToneBG.Rotate(170915, 170998, rotatingbgs.ValueAtTime(170915), rotatingbgs.ValueAtTime(170998));
            duoToneBG.Fade(170998, 0);
            scanBG.Fade(170998, 1);
            scanBG.Move(170998, 171082, movingbgs.ValueAtTime(170998), movingbgs.ValueAtTime(171082));
            scanBG.Rotate(170998, 171082, rotatingbgs.ValueAtTime(170998), rotatingbgs.ValueAtTime(171082));
            scanBG.Fade(171082, 0);

            bwBG.Fade(171082, 1);
            bwBG.Move(171082, 171165, movingbgs.ValueAtTime(171082), movingbgs.ValueAtTime(171165));
            bwBG.Rotate(171082, 171165, rotatingbgs.ValueAtTime(171082), rotatingbgs.ValueAtTime(171165));
            bwBG.Fade(171165, 0);

            invertBG.Fade(171165, 1);
            invertBG.Move(171165, 171248, movingbgs.ValueAtTime(171165), movingbgs.ValueAtTime(171248));
            invertBG.Rotate(171165, 171248, rotatingbgs.ValueAtTime(171165), rotatingbgs.ValueAtTime(171248));
            invertBG.Fade(171248, 0);

            scanBG.Fade(171248, 1);
            scanBG.Move(171248, 171332, movingbgs.ValueAtTime(171248), movingbgs.ValueAtTime(171332));
            scanBG.Rotate(171248, 171332, rotatingbgs.ValueAtTime(171248), rotatingbgs.ValueAtTime(171332));
            scanBG.Fade(171332, 0);

            invertBG.Fade(171332, 1);
            invertBG.Move(171332, 171415, movingbgs.ValueAtTime(171332), movingbgs.ValueAtTime(171415));
            invertBG.Rotate(171332, 171415, rotatingbgs.ValueAtTime(171332), rotatingbgs.ValueAtTime(171415));
            invertBG.Fade(171415, 0);

            duoToneBG.Fade(171415, 1);
            duoToneBG.Move(171415, 171498, movingbgs.ValueAtTime(171415), movingbgs.ValueAtTime(171498));
            duoToneBG.Rotate(171415, 171498, rotatingbgs.ValueAtTime(171415), rotatingbgs.ValueAtTime(171498));
            duoToneBG.Fade(171498, 0);

            invertBG.Fade(171498, 1);
            invertBG.Move(171498, 171582, movingbgs.ValueAtTime(171498), movingbgs.ValueAtTime(171582));
            invertBG.Rotate(171498, 171582, rotatingbgs.ValueAtTime(171498), rotatingbgs.ValueAtTime(171582));
            invertBG.Fade(171582, 0);

            bwBG.Fade(171582, 1);
            bwBG.Move(171582, 171665, movingbgs.ValueAtTime(171582), movingbgs.ValueAtTime(171665));
            bwBG.Rotate(171582, 171665, rotatingbgs.ValueAtTime(171582), rotatingbgs.ValueAtTime(171665));
            bwBG.Fade(171665, 0);

            duoToneBG.Fade(171665, 1);
            duoToneBG.Move(171665, 171748, movingbgs.ValueAtTime(171665), movingbgs.ValueAtTime(171748));
            duoToneBG.Rotate(171665, 171748, rotatingbgs.ValueAtTime(171665), rotatingbgs.ValueAtTime(171748));
            duoToneBG.Fade(171665, 0);

            scanBG.Fade(171748, 1);
            scanBG.Move(171748, 171832, movingbgs.ValueAtTime(171748), movingbgs.ValueAtTime(171832));
            scanBG.Rotate(171748, 171832, rotatingbgs.ValueAtTime(171748), rotatingbgs.ValueAtTime(171832));
            scanBG.Fade(171832, 0);

            bwBG.Fade(171832, 1);
            bwBG.Move(171832, 171998, movingbgs.ValueAtTime(171832), movingbgs.ValueAtTime(171998));
            bwBG.Rotate(171832, 171998, rotatingbgs.ValueAtTime(171832), rotatingbgs.ValueAtTime(171998));
            bwBG.Fade(171998, 0);

            invertBG.Fade(171998, 172665, 1, 0);

            // 169998
            OsbAnimation noise = GetLayer("Foreground").CreateAnimation("sb/noise/.jpg", 4, 100, OsbLoopType.LoopForever);
            noise.Fade(169998, 1);
            noise.Additive(169998);
            noise.Fade(172665, 0);

            var bg = GetLayer("").CreateSprite(Beatmap.BackgroundPath);
            bg.Scale(172665, (480.0f / 1423) * 1.25);
            bg.Fade(172665, 1);
            endtime = 177998;
            movetimes = new List<int>{
                172665,
                173165,
                173665,
                174165,
                174665,
                174998,
                175332,
                175832,
                176332,
                176832,
                177332,
                177665
            };
            foreach (var movetime in movetimes)
            {
                bg.Move(OsbEasing.OutExpo, movetime,
                      movetimes.IndexOf(movetime) == movetimes.Count - 1 ? endtime : movetimes[movetimes.IndexOf(movetime) + 1],
                      new Vector2(320, 240), new Vector2(320 + Random(-20, 20), 240 + Random(-20, 20)));
                bg.Rotate(OsbEasing.OutExpo, movetime,
                    movetimes.IndexOf(movetime) == movetimes.Count - 1 ? endtime : movetimes[movetimes.IndexOf(movetime) + 1],
                    0, MathHelper.DegreesToRadians(Random(-10, 10)));
            }
            bg.Fade(endtime, 0);
        }
    }
}
