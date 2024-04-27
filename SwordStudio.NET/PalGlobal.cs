using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Resources;
using System.Text.RegularExpressions;
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
using FILE      = System.IO.File;

using SwordStudio.NET;
using SwordStudio.NET.Properties;
using PalCfg;
using PalMain;
using PalVideo;

using static PalGlobal.Pal_File;
using static PalUtil.Pal_Util;
using static PalCfg.Pal_Cfg;
using static PalCommon.Pal_Common;

namespace PalGlobal
{
    public class Pal_File
    {
        public LPSTR    lpszNodeName        = null;
        public LPSTR    lpszFileName        = null;
        public BYTE[]   bufFile             = null;
        public LPSTR[]  lpszFileText        = null;
        public LPSTR[]  lpszArchiveMethod   = null;
        public BYTE[]   bufTmp              = null;

        public static Pal_File
        Pal_File_GetFile(
            LPSTR _lpszNodeName
        )
        {
            return Pal_Global.pfFileList.Where(file => file.lpszNodeName.Equals(_lpszNodeName)).First();
        }
    }

    public class Pal_Global
    {
        public const BOOL   TRUE  = true;
        public const BOOL   FALSE = false;
        
        public static readonly Encoding GB2312  = Encoding.GetEncoding("GB2312");
        public static readonly CHAR     PathDSC = (Environment.OSVersion.Platform == PlatformID.Win32NT) ? '\\' : '/';
        public static readonly LPSTR    NewLine = Environment.NewLine;

        //public static readonly LPSTR    lpszCfgName = "SwordStudio.NET.ini";
        public static readonly LPSTR    lpszCfgName     = $"F:{PathDSC}liuzhier{PathDSC}SwordStudio.NET{PathDSC}docs{PathDSC}SwordStudio.NET.ini.example";
        public static readonly LPSTR    lpszGaemPath    = $"F:{PathDSC}PALDOS{PathDSC}pal";

        public const LPSTR  lpszSetting     = "SETTING";
        public const LPSTR  lpszFile        = "FILE";
        public const LPSTR  lpszMainData    = "MainData";
        public const LPSTR  lpszEvent       = "Event";
        public const LPSTR  lpszScene       = "Scene";
        public const LPSTR  lpszUnit        = "Unit";
        public const LPSTR  lpszUnitSystem  = "UNIT_System";
        public const LPSTR  lpszUnion       = "UNION";
        public const LPSTR  lpszSciptDesc   = "SCRIPT_DESC";
        public const LPSTR  lpszSceneDesc   = "SCENE_DESC";

        public const LPSTR  lpszEnemyBMP    = "EnemyBMP";
        public const LPSTR  lpszGameMapTile = "GameMapTile";
        public const LPSTR  lpszGameMap     = "GameMap";
        public const LPSTR  lpszPalette     = "Palette";

        public const LPSTR  lpszMapID       = "MapID";

        public const LPSTR  lpszEventObjectIndex = "EventObjectIndex";

        public static TabControl        tcMainTabCtrl   = new TabControl();
        public static BOOL              fIsRegEncode    = FALSE;
        public static List<Pal_File>    pfFileList      = new List<Pal_File>();
        public static INT               iThisScene      = -1;

        public        Pal_Video         pvMapEdit       = null;

        public static BOOL
        PAL_IsWINVersion()
        {
            BOOL                    fIsWIN95;
            INT                     iSize, data_size;
            Pal_File                pfFileTmp;
            BYTE[]                  bufTmp;
            PalCfgNode              pcnTmp;

            fIsWIN95 = Pal_Cfg.Version == Pal_VERSION.WIN;

            //
            // Unpacking files data
            //
            {
                //pfFileTmp = Pal_Global.pfFileList.Where(file => file.lpszNodeName.Equals(lpszMainData)).First();
                pfFileTmp   = Pal_File_GetFile(lpszMainData);

                //
                // Extract sub data "Unit"
                //
                pcnTmp      = Pal_Cfg.Pal_Cfg_GetCfgNode(lpszMainData);
                iSize       = pcnTmp.pcniItems.IndexOf(pcnTmp.pcniItems.Where(item => item.lpszNodeName.Equals(lpszUnit)).First());
                data_size   = PAL_MKFGetChunkSize(iSize, pfFileTmp.bufFile);
                bufTmp      = new BYTE[data_size];
                PAL_MKFReadChunk(ref bufTmp, iSize, ref pfFileTmp.bufFile);
            }

            //
            // Get the size of the chunk "Unit"
            //
            iSize = Pal_Cfg_GetChunkSize(lpszUnit);

            //
            // Check if the file format is correct
            //
            if (!(data_size % iSize == 0))
            {
                throw new Exception("Failed: The main DATA file format is incorrect.");
            }

            return fIsWIN95;
        }

