using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text; 
using System.Drawing;
using System.Data;

namespace MideaAscm.Code
{
    public class BarCode128
    {
        private DataTable code128Table = new DataTable();

        /// <summary> 高度 /// </summary> 
        public int HeightImage = 40;//条码高度

        /// <summary> 是否显示条码文本  如果为NULL不显示 /// </summary> 
        public Font TitleFont = null;//条码文本

        /// <summary> 条码宽度放大倍数 </summary> 
        public byte WidthMultiple = 0;//宽度放大倍数

        /// <summary>
        /// 条码类别 
        /// </summary> 
        public enum Encode
        {
            Code128A,
            Code128B,
            Code128C,
            EAN128
        }

        public BarCode128()
        {
            code128Table.Columns.Add("ID");
            code128Table.Columns.Add("Code128A");//A（数字、大写字母、控制字符）
            code128Table.Columns.Add("Code128B");//B（数字、大小字母、字符）
            code128Table.Columns.Add("Code128C");//C（双位数字[00]-[99]的数字对集合, 共100个）
            code128Table.Columns.Add("BandCode");
            code128Table.CaseSensitive = true;
            #region 数据表
            code128Table.Rows.Add("0", " ", " ", "00", "212222");
            code128Table.Rows.Add("1", "!", "!", "01", "222122");
            code128Table.Rows.Add("2", "\"", "\"", "02", "222221");
            code128Table.Rows.Add("3", "#", "#", "03", "121223");
            code128Table.Rows.Add("4", "$", "$", "04", "121322");
            code128Table.Rows.Add("5", "%", "%", "05", "131222");
            code128Table.Rows.Add("6", "&", "&", "06", "122213");
            code128Table.Rows.Add("7", "'", "'", "07", "122312");
            code128Table.Rows.Add("8", "(", "(", "08", "132212");
            code128Table.Rows.Add("9", ")", ")", "09", "221213");
            code128Table.Rows.Add("10", "*", "*", "10", "221312");
            code128Table.Rows.Add("11", "+", "+", "11", "231212");
            code128Table.Rows.Add("12", ",", ",", "12", "112232");
            code128Table.Rows.Add("13", "-", "-", "13", "122132");
            code128Table.Rows.Add("14", ".", ".", "14", "122231");
            code128Table.Rows.Add("15", "/", "/", "15", "113222");
            code128Table.Rows.Add("16", "0", "0", "16", "123122");
            code128Table.Rows.Add("17", "1", "1", "17", "123221");
            code128Table.Rows.Add("18", "2", "2", "18", "223211");
            code128Table.Rows.Add("19", "3", "3", "19", "221132");
            code128Table.Rows.Add("20", "4", "4", "20", "221231");
            code128Table.Rows.Add("21", "5", "5", "21", "213212");
            code128Table.Rows.Add("22", "6", "6", "22", "223112");
            code128Table.Rows.Add("23", "7", "7", "23", "312131");
            code128Table.Rows.Add("24", "8", "8", "24", "311222");
            code128Table.Rows.Add("25", "9", "9", "25", "321122");
            code128Table.Rows.Add("26", ":", ":", "26", "321221");
            code128Table.Rows.Add("27", ";", ";", "27", "312212");
            code128Table.Rows.Add("28", "<", "<", "28", "322112");
            code128Table.Rows.Add("29", "=", "=", "29", "322211");
            code128Table.Rows.Add("30", ">", ">", "30", "212123");
            code128Table.Rows.Add("31", "?", "?", "31", "212321");
            code128Table.Rows.Add("32", "@", "@", "32", "232121");
            code128Table.Rows.Add("33", "A", "A", "33", "111323");
            code128Table.Rows.Add("34", "B", "B", "34", "131123");
            code128Table.Rows.Add("35", "C", "C", "35", "131321");
            code128Table.Rows.Add("36", "D", "D", "36", "112313");
            code128Table.Rows.Add("37", "E", "E", "37", "132113");
            code128Table.Rows.Add("38", "F", "F", "38", "132311");
            code128Table.Rows.Add("39", "G", "G", "39", "211313");
            code128Table.Rows.Add("40", "H", "H", "40", "231113");
            code128Table.Rows.Add("41", "I", "I", "41", "231311");
            code128Table.Rows.Add("42", "J", "J", "42", "112133");
            code128Table.Rows.Add("43", "K", "K", "43", "112331");
            code128Table.Rows.Add("44", "L", "L", "44", "132131");
            code128Table.Rows.Add("45", "M", "M", "45", "113123");
            code128Table.Rows.Add("46", "N", "N", "46", "113321");
            code128Table.Rows.Add("47", "O", "O", "47", "133121");
            code128Table.Rows.Add("48", "P", "P", "48", "313121");
            code128Table.Rows.Add("49", "Q", "Q", "49", "211331");
            code128Table.Rows.Add("50", "R", "R", "50", "231131");
            code128Table.Rows.Add("51", "S", "S", "51", "213113");
            code128Table.Rows.Add("52", "T", "T", "52", "213311");
            code128Table.Rows.Add("53", "U", "U", "53", "213131");
            code128Table.Rows.Add("54", "V", "V", "54", "311123");
            code128Table.Rows.Add("55", "W", "W", "55", "311321");
            code128Table.Rows.Add("56", "X", "X", "56", "331121");
            code128Table.Rows.Add("57", "Y", "Y", "57", "312113");
            code128Table.Rows.Add("58", "Z", "Z", "58", "312311");
            code128Table.Rows.Add("59", "[", "[", "59", "332111");
            code128Table.Rows.Add("60", "\\", "\\", "60", "314111");
            code128Table.Rows.Add("61", "]", "]", "61", "221411");
            code128Table.Rows.Add("62", "^", "^", "62", "431111");
            code128Table.Rows.Add("63", "_", "_", "63", "111224");
            code128Table.Rows.Add("64", "NUL", "`", "64", "111422");
            code128Table.Rows.Add("65", "SOH", "a", "65", "121124");
            code128Table.Rows.Add("66", "STX", "b", "66", "121421");
            code128Table.Rows.Add("67", "ETX", "c", "67", "141122");
            code128Table.Rows.Add("68", "EOT", "d", "68", "141221");
            code128Table.Rows.Add("69", "ENQ", "e", "69", "112214");
            code128Table.Rows.Add("70", "ACK", "f", "70", "112412");
            code128Table.Rows.Add("71", "BEL", "g", "71", "122114");
            code128Table.Rows.Add("72", "BS", "h", "72", "122411");
            code128Table.Rows.Add("73", "HT", "i", "73", "142112");
            code128Table.Rows.Add("74", "LF", "j", "74", "142211");
            code128Table.Rows.Add("75", "VT", "k", "75", "241211");
            code128Table.Rows.Add("76", "FF", "I", "76", "221114");
            code128Table.Rows.Add("77", "CR", "m", "77", "413111");
            code128Table.Rows.Add("78", "SO", "n", "78", "241112");
            code128Table.Rows.Add("79", "SI", "o", "79", "134111");
            code128Table.Rows.Add("80", "DLE", "p", "80", "111242");
            code128Table.Rows.Add("81", "DC1", "q", "81", "121142");
            code128Table.Rows.Add("82", "DC2", "r", "82", "121241");
            code128Table.Rows.Add("83", "DC3", "s", "83", "114212");
            code128Table.Rows.Add("84", "DC4", "t", "84", "124112");
            code128Table.Rows.Add("85", "NAK", "u", "85", "124211");
            code128Table.Rows.Add("86", "SYN", "v", "86", "411212");
            code128Table.Rows.Add("87", "ETB", "w", "87", "421112");
            code128Table.Rows.Add("88", "CAN", "x", "88", "421211");
            code128Table.Rows.Add("89", "EM", "y", "89", "212141");
            code128Table.Rows.Add("90", "SUB", "z", "90", "214121");
            code128Table.Rows.Add("91", "ESC", "{", "91", "412121");
            code128Table.Rows.Add("92", "FS", "|", "92", "111143");
            code128Table.Rows.Add("93", "GS", "}", "93", "111341");
            code128Table.Rows.Add("94", "RS", "~", "94", "131141");
            code128Table.Rows.Add("95", "US", "DEL", "95", "114113");
            code128Table.Rows.Add("96", "FNC3", "FNC3", "96", "114311");
            code128Table.Rows.Add("97", "FNC2", "FNC2", "97", "411113");
            code128Table.Rows.Add("98", "SHIFT", "SHIFT", "98", "411311");
            code128Table.Rows.Add("99", "CODEC", "CODEC", "99", "113141");
            code128Table.Rows.Add("100", "CODEB", "FNC4", "CODEB", "114131");
            code128Table.Rows.Add("101", "FNC4", "CODEA", "CODEA", "311141");
            code128Table.Rows.Add("102", "FNC1", "FNC1", "FNC1", "411131");
            code128Table.Rows.Add("103", "StartA", "StartA", "StartA", "211412");
            code128Table.Rows.Add("104", "StartB", "StartB", "StartB", "211214");
            code128Table.Rows.Add("105", "StartC", "StartC", "StartC", "211232");
            code128Table.Rows.Add("106", "Stop", "Stop", "Stop", "2331112");
            #endregion
        }

