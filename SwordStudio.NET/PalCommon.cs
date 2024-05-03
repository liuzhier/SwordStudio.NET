using System;
using System.IO;

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

using SwordStudio.NET;
using PalCfg;
using PalVideo;

using static PalGlobal.Pal_Global;
using static PalUtil.Pal_Util;
using static PalYJ_1.Pal_YJ_1;
using static PalVideo.Pal_Video;

namespace PalCommon
{
    public class Pal_Common
    {
        public static PAL_POS
        PAL_XY(dynamic x, dynamic y)  => (PAL_POS) ((((WORD)y << 16) & 0xFFFF0000) | ((WORD)x & 0xFFFF));

        public static dynamic
        PAL_X(PAL_POS xy)       => (WORD) ((xy) & 0xFFFF);

        public static dynamic
        PAL_Y(PAL_POS xy)       => (WORD) (((xy) >> 16) & 0xFFFF);

        public static BYTE
        PAL_CalcShadowColor(
           BYTE bSourceColor
        )
        {
            return (BYTE)((bSourceColor & 0xF0) | ((bSourceColor & 0x0F) >> 1));
        }

        public static INT
        PAL_RLEBlitToSurface(
           BYTE[]           lpBitmapRLE,
           Surface          lpDstSurface,
           PAL_POS          pos
        )
        {
            return PAL_RLEBlitToSurfaceWithShadow(lpBitmapRLE, lpDstSurface, pos, FALSE);
        }

        public static INT
        PAL_RLEBlitToSurfaceWithShadow(
           BYTE[]           lpBitmapRLE,
           Surface          lpDstSurface,
           PAL_POS          pos,
           BOOL             bShadow
        )
        {
            UINT    i, j, k, sx;
            INT     x, y;
            UINT    uiLen       = 0;
            UINT    uiWidth     = 0;
            UINT    uiHeight    = 0;
            UINT    uiSrcX      = 0;
            BYTE    T;
            INT     dx          = PAL_X(pos);
            INT     dy          = PAL_Y(pos);

            INT     iRleOffset = 0, iPixelsOffset = 0;

            //
            // Check for NULL pointer.
            //
            if (lpBitmapRLE == null || lpDstSurface == null)
            {
                return -1;
            }

            //
            // Skip the 0x00000002 in the file header.
            //
            if (lpBitmapRLE[iRleOffset    ] == 0x02 && lpBitmapRLE[iRleOffset + 1] == 0x00 &&
                lpBitmapRLE[iRleOffset + 2] == 0x00 && lpBitmapRLE[iRleOffset + 3] == 0x00)
            {
                iRleOffset += 4;
            }

            //
            // Get the width and height of the bitmap.
            //
            uiWidth  = (uint)(lpBitmapRLE[iRleOffset    ] | (lpBitmapRLE[iRleOffset + 1] << 8));
            uiHeight = (uint)(lpBitmapRLE[iRleOffset + 2] | (lpBitmapRLE[iRleOffset + 3] << 8));

            //
            // Check whether bitmap intersects the surface.
            //
            if (uiWidth  + dx <= 0 || dx >= lpDstSurface.w ||
                uiHeight + dy <= 0 || dy >= lpDstSurface.h)
            {
                goto end;
            }

            //
            // Calculate the total length of the bitmap.
            // The bitmap is 8-bpp, each pixel will use 1 BYTE.
            //
            uiLen = uiWidth * uiHeight;

            //
            // Start decoding and blitting the bitmap.
            //
            iRleOffset += 4;
            for (i = 0; i < uiLen;)
            {
                T = lpBitmapRLE[iRleOffset++];
                if (((T & 0x80) != 0) && (T <= (0x80 + uiWidth)))
                {
                    i      += (UINT)(T - 0x80);
                    uiSrcX += (UINT)(T - 0x80);

                    if (uiSrcX >= uiWidth)
                    {
                        uiSrcX -= uiWidth;
                        dy++;
                    }
                }
                else
                {
                    //
                    // Prepare coordinates.
                    //
                    j = 0;
                    sx = uiSrcX;
                    x = (INT)(dx + uiSrcX);
                    y = dy;

                    //
                    // Skip the points which are out of the surface.
                    //
                    if (y < 0)
                    {
                        j += (UINT)(-y * uiWidth);
                        y = 0;
                    }
                    else if (y >= lpDstSurface.h)
                    {
                        goto end; // No more pixels needed, break out
                    }

                    while (j < T)
                    {
                        //
                        // Skip the points which are out of the surface.
                        //
                        if (x < 0)
                        {
                            j -= (UINT)x;

                            if (j >= T) break;

                            sx -= (UINT)x;
                            x = 0;
                        }
                        else if (x >= lpDstSurface.w)
                        {
                            j += uiWidth - sx;
                            x = (INT)(x - sx);
                            sx = 0;
                            y++;

                            if (y >= lpDstSurface.h)
                            {
                                goto end; // No more pixels needed, break out
                            }

                            continue;
                        }

                        //
                        // Put the pixels in row onto the surface
                        //
                        k = T - j;

                        if (lpDstSurface.w - x < k) k = (uint)(lpDstSurface.w - x);
                        if (uiWidth - sx < k) k = uiWidth - sx;

                        sx += k;
                        iPixelsOffset = y * lpDstSurface.pitch;

                        if (bShadow)
                        {
                            j += k;

                            for (; k != 0; k--)
                            {
                                lpDstSurface.pixels[iPixelsOffset + x] = PAL_CalcShadowColor(lpDstSurface.pixels[iPixelsOffset + x]);
                                x++;
                            }
                        }
                        else
                        {
                            for (; k != 0; k--)
                            {
                                lpDstSurface.pixels[iPixelsOffset + x] = lpBitmapRLE[iRleOffset + j];
                                j++;
                                x++;
                            }
                        }

                        if (sx >= uiWidth)
                        {
                            sx -= uiWidth;
                            x = (INT)(x - uiWidth);
                            y++;

                            if (y >= lpDstSurface.h)
                            {
                                goto end; // No more pixels needed, break out
                            }
                        }
                    }

                    iRleOffset += T;
                    i += T;
                    uiSrcX += T;

                    while (uiSrcX >= uiWidth)
                    {
                        uiSrcX -= uiWidth;
                        dy++;
                    }
                }
            }

        end:
            //
            // Success
            //
            return 0;
        }

