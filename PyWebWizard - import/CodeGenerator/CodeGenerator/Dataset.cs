using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Collections;
using System.Security.Cryptography;

namespace CodeGenerator
{
    [Serializable]
    public class Dataset
    {
        private string datasetname1;
        private Table table1;
        private string dstype1;
        private ArrayList filterslist1;

        public Dataset(string datasetnametemp, string dstypetemp)
        {
            datasetname1 = datasetnametemp;
            dstype1 = dstypetemp;
            filterslist1 = new ArrayList();
        }

        public Dataset(BinaryReader reader1)
        {
            filterslist1 = new ArrayList();
            datasetname1 = reader1.ReadString().ToString();
            dstype1 = reader1.ReadString().ToString();
            for (int i = 0; i < reader1.ReadInt32(); i++)
            {
                filterslist1.Add(new FilterDesc(reader1));
            }
            SetTable(new Table(reader1));
        }


        public void SaveDataset(BinaryWriter writer1)
        {
            writer1.Write(datasetname1);
            writer1.Write(dstype1);
            writer1.Write(filterslist1.Count);
            for (int i2 = 0; i2 < filterslist1.Count; i2++)
            {
                FilterDesc filter1 = (FilterDesc)filterslist1[i2];
                filter1.SaveFilter(writer1);
            }
            table1.SaveTable(writer1);
        }

        public bool isQuery()
        {
            if (dstype1 == "EXIST")
            {
                return true;
            }
            return false;
        }

        public void SetDatasetName(string datasetnametemp)
        {
            datasetname1 = datasetnametemp;
        }

        public void SetTable(Table tabletemp)
        {
            if (dstype1 == "EXIST")
            {
                table1 = tabletemp;
            }
            else
            {
                table1 = tabletemp;
            }
        }

        public Table GetTable()
        {
            return table1;
        }

        public string GetName()
        {
            return datasetname1;
        }

        public ArrayList GetFilters()
        {
            return filterslist1;
        }

        public void SetFilters(ArrayList filterslisttemp1)
        {
            filterslist1 = filterslisttemp1;
        }

        public void ClearFilters()
        {
            filterslist1.Clear();
        }
    }

}
