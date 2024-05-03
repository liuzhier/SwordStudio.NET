using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Resources;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Security.AccessControl;

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
using FILE      = System.IO.File;

using SwordStudio.NET;
using SwordStudio.NET.Properties;
using PalCfg;
using PalMain;
using PalVideo;
using PalUtil;

using static PalGlobal.Pal_File;
using static PalGlobal.Pal_Global;
using static PalUtil.Pal_Util;
using static PalConfig.Pal_Config;
using static PalCfg.Pal_Cfg;
using static PalCommon.Pal_Common;

namespace PalGlobal
{
    public enum PAL_OBJECT_TYPE
    {
        BOOL    = 0,
        CHAR    = 1,
        BYTE    = 2,

        SHORT   = 3,
        WORD    = 4,

        INT     = 5,
        UINT    = 6,

        SDWORD  = 7,
        DWORD   = 8,

        SQWORD  = 9,
        QWORD   = 10,
    }

    public class Pal_Object
    {
        public LPSTR                    TableName;
        public LPSTR[]                  HeadName;
        public dynamic[,]               Data;
        public PAL_OBJECT_TYPE[,]       Type;

        private Pal_Object() { }

        public  Pal_Object(
            BYTE[]                  binDataAll,
            PalCfgNode              pcnNode,
            PAL_FILE_READMODE       pdrReadMode = PAL_FILE_READMODE.Horizontal
        )
        {
            INT             i, j, iByteOffset = 0, iChunkSize, iChunkCount, iTypeSize, iDataLenght = pcnNode.pcniItems.Count;
            PalCfgNodeItem  pcniItem, pcniChildItem;
            dynamic         data = null;

            TableName   = pcnNode.lpszNodeName;

            iChunkSize  = binDataAll.Length;
            iChunkCount = iChunkSize / Pal_Cfg_GetChunkSize(pcnNode.lpszNodeName);

            HeadName    = new LPSTR[iDataLenght];
            Data        = new dynamic[iChunkCount, iDataLenght];
            Type        = new PAL_OBJECT_TYPE[iChunkCount, iDataLenght];

            if (pcnNode.pcniItems[0].lpszType.Equals(lpszUnion)) pcnNode = Pal_Cfg_GetCfgNode(pcnNode.pcniItems[0].lpszNodeName);

            for (i = 0; i < iChunkCount; i++)
            {
                for (j = 0; j < iDataLenght; j++)
                {
                    pcniItem        = pcnNode.pcniItems[j];
                    iTypeSize       = UTIL_GetTypeSize(pcniItem.lpszType);

                    if (i == 0) HeadName[j] = pcniItem.lpszNodeName;

                    if (pdrReadMode == PAL_FILE_READMODE.Horizontal)
                    {
                        data = BitConverter.ToInt64(UTIL_SubBytes(binDataAll, ref iByteOffset, iTypeSize, SIZE_Of_QWORD), 0);
                    }
                    else
                    {
                        if (j == 0) iByteOffset = 0;

                        iByteOffset += i * iTypeSize;
                        data = BitConverter.ToInt64(UTIL_SubBytes(binDataAll, iByteOffset, iTypeSize, SIZE_Of_QWORD), 0);
                        iByteOffset += iTypeSize * (iChunkCount - i);
                    }

                    if (!pcniItem.lpszNodeName.Equals(lpszNull)) SetItem(i, j, data, pcniItem.lpszType);
                }
            }
        }

        public void
        SetItem(
            INT                 iItemGroupNum,
            INT                 iItemNum,
            dynamic             decData,
            PAL_OBJECT_TYPE     pszType
        )
        {
            Data[iItemGroupNum, iItemNum]   = GetActualData(decData, pszType);
            Type[iItemGroupNum, iItemNum]   = pszType;
        }

