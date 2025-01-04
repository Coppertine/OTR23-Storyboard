using OpenTK;
using OpenTK.Graphics;
using StorybrewCommon.Animations;
using StorybrewCommon.Mapset;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Storyboarding.Util;
using StorybrewCommon.Subtitles;
using StorybrewCommon.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace StorybrewScripts
{
    public class Drop4 : StoryboardObjectGenerator
    {
        public override void Generate()
        {

            //  OsbSprite bg = GetLayer("Background").CreateSprite(Beatmap.BackgroundPath);
            // bg.Scale(348763, (480.0f / 1423) * 1.25);
            // bg.Fade(348763, 1);
            // bg.Fade(393130, 0);
            Wobble(348763,393130, 1, 10,10, true, MathHelper.DegreesToRadians(10));

            GenerateSpirals(348763, 370096, 8, 500, (int)Beatmap.GetTimingPointAt(348763).BeatDuration * 3, (int)Beatmap.GetTimingPointAt(348763).BeatDuration / 6, 1, Color4.Magenta);
            GenerateSpirals(348763, 370096, 8, 500, (int)Beatmap.GetTimingPointAt(348763).BeatDuration * 6, (int)Beatmap.GetTimingPointAt(348763).BeatDuration / 6,-1, Color4.MediumOrchid);


            // OsbSprite girlOverlay = GetLayer("Girl Overlay").CreateSprite("sb/girl.png");
            // girlOverlay.Scale(348763, (480.0f / 1423) * 1.25);
            // girlOverlay.Fade(348763, 1);
            // girlOverlay.Fade(393130, 0);

            // float currRot = 0;
            // Vector2 currPos = new Vector2(320, 240);
            // double currTime = 348763;
            // Beatmap.ForEachTick(348763, 393130, 1, (point, time, beat, tick) =>
            // {
            //     if (beat % 8 == 0)
            //     {
            //         Vector2 pos = new Vector2(
            //             320 + Random(-10, 10),
            //             240 + Random(-10, 10)
            //         );
            //         bg.Move(currTime, time, currPos, pos);
            //         girlOverlay.Move(currTime, time, currPos, pos);
            //         float rot = (float)Random(MathHelper.DegreesToRadians(-5), MathHelper.DegreesToRadians(5));
            //         bg.Rotate(currTime, time, currRot, rot);
            //         girlOverlay.Rotate(currTime, time, currRot, rot);
            //         currPos = pos;
            //         currTime = time;
            //         currRot = rot;
            //     }
            // });
		    
            /** Spectrum **/
            var spectrumSpritePath = "sb/light.png";
            var spectrumSpriteOrigin = OsbOrigin.Centre;
            var spectrumSpritePosition = new Vector2(-107,480);
            int barCount = 15;
            var startTime = 348763;
            var endTime = 393130;
            var beatDivisor = 8;
            var easing = OsbEasing.InOutExpo;
            float Width = 847;
            var FrequencyCutOff = 16000;
            int LogScale = 600;
            float MinimalHeight = 0.1f;
            var spriteScale = new Vector2(20,300);
            double Tolerance = 0.02;
                        
            Bitmap spectrumSpriteBitmap = GetMapsetBitmap(spectrumSpritePath);

            KeyframedValue<float>[] heightKeyframes = new KeyframedValue<float>[barCount];
            for (var i = 0; i < barCount; i++)
                heightKeyframes[i] = new KeyframedValue<float>(null);
            double fftTimeStep = Beatmap.GetTimingPointAt(startTime).BeatDuration / beatDivisor;
            double fftOffset = fftTimeStep * 0.2;
            for (var time = (double)startTime; time < endTime; time += fftTimeStep)
            {
                var fft = GetFft(time + fftOffset, barCount, null, easing, FrequencyCutOff);
                for (var i = 0; i < barCount; i++)
                {
                    var height = (float)Math.Log10(1 + fft[i] * LogScale) * spriteScale.Y / spectrumSpriteBitmap.Height;
                    if (height < MinimalHeight) height = MinimalHeight;

                    heightKeyframes[i].Add(time, height);
                }
            }

            var spectrumLayer = GetLayer("Spectrum");
            var barWidth = Width / barCount;
            for (var i = 0; i < barCount; i++)
            {
                var keyframes = heightKeyframes[i];
                keyframes.Simplify1dKeyframes(Tolerance, h => h);

                var bar = spectrumLayer.CreateSprite(spectrumSpritePath, spectrumSpriteOrigin, new Vector2(spectrumSpritePosition.X + i * barWidth, spectrumSpritePosition.Y));
                bar.CommandSplitThreshold = 300;
                bar.ColorHsb(startTime, (i * 360.0 / barCount) + Random(-20.0, 10.0), 0.6 + Random(0.4), 1);
                bar.Additive(startTime, endTime);
                bar.Fade(startTime,0.3);

                var scaleX = spriteScale.X * barWidth / spectrumSpriteBitmap.Width;
                scaleX = (float)Math.Floor(scaleX * 10) / 10.0f;

                var hasScale = false;
                keyframes.ForEachPair(
                    (start, end) =>
                    {
                        hasScale = true;
                        bar.ScaleVec(start.Time, end.Time,
                            scaleX, start.Value,
                            scaleX, end.Value);
                    },
                    MinimalHeight,
                    s => (float)Math.Round(s, 2)
                );
                if (!hasScale) bar.ScaleVec(startTime, scaleX, MinimalHeight);
            }
        }

        public void GenerateSpirals(int startTime, int endTime, int count, int distance, int duration, int timeBetween, int angleMove, Color4 spriteColor)
        {

            double angle = (2 * Math.PI)/count;
            OsbSpritePool spritePool;
            
            spritePool = new OsbSpritePool(GetLayer("KiaiSpiral"), "sb/dot.png", OsbOrigin.Centre, (sprite, StartTime, EndTime) => {
                sprite.Fade(StartTime,EndTime,1, 1);
                sprite.Color(StartTime, spriteColor);
            });
            spritePool.MaxPoolDuration = 2000;
            using(spritePool)
            {
                float CurrentangleExtra = 0;
                for(int time = startTime; time <= endTime; time += timeBetween)
                {
                    for(int i = 1; i <= count; i++)
                    {
                        var radius = distance;
                        var anglePos = new Vector2(
                            340 + radius * (float)Math.Cos((angle * i) + CurrentangleExtra),
                            240 + radius * (float)Math.Sin((angle * i) + CurrentangleExtra));
                        var anglePosStart = new Vector2(
                            340 + 65 * (float)Math.Cos((angle * i) + CurrentangleExtra),
                            240 + 65 * (float)Math.Sin((angle * i) + CurrentangleExtra));
                        var sprite = spritePool.Get(time, time + duration);
                        sprite.Move(OsbEasing.None, time, time + duration, anglePosStart, anglePos);
                    }
                    CurrentangleExtra += MathHelper.DegreesToRadians(angleMove);
                }
            }
        }

          public void Wobble(int StartTime, int EndTime, double Opacity, int RandX, int RandY, Boolean ShouldRotate, double RandRotate, int offsety = 0, int offsetx = 0)
        {
            var bitmap1 = GetMapsetBitmap("sb/p.png");
            var back = GetLayer("Background").CreateSprite("sb/p.png");

            var mainbitmap = GetMapsetBitmap("sb/girl.png");
            var main_defocusbitmap = GetMapsetBitmap("sb/girl_defocus.png");

            var main = GetLayer("Girl Overlay").CreateSprite("sb/girl.png");
            var main_defocus = GetLayer("Girl Overlay").CreateSprite("sb/girl_defocus.png");

            var overlay = GetLayer("Background").CreateSprite("sb/layer/overlay.png");
            // var effect = GetLayer("BACKGROUND").CreateSprite("sb/layer/effect.png");
            // effect.Additive(StartTime);

            OsbSprite[] sprites = { back };
            OsbSprite[] sprites_group = { main, main_defocus };

            int beat = (int)Beatmap.GetTimingPointAt(StartTime).BeatDuration;
            int measure = beat * 4;
            foreach (OsbSprite bg in sprites)
            {
                // bg.Fade(0, 0);
                bg.Fade(StartTime, Opacity);
                bg.Fade(EndTime, 0);
                bg.ScaleVec(StartTime, new Vector2(847, 480) * 1.2f);
                bg.Color(StartTime, new Color4(20,23,38, 255));
            }
            foreach (OsbSprite bg in sprites_group)
            {
                // bg.Fade(0, 0);
                bg.Fade(StartTime, Opacity);
                bg.Fade(EndTime, 0);
            }
            main.Scale(StartTime, (480.0f / mainbitmap.Height) + 0.01);
            main.Scale(StartTime, (480.0f / main_defocusbitmap.Height) + 0.01);
            

            // effect.Fade(0, 0);
            // effect.Fade(StartTime, 0.4);
            // effect.Fade(EndTime, 0);
            // effect.Scale(StartTime, (480.0f / bitmap1.Height) + 0.01);

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
            // foreach (OsbSprite bg in sprites)
            // {
            //     for (double i = StartTime; i <= EndTime; i += time_lag * 4)
            //     {
            //         bg.Rotate(OsbEasing.InOutQuad, i, i + time_lag * 4, rad, rad1);

            //         rad = rad1;
            //         rad1 = rad + Random(0.001, RandRotate) * a;
            //         a *= -1;
            //         if (!ShouldRotate)
            //             continue;
            //     }
            // }
            // foreach (OsbSprite bg in sprites)
            // {
            //     if (bg == overlay)
            //     {
            //         offsetx = -10;
            //     }
            //     else
            //     {
            //         offsetx = 0;
            //     }
            //     for (double i = StartTime; i <= EndTime; i += time_lag * 4)
            //     {
            //         bg.Move(OsbEasing.InOutSine, i, i + time_lag * 2, x, y, xm + xr, ym + yr);
            //         bg.Move(OsbEasing.InOutSine, i + time_lag * 2, i + time_lag * 4, xm + xr, ym + yr, 320 + offsetx + xr, 240 + offsety + yr);

            //         x = 320 + offsetx + xr;
            //         y = 240 + offsety + yr;
            //         if (bg == back)
            //         {
            //             xr = Random(1, RandX * 0.6);
            //             yr = Random(1, RandY * 0.6);
            //         }
            //         else if (bg == overlay)
            //         {
            //             xr = Random(5, RandX * 2);
            //             yr = Random(5, RandY * 2);
            //         }
            //         else
            //         {
            //             xr = Random(3, RandX);
            //             yr = Random(3, RandY);
            //         }
            //     }
            // }


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

                    // bg.Scale(OsbEasing.InOutSine, i, i + timeLagIntervalZoom / 2, (485.0f / bitmap1.Height) + 0.05, (485.0f / bitmap1.Height) + 0.05 + randScale);
                    // bg.Scale(OsbEasing.InOutSine, i + timeLagIntervalZoom / 2, i + timeLagIntervalZoom, (485.0f / bitmap1.Height) + 0.05 + randScale, (485.0f / bitmap1.Height) + 0.05);
                }
            }
            var timeLagIntervalZoom2 = time_lag * Random(2.9, 3.8);
            for (double i = StartTime; i <= EndTime - timeLagIntervalZoom2; i += timeLagIntervalZoom2)
            {
                var randScale = Random(0.012, 0.021);
                main.Scale(OsbEasing.InOutSine, i, i + timeLagIntervalZoom2 / 2, (485.0f / mainbitmap.Height) + 0.07, (485.0f / mainbitmap.Height) + 0.07 + randScale);
                main.Scale(OsbEasing.InOutSine, i + timeLagIntervalZoom2 / 2, i + timeLagIntervalZoom2, (485.0f / mainbitmap.Height) + 0.07 + randScale, (485.0f / mainbitmap.Height) + 0.07);
                main_defocus.Scale(OsbEasing.InOutSine, i, i + timeLagIntervalZoom2 / 2, (485.0f / main_defocusbitmap.Height) + 0.07, (485.0f / main_defocusbitmap.Height) + 0.07 + randScale);
                main_defocus.Scale(OsbEasing.InOutSine, i + timeLagIntervalZoom2 / 2, i + timeLagIntervalZoom2, (485.0f / main_defocusbitmap.Height) + 0.07 + randScale, (485.0f / main_defocusbitmap.Height) + 0.07);
            }

        }
    }
}
