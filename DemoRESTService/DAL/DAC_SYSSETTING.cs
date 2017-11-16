using DemoRESTService.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using WebLib;

namespace DemoRESTService.DAL
{
    [DataObject]
    public class DAC_SYSSETTING :DAC
    {
        private const string _tableName = "SYS_SETTING";
        private const string _allFields = " ID,SERVICENAME, SERVICE_INITIAL,STARTTIME,ENDTIME,SD,ED, WEEKLY, FEQUENCY";
        private string _select = "select " + _allFields;
        private string _from = " from " + _tableName;
        private string _orderBy = "ID";
        private string _where = " where 1=1 ";
        //private string _insert = "FIRSTNAME, LASTNAME,PAYRATE,STARTDATE,ENDDATE, DB_APPNO, DB_CRDAT, DB_CRUSR, DB_TRDAT, DB_TRUSR";
        //private string _insertValue = "@FIRSTNAME, @LASTNAME, @PAYRATE, @STARTDATE, @ENDDATE, @DB_APPNO, @DB_CRDAT, @DB_CRUSR, @DB_TRDAT, @DB_TRUSR";

        string connectionString = ConfigurationManager.ConnectionStrings["Debug"].ConnectionString.ToString();

        public DAC_SYSSETTING()
          : base()
        {
        }

        public DAC_SYSSETTING(DbConnection conn)
          : base(conn)
        {
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public DAC_SYSSETTINGList SelectByInitial(string initial)
        {
            DAC.ConnectionString = connectionString;
            DAC_SYSSETTINGList result = new DAC_SYSSETTINGList();
            IDataList list = result as IDataList;
            DbCommand cmdSelect = NewCommand();
            cmdSelect.CommandText = _select + _from + _where 
                    + "  and SERVICE_INITIAL = @initial "
                    ;
            AddParam(cmdSelect, "initial", initial);
            Select(cmdSelect, ref list);
            return result;
        }

        ///<summary>
        ///Person 資料集合類別
        ///</summary>
        [Serializable]
        public partial class DAC_SYSSETTINGList : List<SystemSetting>, IDataList
        {
            public void Fill(DbDataReader reader)
            {
                FillData(reader, this);
            }
        }
    }
}