        /// <summary> 
        /// 获取128图形 
        /// </summary> 
        /// <param name="Code128">文字</param> 
        /// <param name="Code128Type">编码</param>       
        /// <returns>图形</returns> 
        public Bitmap GetCodeImage(string Code128,string title, Encode Code128Type)
        {
            string code128 = Code128;
            string sText = "";
            IList<int> ilistTextNumb = new List<int>();
            int iFist = 0;  //首位 
            switch (Code128Type)
            {
                case Encode.Code128C:
                    iFist = 105;
                    if (!((Code128.Length & 1) == 0)) throw new Exception("128C长度必须是偶数");
                    while (Code128.Length != 0)
                    {
                        int _Temp = 0;
                        try
                        {
                            int _CodeNumb128 = Int32.Parse(Code128.Substring(0, 2));
                        }
                        catch
                        {
                            throw new Exception("128C必须是数字！");
                        }
                        sText += GetValue(Code128Type, Code128.Substring(0, 2), ref _Temp);
                        ilistTextNumb.Add(_Temp);
                        Code128 = Code128.Remove(0, 2);
                    }
                    break;
                case Encode.EAN128:
                    iFist = 105;
                    if (!((Code128.Length & 1) == 0)) throw new Exception("EAN128长度必须是偶数");
                    ilistTextNumb.Add(102);
                    sText += "411131";
                    while (Code128.Length != 0)
                    {
                        int _Temp = 0;
                        try
                        {
                            int _CodeNumb128 = Int32.Parse(Code128.Substring(0, 2));
                        }
                        catch
                        {
                            throw new Exception("128C必须是数字！");
                        }
                        sText += GetValue(Encode.Code128C, Code128.Substring(0, 2), ref _Temp);
                        ilistTextNumb.Add(_Temp);
                        Code128 = Code128.Remove(0, 2);
                    }
                    break;
                default:
                    if (Code128Type == Encode.Code128A)
                    {
                        iFist = 103;
                    }
                    else
                    {
                        iFist = 104;
                    }

                    while (Code128.Length != 0)
                    {
                        int _Temp = 0;
                        string _ValueCode = GetValue(Code128Type, Code128.Substring(0, 1), ref _Temp);
                        if (_ValueCode.Length == 0) throw new Exception("无效的字符集!" + Code128.Substring(0, 1).ToString());
                        sText += _ValueCode;
                        ilistTextNumb.Add(_Temp);
                        Code128 = Code128.Remove(0, 1);
                    }
                    break;
            }
            if (ilistTextNumb.Count == 0) throw new Exception("错误的编码,无数据");
            sText = sText.Insert(0, GetValue(iFist)); //获取开始位 

            for (int i = 0; i != ilistTextNumb.Count; i++)
            {
                iFist += ilistTextNumb[i] * (i + 1);
            }
            iFist = iFist % 103;           //获得严效位 
            sText += GetValue(iFist);  //获取严效位 
            sText += "2331112"; //结束位 
            Bitmap _CodeImage = GetImage(sText);
            GetViewText(_CodeImage, title);
            return _CodeImage;
        }


