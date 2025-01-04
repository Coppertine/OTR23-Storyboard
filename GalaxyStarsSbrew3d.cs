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
using StorybrewCommon.Storyboarding3d;
using OpenTK.Graphics.OpenGL;
using System.Runtime.InteropServices;

namespace StorybrewScripts
{
    
    public class GalaxyStarsSbrew3d : StoryboardObjectGenerator
    {
        [Configurable]
        public int startTime = 85;
        [Configurable]
        public int endTime = 3632811;

        [Configurable]
        public int StarCount = 200;
        [Configurable]
        public Vector2 PlayfieldSize = new Vector2(200,6000);
        [Configurable]
        public float PlayfieldHeight = 100.0f;

        public override void Generate()
        {
		    
            for (int i = 0; i < StarCount; i++)
            {
                double spriteDuration = Random(3000,7000);


                Scene3d scene = new Scene3d();
                PerspectiveCamera camera = new PerspectiveCamera();
                camera.PositionX.Add(startTime, 0);
                camera.PositionY.Add(startTime, 0);
                camera.PositionZ.Add(startTime, 100);
                scene.Root.PositionX.Add(startTime, 0);
                scene.Root.PositionY.Add(startTime, 0);
                scene.Root.PositionZ.Add(startTime, 0);
                scene.Root.Opacity.Add(startTime, 0).Add(startTime + (spriteDuration / 4), 1);
                Sprite3d star = new Sprite3d{
                    SpritePath = "sb/dot.png",
                    UseDistanceFade = true,
                    
                };
                star.ConfigureGenerators(s => {
                    s.ScaleDecimals = 5;
                    s.ScaleTolerance = 0.05f;
                });

                star.PositionX.Add(startTime, Random(-PlayfieldSize.X, PlayfieldSize.X));
                star.PositionY.Add(startTime, Random(-PlayfieldHeight,PlayfieldHeight));
                star.PositionZ.Add(startTime, Random(0, PlayfieldSize.Y));
                star.ScaleX.Add(startTime, Random(0.5f, 0.7f));
                star.ScaleY.Add(startTime, Random(0.5f, 0.7f));
                star.ScaleZ.Add(startTime, Random(0.5f, 0.7f));

                // sprite.PositionX.Add(5000, Random(-width, width));
                // sprite.PositionY.Add(5000, Random(-height, height));
                // sprite.PositionZ.Add(5000, Random(-width, width));
                scene.Add(star);  
                scene.Root.PositionZ.Add(startTime + spriteDuration, PlayfieldSize.Y - 200);
                scene.Generate(camera, GetLayer("Galaxy").CreateSegment(), startTime, startTime + spriteDuration, Beatmap.GetTimingPointAt(startTime).BeatDuration / 8, (int)((endTime - startTime) / spriteDuration) + 1);             
            }
            
        }
    }
}
