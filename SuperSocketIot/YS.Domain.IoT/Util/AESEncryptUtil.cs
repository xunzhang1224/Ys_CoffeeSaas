using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YS.Domain.IoT.Util
{
    /// <summary>
    /// AES加密工具类
    /// </summary>
    public class AESEncryptUtil
    {
        private const byte Keylength = 32;
        private const byte Blocksize = 16;
        private const byte Rounds = 14;
        private const byte Bpoly = 0x1b;

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="buffer">buffer</param>
        /// <param name="key">key</param>
        /// <param name="chainBlock">chainBlock</param>
        public static void AesEncrypt(byte[] buffer, byte[] key, ref byte[] chainBlock)
        {
            var block1 = new byte[256];
            var block2 = new byte[256];
            var tempbuf = new byte[256];
            byte[] aesKeyTable;
            if (key.Length < 32)
            {
                aesKeyTable = new byte[32];
                Array.Copy(key, aesKeyTable, key.Length);
            }
            else
            {
                aesKeyTable = key;
            }

            AesEncInit(aesKeyTable, block1, block2, tempbuf);
            XorBytes(ref buffer, ref chainBlock, Blocksize);
            Cipher(ref buffer, ref block1, block2, tempbuf);
            CopyBytes(ref chainBlock, ref buffer, Blocksize);
        }
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="buffer">buffer</param>
        /// <param name="key">key</param>
        /// <param name="chainBlock">chainBlock</param>
        public static void AesDecrypt(byte[] buffer, byte[] key, ref byte[] chainBlock)
        {
            var block1 = new byte[256];
            var block2 = new byte[256];
            var tempbuf = new byte[256];
            var aesKeyTable = key;
            AesDecInit(aesKeyTable, block1, block2, tempbuf);
            var temp = new byte[Blocksize];

            CopyBytes(ref temp, ref buffer, Blocksize);
            InvCipher(ref buffer, ref block1, block2);
            XorBytes(ref buffer, ref chainBlock, Blocksize);
            CopyBytes(ref chainBlock, ref temp, Blocksize);
        }

        private static void CalcPowLog(byte[] block1, byte[] tempbuf)
        {
            byte i = 0, t = 1;

            do
            {
                block1[i] = t;
                tempbuf[t] = i;
                i++;
                var k = (byte)(t & 0x80);
                if (k != 0)
                {
                    t ^= (byte)((t << 1) ^ Bpoly);
                }
                else
                {
                    t ^= (byte)((t << 1) ^ 0);
                }
            }
            while (t != 1);
            block1[255] = block1[0];
        }

        private static void CalcSBox(byte[] block1, byte[] block2, byte[] tempbuf)
        {
            byte i = 0;
            do
            {
                var temp = i > 0 ? block1[255 - tempbuf[i]] : (byte)0;
                var result = (byte)(temp ^ 0x63);
                byte rot;
                for (rot = 0; rot < 4; rot++)
                {
                    temp = (byte)((temp << 1) | (temp >> 7));
                    result ^= temp;
                }
                block2[i] = result;
            }
            while (++i != 0);
        }

        /// <summary>
        /// CycleLeft
        /// </summary>
        /// <param name="row"> row</param>
        private static void CycleLeft(ref byte[] row)
        {
            var temp = row[0];

            row[0] = row[1];
            row[1] = row[2];
            row[2] = row[3];
            row[3] = temp;
        }

        /// <summary>
        /// SubBytes
        /// </summary>
        /// <param name="bytes">bytes</param>
        /// <param name="count">count</param>
        /// <param name="adFlag">adFlag</param>
        /// <param name="block2">block2</param>
        /// <param name="tempbuf">tempbuf</param>
        private static void SubBytes(ref byte[] bytes, byte count, bool adFlag, byte[] block2, byte[] tempbuf)
        {
            byte cnt = 0;
            do
            {
                if (adFlag)
                    bytes[cnt] = block2[bytes[cnt]];
                else
                    bytes[cnt] = tempbuf[bytes[cnt]];
                cnt++;
            }
            while (--count != 0);
        }

        /// <summary>
        /// XorBytes
        /// </summary>
        /// <param name="bytes1">bytes1</param>
        /// <param name="bytes2">bytes2</param>
        /// <param name="count">count</param>
        private static void XorBytes(ref byte[] bytes1, ref byte[] bytes2, byte count)
        {
            byte cnt = 0;
            do
            {
                bytes1[cnt] ^= bytes2[cnt];
                cnt++;
            }
            while (--count != 0);
        }

        /// <summary>
        /// KeyExpansion
        /// </summary>
        /// <param name="adFlag">adFlag</param>
        /// <param name="aesKeyTable">aesKeyTable</param>
        /// <param name="block1">block1</param>
        /// <param name="block2">block2</param>
        /// <param name="tempbuf">tempbuf</param>
        private static void KeyExpansion(bool adFlag, byte[] aesKeyTable, byte[] block1, byte[] block2, byte[] tempbuf)
        {
            var temp = new byte[4];
            var rcon = new byte[] { 0x01, 0x00, 0x00, 0x00 }; // Round constant.

            var i = Keylength;
            byte cnt = 0;
            do
            {
                block1[cnt] = aesKeyTable[cnt];
                cnt++;
            }
            while ((--i) != 0);

            cnt -= 4;
            temp[0] = block1[cnt++];
            temp[1] = block1[cnt++];
            temp[2] = block1[cnt++];
            temp[3] = block1[cnt++];

            i = Keylength;
            while (i < Blocksize * (Rounds + 1))
            {
                // Are we at the start of a multiple of the key size?
                if ((i % Keylength) == 0)
                {
                    CycleLeft(ref temp); // Cycle left once.
                    SubBytes(ref temp, 4, adFlag, block2, tempbuf); // Substitute each byte.
                    XorBytes(ref temp, ref rcon, 4); // Add constant in GF(2).
                                                     // if(Rcon )
                                                     // *Rcon = (*Rcon << 1) ^ (*Rcon & 0x80 ? BPOLY : 0);
                    if ((rcon[0] & 0x80) != 0)
                    {
                        rcon[0] = (byte)((rcon[0] << 1) ^ Bpoly);
                    }
                    else
                    {
                        rcon[0] = (byte)((rcon[0] << 1) ^ 0);
                    }
                }
                else
                {
                    // if (Keylength > 24)
                    // {
                    //    if ((i % Keylength) == Blocksize)
                    //    {
                    //        SubBytes(ref temp, 4, adFlag, block2, tempbuf); // Substitute each byte.
                    //    }
                    // }
                    if (i % Keylength == Blocksize)
                    {
                        SubBytes(ref temp, 4, adFlag, block2, tempbuf); // Substitute each byte.
                    }
                }

                cnt = (byte)(cnt - Keylength);
                var expandedKey = new byte[Keylength];
                Array.Copy(block1, cnt, expandedKey, 0, Keylength);
                XorBytes(ref temp, ref expandedKey, 4);
                cnt = (byte)(cnt + Keylength);
                block1[cnt++] = temp[0];
                block1[cnt++] = temp[1];
                block1[cnt++] = temp[2];
                block1[cnt++] = temp[3];

                i += 4; // Next 4 bytes.
            }
        }

        /// <summary>
        /// CopyBytes
        /// </summary>
        /// <param name="to">to</param>
        /// <param name="from">from</param>
        /// <param name="count">count</param>
        private static void CopyBytes(ref byte[] to, ref byte[] from, byte count)
        {
            byte cnt = 0;
            do
            {
                to[cnt] = from[cnt];
                cnt++;
            }
            while ((--count) != 0);
        }

        private static byte Multiply(byte num, byte factor)
        {
            byte mask = 1;
            byte result = 0;

            while (mask != 0)
            {
                if ((mask & factor) != 0)
                {
                    result ^= num;
                }
                mask <<= 1;

                if ((num & 0x80) != 0)
                {
                    num = (byte)((num << 1) ^ Bpoly);
                }
                else
                {
                    num = (byte)((num << 1) ^ 0);
                }
            }

            return result;
        }

        private static byte DotProduct(ref byte[] vector1, ref byte[] vector2)
        {
            byte result = 0;

            result ^= Multiply(vector1[0], vector2[0]); // *vector1++, *vector2++ );
            result ^= Multiply(vector1[1], vector2[1]);
            result ^= Multiply(vector1[2], vector2[2]);
            result ^= Multiply(vector1[3], vector2[3]);

            return result;
        }

        private static void MixColumn(ref byte[] column)
        {
            var row = new byte[] { 0x02, 0x03, 0x01, 0x01, 0x02, 0x03, 0x01, 0x01 };

            var result = new byte[4]; // Prepare first row of matrix twice, to eliminate need for cycling.

            var kk = new byte[8]; // Take dot products of each matrix row and the column vector.
            Array.Copy(row, 0, kk, 0, kk.Length);
            result[0] = DotProduct(ref kk, ref column);
            Array.Copy(kk, 0, row, 0, kk.Length);

            kk = new byte[5];
            Array.Copy(row, 3, kk, 0, kk.Length);
            result[1] = DotProduct(ref kk, ref column);
            Array.Copy(kk, 0, row, 3, kk.Length);

            kk = new byte[6];
            Array.Copy(row, 2, kk, 0, kk.Length);
            result[2] = DotProduct(ref kk, ref column);
            Array.Copy(kk, 0, row, 2, kk.Length);

            kk = new byte[7];
            Array.Copy(row, 1, kk, 0, kk.Length);
            result[3] = DotProduct(ref kk, ref column);
            Array.Copy(kk, 0, row, 1, kk.Length);

            column[0] = result[0]; // Copy temporary result to original column.
            column[1] = result[1];
            column[2] = result[2];
            column[3] = result[3];
        }

        private static void MixColumns(ref byte[] state)
        {
            var kk = new byte[state.Length];
            Array.Copy(state, 0, kk, 0, kk.Length);
            MixColumn(ref kk);
            Array.Copy(kk, 0, state, 0, kk.Length);

            kk = new byte[state.Length - 4];
            Array.Copy(state, 4, kk, 0, kk.Length);
            MixColumn(ref kk);
            Array.Copy(kk, 0, state, 4, kk.Length);

            kk = new byte[state.Length - 8];
            Array.Copy(state, 8, kk, 0, kk.Length);
            MixColumn(ref kk);
            Array.Copy(kk, 0, state, 8, kk.Length);

            kk = new byte[state.Length - 12];
            Array.Copy(state, 12, kk, 0, kk.Length);
            MixColumn(ref kk);
            Array.Copy(kk, 0, state, 12, kk.Length);
        }

        private static void ShiftRows(ref byte[] state)
        {
            var temp = state[1 + (0 * 4)];
            state[1 + (0 * 4)] = state[1 + (1 * 4)];
            state[1 + (1 * 4)] = state[1 + (2 * 4)];
            state[1 + (2 * 4)] = state[1 + (3 * 4)];
            state[1 + (3 * 4)] = temp;

            // Cycle third row left two times.
            temp = state[2 + (0 * 4)];
            state[2 + (0 * 4)] = state[2 + (2 * 4)];
            state[2 + (2 * 4)] = temp;
            temp = state[2 + (1 * 4)];
            state[2 + (1 * 4)] = state[2 + (3 * 4)];
            state[2 + (3 * 4)] = temp;

            // Cycle fourth row left three times, ie. right once.
            temp = state[3 + (3 * 4)];
            state[3 + (3 * 4)] = state[3 + (2 * 4)];
            state[3 + (2 * 4)] = state[3 + (1 * 4)];
            state[3 + (1 * 4)] = state[3 + (0 * 4)];
            state[3 + (0 * 4)] = temp;
        }

        /// <summary>
        /// Cipher
        /// </summary>
        /// <param name="block">block</param>
        /// <param name="expandedKey">expandedKey</param>
        /// <param name="block2">block2</param>
        /// <param name="tempbuf">tempbuf</param>
        private static void Cipher(ref byte[] block, ref byte[] expandedKey, byte[] block2, byte[] tempbuf)
        {
            byte[] buffer;
            var round = (byte)(Rounds - 1); // 10
            byte cnt = 0;

            XorBytes(ref block, ref expandedKey, 16);

            cnt += Blocksize;

            do
            {
                SubBytes(ref block, 16, true, block2, tempbuf);
                ShiftRows(ref block);
                MixColumns(ref block);
                buffer = new byte[Blocksize];
                Array.Copy(expandedKey, cnt, buffer, 0, buffer.Length);
                XorBytes(ref block, ref buffer, 16);

                cnt += Blocksize;
            }
            while ((--round) != 0);

            SubBytes(ref block, 16, true, block2, tempbuf);
            ShiftRows(ref block);
            buffer = new byte[Blocksize];
            Array.Copy(expandedKey, cnt, buffer, 0, buffer.Length);
            XorBytes(ref block, ref buffer, 16);
        }

        /// <summary>
        /// AesEncInit
        /// </summary>
        /// <param name="aesKeyTable">aesKeyTable</param>
        /// <param name="block1">block1</param>
        /// <param name="block2">block2</param>
        /// <param name="tempbuf">tempbuf</param>
        private static void AesEncInit(byte[] aesKeyTable, byte[] block1, byte[] block2, byte[] tempbuf)
        {
            CalcPowLog(block1, tempbuf);

            CalcSBox(block1, block2, tempbuf);

            KeyExpansion(true, aesKeyTable, block1, block2, tempbuf);
        }

        /// <summary>
        /// CalcPowLog1
        /// </summary>
        /// <param name="block1">block1</param>
        /// <param name="block2">block2</param>
        private static void CalcPowLog1(byte[] block1, byte[] block2)
        {
            byte i = 0, t = 1;
            do
            {
                block1[i] = t;
                block2[t] = i;
                i++;
                var k = (byte)(t & 0x80);
                if (k != 0)
                {
                    t ^= (byte)((t << 1) ^ Bpoly);
                }
                else
                {
                    t ^= (byte)((t << 1) ^ 0);
                }
            }
            while (t != 1);
            block1[255] = block1[0];
        }

        /// <summary>
        /// CalcSBox1
        /// </summary>
        /// <param name="block1">block1</param>
        /// <param name="block2">block2</param>
        /// <param name="tempbuf">tempbuf</param>
        private static void CalcSBox1(byte[] block1, byte[] block2, byte[] tempbuf)
        {
            byte i = 0;
            do
            {
                var temp = i > 0 ? block1[255 - block2[i]] : (byte)0;

                var result = (byte)(temp ^ 0x63);
                byte rot;
                for (rot = 0; rot < 4; rot++)
                {
                    temp = (byte)((temp << 1) | (temp >> 7));
                    result ^= temp;
                }
                tempbuf[i] = result;
            }
            while (++i != 0);
        }

        /// <summary>
        /// CalcSBoxInv
        /// </summary>
        /// <param name="block2">block2</param>
        /// <param name="tempbuf">tempbuf</param>
        private static void CalcSBoxInv(byte[] block2, byte[] tempbuf)
        {
            byte i = 0;
            byte j = 0;
            do
            {
                do
                {
                    if (tempbuf[j] == i)
                    {
                        block2[i] = j;
                        j = 255;
                    }
                }
                while (++j != 0);
            }
            while (++i != 0);
        }

        /// <summary>
        /// InvSubBytesAndXor
        /// </summary>
        /// <param name="bytes">bytes</param>
        /// <param name="key">key</param>
        /// <param name="count">count</param>
        /// <param name="block2">block2</param>
        private static void InvSubBytesAndXor(ref byte[] bytes, ref byte[] key, byte count, byte[] block2)
        {
            byte bCnt = 0;
            byte kCnt = 0;
            do
            {
                bytes[bCnt] = (byte)(block2[bytes[bCnt]] ^ key[kCnt]); // Use block2 directly. Increases speed.
                bCnt++;
                kCnt++;
            }
            while ((--count) != 0);
        }

        /// <summary>
        /// InvShiftRows
        /// </summary>
        /// <param name="state">state</param>
        private static void InvShiftRows(ref byte[] state)
        {
            var temp = state[1 + (3 * 4)];
            state[1 + (3 * 4)] = state[1 + (2 * 4)];
            state[1 + (2 * 4)] = state[1 + (1 * 4)];
            state[1 + (1 * 4)] = state[1 + (0 * 4)];
            state[1 + (0 * 4)] = temp;

            temp = state[2 + (0 * 4)];
            state[2 + (0 * 4)] = state[2 + (2 * 4)];
            state[2 + (2 * 4)] = temp;
            temp = state[2 + (1 * 4)];
            state[2 + (1 * 4)] = state[2 + (3 * 4)];
            state[2 + (3 * 4)] = temp;

            temp = state[3 + (0 * 4)];
            state[3 + (0 * 4)] = state[3 + (1 * 4)];
            state[3 + (1 * 4)] = state[3 + (2 * 4)];
            state[3 + (2 * 4)] = state[3 + (3 * 4)];
            state[3 + (3 * 4)] = temp;
        }

        /// <summary>
        /// InvMixColumn
        /// </summary>
        /// <param name="column">column</param>
        private static void InvMixColumn(ref byte[] column)
        {
            byte k;

            var r0 = (byte)(column[1] ^ column[2] ^ column[3]);
            var r1 = (byte)(column[0] ^ column[2] ^ column[3]);
            var r2 = (byte)(column[0] ^ column[1] ^ column[3]);
            var r3 = (byte)(column[0] ^ column[1] ^ column[2]);

            for (byte i = 0; i < 4; i++)
            {
                k = (byte)(column[i] & 0x80);
                if (k != 0)
                {
                    column[i] = (byte)((column[i] << 1) ^ Bpoly);
                }
                else
                {
                    column[i] = (byte)((column[i] << 1) ^ 0);
                }
            }

            r0 ^= (byte)(column[0] ^ column[1]);
            r1 ^= (byte)(column[1] ^ column[2]);
            r2 ^= (byte)(column[2] ^ column[3]);
            r3 ^= (byte)(column[0] ^ column[3]);

            for (byte i = 0; i < 4; i++)
            {
                k = (byte)(column[i] & 0x80);
                if (k != 0)
                {
                    column[i] = (byte)((column[i] << 1) ^ Bpoly);
                }
                else
                {
                    column[i] = (byte)((column[i] << 1) ^ 0);
                }
            }

            r0 ^= (byte)(column[0] ^ column[2]);
            r1 ^= (byte)(column[1] ^ column[3]);
            r2 ^= (byte)(column[0] ^ column[2]);
            r3 ^= (byte)(column[1] ^ column[3]);

            for (byte i = 0; i < 4; i++)
            {
                k = (byte)(column[i] & 0x80);
                if (k != 0)
                {
                    column[i] = (byte)((column[i] << 1) ^ Bpoly);
                }
                else
                {
                    column[i] = (byte)((column[i] << 1) ^ 0);
                }
            }

            column[0] ^= (byte)(column[1] ^ column[2] ^ column[3]);
            r0 ^= column[0];
            r1 ^= column[0];
            r2 ^= column[0];
            r3 ^= column[0];

            column[0] = r0;
            column[1] = r1;
            column[2] = r2;
            column[3] = r3;
        }

        /// <summary>
        /// InvMixColumns
        /// </summary>
        /// <param name="state">state</param>
        private static void InvMixColumns(ref byte[] state)
        {
            var kk = new byte[state.Length];
            Array.Copy(state, 0, kk, 0, kk.Length);
            InvMixColumn(ref kk);
            Array.Copy(kk, 0, state, 0, kk.Length);

            kk = new byte[state.Length - 4];
            Array.Copy(state, 4, kk, 0, kk.Length);
            InvMixColumn(ref kk);
            Array.Copy(kk, 0, state, 4, kk.Length);

            kk = new byte[state.Length - 8];
            Array.Copy(state, 8, kk, 0, kk.Length);
            InvMixColumn(ref kk);
            Array.Copy(kk, 0, state, 8, kk.Length);

            kk = new byte[state.Length - 12];
            Array.Copy(state, 12, kk, 0, kk.Length);
            InvMixColumn(ref kk);
            Array.Copy(kk, 0, state, 12, kk.Length);
        }

        /// <summary>
        /// InvCipher
        /// </summary>
        /// <param name="block">block</param>
        /// <param name="expandedKey">expandedKey</param>
        /// <param name="block2">block2</param>
        private static void InvCipher(ref byte[] block, ref byte[] expandedKey, byte[] block2)
        {
            var buffer = new byte[16];
            var round = (byte)(Rounds - 1); // 10
            var cnt = 0;

            cnt += Blocksize * Rounds;
            Array.Copy(expandedKey, cnt, buffer, 0, 16);
            XorBytes(ref block, ref buffer, 16);
            cnt -= Blocksize;

            do
            {
                InvShiftRows(ref block);
                Array.Copy(expandedKey, cnt, buffer, 0, 16);
                InvSubBytesAndXor(ref block, ref buffer, 16, block2);
                cnt -= Blocksize;
                InvMixColumns(ref block);
            }
            while (--round != 0);

            InvShiftRows(ref block);
            Array.Copy(expandedKey, cnt, buffer, 0, 16);
            InvSubBytesAndXor(ref block, ref buffer, 16, block2);
        }

        /// <summary>
        /// AesDecInit
        /// </summary>
        /// <param name="aesKeyTable">aesKeyTable</param>
        /// <param name="block1">block1</param>
        /// <param name="block2">block2</param>
        /// <param name="tempbuf">tempbuf</param>
        private static void AesDecInit(byte[] aesKeyTable, byte[] block1, byte[] block2, byte[] tempbuf)
        {
            CalcPowLog1(block1, block2);
            CalcSBox1(block1, block2, tempbuf);
            KeyExpansion(false, aesKeyTable, block1, block2, tempbuf);
            CalcSBoxInv(block2, tempbuf);
        }
    }
}
