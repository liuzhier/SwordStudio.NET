using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using BOOL      = System.Boolean;
using CHAR      = System.Char;
using BYTE      = System.Byte;
using SHORT     = System.Int16;
using WORD      = System.UInt16;
using INT       = System.Int32;
using UINT      = System.UInt32;
using SDWORD    = System.Int32;
using DWORD     = System.UInt32;
using LPSTR     = System.String;

namespace UitlCtrls
{
    public class UtilButton : Button
    {
        public enum UtilButtonType
        {
            None        = 0,
            Radio       = 1,
            CheckBox    = 2,
        }

        private UtilButtonType  _ButtonType         = UtilButtonType.None;
        private ToolTip         _ToolTip            = new ToolTip();
        private string          _ButtonTipText      = null;
        private bool            _fIsToolTip         = false;
        private Color           NormalBack_Color    = Color.Transparent;
        private Color           TriggerBack_Color   = Color.Pink;
        private Color           HoverBack_Color     = Color.LightPink;
        private bool            _fIsClink           = false;

        [Browsable(true)]
        [DefaultValue(UtilButtonType.None)]
        [Localizable(true)]
        public  UtilButtonType  ButtonType
        {
            get { return _ButtonType; }
            set { _ButtonType = value; }
        }

        [Browsable(true)]
        [DefaultValue(null)]
        [Localizable(true)]
        public  string          ButtonTipText
        {
            get { return _ButtonTipText; }
            set { _ButtonTipText = value; }
        }

        public bool             GetIsClink()    => _fIsClink;

        public UtilButton()
        {
            this.FlatStyle                  = FlatStyle.Flat;
            this.FlatAppearance.BorderSize  = 0;
            this.BackColor                  = NormalBack_Color;
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);

            if (ButtonType == UtilButtonType.Radio)
            {
                foreach (UtilButton ThisBrother in this.Parent.Controls)
                {
                    if (ThisBrother.Equals(this)) continue;

                    ThisBrother._fIsClink = false;
                    ThisBrother.BackColor = NormalBack_Color;
                }

                _fIsClink = true;
                this.BackColor  = TriggerBack_Color;
            }
            else if (ButtonType == UtilButtonType.CheckBox)
            {
                this.BackColor = (_fIsClink = !_fIsClink) ? TriggerBack_Color : NormalBack_Color; 
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);

            this.BackColor = HoverBack_Color;

            if (!_fIsToolTip && _ButtonTipText != null)
            {
                //
                // Initialize Tip Text
                //
                _fIsToolTip = true;
                _ToolTip.SetToolTip(this, _ButtonTipText);
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            this.BackColor = _fIsClink ? TriggerBack_Color : NormalBack_Color;
        }
    }
}
