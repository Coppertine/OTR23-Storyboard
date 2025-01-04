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
    public class Cowbell : StoryboardObjectGenerator
    {
        [Configurable]
        public Color4 Color1 = new Color4();
        [Configurable]
        public Color4 Color2 = new Color4();
        [Configurable]
        public Color4 Color3 = new Color4();
        [Configurable]
        public Color4 Color4 = new Color4();
        [Configurable]
        public Color4 Color5 = new Color4();
        public override void Generate()
        {
            OsbSprite background = GetLayer("").CreateSprite("sb/p.png");
            background.ScaleVec(193981, 853, 480);
            background.Fade(193981, 195648, 1, 1);
            background.Color(193981, Color1);
            background.Color(194315, Color2);
            background.Color(194648, Color3);
            background.Color(194981, Color4);
            background.Color(195315, Color5);

            OsbSprite cowbell = GetLayer("").CreateSprite("sb/cowbell/bell.png", OsbOrigin.Centre, new Vector2(320, 240));
            cowbell.Scale(193981, 0.5);
            cowbell.Fade(193981, 195648, 1, 1);

            OsbSprite mallet = GetLayer("").CreateSprite("sb/cowbell/mallet.png", OsbOrigin.BottomCentre, new Vector2(570, 372));
            mallet.Scale(193981, 0.5);
            mallet.Fade(193981, 195648, 1, 1);
            double halfBeat = Beatmap.GetTimingPointAt(193981).BeatDuration / 2;
            mallet.StartLoopGroup(193981, 5);
            mallet.Rotate(OsbEasing.OutSine, 0, halfBeat, MathHelper.DegreesToRadians(-45), 0);
            mallet.Rotate(OsbEasing.InSine, halfBeat, halfBeat * 2, 0, MathHelper.DegreesToRadians(-45));
            mallet.EndGroup();


            /// Section after Cowbell... because fuck


            Wobble(195648, 203315, 1, 10, 10, true, MathHelper.DegreesToRadians(10));

            // Vector2 currPos = new Vector2(320, 240);
            // float currRot = 0;
            // double currTime = 195648;
            // Beatmap.ForEachTick(195648, 203315, 1, (point, time, beat, tick) =>
            // {
            //     if (beat % 8 == 0)
            //     {
            //         Vector2 pos = new Vector2(
            //             320 + Random(-10, 10),
            //             240 + Random(-10, 10)
            //         );
            //         beatmapBG.Move(currTime, time, currPos, pos);
            //         float rot = (float)Random(MathHelper.DegreesToRadians(-5), MathHelper.DegreesToRadians(5));
            //         beatmapBG.Rotate(currTime, time, currRot, rot);
            //         currPos = pos; 
            //         currTime = time;
            //         currRot = rot;
            //     }
            // });
            int offset = 50;
            OsbSprite beatmapBG = GetLayer("").CreateSprite(Beatmap.BackgroundPath);

            beatmapBG.Scale(203332, (480.0f / 1423) * 2);
            beatmapBG.Move(OsbEasing.OutExpo, 203332, 203680, new Vector2(320 + Random(-offset, offset), 240 + Random(-offset, offset)),
                new Vector2(320 + Random(-offset, offset), 240 + Random(-offset, offset)));
            beatmapBG.Move(OsbEasing.OutExpo, 203680, 204080, new Vector2(320 + Random(-offset, offset), 240 + Random(-offset, offset)),
                new Vector2(320 + Random(-offset, offset), 240 + Random(-offset, offset)));
            beatmapBG.FlipH(203680, 204080);
            beatmapBG.Move(204067, new Vector2(320,240));
            beatmapBG.Scale(OsbEasing.OutExpo, 204067, 208068, (480.0f / 1423) * 2, (480.0f / 1423));
            beatmapBG.Fade(OsbEasing.InExpo,204067,208068, 1, 0);


            /** Spectrum **/
            var spectrumSpritePath = "sb/light.png";
            var spectrumSpriteOrigin = OsbOrigin.Centre;
            var spectrumSpritePosition = new Vector2(-107, 480);
            int barCount = 15;
            var beatDivisor = 8;
            var easing = OsbEasing.InOutExpo;
            float Width = 847;
            var FrequencyCutOff = 16000;
            int LogScale = 600;
            float MinimalHeight = 0.1f;
            var spriteScale = new Vector2(20, 300);
            double Tolerance = 0.02;

            int startTime = 195665;
            int endTime = 208113;


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
                bar.Fade(204080, endTime, 0.3, 0);

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

        OsbSprite back,main,main_defocus;
        public void Wobble(int StartTime, int EndTime, double Opacity, int RandX, int RandY, Boolean ShouldRotate, double RandRotate, int offsety = 0, int offsetx = 0)
        {
            // var bitmap1 = GetMapsetBitmap("sb/p.png");
            back = GetLayer("").CreateSprite("sb/p.png");

            var mainbitmap = GetMapsetBitmap("sb/girl.png");
            var main_defocusbitmap = GetMapsetBitmap("sb/girl_defocus.png");

            main = GetLayer("").CreateSprite("sb/girl.png");
            main_defocus = GetLayer("").CreateSprite("sb/girl_defocus.png");

            var overlay = GetLayer("").CreateSprite("sb/layer/overlay.png");
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
