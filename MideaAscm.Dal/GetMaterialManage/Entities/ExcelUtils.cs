using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Net.SourceForge.Koogra.Excel;
using System.IO;
using System.Data;

namespace MideaAscm.Dal.GetMaterialManage.Entities
{
    /// <summary>
    /// Excel工具类
    /// </summary>
    public class ExcelUtils
    {
        private Workbook book;

        public ExcelUtils(string path)
        {
            this.book = new Workbook(path);
        }

        public ExcelUtils(Stream stream)
        {
            this.book = new Workbook(stream);
        }

        protected DataTable SaveAsDataTable(Worksheet sheet)
        {
            DataTable dt = new DataTable();

            uint minRow = sheet.Rows.MinRow;
            uint maxRow = sheet.Rows.MaxRow;

            Row firstRow = sheet.Rows[minRow];

            uint minCol = firstRow.Cells.MinCol;
            uint maxCol = firstRow.Cells.MaxCol;

            for (uint i = minCol; i <= maxCol; i++)
            {
                dt.Columns.Add(firstRow.Cells[i].FormattedValue());
            }

            for (uint i = minRow + 1; i <= maxRow; i++)
            {
                Row row = sheet.Rows[i];

                if (row != null)
                {
                    DataRow dr = dt.NewRow();

                    for (uint j = minCol; j <= maxCol; j++)
                    {
                        Cell cell = row.Cells[j];

                        if (cell != null)
                        {
                            dr[Convert.ToInt32(j)] = cell.Value != null ? cell.Value.ToString() : string.Empty;
                        }
                    }
                    dt.Rows.Add(dr);
                }
            }

            return dt;
        }

        public DataTable ToDataTable(int index)
        {
            Worksheet sheet = this.book.Sheets[index];

            if (sheet == null)
            {
                throw new ApplicationException(string.Format("索引[{0}]所指定的电子表格不存在!", index));
            }

            return this.SaveAsDataTable(sheet);
        }

        public DataTable ToDataTable(string sheetName)
        {
            Worksheet sheet = this.book.Sheets.GetByName(sheetName);

            if (sheet == null)
            {
                throw new ApplicationException(string.Format("名称[{0}]所指定的电子表格不存在！", sheetName));
            }

            return this.SaveAsDataTable(sheet);
        }

        #region 静态方法

        /// <summary>
        /// 单元格格式为日期时间，使用此方法转换为DateTime类型，若解析失败则返回‘0001-01-01’
        /// </summary>
        public static DateTime ParseDateTime(string cellValue)
        {
            DateTime date = default(DateTime);

            double value = default(double);

            if (double.TryParse(cellValue, out value))
            {
                date = DateTime.FromOADate(value);
            }
            else
            {
                DateTime.TryParse(cellValue, out date);
            }

            if (date.ToString() == "0001-1-1 00:00:00")
            {
                string yearString = cellValue.Substring(cellValue.LastIndexOf('/') + 1, cellValue.Length - cellValue.LastIndexOf('/') - 1);
                string monthString = cellValue.Substring(0, cellValue.IndexOf('/'));
                string dayString = cellValue.Substring(cellValue.IndexOf('/') + 1, cellValue.LastIndexOf('/') - cellValue.IndexOf('/') - 1);
                

                string nowString = DateTime.Now.Year.ToString();

                string dateString = nowString.Substring(0, nowString.Length - yearString.Length) + yearString + "-" + monthString + "-" + dayString;

                DateTime newDate = Convert.ToDateTime(dateString);
                date = newDate;
            }

            return date;
        }

        /// <summary>
        /// 转换为DataTable(文件路径+表名)
        /// </summary>
        public static DataTable TranslateToTable(string path, string sheetName)
        {
            ExcelUtils utils = new ExcelUtils(path);
            return utils.ToDataTable(sheetName);
        }

        /// <summary>
        /// 转换为DataTable(文件路径+表索引)
        /// </summary>
        public static DataTable TranslateToTable(string path, int sheetIndex)
        {
            ExcelUtils utils = new ExcelUtils(path);
            return utils.ToDataTable(sheetIndex);
        }

        /// <summary>
        /// 转换为DataTable(文件路径)
        /// </summary>
        public static DataTable TranslateToTable(string path)
        {
            ExcelUtils utils = new ExcelUtils(path);
            return utils.ToDataTable(0);
        }

        /// <summary>
        /// 转换为DataTable(内存流+表名)
        /// </summary>
        public static DataTable TranslateToTable(System.IO.Stream stream, string sheetName)
        {
            ExcelUtils utils = new ExcelUtils(stream);
            return utils.ToDataTable(sheetName);
        }

        /// <summary>
        /// 转换为DataTable(内存流+表索引)
        /// </summary>
        public static DataTable TranslateToTable(System.IO.Stream stream, int sheetIndex)
        {
            ExcelUtils utils = new ExcelUtils(stream);
            return utils.ToDataTable(sheetIndex);
        }

        /// <summary>
        /// 转换为DataTable(内存流)
        /// </summary>
        public static DataTable TranslateToTable(System.IO.Stream stream)
        {
            ExcelUtils utils = new ExcelUtils(stream);
            return utils.ToDataTable(0);
        }

        #endregion
    }
}
