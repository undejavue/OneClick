using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//using S7HCAX_XLib;
//using S7HCOM_XLib;
//using SimaticLib;


namespace ClassLibrary
{
    /// <summary>
    /// Фрагменты кода для работы с библиотеками STEP7
    /// </summary>
    public class Step7_Works
    {

        public List<mBaseEntity> S7testCode()
        {
            //  Код для работы со Step7
            //Simatic sim = new Simatic();




            //sm.DoImportFromFile()

            //S7HCAX_XLib.ICAXImpExpMgr.DoExportToFile("string", "string", "string", "string", S7HCAX_XLib.EXPORT_FLAGS.S7_EXPORT);

            //CAXImpExpMgr mngr = new CAXImpExpMgr();



            // S7HCAX_XLib.ICAXImpExpMgr.DoImportFromFile(string, string, string, string);

            //IS7Project Proj = sim.Projects.Add(@"C:\MyProject\MyProject.s7p");
            List<mBaseEntity> plist = new List<mBaseEntity>();



            //foreach (S7Project p in sim.Projects)
            //{
            //    mBaseItem nd = new mBaseItem();
            //    nd.Name = p.Name.ToString();
            //    nd.Description = p.LogPath.ToString();

            //    plist.Add(nd);

            //    if (p.Name.Equals("7675"))
            //    {
            //        List<string> slist = new List<string>();
            //        foreach (S7Station s in p.Stations)
            //        {
            //            slist.Add(s.Name);

            //        }

            //        if (slist.Count > 0)
            //        {
            //            string str = slist[0].ToString();
            //        }
            //        try
            //        {
            //            //mngr.DoExportToFile("c:\\Program Files (x86)\\Siemens\\Step7\\S7Tmp\\export.xml", p.LogPath, p.Name, slist[0].ToString(), S7HCAX_XLib.EXPORT_FLAGS.STEP7_EXPORT);
            //            mngr.DoImportFromFile("c:\\Program Files (x86)\\Siemens\\Step7\\S7Tmp\\PLC2_S7300.xml", p.LogPath, p.Name, "PLC2_S7300");
            //        }
            //        catch (Exception ex)
            //        {
            //            string error = ex.Message.ToString();
            //        }
            //    }

            //}



            return plist;
        }
    }
}
