
using System;
using System.Data.SqlClient;
using Genealogy.MyGenealogy;



namespace Genealogy
{
    class Program
    {
        static void Main(string[] args)
        {
            FamilyTree program = new FamilyTree();

            program.Start();

        }
    }
}
