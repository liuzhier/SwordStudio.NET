using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using BOOL	    = System.Boolean;
using CHAR	    = System.Char;
using BYTE	    = System.Byte;
using SHORT	    = System.Int16;
using WORD	    = System.UInt16;
using INT	    = System.Int32;
using UINT      = System.UInt32;
using SDWORD    = System.Int64;
using DWORD	    = System.UInt64;
using LPSTR	    = System.String;

using SwordStudio.NET;
using PalGlobal;
using PalConfig;
using PalCfg;

using static PalUtil.Pal_Util;
using static PalGlobal.Pal_Global;
using static PalCfg.Pal_Cfg;
using static PalConfig.Pal_Config;
using System.IO;

namespace PalMain
{
    public class Pal_Main
    {
        public static void
        main(
            string[]    args
        )
        {
            //
            // Process Parameters
            //
            UTIL_ProcessParameters(args);

            //
            // Open Configuration File
            //
            if (File.Exists(lpszCfgName))
            {
                //
                // The configuration file already exists,
                // starting to load local configuration
                //
                lpszCfgUser = File.ReadAllLines(lpszCfgName);
            }
            else
            {
                //
                // The configuration file does not exist,
                // load default configuration
                //
                lpszCfgUser = lpszCfgDefault;
            }

            //
            // Load Configuration
            //
            Pal_Cfg_LoadConfig(lpszCfgUser);

            //
            // Reading resource files
            //
            PAL_InitGlobals();

            //
            // Check game resource file version
            //
            Pal_Cfg.fIsWIN95 = PAL_IsWINVersion();

            //
            // Load custom Tabpage
            //
            PAL_LoadTabPage();

            return;
        }
    }
}
