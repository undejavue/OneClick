using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace ClassLibrary
{
    public partial class mDataBlock
    {

        public List<string> return_DBinRowForPrint()
        {
            List<string> row = new List<string>();

            row.Add(this.SymbolName);
            row.Add(this.FullName);

            if (this.UDT_Name.Equals("PID"))
                row.Add("FB1");
            else
                row.Add(this.FullName);

            row.Add(this.Title);
            row.Add(this.UDT_Name);
            row.Add(this.MaxArrayIndex.ToString());

            return row;
        }

    }

}
