using OpenTK;
using OpenTK.Graphics;
using StorybrewCommon.Animations;
using StorybrewCommon.Mapset;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Storyboarding.Util;
using StorybrewCommon.Storyboarding3d;
using StorybrewCommon.Subtitles;
using StorybrewCommon.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StorybrewScripts
{
    public class CircleInvasion : StoryboardObjectGenerator
    {
        public override void Generate()
        {

            // ** idk what build up this is... **//
            /// 177998
            GenerateCircleInvadedBySquare();
        }

        private void GenerateCircleInvadedBySquare()
        {
            int startTime = 177998; // fade in early
            int recordScratch = 179998;
            int squareTakeover = 180665;
            int endTime = 183332;
            int starCount = 240;
            int width = 200;
            int height = 200;

            Scene3d scene = new Scene3d();
            // I don't want to fuck around with cameras... sooo camera stays there during ~~sex~~ scene
            PerspectiveCamera camera = new PerspectiveCamera();
            camera.FarClip.Add(startTime, 350);
            camera.FarFade.Add(startTime, 200);
            camera.PositionX.Add(startTime, 0);
            camera.PositionY.Add(startTime, 0);
            camera.PositionZ.Add(startTime, -100);
    
            // Need to create 3 Node3ds, 1 for the Circle tunnel, 1 for the record scratch where a small amount of squares appear
            // and the last one for the major square tunnel where the last one is a fully filled in one.
            //I could add in stars that expand in the Z direction for the hyperspeed effect. 

            Node3d circleTunnel = new Node3d();
            Node3d smallSquareTunnel = new Node3d();
            Node3d squareTunnel = new Node3d();
            Node3d stars = new Node3d();
            for (int i = 0; i < starCount; i++)
            {
                Sprite3d star = new Sprite3d
                {
                    SpritePath = "sb/dot.png",
                    UseDistanceFade = false
                };

                star.PositionX.Add(startTime, Random(-width, width));
                star.PositionY.Add(startTime, Random(-height, height));
                star.PositionZ.Add(startTime, Random(-width, width));
                var scale = Random(0.5f, 0.7f);
                star.ScaleX.Add(startTime, scale);
                star.ScaleY.Add(startTime, scale);
                star.ScaleZ.Add(startTime, scale);

                star.Opacity.Add(startTime, 0);
                star.Opacity.Add(178332, 1);

                // stars.Add(star);
            }

            int ringCount = 12;
            int ringOffset = 20;
            int startZ = 0;
            int endZ = -20;
            int endEndZ = 400;
            for (int i = 0; i < ringCount; i++)
            {
                Sprite3d ring = new Sprite3d
                {
                    SpritePath = "sb/ring.png",
                    UseDistanceFade = true
                };
                ring.ConfigureGenerators((r) =>
                {
                    r.ScaleDecimals = 4;
                });

                ring.PositionZ.Add(startTime, (i * ringOffset) + startZ);
                ring.PositionZ.Add(recordScratch + 500, (i * ringOffset) + endZ);
                ring.PositionZ.Add(squareTakeover, (i * ringOffset) + endEndZ);

                circleTunnel.Add(ring);
            }

            int smallSquareCount = 10;
            int smallSquareOffset = 40;
            startZ = -500;
            endZ = 400;
            for (int i = 0; i < smallSquareCount; i++)
            {
                Sprite3d square = new Sprite3d
                {
                    SpritePath = "sb/box2.png",
                    UseDistanceFade = true
                };
                square.ConfigureGenerators((s) => {
                    s.ScaleDecimals = 4;
                });

                square.PositionZ.Add(recordScratch, (i * smallSquareOffset) + startZ);
                square.PositionZ.Add(recordScratch + 1000, (i * smallSquareOffset) + endZ);

                smallSquareTunnel.Add(square);
            }

            int squareCount = 400;
            int squareOffset = 20;
            startZ = 400;
            endZ = -800;
            for (int i = 0; i < smallSquareCount; i++)
            {
                Sprite3d square = new Sprite3d
                {
                    SpritePath = "sb/box2.png",
                    UseDistanceFade = true
                };
                square.ConfigureGenerators((s) => {
                    s.ScaleDecimals = 4;
                });

                square.PositionZ.Add(squareTakeover, (i * squareCount) + startZ);
                square.PositionZ.Add(endTime, (i * squareOffset) + endZ);
                square.SpriteScale.Add(squareTakeover, new Vector2(2));
                squareTunnel.Add(square);
            }

            Sprite3d finalSquare = new Sprite3d{
                SpritePath = "sb/p.png",
                UseDistanceFade = true
            };
            
            finalSquare.SpriteScale.Add(181998, 100);
            finalSquare.SpriteRotation.Add(181998, Math.PI / 4);
            finalSquare.PositionZ.Add(181998, startZ);
            finalSquare.PositionZ.Add(endTime, -100);
            squareTunnel.Add(finalSquare);

            scene.Add(stars);
            scene.Add(circleTunnel);
            scene.Add(smallSquareTunnel);
            scene.Add(squareTunnel);

            scene.Generate(camera, GetLayer("Square Invasion"), startTime, endTime, 4);
        }
    }
}
