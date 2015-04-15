﻿// 2015,2014 ,Apache2, WinterDev
using System;
using System.Collections.Generic;
using System.Text;
using PixelFarm.Drawing;

namespace LayoutFarm.Text
{

    public abstract class EditableRun : RenderElement
    {

        EditableTextLine ownerTextLine;
        LinkedListNode<EditableRun> _internalLinkedNode; 

        public EditableRun(RootGraphic gfx)
            : base(gfx, 10, 10)
        {
        }
        public bool IsLineBreak { get; set; }
        public abstract EditableRun Clone();
        internal LinkedListNode<EditableRun> internalLinkedNode
        {
            get { return this._internalLinkedNode; }

        }
        internal void SetInternalLinkedNode(LinkedListNode<EditableRun> linkedNode, EditableTextLine ownerTextLine)
        {
            this.ownerTextLine = ownerTextLine;
            this._internalLinkedNode = linkedNode;
            EditableRun.SetParentLink(this, ownerTextLine);
        }

        public abstract bool IsInsertable { get; } 
        public abstract TextSpanStyle SpanStyle { get; }
        public abstract void SetStyle(TextSpanStyle spanStyle);


        public abstract int GetRunWidth(int charOffset);
        public abstract string Text { get; }
        public abstract void UpdateRunWidth();


        public abstract void CopyContentToStringBuilder(StringBuilder stBuilder);
        public abstract char GetChar(int index);
        public abstract int CharacterCount { get; }


        public abstract EditableRun Copy(int startIndex, int length);

        public abstract EditableRun Copy(int startIndex);


        internal EditableTextLine OwnerEditableLine
        {
            get
            {
                return this.ownerTextLine;
            }
        }
        public EditableRun NextTextRun
        {
            get
            {
                if (this.internalLinkedNode != null)
                {
                    if (internalLinkedNode.Next != null)
                    {
                        return internalLinkedNode.Next.Value;
                    }
                }
                return null;
            }
        }
        public EditableRun PrevTextRun
        {
            get
            {

                if (this.internalLinkedNode != null)
                {
                    if (internalLinkedNode.Previous != null)
                    {
                        return internalLinkedNode.Previous.Value;
                    }
                }
                return null;
            }
        }


        internal abstract EditableRun Remove(int startIndex, int length, bool withFreeRun);
        public abstract int GetCharWidth(int index);
        public abstract VisualLocationInfo GetCharacterFromPixelOffset(int pixelOffset);
        public abstract EditableRun LeftCopy(int index);
        public abstract void InsertAfter(int index, char c);


        public static EditableRun InnerRemove(EditableRun tt, int startIndex, int length, bool withFreeRun)
        {
            return tt.Remove(startIndex, length, withFreeRun);
        }
        public static EditableRun InnerRemove(EditableRun tt, int startIndex, bool withFreeRun)
        {
            return tt.Remove(startIndex, tt.CharacterCount - (startIndex), withFreeRun);
        }
        public static VisualLocationInfo InnerGetCharacterFromPixelOffset(EditableRun tt, int pixelOffset)
        {
            return tt.GetCharacterFromPixelOffset(pixelOffset);
        }



#if DEBUG
        public override string ToString()
        {

            return "[" + this.dbug_obj_id + "]" + Text;
        }
#endif
        public static void InnerTextRunTopDownReCalculateContentSize(EditableRun ve)
        {
#if DEBUG
            dbug_EnterTopDownReCalculateContent(ve);
#endif

            ve.UpdateRunWidth();

#if DEBUG
            dbug_ExitTopDownReCalculateContent(ve);
#endif
        }
        public override void TopDownReCalculateContentSize()
        {
            InnerTextRunTopDownReCalculateContentSize(this);
        }
#if DEBUG
        public override string dbug_FullElementDescription()
        {
            string user_elem_id = null;
            if (user_elem_id != null)
            {
                return dbug_FixedElementCode + dbug_GetBoundInfo() + " "
                    + " i" + dbug_obj_id + "a " + ((EditableRun)this).Text + ",(ID " + user_elem_id + ") " + dbug_GetLayoutInfo();
            }
            else
            {
                return dbug_FixedElementCode + dbug_GetBoundInfo() + " "
                 + " i" + dbug_obj_id + "a " + ((EditableRun)this).Text + " " + dbug_GetLayoutInfo();
            }
        }
#endif
    }
}