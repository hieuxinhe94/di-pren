using System;
using Di.Subscription.DataAccess.IssueDate;

namespace Di.Subscription.Logic.IssueDate.Retrievers
{
    internal class IssueDateRetriever : IIssueDateRetriever
    {
        private readonly IIssueDateRepository _issueDateRepository;

        public IssueDateRetriever(IIssueDateRepository issueDateRepository)
        {
            _issueDateRepository = issueDateRepository;
        }

        /// <summary>
        /// Get next issue date
        /// </summary>
        /// <param name="papercode">The papercode</param>
        /// <param name="productNumber">The productnumber</param>
        /// <param name="minDate">The minimun date</param>
        /// <returns>The next possible issue date after the minimum date</returns>
        public DateTime GetNextIssuedate(string papercode, string productNumber, DateTime minDate)
        {
            var dateString =  _issueDateRepository.GetNextIssuedate(papercode, productNumber, minDate);

            return DateTime.Parse(dateString);
        }
    }
}