        public static INT
        PAL_RLEBlitWithColorShift(
           ref BYTE[]       lpBitmapRLE,
           Surface          lpDstSurface,
           PAL_POS          pos,
           INT              iColorShift
        )
        /*++
          Purpose:

            Blit an RLE-compressed bitmap to an SDL surface.
            NOTE: Assume the surface is already locked, and the surface is a 8-bit one.

          Parameters:

            [IN]  lpBitmapRLE - pointer to the RLE-compressed bitmap to be decoded.

            [OUT] lpDstSurface - pointer to the destination SDL surface.

            [IN]  pos - position of the destination area.

            [IN]  iColorShift - shift the color by this value.

          Return value:

            0 = success, -1 = error.

        --*/
        {
            UINT    i, j, k, sx;
            INT     x, y;
            UINT    uiLen       = 0;
            UINT    uiWidth     = 0;
            UINT    uiHeight    = 0;
            UINT    uiSrcX      = 0;
            BYTE    T, b;
            INT     dx          = PAL_X(pos);
            INT     dy          = PAL_Y(pos);

            INT     iRleOffset = 0, iPixelsOffset = 0;

            //
            // Check for NULL pointer.
            //
            if (lpBitmapRLE == null || lpDstSurface == null)
            {
                return -1;
            }

            //
            // Skip the 0x00000002 in the file header.
            //
            if (lpBitmapRLE[iRleOffset    ] == 0x02 && lpBitmapRLE[iRleOffset + 1] == 0x00 &&
                lpBitmapRLE[iRleOffset + 2] == 0x00 && lpBitmapRLE[iRleOffset + 3] == 0x00)
            {
                iRleOffset += 4;
            }

            //
            // Get the width and height of the bitmap.
            //
            uiWidth  = (uint)(lpBitmapRLE[iRleOffset    ] | (lpBitmapRLE[iRleOffset + 1] << 8));
            uiHeight = (uint)(lpBitmapRLE[iRleOffset + 2] | (lpBitmapRLE[iRleOffset + 3] << 8));

            //
            // Check whether bitmap intersects the surface.
            //
            if (uiWidth  + dx <= 0 || dx >= lpDstSurface.w ||
                uiHeight + dy <= 0 || dy >= lpDstSurface.h)
            {
                goto end;
            }

            //
            // Calculate the total length of the bitmap.
            // The bitmap is 8-bpp, each pixel will use 1 BYTE.
            //
            uiLen = uiWidth * uiHeight;

            //
            // Start decoding and blitting the bitmap.
            //
            iRleOffset += 4;
            for (i = 0; i < uiLen;)
            {
                T = lpBitmapRLE[iRleOffset++];
                if (((T & 0x80) != 0) && (T <= (0x80 + uiWidth)))
                {
                    i      += (UINT)(T - 0x80);
                    uiSrcX += (UINT)(T - 0x80);

                    if (uiSrcX >= uiWidth)
                    {
                        uiSrcX -= uiWidth;
                        dy++;
                    }
                }
                else
                {
                    //
                    // Prepare coordinates.
                    //
                    j = 0;
                    sx = uiSrcX;
                    x = (INT)(dx + uiSrcX);
                    y = dy;

                    //
                    // Skip the points which are out of the surface.
                    //
                    if (y < 0)
                    {
                        j += (UINT)(-y * uiWidth);
                        y = 0;
                    }
                    else if (y >= lpDstSurface.h)
                    {
                        goto end; // No more pixels needed, break out
                    }

                    while (j < T)
                    {
                        //
                        // Skip the points which are out of the surface.
                        //
                        if (x < 0)
                        {
                            j -= (UINT)x;

                            if (j >= T) break;

                            sx -= (UINT)x;
                            x = 0;
                        }
                        else if (x >= lpDstSurface.w)
                        {
                            j += uiWidth - sx;
                            x = (INT)(x - sx);
                            sx = 0;
                            y++;

                            if (y >= lpDstSurface.h)
                            {
                                goto end; // No more pixels needed, break out
                            }

                            continue;
                        }

                        //
                        // Put the pixels in row onto the surface
                        //
                        k = T - j;

                        if (lpDstSurface.w - x < k) k = (uint)(lpDstSurface.w - x);
                        if (uiWidth - sx < k) k = uiWidth - sx;

                        sx += k;
                        sx += k;
                        iPixelsOffset = y * lpDstSurface.pitch;

                        for (; k != 0; k--)
                        {
                            b = (BYTE)(lpBitmapRLE[iRleOffset + j] & 0x0F);

                            if ((INT)b + iColorShift > 0x0F)
                            {
                                b = 0x0F;
                            }
                            else if ((INT)b + iColorShift < 0)
                            {
                                b = 0;
                            }
                            else
                            {
                                b = (BYTE)(b + iColorShift);
                            }

                            lpDstSurface.pixels[iPixelsOffset + x] = (BYTE)(b | (lpBitmapRLE[iRleOffset + j] & 0xF0));
                            j++;
                            x++;
                        }

                        if (sx >= uiWidth)
                        {
                            sx -= uiWidth;
                            x = (INT)(x - uiWidth);
                            y++;

                            if (y >= lpDstSurface.h)
                            {
                                goto end; // No more pixels needed, break out
                            }
                        }
                    }

                    iRleOffset += T;
                    i += T;
                    uiSrcX += T;

                    while (uiSrcX >= uiWidth)
                    {
                        uiSrcX -= uiWidth;
                        dy++;
                    }
                }
            }

        end:
            //
            // Success
            //
            return 0;
        }

