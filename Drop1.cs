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
            grayOverlayBG.Fade(103315, 0);
            grayOverlayBG.Color(103315, "#111320");


            int rowCount = 2;
            int widthCount = 34;
            int squareWidth = 20;

            FastNoiseLite noise = new FastNoiseLite();
            noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
            byte[] noiseData = new byte[(rowCount * widthCount) * 64];
            int index = 0;
            for (int i = 0; i < 64; i++)
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

                    var xPos = -92.5F + (25 * x);
                    OsbSprite square = GetLayer("").CreateSprite("sb/p.png", OsbOrigin.Centre, new Vector2(xPos, yPos));
                    square.Scale(100648 + (15 * x), 100648 + (25 * x), 0, squareWidth);
                    var currColour = new Color4(noiseData[x + (1 * y)], noiseData[x + (1 * y)], noiseData[x + (1 * y)], 255);
                    int i = 1;
                    for (int t = 100648; t <= 101648; t += 50)
                    {
                        var newColor = new Color4(noiseData[(x + (1*y)) * i], noiseData[(x + 1 + (1*y)) * i], noiseData[(x + 2 + (1*y)) * i], 255);
                        square.Color(OsbEasing.OutExpo, t, t + 50, currColour, newColor);
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


            barTop.Fade(103315, 0);
            barMid.Fade(103315, 0);
            barBottom.Fade(103315, 0);
            barTopLeft.Fade(103315, 0);
            barMidRight.Fade(103315, 0);
            barBottomLeft.Fade(103315, 0);

            threeLyric.Fade(103315, 0);
            twoLyric.Fade(103315, 0);
            oneLyric.Fade(103315, 0);

            GlitchSection();
        }

        OsbSprite bgR, bgG, bgB;
        void GlitchSection()
        {
            bgR = GetLayer("").CreateSprite("sb/r.jpg");
            bgG = GetLayer("").CreateSprite("sb/g.jpg");
            bgB = GetLayer("").CreateSprite("sb/b.jpg");
            bgR.Additive(103315);
            bgG.Additive(103315);
            bgB.Additive(103315);

            bgR.Fade(103315, 1);
            bgG.Fade(103315, 1);
            bgB.Fade(103315, 1);

            bgR.Fade(112499, 112665, 1, 0);
            bgG.Fade(112499, 112665, 1, 0);
            bgB.Fade(112499, 112665, 1, 0);

            bgR.Fade(113999, 1);
            bgG.Fade(113999, 1);
            bgB.Fade(113999, 1);


            bgR.Fade(121999, 0);
            bgG.Fade(121999, 0);
            bgB.Fade(121999, 0);
            bgR.Scale(103315, (480.0f / 720) * 1.25f);
            bgG.Scale(103315, (480.0f / 720) * 1.25f);
            bgB.Scale(103315, (480.0f / 720) * 1.25f);

            rgbRandomMove(OsbEasing.OutExpo, 103315, 103565);
            rgbRandomMove(OsbEasing.OutExpo, 103565, 103815);
            rgbRandomMove(OsbEasing.OutExpo, 103815, 104148);
            rgbRandomMove(OsbEasing.OutExpo, 104148, 104648);
            rgbRandomMove(OsbEasing.OutExpo, 104648, 104898);
            rgbRandomMove(OsbEasing.OutExpo, 104898, 105148);
            rgbRandomMove(OsbEasing.OutExpo, 105148, 105982);
            rgbRandomMove(OsbEasing.OutExpo, 105982, 106249);
            rgbRandomMove(OsbEasing.OutExpo, 106249, 106482);
            rgbRandomMove(OsbEasing.OutExpo, 106482, 106832-17);
            rgbRandomMove(OsbEasing.OutExpo, 106832-17, 107332-17);
            rgbRandomMove(OsbEasing.OutExpo, 107332-17, 107582-17);
            rgbRandomMove(OsbEasing.OutExpo, 107582-17, 107832-17);
            rgbRandomMove(OsbEasing.OutCubic, 107832-17, 108665-17);

            rgbRandomMove(OsbEasing.OutCubic, 108665-17, 109332-17);
            rgbRandomMove(OsbEasing.OutCubic, 109332-17, 109832-17);
            rgbRandomMove(OsbEasing.OutCubic, 109832-17, 110332-17);
            rgbRandomMove(OsbEasing.OutCubic, 110332-17, 110665-17);
            rgbRandomMove(OsbEasing.OutCubic, 110665-17, 111332-17);

            rgbRandomMove(OsbEasing.OutExpo, 111332-17, 111999-17);
            rgbRandomMove(OsbEasing.OutExpo, 111999-17, 112332-17);
            rgbRandomMove(OsbEasing.OutExpo, 112332-17, 112665-17);

            rgbRandomMove(OsbEasing.OutExpo, 113999-17, 114249-17);
            rgbRandomMove(OsbEasing.OutExpo, 114249-17, 114499-17);
            rgbRandomMove(OsbEasing.OutExpo, 114499-17, 114832-17);
            rgbRandomMove(OsbEasing.OutExpo, 114832-17, 115332-17);
            rgbRandomMove(OsbEasing.OutExpo, 115332-17, 115582-17);
            rgbRandomMove(OsbEasing.OutExpo, 115582-17, 115832-17);
            rgbRandomMove(OsbEasing.OutExpo, 115832-17, 116665-17);
            rgbRandomMove(OsbEasing.OutExpo, 116665-17, 116999-17);
            rgbRandomMove(OsbEasing.OutExpo, 116999-17, 117249-17);
            rgbRandomMove(OsbEasing.OutExpo, 117249-17, 117499-17);
            rgbRandomMove(OsbEasing.OutExpo, 117499-17, 117665-17);
            rgbRandomMove(OsbEasing.OutExpo, 117665-17, 117832-17);
            rgbRandomMove(OsbEasing.OutExpo, 117832-17, 117999-17);
            rgbRandomMove(OsbEasing.OutCubic, 117999-17, 118332-17);
            rgbRandomMove(OsbEasing.OutCubic, 118332-17, 118665-17);
            rgbRandomMove(OsbEasing.OutCubic, 118332-17, 118665-17);
            rgbRandomMove(OsbEasing.OutCubic, 118665-17, 118999-17);
            rgbRandomMove(OsbEasing.OutCubic, 118999-17, 119332-17);
            rgbRandomMove(OsbEasing.OutExpo, 119332-17, 119665-17);
            rgbRandomMove(OsbEasing.OutExpo, 119665-17, 119999-17);
            rgbRandomMove(OsbEasing.OutExpo, 119999-17, 120499-17);
            rgbRandomMove(OsbEasing.OutExpo, 120499-17, 120832-17);
            rgbRandomMove(OsbEasing.OutExpo, 120832-17, 120999-17);
            rgbRandomMove(OsbEasing.OutExpo, 120999-17, 121332-17);
            rgbRandomMove(OsbEasing.OutExpo, 121332-17, 121999-17);


            var bg = GetLayer("").CreateSprite(Beatmap.BackgroundPath);
            bg.Scale(112648, 480.0f / 1423);
            bg.Fade(112648, 0.5);
            bg.Fade(113982, 0);

            bg.Fade(121982, 0.5);
            bg.Fade(124648, 0);
        }

        private void rgbRandomMove(OsbEasing easing, int start, int end)
        {
            bgR.Move(easing, start, end, new Vector2(320, 240), new Vector2(320 + Random(-20,20), 240 + Random(-20, 20)));
            bgR.Rotate(easing, start, end, 0, MathHelper.DegreesToRadians(Random(-10,10)));

            bgG.Move(easing, start, end, new Vector2(320, 240), new Vector2(320 + Random(-10, 10), 240 + Random(-10, 10)));
            bgG.Rotate(easing, start, end, 0, MathHelper.DegreesToRadians(Random(-10,10)));

            bgB.Move(easing, start, end, new Vector2(320, 240), new Vector2(320 + Random(-10, 10), 240 + Random(-10, 10)));
            bgB.Rotate(easing, start, end, 0, MathHelper.DegreesToRadians(Random(-10,10)));

        }

        // The reason why this isn't in Buildup class is that I want to use the same square for basicaly the entire sections.
        void SecondSection()
        {
            // Todo:
            // Small Square particles (clusters) from hitobjects of 129981 - 133981
            // 
            // Main Square on top (Rotated Math.PI / 4)
            // also.. no sprite pools.. for now..

            foreach (var hitobject in Beatmap.HitObjects)
            {
                if (hitobject is OsuSpinner)
                    continue;
                if (129898 <= hitobject.StartTime && hitobject.StartTime <= 133981)
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
                            (hitobject.StartTime < 132648 ?
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
            cover.Color(133648, Color4.Black);
            cover.ScaleVec(133648, new Vector2(847, 480));
            cover.Fade(133648, 1);
            cover.Fade(134832, 0);

            // Little square ring
            OsbSprite squareRing = GetLayer("").CreateSprite("sb/box2.png");
            squareRing.StartLoopGroup(135315, ((155981-135315) / (int)Beatmap.GetTimingPointAt(135315).BeatDuration));
                squareRing.Scale(OsbEasing.OutCirc, 0, Beatmap.GetTimingPointAt(135315).BeatDuration * 0.75, 0.8, 2);
                squareRing.Fade(OsbEasing.OutCirc, 0, Beatmap.GetTimingPointAt(135315).BeatDuration, 1,0);
            squareRing.EndGroup();
            squareRing.StartLoopGroup(156648, ((164648-156648) / (int)Beatmap.GetTimingPointAt(156648).BeatDuration));
                squareRing.Scale(OsbEasing.OutCirc, 0, Beatmap.GetTimingPointAt(156648).BeatDuration * 0.75, 0.8, 2);
                squareRing.Fade(OsbEasing.OutCirc, 0, Beatmap.GetTimingPointAt(156648).BeatDuration, 1,0);
            squareRing.EndGroup();

            OsbSprite squareRing2 = GetLayer("").CreateSprite("sb/box2.png");
            squareRing2.StartLoopGroup(154148, 6);
                squareRing2.Scale(OsbEasing.OutCirc, 0, Beatmap.GetTimingPointAt(135315).BeatDuration * 0.75, 0.8, 2);
                squareRing2.Fade(OsbEasing.OutCirc, 0, Beatmap.GetTimingPointAt(135315).BeatDuration, 1,0);
            squareRing2.EndGroup();

            OsbSprite squareRing3 = GetLayer("").CreateSprite("sb/box2.png");
            squareRing3.StartLoopGroup(155398, 2);
                squareRing3.Scale(OsbEasing.OutCirc, 0, Beatmap.GetTimingPointAt(135315).BeatDuration * 0.75, 0.8, 2);
                squareRing3.Fade(OsbEasing.OutCirc, 0, Beatmap.GetTimingPointAt(135315).BeatDuration, 1,0);
            squareRing3.EndGroup();

            OsbSprite mainSquare = GetLayer("").CreateSprite("sb/p.png");
            mainSquare.Fade(124648, 129981, 0, 1);
            mainSquare.Rotate(124648, Math.PI / 4);
            mainSquare.Scale(124648, 129981, 200, 230);
            mainSquare.StartLoopGroup(129981, 16);
                mainSquare.Scale(OsbEasing.OutExpo, 0, Beatmap.GetTimingPointAt(129981).BeatDuration / 2, 250, 230);
            mainSquare.EndGroup();

            var scaleCurr = 250;
            for (double time = 132648; time <= 133981; time += Beatmap.GetTimingPointAt(129981).BeatDuration / 4)
            {
                mainSquare.Scale(OsbEasing.OutExpo, time, time + Beatmap.GetTimingPointAt(129981).BeatDuration / 4, scaleCurr + 70, scaleCurr + 50);
                scaleCurr += 50;
            }

            mainSquare.Rotate(132648, 133981, Math.PI / 4, (Math.PI / 4) * 7);

            mainSquare.Rotate(133981, Math.PI / 4);
            mainSquare.Scale(OsbEasing.OutExpo, 133981, 134398, 400, 220);

            // New drop...
            mainSquare.StartLoopGroup(135315, ((153981-135315) / (int)Beatmap.GetTimingPointAt(135315).BeatDuration)-1);
                mainSquare.Scale(OsbEasing.OutExpo, 0, Beatmap.GetTimingPointAt(135315).BeatDuration, 230, 220);
            mainSquare.EndGroup();

            mainSquare.StartLoopGroup(153981, ((155315-153981) / (int)(Beatmap.GetTimingPointAt(153981).BeatDuration / 2)));
                mainSquare.Scale(OsbEasing.OutExpo, 0, (Beatmap.GetTimingPointAt(153981).BeatDuration / 2), 230, 220);
            mainSquare.EndGroup();

            mainSquare.StartLoopGroup(155315, (155981-155315) / (int)(Beatmap.GetTimingPointAt(155981).BeatDuration / 4));
                mainSquare.Scale(OsbEasing.OutExpo, 0, (Beatmap.GetTimingPointAt(155981).BeatDuration / 4), 230, 220);
            mainSquare.EndGroup();

            mainSquare.StartLoopGroup(156648, ((164648-156648) / (int)Beatmap.GetTimingPointAt(156648).BeatDuration)-1);
                mainSquare.Scale(OsbEasing.OutExpo, 0, Beatmap.GetTimingPointAt(156648).BeatDuration, 230, 220);
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
                    square.Scale(139315 + (1 * x), 139315 + (25 * x), 0, squareWidth);
                    square.Scale(140648 - (15 * x), 140648 - (1 * x), squareWidth, 0);
                    var currColour = new Color4(noiseData[x + (1 * y)], noiseData[x + (1 * y)], noiseData[x + (1 * y)], 255);
                    int i = 1;
                    for (int t = 139315; t <= 140332; t += 100)
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
                    square.Scale(139648 + (1 * x), 139648 + (25 * x), 0, squareWidth);
                    square.Scale(140481 - (15 * x), 140481 - (1 * x), squareWidth, 0);

                    var currColour = new Color4(noiseData[x + (1 * y)], noiseData[x + (1 * y)], noiseData[x + (1 * y)], 255);
                    int i = 1;
                    for (int t = 139648; t <= 140332; t += 100)
                    {
                        var newColor = new Color4(noiseData[(x + (1 * y)) * i], noiseData[(x + 1 + (1 * y)) * i], noiseData[(x + 2 + (1 * y)) * i], 255);
                        square.Color(OsbEasing.OutExpo, t, t + 100, currColour, newColor);
                        i++;
                        currColour = newColor;
                    }
                }
            }

            // Spirals
            CreateSquareSpiral(142148,142648, new Vector2(60, 365), 25, 2);
            CreateSquareSpiral(142231,142815, new Vector2(580, 300), 25, 6, Math.PI/2);
            CreateSquareSpiral(142315,142981, new Vector2(3, 77), 25, 5, Math.PI);
            CreateSquareSpiral(142398,143148, new Vector2(594, 98), 25, 2, Math.PI);

            mainSquare.Rotate(OsbEasing.OutExpo, 145648, 145981, Math.PI / 4, (Math.PI / 4) * 9);
            mainSquare.Rotate(OsbEasing.InExpo, 145981, 146315, MathHelper.DegreesToRadians(-45), MathHelper.DegreesToRadians((-45 * 5)));

            // Will need to cut the square into two rectangles, so mainSquare's gonna fade for a few moments.
            mainSquare.Fade(147481, 0);
            mainSquare.Fade(147648, 1);
            OsbSprite recLeft = GetLayer("").CreateSprite("sb/p.png", OsbOrigin.CentreRight);
            recLeft.Rotate(147481, Math.PI / 4);
            recLeft.Fade(147481, 1);
            recLeft.ScaleVec(147481, 147648, new Vector2(230 / 2, 230), new Vector2(220 / 2, 220));
            recLeft.Move(OsbEasing.OutExpo, 147481, 147648, new Vector2(320, 240), new Vector2(320 - 25, 240 + 25));

            OsbSprite recRight = GetLayer("").CreateSprite("sb/p.png", OsbOrigin.CentreLeft);
            recRight.Rotate(147481, Math.PI / 4);
            recRight.Fade(147481, 1);
            recRight.ScaleVec(147481, 147648, new Vector2(230 / 2, 230), new Vector2(220 / 2, 220));
            recRight.Move(OsbEasing.OutExpo, 147481, 147648, new Vector2(320, 240), new Vector2(320 + 25, 240 - 25));

            // RGB time
            double startTime = 149648;
            double endtime = 150648;
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
            int mainMoveRGBOffset = 20;
            double mainRotateRGBOffset = 5;

            Vector2 mainMoveRGB = new Vector2(320-mainMoveRGBOffset,240);
            double mainRotateRGB = MathHelper.DegreesToRadians(-45+mainRotateRGBOffset);
            for (double time = startTime; time <= endtime; time += timeStep)
            {
                squareR.Move(OsbEasing.OutExpo, time, time + timeStep, new Vector2(320, 240), new Vector2(320 + Random(-40, 40), 240 + Random(-40, 40)));
                squareG.Move(OsbEasing.OutExpo, time, time + timeStep, new Vector2(320, 240), new Vector2(320 + Random(-40, 40), 240 + Random(-40, 40)));
                squareB.Move(OsbEasing.OutExpo, time, time + timeStep, new Vector2(320, 240), new Vector2(320 + Random(-40, 40), 240 + Random(-40, 40)));
                mainSquare.Move(OsbEasing.OutExpo, time, time +timeStep, new Vector2(320,240), mainMoveRGB);
                mainSquare.Rotate(OsbEasing.OutExpo, time, time +timeStep, MathHelper.DegreesToRadians(-45), mainRotateRGB);
                mainMoveRGB = new Vector2(mainMoveRGB.X == 320 - mainMoveRGBOffset ? 320 + mainMoveRGBOffset : 320 - mainMoveRGBOffset, 240);
                mainRotateRGB = mainRotateRGB == MathHelper.DegreesToRadians(-45+mainRotateRGBOffset) ? MathHelper.DegreesToRadians(-45-mainRotateRGBOffset) : MathHelper.DegreesToRadians(-45+mainRotateRGBOffset);
            }

            squareR.Move(OsbEasing.InOutExpo, 150915, 151665, squareR.PositionAt((int)150915), new Vector2(320, 240));
            squareG.Move(OsbEasing.InOutExpo, 150915, 151665, squareG.PositionAt((int)150915), new Vector2(320, 240));
            squareB.Move(OsbEasing.InOutExpo, 150915, 151665, squareB.PositionAt((int)150915), new Vector2(320, 240));
            mainSquare.Move(OsbEasing.InOutExpo, 150915, 151665, mainSquare.PositionAt((int)150915), new Vector2(320, 240));
            mainSquare.Rotate(OsbEasing.InOutExpo, 150915, 151665, mainSquare.RotationAt((int)150915), MathHelper.DegreesToRadians(-45));

            // Mini Build up
            // Add some small squares that pile up over time moving downwards
            GenerateLittleSquares(151648,155981, 25, 1050);
            GenerateLittleSquares(153981,155981, 50, 500);
            GenerateLittleSquares(155315,155981, 150, 300);

            mainSquare.Rotate(OsbEasing.OutBack,155981, 156648, Math.PI / 4, (Math.PI / 4) * 11);

            mainSquare.Rotate(OsbEasing.OutQuart,157815, 157981, Math.PI / 4, (Math.PI / 4) * 17);
            mainSquare.Rotate(OsbEasing.OutQuart,157981, 158315, (Math.PI / 4) * 8,Math.PI / 4);

            mainSquare.Move(OsbEasing.OutQuart, 157981, 158148, new Vector2(320,240),new Vector2(-280,240));
            mainSquare.Move(OsbEasing.OutQuart, 158148, 158315, new Vector2(800,240),new Vector2(320,240));

            // Zig zag down
            mainSquare.Fade(158481,0);
            OsbSprite zigzagSquare = GetLayer("").CreateSprite("sb/p.png");
            zigzagSquare.Scale(158481,110);
            zigzagSquare.Rotate(158481,Math.PI/4);
            zigzagSquare.Fade(158481,1);
            zigzagSquare.Move(158481, new Vector2(320 - 55, 330));
            zigzagSquare.Scale(158565,55);
            zigzagSquare.Move(158565, new Vector2(320 + 22.5f, 430));

            zigzagSquare.Move(158648, new Vector2(320 - 22.5f, 10));
            zigzagSquare.Move(158731, new Vector2(320 + 22.5f, 65));
            zigzagSquare.Move(158815, new Vector2(320 - 22.5f, 120));
            zigzagSquare.Move(158898, new Vector2(320 + 22.5f, 175));

            zigzagSquare.Fade(158981,0);
            mainSquare.Fade(158981,1);
            mainSquare.Move(OsbEasing.InOutBack, 158981,159315,new Vector2(300,300), new Vector2(320, 240));
            mainSquare.Rotate(OsbEasing.InOutBack, 158981,159315,MathHelper.DegreesToRadians(-20), MathHelper.DegreesToRadians(-45));

            // Breakthrough
            // Topright
            OsbSprite whiteBackground = GetLayer("Back").CreateSprite("sb/p.png");
            whiteBackground.ScaleVec(159648,787,480);
            whiteBackground.Fade(159648, 1);
            whiteBackground.Fade(159981, 0);
            whiteBackground.Fade(160315, 1);
            whiteBackground.Fade(160648, 0);

            OsbSprite wallC = GetLayer("").CreateSprite("sb/p.png");
            OsbSprite wallL = GetLayer("").CreateSprite("sb/p.png", OsbOrigin.TopCentre);
            OsbSprite wallR = GetLayer("").CreateSprite("sb/p.png", OsbOrigin.BottomCentre);
            
            OsbSprite smallSquareBreak = GetLayer("").CreateSprite("sb/p.png");
            int brokenSquares = 8;

            float wallOffset = 120;
            wallC.Move(159315,new Vector2(747-wallOffset,0+wallOffset));
            wallC.ScaleVec(OsbEasing.OutExpo, 159315, 159648, new Vector2(40, 0), new Vector2(40, (float)(wallOffset / Math.Sin(Math.PI / 4))*2.3f));
            wallC.Rotate(159315, -Math.PI / 4);
            wallC.Fade(159315,1);

            mainSquare.Move(OsbEasing.InOutBack, 159315,159565,new Vector2(320,240), new Vector2(320 - 50, 240 + 50));
            mainSquare.Move(OsbEasing.InOutBack, 159565,159648,new Vector2(320 - 50,240 + 50), new Vector2(320 + 50, 240 - 50));
            mainSquare.Fade(159648,0);
            double centralDis = Math.Sqrt(Math.Pow(747 - 320,2) + Math.Pow(480-240,2));

            wallC.Fade(159648,0);
            wallL.Color(159648, "#000");
            wallL.Move(159648,new Vector2(200- 150, 119 - 150));
            wallL.ScaleVec(159648,new Vector2(40, ((float)(centralDis / Math.Sin(Math.PI / 4))) / 2));
            wallL.Rotate(OsbEasing.OutBack,159648,159981,MathHelper.DegreesToRadians(-45), MathHelper.DegreesToRadians(-80));

            wallR.Move(159648,new Vector2(454+150, 360 +150));
            wallR.Color(159648, "#000");
            wallR.ScaleVec(159648,new Vector2(40, ((float)(centralDis / Math.Sin(Math.PI / 4))) / 2));
            wallR.Rotate(OsbEasing.OutBack,159648,159981,MathHelper.DegreesToRadians(-45), MathHelper.DegreesToRadians(0));

            smallSquareBreak.Scale(159648,159981, 60,50);
            smallSquareBreak.Color(159648, "#000");
            smallSquareBreak.Move(159648,159981, new Vector2(320,240), new Vector2(320 + 200, 240 - 200));
            smallSquareBreak.Rotate(OsbEasing.InSine,159648,159981, MathHelper.DegreesToRadians(45), MathHelper.DegreesToRadians(150));
            
            // I can't be fucked scrolling up to find the start time
            squareRing.Color(squareRing.CommandsStartTime, "#fff");
            squareRing.Color(159648, "#000");
            squareRing.Color(159981, "#fff");
            squareRing.Color(160315, "#000");
            squareRing.Color(160648, "#fff");
            for (int i = 0; i < brokenSquares; i++)
            {
                OsbSprite brokenSquareParticle = GetLayer("").CreateSprite("sb/p.png");
                brokenSquareParticle.Color(159648,"#000");
                brokenSquareParticle.Scale(159648, Random(3,8));
                var randCosSin = Math.SinCos(Random(-Math.PI /2 - (Math.PI /4), Math.PI/2- (Math.PI /4)));
                var rad = 40;
                brokenSquareParticle.Move(OsbEasing.OutSine, 159648, 159981,smallSquareBreak.PositionAt(159648), new Vector2((float)(rad * randCosSin.Cos + smallSquareBreak.PositionAt(159981).X),(float)(rad * randCosSin.Sin+  smallSquareBreak.PositionAt(159981).Y )));
            }

            mainSquare.Fade(159981,1);
            wallC.Fade(159981,1);
            wallC.Move(159981,new Vector2(-107+wallOffset,480-wallOffset));
            wallC.ScaleVec(OsbEasing.OutExpo, 159981, 160315, new Vector2(40, 0), new Vector2(40, (float)(wallOffset / Math.Sin(Math.PI / 4))*2.3f));
            mainSquare.Move(OsbEasing.InOutBack, 159981,160231,new Vector2(320,240), new Vector2(320 + 50, 240 - 50));
            mainSquare.Move(OsbEasing.InOutBack, 160231,160315,new Vector2(320 + 50,240 - 50), new Vector2(320 - 50, 240 + 50));

            mainSquare.Fade(160315,0);
            wallC.Fade(160315,0);
            wallL.Color(160315, "#000");
            wallL.Move(160315,new Vector2(200- 150, 119 - 150));
            wallL.ScaleVec(160315,new Vector2(40, ((float)(centralDis / Math.Sin(Math.PI / 4))) / 2));
            wallL.Rotate(OsbEasing.OutBack,160315,160648,MathHelper.DegreesToRadians(-45), MathHelper.DegreesToRadians(-0));

            wallR.Move(160315,new Vector2(454+150, 360 +150));
            wallR.Color(160315, "#000");
            wallR.ScaleVec(160315,new Vector2(40, ((float)(centralDis / Math.Sin(Math.PI / 4))) / 2));
            wallR.Rotate(OsbEasing.OutBack,160315,160648,MathHelper.DegreesToRadians(-45), MathHelper.DegreesToRadians(-80));



            smallSquareBreak.Scale(160315,160648, 60,50);
            smallSquareBreak.Color(160315, "#000");
            smallSquareBreak.Move(160315,160648, new Vector2(320,240), new Vector2(320 - 200, 240 + 200));
            smallSquareBreak.Rotate(OsbEasing.InSine,160315,160648, MathHelper.DegreesToRadians(45), MathHelper.DegreesToRadians(840));
            
            for (int i = 0; i < brokenSquares; i++)
            {
                OsbSprite brokenSquareParticle = GetLayer("").CreateSprite("sb/p.png");
                brokenSquareParticle.Color(160315,"#000");
                brokenSquareParticle.Scale(160315, Random(3,8));
                var randCosSin = Math.SinCos(Random(Math.PI, Math.PI * 2));
                var rad = 40;
                brokenSquareParticle.Move(OsbEasing.OutSine, 160315,160648,smallSquareBreak.PositionAt(160315), new Vector2((float)(rad * randCosSin.Cos + smallSquareBreak.PositionAt(160648).X),(float)(rad * randCosSin.Sin+  smallSquareBreak.PositionAt(160648).Y )));
            }


            mainSquare.Fade(160648,1);
            mainSquare.Move(160648,new Vector2(320,240));


            // eNDING Part
            mainSquare.Scale(164648, 550);
            mainSquare.Move(OsbEasing.OutSine, 164648, 164981, new Vector2(50, 300), new Vector2(-10,320));
            mainSquare.Move(OsbEasing.OutSine, 164981, 165315, new Vector2(620, 50), new Vector2(680,25));
            mainSquare.Move(OsbEasing.OutSine, 165315, 165648, new Vector2(340, 450), new Vector2(345,500));
            mainSquare.Move(OsbEasing.OutSine, 165981, 166315, new Vector2(-85, 358), new Vector2(50, 358));
            mainSquare.Move(OsbEasing.OutSine, 166315, 166481, new Vector2(672, 62), new Vector2(722, 36));
            mainSquare.Rotate(OsbEasing.OutSine, 164981, 165315, MathHelper.DegreesToRadians(-45 * 3),MathHelper.DegreesToRadians(45 * 3));
            mainSquare.Rotate(OsbEasing.OutSine, 165315, 165648, MathHelper.DegreesToRadians(-45),MathHelper.DegreesToRadians(-43));
            mainSquare.Rotate(OsbEasing.OutSine, 165648, 165981, MathHelper.DegreesToRadians(45),MathHelper.DegreesToRadians(395));
            mainSquare.Rotate(OsbEasing.OutSine, 165981, 166315, MathHelper.DegreesToRadians(45), MathHelper.DegreesToRadians(50));
            //wob
            mainSquare.StartLoopGroup(165648, 10);
                var wobdur = (165981 - 165648) / 10;
                mainSquare.Move(0,wobdur / 2, new Vector2(139, 21), new Vector2(113, 11));
                mainSquare.Move(wobdur / 2,wobdur, new Vector2(113, 11),new Vector2(139, 21));
            mainSquare.EndGroup();
            
            mainSquare.Move(166481, new Vector2(320,240));
            mainSquare.Scale(OsbEasing.InOutSine,166481, 167315, 200, 5);
            mainSquare.Rotate(OsbEasing.InOutSine,166481, 167315, MathHelper.DegreesToRadians(-45), MathHelper.DegreesToRadians(360));
            mainSquare.Fade(167332, 0);
            

            KikouHana();

        }

        private void CreateSquareSpiral(int start, int end, Vector2 pos, int numPoints, double a, double offset = 0)
        {
            var points = GetSpiralPoints(a, numPoints, Math.PI * 6, offset);
            int timeBetweenPoint = (end - start) / numPoints;
            for (int i = 0; i <= points.Count - 1; i++)
            {
                Vector2 position = new((float)(pos.X + points[i].x), (float)(pos.Y + points[i].y)); 
                OsbSprite square = GetLayer("SquareParticles").CreateSprite("sb/p.png", OsbOrigin.Centre, position);
                square.Scale(OsbEasing.OutQuart, start + (i * timeBetweenPoint), end + (end - start) + (i * timeBetweenPoint), 0, 5 + (i * 1.015f));
                // square.Scale(OsbEasing.OutQuart, end + (i * timeBetweenPoint), , 4 + (i * 1.015f),6 + (i * 1.015f) );
                square.Rotate(start, Math.Atan2(position.Y - pos.Y, position.X - pos.X));
                square.Fade(OsbEasing.OutQuart, end + (i * timeBetweenPoint), end + (end - start) + (i * timeBetweenPoint), 1,0 );

            }
        }

        private void GenerateLittleSquares(double startTime, double endTime, int particleCount, int particleDuration)
        {
            for (int i = 0; i < particleCount; i++)
            {
                double start = startTime + Random(0, particleDuration);
                OsbSprite square = GetLayer("SquareParticles").CreateSprite("sb/p.png", OsbOrigin.Centre, new Vector2(Random(-108, 747), -25));
                square.Scale(start, Random(25,55));
                square.Fade(start, 1);
                square.Fade(endTime, 0);
                square.StartLoopGroup(start, (int)(endTime - start) / particleDuration);
                    square.MoveY(0, particleDuration, -25, 515);
                square.EndGroup();
                double squareRotateStart = MathHelper.DegreesToRadians(Random(-60, 60));
                square.Rotate(start, endTime, squareRotateStart, squareRotateStart + MathHelper.DegreesToRadians(Random(-25, 25)));
            }
        }

        void KikouHana()
        {
            OsbSprite grid = GetLayer("").CreateSprite("sb/grid.png");
            grid.Scale(167648, 480.0f / 720);
            grid.Fade(167565,167648,0, 0.1);
            grid.Fade(167648,167815, 0.1, 0);
            grid.Fade(169315,169398, 0.1, 0);
            grid.Fade(167981,168231, 0.1, 0);
            grid.Fade(168231,168481, 0.1, 0);
            grid.Fade(168481,168648, 0.1, 0);
            grid.Fade(168648,168898, 0.5, 0);
            grid.Fade(168898,169148, 0.5, 0);
            grid.Fade(169148,169315, 0.5, 0.4);

            List<int> movetimes = new List<int>() {
                169981,
                170231,
                170481,
                170648,
                170815,
                171148,
                171315,
                171481,
                171648,
                171815,
                171981
            };
            var movingbgs = new AnimatedValue<CommandPosition>();
            var rotatingbgs = new AnimatedValue<CommandDecimal>();
            var endtime = 172665;
            foreach (var movetime in movetimes)
            {
                movingbgs.Add(new MoveCommand(OsbEasing.InOutSine, movetime,
                    movetimes.IndexOf(movetime) == movetimes.Count - 1 ? endtime : movetimes[movetimes.IndexOf(movetime) + 1],
                    new Vector2(320, 240), new Vector2(320 + Random(-20, 20), 240 + Random(-20, 20))));
                rotatingbgs.Add(new RotateCommand(OsbEasing.InOutSine, movetime,
                    movetimes.IndexOf(movetime) == movetimes.Count - 1 ? endtime : movetimes[movetimes.IndexOf(movetime) + 1],
                    0, MathHelper.DegreesToRadians(Random(-10.0f, 10.0f))));
            }

            OsbSprite duoToneBG = GetLayer("").CreateSprite("sb/duotone.jpg");
            OsbSprite scanBG = GetLayer("").CreateSprite("sb/scan.jpg");
            OsbSprite invertBG = GetLayer("").CreateSprite("sb/invert.jpg");
            OsbSprite bwBG = GetLayer("").CreateSprite("sb/bw.jpg");



            duoToneBG.Scale(170231, (480.0f / 720) * 1.25);
            scanBG.Scale(170065, (480.0f / 720) * 1.25);
            invertBG.Scale(169981, (480.0f / 720) * 1.25);

            invertBG.Fade(169981, 1);
            invertBG.Move(169981, 170065, movingbgs.ValueAtTime(169981), movingbgs.ValueAtTime(170065));
            invertBG.Rotate(169981, 170065, rotatingbgs.ValueAtTime(169981), rotatingbgs.ValueAtTime(170065));
            invertBG.Fade(170065, 0);
            scanBG.Fade(170065, 1);
            scanBG.Move(170065, 170148, movingbgs.ValueAtTime(170065), movingbgs.ValueAtTime(170148));
            scanBG.Rotate(170065, 170148, rotatingbgs.ValueAtTime(170065), rotatingbgs.ValueAtTime(170148));
            scanBG.Fade(170148, 0);
            duoToneBG.Fade(170231, 1);
            duoToneBG.Move(170231, 170315, movingbgs.ValueAtTime(170231), movingbgs.ValueAtTime(170315));
            duoToneBG.Rotate(170231, 170315, rotatingbgs.ValueAtTime(170231), rotatingbgs.ValueAtTime(170315));
            duoToneBG.Fade(170315, 0);
            scanBG.Fade(170315, 1);
            scanBG.Move(170315, 170481, movingbgs.ValueAtTime(170315), movingbgs.ValueAtTime(170481));
            scanBG.Rotate(170315, 170481, rotatingbgs.ValueAtTime(170315), rotatingbgs.ValueAtTime(170481));
            scanBG.Fade(170481, 0);

            invertBG.FlipH(170481,170648);

            for (int time = 170481; time < 170648; time += 44)
            {
                invertBG.Fade(time, 0.8);
                invertBG.Move(time, time + 22, movingbgs.ValueAtTime(time), movingbgs.ValueAtTime(time + 22));
                invertBG.Rotate(time, time + 22, rotatingbgs.ValueAtTime(time), rotatingbgs.ValueAtTime(time + 22));
                invertBG.Fade(time + 22, 0);
                scanBG.Fade(time + 22, 1);
                scanBG.Move(time, time + 22, movingbgs.ValueAtTime(time), movingbgs.ValueAtTime(time + 22));
                scanBG.Rotate(time, time + 22, rotatingbgs.ValueAtTime(time), rotatingbgs.ValueAtTime(time + 22));
                scanBG.Fade(time + 44, 0);
            }

            duoToneBG.Fade(170648, 1);
            duoToneBG.Move(170648, 170731, movingbgs.ValueAtTime(170648), movingbgs.ValueAtTime(170731));
            duoToneBG.Rotate(170648, 170731, rotatingbgs.ValueAtTime(170648), rotatingbgs.ValueAtTime(170731));
            duoToneBG.Fade(170731, 0);
            scanBG.Fade(170731, 1);
            scanBG.Move(170731, 170815, movingbgs.ValueAtTime(170731), movingbgs.ValueAtTime(170815));
            scanBG.Rotate(170731, 170815, rotatingbgs.ValueAtTime(170731), rotatingbgs.ValueAtTime(170815));
            scanBG.Fade(170815, 0);
            invertBG.Fade(170815, 1);
            invertBG.Move(170815, 170898, movingbgs.ValueAtTime(170815), movingbgs.ValueAtTime(170898));
            invertBG.Rotate(170815, 170898, rotatingbgs.ValueAtTime(170815), rotatingbgs.ValueAtTime(170898));
            invertBG.Fade(170898, 0);
            duoToneBG.Fade(170898, 1);
            duoToneBG.Move(170898, 170981, movingbgs.ValueAtTime(170898), movingbgs.ValueAtTime(170981));
            duoToneBG.Rotate(170898, 170981, rotatingbgs.ValueAtTime(170898), rotatingbgs.ValueAtTime(170981));
            duoToneBG.Fade(170981, 0);
            scanBG.Fade(170981, 1);
            scanBG.Move(170981, 171065, movingbgs.ValueAtTime(170981), movingbgs.ValueAtTime(171065));
            scanBG.Rotate(170981, 171065, rotatingbgs.ValueAtTime(170981), rotatingbgs.ValueAtTime(171065));
            scanBG.Fade(171065, 0);

            bwBG.Scale(171065, (480.0f / 720) * 1.25);
            bwBG.Fade(171065, 1);
            bwBG.Move(171065, 171148, movingbgs.ValueAtTime(171065), movingbgs.ValueAtTime(171148));
            bwBG.Rotate(171065, 171148, rotatingbgs.ValueAtTime(171065), rotatingbgs.ValueAtTime(171148));
            bwBG.Fade(171148, 0);

            invertBG.Fade(171148, 1);
            invertBG.Move(171148, 171231, movingbgs.ValueAtTime(171148), movingbgs.ValueAtTime(171231));
            invertBG.Rotate(171148, 171231, rotatingbgs.ValueAtTime(171148), rotatingbgs.ValueAtTime(171231));
            invertBG.Fade(171231, 0);

            scanBG.Fade(171231, 1);
            scanBG.Move(171231, 171315, movingbgs.ValueAtTime(171231), movingbgs.ValueAtTime(171315));
            scanBG.Rotate(171231, 171315, rotatingbgs.ValueAtTime(171231), rotatingbgs.ValueAtTime(171315));
            scanBG.Fade(171315, 0);

            invertBG.Fade(171315, 1);
            invertBG.Move(171315, 171398, movingbgs.ValueAtTime(171315), movingbgs.ValueAtTime(171398));
            invertBG.Rotate(171315, 171398, rotatingbgs.ValueAtTime(171315), rotatingbgs.ValueAtTime(171398));
            invertBG.Fade(171398, 0);

            duoToneBG.Fade(171398, 1);
            duoToneBG.Move(171398, 171481, movingbgs.ValueAtTime(171398), movingbgs.ValueAtTime(171481));
            duoToneBG.Rotate(171398, 171481, rotatingbgs.ValueAtTime(171398), rotatingbgs.ValueAtTime(171481));
            duoToneBG.Fade(171481, 0);

            invertBG.Fade(171481, 1);
            invertBG.Move(171481, 171565, movingbgs.ValueAtTime(171481), movingbgs.ValueAtTime(171565));
            invertBG.Rotate(171481, 171565, rotatingbgs.ValueAtTime(171481), rotatingbgs.ValueAtTime(171565));
            invertBG.Fade(171565, 0);

            bwBG.Fade(171565, 1);
            bwBG.Move(171565, 171648, movingbgs.ValueAtTime(171565), movingbgs.ValueAtTime(171648));
            bwBG.Rotate(171565, 171648, rotatingbgs.ValueAtTime(171565), rotatingbgs.ValueAtTime(171648));
            bwBG.Fade(171648, 0);

            duoToneBG.Fade(171648, 1);
            duoToneBG.Move(171648, 171731, movingbgs.ValueAtTime(171648), movingbgs.ValueAtTime(171731));
            duoToneBG.Rotate(171648, 171731, rotatingbgs.ValueAtTime(171648), rotatingbgs.ValueAtTime(171731));
            duoToneBG.Fade(171648, 0);

            scanBG.Fade(171731, 1);
            scanBG.Move(171731, 171815, movingbgs.ValueAtTime(171731), movingbgs.ValueAtTime(171815));
            scanBG.Rotate(171731, 171815, rotatingbgs.ValueAtTime(171731), rotatingbgs.ValueAtTime(171815));
            scanBG.Fade(171815, 0);

            bwBG.Fade(171815, 1);
            bwBG.Move(171815, 171981, movingbgs.ValueAtTime(171815), movingbgs.ValueAtTime(171981));
            bwBG.Rotate(171815, 171981, rotatingbgs.ValueAtTime(171815), rotatingbgs.ValueAtTime(171981));
            bwBG.Fade(171981, 0);

            invertBG.Fade(171981, 172665, 1, 0);

            // 169981
            OsbAnimation noise = GetLayer("Foreground").CreateAnimation("sb/noise/.jpg", 4, 100, OsbLoopType.LoopForever);
            noise.Fade(169981, 1);
            noise.Additive(169981);
            noise.Fade(172665, 0);

            var bg = GetLayer("").CreateSprite(Beatmap.BackgroundPath);
            bg.Scale(172665, (480.0f / 1423) * 1.5);
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
                if(movetimes.IndexOf(movetime) % 2 == 0)
                    bg.FlipH(movetime, movetimes.IndexOf(movetime) == movetimes.Count - 1 ? endtime : movetimes[movetimes.IndexOf(movetime) + 1]);
            }
            bg.Fade(endtime, 0);
        }

        // https://canary.discord.com/channels/203050773645492224/483948843571085312/1168005481147682846
        // Credit - TunnelBrick
        public List<(double x, double y)> GetSpiralPoints(double a, int numPoints, double thetaMax, double thetaOffset = 0)
        {
            List<(double x, double y)> points = new List<(double x, double y)>();

            double thetaStep = thetaMax / numPoints;

            for (double theta = 0; theta < thetaMax; theta += thetaStep)
            {
                double r = a * theta;
                double x = r * Math.Cos(theta + thetaOffset);
                double y = r * Math.Sin(theta + thetaOffset);
                points.Add((x, y));
            }

            return points;
        }
    }
}