        public void
        SetItem(
            INT         iItemGroupNum,
            INT         iItemNum,
            dynamic     decData,
            LPSTR       lpszType
        ) => SetItem(iItemGroupNum, iItemNum, decData, GetObjectType(lpszType));

        public dynamic
        GetActualData(
            dynamic             decValue,
            PAL_OBJECT_TYPE     pszType
        )
        {
            switch (pszType)
            {
                case PAL_OBJECT_TYPE.BOOL:
                case PAL_OBJECT_TYPE.CHAR:
                    {
                        return (CHAR)decValue;
                    }

                case PAL_OBJECT_TYPE.BYTE:
                    {
                        return (BYTE)decValue;
                    }

                case PAL_OBJECT_TYPE.SHORT:
                    {
                        return (SHORT)decValue;
                    }

                case PAL_OBJECT_TYPE.WORD:
                    {
                        return (WORD)decValue;
                    }

                case PAL_OBJECT_TYPE.SDWORD:
                    {
                        return (SDWORD)decValue;
                    }

                case PAL_OBJECT_TYPE.DWORD:
                    {
                        return (DWORD)decValue;
                    }

                case PAL_OBJECT_TYPE.SQWORD:
                    {
                        return (SQWORD)decValue;
                    }

                case PAL_OBJECT_TYPE.QWORD:
                    {
                        return (QWORD)decValue;
                    }

                default:
                    {
                        return (WORD)decValue;
                    }
            }
        }

        public dynamic
        GetActualData(
            dynamic     decValue,
            LPSTR       lpszType
        ) => GetActualData(decValue, GetObjectType(lpszType));

        public static PAL_OBJECT_TYPE
        GetObjectType(
            LPSTR       lpszType
        )
        {
            switch (lpszType)
            {
                case "BOOL":    return PAL_OBJECT_TYPE.BOOL;
                case "CHAR":    return PAL_OBJECT_TYPE.CHAR;
                case "BYTE":    return PAL_OBJECT_TYPE.BYTE;
                case "SHORT":   return PAL_OBJECT_TYPE.SHORT;
                case "WORD":    return PAL_OBJECT_TYPE.WORD;
                case "INT":     return PAL_OBJECT_TYPE.INT;
                case "UINT":    return PAL_OBJECT_TYPE.UINT;
                case "SDWORD":  return PAL_OBJECT_TYPE.SDWORD;
                case "DWORD":   return PAL_OBJECT_TYPE.DWORD;
                case "SQWORD":  return PAL_OBJECT_TYPE.SQWORD;
                case "QWORD":   return PAL_OBJECT_TYPE.QWORD;

                default:        return PAL_OBJECT_TYPE.WORD;
            }
        }

        public dynamic
        GetItem(
            INT         iItemIndex,
            LPSTR       lpszName
        ) => Data[iItemIndex, Array.IndexOf(HeadName, HeadName.Where(Item => Item.Equals(lpszName)).First())];
    }

    public enum PAL_DISPLAY_MODE
    {
        None        = 0,
        LowTile     = 1 << 0,
        HighTile    = 1 << 1,
        NoPassTile  = 1 << 2,
        EventSprite = 1 << 3,
    }

    public enum PAL_FILE_READMODE
    {
        Vertical,
        Horizontal
    }

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

        public static readonly LPSTR    lpszCfgName     = "SwordStudio.NET.ini";
        public static readonly LPSTR    lpszGaemPath    = $".{PathDSC}";
        //public static readonly LPSTR    lpszCfgName     = $"F:{PathDSC}liuzhier{PathDSC}SwordStudio.NET{PathDSC}docs{PathDSC}SwordStudio.NET.ini.example";
        //public static readonly LPSTR    lpszGaemPath    = $"F:{PathDSC}PALDOS{PathDSC}pal";

        public const  INT               SIZE_Of_QWORD       = sizeof(QWORD);

