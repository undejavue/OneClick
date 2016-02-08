using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary
{
    public class mKey : mBaseEntity
    {
        public int KeyId { get; set; }

        public virtual mCategory mCategory { get; set; }
    }
}
