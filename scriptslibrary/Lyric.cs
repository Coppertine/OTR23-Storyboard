using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK;
using OpenTK.Graphics;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Subtitles;

namespace StorybrewScripts
{
    public class Lyric
    {
        public List<OsbSprite> Letters { get; set; }
        public string line;
        public FontGenerator font;
        private StoryboardObjectGenerator gen;
        public double scale { get; set; }
        private Vector2 startPosition;
        private Vector2 endPosition;
        private bool vertical;

        public Lyric(StoryboardLayer layer, StoryboardObjectGenerator gen, FontGenerator font, string line, OsbOrigin origin, double scale, Vector2 initialPosition, bool vertical)
        {
            this.scale = scale;
            this.line = line;
            this.font = font;
            this.gen = gen;
            this.startPosition = initialPosition;
            this.endPosition = initialPosition;
            this.vertical = vertical;

            Letters = new List<OsbSprite>();
            if (!vertical)
            {
                var lineWidth = GetLineWidth(this.scale);

                var letterX = initialPosition.X - lineWidth * 0.5f;
                foreach (char letter in line)
                {
                    var letterTexture = font.GetTexture(letter.ToString());
                    // "Hello World!"
                    if (!letterTexture.IsEmpty)
                    {
                        var letterPossition = new Vector2((float)letterX, initialPosition.Y) + letterTexture.OffsetFor(origin) * (float)scale;
                        var letterSprite = layer.CreateSprite(letterTexture.Path, origin, letterPossition);
                        Letters.Add(letterSprite);
                    }
                    else
                    {
                        Letters.Add(new OsbSprite());
                    }
                    letterX += letterTexture.BaseWidth * scale;
                }
            }
            else
            {
                var lineHeight = GetLineHeight(this.scale);

                var letterY = initialPosition.Y - lineHeight * 0.5f;
                foreach (char letter in line)
                {
                    var letterTexture = font.GetTexture(letter.ToString());
                    // "Hello World!"
                    if (!letterTexture.IsEmpty)
                    {
                        var letterPossition = new Vector2(initialPosition.X, (float)letterY) + letterTexture.OffsetFor(origin) * (float)scale;
                        if(letter == '?')
                            letterPossition += new Vector2(5,0);
                        var letterSprite = layer.CreateSprite(letterTexture.Path, origin, letterPossition);
                        Letters.Add(letterSprite);
                    }
                    else
                    {
                        Letters.Add(new OsbSprite());
                    }
                    letterY += letterTexture.BaseHeight * scale;
                }
            }
        }

        private double GetLineWidth(double scale)
        {
            var lineWidth = 0.0;
            //  "Test" => 'T' 'e' 's' 't'
            foreach (char letter in line)
            {
                var texture = font.GetTexture(letter.ToString());
                lineWidth += texture.BaseWidth * scale;
            }
            return lineWidth;
        }

        private double GetLineHeight(double scale)
        {
            var lineHeight = 0.0;
            //  "Test" => 'T' 'e' 's' 't'
            foreach (char letter in line)
            {
                var texture = font.GetTexture(letter.ToString());
                lineHeight += texture.BaseHeight * scale;
            }
            return lineHeight;
        }

        public void Fade(OsbEasing easing, double startTime, double endTime, double startOpacity, double endOpacity)
        {
            foreach (var letter in Letters)
            {
                if (letter.CommandCount == 0)
                {
                    letter.Scale(startTime, scale);
                }
                letter.Fade(easing,startTime, endTime, startOpacity, endOpacity);
            }
        }
        public void Fade(double startTime, double endTime, double startOpacity, double endOpacity)
        {
            foreach (var letter in Letters)
            {
                if (letter.CommandCount == 0)
                {
                    letter.Scale(startTime, scale);
                }
                letter.Fade(startTime, endTime, startOpacity, endOpacity);
            }
        }
        
        public void Additive(double time)
        {
            foreach(var letter in Letters)
            {
                letter.Additive(time);
            }
        }

        public void Fade(double time, double opacity)
        {
            foreach (var letter in Letters)
            {
                if (letter.CommandCount == 0)
                {
                    letter.Scale(time, scale);
                }
                letter.Fade(time, opacity);
            }
        }

        public void Color(OsbEasing easing, double startTime, double endTime, Color4 startColor, Color4 endColor)
        {
            foreach (var letter in Letters)
            {
                if (letter.CommandCount == 0)
                {
                    letter.Scale(startTime, scale);
                }
                letter.Color(easing, startTime, endTime, startColor, endColor);
            }
        }

        public void Color(double time, Color4 color)
        {
            foreach (var letter in Letters)
            {
                if (letter.CommandCount == 0)
                {
                    letter.Scale(time, scale);
                }
                letter.Color(time, color);
            }
        }

