using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

using SwordStudio.NET.Properties;
using PalGlobal;
using PalMap;

using static PalGlobal.Pal_Global;
using static PalCfg.Pal_Cfg;
using static PalConfig.Pal_Config;
using static PalCommon.Pal_Common;

namespace PalRes
{
    public class Pal_Res
    {
        public class RESOURCES
        {
            public BYTE         bLoadFlags;

            public Pal_Map      lpMap;                                      // current loaded map
            public BYTE[,]      lppEventObjectSprites;                      // event object sprites
            public INT          nEventObject;                               // number of event objects

            public BYTE[]       lpPlayerSprite;                             // player sprites
        }

        public static RESOURCES gpResources = new RESOURCES();

        public static BYTE[]
        PAL_GetEventObjectSprite(
            WORD             wEventObjectID
        )
        /*++
          Purpose:

            Get the sprite of the specified event object.

          Parameters:

            [IN]  wEventObjectID - the ID of event object.

          Return value:

            Pointer to the sprite.

        --*/
        {
            BYTE[]         binEventSprite = null;

            if (PAL_MKFDecompressChunk(ref binEventSprite, wEventObjectID, Pal_File.Pal_File_GetFile(lpszEventBMP).bufFile) == -1) return null;

            return binEventSprite;
        }

    }
}
