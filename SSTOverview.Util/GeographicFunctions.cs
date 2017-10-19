namespace SSTOverview.Util
{
    public class GeographicFunctions
    {
        /// <summary>
        /// Convert a latitude to a specific table row, according to max numbers of rows and a oscilator set.
        /// </summary>
        /// <param name="latitude">Specific point on earth that varies between -90/90</param>
        /// <param name="maxTableRows">Total number of rows of the table</param>
        /// <param name="oscilator">decree oscilator of each row. Ex: oscilator = 1, tableRow[0] = -90, tableRow[1] = -89</param>
        public static int LatitudeToTableRow(float latitude, float oscilator)
        {
            return (int)((latitude * oscilator) + (90 * oscilator));
        }

        /// <summary>
        /// Convert a longitude to a specific table row, according to max numbers of rows and a oscilator set.
        /// </summary>
        /// <param name="latitude">Specific point on earth that varies between -180/180</param>
        /// <param name="maxTableRows">Total number of rows of the table</param>
        /// <param name="oscilator">decree oscilator of each row. Ex: oscilator = 1, tableRow[0] = -180, tableRow[1] = -179</param>
        public static int LongitudeToTableColumn(float longitude, float oscilator)
        {
            return (int)((longitude * oscilator) + (180 * oscilator));
        }
    }
}
