using System;

namespace Notky.Model
{
    [Serializable]
    public class NoteModel
    {
        public double left;
        public double top;
        public double height;
        public double width;
        public double fontSize;
        public string text;
        public string bgColor;
        public string frColor;

        public NoteModel(double left, double top, double height, double width, double fontSize, string text, string bgColor, string frColor)
        {
            this.left = left;
            this.top = top;
            this.height = height;
            this.width = width;
            this.fontSize = fontSize;
            this.text = text;
            this.bgColor = bgColor;
            this.frColor = frColor;
        }
    }
}