using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Text.RegularExpressions;
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

using static PalGlobal.Pal_Global;
using PalGlobal;

namespace PalCfg
{
    public enum Pal_VERSION
    {
        AUTO    = (1 << 0),
        DOS     = (1 << 1),
        WIN     = (1 << 2),
        UNION   = (1 << 3),
        HACK    = (1 << 4),
    }

    public class PalCfgNodeItem
    {
        public Pal_VERSION  Version = Pal_VERSION.UNION;        // Some nodes are only loaded in the WIN version
        public LPSTR        lpszType;               // Node Type
        public LPSTR        lpszNodeName;           // Object Name / Node Name
        public LPSTR        lpszTitle;              // Title

        public LPSTR        lpszFileName;           // File Name
        public LPSTR[]      lpszArchiveMethod;      // Archive Method
        public LPSTR        lpszTextEncoding;       // Text Encoding

        public LPSTR        lpszCommandID;          // Command ID
        public LPSTR        lpszScriptTitle;        // Script Title
        public LPSTR        lpszParameterTitle_1;   // Parameter 1 Title
        public LPSTR        lpszParameterTitle_2;   // Parameter 2 Title
        public LPSTR        lpszParameterTitle_3;   // Parameter 3 Title
        public LPSTR        lpszScriptComments;     // Script Comments
        
        public List<INT>    iRangeList;             // Data Area Range

        public PalCfgNode   pcnChildList;           // Child Node List
    }

    public class PalCfgNode
    {
        public LPSTR                lpszNodeName    = null;
        public List<PalCfgNodeItem> pcniItem        = new List<PalCfgNodeItem>();
    }

    public class Pal_Cfg
    {
        public static List<PalCfgNode>  pcnRootList     = new List<PalCfgNode>();    // Root node list
        public static Pal_VERSION       Version         = Pal_VERSION.AUTO;
        public static BOOL              fIsWIN95        = FALSE;
        public static readonly LPSTR    lpszOMIT        = "OMIT";