        public static void
        PAL_InitGlobals()
        /*++
          Purpose:

            Initialize global data.

          Parameters:

            None.

          Return value:

            None.

        --*/
        {
            List<PalCfgNodeItem>    pcniNodeItemList;
            Pal_File                pfThisFile;
            LPSTR                   lpszPath;

            //
            // Get the number of files
            //
            pcniNodeItemList = Pal_Cfg.pcnRootList.Where(node => node.lpszNodeName == "FILE").First().pcniItems;

            //
            // Register text encoding set
            //
            UTIL_RegEncode();

            //
            // Start reading file binary
            //
            foreach (PalCfgNodeItem pcniThis in pcniNodeItemList)
            {
                lpszPath = UTIL_GetFileCompletePath(pcniThis.lpszFileName);

                //
                // Skip non-existent files
                //
                if (!FILE.Exists(lpszPath)) continue;

                pfThisFile              = new Pal_File();
                pfThisFile.lpszNodeName = pcniThis.lpszNodeName;
                pfThisFile.lpszFileName = pcniThis.lpszFileName;

                if (pcniThis.Version == Pal_VERSION.HACK)
                    pfThisFile.lpszFileText     = FILE.ReadAllLines(lpszPath, Encoding.GetEncoding(pcniThis.lpszTextEncoding));
                else
                    pfThisFile.bufFile          = FILE.ReadAllBytes(lpszPath);

                pfThisFile.lpszArchiveMethod    = pcniThis.lpszArchiveMethod;

                Pal_Global.pfFileList.Add(pfThisFile);
            }
        }

        /*
        public static void
        PAL_LoadTabPage()
        {
            INT             i;
            PalCfgNode      pcnTabMain = Pal_Cfg_GetCfgNode(lpszTabMain);
            PalCfgNode      pcnTabThis;
            TabPage         tpTabPageThis;
            TabControl      tcTabCtrl;
            LPSTR[]         lpszNodeNameList;
            Pal_File        pfFileThis;

            LPSTR           This_TabLabel, This_NodeName;
            PalCfgNodeItem  This_NodeItem;

            //
            // Overwrite tab title
            //
            pcnTabThis = Pal_Cfg_GetCfgNode(lpszTabMain);
            for (i = 0; i < pcnTabThis.pcniItem.Count; i++)
            {
                Pal_Global.lpszTabLabel[i] = pcnTabThis.pcniItem[i].lpszTitle;
            }

            //
            // Load TabPage: Util
            //
            {
                //
                // TabPage First Layer
                // Add TabPage to the first level navigation bar
                //
                This_TabLabel = Pal_Global.lpszTabLabel[0];

                This_NodeItem = pcnTabThis.pcniItem[0];
                This_NodeName = This_NodeItem.lpszNodeName + "_" + pcnTabThis.lpszNodeName;
                tpTabPageThis = UTIL_AddTabPage(This_NodeName, This_TabLabel, Pal_Global.tcMainTabCtrl);

                //
                // Add Subtab Group
                //
                tcTabCtrl = UTIL_AddTabCtrl(tpTabPageThis);

                pcnTabThis = Pal_Cfg_GetCfgNode(lpszUnit);
                for (i = pcnTabThis.pcniItem.Count - 1; i >= 0; i--)
                {
                    //
                    // TabPage second Layer
                    // Add TabPage to the secondary navigation bar
                    //
                    This_NodeItem = pcnTabThis.pcniItem[i];
                    This_NodeName = This_NodeItem.lpszNodeName + "_" + pcnTabThis.lpszNodeName;
                    tpTabPageThis = UTIL_AddTabPage(This_NodeName, This_NodeItem.lpszTitle, tcTabCtrl);
                }

                //
                // Get data area
                //

                //
                // Add table to TabPage
                //
            }

            //for (i = 0; i < Pal_Global.lpszTabLabel.Length; i++)
            {
                //
                // Splitting node links
                //
                lpszNodeNameList = pcniThis_Second.lpszNodeName.Split(".");
                pfFileThis          = Pal_Global.pfFileList.Where(file => file.lpszNodeName.Equals(lpszNodeNameList[0])).First();

                //
                // Locate the corresponding nodes based on the node chain
                //
                LPSTR lpszNodeNameThis = lpszNodeNameList[0];
                PalCfgNode pcnSubNode = Pal_Cfg_GetCfgNode(lpszNodeNameThis);

                pcnTabThis = Pal_Cfg_GetCfgNode(lpszNodeNameThis);

                foreach (LPSTR lpszArchiveMethodThis in pfFileThis.lpszArchiveMethod)
                {
                    //
                    // Layer by layer parsing of data
                    //
                    switch (lpszArchiveMethodThis)
                    {
                        case "MKF":
                            {
                                INT iChunkNum = pcnSubNode.pcniItem.IndexOf(pcnSubNode.pcniItem.Where(subNode => subNode.lpszNodeName.Equals(lpszNodeNameList[1])).First());

                                PAL_MKFReadChunk(ref pfFileThis.bufTmp, iChunkNum, ref pfFileThis.bufFile);
                            }
                            break;

                        case "BIN":
                        case "VOC":
                        case "WAV":
                        default:
                            {

                            }
                            break;
                    }
                }

                //
                // 
                //
                pcnSubNode.pcniItem.Where(subNode => subNode.lpszNodeName.Equals(lpszNodeNameList[1])).First();
            }
        }
        */
    }
}