        public static INT
        PAL_RLEBlitMonoColor(
           ref BYTE[]       lpBitmapRLE,
           Surface          lpDstSurface,
           PAL_POS          pos,
           BYTE             bColor,
           INT              iColorShift
        )
        /*++
          Purpose:

            Blit an RLE-compressed bitmap to an SDL surface in mono-color form.
            NOTE: Assume the surface is already locked, and the surface is a 8-bit one.

          Parameters:

            [IN]  lpBitmapRLE - pointer to the RLE-compressed bitmap to be decoded.

            [OUT] lpDstSurface - pointer to the destination SDL surface.

            [IN]  pos - position of the destination area.

            [IN]  bColor - the color to be used while drawing.

            [IN]  iColorShift - shift the color by this value.

          Return value:

            0 = success, -1 = error.

        --*/
        {
            UINT          i, j, k, sx;
            INT           x, y;
            UINT          uiLen       = 0;
            UINT          uiWidth     = 0;
            UINT          uiHeight    = 0;
            UINT          uiSrcX      = 0;
            BYTE          T, b;
            INT           dx          = PAL_X(pos);
            INT           dy          = PAL_Y(pos);

            INT iRleOffset = 0, iPixelsOffset = 0;

            //
            // Check for NULL pointer.
            //
            if (lpBitmapRLE == null || lpDstSurface == null)
            {
                return -1;
            }

            //
            // Skip the 0x00000002 in the file header.
            //
            if (lpBitmapRLE[iRleOffset    ] == 0x02 && lpBitmapRLE[iRleOffset + 1] == 0x00 &&
                lpBitmapRLE[iRleOffset + 2] == 0x00 && lpBitmapRLE[iRleOffset + 3] == 0x00)
            {
                iRleOffset += 4;
            }

            //
            // Get the width and height of the bitmap.
            //
            uiWidth  = (uint)(lpBitmapRLE[iRleOffset    ] | (lpBitmapRLE[iRleOffset + 1] << 8));
            uiHeight = (uint)(lpBitmapRLE[iRleOffset + 2] | (lpBitmapRLE[iRleOffset + 3] << 8));

            //
            // Check whether bitmap intersects the surface.
            //
            if (uiWidth  + dx <= 0 || dx >= lpDstSurface.w ||
                uiHeight + dy <= 0 || dy >= lpDstSurface.h)
            {
                goto end;
            }

            //
            // Calculate the total length of the bitmap.
            // The bitmap is 8-bpp, each pixel will use 1 BYTE.
            //
            uiLen = uiWidth * uiHeight;

            //
            // Start decoding and blitting the bitmap.
            //
            iRleOffset += 4;
            bColor &= 0xF0;
            for (i = 0; i < uiLen;)
            {
                T = lpBitmapRLE[iRleOffset++];
                if (((T & 0x80) != 0) && (T <= (0x80 + uiWidth)))
                {
                    i      += (UINT)(T - 0x80);
                    uiSrcX += (UINT)(T - 0x80);

                    if (uiSrcX >= uiWidth)
                    {
                        uiSrcX -= uiWidth;
                        dy++;
                    }
                }
                else
                {
                    //
                    // Prepare coordinates.
                    //
                    j = 0;
                    sx = uiSrcX;
                    x = (INT)(dx + uiSrcX);
                    y = dy;

                    //
                    // Skip the points which are out of the surface.
                    //
                    if (y < 0)
                    {
                        j += (UINT)(-y * uiWidth);
                        y = 0;
                    }
                    else if (y >= lpDstSurface.h)
                    {
                        goto end; // No more pixels needed, break out
                    }

                    while (j < T)
                    {
                        //
                        // Skip the points which are out of the surface.
                        //
                        if (x < 0)
                        {
                            j -= (UINT)x;

                            if (j >= T) break;

                            sx -= (UINT)x;
                            x = 0;
                        }
                        else if (x >= lpDstSurface.w)
                        {
                            j += uiWidth - sx;
                            x = (INT)(x - sx);
                            sx = 0;
                            y++;

                            if (y >= lpDstSurface.h)
                            {
                                goto end; // No more pixels needed, break out
                            }

                            continue;
                        }

                        //
                        // Put the pixels in row onto the surface
                        //
                        k = T - j;

                        if (lpDstSurface.w - x < k) k = (uint)(lpDstSurface.w - x);
                        if (uiWidth - sx < k) k = uiWidth - sx;

                        sx += k;
                        iPixelsOffset = y * lpDstSurface.pitch;

                        for (; k != 0; k--)
                        {
                            b = (BYTE)(lpBitmapRLE[iRleOffset + j] & 0x0F);

                            if ((INT)b + iColorShift > 0x0F)
                            {
                                b = 0x0F;
                            }
                            else if ((INT)b + iColorShift < 0)
                            {
                                b = 0;
                            }
                            else
                            {
                                b = (BYTE)(b + iColorShift);
                            }

                            lpDstSurface.pixels[iPixelsOffset + x] = (BYTE)(b | bColor);
                            j++;
                            x++;
                        }

                        if (sx >= uiWidth)
                        {
                            sx -= uiWidth;
                            x = (INT)(x - uiWidth);
                            y++;

                            if (y >= lpDstSurface.h)
                            {
                                goto end; // No more pixels needed, break out
                            }
                        }
                    }

                    iRleOffset += T;
                    i += T;
                    uiSrcX += T;

                    while (uiSrcX >= uiWidth)
                    {
                        uiSrcX -= uiWidth;
                        dy++;
                    }
                }
            }

        end:
            //
            // Success
            //
            return 0;
        }

