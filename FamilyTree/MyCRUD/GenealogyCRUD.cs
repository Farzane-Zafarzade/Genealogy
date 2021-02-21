using System;
using System.Collections.Generic;
using System.Text;
using Genealogy.MyDatabase;
using Genealogy.Persons;
using System.Data;
using System.Collections;

namespace Genealogy.MyCRUD
{
    class GenealogyCRUD : SQLDatabase
    {
        
        public void CheckDatabase()
        {
            SQLDatabase database = new SQLDatabase();
            if (!database.DoesDatabaseExists(databaseName))
            {
                CreateDatabase();
                CreateStartPoint();
            }
        }


        public void CreateStartPoint()
        {

            CreateFmilyTreeTable();
            CreateFathersTable();
            CreateMothersTable();
            CreateChilderenTable();
            CreateParentsTable();
            person firstFather = new person();
            firstFather.FirstName = "Ali";
            firstFather.LastName = "Zafar";
            firstFather.BirthDate = "1933";
            firstFather.DeathDate = "1989";
            CreatePerson(firstFather);
            CreateFather("Ali", "Zafar");

            person firstMother = new person();
            firstMother.FirstName = "Maryam";
            firstMother.LastName = "Rezaei";
            firstMother.BirthDate = "1940";
            firstMother.DeathDate = "1978";
            CreatePerson(firstMother);
            CreateMother("Maryam", "Rezai");
            InsertParents(1,1);
           
            person firstChild = new person();
            firstChild.FirstName = "Karim";
            firstChild.LastName = "Zafar";
            firstChild.BirthDate = "1955";
            firstChild.DeathDate = "";
            firstChild.MotherID = SearchMother("Maryam", "Rezai");
            firstChild.FatherID = SearchFather("Ali", "zafar");
            CreatePerson(firstChild);
            firstChild = SearchPerson("Karim", "Zafar");
            InsertChilderen(firstChild.Id, 1);
            CreateFather("Karim", "zafar");

 
            person secondtMother = new person();
            secondtMother.FirstName = "Marina";
            secondtMother.LastName = "Esmaeel";
            secondtMother.BirthDate = "1946";
            secondtMother.DeathDate = "";
            CreatePerson(secondtMother);
            CreateMother("Marina", "Esmaeel");
            secondtMother = SearchPerson("Marina", "Esmaeel");
            InsertParents(secondtMother.Id, firstChild.Id);

            person secondChild = new person();
            secondChild.FirstName = "Mohammad";
            secondChild.LastName = "Zafar";
            secondChild.BirthDate = "1978";
            secondChild.DeathDate = "";
            secondChild.MotherID = SearchMother("Marina", "Esmaeel");
            secondChild.FatherID = SearchFather("Karim", "zafar");
            CreatePerson(secondChild);
            int parentId = SearchParents(SearchFather("karim","zafar"), SearchMother("marina","esmaeel"));
            secondChild = SearchPerson("Mohammad", "Zafar");
            InsertChilderen(secondChild.Id, parentId);

        }


        public void CreateFmilyTreeTable()
        {
            if (!TableExists("FamilyTree"))
            {
                string commandStr = "CREATE TABLE FamilyTree (ID int IDENTITY(1, 1) PRIMARY KEY, First_Name char(50), Last_Name char(50), Birth_Date char(50), Death_Date char(50), MotherID int, FatherID int); ";

                ExecuteSQL(commandStr);

            }

        }


        public void CreateFathersTable()
        {
            if (!TableExists("Fathers"))
            {
                string commandStr = "CREATE TABLE Fathers (FatherID int IDENTITY(1, 1) PRIMARY KEY, First_Name char(50), Last_Name char(50)); ";

                ExecuteSQL(commandStr);

            }
        }

        public void CreateMothersTable()
        {
            if (!TableExists("Mothers"))
            {
                string commandStr = "CREATE TABLE Mothers (MotherID int IDENTITY(1, 1) PRIMARY KEY, First_Name char(50), Last_Name char(50)); ";

                ExecuteSQL(commandStr);

            }
        }

        public void CreateChilderenTable()
        {
            if (!TableExists("Childeren"))
            {
                string commandStr = "CREATE TABLE Childeren (ChildID int NOT NULL PRIMARY KEY,ParentID int); ";

                ExecuteSQL(commandStr);

            }
        }

