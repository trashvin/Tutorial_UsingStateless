using System;
namespace AssetWorkflow
{
    public class Person
    {
        public Person(int personID, string personName, string emailAddress)
        {
            PersonID = personID;
            PersonName = personName;
            EmailAddress = emailAddress;
        }

        public int PersonID { get; set; }
        public string PersonName { get; set; }
        public string EmailAddress { get; set; }
    }
}
