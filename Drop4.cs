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

             OsbSprite bg = GetLayer("Background").CreateSprite(Beatmap.BackgroundPath);
            bg.Scale(348763, (480.0f / 1423) * 1.25);
            bg.Fade(348763, 1);
            bg.Fade(393130, 0);
            
            GenerateSpirals(348763, 370096, 8, 500, (int)Beatmap.GetTimingPointAt(348763).BeatDuration * 2, (int)Beatmap.GetTimingPointAt(348763).BeatDuration / 8, 1, Color4.Magenta);
            GenerateSpirals(348763, 370096, 8, 500, (int)Beatmap.GetTimingPointAt(348763).BeatDuration * 4, (int)Beatmap.GetTimingPointAt(348763).BeatDuration / 8,-1, Color4.MediumOrchid);


            OsbSprite girlOverlay = GetLayer("Girl Overlay").CreateSprite("sb/girl.png");
            girlOverlay.Scale(348763, (480.0f / 1423) * 1.25);
            girlOverlay.Fade(348763, 1);
            girlOverlay.Fade(393130, 0);

            float currRot = 0;
            Vector2 currPos = new Vector2(320, 240);
            double currTime = 348763;
            Beatmap.ForEachTick(348763, 393130, 1, (point, time, beat, tick) =>
            {
                if (beat % 8 == 0)
                {
                    Vector2 pos = new Vector2(
                        320 + Random(-10, 10),
                        240 + Random(-10, 10)
                    );
                    bg.Move(currTime, time, currPos, pos);
                    girlOverlay.Move(currTime, time, currPos, pos);
                    float rot = (float)Random(MathHelper.DegreesToRadians(-5), MathHelper.DegreesToRadians(5));
                    bg.Rotate(currTime, time, currRot, rot);
                    girlOverlay.Rotate(currTime, time, currRot, rot);
                    currPos = pos;
                    currTime = time;
                    currRot = rot;
                }
            });
		    
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
                        var sprite = spritePool.Get(time, time + duration);
                        sprite.Move(OsbEasing.None, time, time + duration, new Vector2(320,240), anglePos);
                    }
                    CurrentangleExtra += MathHelper.DegreesToRadians(angleMove);
                }
            }
        }
    }
}