        public void CreateParentsTable()
        {
            if (!TableExists("Parents"))
            {
                string commandStr = "CREATE TABLE Parents (ParentID int IDENTITY(1, 1) PRIMARY KEY,MotherID int , FatherID int); ";

                ExecuteSQL(commandStr);

            }
        }

        public void InsertChilderen(int ID, int parentID)
        {
            string CommandString = "INSERT INTO Childeren (ChildID ,ParentID) VALUES (@ChildID,@ParentID);";
            string[] paramNames = { "@ChildID", "@ParentID" };
            var paramValues = new ArrayList() { ID, parentID };
            ExecuteSQL(CommandString, paramNames, paramValues);
        }


        public void InsertParents(int motherID, int fatherID)
        {
            string CommandString = "INSERT INTO Parents (MotherID ,FatherID) VALUES (@MotherID,@FatherID);";
            string[] paramNames = { "@MotherID", "@FatherID" };
            var paramValues = new ArrayList() { motherID, fatherID };
            ExecuteSQL(CommandString, paramNames, paramValues);
        }


        public void CreatePerson(person person)
        {
            string commandString = @"INSERT INTO FamilyTree (First_Name,Last_Name,Birth_Date,Death_Date,MotherID,FatherID) values (@First_Name,@Last_Name,@Birth_Date,@Death_Date,@MotherID,@FatherID);";
            string[] paramNames = { "@First_Name", "@Last_Name", "@Birth_Date", "@Death_Date", "@MotherID", "@FatherID" };
            var paramValues = new ArrayList() { person.FirstName, person.LastName, person.BirthDate, person.DeathDate, person.MotherID, person.FatherID };
            ExecuteSQL(commandString, paramNames, paramValues);
        }


        public void CreateMother(string fName, string lName)
        {
            string CommandString = "INSERT INTO Mothers (First_Name ,Last_Name) VALUES (@First_Name,@Last_Name);";
            string[] paramNames = { "@First_Name", "@Last_Name" };
            var paramValues = new ArrayList() { fName, lName };
            ExecuteSQL(CommandString, paramNames, paramValues);
        }


        public void CreateFather(string fName, string lName)
        {
            CreateFathersTable();
            string CommandString = "INSERT INTO Fathers (First_Name ,Last_Name) VALUES (@First_Name,@Last_Name);";
            string[] paramNames = { "@First_Name", "@Last_Name" };
            var paramValues = new ArrayList() { fName, lName };
            ExecuteSQL(CommandString, paramNames, paramValues);
        }


        public void DeletePerson(person person)
        {
            string CommandString = "DELETE FROM FamilyTree WHERE ID =@ID;";
            string[] paramNames = { "@ID" };
            var paramValues = new ArrayList() { person.Id };
            ExecuteSQL(CommandString, paramNames, paramValues);
        }

