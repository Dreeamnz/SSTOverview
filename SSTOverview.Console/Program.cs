using SSTOverview.HDFManipulator;
using SSTOverview.Model;
using SSTOverview.Model.DTO;
using SSTOverview.Util;
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace SSTOverview.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            CultureInfo culture = new CultureInfo(ConfigurationManager.AppSettings["DefaultCulture"]);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            int oscilator = 4;
            Reader reader = new Reader(ConfigurationManager.AppSettings["HDF_DIRECTORY"], ConfigurationManager.AppSettings["HDF_GROUP"], ConfigurationManager.AppSettings["HDF_DATASET"]);
            List<HDFContent> hdfList = reader.ReadHDF5();
            Coordinates coordinates = null;

            Presentation();

            do
            {
                Console.WriteLine(Environment.NewLine);
                coordinates = InputRead();
            } while (coordinates == null);

            int fileNumber = 0;
            int maxFiles = hdfList.Count;
            foreach (var hdf in hdfList)
            {
                fileNumber++;

                ExcelWriter excelWriter = new ExcelWriter("C:\\Desenvolvimento\\Excel", hdf.FileName, "XLS");
                excelWriter.CreateSheet("SST");

                BaseProgress(fileNumber, maxFiles, hdf.FileName);

                int excelRow = 0,
                    excelColumn;
                for (int row = GeographicFunctions.LatitudeToTableRow(coordinates.NorthEastLatitude, oscilator); 
                            row < GeographicFunctions.LatitudeToTableRow(coordinates.SouthWestLatitude, oscilator); row++)
                {
                    excelColumn = 0;
                    for (int column = GeographicFunctions.LongitudeToTableColumn(coordinates.NorthEastLongitude, oscilator); 
                             column < GeographicFunctions.LongitudeToTableColumn(coordinates.SouthWestLongitude, oscilator); column++)
                    {
                        excelWriter.CreateCellValue("SST", excelRow, excelColumn, hdf.Data[row, column].ToString());
                        UpdateProgress(PercentProcessConvertCalc((row + 1), (column + 1), hdf.Lines, hdf.Columns));
                        excelColumn++;
                    }
                    excelRow++;
                }

                excelWriter.SaveExcelFile();
            }

            ConcludeProcess();
        }

        static void Presentation()
        {
            string presentation = string.Format(
                @"***************************************************************************************
********************************** Let's cut this! ***********************************
**************************************************************************************");
            Console.Write(presentation + Environment.NewLine);
            
        }

        static void BaseProgress(int fileNumber, int maxFiles, string fileName)
        {
            Console.Clear();
            Console.WriteLine(string.Format("Processing file {0}/{1}", fileNumber, maxFiles));
            Console.WriteLine(string.Format("{0}", fileName));
        }

        static void UpdateProgress(float filePercentProcessed)
        {
            Console.Write(string.Format("\r{0}%", filePercentProcessed.ToString()));
        }

        static void ConcludeProcess()
        {
            Console.Clear();
            Console.WriteLine("Process done!");
            Console.WriteLine("Hit any key to exit.");
            Console.ReadKey();
        }

        static Coordinates InputRead()
        {
            try
            {
                Coordinates coordinates = new Coordinates();

                Console.WriteLine("Inform NE latitude: ");
                coordinates.NorthEastLatitude = TryConvert();
                
                Console.WriteLine("Inform NE longitude: ");
                coordinates.NorthEastLongitude = TryConvert();

                Console.WriteLine("Inform SW latitude: ");
                coordinates.SouthWestLatitude = TryConvert();

                Console.WriteLine("Inform SW longitude: ");
                coordinates.SouthWestLongitude = TryConvert();

                return coordinates;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        static int PercentProcessConvertCalc(int row, int column, int maxRows, int maxColumns)
        {
            return (((maxColumns * row) - maxColumns) + column) * 100 / (maxRows * maxColumns);
        }

        static float TryConvert()
        {
            float convert = 0;
            if (!float.TryParse(Console.ReadLine(), out convert))
                throw new Exception("Inform a decimal number using a floating point if necessary.");

            return convert;
        }
    }
}
