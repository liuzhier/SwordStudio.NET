using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;

using static System.Array;

using static PalYJ_1.Pal_YJ_1;
using static PalUtil.Pal_Util;
using PalCfg;
using System;
using System.IO;

namespace PalYJ_1
{
    public unsafe class Pal_YJ_1
    {

        public class YJ1_TreeNode   // 节点
        {
            public ushort   index  = 0; // List 索引
            public byte     value  = 0; // 值
            public byte     leaf   = 0; // 叶
            public ushort   level  = 0; // 层级
            public uint     weight = 0; // 这个不知道

            public YJ1_TreeNode parent = null; // 父节点
            public YJ1_TreeNode left   = null; // 左子节点
            public YJ1_TreeNode right  = null; // 右子节点
        }

        public class YJ1_TreeNodeList   // 节点集合
        {
            public YJ1_TreeNode     *node = null;  // 节点
            public YJ1_TreeNodeList *next = null;  // 下一个节点集合
        }

        public class YJ_1_FILEHEADER    // YJ_1 头部数据块
        {
            public uint     Signature          = 0; // 'YJ_1' 标识
            public uint     UncompressedLength = 0; // SMKF 数据块压缩前的大小
            public uint     CompressedLength   = 0; // SMKF 数据块压缩后的 YJ_1 数据块的大小
            public ushort   BlockCount         = 0; // 区块数量
            public byte     Unknown            = 0; // 凑数块
            public byte     HuffmanTreeLength  = 0; // 哈夫曼树的长度
        }

        public class YJ_1_BLOCKHEADER   // 区块的头部数据块
        {
            public ushort       UncompressedLength        = 0;              // 数据块压缩前的长度最大为 0x4000
            public ushort       CompressedLength          = 0;              // 数据块压缩后的长度，注意其中包括数据块的头部
            public ushort[]     LZSSRepeatTable           = new ushort[4];  // LZSS 重复表
            public byte[]       LZSSOffsetCodeLengthTable = new byte[4];    // LZSS 偏移代码长度表
            public byte[]       LZSSRepeatCodeLengthTable = new byte[3];    // LZSS 重复代码长度表
            public byte[]       CodeCountCodeLengthTable  = new byte[3];    // 代码计数 \ 代码长度表
            public byte[]       CodeCountTable            = new byte[2];    // 代码计数表
        }

        public static uint
        yj1_get_bits(
            ref byte[]  src,
            ref uint    bitptr,
            uint        count
        )
        {
            uint        i = (bitptr >> 4) << 1;
            uint        bptr = bitptr & 0xf, uiBits;
            ushort      mask;
            ushort      usLow, usHigh;

            bitptr += count;

            if (i >= src.Length) return 0;

            if (count > 16 - bptr)
            {
                count += bptr - 16;
                mask = (ushort)(0xffff >> (int)bptr);

                usLow = (ushort)(((src[i] | (src[i + 1] << 8)) & mask) << (int)count);
                usHigh = (ushort)((src[i + 2] | (src[i + 3] << 8)) >> (int)(16 - count));
                uiBits = (uint)(usLow | usHigh);
            }
            else
            {
                usLow = (ushort)((src[i] | (src[i + 1] << 8)) << (int)bptr);
                uiBits = (uint)(usLow >> (int)(16 - count));
            }

            return uiBits;
        }

        public static ushort
        yj1_get_loop(
            ref byte[]              src,
            ref uint                bitptr,
            ref YJ_1_BLOCKHEADER    header
        )
        {
            if (yj1_get_bits(ref src, ref bitptr, 1) != 0)
            {
                return header.CodeCountTable[0];
            }
            else
            {
                uint temp = yj1_get_bits(ref src, ref bitptr, 2);

                if (temp != 0)
                {
                    return (ushort)yj1_get_bits(ref src, ref bitptr, header.CodeCountCodeLengthTable[temp - 1]);
                }
                else
                {
                    return header.CodeCountTable[1];
                }
            }
        }

        public static ushort
        yj1_get_count(
            ref byte[] src,
            ref uint bitptr,
            ref YJ_1_BLOCKHEADER header
        )
        {
            ushort temp;

            if ((temp = (ushort)yj1_get_bits(ref src, ref bitptr, 2)) != 0)
            {
                if (yj1_get_bits(ref src, ref bitptr, 1) != 0)
                    return (ushort)yj1_get_bits(ref src, ref bitptr, header.LZSSRepeatCodeLengthTable[temp - 1]);
                else
                    return UTIL_SwapLE16(header.LZSSRepeatTable[temp]);
            }
            else
            {
                return UTIL_SwapLE16(header.LZSSRepeatTable[0]);
            }
        }

