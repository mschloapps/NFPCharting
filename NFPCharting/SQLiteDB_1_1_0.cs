using System;
using System.Collections.Generic;
using SQLite;

namespace NFPCharting
{
    public class SQLiteDB_1_1_0
    {
        readonly SQLiteConnection database;

        public SQLiteDB_1_1_0(string dbPath)
        {
            database = new SQLiteConnection(dbPath);
            database.ExecuteScalar<string>("PRAGMA journal_mode=DELETE");
            database.CreateTable<NFPCycles>();
            database.CreateTable<NFPData_1_1_0>();
        }

        public int getNFPCyclesNumRows()
        {
            return database.Table<NFPCycles>().Count();
        }

        public List<NFPCycles> GetNFPCycles(int num = 99999)
        {
            string sql = "SELECT * FROM [NFPCycles] ORDER BY date([StartingDate]) DESC LIMIT ?";
            return database.Query<NFPCycles>(sql, num);
        }

        public List<NFPCycles> GetNFPCyclesASC(int num = 99999)
        {
            //string sql = "SELECT * FROM (SELECT * FROM [NFPCycles] ORDER BY [CycleID] DESC LIMIT ?) sub ORDER BY date([StartingDate]) ASC";
            string sql = "SELECT * FROM (SELECT * FROM [NFPCycles] ORDER BY date([StartingDate]) DESC LIMIT ?) sub ORDER BY date([StartingDate]) ASC";
            return database.Query<NFPCycles>(sql, num);
        }

        public List<NFPCycles> GetCycleInfo(string date)
        {
            string sql = "SELECT * FROM [NFPCycles] WHERE [StartingDate] = ? LIMIT 1";
            return database.Query<NFPCycles>(sql, date);
        }

        public List<NFPCycles> GetCycleInfoID(int id)
        {
            string sql = "SELECT * FROM [NFPCycles] WHERE [CycleID] = ? LIMIT 1";
            return database.Query<NFPCycles>(sql, id);
        }

        public List<NFPCycles> GetCurrentCycle()
        {
            string sql = "SELECT * FROM [NFPCycles] WHERE [IsCurrent] = 1 LIMIT 1";
            return database.Query<NFPCycles>(sql);
        }

        public void SetCurrent(string date)
        {
            string sql = "UPDATE [NFPCycles] SET [IsCurrent] = 0, [IsCurrentTxt] = 'No'";
            database.Query<NFPCycles>(sql);
            int numCycles = getNFPCyclesNumRows();
            if (numCycles == 1)
            {
                sql = "UPDATE [NFPCycles] SET [IsCurrent] = 1, [IsCurrentTxt] = 'Yes'";
            }
            else
            {
                sql = "UPDATE [NFPCycles] SET [IsCurrent] = 1, [IsCurrentTxt] = 'Yes' WHERE [StartingDate] = ?";
            }
            database.Query<NFPCycles>(sql, date);
            App.CurCycle = GetCurrentCycle();
        }

        public void SetCurrentID(int cyid)
        {
            string sql = "UPDATE [NFPCycles] SET [IsCurrent] = 0, [IsCurrentTxt] = 'No'";
            database.Query<NFPCycles>(sql);
            int numCycles = getNFPCyclesNumRows();
            if (numCycles == 1)
            {
                sql = "UPDATE [NFPCycles] SET [IsCurrent] = 1, [IsCurrentTxt] = 'Yes'";
            }
            else
            {
                sql = "UPDATE [NFPCycles] SET [IsCurrent] = 1, [IsCurrentTxt] = 'Yes' WHERE [CycleID] = ?";
            }
            database.Query<NFPCycles>(sql, cyid);
            App.CurCycle = GetCurrentCycle();
        }



        public void UpdateLastEdit(int cyid, int ledit)
        {
            string sql = "UPDATE [NFPCycles] SET [LastEdit] = ? WHERE [CycleID] = ?";
            database.Query<NFPCycles>(sql, ledit, cyid);
        }

