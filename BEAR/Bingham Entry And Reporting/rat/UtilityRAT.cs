using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BEAR.rat
{
    public class UtilityRAT
    {
        protected bool[] rowChanged; ///<an array of rows in the Results Grid used to track rows that need to be saved.*/

        public void SetRowChanged(int totalRows)
        {
            this.rowChanged = new bool[totalRows];
        }
        public bool[] GetRowChanged()
        {
            return this.rowChanged;
        }

    }

}
