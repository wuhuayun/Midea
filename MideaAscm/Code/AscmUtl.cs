using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Text.RegularExpressions;

namespace MideaAscm.Code
{
    public class AscmUtl
    {
        private static AscmUtl ascmUtl;
        public static AscmUtl GetInstance()
        {
            if (ascmUtl == null)
                ascmUtl = new AscmUtl();
            return ascmUtl;
        }

		public static string Now 
		{
			get 
			{
				return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
			}
		}

        /// <summary>
        /// 将字母转换成数字
        /// </summary>
        /// <param name="letter">单个字母</param>
        /// <returns>返回转换后的数字</returns>
        public int ConvertLetterToInt(string letter)
        {
            byte[] array = new byte[1];
            array = System.Text.Encoding.ASCII.GetBytes(letter);
            int result = Convert.ToInt32(array[0]);
            return result;
        }
        /// <summary>
        /// 将数字转换成字母
        /// </summary>
        /// <param name="number">整数(a-z：97-122，A-Z：65-90)</param>
        /// <returns>返回转换后的字母</returns>
        public string ConvertIntToLetter(int number)
        {
            byte[] array = new byte[1];
            array[0] = (byte)(number); 
            string result = System.Text.Encoding.ASCII.GetString(array);
            return result;
        }

        public string GetQueryConditionByList(List<int> list, string keyWord, int groupCount = 100)
        {
            StringBuilder sbCondition = new StringBuilder();
            if (list != null && list.Count > 0)
            {
                var ieItem = list.Distinct();
                int count = ieItem.Count();
                StringBuilder sbKeyWord = new StringBuilder();
                for (int i = 0; i < count; i++)
                {
                    if (sbKeyWord.Length > 0)
                        sbKeyWord.Append(",");
                    sbKeyWord.Append(ieItem.ElementAt(i));
                    if ((i + 1) % groupCount == 0 || (i + 1) == count)
                    {
                        if (sbKeyWord.Length > 0)
                        {
                            if (sbCondition.Length > 0)
                                sbCondition.Append(" or ");
                            sbCondition.Append(keyWord + " in(" + sbKeyWord.ToString() + ")");
                        }
                        sbKeyWord.Clear();
                    }
                }
            }
            return sbCondition.ToString();
        }
        public string GetQueryConditionByList(List<string> list, string keyWord, int groupCount = 100)
        {
            StringBuilder sbCondition = new StringBuilder();
            if (list != null && list.Count > 0)
            {
                var ieItem = list.Distinct();
                int count = ieItem.Count();
                StringBuilder sbKeyWord = new StringBuilder();
                for (int i = 0; i < count; i++)
                {
                    if (sbKeyWord.Length > 0)
                        sbKeyWord.Append(",");
                    sbKeyWord.Append("'" + ieItem.ElementAt(i) + "'");
                    if ((i + 1) % groupCount == 0 || (i + 1) == count)
                    {
                        if (sbKeyWord.Length > 0)
                        {
                            if (sbCondition.Length > 0)
                                sbCondition.Append(" or ");
                            sbCondition.Append(keyWord + " in(" + sbKeyWord.ToString() + ")");
                        }
                        sbKeyWord.Clear();
                    }
                }
            }
            return sbCondition.ToString();
        }
    }
}