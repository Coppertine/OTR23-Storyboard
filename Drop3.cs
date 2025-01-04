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
    public class Drop3 : StoryboardObjectGenerator
    {
        OsbSprite invert;
        public override void Generate()
        {
            /**
                List to add in.
                * Paralax bg
                * Particles
                * Spectrum
            **/
            StoryboardLayer layer = GetLayer("Background");
            // OsbSprite background = layer.CreateSprite(Beatmap.BackgroundPath);
            Wobble(306096,342096, 1, 10,10, true, MathHelper.DegreesToRadians(10));
            
            invert = layer.CreateSprite("sb/invert.jpg");
            // background.Scale(306096, (480.0f / 1423) * 1.25);
            invert.Scale(306096, (480.0f / 720) * 1.25);
            // background.Fade(306096, 1);
            // background.Fade(342096, 0);
            invert.Fade(306096, 0);
            // float currRot = 0;
            // Vector2 currPos = new Vector2(320, 240);
            // double currTime = 306096;
            // Beatmap.ForEachTick(306096, 342096, 1, (point, time, beat, tick) =>
            // {
            //     if (beat % 8 == 0)
            //     {
            //         Vector2 pos = new Vector2(
            //             320 + Random(-10, 10),
            //             240 + Random(-10, 10)
            //         );
            //         background.Move(currTime, time, currPos, pos);
            //         float rot = (float)Random(MathHelper.DegreesToRadians(-5), MathHelper.DegreesToRadians(5));
            //         background.Rotate(currTime, time, currRot, rot);
            //         currPos = pos;
            //         currTime = time;
            //         currRot = rot;
            //     }
            // });
            

            OsbSprite bgFlash = layer.CreateSprite(Beatmap.BackgroundPath);
            bgFlash.Scale(OsbEasing.OutExpo, 311430, 311846, (480.0f / 1423) * 1.0125, (480.0f / 1423) * 1.333);
            bgFlash.Rotate(311430, 311846, main.RotationAt(311430), main.RotationAt(311846));
            bgFlash.Move(311430, 311846, main.PositionAt(311430), main.PositionAt(311846));
            bgFlash.Fade(OsbEasing.OutExpo, 311430, 311846, 1, 0);
            bgFlash.Additive(311430);

            // Off to do a loop for 4 times
            int currtime = 314096;
            int timestep = (int)Beatmap.GetTimingPointAt(currtime).BeatDuration * 2;
            for (int i = 0; i <= 4; i++)
            {
                bgFlash.Scale(OsbEasing.OutExpo, currtime, currtime + timestep, (480.0f / 1423) * 1.25, (480.0f / 1423) * 1.75);
                bgFlash.Rotate(currtime, currtime + timestep, main.RotationAt(currtime), main.RotationAt(currtime + timestep));
                bgFlash.Move(currtime, currtime + timestep, main.PositionAt(currtime), main.PositionAt(currtime + timestep));
                bgFlash.Fade(OsbEasing.OutExpo, currtime, currtime + timestep, 1, 0);
                currtime += timestep;
            }

            bgFlash.Scale(OsbEasing.OutExpo, 322096, 322430, (480.0f / 1423) * 1.25, (480.0f / 1423) * 1.75);
            bgFlash.Rotate(322096, 322430, main.RotationAt(322096), main.RotationAt(322430));
            bgFlash.Move(322096, 322430, main.PositionAt(322096), main.PositionAt(322430));
            bgFlash.Fade(OsbEasing.OutExpo, 322096, 322430, 1, 0);

            currtime = 324763;
            for (int i = 0; i < 4; i++)
            {
                bgFlash.Scale(OsbEasing.OutExpo, currtime, currtime + timestep, (480.0f / 1423) * 1.25, (480.0f / 1423) * 1.75);
                bgFlash.Rotate(currtime, currtime + timestep, main.RotationAt(currtime), main.RotationAt(currtime + timestep));
                bgFlash.Move(currtime, currtime + timestep, main.PositionAt(currtime), main.PositionAt(currtime + timestep));
                bgFlash.Fade(OsbEasing.OutExpo, currtime, currtime + timestep, 1, 0);
                currtime += timestep;
            }


            FlashInvert(OsbEasing.OutExpo, main, 327413, 1500, true);
            FlashInvert(OsbEasing.OutExpo, main, 329746, 329996, true);
            FlashInvert(OsbEasing.OutExpo, main, 330079, 1500, true);
            FlashInvert(OsbEasing.OutExpo, main, 332763, 1500, true);


            FlashInvert(OsbEasing.OutExpo, main, 334746, 334996, true);
            FlashInvert(OsbEasing.OutExpo, main, 335079, 335413, true);

            FlashInvert(OsbEasing.OutExpo, main, 335413, 1500, true);
            FlashInvert(OsbEasing.OutExpo, main, 336079, 1500, true);
            FlashInvert(OsbEasing.OutExpo, main, 336746, 1500, true);


            /** Spectrum **/
            var spectrumSpritePath = "sb/light.png";
            var spectrumSpriteOrigin = OsbOrigin.Centre;
            var spectrumSpritePosition = new Vector2(-107, 480);
            int barCount = 15;
            var startTime = 306096;
            var endTime = 343430;
            var beatDivisor = 8;
            var easing = OsbEasing.InOutExpo;
            float Width = 847;
            var FrequencyCutOff = 16000;
            int LogScale = 600;
            float MinimalHeight = 0.1f;
            var spriteScale = new Vector2(20, 300);
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
                bar.ColorHsb(startTime, (i * 360.0 / barCount) + Random(-10.0, 10.0), 0.6 + Random(0.4), 1);
                bar.Additive(startTime, endTime);
                bar.Fade(startTime, 0.3);

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

        private void FlashInvert(OsbEasing easing, OsbSprite referenceBackground, int startTime, int duration, bool additive)
        {
            // Assuming X and Y are the same... I HOPE
            float endScale = (referenceBackground.ScaleAt(startTime).X) * 1.125f;
            int endTime = duration < startTime ? startTime + duration : duration;
            invert.Rotate(startTime, endTime, referenceBackground.RotationAt(startTime), referenceBackground.RotationAt(endTime));
            invert.Move(startTime, endTime, referenceBackground.PositionAt(startTime), referenceBackground.PositionAt(endTime));
            invert.Scale(easing, startTime, endTime, referenceBackground.ScaleAt(startTime).X, endScale);
            invert.Fade(easing, startTime, endTime, 0.5, 0);
            if (additive)
                invert.Additive(startTime, endTime);
        }
        OsbSprite back,main,main_defocus;

        public void Wobble(int StartTime, int EndTime, double Opacity, int RandX, int RandY, Boolean ShouldRotate, double RandRotate, int offsety = 0, int offsetx = 0)
        {
            // var bitmap1 = GetMapsetBitmap("sb/p.png");
            back = GetLayer("Background").CreateSprite("sb/p.png");

            var mainbitmap = GetMapsetBitmap("sb/girl.png");
            var main_defocusbitmap = GetMapsetBitmap("sb/girl_defocus.png");

            main = GetLayer("Background").CreateSprite("sb/girl.png");
            main_defocus = GetLayer("Background").CreateSprite("sb/girl_defocus.png");

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
