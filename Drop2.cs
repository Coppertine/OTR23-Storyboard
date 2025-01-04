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
    public class Drop2 : StoryboardObjectGenerator
    {
        public override void Generate()
        {
            StoryboardLayer layer = GetLayer("Background");
            OsbSprite backgroundBlur = layer.CreateSprite("sb/ultrablur.jpg");
            backgroundBlur.Scale(183315, (480.0f / 720) * 1.25);
            backgroundBlur.Fade(183315, 0.5);
            backgroundBlur.Fade(193998, 0);
            OsbSprite background = layer.CreateSprite(Beatmap.BackgroundPath);
            background.Scale(183315, (480.0f / 1423) * 1.25);
            background.Fade(183315, 0.4);
            background.Fade(193998, 0);
            background.Additive(183315);


            float currRot = 0;
            Vector2 currPos = new Vector2(320, 240);
            double currTime = 183315;
            Beatmap.ForEachTick(183315, 193998, 4, (point, time, beat, tick) =>
            {
                if (tick % 4 == 0 || IsStrongHit(time))
                {
                    Vector2 pos = new Vector2(
                        320 + (IsStrongHit(time) ? Random(-20, 20) :Random(-10, 10)),
                        240 + (IsStrongHit(time) ? Random(-20, 20) :Random(-10, 10))
                    );
                    background.Move(OsbEasing.OutExpo, currTime, time, currPos, pos);
                    backgroundBlur.Move(OsbEasing.InCirc, currTime, time, currPos, pos);
                    float rot = (float)Random(MathHelper.DegreesToRadians(-5), MathHelper.DegreesToRadians(5)) * (IsStrongHit(time) ? 5f : 1f);
                    background.Rotate(OsbEasing.OutExpo, currTime, time, currRot, rot);
                    backgroundBlur.Rotate(OsbEasing.InCirc, currTime, time, currRot, rot);

                    currPos = pos;
                    currTime = time;
                    currRot = rot;
                }
            });

            var beatduration = Beatmap.GetTimingPointAt(183315).BeatDuration;
            var startTime = 183315;
            var endTime = 193998;
            using (var pool = new OsbSpritePool(GetLayer("Squares"), "sb/p.png", OsbOrigin.Centre, (sprite, start, end) =>
            {
                sprite.Scale(start, Random(20f, 80f));
                sprite.MoveX(start, Random(-107, 757));
                sprite.Fade(start, Random(0.2f, 0.9f));
                if (end > endTime - beatduration * 4) //Hide sprites if they cross the end time
                    sprite.Fade(endTime, 0f);
            }))
            {
                for (var sTime = (double)startTime; sTime <= endTime - beatduration * 4; sTime += beatduration / 2f)
                {
                    var baseSpeed = Random(40, 120);
                    if (Random(0, 4) > 0) //HACK move the time back in order to increase the particle count without running into syncing issues
                        sTime -= beatduration / 2f;
                    var eTime = sTime + Math.Ceiling(620f / baseSpeed) * beatduration;

                    var sprite = pool.Get(sTime, eTime);
 
                    var moveSpeed = baseSpeed * -1;
                    var currentTime = sTime + (sTime - Beatmap.GetTimingPointAt(183315).Offset) % beatduration;

                    sprite.MoveY(sTime, 540);
                    while (sprite.PositionAt(currentTime).Y > -60)
                    {

                        var yPos = sprite.PositionAt(currentTime).Y;
                        var yRot = sprite.RotationAt(currentTime);

                        sprite.MoveY(OsbEasing.OutExpo, currentTime, currentTime + beatduration, yPos, yPos + moveSpeed);
                        sprite.Rotate(OsbEasing.OutExpo, currentTime, currentTime + beatduration, yRot, yRot + Math.PI * 0.25f);

                        currentTime += beatduration;
                    }

                    
                }

            }

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

        private bool IsStrongHit(double time)
        {
            List<double> times = new List<double>{
                187315,
            };
            foreach(var listTime in times)
            {
                if(listTime - 2<= time && time <= listTime + 2)
                    return true;
            }

            return false;
        }
        
    }
}