        public static INT
        PAL_FBPBlitToSurface(
           ref BYTE[]       lpBitmapFBP,
           Surface          lpDstSurface
        )
        /*++
          Purpose:

            Blit an uncompressed bitmap in FBP.MKF to an SDL surface.
            NOTE: Assume the surface is already locked, and the surface is a 8-bit 320x200 one.

          Parameters:

            [IN]  lpBitmapFBP - pointer to the RLE-compressed bitmap to be decoded.

            [OUT] lpDstSurface - pointer to the destination SDL surface.

          Return value:

            0 = success, -1 = error.

        --*/
        {
            INT     x, y, iPixOffset = 0, iBmpOffset = 0;

            if (lpBitmapFBP == null   || lpDstSurface == null ||
                lpDstSurface.w != 320 || lpDstSurface.h != 200)
            {
                return -1;
            }

            //
            // simply copy everything to the surface
            //
            for (y = 0; y < 200; y++)
            {
                iPixOffset = y * lpDstSurface.pitch;
                for (x = 0; x < 320; x++)
                {
                    lpDstSurface.pixels[iPixOffset++] = lpBitmapFBP[iBmpOffset++];
                }
            }

            return 0;
        }

        public static INT
        PAL_RLEGetWidth(
            BYTE[]       lpBitmapRLE
        )
        /*++
          Purpose:

            Get the width of an RLE-compressed bitmap.

          Parameters:

            [IN]  lpBitmapRLE - pointer to an RLE-compressed bitmap.

          Return value:

            Integer value which indicates the height of the bitmap.

        --*/
        {
            int iRleOffset = 0;

            if (lpBitmapRLE == null)
            {
                return 0;
            }

            //
            // Skip the 0x00000002 in the file header.
            //
            if (lpBitmapRLE[0] == 0x02 && lpBitmapRLE[1] == 0x00 &&
               lpBitmapRLE[2] == 0x00 && lpBitmapRLE[3] == 0x00)
            {
                iRleOffset += 4;
            }

            //
            // Return the width of the bitmap.
            //
            return lpBitmapRLE[iRleOffset] | (lpBitmapRLE[iRleOffset + 1] << 8);
        }

