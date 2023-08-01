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
    public class Particles3D : StoryboardObjectGenerator
    {
        [Configurable]
        public string SpritePath = "";
        [Configurable]
        public Vector2 SpriteScale = new Vector2(1,1);
        [Configurable]
        public float FOV = 90;
        [Configurable]
        public int Set = -1;
        [Configurable]
        public int CustomRefreshTime = 5;
        [Configurable]
        public int FadeInTime = 300;
        [Configurable]
        public int MaxRefresh = 400;
        [Configurable]
        public bool Additive = false;
        [Configurable]
        public float MaxZ = -55.5f;
        [Configurable] 
        public bool ExtendedPerspective = false;

        [Configurable]
        public bool UseOsbSpriteLoops = true;
        public override void Generate()
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            GenerateObjects();
            AddCameras();
            RenderObjects();
            stopwatch.Stop();
            Log($"Particles3D: {stopwatch.ElapsedMilliseconds}ms");
        }
        void RenderObjects()
        {
            for (var c = 0; c < Cameras.Count; c++)
            {
                if (Cameras[c].continous)
                {
                    foreach (Object obj in Objects.OrderBy(x => x.position.Z))
                    {
                        var bitmap = GetMapsetBitmap(obj.name);
                        var sprite = GetLayer("").CreateSprite(obj.name, OsbOrigin.Centre);

                        Vector3 prevPosition3D = Rotate(obj.position - Cameras[c - 1].position, Cameras[c - 1].rotation); Vector2 prevPosition = new Vector2(-999, -999); Vector2 prevScale = new Vector2(0, 0); double prevRotation = 0;
                        int prevTime = -1111; int FirstActive = -1111; int LastActive = -1111;
                        
                        if(UseOsbSpriteLoops)
                        {
                            for (var i = Cameras[c - 1].time; i <= Cameras[c].time; i += Math.Min(obj.commands.Exists(o => o.time > i) ? obj.commands.OrderBy(o => o.time).First(o => o.time > i).time - i : 999999, Math.Min(Math.Max(Math.Min(Math.Abs((int)(prevPosition3D.Z * CustomRefreshTime)), MaxRefresh), 50), Cameras[c].time - i > 0 ? Cameras[c].time - i : 1)))
                            {
                                var t = (i - Cameras[c - 1].time) / (float)(Cameras[c].time - Cameras[c - 1].time);
                                var LerpCamRot = Vector3.Lerp(Cameras[c - 1].rotation, Cameras[c].rotation, t);
                                var LerpCamPos = Vector3.Lerp(Cameras[c - 1].position, Cameras[c].position, t);
                                var Command1 = obj.commands.LastOrDefault(o => o.time <= i);
                                var Command2 = obj.commands.FirstOrDefault(o => o.time > i);
                                var LerpObjPos = obj.commands.Count() > 0 ? Command2.continous ? Vector3.Lerp(Command1.position, Command2.position, (i - Command1.time)/(float)(Command2.time - Command1.time)) : Command1.position : obj.position;
                                var RelPos = Rotate(LerpObjPos - LerpCamPos, LerpCamRot);
                                var RelScale = GetScale(RelPos);
                                if (RelPos.Z < 0)
                                {
                                    Vector2 Position = new Vector2(RelPos * RelScale) + new Vector2(320, 240);
                                    double Rotation = obj.rotation.Z - LerpCamRot.Z;
                                    var PosNoRot = obj.position - LerpCamPos;
                                    float ScaleX = !ExtendedPerspective ? (float)Math.Cos(obj.rotation.Y - LerpCamRot.Y) : (float)Math.Cos(Look(new Vector2(0, 0), new Vector2(PosNoRot.X, Math.Abs(PosNoRot.Z))) - obj.rotation.Y);
                                    float ScaleY = !ExtendedPerspective ? (float)Math.Cos(obj.rotation.X - LerpCamRot.X) : (float)Math.Cos(Look(new Vector2(0, 0), new Vector2(PosNoRot.Y, Math.Abs(PosNoRot.Z))) - obj.rotation.X);
                                    if(ExtendedPerspective)
                                    {
                                        var sx = ScaleX;
                                        ScaleX = (float)Lerp(ScaleX, ScaleY, Math.Abs(Math.Cos(obj.rotation.Z)));
                                        ScaleY = (float)Lerp(ScaleY, sx, Math.Abs(Math.Cos(obj.rotation.Z)));
                                    }
                                    Vector2 Scale = new Vector2(obj.scale.X * ScaleX, obj.scale.Y * ScaleY) * RelScale;
                                    if (!obj.commands.Exists(o => o.time == i && !o.continous) && prevTime != -1111 && (CheckVisibility(Position, Scale, Rotation) || CheckVisibility(prevPosition, prevScale, prevRotation)) && (prevPosition3D.Z > MaxZ && RelPos.Z > (MaxZ + 2f)))
                                    {
                                        if(FirstActive == -1111){
                                            if(Cameras[c].loopCount > 0) { sprite.StartLoopGroup(0, Cameras[c].loopCount); }
                                            if(obj.randomFlip)sprite.FlipH(Cameras[c - 1].time);
                                            if(obj.color != Color4.White) sprite.Color(prevTime,  obj.color);
                                            FirstActive = !obj.commands.Exists(o => o.time == i && !o.continous) ? prevTime : i; 
                                        }
                                        sprite.Move(prevTime, i, prevPosition, Position);
                                        sprite.Rotate(prevTime, i, prevRotation, Rotation);
                                        sprite.ScaleVec(prevTime, i, prevScale, Scale);
                                        LastActive = i;
                                    }
                                    else if(LastActive == prevTime && CheckVisibility(prevPosition, prevScale, prevRotation))
                                    {
                                        sprite.Move(prevTime, i, new Vector2(-10000,-10000), new Vector2(-10000,-10000));
                                    }
                                    prevTime = i; prevPosition = Position; prevRotation = Rotation; prevScale = Scale;
                                }
                                prevPosition3D = RelPos;
                            }
                            if(FirstActive != -1111 && LastActive != -1111 && FirstActive != LastActive)
                            {
                                if(Cameras[c - 1].time != FirstActive)
                                {   
                                    sprite.Fade(Cameras[c - 1].time, FirstActive, 0, 0);
                                    sprite.Fade(FirstActive, Math.Min(LastActive,  FirstActive + FadeInTime), 0, 1);
                                }
                                else
                                    sprite.Fade(FirstActive, LastActive, 1, 1);
                                if(Cameras[c].time != LastActive)
                                    sprite.Fade(LastActive, Cameras[c].time, 0, 0);
                                if(Additive)
                                    sprite.Additive(FirstActive);
                            }
                        } else {
                            for(int loopNum = 0; loopNum <= Cameras[c].loopCount; loopNum++) {
                                for (var i = Cameras[c - 1].time; i <= Cameras[c].time; i += Math.Min(obj.commands.Exists(o => o.time > i) ? obj.commands.OrderBy(o => o.time).First(o => o.time > i).time - i : 999999, Math.Min(Math.Max(Math.Min(Math.Abs((int)(prevPosition3D.Z * CustomRefreshTime)), MaxRefresh), 50), Cameras[c].time - i > 0 ? Cameras[c].time - i : 1)))
                                {
                                    var t = (i - Cameras[c - 1].time) / (float)(Cameras[c].time - Cameras[c - 1].time);
                                    var LerpCamRot = Vector3.Lerp(Cameras[c - 1].rotation, Cameras[c].rotation, t);
                                    var LerpCamPos = Vector3.Lerp(Cameras[c - 1].position, Cameras[c].position, t);
                                    var Command1 = obj.commands.LastOrDefault(o => o.time <= i);
                                    var Command2 = obj.commands.FirstOrDefault(o => o.time > i);
                                    var LerpObjPos = obj.commands.Count() > 0 ? Command2.continous ? Vector3.Lerp(Command1.position, Command2.position, (i - Command1.time)/(float)(Command2.time - Command1.time)) : Command1.position : obj.position;
                                    var RelPos = Rotate(LerpObjPos - LerpCamPos, LerpCamRot);
                                    var RelScale = GetScale(RelPos);
                                    if (RelPos.Z < 0)
                                    {
                                        Vector2 Position = new Vector2(RelPos * RelScale) + new Vector2(320, 240);
                                        double Rotation = obj.rotation.Z - LerpCamRot.Z;
                                        var PosNoRot = obj.position - LerpCamPos;
                                        float ScaleX = !ExtendedPerspective ? (float)Math.Cos(obj.rotation.Y - LerpCamRot.Y) : (float)Math.Cos(Look(new Vector2(0, 0), new Vector2(PosNoRot.X, Math.Abs(PosNoRot.Z))) - obj.rotation.Y);
                                        float ScaleY = !ExtendedPerspective ? (float)Math.Cos(obj.rotation.X - LerpCamRot.X) : (float)Math.Cos(Look(new Vector2(0, 0), new Vector2(PosNoRot.Y, Math.Abs(PosNoRot.Z))) - obj.rotation.X);
                                        if(ExtendedPerspective)
                                        {
                                            var sx = ScaleX;
                                            ScaleX = (float)Lerp(ScaleX, ScaleY, Math.Abs(Math.Cos(obj.rotation.Z)));
                                            ScaleY = (float)Lerp(ScaleY, sx, Math.Abs(Math.Cos(obj.rotation.Z)));
                                        }
                                        Vector2 Scale = new Vector2(obj.scale.X * ScaleX, obj.scale.Y * ScaleY) * RelScale;
                                        if (!obj.commands.Exists(o => o.time == i && !o.continous) && prevTime != -1111 && (CheckVisibility(Position, Scale, Rotation) || CheckVisibility(prevPosition, prevScale, prevRotation)) && (prevPosition3D.Z > MaxZ && RelPos.Z > (MaxZ + 2f)))
                                        {
                                            if(FirstActive == -1111){
                                                //if(Cameras[c].loopCount > 0) { sprite.StartLoopGroup(0, Cameras[c].loopCount); }
                                                if(obj.randomFlip)sprite.FlipH(Cameras[c - 1].time);
                                                if(obj.color != Color4.White) sprite.Color(prevTime,  obj.color);
                                                FirstActive = !obj.commands.Exists(o => o.time == i && !o.continous) ? prevTime : i; 
                                            }
                                            sprite.Move(prevTime + loopNum * Cameras[c].time, i + loopNum * Cameras[c].time, prevPosition, Position);
                                            // sprite.Rotate(prevTime + loopNum * Cameras[c].time, i + loopNum * Cameras[c].time, prevRotation, Rotation);
                                            sprite.ScaleVec(prevTime + loopNum * Cameras[c].time, i + loopNum * Cameras[c].time, prevScale, Scale);
                                            LastActive = i;
                                        }
                                        else if(LastActive == prevTime && CheckVisibility(prevPosition, prevScale, prevRotation))
                                        {
                                            sprite.Move(prevTime + loopNum * Cameras[c].time, i + loopNum * Cameras[c].time, new Vector2(-10000,-10000), new Vector2(-10000,-10000));
                                        }
                                        prevTime = i; prevPosition = Position; prevRotation = Rotation; prevScale = Scale;
                                    }
                                    prevPosition3D = RelPos;                                
                                }
                                if(FirstActive != -1111 && LastActive != -1111 && FirstActive != LastActive)
                                {
                                    if(Cameras[c - 1].time != FirstActive)
                                    {   
                                        sprite.Fade(Cameras[c - 1].time, FirstActive, 0, 0);
                                        sprite.Fade(FirstActive, Math.Min(LastActive,  FirstActive + FadeInTime), 0, 1);
                                    }
                                    else
                                        sprite.Fade(FirstActive, LastActive, 1, 1);
                                    if(Cameras[c].time != LastActive)
                                        sprite.Fade(LastActive, Cameras[c].time, 0, 0);
                                    if(Additive)
                                        sprite.Additive(FirstActive);
                                }
                            }
                        }
                    }
                }
            }
        }

        public double Look(Vector2 Vector, Vector2 Vector2)
        {
            var SubVector = Vector2 - Vector;
            var DefaultFacingDirection = new Vector2(0, 1);
            return Math.Atan2(SubVector.Y, SubVector.X) - Math.Atan2(DefaultFacingDirection.Y, DefaultFacingDirection.X);
        }
        public static double Distance(Vector2 Vector)
        {
            return Math.Sqrt(Math.Pow(Vector.X, 2) + Math.Pow(Vector.Y, 2));
        }
        bool CheckVisibility(Vector2 Pos, Vector2 Scale, double Rotation)
        {
            Scale = new Vector2((float)Math.Abs(Lerp(Scale.X, Scale.Y, Math.Sin(Rotation))), (float)Math.Abs(Lerp(Scale.Y, Scale.X, Math.Sin(Rotation))));
            var bitmap = GetMapsetBitmap(SpritePath);
            if (Pos.X + ((Scale.X * bitmap.Width) / 2f) >= -107f && Pos.X - ((Scale.X * bitmap.Width) / 2f) <= 747f && Pos.Y + ((Scale.Y * bitmap.Height) / 2f) >= 0f && Pos.Y - ((Scale.Y * bitmap.Height) / 2f) <= 480f) return true;
            else return false;
        }
        List<Object> Objects = new List<Object>();
        void GenerateObjects()
        {
            switch (Set)
            {
                case 1:
                    for (int z = 25; z > 0; z--)
                    {
                        for (int x = -60; x < 60; x++)
                        {
                            string[] Sprites = { SpritePath };
                            var Name = Sprites[Random(0, Sprites.Count())];
                            var Pos = Rotate(new Vector3(Random(-60f, 0), 0, Random(-60f, -10f)), new Vector3((float)Random(-Math.PI, Math.PI), 0, (float)Random(-Math.PI, Math.PI)));

                            var Rot = new Vector3(0, 0, 0);
                            var Flip = false;
                            var Color = Color4.White;
                            var Scale = new Vector3(SpriteScale.X, SpriteScale.Y, 1);
                            Objects.Add(new Object(Name, Pos, Rot, Scale, Flip, Color, new List<State>()));
                            var Pos2 = Pos - new Vector3(0, 0, 60f);
                            Objects.Add(new Object(Name, Pos2, Rot, Scale, Flip, Color, new List<State>()));
                            var Pos3 = Pos - new Vector3(0, 0, 120);
                            Objects.Add(new Object(Name, Pos3, Rot, Scale, Flip, Color, new List<State>()));

                        }
                    }
                    break;
            }
        }
        struct Object
        {
            public string name;
            public Vector3 position, rotation, scale;
            public bool randomFlip;
            public Color4 color;
            public List<State> commands;
            public Object(string Name, Vector3 Position, Vector3 Rotation, Vector3 Scale, bool RandomFlip, Color4 Color, List<State> Commands)
            {
                name = Name;
                position = Position;
                rotation = Rotation;
                scale = Scale;
                randomFlip = RandomFlip;
                color = Color;
                commands = Commands;
            }
            public Object(string Name, State State, bool RandomFlip, Color4 Color, List<State> Commands)
            {
                name = Name;
                position = State.position;
                rotation = State.rotation;
                scale = State.scale;
                randomFlip = RandomFlip;
                color = Color;
                commands = Commands;
            }
        }

        List<State> Cameras = new List<State>();
        void AddCameras()
        {
            switch(Set)
            {
                case 1:
                    Cameras.Add(new State(218812, new Vector3(0, -0.6f, 0.0f), new Vector3(0, 0, 0), new Vector3(1), false, 0));
                    Cameras.Add(new State(218812+(int)(Beatmap.GetTimingPointAt(218812).BeatDuration * 16), new Vector3(0, -0.6f, -60f), new Vector3(0, 0, 0), new Vector3(1), true, ((294430 - (218812 + (int)(Beatmap.GetTimingPointAt(218812).BeatDuration * 16)))/ (int)(Beatmap.GetTimingPointAt(218812).BeatDuration * 16))+1));
                    // Cameras.Add(new State(85+7000, new Vector3(0, -0.6f, -60f), new Vector3(0, 0, 0), new Vector3(1), true, ((741903 - (85+7000))/ 7000)+1));

                    // Cameras.Add(new State(741903, new Vector3(0, -0.6f, 0.0f), new Vector3(0, 0, 0), new Vector3(1), false, 0));
                    // Cameras.Add(new State(741903 + 2000, new Vector3(0, -0.6f, -60f), new Vector3(0, 0, 0), new Vector3(1), true, ((763721 - (741903 + 1000))/ 2000)));

                    // Cameras.Add(new State(763721, new Vector3(0, -0.6f, 0.0f), new Vector3(0, 0, 0), new Vector3(1), false, 0));
                    // Cameras.Add(new State(763721 + 7000, new Vector3(0, -0.6f, -60f), new Vector3(0, 0, 0), new Vector3(1), true, ((3633493 - (763721 + 7000)) / 7000)));
                    break;
            }
        }

        struct State
        {
            public int time;
            public Vector3 position, rotation, scale;
            public bool continous;
            public int loopCount;
            public State(int Time, Vector3 Position, Vector3 Rotation, Vector3 Scale, bool Continous, int LoopCount)
            {
                time = Time; position = Position; rotation = Rotation; scale = Scale; continous = Continous; loopCount = LoopCount;
            }
        }

        public float GetScale(Vector3 Vector)
        {
            return 480f / (float)(2f * Math.Tan(MathHelper.DegreesToRadians(FOV / 2f)) * Math.Abs(Vector.Z));
        }
        public Vector3 Rotate(Vector3 Vector, Vector3 Rotation)
        {
            var Rot = new Quaternion(Rotation.Z, Rotation.Y, Rotation.X);
            return Vector3.Transform(Vector, Rot);
        }
        public Vector2 InvertY(Vector2 Vector)
        {
            Vector.Y = -Vector.Y;
            return Vector;
        }
        public double Distance(Vector3 v1, Vector3 v2)
        {
            var difference = new Vector3(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
            return Math.Sqrt(Math.Pow(difference.X, 2f) + Math.Pow(difference.Y, 2f) + Math.Pow(difference.Z, 2f));
        }
        public double Lerp(double a, double b, double t)
        {
            return a * (1 - t) + b * t;
        }
        public double easeInQuart(double x) {
            return 1 - (1 - x) * (1 - x);
        }
    }
}