        public static void
        Pal_Cfg_LoadConfig(
            LPSTR[]         lpszCfg
        )
        {
            INT             i, j;
            LPSTR[]         lines, lpszParameterTable;
            LPSTR           lpszThisLine, lpszNodeName = null, lpszTagName;
            PalCfgNode      pcnThisNode         = null;
            PalCfgNodeItem  pcniThisNodeItem    = null;
            BOOL            fIsBegin = FALSE, fIsSystemTag = FALSE;
            Match           match;

            //
            // Check if the file has been read completely
            //
            if (lpszCfg == null) return;

            foreach (LPSTR line in lpszCfg)
            {
                //
                // Remove whitespace characters at the beginning and end
                //
                lpszThisLine = line.Trim();

                //
                // Check if the file has been read completely
                //
                if (String.IsNullOrEmpty(lpszThisLine)) continue;

                //
                // Skip annotations......
                //
                if (lpszThisLine[0] == '#') continue;

                lpszTagName = (!fIsBegin) ? "BEGIN" : "END";

                //
                // Retrieve "BEGIN" or "END" tags
                //
                match = Regex.Match(lpszThisLine, $@"\[{lpszTagName}\s*(.*?)\]");

                if (match.Success)
                {
                    //
                    // This line is a tag / node
                    //
                    fIsBegin = !fIsBegin;

                    lpszNodeName = match.Groups[1].Value;

                    if (fIsBegin)
                    {
                        //
                        // Found "BEGIN" tag
                        //
                        pcnThisNode = new PalCfgNode();
                        pcnThisNode.lpszNodeName = lpszNodeName;
                    }
                    else
                    {
                        //
                        // Found "END" tag
                        //
                        if (pcnThisNode.Equals(lpszNodeName)) throw new Exception("The 'END' tag name is incorrect!");

                        if (!fIsSystemTag) pcnRootList.Add(pcnThisNode);

                        fIsSystemTag = FALSE;
                    }

                    //
                    // Skip directly
                    //
                    continue;
                }
                else
                {
                    //
                    // This line is an item
                    //
                    if (!fIsBegin) continue;

                    //lpszParameterTable  = lpszThisLine.Split('\t', StringSplitOptions.None);
                    lpszParameterTable  = lpszThisLine.Split('\t');
                    pcniThisNodeItem    = new PalCfgNodeItem();

                    //
                    // Read item content, case insensitive
                    //
                    for (i = 0; i < lpszParameterTable.Length;)
                    {
                        if (String.IsNullOrEmpty(lpszParameterTable[i]))    break;
                        if (lpszParameterTable[i][0].Equals('#'))           break;

                        //
                        // Check item version
                        //
                        switch (lpszParameterTable[i++])
                        {
                            case "DOS":
                                {
                                    pcniThisNodeItem.Version = Pal_VERSION.DOS;
                                }
                                break;

                            case "WIN":
                                {
                                    pcniThisNodeItem.Version = Pal_VERSION.WIN;
                                }
                                break;

                            case "HACK":
                                {
                                    pcniThisNodeItem.Version = Pal_VERSION.HACK;
                                }
                                break;

                            default:
                                {
                                    pcniThisNodeItem.Version = Pal_VERSION.UNION;
                                    i--;
                                }
                                break;
                        }

                        //
                        // Check system nodes
                        //
                        switch (lpszNodeName)
                        {
                            case "SETTING":
                                {
                                    fIsSystemTag = TRUE;

                                    switch (lpszParameterTable[i++])
                                    {
                                        case "VERSION":
                                            {
                                                switch (lpszParameterTable[i++])
                                                {
                                                    case "DOS":
                                                        {
                                                            Version = Pal_VERSION.DOS;
                                                        }
                                                        break;

                                                    case "WIN":
                                                        {
                                                            Version = Pal_VERSION.WIN;
                                                        }
                                                        break;
                                                    
                                                    case "AUTO":
                                                    default:
                                                        {
                                                            Version = Pal_VERSION.AUTO;
                                                        }
                                                        break;
                                                }
                                            }
                                            break;

                                        default:
                                            {

                                            }
                                            break;
                                    }
                                }
                                break;

                            case "FILE":
                                {
                                    pcniThisNodeItem.lpszType           = lpszParameterTable[i++];
                                    pcniThisNodeItem.lpszNodeName       = lpszParameterTable[i++];
                                    pcniThisNodeItem.lpszFileName       = lpszParameterTable[i++];
                                    pcniThisNodeItem.lpszArchiveMethod  = lpszParameterTable[i++].Split('|').Select(item => item.Trim()).ToArray();

                                    if (pcniThisNodeItem.lpszArchiveMethod.Last().Contains(':'))
                                    {
                                        //
                                        // Use the encoding specified by the last ':' symbol
                                        //
                                        foreach (LPSTR lpszThis in pcniThisNodeItem.lpszArchiveMethod.Last().Split(':'))
                                        {
                                            pcniThisNodeItem.lpszTextEncoding = lpszThis.Trim();
                                        }
                                    }
                                }
                                break;

                            case "TAB_MAIN":
                                {
                                    pcniThisNodeItem.lpszTitle  = lpszParameterTable[i++];
                                }
                                break;

                            case "SCRIPT_DESC":
                                {
                                    pcniThisNodeItem.lpszCommandID          = lpszParameterTable[i++];
                                    pcniThisNodeItem.lpszScriptTitle        = lpszParameterTable[i++];
                                    pcniThisNodeItem.lpszParameterTitle_1   = lpszParameterTable[i++];
                                    pcniThisNodeItem.lpszParameterTitle_2   = lpszParameterTable[i++];
                                    pcniThisNodeItem.lpszParameterTitle_3   = lpszParameterTable[i++];
                                    pcniThisNodeItem.lpszScriptComments     = lpszParameterTable[i++];
                                }
                                break;

                            default:
                                {
                                    //
                                    // User defined configuration
                                    //
                                    pcniThisNodeItem.lpszType       = lpszParameterTable[i++];
                                    pcniThisNodeItem.lpszNodeName   = lpszParameterTable[i++];
                                    pcniThisNodeItem.lpszTitle      = lpszParameterTable[i++];

                                    switch (pcniThisNodeItem.lpszType)
                                    {
                                        case "UNION":
                                            {
                                                INT         start, end;
                                                LPSTR       input   = lpszParameterTable[i++];
                                                const LPSTR pattern = @"(\d+)\s*~\s*(\d+)";

                                                if (input.Equals(lpszOMIT)) break;

                                                pcniThisNodeItem.iRangeList = new List<INT>();

                                                match = Regex.Match(input, pattern);
                                                if (match.Success)
                                                {
                                                    pcniThisNodeItem.iRangeList.Add(int.Parse(match.Groups[1].Value));
                                                    pcniThisNodeItem.iRangeList.Add(int.Parse(match.Groups[2].Value));
                                                }

                                                LPSTR[] lpszRangeList = input.Split('|');
                                                for (j = 0; j < lpszRangeList.Length; j++)
                                                {
                                                    lpszRangeList[j] = lpszRangeList[j].Trim();

                                                    if (!Regex.IsMatch(lpszRangeList[j], @"^\d+$")) continue;

                                                    pcniThisNodeItem.iRangeList.Add(-int.Parse(lpszRangeList[j]));
                                                }
                                            }
                                            break;

                                        case "TAB":
                                        case "CHUNK":
                                        case "CHAR":
                                        case "BYTE":
                                        case "SHORT":
                                        case "WORD":
                                        case "INT":
                                        case "UINT":
                                        case "SDWORD":
                                        case "DWORD":
                                        case "SQWORD":
                                        case "QWORD":
                                        default:
                                            {

                                            }
                                            break;
                                    }
                                }
                                break;
                        }
                    }

                    //
                    // Add item to node
                    //
                    if (!fIsSystemTag) pcnThisNode.pcniItem.Add(pcniThisNodeItem);
                }
            }
        }

        public static PalCfgNode
        Pal_Cfg_GetCfgNode(
            LPSTR       _lpszNodeName
        )
        {
            return Pal_Cfg.pcnRootList.Where(node => node.lpszNodeName.Equals(_lpszNodeName)).First();
        }
    }
}
