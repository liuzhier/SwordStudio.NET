using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

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

using PalVideo;

using static PalGlobal.Pal_Global;
using static PalCommon.Pal_Common;
using static PalVideo.Pal_Video;
using System.Windows.Forms;

namespace PalMap
{
    public class Pal_Map
    {
        public static   DWORD[,,]   Tiles;
        public static   BYTE[]      TileSprite;
        public static   INT         iMapNum = -1;

        public static BYTE[]
        PAL_MapGetTileBitmap(
            BYTE        x,
            BYTE        y,
            BYTE        h,
            BOOL        fLayer
        )
        /*++
          Purpose:

            Get the tile bitmap on the specified layer at the location (x, y, h).

          Parameters:

            [IN]  x - Column number of the tile.

            [IN]  y - Line number in the map.

            [IN]  h - Each line in the map has two lines of tiles, 0 and 1.
                      (See map.h for details.)

            [IN]  fLayer - The layer. FALSE for bottom, TRUE for top.

          Return value:

            Pointer to the bitmap. NULL if failed.

        --*/
        {
            DWORD d;

            //
            // Check for invalid parameters.
            //
            if (x >= 64 || y >= 128 || h > 1)
            {
                return null;
            }

            //
            // Get the tile data of the specified location.
            //
            d = Tiles[y, x, h];

            if (fLayer)
            {
                //
                // Top layer
                //
                d >>= 16;
                return PAL_SpriteGetFrame(ref TileSprite, (INT)(((d & 0xFF) | ((d >> 4) & 0x100)) - 1));
            }
            else
            {
                //
                // Bottom layer
                //
                return PAL_SpriteGetFrame(ref TileSprite, (INT)((d & 0xFF) | ((d >> 4) & 0x100)));
            }
        }

        public static void
        PAL_MapBlitToSurface(
            Surface             sfSurface,
            Rect                SrcRect,
            BOOL                fLayer
        )
        /*++
          Purpose:

            Blit the specified map area to a SDL Surface.

          Parameters:

            [OUT] lpSurface - Pointer to the destination surface.

            [IN]  lpSrcRect - Pointer to the source area.

            [IN]  fLayer - The layer. FALSE for bottom, TRUE for top.

          Return value:

            None.

        --*/
        {
            int     sx, sy, dx, dy, x, y, h;
            WORD    xPos, yPos;
            BYTE[]  Bitmap = null;

            //
            // Convert the coordinate
            //
            sy = SrcRect.y / 16 - 1;
            dy = (SrcRect.y + SrcRect.h) / 16 + 2;
            sx = SrcRect.x / 32 - 1;
            dx = (SrcRect.x + SrcRect.w) / 32 + 2;

            //
            // Do the drawing.
            //
            yPos = (WORD)(sy * 16 - 8 - SrcRect.y);

            for (y = sy; y < dy; y++)
            {
                for (h = 0; h < 2; h++, yPos += 8)
                {
                    xPos = (WORD)(sx * 32 + h * 16 - 16 - SrcRect.x);

                    for (x = sx; x < dx; x++, xPos += 32)
                    {
                        Bitmap = PAL_MapGetTileBitmap((BYTE) x, (BYTE) y, (BYTE) h, fLayer);

                        if (Bitmap == null)
                        {
                            if (fLayer) continue;

                            Bitmap = PAL_MapGetTileBitmap(0, 0, 0, fLayer);
                        }

                        PAL_RLEBlitToSurface(ref Bitmap, sfSurface, PAL_XY(xPos, yPos));
                    }
                }
            }
        }

        public static void
        PAL_DrawMapToSurface(
            Surface         sfMapPreview,
            Rect            rect,
            PictureBox      pictureBox,
            INT             n_tupling
        )
        {
            //
            // Draw a scene map
            //
            {
                PAL_MapBlitToSurface(sfMapPreview.CleanSpirit(), rect, FALSE);
                PAL_MapBlitToSurface(sfMapPreview,               rect, TRUE);
            }

            //
            // Display the currently selected scene image
            //
            Video_DrawEnlargeBitmap(sfMapPreview, pictureBox.Image, n_tupling);

            pictureBox.Refresh();
        }
    }
}
