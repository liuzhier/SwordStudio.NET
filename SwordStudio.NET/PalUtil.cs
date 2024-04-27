using PalGlobal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

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

using static PalGlobal.Pal_Global;

namespace PalUtil
{
    public enum Pal_INT_SIZE
    {
        BOOL    = (1 << 0),
        CHAR    = (1 << 0),
        BYTE    = (1 << 0),

        SHORT   = (1 << 1),
        WORD    = (1 << 1),

        INT     = (1 << 2),
        UINT    = (1 << 2),

        SDWORD  = (1 << 3),
        DWORD   = (1 << 3),

        SQWORD  = (1 << 4),
        QWORD   = (1 << 4),
    }

    public class Pal_Util
    {
        public static void
        UTIL_ProcessParameters(
            string[]    args
        )
        {

        }

        public static void
        UTIL_RegEncode()
        {
            if (!Pal_Global.fIsRegEncode)
            {
                Pal_Global.fIsRegEncode = TRUE;
                //Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            }
        }

        public static BYTE[]
        UTIL_SubBytes(
            BYTE[]  array,
            INT     iOffset,
            INT     iLength         = -1,
            INT     iAlignmentNum   = -1
        )
        {
            //ArraySegment<BYTE>  asList;
            BYTE[]      bytes;
            INT         i, iRealLength = iLength;

            if (iRealLength == -1)
            {
                iRealLength = array.Length - iOffset;
            }

            if (iAlignmentNum != -1 && iRealLength % iAlignmentNum != 0)
            {
                iRealLength += iAlignmentNum - (iRealLength + iAlignmentNum) % iAlignmentNum;
            }

            bytes = new BYTE[iRealLength];

            if (iLength == -1) iLength =  iRealLength;
            for (i = 0; i < iLength; i++) bytes[i] = array[iOffset + i];


            //asList = new ArraySegment<BYTE>(array, iOffset, iLength);
                /*
                if (iAlignmentNum != -1)
                {
                    if (asList.Count % iAlignmentNum != 0)
                    {
                        for (i = 0; i < iAlignmentNum - (asList.Count % iAlignmentNum); i++) asList.Append((BYTE)0);
                    }
                }

                return asList.ToArray();
                */

            return bytes;
        }

        public static BYTE[]
        UTIL_SubBytes(
            BYTE[]          array,
            ref INT         iOffset,
            INT             iLength
        )
        {
            BYTE[] tmp = new ArraySegment<BYTE>(array, iOffset, iLength).ToArray();
            iOffset += iLength;

            return tmp;
        }

        public static TabControl
        UTIL_AddTabCtrl(
            TabPage     tcFather
        )
        {
            TabControl      This_TabCtrl;

            This_TabCtrl               = new TabControl();
            This_TabCtrl.SuspendLayout();
            tcFather.Controls.Add(This_TabCtrl);

            This_TabCtrl.Dock          = DockStyle.Fill;
            This_TabCtrl.Font          = new Font("Microsoft YaHei UI", 12.1008406F);
            This_TabCtrl.Location      = new Point(0, 0);
            This_TabCtrl.Name          = tcFather.Name + "_TabCtrl";
            This_TabCtrl.SelectedIndex = 0;
            This_TabCtrl.Size          = new Size(1395, 739);
            This_TabCtrl.SizeMode      = TabSizeMode.Fixed;
            This_TabCtrl.TabIndex      = 0;
            This_TabCtrl.ResumeLayout(false);

            return This_TabCtrl;
        }

        public static TabPage
        UTIL_AddTabPage(
            LPSTR       lpszTabName,
            LPSTR       lpszTabTitle,
            TabControl  tcFather
        )
        {
            TabPage         This_TabPage;
            Label           This_TabPage_Label;

            This_TabPage_Label = new Label();
            This_TabPage_Label.Dock = DockStyle.Fill;
            This_TabPage_Label.Font = new Font("Microsoft YaHei UI", 12.1008406F);
            This_TabPage_Label.Location = new Point(3, 3);
            This_TabPage_Label.Name = lpszTabName + "_Label";
            This_TabPage_Label.Size = new Size(1381, 693);
            This_TabPage_Label.TabIndex = 0;

            This_TabPage = new TabPage();
            This_TabPage.SuspendLayout();
            This_TabPage.Controls.Add(This_TabPage_Label);
            This_TabPage.Font = new Font("Microsoft YaHei UI", 12.1008406F);
            This_TabPage.Location = new Point(4, 36);
            This_TabPage.Name = lpszTabName + "TabPage";
            This_TabPage.Padding = new Padding(3);
            This_TabPage.Size = new Size(1387, 699);
            This_TabPage.TabIndex = 0;
            This_TabPage.Text = lpszTabTitle;
            This_TabPage.UseVisualStyleBackColor = true;

            tcFather.Controls.Add(This_TabPage);

            return This_TabPage;
        }

        public static LPSTR
        UTIL_GetFileCompletePath(
            LPSTR       lpszFileName
        )
        {
            return lpszGaemPath + PathDSC + lpszFileName;
        }

        public static INT
        UTIL_GetTypeSize(
            LPSTR       lpszType
        )
        {
            Pal_INT_SIZE     TypeSize = 0;

            switch (lpszType)
            {
                case "BOOL":
                case "CHAR":
                case "BYTE":
                    {
                        TypeSize = Pal_INT_SIZE.CHAR;
                    }
                    break;

                case "SHORT":
                case "WORD":
                    {
                        TypeSize = Pal_INT_SIZE.WORD;
                    }
                    break;

                case "INT":
                case "UINT":
                    {
                        TypeSize = Pal_INT_SIZE.UINT;
                    }
                    break;

                case "SDWORD":
                case "DWORD":
                    {
                        TypeSize = Pal_INT_SIZE.DWORD;
                    }
                    break;

                case "SQWORD":
                case "QWORD":
                    {
                        TypeSize = Pal_INT_SIZE.QWORD;
                    }
                    break;
            }

            return (INT)TypeSize;
        }

        public static ushort
        UTIL_SwapLE16(
            ushort value
        )
        {
            return BitConverter.ToUInt16(BitConverter.GetBytes(value), 0);
        }

        public static uint
        UTIL_SwapLE32(
            uint value
        )
        {
            return BitConverter.ToUInt32(BitConverter.GetBytes(value), 0);
        }
    }
}