        public static INT
        PAL_RLEGetHeight(
            BYTE[]       lpBitmapRLE
        )
        /*++
          Purpose:

            Get the width of an RLE-compressed bitmap.

          Parameters:

            [IN]  lpBitmapRLE - pointer to an RLE-compressed bitmap.

          Return value:

            Integer value which indicates the height of the bitmap.

        --*/
        {
            int iRleOffset = 2;

            if (lpBitmapRLE == null)
            {
                return 0;
            }

            //
            // Skip the 0x00000002 in the file header.
            //
            if (lpBitmapRLE[0] == 0x02 && lpBitmapRLE[1] == 0x00 &&
               lpBitmapRLE[2] == 0x00 && lpBitmapRLE[3] == 0x00)
            {
                iRleOffset += 4;
            }

            //
            // Return the width of the bitmap.
            //
            return lpBitmapRLE[iRleOffset] | (lpBitmapRLE[iRleOffset + 1] << 8);
        }

        public static BYTE[]
        PAL_MKFSpriteGetFrame(
           ref BYTE[]       lpBuffer,
           UINT             uiChunkNum
        )
        /*++
          Purpose:

            Get the size of a chunk in an MKF archive.

          Parameters:

            [IN]  lpBuffer - pointer to the destination buffer.

            [IN]  uiChunkNum - the number of the chunk in the MKF archive.

          Return value:

            Integer value which indicates the size of the chunk.
            -1 if the chunk does not exist.

        --*/
        {
            INT  iByteOffset, iOffset, iNextOffset;

            //
            // Check whether the uiChunkNum is out of range..
            //
            if (uiChunkNum >= PAL_MKFGetChunkCount(lpBuffer)) return null;

            //
            // Get the offset of the specified chunk and the next chunk.
            //
            iByteOffset = (INT)uiChunkNum;
            //iOffset     = BitConverter.ToInt32(lpBuffer[iByteOffset..(iByteOffset += 4)]);
            //iNextOffset = BitConverter.ToInt32(lpBuffer[iByteOffset..(iByteOffset +  4)]);
            iOffset     = BitConverter.ToInt32(UTIL_SubBytes(lpBuffer, ref iByteOffset, 4), 0);
            iNextOffset = BitConverter.ToInt32(UTIL_SubBytes(lpBuffer, iByteOffset, 4), 0);

            //
            // Return the length of the chunk.
            //
            //return lpBuffer[iOffset..iNextOffset];
            return UTIL_SubBytes(lpBuffer, iOffset, iNextOffset - iOffset);
        }