        public static int
        YJ1_Decompress(
            ref byte[]      Source,
            ref byte[]      Destination
        )
        {
            int              i, j, iOffset = 0, iNodeOffset = 0, iSrcOffset = 0, iDestOffset = 0;
            YJ_1_FILEHEADER  FH_YJ_1;
            YJ1_TreeNode     TN_YJ_1_This;
            YJ_1_BLOCKHEADER BH_YJ_1_This;
            ArrayList        root;

            byte[]           src;

            //
            // 判断源文件是否为空
            //
            if (Source == null)
            {
                return -1;
            }

            src = Source;

            //
            // 获取 YJ_1 文件头部信息
            //
            FH_YJ_1                     = new YJ_1_FILEHEADER();
            //FH_YJ_1.Signature          = BitConverter.ToUInt32(src[iOffset..(iOffset += 4)], 0);
            //FH_YJ_1.UncompressedLength = BitConverter.ToUInt32(src[iOffset..(iOffset += 4)], 0);
            //FH_YJ_1.CompressedLength   = BitConverter.ToUInt32(src[iOffset..(iOffset += 4)], 0);
            //FH_YJ_1.BlockCount         = BitConverter.ToUInt32(src[iOffset..(iOffset += 4)], 0);
            FH_YJ_1.Signature           = BitConverter.ToUInt32(UTIL_SubBytes(src, ref iOffset, 4), 0);
            FH_YJ_1.UncompressedLength  = BitConverter.ToUInt32(UTIL_SubBytes(src, ref iOffset, 4), 0);
            FH_YJ_1.CompressedLength    = BitConverter.ToUInt32(UTIL_SubBytes(src, ref iOffset, 4), 0);
            FH_YJ_1.BlockCount          = (ushort)(src[iOffset] | (src[++iOffset] << 8));
            FH_YJ_1.Unknown             = src[++iOffset];
            FH_YJ_1.HuffmanTreeLength   = src[++iOffset];

            //
            // 验证文件头部标识 'YJ_1'
            //
            if (FH_YJ_1.Signature != 0x315f4a59)
            {
                return -1;
            }

            //
            // 初始化 YJ_1 编码解压缩后的内存
            //
            Destination = new byte[FH_YJ_1.UncompressedLength];

            do
            {
                //
                // 获取 Huffman 树长度，当前比特位的索引
                // 还有 ...... flag 标识？？？
                //
                ushort tree_len = (ushort)(FH_YJ_1.HuffmanTreeLength * 2);
                uint   bitptr   = 0;
                byte[] flag;

                i    = 16 + tree_len;
                //flag = src[i..];
                flag = UTIL_SubBytes(src, i);

                root = new ArrayList();

                for (i = 0; i <= tree_len; i++)
                {
                    root.Add(new YJ1_TreeNode());
                    ((YJ1_TreeNode)root[i]).index = (ushort)i;
                }

                //
                // 获取根节点
                //
                TN_YJ_1_This       = (YJ1_TreeNode)root[0];
                TN_YJ_1_This.index = 0;
                TN_YJ_1_This.leaf  = 0;
                TN_YJ_1_This.value = 0;
                TN_YJ_1_This.left  = (YJ1_TreeNode)root[1];
                TN_YJ_1_This.right = (YJ1_TreeNode)root[2];

                //
                // 遍历整棵树
                //
                for (i = 1; i <= tree_len; i++)
                {
                    TN_YJ_1_This       = (YJ1_TreeNode)root[i];
                    TN_YJ_1_This.leaf  = (byte)(!(yj1_get_bits(ref flag, ref bitptr, 1) != 0) ? 1 : 0);
                    TN_YJ_1_This.value = src[15 + i];

                    if (TN_YJ_1_This.leaf != 0)
                    {
                        TN_YJ_1_This.left = TN_YJ_1_This.right = null;
                    }
                    else
                    {
                        //
                        // 这里获取左子节点可能错误，但解决方法未知......
                        //
                        TN_YJ_1_This.left  = (YJ1_TreeNode)root[(TN_YJ_1_This.value << 1) + 1];
                        TN_YJ_1_This.right = (YJ1_TreeNode)root[TN_YJ_1_This.left.index + 1];
                    }
                }

                iSrcOffset += 16 + tree_len + ((((tree_len & 0xf) != 0) ? (tree_len >> 4) + 1 : (tree_len >> 4)) << 1);
                //src = Source[iSrcOffset..];
                src = UTIL_SubBytes(Source, iSrcOffset);
            } while (false);

            for (i = 0; i < UTIL_SwapLE16(FH_YJ_1.BlockCount); i++)
            {
                uint bitptr = 0;
                iOffset = 0;

                //
                // 获取当前区块的头部数据块
                //
                BH_YJ_1_This = new YJ_1_BLOCKHEADER();
                BH_YJ_1_This.UncompressedLength           = (ushort)(src[  iOffset] | (src[++iOffset] << 8));
                BH_YJ_1_This.CompressedLength             = (ushort)(src[++iOffset] | (src[++iOffset] << 8));

                BH_YJ_1_This.LZSSRepeatTable[0]           = (ushort)(src[++iOffset] | (src[++iOffset] << 8));
                BH_YJ_1_This.LZSSRepeatTable[1]           = (ushort)(src[++iOffset] | (src[++iOffset] << 8));
                BH_YJ_1_This.LZSSRepeatTable[2]           = (ushort)(src[++iOffset] | (src[++iOffset] << 8));
                BH_YJ_1_This.LZSSRepeatTable[3]           = (ushort)(src[++iOffset] | (src[++iOffset] << 8));

                BH_YJ_1_This.LZSSOffsetCodeLengthTable[0] = src[++iOffset];
                BH_YJ_1_This.LZSSOffsetCodeLengthTable[1] = src[++iOffset];
                BH_YJ_1_This.LZSSOffsetCodeLengthTable[2] = src[++iOffset];
                BH_YJ_1_This.LZSSOffsetCodeLengthTable[3] = src[++iOffset];

                BH_YJ_1_This.LZSSRepeatCodeLengthTable[0] = src[++iOffset];
                BH_YJ_1_This.LZSSRepeatCodeLengthTable[1] = src[++iOffset];
                BH_YJ_1_This.LZSSRepeatCodeLengthTable[2] = src[++iOffset];

                BH_YJ_1_This.CodeCountCodeLengthTable[0]  = src[++iOffset];
                BH_YJ_1_This.CodeCountCodeLengthTable[1]  = src[++iOffset];
                BH_YJ_1_This.CodeCountCodeLengthTable[2]  = src[++iOffset];

                BH_YJ_1_This.CodeCountTable[0]            = src[++iOffset];
                BH_YJ_1_This.CodeCountTable[1]            = src[++iOffset];

                //
                //
                iSrcOffset += 4;
                //src = Source[iSrcOffset..];
                src = UTIL_SubBytes(Source, iSrcOffset);

                //
                // 判断压缩后的块大小是否大于 0
                //
                if (!(UTIL_SwapLE16(BH_YJ_1_This.CompressedLength) != 0))
                {
                    ushort hul = UTIL_SwapLE16(BH_YJ_1_This.UncompressedLength);

                    while (hul-- != 0)
                    {
                        Destination[iDestOffset++] = src[iSrcOffset++];
                        //src = Source[iSrcOffset..];
                        src = UTIL_SubBytes(Source, iSrcOffset);
                    }

                    continue;
                }

                //
                // 直接跳到 代码计数 \ 代码长度表
                //
                iSrcOffset += 20;
                //src = Source[iSrcOffset..];
                src = UTIL_SubBytes(Source, iSrcOffset);

                for (j = 0;; j++)
                {
                    ushort loop;
                    if ((loop = yj1_get_loop(ref src, ref bitptr, ref BH_YJ_1_This)) == 0)
                        break;

                    while (loop-- != 0)
                    {
                        TN_YJ_1_This = (YJ1_TreeNode)root[0];

                        for (; !(TN_YJ_1_This.leaf != 0);)
                        {
                            if (yj1_get_bits(ref src, ref bitptr, 1) != 0)
                                TN_YJ_1_This = TN_YJ_1_This.right;
                            else
                                TN_YJ_1_This = TN_YJ_1_This.left;
                        }

                        Destination[iDestOffset++] = TN_YJ_1_This.value;
                    }

                    if ((loop = yj1_get_loop(ref src, ref bitptr, ref BH_YJ_1_This)) == 0)
                        break;

                    while (loop-- != 0)
                    {
                        uint pos, count;

                        count = yj1_get_count(ref src, ref bitptr, ref BH_YJ_1_This);
                        pos   = yj1_get_bits(ref src, ref bitptr, 2);
                        pos   = yj1_get_bits(ref src, ref bitptr, BH_YJ_1_This.LZSSOffsetCodeLengthTable[pos]);

                        while (count-- != 0)
                        {
                            Destination[iDestOffset] = Destination[iDestOffset - pos];
                            iDestOffset++;
                        }
                    }
                }

                //iDestOffset = UTIL_SwapLE16(BH_YJ_1_This.CompressedLength);

                iSrcOffset += UTIL_SwapLE16(BH_YJ_1_This.CompressedLength) - 24;
                //src = Source[iSrcOffset..];
                src = UTIL_SubBytes(Source, iSrcOffset);
            }

            return (int)UTIL_SwapLE32(FH_YJ_1.UncompressedLength);
        }

        public static int
        Decompress(
            ref byte[] Source,
            ref byte[] Destination
        )
        {
            if (!Pal_Cfg.fIsWIN95) return YJ1_Decompress(ref Source, ref Destination);
            //else return YJ2_Decompress(ref Source, ref Destination);

            return 0;
        }

        public static void
        YJ_1_Main()
        {
            string filePath = @"F:\\TEMP\\ABC1.YJ_1";

            byte[] bSrc_YJ_1 = File.ReadAllBytes(filePath), bDest_YJ_1 = null;

            YJ1_Decompress(ref bSrc_YJ_1, ref bDest_YJ_1);

            for (int i = 0; i < bDest_YJ_1.Length; i++)
            {
                Console.Write(bDest_YJ_1[i]);
                Console.Write(((i + 1) % 16 == 0) ? "\n" : "\t");
            }
        }
    }
}