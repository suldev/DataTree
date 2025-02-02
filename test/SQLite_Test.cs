﻿using Slowcat.Data;
using System;
using System.Data;

namespace DataIOTest
{
    class SQLite_Test : Base_Test
    {
        SQLite dataBase;
        public void TestAll()
        {
            TestClass = "SQLite";
            CreateInMemoryDataTable();
            CreateMultipleInMemoryDataTables();
            CreateInMemoryColumn();
            CreateInMemoryRow();
            AddCellData();
            GetIntCellDataAsDouble();
            GetStringCellDataAsInt();
            CreateNewDataBaseFile();
            CreateDataBaseFileInInternalMode();
            LoadExistingDataBaseFile();
            WriteEmptyTableToDataBaseFile();
            WriteEmptyTableToDataBaseFileNoCommit();
            WriteInReadOnlyMode();
            WriteDataAndReadBack();
        }

        private void CreateInMemoryDataTable()
        {
            InitializeTest("Create an empty in-memory DataTable");
            try
            {
                dataBase = new SQLite();
                ConditionTest<int>(dataBase.GetTableCount(), Condition.EQ, 0);
            }
            catch(Exception e)
            {
                ConsoleMessage(State.FAILED, null, null);
                DisplayException(e);
            }
        }

        private void CreateMultipleInMemoryDataTables()
        {
            InitializeTest("Create three empty in-memory DataTables");
            try
            {
                dataBase = new SQLite();
                dataBase.AddTable(new DataTable("Table1"));
                dataBase.AddTable(new DataTable("Table2"));
                dataBase.AddTable(new DataTable("Table3"));
                ConditionTest<int>(dataBase.GetTableCount(), Condition.EQ, 3);
            }
            catch(Exception e)
            {
                ConsoleMessage(State.FAILED, null, null);
                DisplayException(e);
            }
        }

        private void CreateInMemoryColumn()
        {
            InitializeTest("Create three empty columns");
            try
            {
                dataBase = new SQLite();
                dataBase.AddTable(new DataTable("Table1"));
                dataBase.AddColumn("Table1", "Column1", SQLite.DataType.Int, false);
                dataBase.AddColumn("Table1", "Column2", SQLite.DataType.Int, false);
                dataBase.AddColumn("Table1", "Column3", SQLite.DataType.Int, false);
                ConditionTest<int>(dataBase["Table1"].Columns.Count, Condition.EQ, 3);
            }
            catch(Exception e)
            {
                ConsoleMessage(State.FAILED, null, null);
                DisplayException(e);
            }
        }

        private void CreateInMemoryRow()
        {
            InitializeTest("Create three rows");
            try
            {
                dataBase = new SQLite();
                dataBase.AddTable(new DataTable("Table1"));
                dataBase.AddColumn("Table1", "Column1", SQLite.DataType.Int, false);
                dataBase.AddRow("Table1");
                dataBase.AddRow("Table1");
                dataBase.AddRow("Table1");
                ConditionTest<int>(dataBase["Table1"].Rows.Count, Condition.EQ, 3);
            }
            catch(Exception e)
            {
                ConsoleMessage(State.FAILED, null, null);
                DisplayException(e);
            }
        }

        private void AddCellData()
        {
            InitializeTest("Insert data into table");
            try
            {
                dataBase = new SQLite();
                dataBase.AddTable(new DataTable("Table1"));
                dataBase.AddColumn("Table1", "Column1", SQLite.DataType.Int, false);
                dataBase.AddRow("Table1");
                dataBase.SetData("Table1", 0, 0, 1);
                ConditionTest<int>(dataBase.GetData<int>("Table1", 0, 0), Condition.EQ, 1);
            }
            catch(Exception e)
            {
                ConsoleMessage(State.FAILED, null, null);
                DisplayException(e);
            }
        }

        private void GetIntCellDataAsDouble()
        {
            InitializeTest("Get integer cell data as double");
            try
            {
                dataBase = new SQLite();
                dataBase.AddTable(new DataTable("Table1"));
                dataBase.AddColumn("Table1", "Column1", SQLite.DataType.Int, false);
                dataBase.AddRow("Table1");
                dataBase.SetData("Table1", 0, 0, 1);
                ConditionTest<double>(dataBase.GetData<double>("Table1", 0, 0), Condition.EQ, 1.0);
            }
            catch(Exception e)
            {
                ConsoleMessage(State.FAILED, null, null);
                DisplayException(e);
            }
        }