        public void Move(OsbEasing easing, double startTime, double endTime, Vector2 startPosition, Vector2 endPosition)
        {
            if (!vertical)
            {
                var lineWidth = GetLineWidth(this.scale);
                var startLetterX = startPosition.X - lineWidth * 0.5f;
                var endLetterX = endPosition.X - lineWidth * 0.5f;

                var i = 0;
                foreach (var letter in Letters)
                {
                    if (letter.CommandCount == 0)
                    {
                        letter.Scale(startTime, scale);
                    }
                    if (!font.GetTexture(line[i].ToString()).IsEmpty && letter != new OsbSprite())
                    {
                        var letterStartPosition = new Vector2((float)startLetterX, startPosition.Y) + font.GetTexture(line[i].ToString()).OffsetFor(letter.Origin) * (float)scale;
                        var letterEndPosition = new Vector2((float)endLetterX, endPosition.Y) + font.GetTexture(line[i].ToString()).OffsetFor(letter.Origin) * (float)scale;
                        letter.Move(easing, startTime, endTime, letterStartPosition, letterEndPosition);
                    }
                    startLetterX += font.GetTexture(line[i].ToString()).BaseWidth * scale;
                    endLetterX += font.GetTexture(line[i].ToString()).BaseWidth * scale;
                    i++;
                }
            }
            else
            {
                var lineHeight = GetLineHeight(this.scale);
                var startLetterY = startPosition.Y - lineHeight * 0.5f;
                var endLetterY = endPosition.Y - lineHeight * 0.5f;

                var i = 0;
                foreach (var letter in Letters)
                {
                    if (letter.CommandCount == 0)
                    {
                        letter.Scale(startTime, scale);
                    }
                    if (!font.GetTexture(line[i].ToString()).IsEmpty && letter != new OsbSprite())
                    {
                        var letterStartPosition = new Vector2(startPosition.X, (float)startLetterY) + font.GetTexture(line[i].ToString()).OffsetFor(letter.Origin) * (float)scale;
                        var letterEndPosition = new Vector2(endPosition.X, (float)endLetterY) + font.GetTexture(line[i].ToString()).OffsetFor(letter.Origin) * (float)scale;
                        letter.Move(easing, startTime, endTime, letterStartPosition, letterEndPosition);
                    }
                    startLetterY += font.GetTexture(line[i].ToString()).BaseHeight * scale;
                    endLetterY += font.GetTexture(line[i].ToString()).BaseHeight * scale;
                    i++;
                }
            }
        }

