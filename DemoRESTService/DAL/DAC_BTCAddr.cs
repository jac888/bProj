using DemoRESTService.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Web;
using WebLib;

namespace DemoRESTService.DAL
{
    [DataObject]
    public class DAC_BTCAddr :DAC
    {
        private const string apikey = "";
        private const string _tableName = "WALLET_DAT";
        private const string _allFields = "PUBLIC_HASH , ADDRESSM , SIGNATURE_N , T_BALANCE ,SIGNATURE_E ,ADDRESSV , M_BALANCE ,UNCONFIRM_NO , T_F_BALANCE ,M_F_BALANCE ,CR_DAT ,CR_USR ,TD_DAT ,TD_USR";
        private string _select = "select " + _allFields;
        private string _from = " from " + _tableName;
        private string _orderBy = "ID";
        private string _where = " where 1=1 ";
        //private string _insert = "FIRSTNAME, LASTNAME,PAYRATE,STARTDATE,ENDDATE, DB_APPNO, DB_CRDAT, DB_CRUSR, DB_TRDAT, DB_TRUSR";
        //private string _insertValue = "@FIRSTNAME, @LASTNAME, @PAYRATE, @STARTDATE, @ENDDATE, @DB_APPNO, @DB_CRDAT, @DB_CRUSR, @DB_TRDAT, @DB_TRUSR";

        string connectionString = ConfigurationManager.ConnectionStrings["Development"].ConnectionString.ToString();

        public DAC_BTCAddr()
          : base()
        {
        }

        public DAC_BTCAddr(DbConnection conn)
          : base(conn)
        {
        }

        public int vaildAddress(string address)
        {
            int no = 0;
            if (!string.IsNullOrEmpty(address))
            {
                var tmp = SelectTAddress(address);
                if (tmp != null)
                    no = 1;
            }
            return no;
        }

        public int verifyKey(string apikey)
        {
            var result = 0;
            if (!string.IsNullOrEmpty(apikey))
                result = vaildKey(apikey);
            return result;
        }

        ///<summary>
        /// check apikey is vaild
        ///</summary>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public int vaildKey(string apikey)
        {
            DAC.ConnectionString = connectionString;
            DAC_BTCAddrList result = new DAC_BTCAddrList();
            IDataList list = result as IDataList;
            DbCommand cmdSelect = NewCommand();
            cmdSelect.CommandText = "select 1 " + _from + _where
                    + "  and PUBLIC_KEY = @apikey "
                    ;
            AddParam(cmdSelect, "apikey", apikey);
            int i;
            ExecuteScalar(cmdSelect, out i);
            return i;
        }

        ///<summary>
        /// select btc address from testnet 
        ///</summary>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public DAC_BTCAddrList SelectAllTAddress()
        {
            DAC.ConnectionString = connectionString;
            DAC_BTCAddrList result = new DAC_BTCAddrList();
            IDataList list = result as IDataList;
            DbCommand cmdSelect = NewCommand();
            cmdSelect.CommandText = "Select ADDRESSV " + _from + _where;
            Select(cmdSelect, ref list);
            return result;
        }

        ///<summary>
        /// select btc address from testnet 
        ///</summary>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public DAC_BTCAddrList SelectTAddress(string address)
        {
            DAC.ConnectionString = connectionString;
            DAC_BTCAddrList result = new DAC_BTCAddrList();
            IDataList list = result as IDataList;
            DbCommand cmdSelect = NewCommand();
            cmdSelect.CommandText = _select + _from + _where
                    + "  and ADDRESSV = @address "
                    ;
            AddParam(cmdSelect, "address", address);
            Select(cmdSelect, ref list);
            return result;
        }

        ///<summary>
        ///Person 資料集合類別
        ///</summary>
        [Serializable]
        public partial class DAC_BTCAddrList : List<Address>, IDataList
        {
            public void Fill(DbDataReader reader)
            {
                FillData(reader, this);
            }
        }
    }
}