        private void GetStringCellDataAsInt()
        {
            InitializeTest("Get non-numeric string cell data as int");
            try
            {
                dataBase = new SQLite();
                dataBase.AddTable(new DataTable("Table1"));
                dataBase.AddColumn("Table1", "Column1", SQLite.DataType.String, false);
                dataBase.AddRow("Table1");
                dataBase.SetData("Table1", 0, 0, "String");
                int test = dataBase.GetData<int>("Table1", 0, 0);
                ConditionTest<int>(test, Condition.EQ, default(int));
            }
            catch(Exception e)
            {
                ConsoleMessage(State.FAILED, null, null);
                DisplayException(e);
            }
        }

        private void CreateNewDataBaseFile()
        {
            InitializeTest("Create a new database");
            try
            {
                dataBase = new SQLite("newDatabase.db", SQLite.FileMode.CreateWrite);
                ConditionTest<bool>(dataBase.Open(), Condition.EQ, true);
                dataBase.Close();
            }
            catch(Exception e)
            {
                ConsoleMessage(State.FAILED, null, null);
                DisplayException(e);
            }
        }

        private void CreateDataBaseFileInInternalMode()
        {
            InitializeTest("Catch creation of database file in Internal mode");
            try
            {
                dataBase = new SQLite("newDatabase.db", SQLite.FileMode.Internal);
                ConditionTest<bool>(dataBase.Open(), Condition.EQ, false);
                dataBase.Close();
            }
            catch(Exception e)
            {
                ConsoleMessage(State.FAILED, null, null);
                DisplayException(e);
            }
        }

        private void LoadExistingDataBaseFile()
        {
            InitializeTest("Open an existing database");
            try
            {
                dataBase = new SQLite("newDatabase.db", SQLite.FileMode.ReadWrite);
                ConditionTest<bool>(dataBase.Open(), Condition.EQ, true);
                dataBase.Close();
            }
            catch (Exception e)
            {
                ConsoleMessage(State.FAILED, null, null);
                DisplayException(e);
            }
        }

        private void WriteEmptyTableToDataBaseFile()
        {
            InitializeTest("Write an empty table to existing database");
            try
            {
                dataBase = new SQLite("newDatabase.db", SQLite.FileMode.CreateWrite);
                dataBase.Open();
                dataBase.AddTable(new DataTable("Table1"));
                dataBase.AddColumn("Table1", "Column1", SQLite.DataType.Int, true);
                dataBase.Commit();
                dataBase.Write();
                dataBase.Close();
                dataBase = new SQLite("newDatabase.db", SQLite.FileMode.ReadOnly);
                dataBase.Open();
                ConditionTest<int>(dataBase.GetTableCount(), Condition.EQ, 1);
                dataBase.Close();
            }
            catch (Exception e)
            {
                ConsoleMessage(State.FAILED, null, null);
                DisplayException(e);
            }
        }

        private void WriteEmptyTableToDataBaseFileNoCommit()
        {
            InitializeTest("Write database without committing new table");
            try
            {
                dataBase = new SQLite("newDatabase.db", SQLite.FileMode.ReadWrite);
                dataBase.Open();
                dataBase.AddTable(new DataTable("Table2"));
                dataBase.Write();
                dataBase.Close();
                dataBase = new SQLite("newDatabase.db", SQLite.FileMode.ReadOnly);
                dataBase.Open();
                ConditionTest<int>(dataBase.GetTableCount(), Condition.EQ, 1);
                dataBase.Close();
            }
            catch (Exception e)
            {
                ConsoleMessage(State.FAILED, null, null);
                DisplayException(e);
            }
        }

        private void WriteInReadOnlyMode()
        {
            InitializeTest("Catch writing to database file in ReadOnly mode");
            try
            {
                dataBase = new SQLite("newDatabase.db", SQLite.FileMode.ReadOnly);
                dataBase.Open();
                ConditionTest<bool>(dataBase.Write(), Condition.EQ, false);
                dataBase.Close();

            }
            catch(Exception e)
            {
                ConsoleMessage(State.FAILED, null, null);
                DisplayException(e);
            }
        }

        private void WriteDataAndReadBack()
        {
            InitializeTest("Write new data to database and read back");
            try
            {
                dataBase = new SQLite("newDatabase.db", SQLite.FileMode.CreateWrite);
                dataBase.Open();
                dataBase.AddTable(new DataTable("Table1"));
                dataBase.AddColumn("Table1", "Column1", SQLite.DataType.Int, true);
                dataBase.AddRow("Table1");
                dataBase.SetData("Table1", 0, 0, 100);
                dataBase.Commit();
                dataBase.Write();
                dataBase.Close();
                dataBase = new SQLite("newDatabase.db", SQLite.FileMode.ReadOnly);
                dataBase.Open();
                ConditionTest<int>(dataBase.GetData<int>("Table1", 0, 0), Condition.EQ, 100);
                dataBase.Close();
            }
            catch (Exception e)
            {
                ConsoleMessage(State.FAILED, null, null);
                DisplayException(e);
            }
        }
    }
}
