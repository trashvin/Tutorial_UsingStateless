using System;
using Xunit;

using AssetWorkflow;
using System.Collections.Generic;



namespace Stateless.Tests
{

    public class AssetWorkflowTests : IDisposable
    {
        private List<Person> _owners;
        private List<Asset> _assets;

        public AssetWorkflowTests()
        {
            LoadTestData();
        }

        private void LoadTestData()
        {
            Asset testAsset1 = new Asset(new AssetInformation(1, "Test 1"));
            Asset testAsset2 = new Asset(new AssetInformation(2, "Test 2"));
            Asset testAsset3 = new Asset(new AssetInformation(3, "Test 3"));
            Asset testAsset4 = new Asset(new AssetInformation(4, "Test 4"));

            Person owner1 = new Person(1, "Test", "test@test.com");
            Person owner2 = new Person(2, "QA", "qa@test.com");

            _owners = new List<Person>();
            _assets = new List<Asset>();

            _owners.Add(owner1);
            _owners.Add(owner2);

            _assets.Add(testAsset1);
            _assets.Add(testAsset2);
            _assets.Add(testAsset3);
            _assets.Add(testAsset4);
        }

        public void Dispose()
        {
        }

        [Fact]
        public void FinishedTestingTriggerTest()
        {
            LoadTestData();

            Asset toTest = _assets[0];

            toTest.AssetState = Asset.State.New;
            toTest.FinishedTesting();

            Assert.Equal(Asset.State.Available, toTest.AssetState);
        }

        [Theory]
        [InlineData(0, 1)]
        [InlineData(1, 2)]
        public void AssignTriggerTest(int ownerIndex, int expectedID)
        {
            LoadTestData();

            Asset toTest = _assets[0];

            toTest.AssetState = Asset.State.Available;
            toTest.Assign(_owners[ownerIndex]);

            Assert.Equal(Asset.State.Allocated, toTest.AssetState);
            Assert.Equal(expectedID, toTest.AssetData.Owner.PersonID);
        }

        [Theory]
        [InlineData(0, 1, 2)]
        [InlineData(1, 0, 1)]
        public void TransferTriggerTest(int oldOwner, int newOwner, int expectedID)
        {
            LoadTestData();

            Asset toTest = _assets[0];
            toTest.AssetState = Asset.State.Available;

            toTest.Assign(_owners[oldOwner]);
            toTest.Transfer(_owners[newOwner]);

            Assert.Equal(Asset.State.Allocated, toTest.AssetState);
            Assert.Equal(expectedID, toTest.AssetData.Owner.PersonID);
        }


        //TODO : Add more test scenario
        [Theory]
        [InlineData(Asset.State.New, Asset.Trigger.Released)]
        [InlineData(Asset.State.New, Asset.Trigger.Discarded)]
        [InlineData(Asset.State.New, Asset.Trigger.Found)]
        [InlineData(Asset.State.New, Asset.Trigger.Repaired)]
        [InlineData(Asset.State.New, Asset.Trigger.RequestRepair)]
        [InlineData(Asset.State.New, Asset.Trigger.RequestUpdate)]
        [InlineData(Asset.State.New, Asset.Trigger.Transferred)]
        public void InvalidTriggerTest(Asset.State initialState, Asset.Trigger trigger)
        {
            LoadTestData();

            Asset toTest = _assets[0];
            toTest.AssetState = initialState;
            toTest.Fire(trigger);

            Assert.True(toTest.AssetState == initialState);


        }
    }
}
