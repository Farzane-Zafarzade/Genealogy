using System;
using System.Collections.Generic;
using System.Text;
using Genealogy.MyDatabase;

namespace Genealogy.Persons
{
    class person
    {
        private int id;
        private string firstName;
        private string lastName;
        private string birthDate;
        private string deathDate;
        private int motherID;
        private int fatherID;

        public int Id { get => id; set => id = value; }
        public string FirstName { get => firstName; set => firstName = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public string BirthDate { get => birthDate; set => birthDate = value; }
        public string DeathDate { get => deathDate; set => deathDate = value; }
        public int MotherID { get => motherID; set => motherID = value; }
        public int FatherID { get => fatherID; set => fatherID = value; }
    }
}
