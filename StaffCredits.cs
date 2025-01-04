using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using StorybrewCommon.Mapset;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Storyboarding.Util;
using StorybrewCommon.Subtitles;
using StorybrewCommon.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace StorybrewScripts
{
    public class StaffCredits : StoryboardObjectGenerator
    {
        public override void Generate()
        {

            var mainFont = LoadFont("sb/f/s", new FontDescription
            {
                FontPath = "fonts/PretendardJP-Thin.otf",
                Color = Color4.White
            },
            new FontShadow()
            {
                Thickness = 4,
                Color = Color4.Black
            });

            StoryboardLayer poemLayer = GetLayer("Lyric");
            /*
                人間界がどんなものなのかを知るためには十分の時間を過ごした。
                だから元の世界に戻ってきた。
                でもあの人たちは。。。
                あの人たちはいつやってくるの?
                この場所であの人たちと再会したい
                美しい天使として。 
                私たちの世界が控訴するときは
                いつ来るのだろう?


                ningenkai ga donna monona no ka o shiru tame ni wa jūbun no jikan o sugoshita.
                dakara moto no sekai ni modottekita.
                demo ano hitotachi wa...
                ano hitotachi wa itsu yattekuru no?
                kono basho de ano hitotachi to saikai shitai
                utsukushī tenshi toshite.
                watashitachi no sekai ga kōso suru toki wa
                itsu kuru no darō?

            */

            StoryboardLayer layer = GetLayer("Poem");
            Lyric l1 = layer.CreateLyric(this, mainFont, "人間界がどんなものなのかを知る", OsbOrigin.Centre, 0.2, new Vector2(680, 200), true);
            Lyric l2 = layer.CreateLyric(this, mainFont, "ためには十分の時間を過ごした。", OsbOrigin.Centre, 0.2, new Vector2(665, 200), true);
            Lyric l3 = layer.CreateLyric(this, mainFont, "でもあの人たちは。。。", OsbOrigin.Centre, 0.2, new Vector2(650, 172), true);
            Lyric l4 = layer.CreateLyric(this, mainFont, "あの人たちはいつやってくるの?", OsbOrigin.Centre, 0.2, new Vector2(635, 200), true);
            Lyric l5 = layer.CreateLyric(this, mainFont, "この場所であの人たちと再会したい", OsbOrigin.Centre, 0.2, new Vector2(620, 207.5f), true);
            Lyric l6 = layer.CreateLyric(this, mainFont, "美しい天使として。", OsbOrigin.Centre, 0.2, new Vector2(605, 157), true);
            Lyric l7 = layer.CreateLyric(this, mainFont, "私たちの世界が控訴するときは", OsbOrigin.Centre, 0.2, new Vector2(590, 193.5F), true);


            double letterFadeSpacing = 100;
            double letterFadeDuration = 1000;
            l1.Scale(244388, 0.12);
            l2.Scale(247524, 0.12);
            l3.Scale(250066, 0.12);
            l4.Scale(253349, 0.12);
            l5.Scale(258878, 0.12);
            l6.Scale(262587, 0.12);
            l7.Scale(265318, 0.12);
            l1.Letters.ForEach((letter) =>
            {
                var s = 244388 + (l1.Letters.IndexOf(letter) * letterFadeSpacing);
                letter.Fade(s, s + letterFadeDuration, 0, 1);
            });
            l2.Letters.ForEach((letter) =>
            {
                var s = 247524 + (l2.Letters.IndexOf(letter) * letterFadeSpacing);
                letter.Fade(s, s + letterFadeDuration, 0, 1);
            });
            l3.Letters.ForEach((letter) =>
            {
                var s = 250066 + (l3.Letters.IndexOf(letter) * letterFadeSpacing);
                letter.Fade(s, s + letterFadeDuration, 0, 1);
            });
            l4.Letters.ForEach((letter) =>
            {
                var s = 253349 + (l4.Letters.IndexOf(letter) * letterFadeSpacing);
                letter.Fade(s, s + letterFadeDuration, 0, 1);
            });
            l5.Letters.ForEach((letter) =>
            {
                var s = 258878 + (l5.Letters.IndexOf(letter) * letterFadeSpacing);
                letter.Fade(s, s + letterFadeDuration, 0, 1);
            });
            l6.Letters.ForEach((letter) =>
            {
                var s = 262587 + (l6.Letters.IndexOf(letter) * letterFadeSpacing);
                letter.Fade(s, s + letterFadeDuration, 0, 1);
            });
            l7.Letters.ForEach((letter) =>
            {
                var s = 265318 + (l7.Letters.IndexOf(letter) * letterFadeSpacing);
                letter.Fade(s, s + letterFadeDuration, 0, 1);
            });

            // l1.Letters.ForEach((letter) => {
            //     var s = 273882 + (l1.Letters.IndexOf(letter) * letterFadeSpacing);
            //     letter.Fade(s, s + letterFadeDuration, 1,0);
            // });
            // l2.Letters.ForEach((letter) => {
            //     var s = 273882 + (l2.Letters.IndexOf(letter) * letterFadeSpacing);
            //     letter.Fade(s, s + letterFadeDuration,  1,0);
            // });
            // l3.Letters.ForEach((letter) => {
            //     var s = 273882 + (l3.Letters.IndexOf(letter) * letterFadeSpacing);
            //     letter.Fade(s, s + letterFadeDuration,  1,0);
            // });
            // l4.Letters.ForEach((letter) => {
            //     var s = 273882 + (l4.Letters.IndexOf(letter) * letterFadeSpacing);
            //     letter.Fade(s, s + letterFadeDuration,  1,0);
            // });
            // l5.Letters.ForEach((letter) => {
            //     var s = 273882 + (l5.Letters.IndexOf(letter) * letterFadeSpacing);
            //     letter.Fade(s, s + letterFadeDuration,  1,0);
            // });
            // l6.Letters.ForEach((letter) => {
            //     var s = 273882 + (l6.Letters.IndexOf(letter) * letterFadeSpacing);
            //     letter.Fade(s, s + letterFadeDuration,  1,0);
            // });
            // l7.Letters.ForEach((letter) => {
            //     var s = 273882 + (l7.Letters.IndexOf(letter) * letterFadeSpacing);
            //     letter.Fade(s, s + letterFadeDuration,  1,0);
            // });


            // Get each letter and move from position into a spiral that goes to the center
            DoSpiralLyricChars(13, l1, 271111, 272611, letterFadeSpacing, letterFadeDuration);
            DoSpiralLyricChars(12.5, l2, 270945, 272611, letterFadeSpacing, letterFadeDuration);
            DoSpiralLyricChars(12, l3, 270778, 272611, letterFadeSpacing, letterFadeDuration);
            DoSpiralLyricChars(11.5, l4, 270611, 272611, letterFadeSpacing, letterFadeDuration);
            DoSpiralLyricChars(11, l5, 270445, 272611, letterFadeSpacing, letterFadeDuration);
            DoSpiralLyricChars(10.5, l6, 270278, 272611, letterFadeSpacing, letterFadeDuration);
            DoSpiralLyricChars(10, l7, 270111, 272611, letterFadeSpacing, letterFadeDuration);

            // Small batch of particles from middle
            Particles(OsbEasing.OutExpo, 274079, 1000, new Vector2(320,240));
            var title = GetLayer("").CreateLyric(this, mainFont, "天使の帰郷", OsbOrigin.Centre, 0.2, new Vector2(320, 240));
            var artist = GetLayer("").CreateLyric(this, mainFont, "Asatsumei feat. L4hee", OsbOrigin.Centre, 0.2, new Vector2(320, 240));
            var lineSeperator = GetLayer("").CreateSprite("sb/p.png");

            lineSeperator.ScaleVec(OsbEasing.OutExpo, 274079, 274413, new Vector2(1, 1), new Vector2(200, 1));
            title.MoveY(OsbEasing.OutExpo, 274079, 274413, 240, 225);
            artist.MoveY(OsbEasing.OutExpo, 274079, 274413, 240, 255);
            title.Fade(OsbEasing.OutExpo, 274079, 274413, 0, 1);
            artist.Fade(OsbEasing.OutExpo, 274079, 274413, 0, 1); 

            title.MoveY(OsbEasing.OutExpo, 279096, 279346, 225, 240);
            artist.MoveY(OsbEasing.OutExpo, 279096, 279346, 255, 240);
            lineSeperator.ScaleVec(OsbEasing.OutExpo, 279096, 279346, new Vector2(200, 1), new Vector2(1, 1));


            title.Fade(OsbEasing.OutExpo, 279096, 279346, 1, 0);
            artist.Fade(OsbEasing.OutExpo, 279096, 279346, 1, 0);

            // 274096
            var HostGroup = new StaffGroup("HOST");
            var GFXGroup = new StaffGroup("GFX/DEVELOPER");
            var MappersGroup = new StaffGroup("MAPPERS");
            var MappoolTeam = new StaffGroup("POOLERS");
            var PlaytestTeam = new StaffGroup("PLAYTESTING");
            var RefereeTeam = new StaffGroup("REFEREES");
            var StreamerTeam = new StaffGroup("STREAMERS");
            var CasterTeam = new StaffGroup("CASTERS");
            var Storyboard = new StaffGroup("STORYBOARD");
            var Hitsound = new StaffGroup("HITSOUND");

            string line;
            using (StreamReader reader = new StreamReader(OpenProjectFile("stafflist.txt"), Encoding.UTF8))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    string[] staff = line.Split(',');
                    switch (staff[0])
                    {
                        case "HOST":
                            HostGroup.AddStaff(staff[1]);
                            break;
                        case "GFX/DEVELOPER":
                            GFXGroup.AddStaff(staff[1]);
                            break;
                        case "MAPPER":
                            MappersGroup.AddStaff(staff[1]);
                            break;
                        case "POOLING":
                            MappoolTeam.AddStaff(staff[1]);
                            break;
                        case "PLAYTESTING":
                            PlaytestTeam.AddStaff(staff[1]);
                            break;
                        case "REFEREE":
                            RefereeTeam.AddStaff(staff[1]);
                            break;
                        case "STREAMER":
                            StreamerTeam.AddStaff(staff[1]);
                            break;
                        case "CASTER":
                            CasterTeam.AddStaff(staff[1]);
                            break;
                        case "HITSOUNDER":
                            Hitsound.AddStaff(staff[1]);
                            break;
                        case "STORYBOARD":
                            Storyboard.AddStaff(staff[1]);
                            break;
                    }
                }
                reader.Dispose();
            }
            float spacing = 20;
            HostGroup.displayStaff(GetLayer("Staff"), mainFont, 279430, 284763, new Vector2(320, 70));
            // AdminGroup.displayStaff(GetLayer("Staff"), mainFont, 154880, 284763, new Vector2(50, 90));
            GFXGroup.displayStaff(GetLayer("Staff"), mainFont, 279430, 284763, new Vector2((320 / 2) - spacing, 90));
            MappersGroup.displayStaff(GetLayer("Staff"), mainFont, 279430, 284763, new Vector2((320 / 2) - spacing, 180));
            MappoolTeam.displayStaff(GetLayer("Staff"), mainFont, 279430, 284763, new Vector2((320 / 2) - spacing, 300));
            PlaytestTeam.displayStaff(GetLayer("Staff"), mainFont, 279430, 282096, new Vector2(320 + (320 / 2) + spacing, 250));
            RefereeTeam.displayStaff(GetLayer("Staff"), mainFont, 279430, 282096, new Vector2(320 + (320 / 2) + spacing, 90));
            StreamerTeam.displayStaff(GetLayer("Staff"), mainFont, 282096, 284763, new Vector2(320 + (320 / 2) + spacing, 90));
            CasterTeam.displayStaff(GetLayer("Staff"), mainFont, 282096, 284763, new Vector2(320 + (320 / 2) + spacing, 170));
            Hitsound.displayStaff(GetLayer("Staff"), mainFont, 282096, 284763, new Vector2(320 + (320 / 2) - 50, 300));
            Storyboard.displayStaff(GetLayer("Staff"), mainFont, 282096, 284763, new Vector2(320 + (320 / 2) + 100, 300));
        }

        private void DoSpiralLyricChars(double size, Lyric line, int start, int end, double letterFadeSpacing, double letterFadeDuration)
        {
            line.Letters.ForEach((letter) =>
            {
                var s = start + (line.Letters.IndexOf(letter) * letterFadeSpacing);
                int pointCount = 55;
                var points = GetSpiralPoints(size, pointCount, 6.9 * Math.PI, Math.Atan2(240 - letter.PositionAt(s).Y, 320 - letter.PositionAt(s).X));
                var time = s;
                var e = end + (line.Letters.IndexOf(letter) * letterFadeSpacing);

                var pointDur = (e - s) / pointCount;
                foreach (var point in points)
                {
                    letter.MoveX(time, time + pointDur, letter.PositionAt(time).X, 320 + point.x);
                    letter.MoveY(time, time + pointDur, letter.PositionAt(time).Y, 240 + point.y);
                    letter.Rotate(time, time + pointDur, letter.RotationAt(time), Math.Atan2(240 - letter.PositionAt(time+pointDur).Y, 320 - letter.PositionAt(time+pointDur).X) - Math.PI / 2);
                    time += pointDur;
                }
                letter.Fade(OsbEasing.InExpo, e - letterFadeDuration, e, 1, 0);
            });
        }

        // https://canary.discord.com/channels/203050773645492224/483948843571085312/1168005481147682846
        // Credit - TunnelBrick
        public List<(double x, double y)> GetSpiralPoints(double a, int numPoints, double thetaMax, double thetaOffset = 0)
        {
            List<(double x, double y)> points = new List<(double x, double y)>();

            double thetaStep = thetaMax / numPoints;

            for (double theta = thetaMax; theta > 0; theta -= thetaStep)
            {
                double r = a * theta;
                double x = r * Math.Cos(theta + thetaOffset);
                double y = r * Math.Sin(theta + thetaOffset);
                points.Add((x, y));
            }

            return points;
        }

        public class StaffGroup
        {
            public string name;
            public List<string> staffs = new List<string>();

            public StaffGroup(string name)
            {
                this.name = name;
            }

            public void AddStaff(string staff)
            {
                staffs.Add(staff);
            }

            public void displayStaff(StoryboardLayer layer, FontGenerator font, double startTime, double endTime, Vector2 position)
            {
                var groupNameSprite = layer.CreateLyric(StaffCredits.Current, font, name, OsbOrigin.Centre, 0.2, position);
                groupNameSprite.Fade(startTime, startTime + 200, 0, 1);
                groupNameSprite.MoveY(OsbEasing.InOutSine, startTime, startTime + 200, position.Y + 10, position.Y);
                groupNameSprite.Fade(endTime - 200, endTime, 1, 0);
                float y = position.Y;
                double time = startTime;
                foreach (var staff in staffs)
                {
                    y += 20;
                    time += 20;
                    float x = position.X;
                    if (name == "GFX/DEVELOPER" || name == "REFEREES" || name == "PLAYTESTING" || name == "CASTERS" || name == "STREAMERS" || name == "MAPPERS" || name == "POOLERS")
                    {
                        x = staffs.IndexOf(staff) % 2 == 0 ? position.X + 70 : position.X - 70;
                        y = staffs.IndexOf(staff) % 2 == 0 ? y : y - 20;
                    }
                    var staffSprite = layer.CreateLyric(StaffCredits.Current, font, staff, OsbOrigin.Centre, 0.2, new Vector2(x, y));
                    staffSprite.Fade(time, time + 200, 0, 1);
                    staffSprite.MoveY(OsbEasing.InOutSine, time, time + 200, y + 10, y);
                    staffSprite.Fade(endTime - 200, endTime, 1, 0);

                }
            }



        }

        void Particles(OsbEasing easing, double start, double duration, Vector2 pos)
        {
            int particleCount = 100;
            double maxRadius = 150;
            double endTime = start + duration;

            for (int i = 0; i <= particleCount; i++)
            {
                OsbSprite particle = GetLayer("").CreateSprite("sb/dot.png");
                double angle = Random(0, Math.PI * 2);
                double radius = Random(2, maxRadius);
                Vector2 position = new Vector2(
                    (float)(pos.X + Math.Cos(angle) * radius),
                    (float)(pos.Y + Math.Sin(angle) * radius)
                );
                particle.Move(easing, start, endTime, pos, position);
                particle.Fade(easing, start, endTime, 1, 0);
                particle.Scale(start, Random(0.1, 0.5));
            }
        }
    }
}