        public static WORD
        PAL_SpriteGetNumFrames(
            BYTE[]          lpSprite
        )
        /*++
          Purpose:

            Get the total number of frames of a sprite.

          Parameters:

            [IN]  lpSprite - pointer to the sprite.

          Return value:

            Number of frames of the sprite.

        --*/
        {
            if (lpSprite == null) return 0;

            return (WORD)((lpSprite[0] | (lpSprite[1] << 8)) - 1);
        }

        public static BYTE[]
        PAL_SpriteGetFrame(
           BYTE[]           lpSprite,
           INT              iFrameNum
        )
        /*++
          Purpose:

            Get the pointer to the specified frame from a sprite.

          Parameters:

            [IN]  lpSprite - pointer to the sprite.

            [IN]  iFrameNum - number of the frame.

          Return value:

            Pointer to the specified frame. NULL if the frame does not exist.

        --*/
        {
            INT imagecount, iSMKFSize, iByteOffset, iOffset, iNextOffset;

            iSMKFSize = lpSprite.Length;

            if (lpSprite == null) return null;

            //
            // Hack for broken sprites like the Bloody-Mouth Bug
            //
            //   imagecount = (lpSprite[0] | (lpSprite[1] << 8)) - 1;
            //imagecount = lpSprite[0] | (lpSprite[1] << 8);
            imagecount = PAL_SpriteGetNumFrames(lpSprite);

            //
            // The frame does not exist
            //
            if (iFrameNum < 0 || iFrameNum >= (imagecount + 1)) return null;

            //
            // Get the offset of the frame
            //
            iByteOffset   = iFrameNum << 1;
            iOffset       = (lpSprite[iByteOffset++] | (lpSprite[iByteOffset++] << 8)) << 1;
            iNextOffset   = (lpSprite[iByteOffset++] | (lpSprite[iByteOffset++] << 8)) << 1;

            if (iOffset     == 0x18444) iOffset = (WORD)iOffset;
            if (iOffset     == 0) return null;
            if (iNextOffset == 0 || iFrameNum == imagecount || iNextOffset > iSMKFSize || iNextOffset < iOffset) iNextOffset = lpSprite.Length;

            //return lpSprite[iOffset..iNextOffset];
            return UTIL_SubBytes(lpSprite, iOffset, iNextOffset - iOffset);
        }

        public static INT
        PAL_MKFGetChunkCount(
            BYTE[]          lpFileBuf
        )
        /*++
          Purpose:

            Get the number of chunks in an MKF archive.

          Parameters:

            [IN]  lpFileBuf - pointer to an fopen'ed MKF file.

          Return value:

            Integer value which indicates the number of chunks in the specified MKF file.

        --*/
        {
            if (lpFileBuf == null) return -1;

            //return (BitConverter.ToInt32(lpFileBuf[0..4]) >> 2) - 1;
            return (BitConverter.ToInt32(UTIL_SubBytes(lpFileBuf, 0, 4), 0) >> 2) - 1;
        }

