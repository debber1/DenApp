using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PDA_DePaddel.Models;

using System.Linq;

using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace PDA_DePaddel.Models
{
  public class Variables
    {
        private static string sName = "User";
        public static string Name
        {
            get { return sName; }
            set { sName = value; }
        }

        private static string sEventName = "test";
        public static string EventName
        {
            get { return sEventName; }
            set { sEventName = value; }
        }

        private static string sOperator = "test";
        public static string Operator
        {
            get { return sOperator; }
            set { sOperator = value; }
        }

        private static Decimal sTotalPrice;
        public static Decimal TotalPrice
        {
            get { return sTotalPrice; }
            set { sTotalPrice = value; }
        }
        private static ObservableCollection<TokenOrder> sTokenOrder;
        public static ObservableCollection<TokenOrder> TokenOrder
        {
            get { return sTokenOrder; }
            set { sTokenOrder = value; }
        }

        private static ObservableCollection<ProductOrder> sProductOrder;
        public static ObservableCollection<ProductOrder> ProductOrder
        {
            get { return sProductOrder; }
            set { sProductOrder = value; }
        }

        private static string sCurrentGuid;
        public static string CurrentGuid
        {
            get { return sCurrentGuid; }
            set { sCurrentGuid = value; }
        }

        // v0.2
        private static string sEventGuid;
        public static string EventGuid
        {
            get { return sEventGuid; }
            set { sEventGuid = value; }
        }

        private static int sPin;
        public static int Pin
        {
            get { return sPin; }
            set { sPin = value; }
        }

        private static int sNFC_Enable;
        public static int NFC_Enable
        {
            get { return sNFC_Enable; }
            set { sNFC_Enable = value; }
        }

        private static string sGuidUser;
        public static string GuidUser
        {
            get { return sGuidUser; }
            set { sGuidUser = value; }
        }
        private static string sPhoto = "";
        public static string Photo
        {
            get { return sPhoto; }
            set { sPhoto = value; }
        }
        //drankkaarten
        private static Boolean sRenew = false;
        public static Boolean Renew
        {
            get { return sRenew; }
            set { sRenew = value; }
        }
        //bestellingen
        private static Boolean sRenew2 = false;
        public static Boolean Renew2
        {
            get { return sRenew2; }
            set { sRenew2 = value; }
        }
        //drank
        private static Boolean sRenew3 = false;
        public static Boolean Renew3
        {
            get { return sRenew3; }
            set { sRenew3 = value; }
        }  
        //kassa
        private static Boolean sRenew4 = false;
        public static Boolean Renew4
        {
            get { return sRenew4; }
            set { sRenew4 = value; }
        }
        private static int sOrderNumber;
        public static int OrderNumber
        {
            get { return sOrderNumber; }
            set { sOrderNumber = value; }
        }
        private static ObservableCollection<OrderHead> sOrderNotDone;
        public static ObservableCollection<OrderHead> OrderNotDone
        {
            get { return sOrderNotDone; }
            set { sOrderNotDone = value; }
        }


    }
}