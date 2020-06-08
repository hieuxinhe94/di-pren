using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DIClassLib.DbHelpers;
using System.Data.SqlClient;
using System.Data;

namespace DIClassLib.DbHandlers
{
    /// <summary>
    /// This class handles communication with the database for contest answers
    /// </summary>
    public static class ContestDbHandler
    {
        private static string _connStrContest = "DagensIndustriMISC";

        public static void InsertContestAnswer(int epiPageId, long cusno, string name, string mail, string phone, String answerData)
        {
            try 
            {
                var p = new SqlParameter[] 
                { 
                    new SqlParameter("@cusno", cusno),
                    new SqlParameter("@epiPageId", epiPageId),
                    new SqlParameter("@name", name ?? ""),
                    new SqlParameter("@mail", mail ?? ""),
                    new SqlParameter("@phone", phone ?? ""),
                    new SqlParameter("@answerData", answerData)
                };

                SqlHelper.ExecuteNonQuery(_connStrContest, "InsertContestAnswer", p);
            }
            catch (Exception ex)
            {
                new Logger("InsertContestAnswer() failed", ex.ToString());
            }
        }

        public static DataSet GetContestAnswers(int? epiPageId)
        {
            try
            {
                return SqlHelper.ExecuteDataset(_connStrContest, "GetContestAnswers", new SqlParameter("@epiPageId", epiPageId));
            }
            catch (Exception ex)
            {
                new Logger("AddContestAnswer() failed", ex.ToString());
            }
            return null;
        }

        public static DataSet GetContestCusnoByCode(Guid code)
        {
            try
            {
                return SqlHelper.ExecuteDataset(_connStrContest, "GetContestCusnoByCode", new SqlParameter("@code", code));
            }
            catch (Exception ex)
            {
                new Logger("GetContestCusnoByCode() failed for code: " + code.ToString(), ex.ToString());
            }
            return null;
        }
        
        

    }
}
