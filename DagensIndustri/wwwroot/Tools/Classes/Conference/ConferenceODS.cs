using System;
using System.Web.UI.WebControls;
using DagensIndustri.Templates.Public.Units.Placeable.Conference;
using System.Collections.Generic;
using System.Data.SqlClient;
using DIClassLib.DbHandlers;
using EPiServer.Core;
using System.Data;


namespace DagensIndustri.Tools.Classes.Conference
{
    /// <summary>
    /// ConferenceODS is used as objectdatasource in CampaignAdmin plugin tool
    /// </summary>
    public class ConferenceODS
    {
        public DataSet GetPersons(int conferenceid, int dayid, int seminarid)
        {
            return MsSqlHandler.GetPersonsForConference(conferenceid, dayid, seminarid);
        }

        public void UpdatePerson(int personid, string firstname, string lastname, string company, string title, string orgno, string phone, string email, string invoiceaddress, string invoicereference, string zip, string city, string code, string price)
        {
            MsSqlHandler.UpdatePerson(personid, firstname, lastname, company, title, orgno, phone, email, invoiceaddress, invoicereference, zip, city, code, price);
        }

        public void DeletePerson(int personId)
        {
            MsSqlHandler.DeletePerson(personId);
        }

        public DataSet GetInfoChannels()
        {
            return MsSqlHandler.GetInfoChannels();
        }

        public DataSet GetInfoChannelsEn()
        {
            return MsSqlHandler.GetInfoChannelsEn();
        }
    }
}