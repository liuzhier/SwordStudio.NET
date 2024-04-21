using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

using BOOL      = System.Boolean;
using CHAR      = System.Char;
using BYTE      = System.Byte;
using SHORT     = System.Int16;
using WORD      = System.UInt16;
using INT       = System.Int32;
using UINT      = System.UInt32;
using SDWORD    = System.Int64;
using DWORD     = System.UInt64;
using LPSTR     = System.String;

using PAL_POS   = System.UInt64;

namespace PalVideo
{
    public struct RGB
    {
        public BYTE     red;
        public BYTE     gree;
        public BYTE     blue;
    }

    public class Surface
    {
        public INT      w, h;
        public INT      pitch;
        public RGB[]    palette = new RGB[256];
        public BYTE[]   pixels;
        public BYTE[]   colors;
    }

    public class
    Pal_Video
    {
        public Surface _Surface = new Surface();

        public
        Pal_Video()
        {

        }
    }
}
