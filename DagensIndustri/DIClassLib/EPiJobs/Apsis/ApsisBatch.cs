using System;
using System.Linq;
using DIClassLib.DbHandlers;

namespace DIClassLib.EPiJobs.Apsis
{
    public class ApsisBatch
    {
        public readonly int BatchId; 
        public DateTime DateBatch;

        MailSenderDbHandler _db;

        private MailSenderDbHandler Db
        {
            get { return _db ?? (_db = new MailSenderDbHandler()); }
        }


        /// <summary>
        /// Creates new batch row in db.
        /// Sets member values BatchId and DateBatch
        /// </summary>
        public ApsisBatch()
        {
            DateBatch = DateTime.Now;
            BatchId = Db.InsertBatch();
        }
    }
}