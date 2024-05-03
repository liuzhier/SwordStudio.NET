using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Diagnostics;

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

using PAL_POS   = System.UInt32;

using PalMap;

using static PalCommon.Pal_Common;
using static PalMap.Pal_Map;

namespace PalScene
{
    public class Pal_Scene
    {
        public class SPRITE_TO_DRAW
        {
            public  BYTE[]      lpSpriteFrame; // pointer to the frame bitmap
            public  PAL_POS     pos;           // position on the scene
            public  INT         iLayer;        // logical layer
        }
        
        public const    INT                 MAX_SPRITE_TO_DRAW  = 2048;
        public static   SPRITE_TO_DRAW[]    g_rgSpriteToDraw    = new SPRITE_TO_DRAW[MAX_SPRITE_TO_DRAW];
        public static   INT                 g_nSpriteToDraw;

        public static void
        PAL_AddSpriteToDraw(
            BYTE[]          lpSpriteFrame,
            int             x,
            int             y,
            int             iLayer
        )
        /*++
           Purpose:

             Add a sprite to our list of drawing.

           Parameters:

             [IN]  lpSpriteFrame - the bitmap of the sprite frame.

             [IN]  x - the X coordinate on the screen.

             [IN]  y - the Y coordinate on the screen.

             [IN]  iLayer - the layer of the sprite.

           Return value:

             None.

        --*/
        {
            Debug.Assert(g_nSpriteToDraw < MAX_SPRITE_TO_DRAW);

            if (g_rgSpriteToDraw[g_nSpriteToDraw] == null) g_rgSpriteToDraw[g_nSpriteToDraw] = new SPRITE_TO_DRAW();

            g_rgSpriteToDraw[g_nSpriteToDraw].lpSpriteFrame = lpSpriteFrame;
            g_rgSpriteToDraw[g_nSpriteToDraw].pos           = PAL_XY(x, y);
            g_rgSpriteToDraw[g_nSpriteToDraw].iLayer        = iLayer;

            g_nSpriteToDraw++;
        }

        public static void
        PAL_CalcCoverTiles(
           SPRITE_TO_DRAW       lpSpriteToDraw
        )
        /*++
           Purpose:

             Calculate all the tiles which may cover the specified sprite. Add the tiles
             into our list as well.

           Parameters:

             [IN]  lpSpriteToDraw - pointer to SPRITE_TO_DRAW struct.

           Return value:

             None.

        --*/
        {
            int             x, y, i, l, iTileHeight;
            BYTE[]          lpTile;

            INT sx = PAL_X(Pal_Map.Viewport) + PAL_X(lpSpriteToDraw.pos) - lpSpriteToDraw.iLayer / 2;
            INT sy = PAL_Y(Pal_Map.Viewport) + PAL_Y(lpSpriteToDraw.pos) - lpSpriteToDraw.iLayer;
            INT sh = (sx % 32 != 0) ? 1 : 0;

            INT width   = PAL_RLEGetWidth(lpSpriteToDraw.lpSpriteFrame);
            INT height  = PAL_RLEGetHeight(lpSpriteToDraw.lpSpriteFrame);

            int dx = 0;
            int dy = 0;
            int dh = 0;

            //
            // Loop through all the tiles in the area of the sprite.
            //
            for (y = (sy - height - 15) / 16; y <= sy / 16; y++)
            {
                for (x = (sx - width / 2) / 32; x <= (sx + width / 2) / 32; x++)
                {
                    for (i = ((x == (sx - width / 2) / 32) ? 0 : 3); i < 5; i++)
                    {
                        //
                        // Scan tiles in the following form (* = to scan):
                        //
                        // . . . * * * . . .
                        //  . . . * * . . . .
                        //
                        switch (i)
                        {
                            case 0:
                                dx = x;
                                dy = y;
                                dh = sh;
                                break;

                            case 1:
                                dx = x - 1;
                                break;

                            case 2:
                                dx = sh != 0 ? x : (x - 1);
                                dy = sh != 0 ? (y + 1) : y;
                                dh = 1 - sh;
                                break;

                            case 3:
                                dx = x + 1;
                                dy = y;
                                dh = sh;
                                break;

                            case 4:
                                dx = sh != 0 ? (x + 1) : x;
                                dy = sh != 0 ? (y + 1) : y;
                                dh = 1 - sh;
                                break;
                        }

                        for (l = 0; l < 2; l++)
                        {
                            lpTile      = PAL_MapGetTileBitmap((BYTE)dx, (BYTE)dy, (BYTE)dh, (BYTE)l);
                            iTileHeight = (CHAR)PAL_MapGetTileHeight((BYTE)dx, (BYTE)dy, (BYTE)dh, (BYTE)l);

                            //
                            // Check if this tile may cover the sprites
                            //
                            if (lpTile != null && iTileHeight > 0 && (dy + iTileHeight) * 16 + dh * 8 >= sy)
                            {
                                //
                                // This tile may cover the sprite
                                //
                                PAL_AddSpriteToDraw(lpTile, dx * 32 + dh * 16 - 16 - PAL_X(Pal_Map.Viewport),
                                    dy * 16 + dh * 8 + 7 + l + iTileHeight * 8 - PAL_Y(Pal_Map.Viewport), iTileHeight * 8 + l);
                            }
                        }
                    }
                }
            }
        }
    }
}