        public static bool DoesPersonExits(int ID)
        {
            SQLDatabase database = new SQLDatabase();
            DataTable dt = new DataTable();
            string sqlCommand = "SELECT * from FamilyTree WHERE ID = @ID;";
            string[] paramNames = { "@ID" };
            var paramValues = new ArrayList() { ID };

            dt = database.Read(sqlCommand, paramNames, paramValues);
            if (dt.Rows.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        public static bool DoesPersonExits(string firstName, string lastName)
        {
            SQLDatabase database = new SQLDatabase();
            DataTable dt = new DataTable();
            string sqlCommand = "SELECT * from FamilyTree WHERE First_Name = @First_Name AND Last_Name=@Last_Name;";
            string[] paramNames = { "@First_Name", "@Last_Name" };
            var paramValues = new ArrayList() { firstName, lastName };
            dt = database.Read(sqlCommand, paramNames, paramValues);
            if (dt.Rows.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        public static bool DoesMotherExits(int ID)
        {
            SQLDatabase database = new SQLDatabase();
            DataTable dt = new DataTable();
            string sqlCommand = "SELECT * from Mothers WHERE MotherID = @ID;";
            string[] paramNames = { "@ID" };
            var paramValues = new ArrayList() { ID };

            dt = database.Read(sqlCommand, paramNames, paramValues);
            if (dt.Rows.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        public static bool DoesMothersExits(string firstName, string lastName)
        {
            SQLDatabase database = new SQLDatabase();
            DataTable dt = new DataTable();
            string sqlCommand = "SELECT * from Mothers WHERE First_Name = @First_Name AND Last_Name = @Last_Name;";
            string[] paramNames = { "@First_Name", "@Last_Name" };
            var paramValues = new ArrayList() { firstName, lastName };
            dt = database.Read(sqlCommand, paramNames, paramValues);
            if (dt.Rows.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }



        public static bool DoesFatherExits(int ID)
        {
            SQLDatabase database = new SQLDatabase();
            DataTable dt = new DataTable();
            string sqlCommand = "SELECT * from Fathers WHERE FatherID = @ID;";
            string[] paramNames = { "@ID" };
            var paramValues = new ArrayList() { ID };

            dt = database.Read(sqlCommand, paramNames, paramValues);
            if (dt.Rows.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }



        public static bool DoesFatherExits(string firstName, string lastName)
        {
            SQLDatabase database = new SQLDatabase();
            DataTable dt = new DataTable();
            string sqlCommand = "SELECT * from Fathers WHERE First_Name = @First_Name AND Last_Name = @Last_Name;";
            string[] paramNames = { "@First_Name", "@Last_Name" };
            var paramValues = new ArrayList() { firstName, lastName };
            dt = database.Read(sqlCommand, paramNames, paramValues);
            if (dt.Rows.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }



        public void GetMother(person person)
        {
            person mother = new person();

            if (DoesPersonExits(person.Id))//check if finns i person table
            {
                if (DoesMotherExits(person.MotherID))
                {
                    mother = SearchMother(person.MotherID);
                    Console.Write(" Mother's name is :");
                    Console.Write(mother.FirstName);
                    Console.Write(mother.LastName);
                }
                else
                {
                    Console.WriteLine(" Mother is not found! ");
                }
            }
            
        }


        public void GetFather(person person)
        {
            person father = new person();

            if (DoesPersonExits(person.Id))//check if finns i person table
            {
                if (DoesFatherExits(person.FatherID))
                {
                    father = SearchFather(person.FatherID);
                    Console.Write(" Father's name is :");
                    Console.Write(father.FirstName);
                    Console.Write(father.LastName);
                }
                else
                {
                    Console.WriteLine(" Father is not found! ");
                }
            }

        }



        public static void List(string filter, ArrayList paramValue)
        {
            SQLDatabase database = new SQLDatabase();
            string[] parmName = { "@input" };
            DataTable dt = new DataTable();
            string sqlCommand = "SELECT  First_Name , Last_Name FROM FamilyTree WHERE " + filter;
            dt = database.Read(sqlCommand, parmName, paramValue);

            foreach (DataRow row in dt.Rows)
            {
                foreach (var item in row.ItemArray)
                {
                    Console.Write(item);
                }
                Console.WriteLine();
            }

        }

        public static void List()
        {
            SQLDatabase database = new SQLDatabase();
            string[] parmName = { "@First_Name", "@Last_Name" };
            var paramValues = new ArrayList() { "First_Name", "Last_Name" };
            DataTable dt = new DataTable();
            string sqlCommand = "select First_Name , Last_Name from FamilyTree inner join Childeren on FamilyTree.ID=Childeren.ChildID;";
            dt = database.Read(sqlCommand, parmName, paramValues);

            foreach (DataRow row in dt.Rows)
            {
                foreach (var item in row.ItemArray)
                {
                    Console.Write(item);
                }
                Console.WriteLine();
            }

        }



        public void UpdatePerson(person person)
        {
            string sqlCommand = "UPDATE FamilyTree SET First_Name = @First_Name ,  Last_Name = @Last_Name ," +
                                                " Birth_Date =@Birth_Date , Death_Date = @Death_Date ," +
                                                " MotherID=@MotherID,FatherID=@FatherID WHERE ID=@ID;";

            string[] paramNames = { "@First_Name", "@Last_Name", "@Birth_Date", "@Death_Date", "@MotherID", "@FatherID", "@ID" };

            var paramValues = new ArrayList() { person.FirstName, person.LastName,
                                                person.BirthDate, person.DeathDate,
                                                person.MotherID, person.FatherID ,person.Id};

            ExecuteSQL(sqlCommand, paramNames, paramValues);

        }


        public int SearchParents(int fatderId, int motherId)
        {
            SQLDatabase database = new SQLDatabase();
            int parentID = 0;
            DataTable dt = new DataTable();
            string sqlCommand = "SELECT ParentID from parents WHERE MotherID = @MotherID AND FatherID= @FatherID;";
            string[] paramNames = { "@MotherID", "@FatherID" };
            var paramValues = new ArrayList() { motherId, fatderId };

            dt = database.Read(sqlCommand, paramNames, paramValues);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    parentID = (int)row["ParentID"];
                }
            }

            return parentID;

        }


        public static person SearchFather(int id)
        {
            SQLDatabase database = new SQLDatabase();
            person father = new person();
            DataTable dt = new DataTable();
            string sqlCommand = "SELECT First_Name,Last_Name from Fathers WHERE FatherID = @ID;";
            string[] paramNames = { "@ID" };
            var paramValues = new ArrayList() { id };

            dt = database.Read(sqlCommand, paramNames, paramValues);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    father.FirstName = row["First_Name"].ToString();
                    father.LastName = row["Last_Name"].ToString();
                }
            }
            return father;

        }


        public static int SearchFather(string fName, string lName)
        {
            SQLDatabase database = new SQLDatabase();
            int fID = 0;
            DataTable dt = new DataTable();
            string sqlCommand = "SELECT FatherID from Fathers WHERE First_Name = @First_Name AND Last_Name= @Last_Name;";
            string[] paramNames = { "@First_Name", "@Last_Name" };
            var paramValues = new ArrayList() { fName, lName };

            dt = database.Read(sqlCommand, paramNames, paramValues);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    fID = (int)row["FatherID"];
                }
            }

            return fID;

        }

        public static person SearchMother(int id)
        {
            SQLDatabase database = new SQLDatabase();
            person mother = new person();
            DataTable dt = new DataTable();
            string sqlCommand = "SELECT * from Mothers WHERE MotherID = @ID;";
            string[] paramNames = { "@ID" };
            var paramValues = new ArrayList() { id };

            dt = database.Read(sqlCommand, paramNames, paramValues);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];

                    mother.FirstName = row["First_Name"].ToString();
                    mother.LastName = row["Last_Name"].ToString();
                }
            }
            return mother;

        }