        public static TabControl        tcMainTabCtrl       = new TabControl();
        public static BOOL              fIsRegEncode        = FALSE;
        public static List<Pal_File>    pfFileList          = new List<Pal_File>();
        public static Pal_Object[]      poCoreData;
        public static Pal_Object[]      poMainData;
        public static INT               iThisScene          = -1;
        public static WORD              wScreenWave         = 0;
        public static SHORT             sWaveProgression    = 0;

        public        Pal_Video         pvMapEdit           = null;

        // state of event object, used by the sState field of the EVENTOBJECT struct
        public enum OBJECTSTATE
        {
            Hidden  = 0,
            Normal  = 1,
            Blocker = 2
        }

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
                PAL_MKFReadChunk(ref bufTmp, iSize, pfFileTmp.bufFile);
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
            INT                     i, j, iByteOffset, iChunkSize;
            List<PalCfgNodeItem>    pcniNodeItemList;
            PalCfgNode              pcnNode;
            PalCfgNode              pcnChildNode;
            PalCfgNodeItem          pcniItem;
            PalCfgNodeItem          pcniChildItem;
            Pal_File                pfThisFile;
            LPSTR                   lpszPath;
            BYTE[]                  dataChunk = null, dataCount = null;

            //
            // Get the number of files
            //
            pcniNodeItemList = Pal_Cfg.pcnRootList.Where(node => node.lpszNodeName == lpszFile).First().pcniItems;

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

                if (pcniThis.lpszArchiveMethod[0].Contains(lpszTXT))
                    pfThisFile.lpszFileText     = FILE.ReadAllLines(lpszPath, Encoding.GetEncoding(pcniThis.lpszTextEncoding));
                else
                    pfThisFile.bufFile          = FILE.ReadAllBytes(lpszPath);

                pfThisFile.lpszArchiveMethod    = pcniThis.lpszArchiveMethod;

                Pal_Global.pfFileList.Add(pfThisFile);
            }

            //
            // Initialize core data
            //
            {
                pfThisFile              = Pal_File_GetFile(lpszCoreData);
                pcnNode                 = Pal_Cfg_GetCfgNode(lpszCoreData);
                Pal_Global.poCoreData   = new Pal_Object[pcnNode.pcniItems.Count];

                //
                // Initialize each item
                //
                for (i = 0; i < poCoreData.Length; i++)
                {
                    pcniItem = pcnNode.pcniItems[i];

                    if (pcniItem.lpszNodeName.Equals(lpszNull)) continue;

                    pcnChildNode = Pal_Cfg_GetCfgNode(pcniItem.lpszNodeName);
                    PAL_MKFReadChunk(ref dataChunk, i, pfThisFile.bufFile);

                    switch (i)
                    {
                        case 3:
                        case 13:
                            {
                                Pal_Global.poCoreData[i] = new Pal_Object(dataChunk, pcnChildNode, PAL_FILE_READMODE.Vertical);
                            }
                            break;

                        default:
                            {
                                Pal_Global.poCoreData[i] = new Pal_Object(dataChunk, pcnChildNode);
                            }
                            break;
                    }
                }
            }

            //
            // Initialize main data
            //
            {
                pfThisFile              = Pal_File_GetFile(lpszMainData);
                pcnNode                 = Pal_Cfg_GetCfgNode(lpszMainData);
                Pal_Global.poMainData   = new Pal_Object[pcnNode.pcniItems.Count];

                //
                // Initialize each item
                //
                for (i = 0; i < poMainData.Length; i++)
                {
                    pcniItem        = pcnNode.pcniItems[i];

                    if (pcniItem.lpszNodeName.Equals(lpszNull)) continue;

                    pcnChildNode    = Pal_Cfg_GetCfgNode(pcniItem.lpszNodeName);
                    PAL_MKFReadChunk(ref dataChunk, i, pfThisFile.bufFile);

                    Pal_Global.poMainData[i] = new Pal_Object(dataChunk, pcnChildNode);
                }
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