        public void MoveX(OsbEasing easing, double startTime, double endTime, double startX, double endX)
        {
            if (!vertical)
            {
                var lineWidth = GetLineWidth(this.scale);
                var startLetterX = startPosition.X - lineWidth * 0.5f;
                var endLetterX = endPosition.X - lineWidth * 0.5f;
                this.startPosition.X = (float)startX;
                this.endPosition.X = (float)endX;


                var i = 0;
                foreach (var letter in Letters)
                {
                    if (letter.CommandCount == 0)
                    {
                        letter.Scale(startTime, scale);
                    }
                    if (!font.GetTexture(line[i].ToString()).IsEmpty && letter != new OsbSprite())
                    {
                        var letterStartX = new Vector2((float)startLetterX, 0) + font.GetTexture(line[i].ToString()).OffsetFor(letter.Origin) * (float)scale;
                        var letterEndX = new Vector2((float)endLetterX, 0) + font.GetTexture(line[i].ToString()).OffsetFor(letter.Origin) * (float)scale;
                        letter.MoveX(easing, startTime, endTime, letterStartX.X, letterEndX.X);
                    }
                    startLetterX += font.GetTexture(line[i].ToString()).BaseWidth * scale;
                    endLetterX += font.GetTexture(line[i].ToString()).BaseWidth * scale;
                    i++;
                }
            }
            else
            {
                foreach (var letter in Letters)
                {
                    if (letter.CommandCount == 0)
                    {
                        letter.Scale(startTime, scale);
                    }
                    letter.MoveX(easing, startTime, endTime, startX, endX);
                }
            }
        }
        public void MoveY(OsbEasing easing, double startTime, double endTime, double startY, double endY)
        {
            if (!vertical)
            {
                foreach (var letter in Letters)
                {
                    if (letter.CommandCount == 0)
                    {
                        letter.Scale(startTime, scale);
                    }
                    letter.MoveY(easing, startTime, endTime, startY, endY);
                }
            }
            else
            {
                var lineHeight = GetLineHeight(this.scale);
                this.startPosition.Y = (float)startY;
                this.endPosition.Y = (float)endY;
                var startLetterY = startPosition.Y - lineHeight * 0.5f;
                var endLetterY = endPosition.Y - lineHeight * 0.5f;
                
                var i = 0;
                foreach (var letter in Letters)
                {
                    if (letter.CommandCount == 0)
                    {
                        letter.Scale(startTime, scale);
                    }
                    if (!font.GetTexture(line[i].ToString()).IsEmpty && letter != new OsbSprite())
                    {
                        var letterStartY = new Vector2(0, (float)startLetterY) + font.GetTexture(line[i].ToString()).OffsetFor(letter.Origin) * (float)scale;
                        var letterEndY = new Vector2(0, (float)endLetterY) + font.GetTexture(line[i].ToString()).OffsetFor(letter.Origin) * (float)scale;
                        gen.Log($"{startTime} {endTime} | {letterStartY.Y} {letterEndY.Y}");
                        letter.MoveY(easing, startTime, endTime, letterStartY.Y, letterEndY.Y);
                    }
                    startLetterY += font.GetTexture(line[i].ToString()).BaseHeight * scale;
                    endLetterY += font.GetTexture(line[i].ToString()).BaseHeight * scale;
                    i++;
                }
            }
        }
        public void MoveY(double endTime, double endY)
        {
            if (!vertical)
            {
                foreach (var letter in Letters)
                {
                    if (letter.CommandCount == 0)
                    {
                        letter.Scale(endTime, scale);
                    }
                    letter.MoveY(endTime, endY);
                }
            }
            else
            {
                var lineHeight = GetLineHeight(this.scale);
                var endLetterY = endPosition.Y - lineHeight * 0.5f;
                this.endPosition.Y = (float)endY;

                var i = 0;
                foreach (var letter in Letters)
                {
                    if (letter.CommandCount == 0)
                    {
                        letter.Scale(endTime, scale);
                    }
                    if (!font.GetTexture(line[i].ToString()).IsEmpty && letter != new OsbSprite())
                    {
                        var letterEndY = new Vector2(0, (float)endLetterY) + font.GetTexture(line[i].ToString()).OffsetFor(letter.Origin) * (float)scale;
                        letter.MoveY(endTime, letterEndY.Y);
                    }
                    endLetterY += font.GetTexture(line[i].ToString()).BaseHeight * scale;
                    i++;
                }
            }
        }

        public void MoveX(double time, double x)
        {
            if (!vertical)
            {
                var lineWidth = GetLineWidth(this.scale);

                var endLetterX = endPosition.X - lineWidth * 0.5f;
                this.startPosition.X = (float)x;
                this.endPosition.X = (float)x;

                var i = 0;
                foreach (var letter in Letters)
                {
                    if (letter.CommandCount == 0)
                    {
                        letter.Scale(time, scale);
                    }
                    if (!font.GetTexture(line[i].ToString()).IsEmpty && letter != new OsbSprite())
                    {

                        var letterEndX = new Vector2((float)endLetterX, 0) + font.GetTexture(line[i].ToString()).OffsetFor(letter.Origin) * (float)scale;
                        letter.MoveX(time, letterEndX.X);
                    }
                    endLetterX += font.GetTexture(line[i].ToString()).BaseWidth * scale;
                    i++;
                }
            }
            else
            {
                foreach (var letter in Letters)
                {
                    if (letter.CommandCount == 0)
                    {
                        letter.Scale(time, scale);
                    }
                    letter.MoveX(time, x);
                }
            }
        }

