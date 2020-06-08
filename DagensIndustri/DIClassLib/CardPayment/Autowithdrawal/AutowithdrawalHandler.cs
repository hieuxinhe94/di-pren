using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DIClassLib.CardPayment.Nets;
using DIClassLib.DbHandlers;
using DIClassLib.DbHelpers;
using DIClassLib.Misc;
using DIClassLib.Subscriptions;
using System.Data;

namespace DIClassLib.CardPayment.Autowithdrawal
{
    public class AutowithdrawalHandler
    {
        public AutowithdrawalHandler() { }


        /// <summary>
        /// Called by daily scheduled job in EPi. 
        /// Returns a string with num payed, num failed payed, num archived
        /// </summary>
        public string MakeAutowithdrawalPayments()
        {
            int batchId = MsSqlHandler.InsertAwdBatch();

            List<Awd> awds = GetAutowithdrawalsForPayment();
            foreach (Awd awd in awds)
            {
                //120814 any cancelReason kills sub
                if (awd.RenewSub == null || !Settings.SubsStateActiveValues.Contains(awd.RenewSub.SubsState) || !string.IsNullOrEmpty(awd.RenewSub.CancelReason))
                {
                    MsSqlHandler.SetAwdIncludeInBatchFlag(awd.AurigaSubsId, false);
                }
                else
                {
                    MakeAutowithdrawalPaymentNets(batchId, awd);
                }
            }

            return "BatchId: " + batchId.ToString() + ", ant hämtade kunder: " + awds.Count.ToString();
        }


        private List<Awd> GetAutowithdrawalsForPayment()
        {
            List<Awd> awds = new List<Awd>();
            List<Subscription> subs = new List<Subscription>();

            DataSet ds = MsSqlHandler.GetAwdsForPayment();
            if (DbHelpMethods.DataSetHasRows(ds))
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string aurigaSubsId = dr["aurigaSubsId"].ToString();
                    int cusno = int.Parse(dr["cusno"].ToString());
                    int subsno = int.Parse(dr["subsno"].ToString());
                    int campno = int.Parse(dr["campno"].ToString());
                    Guid pageGuid = new Guid(dr["pageGuid"].ToString());
                    DateTime dateSubsEnd = DateTime.Parse(dr["dateSubsEnd"].ToString());
                    Awd aw = new Awd(aurigaSubsId, cusno, subsno, campno, pageGuid, dateSubsEnd.Date, subs);
                    
                    if (IsAutowithdrawalInInterval(aw))
                    {
                        awds.Add(aw);
                    }
                }
            }