        public static INT
        PAL_MKFGetChunkSize(
           INT              iChunkNum,
           BYTE[]           lpFileBuf
        )
        /*++
          Purpose:

            Get the size of a chunk in an MKF archive.

          Parameters:

            [IN]  iChunkNum - the number of the chunk in the MKF archive.

            [IN]  lpFileBuf - pointer to the fopen'ed MKF file.

          Return value:

            Integer value which indicates the size of the chunk.
            -1 if the chunk does not exist.

        --*/
        {
            INT iByteOffset, iOffset, iNextOffset;

            //
            // Get the total number of chunks.
            //
            if (iChunkNum >= PAL_MKFGetChunkCount(lpFileBuf)) return -1;

            //
            // Get the offset of the specified chunk and the next chunk.
            //
            iByteOffset = iChunkNum << 2;
            //iOffset     = BitConverter.ToInt32(lpFileBuf[iByteOffset..(iByteOffset += 4)]);
            //iNextOffset = BitConverter.ToInt32(lpFileBuf[iByteOffset..(iByteOffset +  4)]);
            iOffset     = BitConverter.ToInt32(UTIL_SubBytes(lpFileBuf, ref iByteOffset, 4), 0);
            iNextOffset = BitConverter.ToInt32(UTIL_SubBytes(lpFileBuf, iByteOffset, 4), 0);

            //
            // Return the length of the chunk.
            //
            return iNextOffset - iOffset;
        }

        public static INT
        PAL_MKFReadChunk(
        ref BYTE[]          lpBuffer,
            INT             iChunkNum,
            BYTE[]          lpFileBuf
        )
        /*++
          Purpose:

            Read a chunk from an MKF archive into lpBuffer.

          Parameters:

            [OUT] lpBuffer - pointer to the destination buffer.

            [IN]  iChunkNum - the number of the chunk in the MKF archive to read.

            [IN]  lpFileBuf - pointer to the fopen'ed MKF file.

          Return value:

            Integer value which indicates the size of the chunk.
            -1 if there are error in parameters.

        --*/
        {
            INT iSize, iByteOffset, iOffset, iNextOffset;

            if (lpFileBuf == null) return -1;

            //
            // Get the total number of chunks.
            //
            iSize = PAL_MKFGetChunkCount(lpFileBuf);
            if (iChunkNum >= iSize) return -1;

            //
            // Get the offset of the chunk.
            //
            iByteOffset = iChunkNum << 2;
            //iOffset     = BitConverter.ToInt32(lpFileBuf[iByteOffset..(iByteOffset += 4)]);
            //iNextOffset = BitConverter.ToInt32(lpFileBuf[iByteOffset..(iByteOffset +  4)]);
            iOffset     = BitConverter.ToInt32(UTIL_SubBytes(lpFileBuf, ref iByteOffset, 4), 0);
            iNextOffset = BitConverter.ToInt32(UTIL_SubBytes(lpFileBuf,     iByteOffset, 4), 0);

            //
            // Copy Array......
            //
            iSize       = iNextOffset - iOffset;
            //lpBuffer    = lpFileBuf[iOffset..iNextOffset];
            lpBuffer    = UTIL_SubBytes(lpFileBuf, iOffset, iSize);

            //
            // Return the length of the chunk.
            //
            return iSize;
        }

        public static INT
        PAL_MKFGetDecompressedSize(
            INT              iChunkNum,
            BYTE[]           lpFileBuf
        )
        /*++
          Purpose:

            Get the decompressed size of a compressed chunk in an MKF archive.

          Parameters:

            [IN]  iChunkNum - the number of the chunk in the MKF archive.

            [IN]  lpFileBuf - pointer to the fopen'ed MKF file.

          Return value:

            Integer value which indicates the size of the chunk.
            -1 if the chunk does not exist.

        --*/
        {
            DWORD[] buf = new DWORD[6];
            INT iOffset, iByteOffset;

            if (lpFileBuf == null) return -1;

            //
            // Get the total number of chunks.
            //
            if (iChunkNum >= PAL_MKFGetChunkCount(lpFileBuf)) return -1;

            //
            // Get the offset of the chunk.
            //
            iByteOffset = iChunkNum << 2;
            //iOffset     = BitConverter.ToInt32(lpFileBuf[iByteOffset..(iByteOffset += 4)]);
            //buf[0]      = BitConverter.ToUInt32(lpFileBuf[iOffset..(iOffset += 4)]);
            iOffset     = BitConverter.ToInt32( UTIL_SubBytes(lpFileBuf, ref iByteOffset, 4), 0);
            buf[0]      = BitConverter.ToUInt32(UTIL_SubBytes(lpFileBuf, ref iOffset,     4), 0);

            //
            // Read the header.
            //
            if (Pal_Cfg.fIsWIN95)
            {
                return (INT)buf[0];
            }
            else
            {
                //buf[1] = BitConverter.ToUInt32(lpFileBuf[iOffset..(iOffset += 4)]);
                buf[1] = BitConverter.ToUInt32(UTIL_SubBytes(lpFileBuf, iOffset, 4), 0);

                return (buf[0] != 0x315f4a59) ? -1 : (INT)buf[1];
            }
        }

