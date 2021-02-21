using System;
using System.Collections.Generic;
using System.Text;
using Genealogy.MyCRUD;
using Genealogy.Persons;
using Genealogy.MyDatabase;
using System.Collections;

namespace Genealogy.MyGenealogy
{
    class FamilyTree
    {
        bool goToMenu = true;
        private GenealogyCRUD myCrud = new GenealogyCRUD();


        public void Start()
        {
            myCrud.CheckDatabase();

            while (goToMenu)
            {
                Console.WriteLine();
                Console.WriteLine("    *** WELCOME TO MY FAMILY TREE PRESENTATION ***");
                Console.WriteLine("=======================================================");
                Console.WriteLine();
                PrintMainMenu();
                Console.WriteLine();
                Console.Write(" > ");
                int input = Convert.ToInt32(Console.ReadLine());
                switch (input)
                {
                    case 1:
                        Console.Clear();
                        ShowMemberDetails();
                        BackToMenu();
                        break;

                    case 2:
                        ShowParents();
                        BackToMenu();
                        break;

                    case 3:
                        AddMember();
                        BackToMenu();
                        break;

                    case 4:
                        DeleteMember();
                        BackToMenu();
                        break;

                    case 5:
                        EditMember();
                        BackToMenu();
                        break;

                    case 6:
                        ListMembers();
                        BackToMenu();
                        break;

                    case 7:
                        goToMenu = false;
                        break;

                }

            }
        }


        public void PrintMainMenu()
        {
            Console.WriteLine(" 1. Show a member's details");
            Console.WriteLine(" 2. Show parents ");
            Console.WriteLine(" 3. Insert a new member to family tree");
            Console.WriteLine(" 4. Delete a member ");
            Console.WriteLine(" 5. Edit a member ");
            Console.WriteLine(" 6. List members");
            Console.WriteLine(" 7. Exit program");
        }

        public void BackToMenu()
        {
            Console.WriteLine();
            Console.WriteLine(" Press Enter to back to menu");
            Console.ReadKey();
            Console.Clear();
        }


        public void ShowMemberDetails()
        {
            person currentPerson = new person();
            person mother = new person();
            person father = new person();
            
            Console.WriteLine();
            Console.WriteLine(" Enter first name and last name to show details:");
            Console.WriteLine("--------------------------------------------------");
            Console.Write(" First name: ");
            string fName = Console.ReadLine();
            Console.Write(" last name: ");
            string lName = Console.ReadLine();
            currentPerson = MyCRUD.GenealogyCRUD.SearchPerson(fName, lName);
            father= MyCRUD.GenealogyCRUD.SearchFather(currentPerson.FatherID);
            mother = MyCRUD.GenealogyCRUD.SearchMother(currentPerson.MotherID);
            Console.WriteLine("...........................................................................................");
            Console.WriteLine();
            Console.Write("  First name :");
            Console.WriteLine(currentPerson.FirstName);
            Console.Write("  Last name :");
            Console.WriteLine(currentPerson.LastName);
            Console.Write("  Birth date :");
            Console.WriteLine(currentPerson.BirthDate);
            Console.Write("  Death date :");
            Console.WriteLine(currentPerson.DeathDate);
            Console.Write("  Father's name :");
            Console.Write(father.FirstName);
            Console.WriteLine(father.LastName);
            Console.Write("  Mother's name :");
            Console.Write(mother.FirstName);
            Console.WriteLine(mother.LastName);
            Console.WriteLine("...........................................................................................");
        }


        public void ShowParents()
        {
            person currentPerson = new person();
            Console.WriteLine(" Enter first name and last name to show parents:");
            Console.WriteLine("--------------------------------------------------");
            Console.Write(" First name: ");
            string fName = Console.ReadLine();
            Console.Write(" last name: ");
            string lName = Console.ReadLine();
            Console.WriteLine("--------------------------------------------------");
            currentPerson = MyCRUD.GenealogyCRUD.SearchPerson(fName, lName);
            Console.WriteLine();
            myCrud.GetMother(currentPerson);
            myCrud.GetFather(currentPerson);
            Console.WriteLine("--------------------------------------------------");

        }

        public void ListChildren()
        {
            Genealogy.MyCRUD.GenealogyCRUD.List();
            
        }

