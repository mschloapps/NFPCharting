using System;
using System.Collections.Generic;
using SQLite;

namespace NFPCharting
{
    public class SQLiteDB
    {
        readonly SQLiteConnection database;

        public SQLiteDB(string dbPath)
        {
            database = new SQLiteConnection(dbPath);
            database.CreateTable<NFPCycles>();
            database.CreateTable<NFPData>();
        }

        public int getNFPCyclesNumRows()
        {
            return database.Table<NFPCycles>().Count();
        }

        public List<NFPCycles> GetNFPCycles(int num = 9999)
        {
            string sql = "SELECT * FROM [NFPCycles] ORDER BY date([StartingDate]) DESC LIMIT ?";
            return database.Query<NFPCycles>(sql, num);
        }

        public List<NFPCycles> GetNFPCyclesASC(int num = 9999)
        {
            string sql = "SELECT * FROM [NFPCycles] ORDER BY date([StartingDate]) ASC LIMIT ?";
            return database.Query<NFPCycles>(sql, num);
        }

        public List<NFPCycles> GetCycleInfo(string date)
        {
            string sql = "SELECT * FROM [NFPCycles] WHERE [StartingDate] = ? LIMIT 1";
            return database.Query<NFPCycles>(sql, date);
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
                    NFPData dt = new NFPData()
                    {
                        DayID = i,
                        CycleID = cy.CycleID,
                        Date = dt2.ToString("yyyy-MM-dd"),
                        Menstrual = 0,
                        Indicator = 0,
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

        public List<NFPData> GetNFPData(int cyid)
        {
            string sql = "SELECT * FROM [NFPData] WHERE [CycleID] = ? ORDER BY [DayID] ASC";
            return database.Query<NFPData>(sql, cyid);
        }

        public List<NFPData> GetAllNFPData()
        {
            string sql = "SELECT * FROM [NFPData]";
            return database.Query<NFPData>(sql);
        }

        public List<NFPData> GetDayData(int cyid, int dyid)
        {
            string sql = "SELECT * FROM [NFPData] WHERE [CycleID] = ? AND [DayID] = ?";
            return database.Query<NFPData>(sql, cyid, dyid);
        }

        public bool UpdateNFPData(string date, int mens, int idct, int freq, int pk, int dayct, int itcs, string nt, string cl, int stsel, int img, int dyid, int cyid)
        {
            try
            {
                int ledit = dyid;
                UpdateLastEdit(cyid, ledit);
                string sql = "UPDATE [NFPData] SET [Date] = ?, [Menstrual] = ?, [Indicator] = ?, [Frequency] = ?, [Peak] = ?, [DayCount] = ?, [Intercourse] = ?, [Notes] = ?, [Color] = ?, [StSelect] = ?, [Image] = ? WHERE [DayID] = ? AND [CycleID] = ?";
                database.Query<NFPData>(sql, date, mens, idct, freq, pk, dayct, itcs, nt, cl, stsel, img, dyid, cyid);
                /*
                if (pk == 1)
                {
                    if (UpdateDayCount(cyid, dyid, idct))
                    {
                        return true;
                    } else 
                    {
                        return false;
                    }
                } 
                else
                {
                    return true;
                }
                */
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
                        List<NFPData> dayData = GetDayData(cyid, pkdy + 1);
                        string sql = "UPDATE [NFPData] SET [DayCount] = ?, [Color] = ?, [Image] = ? WHERE [DayID] = ? AND [CycleID] = ?";
                        if (dayData[0].Indicator <= 4)
                        {
                            database.Query<NFPData>(sql, i, "LightGreen", 1, pkdy + i, cyid);
                        }
                        else
                        {
                            database.Query<NFPData>(sql, i, "LightGray", 1, pkdy + i, cyid);
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
                string sql = "DELETE FROM [NFPData] WHERE [CycleID] = ?";
                database.Query<NFPData>(sql, cyid);
                sql = "DELETE FROM [NFPCycles] WHERE [StartingDate] = ?";
                database.Query<NFPCycles>(sql, date);
                var lst = GetNFPCycles();
                if (lst.Count > 0)
                {
                    SetCurrent(lst[0].StartingDate);
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
    }
}