        /// <summary> 
        /// 获取目标对应的数据 
        /// </summary> 
        /// <param name="Code128Type">编码</param> 
        /// <param name="Code128">数值 A b  30</param> 
        /// <param name="p_SetID">返回编号</param> 
        /// <returns>编码</returns> 
        private string GetValue(Encode Code128Type, string Code128, ref int p_SetID)
        {
            if (code128Table == null) 
                return "";
            DataRow[] _Row = code128Table.Select(Code128Type.ToString() + "='" + Code128 + "'");
            if (_Row.Length != 1) 
                throw new Exception("错误的编码" + Code128.ToString());
            p_SetID = Int32.Parse(_Row[0]["ID"].ToString());
            return _Row[0]["BandCode"].ToString();
        }

        /// <summary> 
        /// 根据编号获得条纹 
        /// </summary> 
        /// <param name="p_CodeId"></param> 
        /// <returns></returns> 
        private string GetValue(int p_CodeId)
        {
            DataRow[] _Row = code128Table.Select("ID='" + p_CodeId.ToString() + "'");
            if (_Row.Length != 1) 
                throw new Exception("验效位的编码错误" + p_CodeId.ToString());
            return _Row[0]["BandCode"].ToString();
        }

        /// <summary> 
        /// 获得条码图形 
        /// </summary> 
        /// <param name="Code128">文字</param> 
        /// <returns>图形</returns> 
        private Bitmap GetImage(string Code128)
        {
            char[] code128 = Code128.ToCharArray();
            int codeWidth = 0;
            for (int i = 0; i != code128.Length; i++)
            {
                codeWidth += Int32.Parse(code128[i].ToString()) * (WidthMultiple + 1);
            }
            Bitmap codeBitmap = new Bitmap(codeWidth, (int)HeightImage);
            Graphics codeGarphics = Graphics.FromImage(codeBitmap);
            int _LenEx = 0;
            for (int i = 0; i != code128.Length; i++)
            {
                int iValueNumb = Int32.Parse(code128[i].ToString()) * (WidthMultiple + 1);  //获取宽和放大系数 
                if (!((i & 1) == 0))
                {
                    codeGarphics.FillRectangle(Brushes.White, new Rectangle(_LenEx, 0, iValueNumb, (int)HeightImage));
                }
                else
                {
                    codeGarphics.FillRectangle(Brushes.Black, new Rectangle(_LenEx, 0, iValueNumb, (int)HeightImage));
                }
                _LenEx += iValueNumb;
            }
            codeGarphics.Dispose();
            return codeBitmap;
        }
        /// <summary> 
        /// 显示可见条码文字 如果小于40 不显示文字 
        /// </summary> 
        /// <param name="bitmap">图形</param>            
        private void GetViewText(Bitmap bitmap, string title)
        {
            if (TitleFont == null) return;

            Graphics graphics = Graphics.FromImage(bitmap);
            SizeF drawSize = graphics.MeasureString(title, TitleFont);
            //if (_DrawSize.Height > p_Bitmap.Height - 10 || _DrawSize.Width > p_Bitmap.Width)
            //{
            //    _Graphics.Dispose();
            //    return;
            //}

            int starY = bitmap.Height - (int)drawSize.Height;
            graphics.FillRectangle(Brushes.White, new Rectangle(0, starY, bitmap.Width, (int)drawSize.Height));
            graphics.DrawString(title, TitleFont, Brushes.Black, (bitmap.Width - drawSize.Width) / 2, starY);
        }

        public static byte[] GetBarcode(string barcode)
        {
            byte[] btBarcode = new byte[] { };
            if (!string.IsNullOrEmpty(barcode))
            {
                using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
                {
                    BarCode128 barCode128 = new BarCode128();
                    barCode128.TitleFont = new System.Drawing.Font("宋体", 10);
                    barCode128.HeightImage = 50;
                    System.Drawing.Bitmap bitmap = barCode128.GetCodeImage(barcode, barcode, Code.BarCode128.Encode.Code128B);
                    bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Gif);
                    btBarcode = memoryStream.ToArray();
                    bitmap.Dispose();
                }
            }
            return btBarcode;
        }
    }
}