        public static int SearchMother(string fName, string lName)
        {
            SQLDatabase database = new SQLDatabase();
            int mID = 0;
            DataTable dt = new DataTable();
            string sqlCommand = "SELECT * from Mothers WHERE First_Name = @First_Name AND Last_Name= @Last_Name;";
            string[] paramNames = { "@First_Name", "@Last_Name" };
            var paramValues = new ArrayList() { fName, lName };

            dt = database.Read(sqlCommand, paramNames, paramValues);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    mID = (int)row["MotherID"];


                }
            }

            return mID;

        }

        public static person SearchPerson(int ID)

        {
            SQLDatabase database = new SQLDatabase();
            person myPerson = new person();
            DataTable dt = new DataTable();
            string sqlCommand = "SELECT * from FamilyTree WHERE ID = @ID;";
            string[] paramNames = { "@ID" };
            var paramValues = new ArrayList() { ID };

            dt = database.Read(sqlCommand, paramNames, paramValues);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    myPerson.Id = (int)row["ID"];
                    myPerson.FirstName = row["First_Name"].ToString();
                    myPerson.LastName = row["Last_Name"].ToString();
                    myPerson.BirthDate = row["Birth_Date"].ToString();
                    myPerson.DeathDate = row["Death_Date"].ToString();
                    myPerson.MotherID = (int)row["MotherID"];
                    myPerson.FatherID = (int)row["FatherID"];
                }
            }

            return myPerson;
        }

        public static person SearchPerson(string fName, string lName)

        {
            SQLDatabase database = new SQLDatabase();
            person myPerson = new person();
            DataTable dt = new DataTable();
            string sqlCommand = "SELECT * from FamilyTree WHERE First_Name = @First_Name AND Last_Name = @Last_Name;";
            string[] paramNames = { "@First_Name", "@Last_Name" };
            var paramValues = new ArrayList() { fName, lName };

            dt = database.Read(sqlCommand, paramNames, paramValues);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    myPerson.Id = (int)row["ID"];
                    myPerson.FirstName = row["First_Name"].ToString();
                    myPerson.LastName = row["Last_Name"].ToString();
                    myPerson.BirthDate = row["Birth_Date"].ToString();
                    myPerson.DeathDate = row["Death_Date"].ToString();
                    myPerson.MotherID = (int)row["MotherID"];
                    myPerson.FatherID = (int)row["FatherID"];
                }
            }
            return myPerson;

        }




    }
}