        public bool InsertCycle(string date, int ndays)
        {
            try
            {
                NFPCycles cy = new NFPCycles()
                {
                    StartingDate = date,
                    NumDays = ndays,
                    IsCurrent = 1,
                    IsCurrentTxt = "Yes",
                    LastEdit = 1
                };
                database.Insert(cy);
                DateTime dt1 = DateTime.Parse(date);
                for (int i = 1; i <= ndays; i++)
                {
                    DateTime dt2 = dt1.AddDays(i - 1);
                    NFPData_1_1_0 dt = new NFPData_1_1_0()
                    {
                        DayID = i,
                        CycleID = cy.CycleID,
                        Date = dt2.ToString("yyyy-MM-dd"),
                        Menstrual = 0,
                        Indicator_1 = 0,
                        Indicator_2 = 0,
                        Indicator_3 = 0,
                        Frequency = 0,
                        Peak = 0,
                        DayCount = 0,
                        Intercourse = 0,
                        Notes = "",
                        Color = "White",
                        StSelect = 0,
                        Image = 0
                    };
                    database.Insert(dt);
                }

                SetCurrent(date);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<NFPData_1_1_0> GetNFPData(int cyid)
        {
            string sql = "SELECT * FROM [NFPData_1_1_0] WHERE [CycleID] = ? ORDER BY [DayID] ASC";
            
            return database.Query<NFPData_1_1_0>(sql, cyid);
        }

        public List<NFPData_1_1_0> GetAllNFPData(int num = 99999)
        {
            //string sql = "SELECT * FROM NFPData_1_1_0 WHERE CycleID IN (SELECT CycleID FROM [NFPCycles] ORDER BY [CycleID] DESC LIMIT ?) ORDER BY [CycleID] ASC, [DayID] ASC";
            string sql = "SELECT * FROM NFPData_1_1_0 WHERE CycleID IN (SELECT CycleID FROM [NFPCycles] ORDER BY date([StartingDate]) ASC LIMIT ?) ORDER BY [CycleID] ASC, [DayID] ASC";
            return database.Query<NFPData_1_1_0>(sql, num);
        }

        public List<NFPData_1_1_0> GetDayData(int cyid, int dyid)
        {
            string sql = "SELECT * FROM [NFPData_1_1_0] WHERE [CycleID] = ? AND [DayID] = ?";
            return database.Query<NFPData_1_1_0>(sql, cyid, dyid);
        }

        public bool UpdateNFPData(string date, int mens, int idct1, int indct2, int indct3, int freq, int pk, int dayct, int itcs, string nt, string cl, int stsel, int img, int dyid, int cyid)
        {
            try
            {
                int ledit = dyid;
                UpdateLastEdit(cyid, ledit);
                string sql = "UPDATE [NFPData_1_1_0] SET [Date] = ?, [Menstrual] = ?, [Indicator_1] = ?, [Indicator_2] = ?, [Indicator_3] = ?, [Frequency] = ?, [Peak] = ?, [DayCount] = ?, [Intercourse] = ?, [Notes] = ?, [Color] = ?, [StSelect] = ?, [Image] = ? WHERE [DayID] = ? AND [CycleID] = ?";
                database.Query<NFPData_1_1_0>(sql, date, mens, idct1, indct2, indct3, freq, pk, dayct, itcs, nt, cl, stsel, img, dyid, cyid);                
                App.CurCycle = GetCurrentCycle();
                return true;
            }
            catch
            {
                return false;
            }


        }

        public int GetNumDays(int cyid)
        {
            string sql = "SELECT [NumDays] FROM [NFPCycles] WHERE [CycleID] = ?";
            return database.Query<NFPCycles>(sql, cyid)[0].NumDays;
        }

        public void SetNumDays(int ndays, int cyid)
        {
            string sql = "UPDATE [NFPCycles] SET [NumDays] = ? WHERE [CycleID] = ?";
            database.Query<NFPCycles>(sql, ndays, cyid);
        }

        public void SetStartDate(string dt, int ndays, int cyid)
        {
            string sql = "UPDATE [NFPCycles] SET [StartingDate] = ? WHERE [CycleID] = ?";
            database.Query<NFPCycles>(sql, dt, cyid);
            DateTime dt1 = DateTime.Parse(dt);
            sql = "UPDATE [NFPData_1_1_0] SET [Date] = ? WHERE [CycleID] = ? AND [DayID] = ?";
            for (int i=1; i<=ndays; i++)
            {
                DateTime dt2 = dt1.AddDays(i-1);
                database.Query<NFPData_1_1_0>(sql, dt2.ToString("yyyy-MM-dd"), cyid, i);
            }
        }

        public string GetStartingDate(int cyid)
        {
            string sql = "SELECT [StartingDate] FROM [NFPCycles] WHERE [CycleID] = ?";
            return database.Query<NFPCycles>(sql, cyid)[0].StartingDate;
        }

        public bool ModifyCycle(int cyid, int dys, string sdate)
        {
            try
            {
                var cydata = GetCycleInfoID(cyid);                
                if (dys < cydata[0].NumDays)
                {
                    string sql = "DELETE FROM [NFPData_1_1_0] WHERE [CycleID] = ? AND [DayID] > ?";
                    database.Query<NFPData_1_1_0>(sql, cyid, dys);
                    if (cydata[0].LastEdit > dys)
                    {
                        UpdateLastEdit(cyid, dys);
                    }                    
                }
                else if (dys > cydata[0].NumDays)
                {
                    DateTime dt1 = DateTime.Parse(cydata[0].StartingDate);
                    //string d1 = dt1.ToString("yyyy-MM-dd");
                    DateTime dt2 = dt1.AddDays(cydata[0].NumDays);
                    //string d2 = dt2.ToString("yyyy-MM-dd");
                    for (int i = 1; i <= (dys- cydata[0].NumDays); i++)
                    {
                        DateTime dt3 = dt2.AddDays(i-1);
                        //string d3 = dt3.ToString("yyyy-MM-dd");
                        NFPData_1_1_0 dt = new NFPData_1_1_0()
                        {
                            DayID = cydata[0].NumDays + i,
                            CycleID = cyid,
                            Date = dt3.ToString("yyyy-MM-dd"),
                            Menstrual = 0,
                            Indicator_1 = 0,
                            Indicator_2 = 0,
                            Indicator_3 = 0,
                            Frequency = 0,
                            Peak = 0,
                            DayCount = 0,
                            Intercourse = 0,
                            Notes = "",
                            Color = "White",
                            StSelect = 0,
                            Image = 0
                        };
                        database.Insert(dt);
                    }
                }

                SetNumDays(dys, cyid);
                SetStartDate(sdate, dys, cyid);

                return true;
            }
            catch
            {
                return false;
            }

        }



        public bool UpdateDayCount(int cyid, int pkdy)
        {
            try
            {
                if (pkdy < 38)
                {
                    for (int i = 1; i <= 3; i++)
                    {
                        List<NFPData_1_1_0> dayData = GetDayData(cyid, pkdy + 1);
                        string sql = "UPDATE [NFPData_1_1_0] SET [DayCount] = ?, [Color] = ?, [Image] = ? WHERE [DayID] = ? AND [CycleID] = ?";
                        if (dayData[0].Indicator_1 <= 4)
                        {
                            database.Query<NFPData_1_1_0>(sql, i, "LightGreen", 1, pkdy + i, cyid);
                        }
                        else
                        {
                            database.Query<NFPData_1_1_0>(sql, i, "LightGray", 1, pkdy + i, cyid);
                        }
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteCycle(string date)
        {
            try
            {
                List<NFPCycles> cydata = GetCycleInfo(date);
                int cyid = cydata[0].CycleID;
                string sql = "DELETE FROM [NFPData_1_1_0] WHERE [CycleID] = ?";
                database.Query<NFPData_1_1_0>(sql, cyid);
                sql = "DELETE FROM [NFPCycles] WHERE [StartingDate] = ?";
                database.Query<NFPCycles>(sql, date);
                var lst = GetNFPCycles();
                if (lst.Count > 0)
                {
                    SetCurrentID(lst[0].CycleID);
                }
                else
                {
                    App.CurCycle = GetCurrentCycle();
                }
                return true;
            }
            catch
            {
                return false;
            }

        }

        public bool DeleteCycleID(int cyid)
        {
            try
            {
                //List<NFPCycles> cydata = GetCycleInfo(date);
                //int cyid = cydata[0].CycleID;
                string sql = "DELETE FROM [NFPData_1_1_0] WHERE [CycleID] = ?";
                database.Query<NFPData_1_1_0>(sql, cyid);
                sql = "DELETE FROM [NFPCycles] WHERE [CycleID] = ?";
                database.Query<NFPCycles>(sql, cyid);
                var lst = GetNFPCycles();
                if (lst.Count > 0)
                {
                    SetCurrentID(lst[0].CycleID);
                }
                else
                {
                    App.CurCycle = GetCurrentCycle();
                }
                return true;
            }
            catch
            {
                return false;
            }

        }



        public bool MigrateData(List<NFPCycles> cycles, List<NFPData> data)
        {
            try
            {
                for (var i = 0; i < cycles.Count; i++)
                {
                    NFPCycles cy = new NFPCycles()
                    {
                        StartingDate = cycles[i].StartingDate,
                        NumDays = cycles[i].NumDays,
                        IsCurrent = cycles[i].IsCurrent,
                        IsCurrentTxt = cycles[i].IsCurrentTxt,
                        LastEdit = cycles[i].LastEdit
                    };
                    database.Insert(cy);
                }
                for (var i = 0; i < data.Count; i++)
                {
                    int Ind1;
                    int Ind2;
                    int Ind3;
                    switch (data[i].Indicator)
                    {
                        case 0:
                            Ind1 = 0;
                            Ind2 = 0;
                            Ind3 = 0;
                            break;
                        case 1:
                            Ind1 = 1;
                            Ind2 = 0;
                            Ind3 = 0;
                            break;
                        case 2:
                            Ind1 = 2;
                            Ind2 = 0;
                            Ind3 = 0;
                            break;
                        case 3:
                            Ind1 = 3;
                            Ind2 = 0;
                            Ind3 = 0;
                            break;
                        case 4:
                            Ind1 = 4;
                            Ind2 = 0;
                            Ind3 = 0;
                            break;
                        case 5:
                            Ind1 = 5;
                            Ind2 = 1;
                            Ind3 = 0;
                            break;
                        case 6:
                            Ind1 = 5;
                            Ind2 = 2;
                            Ind3 = 0;
                            break;
                        case 7:
                            Ind1 = 5;
                            Ind2 = 3;
                            Ind3 = 0;
                            break;
                        case 8:
                            Ind1 = 5;
                            Ind2 = 0;
                            Ind3 = 2;
                            break;
                        case 9:
                            Ind1 = 5;
                            Ind2 = 4;
                            Ind3 = 0;
                            break;
                        case 10:
                            Ind1 = 5;
                            Ind2 = 0;
                            Ind3 = 1;
                            break;
                        case 11:
                            Ind1 = 5;
                            Ind2 = 0;
                            Ind3 = 3;
                            break;
                        case 12:
                            Ind1 = 5;
                            Ind2 = 5;
                            Ind3 = 0;
                            break;
                        case 13:
                            Ind1 = 6;
                            Ind2 = 1;
                            Ind3 = 0;
                            break;
                        case 14:
                            Ind1 = 6;
                            Ind2 = 2;
                            Ind3 = 0;
                            break;
                        case 15:
                            Ind1 = 6;
                            Ind2 = 3;
                            Ind3 = 0;
                            break;
                        case 16:
                            Ind1 = 6;
                            Ind2 = 0;
                            Ind3 = 2;
                            break;
                        case 17:
                            Ind1 = 6;
                            Ind2 = 4;
                            Ind3 = 0;
                            break;
                        case 18:
                            Ind1 = 6;
                            Ind2 = 0;
                            Ind3 = 1;
                            break;
                        case 19:
                            Ind1 = 6;
                            Ind2 = 0;
                            Ind3 = 3;
                            break;
                        case 20:
                            Ind1 = 6;
                            Ind2 = 5;
                            Ind3 = 0;
                            break;
                        case 21:
                            Ind1 = 7;
                            Ind2 = 1;
                            Ind3 = 0;
                            break;
                        case 22:
                            Ind1 = 7;
                            Ind2 = 2;
                            Ind3 = 0;
                            break;
                        case 23:
                            Ind1 = 7;
                            Ind2 = 3;
                            Ind3 = 0;
                            break;
                        case 24:
                            Ind1 = 7;
                            Ind2 = 0;
                            Ind3 = 2;
                            break;
                        case 25:
                            Ind1 = 7;
                            Ind2 = 4;
                            Ind3 = 0;
                            break;
                        case 26:
                            Ind1 = 7;
                            Ind2 = 0;
                            Ind3 = 1;
                            break;
                        case 27:
                            Ind1 = 7;
                            Ind2 = 0;
                            Ind3 = 3;
                            break;
                        case 28:
                            Ind1 = 7;
                            Ind2 = 5;
                            Ind3 = 0;
                            break;
                        case 29:
                            Ind1 = 8;
                            Ind2 = 0;
                            Ind3 = 0;
                            break;
                        case 30:
                            Ind1 = 9;
                            Ind2 = 0;
                            Ind3 = 0;
                            break;
                        case 31:
                            Ind1 = 10;
                            Ind2 = 0;
                            Ind3 = 0;
                            break;
                        default:
                            Ind1 = 0;
                            Ind2 = 0;
                            Ind3 = 0;
                            break;
                    }
                    NFPData_1_1_0 dt = new NFPData_1_1_0()
                    {
                        DayID = data[i].DayID,
                        CycleID = data[i].CycleID,
                        Date = data[i].Date,
                        Menstrual = data[i].Menstrual,
                        Indicator_1 = Ind1,
                        Indicator_2 = Ind2,
                        Indicator_3 = Ind3,
                        Frequency = data[i].Frequency,
                        Peak = data[i].Peak,
                        DayCount = data[i].DayCount,
                        Intercourse = data[i].Intercourse,
                        Notes = data[i].Notes,
                        Color = data[i].Color,
                        StSelect = data[i].StSelect,
                        Image = data[i].Image
                    };
                    database.Insert(dt);
                }
                return true;
            }
            catch
            {
                return false;
            }

        }
    }
}
