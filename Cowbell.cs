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
            background.ScaleVec(194046, 853, 480);
            background.Fade(194046, 195713, 1, 1);
            background.Color(194046, Color1);
            background.Color(194380, Color2);
            background.Color(194713, Color3);
            background.Color(195046, Color4);
            background.Color(195380, Color5);

            OsbSprite cowbell = GetLayer("").CreateSprite("sb/cowbell/bell.png", OsbOrigin.Centre, new Vector2(320, 240));
            cowbell.Scale(194046, 0.5);
            cowbell.Fade(194046, 195713, 1, 1);

            OsbSprite mallet = GetLayer("").CreateSprite("sb/cowbell/mallet.png", OsbOrigin.BottomCentre, new Vector2(570, 372));
            mallet.Scale(194046, 0.5);
            mallet.Fade(194046, 195713, 1, 1);
            double halfBeat = Beatmap.GetTimingPointAt(194046).BeatDuration / 2;
            mallet.StartLoopGroup(194046, 5);
            mallet.Rotate(OsbEasing.OutSine, 0, halfBeat, MathHelper.DegreesToRadians(-45), 0);
            mallet.Rotate(OsbEasing.InSine, halfBeat, halfBeat * 2, 0, MathHelper.DegreesToRadians(-45));
            mallet.EndGroup();


            /// Section after Cowbell... because fuck
            OsbSprite beatmapBG = GetLayer("").CreateSprite(Beatmap.BackgroundPath);
            beatmapBG.Scale(195713, 480.0f / 1423);
            beatmapBG.Fade(195713, 1);
            beatmapBG.Fade(199332 - 200, 199332, 1, 0.45);
            beatmapBG.Fade(199998, 1);
            beatmapBG.Fade(204080, 208113, 1, 0);

            Vector2 currPos = new Vector2(320, 240);
            float currRot = 0;
            double currTime = 195713;
            Beatmap.ForEachTick(195713, 203332, 1, (point, time, beat, tick) =>
            {
                if (beat % 8 == 0)
                {
                    Vector2 pos = new Vector2(
                        320 + Random(-10, 10),
                        240 + Random(-10, 10)
                    );
                    beatmapBG.Move(currTime, time, currPos, pos);
                    float rot = (float)Random(MathHelper.DegreesToRadians(-5), MathHelper.DegreesToRadians(5));
                    beatmapBG.Rotate(currTime, time, currRot, rot);
                    currPos = pos;
                    currTime = time;
                    currRot = rot;
                }
            });
            int offset = 50;
            beatmapBG.Scale(203332, (480.0f / 1423) * 2);
            beatmapBG.Move(OsbEasing.OutExpo, 203332, 203680, new Vector2(320 + Random(-offset, offset), 240 + Random(-offset, offset)),
                new Vector2(320 + Random(-offset, offset), 240 + Random(-offset, offset)));
            beatmapBG.Move(OsbEasing.OutExpo, 203680, 204080, new Vector2(320 + Random(-offset, offset), 240 + Random(-offset, offset)),
                new Vector2(320 + Random(-offset, offset), 240 + Random(-offset, offset)));
            beatmapBG.Move(204080, new Vector2(320,240));
            beatmapBG.Scale(OsbEasing.OutExpo, 204080, 208113, (480.0f / 1423) * 2, (480.0f / 1423));


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
    }
}
