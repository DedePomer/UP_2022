using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UP_2022
{
    public partial class Material
    { 
        public string PhotoPath
        {
            get 
            {
                if (Image == null)
                {
                    return "..\\materials\\picture.png";
                }
                    return ".." + Image;
            }
        }

        public string TitleBox
        {
            get
            {
                List<MaterialType> TypeName = FolderClasses.BD.Data.MaterialType.ToList();
                string TypeNameStr = TypeName[MaterialTypeID-1].Title;
                return TypeNameStr+" | "+ Title;
            }
        }

        public string SuplerStr
        {
            get
            {
                List <Supplier> Sup = FolderClasses.BD.Data.Supplier.ToList();
                List<MaterialSupplier> MaterialSup = FolderClasses.BD.Data.MaterialSupplier.ToList();

                int id = ID;
                if (id == 7)
                {
                    id = ID;
                }
                List<MaterialSupplier> idSup = MaterialSup.Where(x => x.MaterialID == id).ToList();
                string OutStr = "";
                for (int i = 0; i < idSup.Count; i++)
                {
                    if (i % 3 == 0 && i != 0)
                    {
                        OutStr += "\n" + Sup[idSup[i].SupplierID].Title + ";";
                    }
                    else
                    {
                        OutStr += " " + Sup[idSup[i].SupplierID].Title + ";";
                    }
                    
                }
                if (OutStr != "")
                {
                    OutStr = OutStr.Substring(0, OutStr.Length - 2);
                }
                else
                {
                    OutStr = " нет";
                }
               
                return OutStr;
            }
        }

        public string StockStr
        {
            get
            {
                return "Остаток: " + CountInStock + " " + Unit;
            }
        }


    }
}
