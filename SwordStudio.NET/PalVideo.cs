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
using SDWORD    = System.Int32;
using DWORD     = System.UInt32;
using LPSTR     = System.String;

using PAL_POS   = System.UInt64;

using PalGlobal;

using static PalGlobal.Pal_Global;
using static PalGlobal.Pal_File;
using static PalCommon.Pal_Common;
using System.Collections;
using System.Drawing;

namespace PalVideo
{
    public struct RGB
    {
        public BYTE     red;
        public BYTE     gree;
        public BYTE     blue;
    }

    public struct Rect
    {
        public INT      x,  y;
        public INT      w,  h;
    }

    public class Surface
    {
        public INT      w, h;
        public INT      pitch;
        public RGB[]    palette = new RGB[256];
        public BYTE[]   pixels  = null;
        public BYTE[]   colors  = null;

        private Surface() { }

        public
        Surface(
            INT     iWidth,
            INT     iHeight,
            INT     iPaletteNum,
            BOOL    fNight = FALSE
        )
        {
            INT     i, j;
            BYTE[]  binPat;

            w       = iWidth;
            h       = iHeight;
            pitch   = w * 1;
            pixels  = new BYTE[pitch * h];
            for (i = 0; i < pixels.Length; i++)
            {
                pixels[i] = 0xFF;
            }

            //
            // Get palette data
            //
            binPat  = Pal_File_GetFile(lpszPalette).bufFile;

            PAL_MKFReadChunk(ref binPat, iPaletteNum, ref binPat);

            //
            // Initialize color palette
            //
            for (i = 0; i < palette.Length; i++)
            {
                j = (fNight ? 256 * 3 : 0) + i * 3;

                palette[i].red = (BYTE)(binPat[j] << 2);
                palette[i].gree = (BYTE)(binPat[j + 1] << 2);
                palette[i].blue = (BYTE)(binPat[j + 2] << 2);
            }
        }

        public Surface
        CleanSpirit()
        {
            Array.Clear(pixels, 0, pixels.Length);

            return this;
        }

        public BYTE[]
        GetPixelRGB()
        {
            INT     i, j;
            RGB     rgbPalette;

            colors = new BYTE[pixels.Length * 3];

            for (i = 0; i < pixels.Length; i++)
            {
                j = i * 3;

                rgbPalette = palette[pixels[i]];

                colors[j++] = rgbPalette.red;
                colors[j++] = rgbPalette.gree;
                colors[j++] = rgbPalette.blue;
            }

            return colors;
        }
    }

    public class
    Pal_Video
    {
        public Surface _Surface = null;

        public
        Pal_Video(
            INT     iWidth,
            INT     iHeight
        )
        {
            _Surface = new Surface(iWidth, iHeight, 0);
        }

        public static void Video_DrawEnlargeBitmap(Surface surface, Image dest, INT n_tupling)
        {
            INT scaledWidth, scaledHeight, Width, Height, i, j, x, y, iAlpha = -1;
            Color color;
            Bitmap originalBitmap, scaledBitmap;
            BYTE[] RGB_List;

            Width = surface.w;
            Height = surface.h;

            scaledWidth = Width * n_tupling;
            scaledHeight = Height * n_tupling;

            originalBitmap = new Bitmap(Width, Height);
            scaledBitmap = (Bitmap)dest;

            RGB_List = surface.GetPixelRGB();

            for (i = 0, j = 0; i < RGB_List.Length; i += 3, j++)
            {
                x = j % Width;
                y = j / Width;

                if (RGB_List[i] == surface.palette[0xFF].red &&
                    RGB_List[i + 1] == surface.palette[0xFF].gree &&
                    RGB_List[i + 2] == surface.palette[0xFF].blue)
                    continue;

                color = Color.FromArgb(RGB_List[i], RGB_List[i + 1], RGB_List[i + 2]);
                originalBitmap.SetPixel(x, y, color);
            }

            using (Graphics graphics = Graphics.FromImage(scaledBitmap))
            {
                //
                // Set the drawing mode of the image to pixel alignment
                //
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;

                //
                // Clear the entire drawing surface and fill it with a transparent background color
                //
                graphics.Clear(Color.Transparent);

                //
                // Draw an enlarged image
                //
                graphics.DrawImage(originalBitmap, new Rectangle(0, 0, scaledWidth, scaledHeight), new Rectangle(0, 0, Width, Height), GraphicsUnit.Pixel);
            }
        }
    }
}