            return awds;
        }

        private bool IsAutowithdrawalInInterval(Awd aw)
        {
            if (aw.RenewSub == null)
            {
                return false;
            }

			var nowDate = DateTime.Now.Date;
            var awdNumDaysFwdPayIntervalStart = int.Parse(MiscFunctions.GetAppsettingsValue("awdNumDaysFwdPayIntervalStart"));
            var numDaysFwdIntervalEnd = int.Parse(MiscFunctions.GetAppsettingsValue("awdNumDaysFwdPayIntervalEnd"));
	        
			var intervalStartDate = nowDate.AddDays(awdNumDaysFwdPayIntervalStart);
			var intervalEndDate = nowDate.AddDays(numDaysFwdIntervalEnd);
			
            if (aw.RenewSub.EndDate.Date >= intervalStartDate && 
				aw.RenewSub.EndDate.Date <= intervalEndDate)
            {
                return true;
            }

            return false;
        }

        
        public void MakeAutowithdrawalPaymentNets(int batchId, Awd awd)
        {
            try
            {
                string merchantId = Settings.Nets_MerchantId;
                int cusRefno = MsSqlHandler.GetPayTransCusRefNo();
                double amountIncVatInOre = (awd.PriceIncVat * 100);
                double vatInOre = (awd.VatAmount * 100);
                string panHash = awd.AurigaSubsId;
                
                NetsRecurringPayment rp = new NetsRecurringPayment();
                var transactionId = rp.RegisterRecurringPayment(cusRefno, amountIncVatInOre, vatInOre, panHash);
                var payInfo = new QueryPayment(transactionId);

                MsSqlHandler.SaveDataBeforeCardPaymentNets(cusRefno, merchantId, Settings.Nets_CurrencyCode, (int)amountIncVatInOre, (int)vatInOre, PaymentMethod.TypeOfPaymentMethod.CreditCardAutowithdrawal.ToString(), "", "Avd: Upplaga", "Kort-autodragning, cusno:" + awd.Cusno + ", subsno:" + awd.Subsno, null, transactionId, null);
                MsSqlHandler.InsertAwdCustInBatch(awd.AurigaSubsId, batchId, cusRefno);

                //pay ok
                if (!string.IsNullOrEmpty(transactionId))  
                {
                    if (Settings.Nets_ProcessSale)
                    {
                        var processSaleObj = new ProcessSale(transactionId);
                        if (!processSaleObj.PaymentOk)
                        {
                            HandlePayFailed(cusRefno, processSaleObj.ResponseCode, panHash, payInfo.Issuer, awd);
                            return;
                        }
                    }

                    MsSqlHandler.SaveDataAfterCardPaymentNets(cusRefno, "A", "0", panHash, payInfo.Issuer);
                    MsSqlHandler.UpdateAwdDateSubsEnd(awd.AurigaSubsId, awd.Sub.SubsEndDate);

                    if (TryCreateRenewalInCirix(awd))
                    {
                        //2015-02-01: After talking with Stina we skip step with creating invoice
                        //Find better workflow in future instead of this whole procedure
                        //At the moment HandleInvoiceInCirix() also seem to result in some error response
                        //HandleInvoiceInCirix(awd, false);
                }
                }
                else //pay error
                {
                    HandlePayFailed(cusRefno, Settings.Nets_Err_TransNotFoundInNets, panHash, payInfo.Issuer, awd);
                }

            }
            catch (Exception ex)
            {
                new Logger("MakeAutowithdrawalPaymentNets failed for batchId:" + batchId + "<br>" + awd.ToString(), ex.ToString());
            }
        }


        private void HandlePayFailed(int cusRefno, string respCode, string panHash, string issuer, Awd awd)
        {
            MsSqlHandler.SaveDataAfterCardPaymentNets(cusRefno, "E", respCode, panHash, issuer);

            if (MiscFunctions.GetAppsettingsValue("awdSendPayPageLinkToCust") == "true")
            {
                //last batch pay try failed: send pay page link to cust
                if (awd.DateSubsEnd.Date == DateTime.Now.Date.AddDays(int.Parse(MiscFunctions.GetAppsettingsValue("awdNumDaysFwdPayIntervalStart"))))
                    SendCustomerMailWithLink(awd);
            }
        }


        public bool TryCreateRenewalInCirix(Awd awd)
        {
            string ret = CreateRenewalInCirix(awd);
            
            if (ret.StartsWith("FAILED"))
            {
                SendStaffMailCreateRenewAndInv(awd);
                return false;
            }

            return true;
        }

        private static string CreateRenewalInCirix(Awd awd)
        {
            var communeCode = "0180";    //CirixDbHandler.GetCommuneCode(awd.Sub.Subscriber.ZipCode);
            var priceListNo = SubscriptionController.GetPriceListNo(awd.Sub.PaperCode, awd.Sub.ProductNo, awd.Sub.SubsStartDate, communeCode, awd.Sub.Pricegroup, awd.Sub.CampNo.ToString());

            //Cirix uses new ExtNo and Kayak uses original ExtNo from subscription that is being renewed!
            var iOrigExtNo = (SubscriptionController.ActiveHandler == SubscriptionController.AvailableHandlers.Kayak) ? awd.RenewSub.Extno : (awd.RenewSub.Extno + 1);
            return SubscriptionController.CreateRenewal_DI(awd.Cusno, awd.Subsno, iOrigExtNo, priceListNo, awd.Campno, awd.Sub.SubsLenMons, awd.Sub.SubsLenDays, awd.Sub.SubsStartDate,
                awd.Sub.SubsEndDate, awd.Sub.SubsKind, awd.PriceExVat, awd.PriceExVat, 1, string.Empty, 0, awd.Sub.PackageId, awd.Sub.PaperCode, awd.Sub.ProductNo, string.Empty, Settings.TargetGroupRecurringPayment,
                string.Empty, string.Empty, string.Empty, "N", string.Empty,
                SubscriptionController.ActiveHandler == SubscriptionController.AvailableHandlers.Kayak ? Settings.InvoiceModeKayakCreditCard : Settings.InvoiceMode_KontoKort);
        }

        private void SendStaffMailCreateRenewAndInv(Awd awd)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Följande kund har betalt sin prenumeration med kontokort.<br>");
            sb.Append("Prenumerationssystemet lyckades dock inte skapa en förnyelse på prenumerationen.<br><br>");
            sb.Append("Följande behöver göras i prenumerationssystemet:<br>");
            sb.Append("- Skapa förnyelse<br>");
            sb.Append("- Skapa faktura, markera som betald<br><br>");

            sb.Append("Kundnummer: " + awd.Cusno.ToString() + "<br>");
            sb.Append("Prennummer: " + awd.Subsno.ToString() + "<br>");
            sb.Append("Prenstart: " + awd.Sub.SubsStartDate.ToShortDateString() + "<br>");
            sb.Append("Prenslut: " + awd.Sub.SubsEndDate.ToShortDateString() + "<br>");
            sb.Append("Pris ink moms: " + awd.PriceIncVat.ToString() + "kr <br>");
            sb.Append("Pris ex moms: " + awd.PriceExVat.ToString() + "kr <br><br>");

            sb.Append("<font color='red'><b>");
            sb.Append("OBS!<br>");
            sb.Append("Om det inte går att skapa en förnyelse på kunden, och ett nytt prennr istället skapas måste detta prennr skickas till ");
            sb.Append(MiscFunctions.GetAppsettingsValue("mailWebmasterDiSe") + " - annars kommer kundens autodragning inte att fungera i framtiden.<br>");
            sb.Append("</b></font>");

            MiscFunctions.SendMail(MiscFunctions.GetAppsettingsValue("mailPrenFelDiSe"), MiscFunctions.GetAppsettingsValue("mailPrenFelDiSe"), "Skapa förnyelse och faktura", sb.ToString(), true);
        }

        
        public void HandleInvoiceInCirix(Awd awd, bool tryPayInvoice)
        {
            long invno = SubscriptionController.GetNextInvno();
            string refno = SubscriptionController.BuildRefno2(invno, "00", awd.Sub.PaperCode);
            long immInv = SubscriptionController.CreateImmediateInvoice(awd.Sub, (awd.RenewSub.Extno + 1), 1, invno, refno);

            if (tryPayInvoice)
            {
                string payRet = SubscriptionController.InsertElectronicPayment(awd.Cusno, immInv, refno, awd.PriceIncVat);
                //bool payOk = (payRet.ToUpper().StartsWith("FAILED")) ? false : true;

                //Invoice inv = new Invoice(invno, refno, payOk);
                //if (!payOk)
                //    SendStaffMailHandlePayedBillInCirix(awd, inv);
            }

            //120619 - not needed after WS update
            //Invoice inv = new Invoice(invno, refno, false);
            //SendStaffMailHandlePayedBillInCirix(awd, inv);
        }

        private void SendCustomerMailWithLink(Awd awd)
        {
            string email = SubscriptionController.GetEmailAddress(awd.Cusno);

            if (MiscFunctions.IsValidEmail(email))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Hej,");
                sb.Append("<br><br>");
                sb.Append("Du betalar följande Di-prenumeration via autodragning på kontokort:<br>");
                sb.Append("Prenumerationstyp: " + Settings.GetName_Product(awd.Sub.PaperCode, awd.Sub.ProductNo) + "<br>");
                sb.Append("Kundnummer: " + awd.Cusno + "<br>");
                sb.Append("Prenumerationsnummer: " + awd.Subsno);
                sb.Append("<br><br>");
                sb.Append("Vi har misslyckats att dra pengar för din prenumeration vid följande tidpunkter:<br>");
                sb.Append(GetFailedPaymentsAsHtml(awd.AurigaSubsId));
                sb.Append("<br>");
                sb.Append("Använd länken nedan för att göra betalningen manuellt. Framtida autodragningar ");
                sb.Append("kommer att göras på det kort som anges vid denna betalning. Görs ingen manuell ");
                sb.Append("betalning kommer din prenumeration att upphöra efter " + awd.DateSubsEnd.ToShortDateString() + ".<br>");
                sb.Append("<a href='" + MiscFunctions.GetAppsettingsValue("awdPayPageUrl") + "?code=" + awd.PageGuid.ToString() + "' target='_blank'>");
                sb.Append("Länk till betalningssida");
                sb.Append("</a>");
                sb.Append(".<br><br><br>");
                sb.Append("Med vänlig hälsning<br>");
                sb.Append("Dagens industri<br>");
                sb.Append("Kundtjänst tel 08-573 651 00<br>");
                MiscFunctions.SendMail(MiscFunctions.GetAppsettingsValue("mailPrenDiSe"), email, "Di - misslyckad autodragning på kontokort", sb.ToString(), true);
            }
        }

        private string GetFailedPaymentsAsHtml(string aurigaSubsId)
        {
            StringBuilder sb = new StringBuilder();

            DataSet ds = MsSqlHandler.GetAwdFailedCustPayments(aurigaSubsId);
            if (DbHelpMethods.DataSetHasRows(ds))
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    DateTime dt = DateTime.Parse(dr["Purchase_date"].ToString());
                    //string status = dr["Status"].ToString();
                    //string statusCode = dr["Status_code"].ToString();

                    sb.Append(dt.ToString("yyyy-MM-dd HH:mm"));

                    //if (!string.IsNullOrEmpty(status))
                    //    sb.Append(" - <i>" + MiscFunctions.GetAurigaCardPayStatus(status, statusCode) + "</i>");

                    sb.Append("<br>");
                }
            }

            return sb.ToString();
        }


        //sub not active in cirix: remove from awd payment
        //foreach (Awd a in awds)
        //{
        //    if (!SubActiveInCirix(a.Cusno, a.Subsno))
        //    {
        //        MsSqlHandler.CancelAwdSubscription(a.Subsno);
        //        awds.Remove(a);
        //    }
        //}

        //public bool SubActiveInCirix(int cusno, int subsno)
        //{
        //    bool subsnoInCirix = false;
        //    DataSet ds = CirixDbHandler.GetSubscriptions(cusno, true);

        //    //no sub rows exist
        //    if (!DbHelpers.DbHelpMethods.DataSetHasRows(ds))
        //        return false;

        //    foreach (DataRow dr in ds.Tables[0].Rows)
        //    {
        //        //sub exists in cirix
        //        if (subsno == int.Parse(dr["SUBSNO"].ToString()))
        //        {
        //            subsnoInCirix = true;

        //            //sub not active
        //            if (!string.IsNullOrEmpty(dr["CANCELREASON"].ToString().Trim()))
        //                return false;
        //        }
        //    }

        //    return subsnoInCirix;
        //}


        //private void SendStaffMailHandlePayedBillInCirix(Awd awd, Invoice inv)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("Följande kund har betalt sin prenumeration via autodragning på kontokort.<br>");
        //    sb.Append("Det bör finnas en faktura på kunden i Cirix som ska sättas till 'betald'.<br>");
        //    sb.Append("Kontrollera fakturasätt på prenumerationen (ska vara satt till kontokort).");
        //    sb.Append("<br><br>");
        //    sb.Append("Uppgifter:<br>");
        //    sb.Append("Kundnr: " + awd.Cusno.ToString() + "<br>");
        //    sb.Append("Prennr: " + awd.Subsno.ToString() + "<br>");
        //    sb.Append("Fakturanr: " + inv.Invno + "<br>");
        //    sb.Append("OCR: " + inv.Refno);
        //    MiscFunctions.SendMail(MiscFunctions.GetAppsettingsValue("mailPrenFelDiSe"), MiscFunctions.GetAppsettingsValue("mailPrenFelDiSe"), "Markera faktura som betald i Cirix", sb.ToString(), true);
        //}

    }
}
