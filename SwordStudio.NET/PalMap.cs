using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using SQWORD    = System.Int64;
using QWORD     = System.UInt64;
using LPSTR     = System.String;

using PAL_POS   = System.UInt32;

using SwordStudio.NET;
using SwordStudio.NET.Properties;
using PalVideo;
using PalGlobal;
using PalRes;
using PalScene;

using static PalGlobal.Pal_Global;
using static PalCommon.Pal_Common;
using static PalVideo.Pal_Video;
using static PalConfig.Pal_Config;
using static PalCfg.Pal_Cfg;
using static PalScene.Pal_Scene;
using static PalRes.Pal_Res;

namespace PalMap
{
    public class Pal_Map
    {
        //The width of the horizontal slice of Tile at different y-coordinates
        private static WORD[,] SegmentTable = new WORD[16,32]
        {
            { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, }, // 0
            { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, }, // 1
            { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, }, // 2
            { 2, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 3, 3, 3, 3, 3, 3, 3, }, // 3
            { 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 3, 3, 3, 3, 3, }, // 4
            { 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 3, 3, 3, }, // 5
            { 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 3, }, // 6
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, }, // 7
            { 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 4, }, // 8
            { 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 4, 4, 4, }, // 9
            { 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 4, 4, 4, 4, 4, }, // 10
            { 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 4, 4, 4, 4, 4, 4, 4, }, // 11
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, }, // 12
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, }, // 13
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, }, // 14
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, }, // 14
        };

        public class Pal_Map_Tile
        {
            public BOOL     fIsNoPassBlock  = FALSE;
            public SHORT    LowTile_Num     = 0;
            public BYTE     LowTile_Layer   = 0;
            public SHORT    HighTile_Num    = 0;
            public BYTE     HighTile_Layer  = 0;
        }

        public const    WORD                wMapWidth           = 2064, wMapHeight = 2055;
        public static   readonly    PAL_POS dwMinMapPos         = PAL_XY(0, 0);

        public static   Surface             sfMapViewport;
        public static   PAL_POS             Viewport            = Pal_Map.dwMinMapPos;
        public static   PAL_POS             posClick            = PAL_XY(0, 0);
        public static   PAL_DISPLAY_MODE    DisplayMode;
        public static   Surface             TileSurface         = new Surface(Pal_Map.iTileWidth, Pal_Map.iTileHeight);
        public static   Image               TileImage           = new Bitmap(Pal_Map.TileSurface.w, Pal_Map.TileSurface.h);
        public const    WORD                iTileWidth          = 32, iTileHeight   = 15;
        public static   INT                 iMapNum             = -1, iSceneNum     = -1;
        public static   Pal_Map_Tile[,,]    Tiles               = new Pal_Map_Tile[128, 64, 2];
        public static   BYTE[]              TileSprite;

        public static BOOL
        PAL_MapTileIsBlocked(
            BYTE        x,
            BYTE        y,
            BYTE        h
        )
        /*++
          Purpose:

            Check if the tile at the specified location is blocked.

          Parameters:

            [IN]  x - Column number of the tile.

            [IN]  y - Line number in the map.

            [IN]  h - Each line in the map has two lines of tiles, 0 and 1.
                      (See map.h for details.)

            [IN]  lpMap - Pointer to the loaded map.

          Return value:

            TRUE if the tile is blocked, FALSE if not.

        --*/
        {
            //
            // Check for invalid parameters.
            //
            if (x >= 64 || y >= 128 || h > 1) return TRUE;

            return Tiles[y, x, h].fIsNoPassBlock;
        }

        public static BYTE[]
        PAL_MapGetTileBitmap(
            BYTE            x,
            BYTE            y,
            BYTE            h,
            BYTE            ucLayer
        )
        /*++
          Purpose:

            Get the tile bitmap on the specified layer at the location (x, y, h).

          Parameters:

            [IN]  x - Column number of the tile.

            [IN]  y - Line number in the map.

            [IN]  h - Each line in the map has two lines of tiles, 0 and 1.
                      (See map.h for details.)

            [IN]  ucLayer - The layer. 0 for bottom, 1 for top.

            [IN]  lpMap - Pointer to the loaded map.

          Return value:

            Pointer to the bitmap. NULL if failed.

        --*/
        {
            Pal_Map_Tile    Tile;

            //
            // Check for invalid parameters.
            //
            if (x >= 64 || y >= 128 || h > 1) return null;

            //
            // Get the tile data of the specified location.
            //
            Tile = Tiles[y, x, h];

            if (ucLayer == 0)
            {
                //
                // Bottom layer
                //
                return PAL_SpriteGetFrame(TileSprite, Tile.LowTile_Num);
            }
            else
            {
                //
                // Top layer
                //
                return PAL_SpriteGetFrame(TileSprite, Tile.HighTile_Num);
            }
        }

        public static BYTE
        PAL_MapGetTileHeight(
            BYTE       x,
            BYTE       y,
            BYTE       h,
            BYTE       ucLayer
        )
        /*++
          Purpose:

            Get the logical height value of the specified tile. This value is used
            to judge whether the tile bitmap should cover the sprites or not.

          Parameters:

            [IN]  x - Column number of the tile.

            [IN]  y - Line number in the map.

            [IN]  h - Each line in the map has two lines of tiles, 0 and 1.
                      (See map.h for details.)

            [IN]  ucLayer - The layer. 0 for bottom, 1 for top.

            [IN]  lpMap - Pointer to the loaded map.

          Return value:

            The logical height value of the specified tile.

        --*/
        {
            Pal_Map_Tile    Tile;

            //
            // Check for invalid parameters.
            //
            if (y >= 128 || x >= 64 || h > 1) return 0;

            Tile = Tiles[y, x, h];

            if (ucLayer == 0)
            {
                //
                // Bottom layer
                //
                return (BYTE)Tile.LowTile_Layer;
            }
            else
            {
                //
                // Top layer
                //
                return (BYTE)Tile.HighTile_Layer;
            }
        }

        public static void
        PAL_MapBlitToSurface(
            Surface             sfSurface,
            Rect                SrcRect,
            BYTE                ucLayer
        )
        /*++
          Purpose:

            Blit the specified map area to a SDL Surface.

          Parameters:

            [OUT] lpSurface - Pointer to the destination surface.

            [IN]  lpSrcRect - Pointer to the source area.

            [IN]  ucLayer - The layer. 0 for bottom, 1 for top.

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
                        if (ucLayer < 2)
                        {
                            Bitmap = PAL_MapGetTileBitmap((BYTE)x, (BYTE)y, (BYTE)h, ucLayer);

                            if (Bitmap == null)
                            {
                                if (ucLayer != 0) continue;

                                Bitmap = PAL_MapGetTileBitmap(0, 0, 0, ucLayer);
                            }

                            PAL_RLEBlitToSurface(Bitmap, sfSurface, PAL_XY(xPos, yPos));
                        }
                        else if (PAL_MapTileIsBlocked((BYTE)x, (BYTE)y, (BYTE)h))
                            PAL_RLEBlitToSurface(bitmapNoPass, sfSurface, PAL_XY(xPos, yPos));
                    }
                }
            }
        }

        public static void
        PAL_SceneDrawSprites()
        /*++
           Purpose:

             Draw all the sprites to scene.

           Parameters:

             None.

           Return value:

             None.

        --*/
        {
            int i, x, y, vy;

            Pal_Scene.g_nSpriteToDraw = 0;

            //
            // Put all the sprites to be drawn into our array.
            //
            BYTE[]      lpFrame;
            BYTE[]      lpSprite;
            Pal_Object  poEvent = Pal_Global.poMainData.Where(MainItem => MainItem.TableName.Equals(lpszEvent)).First();
            dynamic[,]  EvtObj  = poEvent.Data;
            dynamic     State, VanishTime, CurrentFrameNum, SpriteFrames, Direction, X, Y, Layer, SpriteNum;
            int         iFrame;

            //
            // Event Objects (Monsters/NPCs/others)
            //
            for (i = Form_SceneSelect._iStartEvent; i < Form_SceneSelect._iEndEvent; i++)
            {
                State           = poEvent.GetItem(i, lpszState);
                VanishTime      = poEvent.GetItem(i, lpszVanishTime);
                CurrentFrameNum = poEvent.GetItem(i, lpszCurrentFrameNum);
                SpriteFrames    = poEvent.GetItem(i, lpszSpriteFrames);
                Direction       = poEvent.GetItem(i, lpszDirection);
                X               = poEvent.GetItem(i, lpszX);
                Y               = poEvent.GetItem(i, lpszY);
                Layer           = poEvent.GetItem(i, lpszLayer);
                SpriteNum       = poEvent.GetItem(i, lpszSpriteNum);

                if (State == ((BYTE)OBJECTSTATE.Hidden) || VanishTime > 0 || State < 0) continue;

                //
                // Get the sprite
                //
                lpSprite        = PAL_GetEventObjectSprite(SpriteNum);

                if (lpSprite == null) continue;

                iFrame = CurrentFrameNum;

                if (SpriteFrames == 3)
                {
                    //
                    // walking character
                    //
                    if (iFrame == 2) iFrame = 0;

                    if (iFrame == 3) iFrame = 2;
                }

                lpFrame = PAL_SpriteGetFrame(lpSprite, Direction * SpriteFrames + iFrame);

                if (lpFrame == null) continue;

                //
                // Calculate the coordinate and check if outside the screen
                //
                x = (SHORT)X - PAL_X(Viewport);
                x -= PAL_RLEGetWidth(lpFrame) / 2;

                if (x >= sfMapViewport.w || x < -PAL_RLEGetWidth(lpFrame))
                {
                    //
                    // outside the screen; skip it
                    //
                    continue;
                }

                y = (SHORT)Y - PAL_Y(Viewport);
                y += Layer * 8 + 9;

                vy = y - PAL_RLEGetHeight(lpFrame) - Layer * 8 + 2;

                //
                // outside the screen; skip it
                //
                if (vy >= sfMapViewport.h || vy < -PAL_RLEGetHeight(lpFrame)) continue;

                //
                // Add it into the array
                //
                PAL_AddSpriteToDraw(lpFrame, x, y, Layer * 8 + 2);

                //
                // Calculate covering map tiles
                //
                PAL_CalcCoverTiles(g_rgSpriteToDraw[Pal_Scene.g_nSpriteToDraw - 1]);
            }

            //
            // All sprites are now in our array; sort them by their vertical positions.
            //
            for (x = 0; x < Pal_Scene.g_nSpriteToDraw - 1; x++)
            {
                SPRITE_TO_DRAW tmp;
                BOOL fSwap = FALSE;

                for (y = 0; y < Pal_Scene.g_nSpriteToDraw - 1 - x; y++)
                {
                    if (PAL_Y(g_rgSpriteToDraw[y].pos) > PAL_Y(g_rgSpriteToDraw[y + 1].pos))
                    {
                        fSwap = TRUE;
                        tmp = g_rgSpriteToDraw[y];

                        g_rgSpriteToDraw[y] = g_rgSpriteToDraw[y + 1];
                        g_rgSpriteToDraw[y + 1] = tmp;
                    }
                }

                if (!fSwap)
                {
                    break;
                }
            }

            //
            // Draw all the sprites to the screen.
            //
            for (i = 0; i < Pal_Scene.g_nSpriteToDraw; i++)
            {
                SPRITE_TO_DRAW p = g_rgSpriteToDraw[i];

                x = PAL_X(p.pos);
                y = PAL_Y(p.pos) - PAL_RLEGetHeight(p.lpSpriteFrame) - p.iLayer;

                PAL_RLEBlitToSurface(p.lpSpriteFrame, sfMapViewport, PAL_XY(x, y));
            }
        }

        public static void
        PAL_DrawMapToSurface(
            Surface             sfMapPreview,
            Rect                rect,
            PictureBox          pictureBox,
            PAL_POS             dwMapPos,
            PAL_DISPLAY_MODE    pdmDisplayMode = PAL_DISPLAY_MODE.None | PAL_DISPLAY_MODE.LowTile | PAL_DISPLAY_MODE.HighTile | PAL_DISPLAY_MODE.NoPassTile | PAL_DISPLAY_MODE.EventSprite,
            INT                 n_tupling = -1
        )
        {
            sfMapViewport   = sfMapPreview;
            Viewport        = dwMapPos;
            DisplayMode     = pdmDisplayMode;

            //
            // Step 1: Draw the complete map, for both of the layers.
            //
            if ((DisplayMode & PAL_DISPLAY_MODE.LowTile)  != 0) PAL_MapBlitToSurface(sfMapViewport.CleanSpirit(), rect, 0);
            if ((DisplayMode & PAL_DISPLAY_MODE.HighTile) != 0) PAL_MapBlitToSurface(sfMapViewport,               rect, 1);

            //
            // Step 2: Apply screen waving effects.
            //
            Video_ApplyWave(sfMapViewport);

            //
            // Step 3: Draw all the sprites.
            //
            if ((DisplayMode & PAL_DISPLAY_MODE.EventSprite) != 0) PAL_SceneDrawSprites();

            PAL_RLEBlitToSurface(bitmapNoPass, sfMapViewport, posClick);

            //
            // Step 4: Draw all NoPass blocks.
            //
            if ((DisplayMode & PAL_DISPLAY_MODE.NoPassTile) != 0) PAL_MapBlitToSurface(sfMapViewport, rect, 2);

            //
            // Display the currently selected scene image
            //
            Video_DrawEnlargeBitmap(sfMapViewport, pictureBox.Image, n_tupling);

            pictureBox.Refresh();
        }

        public static PAL_POS
        PAL_XYH_TO_POS(
            WORD        x,
            WORD        y,
            WORD        h
        ) => PAL_XY(x * 32 + h * 16, y * 16 + h * 8);

        public static void
        PAL_POS_TO_XYH(
            PAL_POS     pos,
        out WORD        x,
        out WORD        y,
        out WORD        h
        )
        {
            WORD Segment, SegmentX, SegmentY;

            if (((SHORT)PAL_X(pos)) < 0 || ((SHORT)PAL_Y(pos)) < 0)
            {
                x   = 0xFFFF;
                y   = 0xFFFF;
                h   = 1;

                return;
            }

            SegmentX    = (WORD)(PAL_X(pos) % 32);
            SegmentY    = (WORD)(PAL_Y(pos) % 16);
            Segment     = SegmentTable[SegmentY, SegmentX];

            x = (BYTE)(PAL_X(pos) / 32);
            y = (BYTE)(PAL_Y(pos) / 16);
            h           = 0;

            if (Segment != 0)
            {
                if (((SHORT)(PAL_X(pos) - 16)) < 0 || ((SHORT)(PAL_Y(pos) - 8)) < 0)
                {
                    x = 0xFFFF;
                    y = 0xFFFF;
                    h = 1;

                    return;
                }

                SegmentX    = (WORD)((PAL_X(pos) - 16) % 32);
                SegmentY    = (WORD)((PAL_Y(pos) - 8) % 16);
                Segment     = SegmentTable[SegmentY, SegmentX];

                x = (BYTE)((PAL_X(pos) - 16) / 32);
                y = (BYTE)((PAL_Y(pos) - 8) / 16);
                h           = 1;
            }

            switch (Segment)
            {
                case 1:
                    {
                        if (h != 0)
                        {
                            y  += 1;
                            h   = 0;
                        }
                        else
                        {
                            x  -= 1;
                            h   = 1;
                        }
                    }
                    break;

                case 2:
                    {
                        if (h != 0)
                        {
                            h   = 0;
                        }
                        else
                        {
                            x  -= 1;
                            y  -= 1;
                            h   = 1;
                        }
                    }
                    break;

                case 3:
                    {
                        if (h != 0)
                        {
                            x  += 1;
                            h   = 0;
                        }
                        else
                        {
                            y  -= 1;
                            h   = 1;
                        }
                    }
                    break;

                case 4:
                    {
                        if (h != 0)
                        {
                            x  += 1;
                            y  += 1;
                            h   = 0;
                        }
                        else
                        {
                            h   = 1;
                        }
                    }
                    break;

                case 0:
                default:
                    {

                    }
                    break;
            }
        }
            
    }
}