        public static INT
        PAL_MKFDecompressChunk(
        ref BYTE[]          lpBuffer,
            INT             iChunkNum,
            BYTE[]          lpFileBuf
        )
        /*++
          Purpose:

            Decompress a compressed chunk from an MKF archive into lpBuffer.

          Parameters:

            [OUT] lpBuffer - pointer to the destination buffer.

            [IN]  iChunkNum - the number of the chunk in the MKF archive to read.

            [IN]  lpFileBuf - pointer to the fopen'ed MKF file.

          Return value:

            Integer value which indicates the size of the chunk.
            -1 if there are error in parameters, or buffer size is not enough.

        --*/
        {
            BYTE[]  buf;
            int     len;

            len = PAL_MKFGetChunkSize(iChunkNum, lpFileBuf);

            if (len <= 0) return len;

            buf = new BYTE[len];

            if (PAL_MKFReadChunk(ref buf, iChunkNum, lpFileBuf) == -1) return -1;

            if (lpBuffer == null) lpBuffer = new BYTE[PAL_MKFGetDecompressedSize(iChunkNum, lpFileBuf)];

            len = Decompress(ref buf, ref lpBuffer);

            return len;
        }

        public static void Rle_Main(
            LPSTR[]     args
        )
        {
            LPSTR       lpszRleName, lpszPatName, lpszOutName;
            BYTE[]      binRle, binPat, binColors;

            //WORD[]*     usRle, usPat, usColors;
            INT         i, j, iWidth, iHeight;
            BOOL        fNight = FALSE;

            lpszRleName = @"F:\TEMP\ABC10.RLE";
            lpszPatName = @"F:\TEMP\PAT0.PAT";
            lpszOutName = @"F:\TEMP\ABC10.PIX";

            //
            // 读取文件二进制内容到字节数组
            //
            binRle = File.ReadAllBytes(lpszRleName);
            binPat = File.ReadAllBytes(lpszPatName);

            //
            // 获取图像宽度
            //
            iWidth  = PAL_RLEGetWidth(binRle);
            iHeight = PAL_RLEGetHeight(binRle);

            //
            // 初始化数组
            //
            binColors = new BYTE[iWidth * iHeight];

            //
            // 使用 WORD 指针指向字节数据，以便读取
            //
            //usRle    = (ushort[]*)(ushort*)&binRle;
            //usPat    = (ushort[]*)(ushort*)&binPat;
            //usColors = (ushort[]*)(ushort*)&binColors;

            //
            // 初始化 Surface
            //
            Pal_Video   palVideo    = new Pal_Video(iWidth, iHeight);
            Surface     bufSprite   = palVideo._Surface;

            //
            // 初始化 Surface 的调色板
            //
            for (i = 0; i < bufSprite.palette.Length; i++)
            {
                j = (fNight ? 256 * 3 : 0) + i * 3;

                bufSprite.palette[i].red  = (BYTE)(binPat[j]     << 2);
                bufSprite.palette[i].gree = (BYTE)(binPat[j + 1] << 2);
                bufSprite.palette[i].blue = (BYTE)(binPat[j + 2] << 2);
            }

            //
            // 解码像素索引到 Surface
            //
            //PAL_RLEBlitToSurface(ref binRle, ref bufSprite, PAL_XY(0, 0));
            //PAL_RLEBlitWithColorShift(ref binRle, ref bufSprite, PAL_XY(0, 0), 0);
            PAL_RLEBlitMonoColor(ref binRle, bufSprite, PAL_XY(0, 0), 0, 0);

            //
            // 根据像素索引分配实际颜色值到 Surface
            //
            for (i = 0; i < bufSprite.pixels.Length; i++)
            {
                j = i * 3;

                RGB rgbPalette = bufSprite.palette[bufSprite.pixels[i]];

                bufSprite.colors[j++] = rgbPalette.red;
                bufSprite.colors[j++] = rgbPalette.gree;
                bufSprite.colors[j++] = rgbPalette.blue;
            }

            //
            // 输出像素点到文件
            //
            File.WriteAllBytes(lpszOutName, bufSprite.colors);
        }
    }
}
