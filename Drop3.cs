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
            OsbSprite background = layer.CreateSprite(Beatmap.BackgroundPath);
            invert = layer.CreateSprite(Beatmap.BackgroundPath);
            background.Scale(306096, (480.0f / 1423) * 1.25);
            invert.Scale(306096, (480.0f / 1423) * 1.25);
            background.Fade(306096, 1);
            background.Fade(342096, 0);
            invert.Fade(306096, 0);
            float currRot = 0;
            Vector2 currPos = new Vector2(320, 240);
            double currTime = 306096;
            Beatmap.ForEachTick(306096, 342096, 1, (point, time, beat, tick) =>
            {
                if (beat % 8 == 0)
                {
                    Vector2 pos = new Vector2(
                        320 + Random(-10, 10),
                        240 + Random(-10, 10)
                    );
                    background.Move(currTime, time, currPos, pos);
                    float rot = (float)Random(MathHelper.DegreesToRadians(-5), MathHelper.DegreesToRadians(5));
                    background.Rotate(currTime, time, currRot, rot);
                    currPos = pos;
                    currTime = time;
                    currRot = rot;
                }
            });

            OsbSprite bgFlash = layer.CreateSprite(Beatmap.BackgroundPath);
            bgFlash.Scale(OsbEasing.OutExpo, 311430, 311846, (480.0f / 1423) * 1.25, (480.0f / 1423) * 1.75);
            bgFlash.Rotate(311430, 311846, background.RotationAt(311430), background.RotationAt(311846));
            bgFlash.Move(311430, 311846, background.PositionAt(311430), background.PositionAt(311846));
            bgFlash.Fade(OsbEasing.OutExpo, 311430, 311846, 1, 0);
            bgFlash.Additive(311430);

            // Off to do a loop for 4 times
            int currtime = 314096;
            int timestep = (int)Beatmap.GetTimingPointAt(currtime).BeatDuration * 2;
            for (int i = 0; i <= 4; i++)
            {
                bgFlash.Scale(OsbEasing.OutExpo, currtime, currtime + timestep, (480.0f / 1423) * 1.25, (480.0f / 1423) * 1.75);
                bgFlash.Rotate(currtime, currtime + timestep, background.RotationAt(currtime), background.RotationAt(currtime + timestep));
                bgFlash.Move(currtime, currtime + timestep, background.PositionAt(currtime), background.PositionAt(currtime + timestep));
                bgFlash.Fade(OsbEasing.OutExpo, currtime, currtime + timestep, 1, 0);
                currtime += timestep;
            }

            bgFlash.Scale(OsbEasing.OutExpo, 322096, 322430, (480.0f / 1423) * 1.25, (480.0f / 1423) * 1.75);
            bgFlash.Rotate(322096, 322430, background.RotationAt(322096), background.RotationAt(322430));
            bgFlash.Move(322096, 322430, background.PositionAt(322096), background.PositionAt(322430));
            bgFlash.Fade(OsbEasing.OutExpo, 322096, 322430, 1, 0);

            currtime = 324763;
            for (int i = 0; i < 4; i++)
            {
                bgFlash.Scale(OsbEasing.OutExpo, currtime, currtime + timestep, (480.0f / 1423) * 1.25, (480.0f / 1423) * 1.75);
                bgFlash.Rotate(currtime, currtime + timestep, background.RotationAt(currtime), background.RotationAt(currtime + timestep));
                bgFlash.Move(currtime, currtime + timestep, background.PositionAt(currtime), background.PositionAt(currtime + timestep));
                bgFlash.Fade(OsbEasing.OutExpo, currtime, currtime + timestep, 1, 0);
                currtime += timestep;
            }


            FlashInvert(OsbEasing.OutExpo, background, 327430, 500, true);
            FlashInvert(OsbEasing.OutExpo, background, 329763, 330096, true);
            FlashInvert(OsbEasing.OutExpo, background, 330096, 500, true);
            FlashInvert(OsbEasing.OutExpo, background, 332763, 500, true);


            FlashInvert(OsbEasing.OutExpo, background, 334763, 335096, true);
            FlashInvert(OsbEasing.OutExpo, background, 335096, 335430, true);

            FlashInvert(OsbEasing.OutExpo, background, 335430, 500, true);
            FlashInvert(OsbEasing.OutExpo, background, 336096, 500, true);
            FlashInvert(OsbEasing.OutExpo, background, 336763, 1500, true);


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
            float endScale = referenceBackground.ScaleAt(startTime).X * 1.25f;
            int endTime = duration < startTime ? startTime + duration : duration;
            invert.Rotate(startTime, endTime, referenceBackground.RotationAt(startTime), referenceBackground.RotationAt(endTime));
            invert.Move(startTime, endTime, referenceBackground.PositionAt(startTime), referenceBackground.PositionAt(endTime));
            invert.Scale(easing, startTime, endTime, referenceBackground.ScaleAt(startTime).X, endScale);
            invert.Fade(easing, startTime, endTime, 1, 0);
            if (additive)
                invert.Additive(startTime, endTime);
        }
    }
}
