using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace GMP.Helper
{
    public class MathHelper
    {
        public static double Evaluate(string expression)
        {

            var loDataTable = new DataTable();
            var loDataColumn = new DataColumn("Eval", typeof(double), expression);
            loDataTable.Columns.Add(loDataColumn);
            loDataTable.Rows.Add(0);

            double value = (double)(loDataTable.Rows[0]["Eval"]);

            loDataTable.Dispose();
            loDataColumn.Dispose();

            return value;
        } 
    }
}
