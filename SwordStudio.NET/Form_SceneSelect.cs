using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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

using PalGlobal;
using PalCfg;
using PalVideo;
using PalMap;

using static PalGlobal.Pal_Global;
using static PalGlobal.Pal_File;
using static PalCommon.Pal_Common;
using static PalUtil.Pal_Util;
using static PalCfg.Pal_Cfg;
using static PalMap.Pal_Map;
using static PalVideo.Pal_Video;

namespace SwordStudio.NET
{
    public partial class Form_SceneSelect : Form
    {
        private static      INT         nEvent, nScene, iSizeOfScene;
        private static      Pal_File    pfFile_Map = null, pfFile_SSS = null;
        private static      Surface     sfMapPreview        = null;
        private static      BYTE[]      Map_Tmp             = null;
        private static      BOOL        fIsLoadingCompleted = FALSE;
        private static      Rect        rect                = new Rect();
        private const       WORD        wMapWidth           = 2064, wMapHeight = 2055;
        private static readonly PAL_POS     dwMinMapPos         = PAL_XY(0, 0);
        private readonly    PAL_POS     dwMaxMapPos;

        private static      BOOL        fIsMovingMap        = FALSE;
        private static      Point       MouseBeginPos;
        private static      Point        MouseEndPos;

        public              INT         _iThisScene         = -1;
        public              BOOL        _fIsEnter           = FALSE;
        public              PAL_POS     _dwMapPos           = dwMinMapPos;
        public              BYTE[]      _AllSceneData;

        private Form_SceneSelect() { }

        public Form_SceneSelect(
            Form        Father_Form
        )
        {
            InitializeComponent();

            this.Owner                  = Father_Form;

            MapPreview_PictureBox.Image = new Bitmap(MapPreview_PictureBox.Width, MapPreview_PictureBox.Height);
            sfMapPreview                = new Surface(MapPreview_PictureBox.Width, MapPreview_PictureBox.Height);

            dwMaxMapPos                 = PAL_XY((WORD)(wMapHeight - MapPreview_PictureBox.Width), (WORD)(wMapHeight - MapPreview_PictureBox.Height));

            rect.x                      = PAL_X(_dwMapPos);
            rect.y                      = PAL_Y(_dwMapPos);
            rect.w                      = sfMapPreview.w;
            rect.h                      = sfMapPreview.h;
        }

        private void MapPreview_PictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                fIsMovingMap    = true;