        public void AddMember()
        {
            person newPerson = new person();
            int fID=0;
            int mID=0;
            Console.Write(" Insert first name: ");
            newPerson.FirstName = Console.ReadLine();
            Console.Write(" Insert last Name: ");
            newPerson.LastName = Console.ReadLine();
            Console.Write(" Insert birth date: ");
            newPerson.BirthDate = Console.ReadLine();
            Console.Write(" Insert death date: ");
            newPerson.DeathDate = Console.ReadLine();
            Console.Write(" Insert mother's first name: ");
            string motherFN = Console.ReadLine();
            Console.Write(" Insert mother's last name: ");
            string motherLN = Console.ReadLine();
            if (MyCRUD.GenealogyCRUD.DoesMothersExits(motherFN, motherLN))
            {
                mID = MyCRUD.GenealogyCRUD.SearchMother(motherFN, motherLN);
                newPerson.MotherID = mID;
            }
            else if(motherFN.Length > 1 && motherLN.Length > 1)
            {
                myCrud.CreateMother(motherFN, motherLN);
                mID = MyCRUD.GenealogyCRUD.SearchMother(motherFN, motherLN);
                newPerson.MotherID = mID;
            }

            Console.Write(" Insert father's first name: ");
            string fatherFN = Console.ReadLine();
            Console.Write(" Insert father's last name: ");
            string fatherLN = Console.ReadLine();
            if (MyCRUD.GenealogyCRUD.DoesFatherExits(fatherFN, fatherLN))
            {
                fID = MyCRUD.GenealogyCRUD.SearchFather(fatherFN, fatherLN);
                newPerson.FatherID = fID;
            }
            else if (fatherFN.Length>1 && fatherLN.Length>1)
            {
                myCrud.CreateFather(fatherFN, fatherLN);
                fID = MyCRUD.GenealogyCRUD.SearchFather(fatherFN, fatherLN);
                newPerson.FatherID = fID;
            }
            mID = MyCRUD.GenealogyCRUD.SearchMother(motherFN, motherLN);
            fID = MyCRUD.GenealogyCRUD.SearchFather(fatherFN, fatherLN);
            myCrud.CreatePerson(newPerson);
            int pratentID = myCrud.SearchParents(fID, mID);
            newPerson = Genealogy.MyCRUD.GenealogyCRUD.SearchPerson(newPerson.FirstName, newPerson.LastName);
            myCrud.InsertChilderen(newPerson.Id, pratentID);
           
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine(" The member has been successfully added!!! ");
            Console.WriteLine("--------------------------------------------------");
        }


        public void DeleteMember()
        {
            person myPerson = new person();
            Console.WriteLine(" Enter first name and last name to delete: ");
            Console.Write(" First name: ");
            string fName = Console.ReadLine();
            Console.Write(" Last name: ");
            string lName = Console.ReadLine();
            if (Genealogy.MyCRUD.GenealogyCRUD.DoesPersonExits(fName, lName))
            {
                myPerson = MyCRUD.GenealogyCRUD.SearchPerson(fName, lName);
                myCrud.DeletePerson(myPerson);
                Console.WriteLine("--------------------------------------------------");
                Console.WriteLine(" The member has been Successfully deleted!!! ");
                Console.WriteLine("--------------------------------------------------");
            }
            else
            {
                Console.WriteLine(" Person is not found!!!");
            }
        }


