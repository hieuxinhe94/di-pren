﻿using System;
using DIClassLib.DbHandlers;

namespace PrenDiSe
{
    public partial class checkalive : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            litOutput.Text = SubscriptionController.TestOracleConnection();
        }
    }
}