                MouseBeginPos   = Cursor.Position;
            }
        }

        private void MapPreview_PictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) fIsMovingMap = false;
        }

        private void MapPreview_PictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            INT     iMaxWidth, iMaxHeight = wMapHeight - MapPreview_PictureBox.Height;

            if (fIsMovingMap)
            {
                MouseEndPos     = Point.Subtract(Cursor.Position, new Size(MouseBeginPos));
                MouseEndPos.X  /= 2;
                MouseEndPos.Y  /= 2;
                MouseEndPos     = Point.Subtract(new Point(PAL_X(_dwMapPos), PAL_Y(_dwMapPos)), new Size(MouseEndPos));

                MouseEndPos.X   = Math.Min(PAL_X(dwMaxMapPos), MouseEndPos.X);
                MouseEndPos.X   = Math.Max(PAL_X(dwMinMapPos), MouseEndPos.X);

                MouseEndPos.Y   = Math.Min(PAL_Y(dwMaxMapPos), MouseEndPos.Y);
                MouseEndPos.Y   = Math.Max(PAL_Y(dwMinMapPos), MouseEndPos.Y);

                _dwMapPos       = PAL_XY((WORD)MouseEndPos.X, (WORD)MouseEndPos.Y);

                ScrollTD_ScrollBoxT_MainBoxL_VScrollBar.Value = (INT)(ScrollTD_ScrollBoxT_MainBoxL_VScrollBar.Maximum * ((double)PAL_X(_dwMapPos) / PAL_X(dwMaxMapPos)));
                ScrollLR_ScrollBoxD_MainBoxL_HScrollBar.Value = (INT)(ScrollLR_ScrollBoxD_MainBoxL_HScrollBar.Maximum * ((double)PAL_Y(_dwMapPos) / PAL_Y(dwMaxMapPos)));

                rect.x          = PAL_X(_dwMapPos);
                rect.y          = PAL_Y(_dwMapPos);
                PAL_DrawMapToSurface(sfMapPreview, rect, MapPreview_PictureBox, 1);
            }
        }

        private void ScrollTD_ScrollBoxT_MainBoxL_VScrollBar_ValueChanged(object sender, EventArgs e)
        {
            double percentage;

            if (fIsMovingMap) return;

            percentage = (double)ScrollTD_ScrollBoxT_MainBoxL_VScrollBar.Value / ScrollTD_ScrollBoxT_MainBoxL_VScrollBar.Maximum;

            _dwMapPos = PAL_XY(PAL_X(_dwMapPos), (WORD)((wMapHeight - MapPreview_PictureBox.Height) * percentage));

            rect.y = PAL_Y(_dwMapPos);
            PAL_DrawMapToSurface(sfMapPreview, rect, MapPreview_PictureBox, 1);
        }

        private void ScrollLR_ScrollBoxD_MainBoxL_HScrollBar_ValueChanged(object sender, EventArgs e)
        {
            double percentage;

            if (fIsMovingMap) return;

            percentage = (double)ScrollLR_ScrollBoxD_MainBoxL_HScrollBar.Value / ScrollLR_ScrollBoxD_MainBoxL_HScrollBar.Maximum;

            _dwMapPos = PAL_XY((WORD)((wMapHeight - MapPreview_PictureBox.Width) * percentage), PAL_Y(_dwMapPos));

            rect.x = PAL_X(_dwMapPos);
            PAL_DrawMapToSurface(sfMapPreview, rect, MapPreview_PictureBox, 1);
        }

        private void SubmitSelect_SceneSelectBoxD_UtilButton_Click(object sender, EventArgs e)
        {
            _fIsEnter = TRUE;
            this.Hide();
        }

        private void MapNameList_SceneSelectBoxT_ListView_ItemActivate(object sender, EventArgs e)
        {
            INT     i, j, k, iThisScene, iEventIndex, iSceneIndex, iSizeOfEvent, iOffset, iNextOffset;
            BYTE[]  Data_Buf = null;
            LPSTR   lpszMapName;

            System.Windows.Forms.ListView _ListView = sender as System.Windows.Forms.ListView;
            ListViewItem                  ThisItem  = _ListView.FocusedItem;

            if (ThisItem != null)
            {
                foreach (ListViewItem brother in _ListView.Items) brother.BackColor = _ListView.BackColor;

                ThisItem.BackColor  = Color.DeepPink;

                //
                // Get the number of the currently selected scene
                //
                iThisScene = _ListView.Items.IndexOf(ThisItem);

                if (_iThisScene == -1 || _iThisScene != iThisScene)
                {
                    _iThisScene     = iThisScene;

                    //
                    // Clean up scene Wave
                    //
                    Pal_Global.wScreenWave      = 0;
                    Pal_Global.sWaveProgression = 0;

                    //
                    // Get file indexes for events and scenes
                    //
                    iSceneIndex = Pal_Cfg_GetCfgNodeItemIndex(lpszMainData, lpszScene);

                    //
                    // Get the size of scenes data
                    //
                    iSceneIndex     = Pal_Cfg_GetCfgNodeItemIndex(lpszScene, lpszEventObjectIndex);

                    //
                    // Get the number of events for the currently selected scene
                    //
                    List<PalCfgNodeItem> pcniTmp    = Pal_Cfg_GetCfgNode(lpszScene).pcniItems;
                    PalCfgNodeItem pcniThisItem     = null;
                    iNextOffset = 0;
                    for (i = 0; i < pcniTmp.Count; i++)
                    {
                        pcniThisItem = pcniTmp[i];

                        if (i >= iSceneIndex) break;

                        iNextOffset += UTIL_GetTypeSize(pcniThisItem.lpszType);
                    }
                    iOffset     = BitConverter.ToInt16(UTIL_SubBytes(_AllSceneData,  _iThisScene      * iSizeOfScene + iNextOffset, UTIL_GetTypeSize(pcniThisItem.lpszType)), 0);
                    iNextOffset = BitConverter.ToInt16(UTIL_SubBytes(_AllSceneData, (_iThisScene + 1) * iSizeOfScene + iNextOffset, UTIL_GetTypeSize(pcniThisItem.lpszType)), 0);

                    //
                    // Display the information of the currently selected scene
                    //
                    lpszMapName = $"当前: [0x{_iThisScene:X4}] {_iThisScene:D5} 事件数: {iNextOffset - iOffset}\n{Pal_Cfg_GetCfgNodeItem(lpszSceneDesc, $"0x{_iThisScene + 1:X4}").lpszTitle}";
                    ThisSceneName_MapNameListT_SceneSelectBoxT_MainBoxR_SplitContainer.Text = lpszMapName;

                    //
                    // Get MapID
                    //
                    {
                        //
                        // Get the size of scenes data
                        //
                        iSceneIndex = Pal_Cfg_GetCfgNodeItemIndex(lpszScene, lpszMapID);

                        iNextOffset = 0;
                        for (i = 0; i < pcniTmp.Count; i++)
                        {
                            pcniThisItem = pcniTmp[i];

                            if (i >= iSceneIndex) break;

                            iNextOffset += UTIL_GetTypeSize(pcniThisItem.lpszType);
                        }
                        Pal_Map.iMapNum = BitConverter.ToInt16(UTIL_SubBytes(_AllSceneData, _iThisScene * iSizeOfScene + iNextOffset, UTIL_GetTypeSize(pcniThisItem.lpszType)), 0);
                    }

                    //
                    // Initialize map tiles
                    //
                    {
                        const INT iTileThird = 128, iTileSsecond = 64, iTileFirst = 2;

                        //
                        // Get map tiles data
                        //
                        Map_Tmp = Pal_File_GetFile(lpszGameMap).bufFile;
                        PAL_MKFDecompressChunk(ref Data_Buf, Pal_Map.iMapNum, ref Map_Tmp);

                        Pal_Map.Tiles = new DWORD[iTileThird, iTileSsecond, iTileFirst];

                        for (i = 0; i < iTileThird; i++)
                        {
                            for (j = 0; j < iTileSsecond; j++)
                            {
                                for (k = 0; k < iTileFirst; k++)
                                {
                                    iOffset = i * iTileSsecond * iTileFirst + j * iTileFirst + k;

                                    Pal_Map.Tiles[i, j, k] = BitConverter.ToUInt32(Data_Buf, iOffset * sizeof(DWORD));
                                }
                            }
                        }

                        Data_Buf = null;
                    }

                    //
                    // Initialize map bitmaps
                    //
                    {
                        //
                        // Get map tiles data
                        //
                        Map_Tmp = Pal_File_GetFile(lpszGameMapTile).bufFile;
                        PAL_MKFReadChunk(ref Pal_Map.TileSprite, Pal_Map.iMapNum, ref Map_Tmp);
                    }

                    _dwMapPos = dwMinMapPos;
                    rect.x      = PAL_X(_dwMapPos);
                    rect.y      = PAL_Y(_dwMapPos);
                    PAL_DrawMapToSurface(sfMapPreview, rect, MapPreview_PictureBox, 1);

                    ScrollTD_ScrollBoxT_MainBoxL_VScrollBar.Value   = 0;
                    ScrollLR_ScrollBoxD_MainBoxL_HScrollBar.Value   = 0;

                    //
                    // Allow clicking on the OK button
                    //
                    if (_iThisScene != -1)
                    {
                        SubmitSelect_SceneSelectBoxD_UtilButton.Enabled = TRUE;
                        MapPreview_PictureBox.Enabled                   = TRUE;
                        ScrollTD_ScrollBoxT_MainBoxL_VScrollBar.Enabled = TRUE;
                        ScrollLR_ScrollBoxD_MainBoxL_HScrollBar.Enabled = TRUE;
                    }
                }
                else
                {
                    //
                    // Simulate scene wave
                    //
                    if (Pal_Global.wScreenWave > 0)
                    {
                        Pal_Global.wScreenWave      = 0;
                        Pal_Global.sWaveProgression = 0;
                    }
                    else
                    {
                        Pal_Global.wScreenWave      = 6;
                        Pal_Global.sWaveProgression = 0;
                    }
                }
            }
        }

        private void Form_SceneSelect_Load(object sender, EventArgs e)
        {
            INT i, iSceneIndex, iSizeOfEvent, iSceneCount, iColumnsW;
            LPSTR lpszMapName;

            //
            // Initialize confirmation selection button
            //
            _fIsEnter = FALSE;

            if (fIsLoadingCompleted) return;

            //
            // Get the map file node
            //
            pfFile_Map = Pal_File_GetFile(lpszGameMap);

            //
            // Get file indexes for scenes
            //
            iSceneIndex = Pal_Cfg_GetCfgNodeItemIndex(lpszMainData, lpszScene);

            //
            // Get main file data
            //
            pfFile_SSS = Pal_File_GetFile(lpszMainData);

            //
            // Get all scene data
            //
            PAL_MKFReadChunk(ref _AllSceneData, iSceneIndex, ref pfFile_SSS.bufFile);

            //
            // Get the size of scenes data
            //
            iSceneIndex = PAL_MKFGetChunkSize(iSceneIndex, pfFile_SSS.bufFile);

            //
            // Get the size of each group of scenes
            //
            iSizeOfScene = Pal_Cfg_GetChunkSize(lpszScene);

            //
            // Get the number of scenes
            //
            nScene = iSceneIndex / iSizeOfScene;

            //
            // Set the table header of the map list
            //
            iColumnsW   = MapNameList_SceneSelectBoxT_ListView.Width;
            iColumnsW  -= MapNameList_SceneSelectBoxT_ListView.Margin.Left;
            iColumnsW  -= MapNameList_SceneSelectBoxT_ListView.Margin.Right;
            MapNameList_SceneSelectBoxT_ListView.Columns.Add("地图编号", iColumnsW, HorizontalAlignment.Center);

            //
            // Add all map names to the map list
            //
            for (i = 1; i < nScene; i++)
            {
                lpszMapName = Pal_Cfg_GetCfgNodeItem(lpszSceneDesc, $"0x{i:X4}").lpszTitle;
                MapNameList_SceneSelectBoxT_ListView.Items.Add(new ListViewItem($"[0x{i:X4}] {i:D5}: {lpszMapName}"));
            }

            //
            // Enable loading completion flag
            //
            fIsLoadingCompleted = TRUE;
        }
    }
}
