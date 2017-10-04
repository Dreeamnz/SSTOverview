
using HDF5DotNet;
using SSTOverview.Model;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SSTOverview.HDFManipulator
{
    public class Reader
    {
        private string _directory;
        private string _groupName;
        private string _dataSetName;

        public Reader(string directory, string groupName, string dataSetName)
        {
            _directory = directory;
            _groupName = groupName;
            _dataSetName = dataSetName;
        }

        public List<HDFContent> ReadHDF5()
        {
            var hdfList = new List<HDFContent>();

            string[] files = Directory.GetFiles(_directory);

            if (files.Length > 0)
            {
                foreach (var file in files)
                {
                    var hdfContent = new HDFContent();

                    H5.Open();
                    H5FileId fileID = H5F.open(file, H5F.OpenMode.ACC_RDONLY);
                    H5GroupId groupID = H5G.open(fileID, _groupName);
                    H5DataSetId datasetID = H5D.open(groupID, _dataSetName);
                    H5DataSpaceId dataSpace = H5D.getSpace(datasetID);
                    long[] dimentions = H5S.getSimpleExtentDims(dataSpace);
                    H5DataTypeId dataType = H5D.getType(datasetID);

                    float[,] dataArray = new float[dimentions[0], dimentions[1]];
                    var wrapArray = new H5Array<float>(dataArray);
                    H5D.read(datasetID, dataType, wrapArray);

                    hdfContent.FileName = Path.GetFileNameWithoutExtension(file);
                    hdfContent.Lines = (int)dimentions[0];
                    hdfContent.Columns = (int)dimentions[1];
                    hdfContent.Data = dataArray;

                    hdfList.Add(hdfContent);
                }
            }

            return hdfList;
        }
    }
}