        /// <summary>
        /// Scales the lyric to a specified size with the option to not allow the scaling of individual letters.
        /// Note: This uses OsbSprite.MoveX() / OsbSprite.MoveY() to move the letters to the spesified spacing, this will not work if Move() has been used.
        /// </summary>
        /// <param name="startTime">The time to start scaling.</param>
        /// <param name="endTime">The time to end scaling.</param>
        /// <param name="startScale">The starting scale.</param>
        /// <param name="endScale">The ending scale.</param>
        /// <param name="scaleLetters">Whether or not to allow individual letter scaling.</param>
        public void Scale(OsbEasing easing, double startTime, double endTime, double startScale, double endScale, bool scaleLetters = true)
        {
            if (!vertical)
            {
                var startLineWidth = GetLineWidth(startScale);
                var endLineWidth = GetLineWidth(endScale);
                var startLetterX = startPosition.X - startLineWidth * 0.5f;
                var endLetterX = endPosition.X - endLineWidth * 0.5f;
                this.scale = endScale;
                var i = 0;
                foreach (var letter in Letters)
                {
                    if (!font.GetTexture(line[i].ToString()).IsEmpty && letter != new OsbSprite())
                    {
                        var letterStartPosition = new Vector2((float)startLetterX, startPosition.Y) + font.GetTexture(line[i].ToString()).OffsetFor(letter.Origin) * (float)scale;
                        var letterEndPosition = new Vector2((float)endLetterX, endPosition.Y) + font.GetTexture(line[i].ToString()).OffsetFor(letter.Origin) * (float)scale;
                        letter.MoveX(easing, startTime, endTime, letterStartPosition.X, letterEndPosition.X);
                        if (scaleLetters)
                        {
                            letter.Scale(easing, startTime, endTime, startScale, endScale);
                        }
                    }
                    startLetterX += font.GetTexture(line[i].ToString()).BaseWidth * startScale;
                    endLetterX += font.GetTexture(line[i].ToString()).BaseWidth * endScale;
                    i++;
                }
            }
            else
            {
                var startLineHeight = GetLineHeight(startScale);
                var endLineHeight = GetLineHeight(endScale);
                var startLetterY = startPosition.Y - startLineHeight * 0.5f;
                var endLetterY = endPosition.Y - endLineHeight * 0.5f;
                this.scale = endScale;

                var i = 0;
                foreach (var letter in Letters)
                {
                    if (!font.GetTexture(line[i].ToString()).IsEmpty && letter != new OsbSprite())
                    {
                        var letterStartPosition = new Vector2(startPosition.X, (float)startLetterY) + font.GetTexture(line[i].ToString()).OffsetFor(letter.Origin) * (float)scale;
                        var letterEndPosition = new Vector2(endPosition.X, (float)endLetterY) + font.GetTexture(line[i].ToString()).OffsetFor(letter.Origin) * (float)scale;
                        letter.MoveY(easing, startTime, endTime, letterStartPosition.Y, letterEndPosition.Y);
                        if (scaleLetters)
                        {
                            letter.Scale(easing, startTime, endTime, startScale, endScale);
                        }
                    }
                    startLetterY += font.GetTexture(line[i].ToString()).BaseHeight * startScale;
                    endLetterY += font.GetTexture(line[i].ToString()).BaseHeight * endScale;
                    i++;
                }
            }
        }

        /// <summary>
        /// Scales the lyric to a specified size with the option to not allow the scaling of individual letters.
        /// Note: This uses OsbSprite.MoveX() / OsbSprite.MoveY() to move the letters to the spesified spacing, this will not work if Move() has been used.
        /// </summary>
        /// <param name="startTime">The time to start scaling.</param>
        /// <param name="endTime">The time to end scaling.</param>
        /// <param name="startScale">The starting scale.</param>
        /// <param name="endScale">The ending scale.</param>
        /// <param name="scaleLetters">Whether or not to allow individual letter scaling.</param>
        public void Scale(double startTime, double endScale, bool scaleLetters = true)
        {
            if (!vertical)
            {
                var endLineWidth = GetLineWidth(endScale);
                var endLetterX = endPosition.X - endLineWidth * 0.5f;
                this.scale = endScale;

                var i = 0;
                foreach (var letter in Letters)
                {
                    if (!font.GetTexture(line[i].ToString()).IsEmpty && letter != new OsbSprite())
                    {
                        var letterEndScale = endScale;
                        var letterEndPosition = new Vector2((float)endLetterX, endPosition.Y) + font.GetTexture(line[i].ToString()).OffsetFor(letter.Origin) * (float)endScale;
                        letter.MoveX(startTime, letterEndPosition.X);
                        if (scaleLetters)
                        {
                            letter.Scale(startTime, letterEndScale);
                        }
                    }
                    endLetterX += font.GetTexture(line[i].ToString()).BaseWidth * endScale;
                    i++;
                }
            }
            else
            {
                var endLineHeight = GetLineHeight(endScale);
                var endLetterY = endPosition.Y - endLineHeight * 0.5f;
                this.scale = endScale;
                var i = 0;
                foreach (var letter in Letters)
                {
                    if (!font.GetTexture(line[i].ToString()).IsEmpty && letter != new OsbSprite())
                    {
                        var letterEndScale = endScale;
                        var letterEndPosition = new Vector2(endPosition.X, (float)endLetterY) + font.GetTexture(line[i].ToString()).OffsetFor(letter.Origin) * (float)endScale;
                        letter.MoveY(startTime, letterEndPosition.Y);
                        if (scaleLetters)
                        {
                            letter.Scale(startTime, letterEndScale);
                        }
                    }
                    endLetterY += font.GetTexture(line[i].ToString()).BaseHeight * endScale;
                    i++;
                }
            }
        }


    }
}