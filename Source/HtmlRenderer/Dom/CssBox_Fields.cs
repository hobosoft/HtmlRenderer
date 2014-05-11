﻿// "Therefore those skilled at the unorthodox
// are infinite as heaven and earth,
// inexhaustible as the great rivers.
// When they come to an end,
// they begin again,
// like the days and months;
// they die and are reborn,
// like the four seasons."
// 
// - Sun Tsu,
// "The Art of War"

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using HtmlRenderer.Entities;
using HtmlRenderer.Handlers;
using HtmlRenderer.Parse;
using HtmlRenderer.Utils;

namespace HtmlRenderer.Dom
{

    //collection features

    partial class CssBox
    {
        //----------------------------------------------------
        #region Fields and Consts
        /// <summary>
        /// the parent css box of this css box in the hierarchy
        /// </summary>
        private CssBox _parentBox;

        /// <summary>
        /// the root container for the hierarchy
        /// </summary>
        protected HtmlContainer _htmlContainer;

        /// <summary>
        /// the html tag that is associated with this css box, null if anonymous box
        /// </summary>
        private readonly HtmlTag _htmltag;

        /// <summary>
        /// the inner text of the box
        /// </summary>
        private SubString _text;

        /// <summary>
        /// Do not use or alter this flag
        /// </summary>
        /// <remarks>
        /// Flag that indicates that CssTable algorithm already made fixes on it.
        /// </remarks>
        internal bool _tableFixed;

        protected bool _wordsSizeMeasured;

        private CssBox _listItemBox;
        //----------------------------------------------------
        //if this is inline box , this CssBox
        //may be 'flowed' into more than one CssLineBox
        private CssLineBox _firstHostingLineBox;
        private CssLineBox _lastHostingLineBox;
        //one CssBox may use more than one cssline     
        //----------------------------------------------------

        int _boxCompactFlags;
        int _rowSpan;
        int _colSpan;

        /// <summary>
        /// handler for loading background image
        /// </summary>
        private ImageLoadHandler _imageLoadHandler;
        WellknownHtmlTagName wellKnownTagName;


        #endregion
        //----------------------------------------------------


        private readonly List<CssRect> _boxWords = new List<CssRect>();
        private readonly List<CssBox> _boxes = new List<CssBox>();
        private readonly LinkedList<CssLineBox> _lineBoxes = new LinkedList<CssLineBox>();

        /// <summary>
        /// Gets the childrenn boxes of this box
        /// </summary>
        List<CssBox> Boxes
        {
            get { return _boxes; }
        }
        public IEnumerable<CssBox> GetChildBoxIter()
        {
            var thisboxes = this._boxes;
            if (thisboxes != null)
            {
                //collection maybe modifier during iter ****
                //need to 'Count'.
                for (int i = 0; i < thisboxes.Count; ++i)
                {
                    yield return thisboxes[i];
                }
            }
        }

        internal IEnumerable<CssLineBox> GetHostLineIter()
        {
            var hostLine = this.FirstHostingLineBox;
            while (hostLine != null)
            {
                yield return hostLine;
                hostLine = hostLine.NextLine;
            }
        }
        public int ChildCount
        {
            get
            {
                return this._boxes.Count;
            }
        }

        public CssBox GetFirstChild()
        {
            return this._boxes[0];
        }
        //-----------------------------------
        public CssBox GetChildBox(int index)
        {
            //if (index > this._boxes.Count - 1)
            //{
            //}
            return this._boxes[index];
        }
        public void InsertChild(int index, CssBox box)
        {
            this.Boxes.Insert(index, box);
        }
        //--------
        internal void ResetLineBoxes()
        {
            //for (int i = _lineBoxes.Count - 1; i >= 0; --i)
            //{
            //    _lineBoxes[i].RemoveAllReferencedContent();
            //}
            _lineBoxes.Clear();
        }
        //-------------------------------------
        internal int RowSpan
        {
            get
            {
                if ((this._boxCompactFlags & CssBoxFlagsConst.EVAL_ROWSPAN) == 0)
                {
                    string att = this.GetAttribute("rowspan", "1");
                    int rowspan;
                    if (!int.TryParse(att, out rowspan))
                    {
                        rowspan = 1;
                    }
                    this._boxCompactFlags |= CssBoxFlagsConst.EVAL_ROWSPAN;
                    return this._rowSpan = rowspan;
                }
                return this._rowSpan;
            }
            set
            {
                this._rowSpan = value;
                this._boxCompactFlags |= CssBoxFlagsConst.EVAL_ROWSPAN;
            }
        }
        internal int ColSpan
        {
            get
            {
                if ((this._boxCompactFlags & CssBoxFlagsConst.EVAL_COLSPAN) == 0)
                {
                    string att = this.GetAttribute("colspan", "1");
                    int colspan;
                    if (!int.TryParse(att, out colspan))
                    {   
                        colspan = 1;
                    }
                    this._boxCompactFlags |= CssBoxFlagsConst.EVAL_COLSPAN;
                    return this._colSpan = colspan;
                }
                return this._colSpan;
            }
            set
            {
                this._colSpan = value;
            }
        }
        //-------------------------------------
    }

}