        public void EditMember()
        {
            person myPerson = new person();
            Console.WriteLine(" Choose an option to change a member detail: ");
            Console.WriteLine("---------------------------------------------");
            Console.WriteLine("  1. First name");
            Console.WriteLine("  2. Last name");
            Console.WriteLine("  3. Birth date");
            Console.WriteLine("  4. Death date");
            Console.WriteLine("  5. Father Name");
            Console.WriteLine("  6. Mother name");
            Console.WriteLine();
            Console.Write(" > ");
            Console.WriteLine();
            int input = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine(" Enter First name and Last name ");
            Console.Write(" First name: ");
            string fName = Console.ReadLine();
            Console.Write(" Last name: ");
            string lName = Console.ReadLine();
            myPerson = MyCRUD.GenealogyCRUD.SearchPerson(fName, lName);
            switch (input)
            {
                case 1:
                    Console.Write(" Enter new first name :");
                    myPerson.FirstName = Console.ReadLine();
                    myCrud.UpdatePerson(myPerson);
                    Console.WriteLine("--------------------------------------------------");
                    Console.WriteLine(" Editing completed Successfully!!! ");
                    Console.WriteLine("--------------------------------------------------");
                    break;

                case 2:
                    Console.Write(" Enter new last name :");
                    myPerson.LastName = Console.ReadLine();
                    myCrud.UpdatePerson(myPerson);
                    Console.WriteLine("--------------------------------------------------");
                    Console.WriteLine(" Editing completed Successfully!!! ");
                    Console.WriteLine("--------------------------------------------------");
                    break;

                case 3:
                    Console.Write(" Enter new birth date :");
                    myPerson.BirthDate = Console.ReadLine();
                    myCrud.UpdatePerson(myPerson);
                    Console.WriteLine("--------------------------------------------------");
                    Console.WriteLine(" Editing completed Successfully!!! ");
                    Console.WriteLine("--------------------------------------------------");

                    break;

                case 4:
                    Console.Write(" Enter new death date :");
                    myPerson.DeathDate = Console.ReadLine();
                    myCrud.UpdatePerson(myPerson);
                    Console.WriteLine("--------------------------------------------------");
                    Console.WriteLine(" Editing completed Successfully!!! ");
                    Console.WriteLine("--------------------------------------------------");
                    break;

                case 5:
                    Console.WriteLine();
                    Console.WriteLine(" Enter new father name :");
                    Console.Write(" First name: ");
                    string fatherFN = Console.ReadLine();
                    Console.Write(" Last name: ");
                    string fatherLN = Console.ReadLine();
                    int fatherID;
                    fatherID = MyCRUD.GenealogyCRUD.SearchFather(fatherFN, fatherLN);
                    myPerson.FatherID = fatherID;
                    myCrud.UpdatePerson(myPerson);
                    Console.WriteLine("--------------------------------------------------");
                    Console.WriteLine(" Editing completed Successfully!!! ");
                    Console.WriteLine("--------------------------------------------------");
                    break;

                case 6:
                    Console.WriteLine();
                    Console.WriteLine(" Enter new mother name :");
                    Console.Write(" First name: ");
                    string motherFN = Console.ReadLine();
                    Console.Write(" Last name: ");
                    string motherLN = Console.ReadLine();
                    int motherID;
                    motherID = MyCRUD.GenealogyCRUD.SearchFather(motherFN, motherLN);
                    myPerson.MotherID = motherID;
                    myCrud.UpdatePerson(myPerson);
                    Console.WriteLine("--------------------------------------------------");
                    Console.WriteLine(" Editing completed Successfully!!! ");
                    Console.WriteLine("--------------------------------------------------");
                    break;

            }
        }

        public void ListMembers()
        {
            string filter = null;
            ArrayList paramValue = new ArrayList();
            Console.WriteLine(" CHoose an option to list members:");
            Console.WriteLine(" 1. Order by starting with a specific letter");
            Console.WriteLine(" 2. Order by birth date");
            Console.WriteLine(" 3. Order by empty field");
            Console.WriteLine(" 4. List children");
            Console.WriteLine();
            Console.Write(">");
            Console.WriteLine();

            int input = Convert.ToInt32(Console.ReadLine());

            switch (input)
            {
                case 1:

                    Console.WriteLine(" Enter The letter:");
                    paramValue.Add(Convert.ToChar(Console.ReadLine())+"%");
                    filter = "First_Name LIKE @input";
                    Genealogy.MyCRUD.GenealogyCRUD.List(filter, paramValue);
                    Console.WriteLine(".................................................");
                    break;

                case 2:
                    Console.WriteLine(" Enter the year value");
                    paramValue.Add("%"+Console.ReadLine()+"%");
                    filter = "Birth_Date LIKE @input";
                    Genealogy.MyCRUD.GenealogyCRUD.List(filter, paramValue);
                    Console.WriteLine(".................................................");
                    break;

                case 3:
                    Console.WriteLine(" Enter the field name: ");
                    Console.WriteLine(" 1. Birt date");
                    Console.WriteLine(" 2. Death date");
                    Console.WriteLine(" 3. Mother name");
                    Console.WriteLine(" 4. Father name");
                    Console.Write(">");
                    Console.WriteLine();
                    int choice = Convert.ToInt32(Console.ReadLine());
                    filter = "@input IS NULL";
                    switch (choice)
                    {
                        case 1:
                            ArrayList parmValue = new ArrayList() { "Birth_Date" };
                            Genealogy.MyCRUD.GenealogyCRUD. List(filter, parmValue);
                            Console.WriteLine(".................................................");
                            break;

                        case 2:
                            paramValue.Add("Death_Date");
                            Genealogy.MyCRUD.GenealogyCRUD.List(filter, paramValue);
                            Console.WriteLine(".................................................");
                            break;

                        case 3:
                            paramValue.Add("Mother_ID");
                            Genealogy.MyCRUD.GenealogyCRUD.List(filter, paramValue);
                            Console.WriteLine(".................................................");
                            break;

                        case 4:
                            paramValue.Add("Father_ID");
                            Genealogy.MyCRUD.GenealogyCRUD.List(filter, paramValue);
                            Console.WriteLine(".................................................");
                            break;
                    }
                    break;

                case 4:
                    ListChildren();
                    Console.WriteLine(".................................................");
                    break;


            }
        }


    }
}
