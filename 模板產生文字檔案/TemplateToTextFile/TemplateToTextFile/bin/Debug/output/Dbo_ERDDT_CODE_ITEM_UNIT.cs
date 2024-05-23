using DB_ERDDT.DB;
using DB_ERDDT.EF;
using DB_ERDDT.XMLModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace DB_ERDDT.XMLModel.Code
{
    [XmlRoot(ElementName = "dbo")]
    public class Dbo_ERDDT_CODE_ITEM_UNIT
    {
        [XmlElement(ElementName = "row")]
        public List<XML_ERDDT_CODE_ITEM_UNIT> row { get; set; }

        /// <summary>
        /// 存檔
        /// </summary>
        public void SaveDB()
        {
            if (row == null)
                return;
            if (row.Count == 0)
                return;
            ERDDT_EFModels db = new ERDDT_EFModels();
            foreach (var item in this.row)
            {   //欄位綁定,綁去EF的模型
                var efModel = MappingHelper.Mapping<ERDDT_CODE_ITEM_UNIT>(item);
                db.Set<ERDDT_CODE_ITEM_UNIT>().AddOrUpdate(efModel);
            }
            db.SaveChanges();
        }
    }
}
