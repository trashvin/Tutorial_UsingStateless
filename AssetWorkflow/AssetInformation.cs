using System;
namespace AssetWorkflow
{
    public class AssetInformation
    {
        public AssetInformation(int assetID, string assetName = "")
        {
            AssetID = assetID;
            AssetName = assetName;
        }

        public int AssetID { get; set; }
        public string AssetName { get; set; }
        public Person Owner { get; set; }
    }
}
