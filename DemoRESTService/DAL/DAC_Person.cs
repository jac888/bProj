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
    public class DAC_Person : DAC
    {
        private const string _tableName = "PERSON";
        private const string _allFields = " ID,FIRSTNAME, LASTNAME,PAYRATE,STARTDATE,ENDDATE,DB_APPNO, DB_CRDAT, DB_CRUSR, DB_TRDAT, DB_TRUSR";
        private string _select = "select " + _allFields;
        private string _from = " from " + _tableName;
        private string _orderBy = "ID";
        private string _where = " where 1=1 ";
        private string _insert = "FIRSTNAME, LASTNAME,PAYRATE,STARTDATE,ENDDATE, DB_APPNO, DB_CRDAT, DB_CRUSR, DB_TRDAT, DB_TRUSR";
        private string _insertValue = "@FIRSTNAME, @LASTNAME, @PAYRATE, @STARTDATE, @ENDDATE, @DB_APPNO, @DB_CRDAT, @DB_CRUSR, @DB_TRDAT, @DB_TRUSR";

        string connectionString = ConfigurationManager.ConnectionStrings["Debug"].ConnectionString.ToString();
        

        public DAC_Person()
          : base()
        {
        }

        public DAC_Person(DbConnection conn)
          : base(conn)
        {
        }

        public string getPerson(int id)
        {
            if (id > 0)
            {
                return "got person";
            }
            return "";
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public DAC_PersonList SelectOne(int ID)
        {
            DAC.ConnectionString = connectionString;
            DAC_PersonList result = new DAC_PersonList();
            IDataList list = result as IDataList;
            DbCommand cmdSelect = NewCommand();
            cmdSelect.CommandText = _select + _from + _where
                    + "  and ID = @ID "
                    ;
            AddParam(cmdSelect, "ID", ID);
            Select(cmdSelect, ref list);
            return result;
        }

        [DataObjectMethod(DataObjectMethodType.Insert)]
        public int InsertOne(Models.Person value)
        {
            DAC.ConnectionString = connectionString;
            using (TransactionScope ts = NewTransactionScope())
            {
                try
                {
                    if (!BeforeInsertOne(value))
                        return 0;
                    //if (!value.Validate())
                    //    return -1;
                    DbCommand cmdInsert = NewCommand();
                    cmdInsert.CommandText =
                        "insert into PERSON"
                        + "  (" + _insert + ") "
                        + " values "
                        + "  (" + _insertValue + ") "
                        ;
                    AddParam(cmdInsert, "FIRSTNAME", value.FirstName);
                    AddParam(cmdInsert, "LASTNAME", value.LastName);
                    AddParam(cmdInsert, "PAYRATE", value.PayRate);
                    AddParam(cmdInsert, "STARTDATE", value.StartDate);
                    AddParam(cmdInsert, "ENDDATE", value.EndDate);
                    AddParam(cmdInsert, "DB_APPNO", 1);
                    AddParam(cmdInsert, "DB_CRDAT", DateTime.Now);
                    AddParam(cmdInsert, "DB_CRUSR", "TestUser");
                    AddParam(cmdInsert, "DB_TRDAT", DBNull.Value);
                    AddParam(cmdInsert, "DB_TRUSR", DBNull.Value);
                    ExecuteNonQuery(cmdInsert);
                    AfterInsertOne(value);
                    ts.Complete();
                    return 999;
                }
                catch (Exception error)
                {
                    bool handled;
                    OnInsertOneError(error, out handled);
                    if (!handled)
                        throw error;
                    return -2;
                }
            }
        }

        ///<summary>
        /// InsertOne 發生錯誤時會執行此方法
        ///</summary>
        ///<param name="handled">錯誤是否已經處理妥當</param>
        protected virtual void OnInsertOneError(Exception error, out bool handled)
        {
            handled = false;
        }

        ///<summary>
        /// InsertOne 之前會執行此方法
        /// 在這裡可以做一些檢查或是自動編號等動作
        /// 回傳 false 會打斷 InsertOne 的執行
        ///</summary>
        protected virtual bool BeforeInsertOne(Person value)
        {
            //decimal MaxId;
            //DbCommand cmdSelect = NewCommand();
            //cmdSelect.CommandText = "SELECT MAX(MACHINEID) " + _from;
            //ExecuteScalar(cmdSelect, out MaxId);
            //value.MACHINEID = MaxId + 1;
            return true;
        }

        ///<summary>
        /// InsertOne 之後會執行此方法
        ///</summary>
        protected virtual void AfterInsertOne(Person value)
        {
        }

        ///<summary>
        ///Person 資料集合類別
        ///</summary>
        [Serializable]
        public partial class DAC_PersonList : List<Person>, IDataList
        {
            public void Fill(DbDataReader reader)
            {
                FillData(reader, this);
            }
        }
    }
}