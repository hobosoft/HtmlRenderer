﻿//2014 Apache2, WinterDev
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text; 

namespace LayoutFarm.Drawing
{

    public struct FontSignature
    {
        public static readonly FontSignature Empty = new FontSignature();
        string _fontName;
        float _fontSize; 
        FontStyle style;
        public FontSignature(string fontName, float fontSize, FontStyle style)
        {
            this._fontName = fontName;
            this._fontSize = fontSize;
            this.style = style;
        }
        public FontStyle FontStyle
        {
            get
            {
                return this.style;
            }
        }

        public string FontName
        {
            get
            {
                return this._fontName;
            }
        }
        public float FontSize
        {
            get
            {
                return this._fontSize;
            }
        }

    }

     
 
}
