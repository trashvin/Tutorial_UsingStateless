using System;
using System.Reflection;

using static System.Console;
using System.Collections.Generic;
using System.Text;

namespace AssetWorkflow
{
    class Program
    {
        private static List<Asset> assetList = new List<Asset>();
        public static int counter = 1;

        static void Main(string[] args)
        {

            int choice =0;
            while ( choice != 4)
            {
                choice = ShowMainMenu();
                switch (choice)
                {
                    case 1:
                        WriteLine("Creating asset..");
                        CreateAsset();
                        break;
                    case 2:
                        WriteLine("Managing assets...");
                        ManageAssets();
                        break;
                    case 3:
                        WriteLine("Listing assets...");
                        ListAssets();
                        break;
                    case 4:
                        WriteLine("Exiting.....");
                        break;
                    default:
                        break;
                }
            }


        }

        private static void CreateAsset()
        {
            AssetInformation assInfo = new AssetInformation(counter);
            counter++;
            assInfo.AssetName = "Asset " + assInfo.AssetID.ToString();
            Asset tempAsset = new Asset(assInfo);
            assetList.Add(tempAsset);
        }

        private static void  ListAssets()
        {
            foreach(Asset asset in assetList)
            {
                StringBuilder data = new StringBuilder();
                data.Append(asset.AssetData.AssetID.ToString() + " : ");
                data.Append(asset.AssetData.AssetName + " : ");

                if (asset.AssetData.Owner != null)
                {
                   data.Append(asset.AssetData.Owner.EmailAddress + " : "); 
                }

                data.Append(asset.AssetState.ToString());

                WriteLine(data.ToString());
            }
        }

        private static int ShowMainMenu()
        {
            int choice = 0;
			WriteLine("Asset Manager v1");
			do
            {
				WriteLine("Menu : (1) Create Asset  (2) Manage Asset (3) List Assets (4)Exit");

				string ch = ReadLine();
                Int32.TryParse(ch,out choice);
		    } while (choice < 1 || choice > 4);
            return choice;
        }

        private static void ManageAssets()
        {
            string assetID;
            ListAssets();

            WriteLine("Select asset to manage.");
            Write("Asset ID :");
            assetID = ReadLine();

            string valid = "ABCDEFGHIJK";

            if (IsAssetExist(assetID))
            {
                string choice = "X";
                WriteLine("Asset Management");
                do
                {
                    WriteLine("Menu : (A) Test (B) Assign (C) Repair (D) Upgrade (E) Release (F) Transfer (G) Repaired (H) Discard (I) Lost (J) Found (K) Exit");

                    choice = ReadLine().ToUpper();


                } while (!valid.Contains(choice));
                         
                Asset asset = assetList.Find(i => i.AssetData.AssetID.ToString() == assetID);

                if ( choice == "K")
                {
                    WriteLine("Exiting asset management.");
					Console.WriteLine(asset.GetDOTGraph());

				}
                else
                {
                    ManageAsset(asset, choice);
                }
            }
            else
            {
                WriteLine("Asset not found.");    
            }
        }

        private static void ManageAsset(Asset asset,string action)
        {
            switch(action)
            {
                case "A":
                    asset.FinishedTesting();
                    break;
                case "B":
                    asset.Assign(GetOwner());
                    break;
                case "C":
                    asset.RequestRepair();
                    break;
                case "D":
                    asset.RequestUpdate();
                    break;
                case "E":
                    asset.Release();
                    break;
                case "F":
                    asset.Transfer(GetOwner());
                    break;
                case "G":
                    asset.Repaired();
                    break;
                case "H":
                    asset.Discard();
                    break;
                case "I":
                    asset.Lost();
                    break;
                case "J":
                    asset.Found();
                    break;
                default:
                    break;
            }

            WriteLine($"Success : {asset.IsSuccessful().ToString()}");
        }
        private static Person GetOwner() 
        {
            int id = counter;
            Console.WriteLine("Enter name : ");
            string name = Console.ReadLine();
            string email = name.Replace(" ", "") + "@email.com";
            counter++;
            return new Person(id, name, email);
        }

        private static bool IsAssetExist(string assetID)
        {
            int id = 0;
            Int32.TryParse(assetID,out id);
            return assetList.Exists(i => i.AssetData.AssetID == id);
        }
    }
}
