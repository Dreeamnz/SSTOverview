using SSTOverview.HDFManipulator;
using SSTOverview.Model;
using System;
using System.Collections.Generic;

namespace SSTOverview.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Reader reader = new Reader("C:\\Desenvolvimento\\HDF", "/HDFEOS/GRIDS/NCEP/Data Fields/", "SST");
            List<HDFContent> hdfList = reader.ReadHDF5();

            foreach (var hdf in hdfList)
            {
                ExcelWriter excelWriter = new ExcelWriter("C:\\Desenvolvimento\\Excel", hdf.FileName, "XLS");
                excelWriter.CreateSheet("SST");

                for (int row = 0; row < hdf.Lines; row++)
                {
                    for (int column = 0; column < hdf.Columns; column++)
                    {
                        excelWriter.CreateCellValue("SST", row, column, hdf.Data[row, column].ToString());
                    }
                }

                excelWriter.SaveExcelFile();
            }
        }
    }
}
