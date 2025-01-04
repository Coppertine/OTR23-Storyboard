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
    public class Main : StoryboardObjectGenerator
    {
        public override void Generate()
        {
            RemoveBackground();
            GenerateIntro();

        }

        public void RemoveBackground() => GetLayer("").CreateSprite(Beatmap.BackgroundPath).Fade(0, 0);
        /**

        **/
        public void GenerateIntro()
        {
            StoryboardLayer layer = GetLayer("Intro");
            OsbSprite background = layer.CreateSprite(Beatmap.BackgroundPath);
            background.Scale(71316, (480.0f / 1423) * 1.25);
            background.Fade(71316, 71833, 0, 1);
            background.Fade(87316, 0);
            float currRot = 0;
            Vector2 currPos = new Vector2(320, 240);
            double currTime = 71316;
            Beatmap.ForEachTick(71316, 89982, 1, (point, time, beat, tick) =>
            {
                if (beat % 8 == 0)
                {
                    Vector2 pos = new Vector2(
                        320 + Random(-10, 10),
                        240 + Random(-10, 10)
                    );
                    background.Move(OsbEasing.InOutSine, currTime, time, currPos, pos);
                    float rot = (float)Random(MathHelper.DegreesToRadians(-5), MathHelper.DegreesToRadians(5));
                    background.Rotate(OsbEasing.InOutSine, currTime, time, currRot, rot);
                    currPos = pos; 
                    currTime = time;
                    currRot = rot;
                }
            });

            OsbSprite introbg = layer.CreateSprite("sb/p.png");
            introbg.ScaleVec(0,854,480);
            introbg.Fade(0, 4000, 0, 1);
            introbg.Color(0, "#111320");
            introbg.Fade(71316,0);
            OsbSprite glow = layer.CreateSprite("sb/g.png", OsbOrigin.CentreLeft, new Vector2(320, 480));
            glow.Rotate(0, -Math.PI / 2);
            glow.Fade(OsbEasing.InOutQuad, 0, 4000, 0, 0.8);
            glow.Additive(0);
            glow.Color(0, new Color4(255, 216, 117, 255));
            Vector2 glowStartScale = new Vector2(0.4f, 9);
            Vector2 glowEndScale = new Vector2(0.5f, 9);
            glow.StartLoopGroup(0, (int)((87316) / (int)Beatmap.GetTimingPointAt(48).BeatDuration / 8) + 1);
            glow.ScaleVec(OsbEasing.InOutSine, 0, Beatmap.GetTimingPointAt(48).BeatDuration * 4, glowStartScale, glowEndScale);
            glow.ScaleVec(OsbEasing.InOutSine, Beatmap.GetTimingPointAt(48).BeatDuration * 4, Beatmap.GetTimingPointAt(48).BeatDuration * 8, glowEndScale, glowStartScale);
            glow.EndGroup();
            glow.Fade(87316, 0);


            // Black White bg
            OsbSprite bw = layer.CreateSprite("sb/bw.jpg");
            bw.Fade(87316, 0.5);
            bw.Scale(87316, 480.0f / 720);
            bw.Fade(89982,0);

            OsbSprite noise = layer.CreateAnimation("sb/noise/.jpg", 3, 50, OsbLoopType.LoopForever);
            noise.Fade(87316, 1);
            noise.Additive(87316);
            noise.Fade(89982,0);

            OsbSprite backgroundCloseups = layer.CreateSprite(Beatmap.BackgroundPath, OsbOrigin.Centre, new Vector2(0,0));
            backgroundCloseups.Scale(89982, (480.0f / 1423) * 2);
            backgroundCloseups.Fade(OsbEasing.OutExpo, 89982, 90482, 1, 0.8);
            backgroundCloseups.Move(OsbEasing.OutExpo, 89982, 90482, new Vector2(580, 250), new Vector2(550, 270));
            backgroundCloseups.Move(OsbEasing.OutExpo, 90482, 90982, new Vector2(100, 200), new Vector2(50, 270));
            backgroundCloseups.Scale(OsbEasing.OutCubic, 90982, 92665, (480.0f / 1423) * 2,(480.0f / 1423));
            backgroundCloseups.Move(90982, new Vector2(320, 240));

            backgroundCloseups.Fade(92665, 0);
        }

        public void Wobble(int StartTime, int EndTime, double Opacity, int RandX, int RandY, Boolean ShouldRotate, double RandRotate, int offsety = 0, int offsetx = 0)
        {
            var bitmap1 = GetMapsetBitmap("sb/layer/base.jpg");
            var back = GetLayer("BACKGROUND").CreateSprite("sb/layer/base.jpg");

            var main = GetLayer("BACKGROUND").CreateSprite("sb/layer/main.png");
            var main_defocus = GetLayer("BACKGROUND").CreateSprite("sb/layer/main_defocus.png");

            var overlay = GetLayer("BACKGROUND").CreateSprite("sb/layer/overlay.png");
            var effect = GetLayer("BACKGROUND").CreateSprite("sb/layer/effect.png");
            effect.Additive(StartTime);

            OsbSprite[] sprites = { back, overlay };
            OsbSprite[] sprites_group = { main, main_defocus };

            int beat = (int)Beatmap.GetTimingPointAt(StartTime).BeatDuration;
            int measure = beat * 4;
            foreach (OsbSprite bg in sprites)
            {
                bg.Fade(0, 0);
                bg.Fade(StartTime, Opacity);
                bg.Fade(EndTime, 0);
                bg.Scale(StartTime, (480.0f / bitmap1.Height) + 0.01);
            }
            foreach (OsbSprite bg in sprites_group)
            {
                bg.Fade(0, 0);
                bg.Fade(StartTime, Opacity);
                bg.Fade(EndTime, 0);
                bg.Scale(StartTime, (480.0f / bitmap1.Height) + 0.01);
            }

            effect.Fade(0, 0);
            effect.Fade(StartTime, 0.4);
            effect.Fade(EndTime, 0);
            effect.Scale(StartTime, (480.0f / bitmap1.Height) + 0.01);

            var time_lag = measure;
            double x = 320 + offsetx;
            double y = 240 + offsety;


            var initRandX = Random(1, RandX);
            var initRandY = Random(1, RandX);

            double xm = 320 + offsetx - initRandX;
            double ym = 240 + offsety - initRandY;

            double xr = 0;
            double yr = 0;
            double rad = 0;
            double rad1 = rad + Random(-RandRotate, RandRotate);
            int a = 1;
            foreach (OsbSprite bg in sprites)
            {
                for (double i = StartTime; i <= EndTime; i += time_lag * 4)
                {
                    bg.Rotate(OsbEasing.InOutQuad, i, i + time_lag * 4, rad, rad1);

                    rad = rad1;
                    rad1 = rad + Random(0.001, RandRotate) * a;
                    a *= -1;
                    if (!ShouldRotate)
                        continue;
                }
            }
            foreach (OsbSprite bg in sprites)
            {
                if (bg == overlay)
                {
                    offsetx = -10;
                }
                else
                {
                    offsetx = 0;
                }
                for (double i = StartTime; i <= EndTime; i += time_lag * 4)
                {
                    bg.Move(OsbEasing.InOutSine, i, i + time_lag * 2, x, y, xm + xr, ym + yr);
                    bg.Move(OsbEasing.InOutSine, i + time_lag * 2, i + time_lag * 4, xm + xr, ym + yr, 320 + offsetx + xr, 240 + offsety + yr);

                    x = 320 + offsetx + xr;
                    y = 240 + offsety + yr;
                    if (bg == back)
                    {
                        xr = Random(1, RandX * 0.6);
                        yr = Random(1, RandY * 0.6);
                    }
                    else if (bg == overlay)
                    {
                        xr = Random(5, RandX * 2);
                        yr = Random(5, RandY * 2);
                    }
                    else
                    {
                        xr = Random(3, RandX);
                        yr = Random(3, RandY);
                    }
                }
            }


            for (double i = StartTime; i <= EndTime; i += time_lag * 4)
            {
                main.Move(OsbEasing.InOutSine, i, i + time_lag * 2, x, y, xm + xr, ym + yr);
                main.Move(OsbEasing.InOutSine, i + time_lag * 2, i + time_lag * 4, xm + xr, ym + yr, 320 + offsetx + xr, 240 + offsety + yr);
                main_defocus.Move(OsbEasing.InOutSine, i, i + time_lag * 2, x, y, xm + xr, ym + yr);
                main_defocus.Move(OsbEasing.InOutSine, i + time_lag * 2, i + time_lag * 4, xm + xr, ym + yr, 320 + offsetx + xr, 240 + offsety + yr);

                x = 320 + offsetx + xr;
                y = 240 + offsety + yr;
            }
            for (double i = StartTime; i <= EndTime; i += time_lag * 4)
            {
                main.Rotate(OsbEasing.InOutQuad, i, i + time_lag * 4, rad, rad1);
                main_defocus.Rotate(OsbEasing.InOutQuad, i, i + time_lag * 4, rad, rad1);

                rad = rad1;
                a *= -1;
                rad1 = rad + Random(0.001, RandRotate) * a;
                if (!ShouldRotate)
                    continue;
            }

            //Blur effect
            var timeLagInterval = time_lag * 2.2;
            for (double i = StartTime; i <= EndTime - timeLagInterval - 3000; i += timeLagInterval)
            {
                main_defocus.Fade(OsbEasing.InOutSine, i, i + timeLagInterval / 2, 0, 0.75);
                main_defocus.Fade(OsbEasing.InOutSine, i + timeLagInterval / 2, i + timeLagInterval, 0.75, 0);
            }
            //Zoom effect

            foreach (OsbSprite bg in sprites)
            {
                var timeLagIntervalZoom = time_lag * Random(2.9, 3.8);
                for (double i = StartTime; i <= EndTime - timeLagIntervalZoom; i += timeLagIntervalZoom)
                {
                    double randScale;
                    if (bg == back)
                    {
                        randScale = 0;
                    }
                    else
                    {
                        randScale = Random(0.012, 0.021);
                    }

                    bg.Scale(OsbEasing.InOutSine, i, i + timeLagIntervalZoom / 2, (485.0f / bitmap1.Height) + 0.05, (485.0f / bitmap1.Height) + 0.05 + randScale);
                    bg.Scale(OsbEasing.InOutSine, i + timeLagIntervalZoom / 2, i + timeLagIntervalZoom, (485.0f / bitmap1.Height) + 0.05 + randScale, (485.0f / bitmap1.Height) + 0.05);
                }
            }
            var timeLagIntervalZoom2 = time_lag * Random(2.9, 3.8);
            for (double i = StartTime; i <= EndTime - timeLagIntervalZoom2; i += timeLagIntervalZoom2)
            {
                var randScale = Random(0.012, 0.021);
                main.Scale(OsbEasing.InOutSine, i, i + timeLagIntervalZoom2 / 2, (485.0f / bitmap1.Height) + 0.07, (485.0f / bitmap1.Height) + 0.07 + randScale);
                main.Scale(OsbEasing.InOutSine, i + timeLagIntervalZoom2 / 2, i + timeLagIntervalZoom2, (485.0f / bitmap1.Height) + 0.07 + randScale, (485.0f / bitmap1.Height) + 0.07);
                main_defocus.Scale(OsbEasing.InOutSine, i, i + timeLagIntervalZoom2 / 2, (485.0f / bitmap1.Height) + 0.07, (485.0f / bitmap1.Height) + 0.07 + randScale);
                main_defocus.Scale(OsbEasing.InOutSine, i + timeLagIntervalZoom2 / 2, i + timeLagIntervalZoom2, (485.0f / bitmap1.Height) + 0.07 + randScale, (485.0f / bitmap1.Height) + 0.07);
            }

        }
    }
}
