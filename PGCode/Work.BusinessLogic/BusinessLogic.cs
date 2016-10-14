﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Data;

using ProcCore.Business.Base;
using ProcCore.NetExtension;

using ProcCore.DatabaseCore;
using ProcCore.DatabaseCore.SQLContextHelp;
using ProcCore.DatabaseCore.DataBaseConnection;
using ProcCore.Business.Logic.TablesDescription;

namespace ProcCore.Business.Logic
{
    #region System Base Class

    #region News
    /// <summary>
    /// 查詢表單模組
    /// </summary>
    public class q_News : QueryBase
    {
        public DateTime s_setdate  { set; get; }
        public Boolean s_isopen { set; get; }
    }
    /// <summary>
    /// 系統資料結構描述模組    
    /// </summary>
    public class m_News : ModuleBase
    {
        public int Id { set; get; }
        public String Title { set; get; }
        public DateTime SetDate { set; get; }
        public bool IsOpen { set; get; }
        public String Context { set; get; }
    }
    /// <summary>
    /// The system database communicate module.   
    /// </summary>
    public class a_News : LogicBase<m_News, q_News, News>
    {
        /// <summary>
        /// NewsData Table進行新增資料動作。
        /// </summary>
        /// <param name="md">傳入m_NewsData class，class需先行建立(new)，其class各項屬性值需指定完成再行傳入</param>
        /// <param name="accountId">傳入Login使用者Id值</param>
        /// <returns>回傳RunInsertEnd class，請參閱RunInsertEnd說明。</returns>
        public override RunInsertEnd InsertMaster(m_News md, int accountId)
        {
            #region Variable declare area
            RunInsertEnd r = new RunInsertEnd(); //宣告回傳物件
            #endregion
            try
            {
                #region Main working
                Connection.BeginTransaction(); //開始交易鎖定

                TablePack<News> dataWork = new TablePack<News>(Connection) { LoginUserID = accountId };

                dataWork.NewRow(); //開始新橧作業 產生新的一行
                #region 指派值
                dataWork.SetDataRowValue(x => x.Id, md.Id);
                dataWork.SetDataRowValue(x => x.Title, md.Title);
                dataWork.SetDataRowValue(x => x.SetDate, md.SetDate);
                dataWork.SetDataRowValue(x => x.IsOpen, md.IsOpen);
                dataWork.SetDataRowValue(x => x.Context, md.Context);
                #endregion
                dataWork.AddRow(); //加載至DataTable
                dataWork.UpdateDataAdapter(); //更新 DataBase Server
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, "Insert DataTable");

                Connection.EndCommit(); //交易確認

                r.Rows = dataWork.AffetCount; //取得影響筆數
                r.InsertId = dataWork.InsertAutoFieldsID; //取得新增後欄位的值
                r.Result = true; //回傳本次執行結果為成功
                dataWork.Dispose(); //釋放資料

                return r;
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System;//系統執行失敗
                r.Message = PackErrMessage(ex); //回傳失敗訊息
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
        }
        /// <summary>
        /// NewsData Table進行更新資料動作。
        /// </summary>
        /// <param name="md">傳入m_NewsData class，class需先行建立(new)，其class各項屬性值需指定完成再行傳入</param>
        /// <param name="accountId">傳入Login使用者Id值</param>
        /// <returns>回傳RunUpdateEnd class，請參閱RunUpdateEnd說明。</returns>
        public override RunUpdateEnd UpdateMaster(m_News md, int accountId)
        {
            #region Variable declare area
            RunUpdateEnd r = new RunUpdateEnd();
            #endregion
            try
            {
                #region Main Working
                Connection.BeginTransaction();
                TablePack<News> dataWork = new TablePack<News>(Connection) { LoginUserID = accountId };

                dataWork.TableModule.KeyFieldModules[dataWork.TableModule.Id.N].V = md.Id; //取得ID欄位的值
                m_News md_Origin = dataWork.GetDataByKey<m_News>(); //取得Key值的Data
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, "Select Modily One Data");

                if (md_Origin == null) //如無資料
                    throw new LogicRoll("Log_Err_MustHaveData"); //此區一定有資料傳出，如無資料應檢查前端id值是否有誤

                dataWork.EditFirstRow(); //編輯第一筆資料，正常只會有一筆資料。
                #region 指派值
                dataWork.SetDataRowValue(x => x.Title, md.Title);
                dataWork.SetDataRowValue(x => x.SetDate, md.SetDate);
                dataWork.SetDataRowValue(x => x.IsOpen, md.IsOpen);
                dataWork.SetDataRowValue(x => x.Context, md.Context);
                #endregion
                md = null;
                dataWork.UpdateDataAdapter(); //進行變更至Database Server
                Connection.EndCommit();
                r.Rows = dataWork.AffetCount; //取得影響筆數
                r.Result = true;
                dataWork.Dispose(); //釋放資料
                return r;
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System;//系統執行失敗
                r.Message = PackErrMessage(ex); //回傳失敗訊息
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
        }
        /// <summary>
        /// NewsData Table進行刪除資料動作。
        /// </summary>
        /// <param name="deleteIds">傳入要刪除資料的主鍵值，此適用該Table只有單一主鍵欄位。</param>
        /// <param name="accountId">傳入Login使用者Id值</param>
        /// <returns>回傳RunDeleteEnd class，請參閱RunDeleteEnd說明。</returns>
        public override RunDeleteEnd DeleteMaster(Int32[] ids, int accountId)
        {
            //此功能主要搭配Grid介面刪除功能製作
            #region Variable declare area
            RunDeleteEnd r = new RunDeleteEnd(); //宣告刪除Result回物件
            #endregion
            try
            {
                #region Main working
                Connection.BeginTransaction();
                //1、要刪除的資料先選出來
                TablePack<News> dataWork = new TablePack<News>(Connection) { LoginUserID = accountId }; //宣告泛型物件並連接Connection
                dataWork.SelectFields(x => x.Id); //只Select 主Key欄位
                dataWork.WhereFields(x => x.Id, ids); //代入陣列Id值

                m_News[] md_Origin = dataWork.DataByAdapter<m_News>(); //取得Key值的Data

                //2、進行全部刪除
                dataWork.DeleteAll(); //先刪除DataTable
                dataWork.UpdateDataAdapter(); //在更新至DataBase Server
                Connection.EndCommit();
                r.Result = true;
                dataWork.Dispose(); //釋放資料
                md_Origin = null;
                return r;
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System;//系統執行失敗
                r.Message = PackErrMessage(ex); //回傳失敗訊息
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
        }
        /// <summary>
        /// NewsData Table進行動態多條件查詢資料動作。
        /// </summary>
        /// <param name="qr">>傳入q_NewsData class，class需先行建立(new)，其class各項屬性值需指定完成再行傳入</param>
        /// <param name="accountId">傳入Login使用者Id值</param>
        /// <returns>回傳RunQueryPackage<m_NewsData>m_NewsData</m_NewsData> class，請參閱RunQueryPackage說明。</returns>
        public override RunQueryPackage<m_News> SearchMaster(q_News qr, int accountId)
        {
            #region 變數宣告
            RunQueryPackage<m_News> r = new RunQueryPackage<m_News>();
            #endregion
            try
            {
                #region Select Data 區段 By 條件
                #region 設定輸出至Grid欄位 以下方式請注音 1、只適合單一Table 2、主要用於Grid顯示，如此方式不適合，可自行組SQL字串再夜由至ExecuteData達行

                TablePack<News> dataWork = new TablePack<News>(Connection) { LoginUserID = accountId, TableAlias = "A" };
                dataWork.SelectFields(x => new { x.Id, x.Title, x.SetDate, x.IsOpen, x.Context });
                #endregion
                #region 設定Where條件
                if( qr.s_isopen == true )
                    dataWork.WhereFields(x => x.IsOpen, qr.s_isopen, WhereCompareType.Equel);
                //if( qr.s_setdate != null)
                //    dataWork.WhereFields(x => x.SetDate,qr.s_setdate, WhereCompareType.ThanEquel);
                #endregion
                #region 設定排序
                dataWork.OrderByFields(x => x.SetDate, OrderByType.DESC);
                #endregion
                #region 輸出物件陣列
                r.SearchData = dataWork.DataByAdapter<m_News>();
                r.Result = true;
                return r;
                #endregion
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }

        }
        /// <summary>
        /// NewsData Table進行主鍵值查詢資料動作。
        /// </summary>
        /// <param name="id">傳入主鍵Value</param>
        /// <param name="accountId">傳入Login使用者Id值</param>
        /// <returns>回傳RunOneDataEnd<m_NewsData>m_NewsData</m_NewsData> class，請參閱RunOneDataEnd說明。</returns>
        public override RunOneDataEnd<m_News> GetDataMaster(int id, int accountId)
        {
            #region 變數宣告
            RunOneDataEnd<m_News> r = new RunOneDataEnd<m_News>();

            #endregion
            try
            {
                #region Main working
                TablePack<News> dataWork = new TablePack<News>(Connection) { LoginUserID = accountId };
                dataWork.TableModule.KeyFieldModules[dataWork.TableModule.Id.N].V = id; //設定KeyValue
                m_News md = dataWork.GetDataByKey<m_News>(); //取得Key該筆資料

                if (md == null) //如無資料
                    throw new LogicRoll("Log_Err_MustHaveData"); //此區一定有資料傳出，如無資料因檢查前端id值是否有誤

                r.SearchData = md;
                r.Result = true;
                dataWork.Dispose(); //釋放資料
                return r;
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System;//系統執行失敗
                r.Message = PackErrMessage(ex); //回傳失敗訊息
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
        }
        public override RunOneDataEnd<m_News> GetDataMaster(int[] id, int accountId)
        {
            throw new NotImplementedException();
        }
    }
    #endregion
    #region QATable
    /// <summary>
    /// 查詢表單模組
    /// </summary>
    public class q_QATable : QueryBase
    {
        public Boolean s_isopen { set; get; }
    }
    /// <summary>
    /// 系統資料結構描述模組    
    /// </summary>
    public class m_QATable : ModuleBase
    {
        public int Id { set; get; }
        public String Question { set; get; }
        public String Answer { set; get; }
        public bool IsOpen { set; get; }
        public int Sort { set; get; }
    }
    /// <summary>
    /// The system database communicate module.   
    /// </summary>
    public class a_QATable : LogicBase<m_QATable, q_QATable, QATable>
    {
        /// <summary>
        /// NewsData Table進行新增資料動作。
        /// </summary>
        /// <param name="md">傳入m_NewsData class，class需先行建立(new)，其class各項屬性值需指定完成再行傳入</param>
        /// <param name="accountId">傳入Login使用者Id值</param>
        /// <returns>回傳RunInsertEnd class，請參閱RunInsertEnd說明。</returns>
        public override RunInsertEnd InsertMaster(m_QATable md, int accountId)
        {
            #region Variable declare area
            RunInsertEnd r = new RunInsertEnd(); //宣告回傳物件
            #endregion
            try
            {
                #region Main working
                Connection.BeginTransaction(); //開始交易鎖定

                TablePack<QATable> dataWork = new TablePack<QATable>(Connection) { LoginUserID = accountId };

                dataWork.NewRow(); //開始新橧作業 產生新的一行
                #region 指派值
                dataWork.SetDataRowValue(x => x.Id, md.Id);
                dataWork.SetDataRowValue(x => x.Question, md.Question);
                dataWork.SetDataRowValue(x => x.Answer, md.Answer);
                dataWork.SetDataRowValue(x => x.IsOpen, md.IsOpen);
                dataWork.SetDataRowValue(x => x.Sort, md.Sort);
                #endregion
                dataWork.AddRow(); //加載至DataTable
                dataWork.UpdateDataAdapter(); //更新 DataBase Server
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, "Insert DataTable");

                Connection.EndCommit(); //交易確認

                r.Rows = dataWork.AffetCount; //取得影響筆數
                r.InsertId = dataWork.InsertAutoFieldsID; //取得新增後欄位的值
                r.Result = true; //回傳本次執行結果為成功
                dataWork.Dispose(); //釋放資料

                return r;
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System;//系統執行失敗
                r.Message = PackErrMessage(ex); //回傳失敗訊息
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
        }
        /// <summary>
        /// NewsData Table進行更新資料動作。
        /// </summary>
        /// <param name="md">傳入m_NewsData class，class需先行建立(new)，其class各項屬性值需指定完成再行傳入</param>
        /// <param name="accountId">傳入Login使用者Id值</param>
        /// <returns>回傳RunUpdateEnd class，請參閱RunUpdateEnd說明。</returns>
        public override RunUpdateEnd UpdateMaster(m_QATable md, int accountId)
        {
            #region Variable declare area
            RunUpdateEnd r = new RunUpdateEnd();
            #endregion
            try
            {
                #region Main Working
                Connection.BeginTransaction();
                TablePack<QATable> dataWork = new TablePack<QATable>(Connection) { LoginUserID = accountId };

                dataWork.TableModule.KeyFieldModules[dataWork.TableModule.Id.N].V = md.Id; //取得ID欄位的值
                m_QATable md_Origin = dataWork.GetDataByKey<m_QATable>(); //取得Key值的Data
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, "Select Modily One Data");

                if (md_Origin == null) //如無資料
                    throw new LogicRoll("Log_Err_MustHaveData"); //此區一定有資料傳出，如無資料應檢查前端id值是否有誤

                dataWork.EditFirstRow(); //編輯第一筆資料，正常只會有一筆資料。
                #region 指派值
                dataWork.SetDataRowValue(x => x.Question, md.Question);
                dataWork.SetDataRowValue(x => x.Answer, md.Answer);
                dataWork.SetDataRowValue(x => x.IsOpen, md.IsOpen);
                dataWork.SetDataRowValue(x => x.Sort, md.Sort);
                #endregion
                md = null;
                dataWork.UpdateDataAdapter(); //進行變更至Database Server
                Connection.EndCommit();
                r.Rows = dataWork.AffetCount; //取得影響筆數
                r.Result = true;
                dataWork.Dispose(); //釋放資料
                return r;
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System;//系統執行失敗
                r.Message = PackErrMessage(ex); //回傳失敗訊息
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
        }
        /// <summary>
        /// NewsData Table進行刪除資料動作。
        /// </summary>
        /// <param name="deleteIds">傳入要刪除資料的主鍵值，此適用該Table只有單一主鍵欄位。</param>
        /// <param name="accountId">傳入Login使用者Id值</param>
        /// <returns>回傳RunDeleteEnd class，請參閱RunDeleteEnd說明。</returns>
        public override RunDeleteEnd DeleteMaster(Int32[] ids, int accountId)
        {
            //此功能主要搭配Grid介面刪除功能製作
            #region Variable declare area
            RunDeleteEnd r = new RunDeleteEnd(); //宣告刪除Result回物件
            #endregion
            try
            {
                #region Main working
                Connection.BeginTransaction();
                //1、要刪除的資料先選出來
                TablePack<QATable> dataWork = new TablePack<QATable>(Connection) { LoginUserID = accountId }; //宣告泛型物件並連接Connection
                dataWork.SelectFields(x => x.Id); //只Select 主Key欄位
                dataWork.WhereFields(x => x.Id, ids); //代入陣列Id值

                m_QATable[] md_Origin = dataWork.DataByAdapter<m_QATable>(); //取得Key值的Data

                //2、進行全部刪除
                dataWork.DeleteAll(); //先刪除DataTable
                dataWork.UpdateDataAdapter(); //在更新至DataBase Server
                Connection.EndCommit();
                r.Result = true;
                dataWork.Dispose(); //釋放資料
                md_Origin = null;
                return r;
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System;//系統執行失敗
                r.Message = PackErrMessage(ex); //回傳失敗訊息
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
        }
        /// <summary>
        /// NewsData Table進行動態多條件查詢資料動作。
        /// </summary>
        /// <param name="qr">>傳入q_NewsData class，class需先行建立(new)，其class各項屬性值需指定完成再行傳入</param>
        /// <param name="accountId">傳入Login使用者Id值</param>
        /// <returns>回傳RunQueryPackage<m_NewsData>m_NewsData</m_NewsData> class，請參閱RunQueryPackage說明。</returns>
        public override RunQueryPackage<m_QATable> SearchMaster(q_QATable qr, int accountId)
        {
            #region 變數宣告
            RunQueryPackage<m_QATable> r = new RunQueryPackage<m_QATable>();
            #endregion
            try
            {
                #region Select Data 區段 By 條件
                #region 設定輸出至Grid欄位 以下方式請注音 1、只適合單一Table 2、主要用於Grid顯示，如此方式不適合，可自行組SQL字串再夜由至ExecuteData達行

                TablePack<QATable> dataWork = new TablePack<QATable>(Connection) { LoginUserID = accountId};
                dataWork.SelectFields(x => new { x.Id, x.Question, x.Answer, x.IsOpen, x.Sort });
                #endregion
                #region 設定Where條件
                if (qr.s_isopen == true)
                    dataWork.WhereFields(x => x.IsOpen, qr.s_isopen, WhereCompareType.Equel);
                #endregion
                #region 設定排序
                dataWork.OrderByFields(x => x.Sort, OrderByType.DESC);
                #endregion
                #region 輸出物件陣列
                r.SearchData = dataWork.DataByAdapter<m_QATable>();
                r.Result = true;
                return r;
                #endregion
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }

        }
        /// <summary>
        /// NewsData Table進行主鍵值查詢資料動作。
        /// </summary>
        /// <param name="id">傳入主鍵Value</param>
        /// <param name="accountId">傳入Login使用者Id值</param>
        /// <returns>回傳RunOneDataEnd<m_NewsData>m_NewsData</m_NewsData> class，請參閱RunOneDataEnd說明。</returns>
        public override RunOneDataEnd<m_QATable> GetDataMaster(int id, int accountId)
        {
            #region 變數宣告
            RunOneDataEnd<m_QATable> r = new RunOneDataEnd<m_QATable>();

            #endregion
            try
            {
                #region Main working
                TablePack<QATable> dataWork = new TablePack<QATable>(Connection) { LoginUserID = accountId };
                dataWork.TableModule.KeyFieldModules[dataWork.TableModule.Id.N].V = id; //設定KeyValue
                m_QATable md = dataWork.GetDataByKey<m_QATable>(); //取得Key該筆資料

                if (md == null) //如無資料
                    throw new LogicRoll("Log_Err_MustHaveData"); //此區一定有資料傳出，如無資料因檢查前端id值是否有誤

                r.SearchData = md;
                r.Result = true;
                dataWork.Dispose(); //釋放資料
                return r;
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System;//系統執行失敗
                r.Message = PackErrMessage(ex); //回傳失敗訊息
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
        }
        public override RunOneDataEnd<m_QATable> GetDataMaster(int[] id, int accountId)
        {
            throw new NotImplementedException();
        }
    }
    #endregion
    #region Address Logic Handle
    public class LogicAddress : LogicBase
    {
        public Dictionary<String, String> GetCity()
        {
            TablePack<_AddressCity> dataWork = new TablePack<_AddressCity>(Connection) { };

            dataWork.SelectFields(x => x.city);
            dataWork.OrderByFields(x => x.sort);
            return dataWork.DataByAdapter().dicMakeKeyValue(0, 0);
        }
        public Dictionary<String, String> GetCounty(String city)
        {
            TablePack<_AddressCounty> dataWork = new TablePack<_AddressCounty>(Connection) { };

            dataWork.SelectFields(x => x.county);
            dataWork.WhereFields(x => x.city, city);
            dataWork.OrderByFields(x => x.sort);
            return dataWork.DataByAdapter().dicMakeKeyValue(0, 0);
        }
        public String GetZip(String city, String county)
        {
            TablePack<_AddressCounty> dataWork = new TablePack<_AddressCounty>(Connection) { };

            dataWork.SelectFields(x => x.zip);
            dataWork.WhereFields(x => x.city, city);
            dataWork.WhereFields(x => x.county, county);

            DataTable dt = dataWork.DataByAdapter();
            return dt.Rows.Count > 0 ? dt.Rows[0][dataWork.TableModule.zip.N].ToString() : "";
        }
    }

    #endregion
    #region Code Sheet

    public class BaseSheet
    {
        public int CodeGroup { get; set; }
        public CommConnection Connection { get; set; }
        protected List<_Code> Codes { get; set; }

        public virtual List<_Code> MakeCodes()
        {
            return this.Codes;
        }
        public Dictionary<String, String> ToDictionary()
        {
            Dictionary<String, String> d = new Dictionary<String, String>();
            foreach (_Code _C in this.MakeCodes())
            {
                d.Add(_C.Code, _C.Value);
            }
            return d;
        }
    }
    public class _Code
    {
        public String Code { get; set; }
        public String Value { get; set; }
    }
    //=============================================================
    #region ReplayArea
    public static class CodeSheet
    {
        public static 消息分類 消息分類 = new 消息分類()
        {
            Active = new _Code() { Code = "Active", Value = "最新活動" },
            News = new _Code() { Code = "News", Value = "最新消息" },
            Post = new _Code() { Code = "Post", Value = "最新公告" }
        };
        public static 使用者狀態 使用者狀態 = new 使用者狀態()
        {
            Normal = new _Code() { Code = "Normal", Value = "正常" },
            Stop = new _Code() { Code = "Stop", Value = "停止" },
            Pause = new _Code() { Code = "Pause", Value = "暫停" }
        };
        public static 產品狀態 產品狀態 = new 產品狀態()
        {
            ComingSoon = new _Code() { Code = "ComingSoon", Value = "即將上市" },
            Normal = new _Code() { Code = "Normal", Value = "正常供貨" },
            ShortSupply = new _Code() { Code = "ShortSupply", Value = "缺貨" }
        };
        public static 會員狀態 會員狀態 = new 會員狀態()
        {
            Regist = new _Code() { Code = "Regist", Value = "註冊中" },
            Normal = new _Code() { Code = "Normal", Value = "正常" },
            Stop = new _Code() { Code = "Stop", Value = "停權" }
        };
        public static 訂單狀態 訂單狀態 = new 訂單狀態()
        {
            New = new _Code() { Code = "New", Value = "新訂單" },
            Handle = new _Code() { Code = "Handle", Value = "處理中" },
            Close = new _Code() { Code = "Close", Value = "完成" },
            Cancel = new _Code() { Code = "Cancel", Value = "取消" }
        };
        public static 上下架 上下架 = new 上下架()
        {
            OffShelf = new _Code() { Code = "OffShelf", Value = "下架" },
            OnShelf = new _Code() { Code = "OnShelf", Value = "上架" }
        };
        public static 付款方式 付款方式 = new 付款方式()
        {
            atm = new _Code() { Code = "atm", Value = "ATM轉帳" },
            pod = new _Code() { Code = "pod", Value = "貨到付款" }
        };
        public static 付款狀態 付款狀態 = new 付款狀態()
        {
            Waiting = new _Code() { Code = "Waiting", Value = "尚未付款" },
            Insufficient = new _Code() { Code = "Insufficient", Value = "付款不足" },
            Finish = new _Code() { Code = "Finish", Value = "完成付款" }
        };
    }
    public class 消息分類 : BaseSheet
    {
        public 消息分類() { this.CodeGroup = 1; }
        public _Code Active { get; set; }
        public _Code News { get; set; }
        public _Code Post { get; set; }
        public override List<_Code> MakeCodes()
        {
            this.Codes = new List<_Code>(); this.Codes.AddRange(new _Code[] { this.Active, this.News, this.Post }); return base.MakeCodes();
        }
    }
    public class 使用者狀態 : BaseSheet
    {
        public 使用者狀態() { this.CodeGroup = 2; }
        public _Code Normal { get; set; }
        public _Code Stop { get; set; }
        public _Code Pause { get; set; }
        public override List<_Code> MakeCodes()
        {
            this.Codes = new List<_Code>(); this.Codes.AddRange(new _Code[] { this.Normal, this.Stop, this.Pause }); return base.MakeCodes();
        }
    }
    public class 產品狀態 : BaseSheet
    {
        public 產品狀態() { this.CodeGroup = 3; }
        public _Code ComingSoon { get; set; }
        public _Code Normal { get; set; }
        public _Code ShortSupply { get; set; }
        public override List<_Code> MakeCodes()
        {
            this.Codes = new List<_Code>(); this.Codes.AddRange(new _Code[] { this.ComingSoon, this.Normal, this.ShortSupply }); return base.MakeCodes();
        }
    }
    public class 會員狀態 : BaseSheet
    {
        public 會員狀態() { this.CodeGroup = 4; }
        public _Code Regist { get; set; }
        public _Code Normal { get; set; }
        public _Code Stop { get; set; }
        public override List<_Code> MakeCodes()
        {
            this.Codes = new List<_Code>(); this.Codes.AddRange(new _Code[] { this.Regist, this.Normal, this.Stop }); return base.MakeCodes();
        }
    }
    public class 訂單狀態 : BaseSheet
    {
        public 訂單狀態() { this.CodeGroup = 5; }
        public _Code New { get; set; }
        public _Code Handle { get; set; }
        public _Code Close { get; set; }
        public _Code Cancel { get; set; }
        public override List<_Code> MakeCodes()
        {
            this.Codes = new List<_Code>(); this.Codes.AddRange(new _Code[] { this.New, this.Handle, this.Close, this.Cancel }); return base.MakeCodes();
        }
    }
    public class 上下架 : BaseSheet
    {
        public 上下架() { this.CodeGroup = 6; }
        public _Code OffShelf { get; set; }
        public _Code OnShelf { get; set; }
        public override List<_Code> MakeCodes()
        {
            this.Codes = new List<_Code>(); this.Codes.AddRange(new _Code[] { this.OffShelf, this.OnShelf }); return base.MakeCodes();
        }
    }
    public class 付款方式 : BaseSheet
    {
        public 付款方式() { this.CodeGroup = 7; }
        public _Code atm { get; set; }
        public _Code pod { get; set; }
        public override List<_Code> MakeCodes()
        {
            this.Codes = new List<_Code>(); this.Codes.AddRange(new _Code[] { this.atm, this.pod }); return base.MakeCodes();
        }
    }
    public class 付款狀態 : BaseSheet
    {
        public 付款狀態() { this.CodeGroup = 8; }
        public _Code Waiting { get; set; }
        public _Code Insufficient { get; set; }
        public _Code Finish { get; set; }
        public override List<_Code> MakeCodes()
        {
            this.Codes = new List<_Code>(); this.Codes.AddRange(new _Code[] { this.Waiting, this.Insufficient, this.Finish }); return base.MakeCodes();
        }
    }
    #endregion
    //=============================================================
    #endregion
    #region Boolean Sheet

    public class BooleanSheetBase
    {
        public String TrueValue { get; set; }
        public String FalseValue { get; set; }
        public String ToSQL { get; set; }
    }

    public static class BooleanSheet
    {
        #region ReplayArea

        public static BooleanSheetBase Boolean = new BooleanSheetBase()
        {
            TrueValue = "True",
            FalseValue = "False",
            ToSQL = "Select Boolean as id,Boolean as Value From _BooleanSheet"
        };


        public static BooleanSheetBase sex = new BooleanSheetBase()
        {
            TrueValue = "男",
            FalseValue = "女",
            ToSQL = "Select Boolean as id,sex as Value From _BooleanSheet"
        };


        public static BooleanSheetBase yn = new BooleanSheetBase()
        {
            TrueValue = "是",
            FalseValue = "否",
            ToSQL = "Select Boolean as id,yn as Value From _BooleanSheet"
        };


        public static BooleanSheetBase ynv = new BooleanSheetBase()
        {
            TrueValue = "✔",
            FalseValue = "",
            ToSQL = "Select Boolean as id,ynv as Value From _BooleanSheet"
        };


        public static BooleanSheetBase ynvx = new BooleanSheetBase()
        {
            TrueValue = "✔",
            FalseValue = "✕",
            ToSQL = "Select Boolean as id,ynvx as Value From _BooleanSheet"
        };

        #endregion
    }

    #endregion
    #region System Program Menu

    public class SYSMenu : LogicBase
    {
        private int _LoginUserID;
        private String _WebAppPath;

        public SYSMenu(int LoginUserID, String WebAppPath, CommConnection conn)
        {
            _LoginUserID = LoginUserID;
            _WebAppPath = WebAppPath;
            this.Connection = conn;

            ProgData TObj = new ProgData();
            TablePack<ProgData> dataWork = new TablePack<ProgData>(Connection) { TableModule = TObj };
            dataWork.WhereFields(x => x.ishidden, false);
            dataWork.WhereFields(x => x.isfolder, true);
            dataWork.OrderByFields(x => x.sort);

            DataTable dt = dataWork.DataByAdapter();

            List<MenuFoler> menuFolder = new List<MenuFoler>();

            foreach (DataRow dr in dt.Rows)
            {
                MenuFoler Folder = new MenuFoler(this._LoginUserID, WebAppPath);
                Folder.Connection = this.Connection;
                Folder.prod_id = dr[TObj.id.N].ToString();
                Folder.AllowMobile = (Boolean)dr[TObj.ismb.N];

                if (Folder.GetMenuItem.Count() > 0)
                {
                    menuFolder.Add(Folder);
                }
                this.GetMenuFolder = menuFolder.ToArray();
            }
        }

        public MenuFoler[] GetMenuFolder { get; set; }
    }
    public class MenuFoler : LogicBase
    {
        private String _id;
        private String _WebAppPath;
        private int _LoginUserID;

        public MenuFoler(int LoginUserID, string WebAppPath)
        {
            _WebAppPath = WebAppPath;
            _LoginUserID = LoginUserID;
        }

        public String prod_id
        {
            get
            {
                return _id;
            }
            set
            {
                this._id = value;
                ProgData TObj = new ProgData();
                TablePack<ProgData> dataWork = new TablePack<ProgData>(Connection) { TableModule = TObj };
                TObj.KeyFieldModules[TObj.id.N].V = this._id;

                DataTable dt = dataWork.GetDataByKey();

                FolderName = dt.Rows[0][TObj.prog_name.N].ToString();
                Sort = dt.Rows[0][TObj.sort.N].ToString();

                dataWork = null;
                dataWork = new TablePack<ProgData>(Connection) { TableModule = TObj };
                //dataWork.Reset();
                dataWork.WhereFields(x => x.ishidden, false);
                dataWork.WhereFields(x => x.isfolder, false);
                dataWork.WhereFields(x => x.sort, Sort, WhereCompareType.LikeRight);
                dataWork.OrderByFields(x => x.sort);

                DataTable item = dataWork.DataByAdapter();

                List<MenuItem> Item = new List<MenuItem>();

                Int32[] 管理者專用系統Id = new Int32[] { 3, 4, 5, 7 };

                foreach (DataRow q in item.Rows)
                {
                    PowerHave pHave = new PowerHave();
                    pHave.Connection = this.Connection;
                    pHave.SetPower(this._LoginUserID, q[TObj.id.N].CInt());

                    if (pHave.PowerDataSet.GetPower(PowersName.瀏覽).HavePower)
                    {
                        MenuItem mItem = new MenuItem(_WebAppPath);
                        if (管理者專用系統Id.Contains(q[TObj.id.N].CInt()))
                        {
                            if (_LoginUserID == 1)
                            {
                                mItem.Connection = this.Connection;
                                mItem.prod_id = q[TObj.id.N].ToString();
                                mItem.AllowMobile = (Boolean)q[TObj.ismb.N];
                                Item.Add(mItem);
                            }
                        }
                        else
                        {
                            mItem.Connection = this.Connection;
                            mItem.prod_id = q[TObj.id.N].ToString();
                            mItem.AllowMobile = (Boolean)q[TObj.ismb.N];
                            Item.Add(mItem);
                        }


                    }
                }
                this.GetMenuItem = Item.ToArray();
            }
        }
        public String FolderName { get; set; }
        public String Sort { get; set; }
        public String Link { get; set; }
        public Boolean AllowMobile { get; set; }
        public MenuItem[] GetMenuItem { get; set; }
    }
    public class MenuItem : LogicBase
    {
        string _WebAppPath = String.Empty;
        public MenuItem(String WebAppPath)
        {
            _WebAppPath = WebAppPath;
        }
        private String _id;
        public String prod_id
        {
            get
            {
                return _id;
            }
            set
            {
                this._id = value;
                ProgData TObj = new ProgData();
                TablePack<ProgData> dataWork = new TablePack<ProgData>(Connection) { TableModule = TObj };
                TObj.KeyFieldModules[TObj.id.N].V = this._id;

                DataTable dt = dataWork.GetDataByKey();

                this.Link = _WebAppPath + dt.Rows[0][TObj.area.N].ToString() + "/" + dt.Rows[0][TObj.controller.N].ToString() + "/" + dt.Rows[0][TObj.action.N].ToString();
                this.ItemName = dt.Rows[0][TObj.prog_name.N].ToString();
            }
        }
        public String ItemName { get; set; }
        public String Link { get; set; }
        public Boolean AllowMobile { get; set; }
    }

    #endregion
    #region Power
    public enum PowersName
    {
        完全控制 = 0, 管理 = 1, 瀏覽 = 2, 新增 = 3, 修改 = 4, 刪除 = 5, 審核 = 6, 回覆 = 7
    }
    public class Power
    {
        public int Id { get; set; }
        public PowersName name { get; set; }

        public int ManagementIntSerial { get; set; }
        public Boolean IsManagement { get; set; }
        public Boolean HavePower { get; set; }
    }
    public class PowerData
    {
        private List<Power> ls_Powers;

        public PowerData()
        {
            ls_Powers = new List<Power>();

            #region 設定權限核心資料
            //String[] GetPowersName = Enum.GetNames(typeof(PowersName));
            for (int i = 0; i < Enum.GetNames(typeof(PowersName)).Length; i++)
            {
                Power p = new Power { Id = i + 1, name = (PowersName)i, ManagementIntSerial = System.Math.Pow(2, i).CInt() };
                ls_Powers.Add(p);
            }

            Powers = ls_Powers.ToArray();
            #endregion
        }

        public Power[] Powers { get; set; }

        public Power GetPower(PowersName p)
        {
            return ls_Powers.Where(x => x.name == p).FirstOrDefault();
        }
    }
    public class PowerManagement : PowerData
    {
        int _PowerInt;

        public PowerManagement()
            : base()
        {

        }

        public PowerManagement(int GetPowerInt)
            : base()
        {
            this._PowerInt = GetPowerInt;
            foreach (Power p in Powers)
            {
                p.IsManagement = (p.ManagementIntSerial & this.PowerInt) > 0;
            }
        }

        public int PowerInt
        {
            get { return this._PowerInt; }
            set
            {
                this._PowerInt = value;
                foreach (Power p in Powers)
                {
                    p.IsManagement = (p.ManagementIntSerial & this.PowerInt) > 0;
                }
            }
        }
        public int Unit { get; set; }
    }
    public class PowerHave : LogicBase
    {
        public PowerHave()
        {
            PowerDataSet = new PowerData();
        }

        public void SetPower(int UserId, int ProgId)
        {
            a_Users ac = new a_Users();
            m_Users md;

            ac.Connection = this.Connection;
            RunOneDataEnd<m_Users> HResult = ac.GetDataMaster(UserId, 0);

            md = HResult.SearchData;

            if (md.isadmin)
                foreach (Power p in this.PowerDataSet.Powers)
                    p.HavePower = true;


            else
            {
                String sql = String.Format("Select PowerID From _PowerUnit Where UnitID={0} and ProgID={1}", md.unit, ProgId);
                DataTable dt = ExecuteData(sql);

                foreach (DataRow dr in dt.Rows)
                {
                    int powerId = dr["PowerID"].CInt();

                    if (powerId == 1)
                    {
                        foreach (Power p in this.PowerDataSet.Powers)
                        {
                            p.HavePower = true;
                        }
                    }

                    //if (powerId == 2) { this.HaveManagementPower = true; }
                    //if (powerId == 3) { this.HaveListPower = true; }
                    //if (powerId == 4) { this.HaveInsertPower = true; }
                    //if (powerId == 5) { this.HaveModifyPower = true; }
                    //if (powerId == 6) { this.HaveDeletePower = true; }
                    //if (powerId == 7) { this.HaveVerifyPower = true; }
                    //if (powerId == 8) { this.HaveReplayPower = true; }

                    this.PowerDataSet.Powers[powerId - 1].HavePower = true;
                }

                dt.Dispose();
                dt = null;
            }
        }

        /// <summary>
        /// 陣列式權限處理
        /// </summary>
        public PowerData PowerDataSet { get; set; }

        #region 基礎權限名稱
        //public Boolean HaveCompleteControlPower { get; set; }
        //public Boolean HaveManagementPower        {            get;            set;        }
        //public Boolean HaveListPower        {            get;            set;        }
        //public Boolean HaveInsertPower        {            get;            set;        }
        //public Boolean HaveModifyPower        {            get;            set;        }
        //public Boolean HaveDeletePower        {            get;            set;        }
        //public Boolean HaveVerifyPower        {            get;            set;        }
        //public Boolean HaveReplayPower        {            get;            set;        }
        #endregion
    }
    #endregion
    #region Power for unit
    public class q_PowerUnit : QueryBase
    {
        public int Unit { get; set; }
    }
    public class a_PowerUnit : LogicBase
    {
        public RunUpdateEnd UpdateMaster(m_PowerUnit md, string AccountID)
        {
            RunUpdateEnd r = new RunUpdateEnd();
            _PowerUnit TObj = new _PowerUnit();

            try
            {
                Connection.BeginTransaction();
                TablePack<_PowerUnit> dataWork = new TablePack<_PowerUnit>(Connection) { TableModule = TObj };
                if (md.Checked)
                {
                    //新增
                    dataWork.NewRow();
                    dataWork.SetDataRowValue(x => x.ProgID, md.prog);
                    dataWork.SetDataRowValue(x => x.PowerID, md.power);
                    dataWork.SetDataRowValue(x => x.UnitID, md.unit);
                    dataWork.SetDataRowValue(x => x.AccessUnit, 1);
                    dataWork.AddRow();
                    dataWork.UpdateDataAdapter();
                }
                else
                {
                    //刪除

                    dataWork.WhereFields(x => x.ProgID, md.prog, WhereCompareType.Equel);
                    dataWork.WhereFields(x => x.PowerID, md.power, WhereCompareType.Equel);
                    dataWork.WhereFields(x => x.UnitID, md.unit, WhereCompareType.Equel);

                    DataTable dt_Origin = dataWork.DataByAdapter();
                    dataWork.DeleteAll();
                    dataWork.UpdateDataAdapter();
                }

                Connection.EndCommit();
                r.Result = true;
                return r;
            }
            catch (LogicRoll ex)
            {
                Connection.Rollback();

                r.Result = false;
                r.ErrType = BusinessErrType.Logic;
                r.Message = ex.Message;
                return r;
            }
            catch (Exception ex)
            {
                Connection.Rollback();
                r.Result = false;
                r.ErrType = BusinessErrType.System;
                r.Message = PackErrMessage(ex);
                return r;
            }
            finally
            {

            }

        }
        public RunQueryEnd SearchMaster(q_PowerUnit qr, int AccountID)
        {
            RunQueryEnd r = new RunQueryEnd();
            _PowerUnit TObj = new _PowerUnit();

            try
            {
                ProgData ProgWork = new ProgData();
                DataTable dt = new DataTable();

                dt.Columns.Add(new DataColumn(ProgWork.id.N, typeof(int)) { AllowDBNull = false });
                dt.Columns.Add(new DataColumn(ProgWork.prog_name.N, typeof(String)) { AllowDBNull = false });

                PowerManagement pCollect = new PowerManagement();

                foreach (Power p in pCollect.Powers)
                    dt.Columns.Add(new DataColumn(p.name.ToString(), typeof(String)) { AllowDBNull = false, DefaultValue = "" });

                TablePack<ProgData> dataWork = new TablePack<ProgData>(Connection) { TableModule = new ProgData() };
                dataWork.SelectFields(x => new { x.id, x.prog_name, x.isfolder, x.power_serial });
                dataWork.WhereFields(x => x.isfolder, false);
                dataWork.OrderByFields(x => x.sort);

                DataTable dt_ProgData = dataWork.DataByAdapter();

                TablePack<_PowerUnit> PowerUnitWork = new TablePack<_PowerUnit>(Connection) { TableModule = new _PowerUnit() };
                PowerUnitWork.SelectFields(x => x.ProgID);
                PowerUnitWork.SelectFields(x => x.PowerID);
                PowerUnitWork.WhereFields(x => x.UnitID, qr.Unit);

                var dt_PowerQuery = PowerUnitWork.DataByAdapter().AsEnumerable();

                foreach (DataRow dr in dt_ProgData.Rows)
                {
                    DataRow NewDR = dt.NewRow();

                    pCollect.PowerInt = dr[ProgWork.power_serial.N].CInt();
                    NewDR[ProgWork.prog_name.N] = dr[ProgWork.prog_name.N];
                    NewDR[ProgWork.id.N] = dr[ProgWork.id.N];

                    foreach (Power p in pCollect.Powers)
                    {
                        IEnumerable<DataRow> query = dt_PowerQuery
                            .Where(x => x.Field<int>(TObj.ProgID.N) == dr[ProgWork.id.N].CInt() &&
                                x.Field<Int16>(TObj.PowerID.N) == p.Id);

                        NewDR[p.name.ToString()] = "{\"PowerID\":" + p.Id.ToString() +
                            ",\"ShowPower\":" + p.IsManagement.BooleanValue("true", "false") +
                            ",\"HavePower\":" + (query.Count() > 0).BooleanValue("true", "false") + "}";
                    }
                    dt.Rows.Add(NewDR);
                }

                r.SearchData = dt;

                r.Result = true;
                return r;
            }
            catch (LogicRoll ex)
            {
                r.Result = false;
                r.ErrType = BusinessErrType.Logic;
                r.Message = ex.Message;
                return r;
            }
            catch (Exception ex)
            {
                r.Result = false;
                r.ErrType = BusinessErrType.System;
                r.Message = PackErrMessage(ex);
                return r;
            }

        }
        public Dictionary<String, String> CollectKeyValueData_Unit()
        {
            String sql = String.Empty;
            sql = string.Format("Select id,name From UnitData Order By sort");
            DataTable dt = ExecuteData(sql);

            Dictionary<String, String> Key_Value = new Dictionary<String, String>();
            foreach (DataRow dr in dt.Rows)
            {
                Key_Value.Add(dr[0].ToString(), dr[1].ToString());
            }

            return Key_Value;
        }
    }
    public class m_PowerUnit : ModuleBase
    {
        public int unit { get; set; }
        public int prog { get; set; }
        public int power { get; set; }
        public Boolean Checked { get; set; }
    }
    #endregion
    #region Power for users
    public class q_PowerUser : QueryBase
    {
        public int user { get; set; }
    }
    public class a_PowerUser : LogicBase
    {
        public RunUpdateEnd UpdateMaster(m_PowerUser md, string AccountID)
        {
            RunUpdateEnd r = new RunUpdateEnd();
            _PowerUsers TObj = new _PowerUsers();
            try
            {
                Connection.BeginTransaction();
                TablePack<_PowerUsers> dataWork = new TablePack<_PowerUsers>(Connection) { TableModule = TObj };
                if (md.Checked)
                {
                    //新增
                    dataWork.NewRow();
                    dataWork.SetDataRowValue(x => x.ProgID, md.prog);
                    dataWork.SetDataRowValue(x => x.PowerID, md.power);
                    dataWork.SetDataRowValue(x => x.UserID, md.user);
                    dataWork.SetDataRowValue(x => x.UnitID, 1);
                    dataWork.AddRow();
                    dataWork.UpdateDataAdapter();
                }
                else
                {
                    //刪除
                    dataWork.WhereFields(x => x.ProgID, md.prog);
                    dataWork.WhereFields(x => x.PowerID, md.power);
                    dataWork.WhereFields(x => x.UserID, md.user);

                    DataTable dt_Origin = dataWork.DataByAdapter();
                    dataWork.DeleteAll();
                    dataWork.UpdateDataAdapter();
                }

                Connection.EndCommit();
                r.Result = true;
                return r;
            }
            catch (LogicRoll ex)
            {
                Connection.Rollback();

                r.Result = false;
                r.ErrType = BusinessErrType.Logic;
                r.Message = ex.Message;
                return r;
            }
            catch (Exception ex)
            {
                Connection.Rollback();
                r.Result = false;
                r.ErrType = BusinessErrType.System;
                r.Message = PackErrMessage(ex);
                return r;
            }
            finally
            {

            }

        }
        public RunQueryEnd SearchMaster(q_PowerUser qr, string AccountID)
        {
            RunQueryEnd r = new RunQueryEnd();
            _PowerUsers TObj = new _PowerUsers();

            try
            {
                ProgData ProgWork = new ProgData();
                DataTable dt = new DataTable();

                dt.Columns.Add(new DataColumn(ProgWork.id.N, typeof(int)) { AllowDBNull = false });
                dt.Columns.Add(new DataColumn(ProgWork.prog_name.N, typeof(String)) { AllowDBNull = false });

                PowerManagement pCollect = new PowerManagement();

                foreach (Power p in pCollect.Powers)
                    dt.Columns.Add(new DataColumn(p.name.ToString(), typeof(String)) { AllowDBNull = false, DefaultValue = "" });

                TablePack<ProgData> dataWork = new TablePack<ProgData>(Connection) { TableModule = new ProgData() };
                dataWork.SelectFields(x => new { x.id, x.prog_name, x.isfolder, x.power_serial });
                dataWork.WhereFields(x => x.isfolder, false);
                dataWork.OrderByFields(x => x.sort);

                DataTable dt_ProgData = dataWork.DataByAdapter();

                TablePack<_PowerUsers> PowerUnitWork = new TablePack<_PowerUsers>(Connection) { TableModule = new _PowerUsers() };
                PowerUnitWork.SelectFields(x => new { x.ProgID, x.PowerID });
                PowerUnitWork.WhereFields(x => x.UserID, qr.user);

                DataTable dt_PowerQuery = PowerUnitWork.DataByAdapter();

                foreach (DataRow dr in dt_ProgData.Rows)
                {
                    DataRow NewDR = dt.NewRow();

                    pCollect.PowerInt = dr[ProgWork.power_serial.N].CInt();
                    NewDR[ProgWork.prog_name.N] = dr[ProgWork.prog_name.N];
                    NewDR[ProgWork.id.N] = dr[ProgWork.id.N];

                    foreach (Power p in pCollect.Powers)
                    {
                        IEnumerable<DataRow> query =
                        from PowerUnit in dt_PowerQuery.AsEnumerable()
                        where
                        PowerUnit.Field<Int32>(TObj.ProgID.N) == dr[ProgWork.id.N].CInt() &&
                        PowerUnit.Field<Int16>(TObj.PowerID.N) == p.Id
                        select PowerUnit;

                        NewDR[p.name.ToString()] = "{\"PowerID\":" + p.Id.ToString() + ",\"ShowPower\":" + p.IsManagement.BooleanValue("true", "false") + ",\"HavePower\":" + (query.Count() > 0).BooleanValue("true", "false") + "}";
                    }
                    dt.Rows.Add(NewDR);
                }

                r.SearchData = dt;
                r.Result = true;

                return r;
            }
            catch (LogicRoll ex)
            {
                r.Result = false;
                r.ErrType = BusinessErrType.Logic;
                r.Message = ex.Message;
                return r;
            }
            catch (Exception ex)
            {
                r.Result = false;
                r.ErrType = BusinessErrType.System;
                r.Message = PackErrMessage(ex);
                return r;
            }

        }
        public Dictionary<String, String> CollectKeyValueData_Users()
        {
            UsersData TObj = new UsersData();
            TablePack<UsersData> dataWork = new TablePack<UsersData>(Connection) { TableModule = TObj };

            dataWork.SelectFields(x => x.id);
            dataWork.SelectFields(x => x.name);
            dataWork.OrderByFields(x => x.id, OrderByType.DESC);

            DataTable dt = dataWork.DataByAdapter();
            return dt.dicMakeKeyValue(0, 1);
        }
    }
    public class m_PowerUser : ModuleBase
    {
        public int prog { get; set; }
        public int power { get; set; }
        public int user { get; set; }

        public Boolean Checked { get; set; }
    }
    #endregion
    #region ProgData
    public class a_WebInfo : LogicBase
    {
        public m_ProgData GetSystemInfo(String area, String controller, String action)
        {
            m_ProgData md = new m_ProgData();

            ProgData TObj = new ProgData();
            TablePack<ProgData> dataWork = new TablePack<ProgData>(Connection) { TableModule = TObj };

            if (!String.IsNullOrEmpty(area))
            {
                dataWork.WhereFields(x => x.area, area);
            }

            if (!String.IsNullOrEmpty(controller))
            {
                dataWork.WhereFields(x => x.controller, controller);
            }

            if (!String.IsNullOrEmpty(action))
            {
                dataWork.WhereFields(x => x.action, action);
            }

            DataTable dt = dataWork.DataByAdapter();
            if (dt.Rows.Count > 0)
            {
                md.prog_name = dt.Rows[0][TObj.prog_name.N].ToString();
                md.id = dt.Rows[0][TObj.id.N].CInt();
                return md;
            }
            else
            {
                return md;
            }
        }
    }
    public class q_ProgData : QueryBase
    {
        public string s_prog_name { set; get; }
        public string s_controller { set; get; }
        public string s_area { set; get; }
    }
    public class n_ProgData : SubQueryBase
    {
    }
    public class m_ProgData : ModuleBase
    {
        public int id { get; set; }
        public String area { get; set; }
        public String controller { get; set; }
        public String action { get; set; }
        public String path { get; set; }
        public String page { get; set; }
        public String prog_name { get; set; }
        public String sort { get; set; }
        public Boolean isfolder { get; set; }
        public Boolean ishidden { get; set; }
        public Boolean isRoute { get; set; }
        public Boolean ismb { get; set; }
        public int power_serial { get; set; }
        public PowerManagement PowerItem { get; set; }
        public List<int> GetPowerItems { get; set; }

    }
    public class a_ProgData : LogicBase<m_ProgData, q_ProgData, ProgData>
    {
        public override RunInsertEnd InsertMaster(m_ProgData md, int AccountID)
        {
            #region Declare area
            RunInsertEnd r = new RunInsertEnd(); //宣告回傳物件
            #endregion
            try
            {
                #region main working
                Connection.BeginTransaction(); //開始交易鎖定
                TablePack<ProgData> dataWork = new TablePack<ProgData>(Connection) { LoginUserID = AccountID };

                dataWork.NewRow(); //開始新橧作業 產生新的一行
                #region 指派值

                dataWork.SetDataRowValue(x => x.id, md.id);
                dataWork.SetDataRowValue(x => x.area, md.area);
                dataWork.SetDataRowValue(x => x.controller, md.controller);
                dataWork.SetDataRowValue(x => x.action, md.action);
                dataWork.SetDataRowValue(x => x.path, md.path);
                dataWork.SetDataRowValue(x => x.page, md.page);
                dataWork.SetDataRowValue(x => x.prog_name, md.prog_name);
                dataWork.SetDataRowValue(x => x.sort, md.sort);
                dataWork.SetDataRowValue(x => x.isfolder, md.isfolder);
                dataWork.SetDataRowValue(x => x.ishidden, md.ishidden);
                dataWork.SetDataRowValue(x => x.isRoute, md.isRoute);

                foreach (int i in md.GetPowerItems)
                {
                    md.power_serial += i;
                }

                dataWork.SetDataRowValue(x => x.power_serial, md.power_serial);
                dataWork.UpdateFieldsInfo(UpdateFieldsInfoType.Insert);
                #endregion
                dataWork.AddRow(); //加載至DataTable
                dataWork.UpdateDataAdapter(); //更新 DataBase Server

                Connection.EndCommit(); //交易確認

                r.Rows = dataWork.AffetCount; //取得影響筆數
                r.InsertId = dataWork.InsertAutoFieldsID; //取得新增後自動新增欄位的值
                r.Result = true; //回傳本次執行結果為成功

                return r;
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System;//系統執行失敗
                r.Message = PackErrMessage(ex); //回傳失敗訊息
                return r;
                #endregion
            }
        }
        public override RunUpdateEnd UpdateMaster(m_ProgData md, int AccountID)
        {
            RunUpdateEnd r = new RunUpdateEnd();
            try
            {
                Connection.BeginTransaction();
                TablePack<ProgData> dataWork = new TablePack<ProgData>(Connection) { LoginUserID = AccountID };

                dataWork.TableModule.KeyFieldModules[dataWork.TableModule.id.N].V = md.id; //取得ID欄位的值
                DataTable dt_Origin = dataWork.GetDataByKey(); //取得Key值的Data

                dataWork.EditFirstRow();
                #region 指派值
                dataWork.SetDataRowValue(x => x.area, md.area);
                dataWork.SetDataRowValue(x => x.controller, md.controller);
                dataWork.SetDataRowValue(x => x.action, md.action);
                dataWork.SetDataRowValue(x => x.path, md.path);
                dataWork.SetDataRowValue(x => x.page, md.page);
                dataWork.SetDataRowValue(x => x.prog_name, md.prog_name);
                dataWork.SetDataRowValue(x => x.sort, md.sort);
                dataWork.SetDataRowValue(x => x.isfolder, md.isfolder);
                dataWork.SetDataRowValue(x => x.ishidden, md.ishidden);
                dataWork.SetDataRowValue(x => x.isRoute, md.isRoute);

                foreach (int i in md.GetPowerItems)
                {
                    md.power_serial += i;
                }

                dataWork.SetDataRowValue(x => x.power_serial, md.power_serial);
                dataWork.UpdateFieldsInfo(UpdateFieldsInfoType.Update);
                #endregion
                dt_Origin.Dispose();
                dataWork.UpdateDataAdapter();

                Connection.EndCommit();

                r.Rows = dataWork.AffetCount;
                r.Result = true;

                return r;
            }
            catch (LogicRoll ex)
            {
                Connection.Rollback();

                r.Result = false;
                r.ErrType = BusinessErrType.Logic;
                r.Message = ex.Message;
                return r;
            }
            catch (Exception ex)
            {
                Connection.Rollback();
                r.Result = false;
                r.ErrType = BusinessErrType.System;
                r.Message = PackErrMessage(ex);
                return r;
            }
            finally
            {
            }

        }
        public override RunDeleteEnd DeleteMaster(Int32[] ids, int AccountID)
        {
            RunDeleteEnd r = new RunDeleteEnd();
            try
            {
                Connection.BeginTransaction();
                //1、要刪除的資料先選出來
                TablePack<ProgData> dataWork = new TablePack<ProgData>(Connection) { LoginUserID = AccountID };
                dataWork.SelectFields(x => x.id);
                dataWork.WhereFields(x => x.id, ids);

                DataTable dt_Origin = dataWork.DataByAdapter();

                //2、進行全部刪除
                dataWork.DeleteAll(); //先刪除DataTable
                dataWork.UpdateDataAdapter(); //在更新至DataBase Server
                Connection.EndCommit();
                r.Result = true;
                return r;
            }
            catch (LogicRoll ex)
            {
                Connection.Rollback();
                r.Result = false;
                r.ErrType = BusinessErrType.Logic;
                r.Message = ex.Message;
                return r;
            }
            catch (Exception ex)
            {
                Connection.Rollback();
                r.Result = false;
                r.ErrType = BusinessErrType.System;
                r.Message = PackErrMessage(ex);
                return r;
            }
        }

        public override RunQueryPackage<m_ProgData> SearchMaster(q_ProgData qr, int AccountID)
        {
            #region 全域變數宣告
            RunQueryPackage<m_ProgData> r = new RunQueryPackage<m_ProgData>();
            #endregion
            try
            {
                #region Select Data 區段 By 條件
                #region 設定輸出至Grid欄位 以下方式請注音 1、只適合單一Table 2、主要用於Grid顯示，如此方式不適合，可自行組SQL字串再夜由至ExecuteData達行
                TablePack<ProgData> dataWork = new TablePack<ProgData>(Connection) { LoginUserID = AccountID };

                dataWork.SelectFields(x => new { x.id, x.prog_name, x.isfolder, x.area, x.controller, x.sort, x.ismb });
                dataWork.TopLimit = 1000;
                #endregion
                #region 設定Where條件
                if (qr.s_prog_name != null)
                {
                    dataWork.WhereFields(x => x.prog_name, qr.s_prog_name, WhereCompareType.Like);
                }
                #endregion
                #region 設定排序
                if (qr.sidx == null)
                {
                    //預設排序
                    dataWork.OrderByFields(x => x.sort, OrderByType.ASC);
                }
                else
                {
                    dataWork.OrderByFields(x => x.sort, OrderByType.ASC);
                }
                #endregion
                #region 輸出成DataTable
                r.SearchData = dataWork.DataByAdapter<m_ProgData>();
                r.Result = true;
                return r;
                #endregion
                #endregion
            }
            catch (LogicRoll ex)
            {
                r.Result = false;
                r.ErrType = BusinessErrType.Logic;
                r.Message = ex.Message;
                return r;
            }
            catch (Exception ex)
            {
                r.Result = false;
                r.ErrType = BusinessErrType.System;
                r.Message = PackErrMessage(ex);
                return r;
            }

        }
        public RunQueryPackage<m_ProgData> SearchMasterLVL1(int AccountID)
        {
            #region 全域變數宣告
            RunQueryPackage<m_ProgData> r = new RunQueryPackage<m_ProgData>();
            #endregion
            try
            {
                #region Select Data 區段 By 條件
                #region 設定輸出至Grid欄位 以下方式請注音 1、只適合單一Table 2、主要用於Grid顯示，如此方式不適合，可自行組SQL字串再夜由至ExecuteData達行
                TablePack<ProgData> dataWork = new TablePack<ProgData>(Connection) { LoginUserID = AccountID };

                dataWork.SelectFields(x => x.id);
                dataWork.SelectFields(x => x.prog_name);
                dataWork.SelectFields(x => x.isfolder);
                dataWork.SelectFields(x => x.area);
                dataWork.SelectFields(x => x.controller);
                dataWork.SelectFields(x => x.sort);
                dataWork.SelectFields(x => x.ismb);
                dataWork.TopLimit = 100;
                #endregion
                #region 設定Where條件
                dataWork.WhereFields(x => x.isfolder, true);
                #endregion
                #region 設定排序
                dataWork.OrderByFields(x => x.sort);
                #endregion
                #region 輸出成DataTable
                r.SearchData = dataWork.DataByAdapter<m_ProgData>();
                r.Result = true;
                return r;
                #endregion
                #endregion
            }
            catch (LogicRoll ex)
            {
                r.Result = false;
                r.ErrType = BusinessErrType.Logic;
                r.Message = ex.Message;
                return r;
            }
            catch (Exception ex)
            {
                r.Result = false;
                r.ErrType = BusinessErrType.System;
                r.Message = PackErrMessage(ex);
                return r;
            }

        }
        public RunQueryPackage<m_ProgData> SearchMasterLVL2(n_ProgData qr, int AccountID)
        {
            #region 全域變數宣告
            RunQueryPackage<m_ProgData> r = new RunQueryPackage<m_ProgData>();
            #endregion
            try
            {
                #region Select Data 區段 By 條件
                #region 設定輸出至Grid欄位 以下方式請注意 1、只適合單一Table 2、主要用於Grid顯示，如此方式不適合，可自行組SQL字串再夜由至ExecuteData達行
                TablePack<ProgData> dataWork = new TablePack<ProgData>(Connection) { LoginUserID = AccountID };

                dataWork.SelectFields(x => x.sort);
                dataWork.TableModule.KeyFieldModules[dataWork.TableModule.id.N].V = qr.id;
                dataWork.TopLimit = 1;
                #endregion
                #region 設定Where條件
                #endregion

                #region 輸出Class
                m_ProgData md = dataWork.GetDataByKey<m_ProgData>();

                dataWork.Reset();
                dataWork.SelectFields(x => new { x.id, x.prog_name, x.isfolder, x.area, x.controller, x.sort });

                dataWork.WhereFields(x => x.sort, md.sort.Substring(0, 3), WhereCompareType.LikeRight);
                dataWork.WhereFields(x => x.isfolder, false);
                dataWork.OrderByFields(x => x.sort);

                r.SearchData = dataWork.DataByAdapter<m_ProgData>();
                r.Result = true;

                dataWork.Dispose();

                return r;
                #endregion
                #endregion
            }
            catch (LogicRoll ex)
            {
                r.Result = false;
                r.ErrType = BusinessErrType.Logic;
                r.Message = ex.Message;
                return r;
            }
            catch (Exception ex)
            {
                r.Result = false;
                r.ErrType = BusinessErrType.System;
                r.Message = PackErrMessage(ex);
                return r;
            }

        }

        public override RunOneDataEnd<m_ProgData> GetDataMaster(int id, int AccountID)
        {
            RunOneDataEnd<m_ProgData> r = new RunOneDataEnd<m_ProgData>();
            //md = new m_ProgData();
            try
            {
                TablePack<ProgData> dataWork = new TablePack<ProgData>(Connection) { };
                dataWork.TableModule.KeyFieldModules[dataWork.TableModule.id.N].V = id; //設定KeyValue
                m_ProgData md = dataWork.GetDataByKey<m_ProgData>(); //取得Key該筆資料

                if (md == null)
                    throw new LogicRoll("Log_Err_MustHaveData"); //此區一定有資料傳出，如無資料因檢查前端id值是否有誤

                md.PowerItem = new PowerManagement(md.power_serial);

                r.SearchData = md;
                r.Result = true;
                return r;
            }
            catch (LogicRoll ex)
            {
                r.Result = false;
                r.ErrType = BusinessErrType.Logic;
                r.Message = ex.Message;
                return r;
            }
            catch (Exception ex)
            {
                r.Result = false;
                r.ErrType = BusinessErrType.System;
                r.Message = PackErrMessage(ex);
                return r;
            }
        }
        public override RunOneDataEnd<m_ProgData> GetDataMaster(int[] id, int accountId)
        {
            throw new NotImplementedException();
        }
    }
    #endregion
    #region User

    public class Password : ModuleBase
    {
        public int id { get; set; }
        public string password_o { get; set; }
        public string password_n { get; set; }
        public string password_s { get; set; }
    }
    public class q_Users : QueryBase
    {
        public string s_name { set; get; }
        public string s_account { set; get; }
    }
    public class m_Users : ModuleBase
    {
        public int id { get; set; }
        public String account { get; set; }
        public String password { get; set; }
        public String name { get; set; }
        public int unit { get; set; }
        public String state { get; set; }
        public Boolean isadmin { get; set; }
        public String type { get; set; }
        public String email { get; set; }
    }
    public class a_Users : LogicBase<m_Users, q_Users, UsersData>
    {
        public RunUpdateEnd UpdateMasterPassword(Password pwd, string AccountID)
        {
            RunUpdateEnd r = new RunUpdateEnd();
            UsersData TObj = new UsersData();
            Connection.BeginTransaction();

            try
            {
                TablePack<UsersData> dataWork = new TablePack<UsersData>(Connection) { TableModule = TObj };

                dataWork.SelectFields(x => new { x.id, x.password });
                dataWork.WhereFields(x => x.id, pwd.id);

                m_Users md = dataWork.DataByAdapter<m_Users>().FirstOrDefault();

                String GetNowPassword = md.password;

                if (GetNowPassword != pwd.password_o)
                    throw new LogicRoll("Log_Err_Password");

                if (GetNowPassword == pwd.password_n)
                    throw new LogicRoll("Log_Err_NewPasswordSame");

                if (pwd.password_s != pwd.password_n)
                    throw new LogicRoll("Log_Err_NewPasswordNotSure");

                dataWork.EditFirstRow();
                dataWork.SetDataRowValue(x => x.password, pwd.password_n);
                //dataWork.UpdateFieldsInfo(UpdateFieldsInfoType.Update);
                dataWork.UpdateDataAdapter();

                Connection.EndCommit();
                r.Result = true;
                return r;
            }
            catch (LogicRoll ex)
            {
                Connection.Rollback();

                r.Result = false;
                r.ErrType = BusinessErrType.Logic;
                r.Message = ex.Message;
                return r;
            }
            catch (Exception ex)
            {
                Connection.Rollback();
                r.Result = false;
                r.ErrType = BusinessErrType.System;
                r.Message = PackErrMessage(ex);
                return r;
            }
            finally
            {
            }

        }
        public LoginSate SystemLogin(String account, String password)
        {
            UsersData TObj = new UsersData();
            TablePack<UsersData> dataWork = new TablePack<UsersData>(Connection) { TableModule = TObj };
            LoginSate loginState = new LoginSate();
            try
            {
                dataWork.SelectFields(x => x.id);
                dataWork.SelectFields(x => x.account);
                dataWork.SelectFields(x => x.password);
                dataWork.SelectFields(x => x.name);
                dataWork.SelectFields(x => x.unit);
                dataWork.SelectFields(x => x.state);
                dataWork.SelectFields(x => x.isadmin);

                dataWork.WhereFields(x => x.account, account);
                dataWork.WhereFields(x => x.password, password);

                DataTable dt = dataWork.DataByAdapter();

                if (dt.Rows.Count == 0)
                    throw new LogicRoll("Login_Err_Password");

                if (dt.Rows[0][TObj.state.N].ToString() != "Normal")
                    throw new LogicRoll("Login_Err_Normal");

                else
                {
                    loginState.Id = (int)dt.Rows[0][TObj.id.N];
                    loginState.IsAdmin = (Boolean)dt.Rows[0][TObj.isadmin.N];
                    loginState.Unit = (int)dt.Rows[0][TObj.unit.N];
                    loginState.Result = true;
                    loginState.Acccount = (String)dt.Rows[0][TObj.account.N];

                    Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, "登入檢查完成。");

                    return loginState;
                }
            }
            catch (LogicRoll ex)
            {
                loginState.Result = false;
                loginState.ErrType = BusinessErrType.Logic;
                loginState.Message = ex.Message;
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return loginState;
            }
            catch (Exception ex)
            {
                loginState.Result = false;
                loginState.ErrType = BusinessErrType.System;
                loginState.Message = PackErrMessage(ex);
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return loginState;
            }
        }

        public override RunInsertEnd InsertMaster(m_Users md, int AccountID)
        {
            RunInsertEnd r = new RunInsertEnd();

            UsersData TObj = new UsersData();

            try
            {
                Connection.BeginTransaction();
                TablePack<UsersData> dataWork = new TablePack<UsersData>(Connection) { TableModule = TObj };

                dataWork.NewRow();

                dataWork.SetDataRowValue(x => x.id, md.id);
                dataWork.SetDataRowValue(x => x.account, md.account);
                dataWork.SetDataRowValue(x => x.password, md.password);
                dataWork.SetDataRowValue(x => x.name, md.name);
                dataWork.SetDataRowValue(x => x.unit, md.unit);
                dataWork.SetDataRowValue(x => x.state, md.state);
                dataWork.SetDataRowValue(x => x.isadmin, md.isadmin);
                dataWork.SetDataRowValue(x => x.type, md.type);
                dataWork.SetDataRowValue(x => x.email, md.email);


                dataWork.UpdateFieldsInfo(UpdateFieldsInfoType.Insert);

                dataWork.AddRow();

                dataWork.UpdateDataAdapter();
                Connection.EndCommit();

                r.Rows = dataWork.AffetCount; //取得影響筆數
                r.InsertId = dataWork.InsertAutoFieldsID; //取得新增後自動新增欄位的值

                r.Result = true;

                return r;
            }
            catch (LogicRoll ex)
            {
                Connection.Rollback();
                r.Result = false;
                r.ErrType = BusinessErrType.Logic;
                r.Message = ex.Message;
                return r;
            }
            catch (Exception ex)
            {
                Connection.Rollback();
                r.Result = false;
                r.ErrType = BusinessErrType.System;
                r.Message = PackErrMessage(ex);
                return r;
            }

        }
        public override RunUpdateEnd UpdateMaster(m_Users md, int AccountID)
        {
            RunUpdateEnd r = new RunUpdateEnd();
            Connection.BeginTransaction();
            UsersData TObj = new UsersData();
            try
            {
                TablePack<UsersData> dataWork = new TablePack<UsersData>(Connection) { TableModule = TObj };
                dataWork.TableModule.KeyFieldModules[TObj.id.N].V = md.id; //取得ID欄位的值
                DataTable dt_Origin = dataWork.GetDataByKey(); //取得Key值的Data

                dataWork.EditFirstRow();

                dataWork.SetDataRowValue(x => x.name, md.name);
                dataWork.SetDataRowValue(x => x.unit, md.unit);
                dataWork.SetDataRowValue(x => x.state, md.state);
                dataWork.SetDataRowValue(x => x.isadmin, md.isadmin);
                dataWork.SetDataRowValue(x => x.type, md.type);
                dataWork.SetDataRowValue(x => x.email, md.email);

                dataWork.UpdateDataAdapter();
                Connection.EndCommit();

                r.Rows = dataWork.AffetCount;
                r.Result = true;

                return r;
            }
            catch (LogicRoll ex)
            {
                Connection.Rollback();

                r.Result = false;
                r.ErrType = BusinessErrType.Logic;
                r.Message = ex.Message;
                return r;
            }
            catch (Exception ex)
            {
                Connection.Rollback();
                r.Result = false;
                r.ErrType = BusinessErrType.System;
                r.Message = PackErrMessage(ex);
                return r;
            }
            finally
            {
            }
        }
        public RunDeleteEnd DeleteMaster(String[] DeleteID, int AccountID)
        {
            RunDeleteEnd r = new RunDeleteEnd();
            UsersData TObj = new UsersData();
            Connection.BeginTransaction();

            try
            {
                //1、要刪除的資料先選出來
                TablePack<UsersData> dataWork = new TablePack<UsersData>(Connection) { TableModule = TObj };
                dataWork.SelectFields(x => x.id);
                dataWork.WhereFields(x => x.id, DeleteID);

                DataTable dt_Origin = dataWork.DataByAdapter();

                //2、進行全部刪除
                dataWork.DeleteAll(); //先刪除DataTable
                dataWork.UpdateDataAdapter(); //在更新至DataBase Server
                Connection.EndCommit();
                r.Result = true;
                return r;
            }
            catch (LogicRoll ex)
            {
                Connection.Rollback();
                r.Result = false;
                r.ErrType = BusinessErrType.Logic;
                r.Message = ex.Message;
                return r;
            }
            catch (Exception ex)
            {
                Connection.Rollback();
                r.Result = false;
                r.ErrType = BusinessErrType.System;
                r.Message = PackErrMessage(ex);
                return r;
            }
        }
        public override RunQueryPackage<m_Users> SearchMaster(q_Users qr, Int32 AccountID)
        {
            #region 全域變數宣告
            RunQueryPackage<m_Users> r = new RunQueryPackage<m_Users>();
            UsersData TObj = new UsersData();
            #endregion

            try
            {
                #region Select Data 區段 By 條件

                #region 設定輸出至Grid欄位 以下方式請注音 1、只適合單一Table 2、主要用於Grid顯示，如此方式不適合，可自行組SQL字串再夜由至ExecuteData達行
                TablePack<UsersData> dataWork = new TablePack<UsersData>(Connection) { TableModule = TObj };
                dataWork.SelectFields(x => new { x.id, x.account, x.unit, x.state, x.isadmin });
                #endregion

                #region 設定Where條件
                //系統帳號不列出
                dataWork.WhereFields(x => x.id, 1, WhereCompareType.UnEquel);
                if (qr.s_account != null)
                {
                    dataWork.WhereFields(x => x.account, qr.s_account);
                }

                if (qr.s_name != null)
                {
                    dataWork.WhereFields(x => x.name, qr.s_name, WhereCompareType.Equel);
                }
                #endregion

                #region 設定排序
                if (qr.sidx == null)
                {
                    //預設排序
                    dataWork.OrderByFields(x => x.id, OrderByType.DESC);
                }
                else
                {
                    dataWork.OrderByFields(x => x.id, OrderByType.ASC);
                }
                #endregion

                #region 輸出成DataTable
                r.SearchData = dataWork.DataByAdapter<m_Users>();
                r.Result = true;
                return r;
                #endregion

                #endregion
            }

            catch (LogicRoll ex)
            {
                #region 羅輯錯誤區區段
                r.Result = false;
                r.ErrType = BusinessErrType.Logic;
                r.Message = ex.Message;
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region 系統錯誤區
                r.Result = false;
                r.ErrType = BusinessErrType.System;
                r.Message = PackErrMessage(ex);
                return r;
                #endregion
            }
        }
        public override RunOneDataEnd<m_Users> GetDataMaster(int id, int AccountID)
        {
            RunOneDataEnd<m_Users> r = new RunOneDataEnd<m_Users>();

            try
            {
                // 取得Table物件 簡化長度
                UsersData TObj = new UsersData();

                TablePack<UsersData> dataWork = new TablePack<UsersData>(Connection) { TableModule = TObj };
                TObj.KeyFieldModules[TObj.id.N].V = id; //設定KeyValue
                r.SearchData = dataWork.GetDataByKey<m_Users>(); //取得Key該筆資料

                if (r.SearchData == null) //如有資料
                    throw new LogicRoll("Log_Err_MustHaveData"); //此區一定有資料傳出，如無資料因檢查前端id值是否有誤

                r.Result = true;
                return r;
            }
            catch (LogicRoll ex)
            {
                r.Result = false;
                r.ErrType = BusinessErrType.Logic;
                r.Message = ex.Message;
                return r;
            }
            catch (Exception ex)
            {
                r.Result = false;
                r.ErrType = BusinessErrType.System;
                r.Message = PackErrMessage(ex);
                return r;
            }
        }
        public override RunDeleteEnd DeleteMaster(int[] ids, int accountId)
        {
            throw new NotImplementedException();
        }
        public override RunOneDataEnd<m_Users> GetDataMaster(int[] ids, int accountId)
        {
            throw new NotImplementedException();
        }

        public Boolean Exists_Account(String account, int accountId)
        {
            RunQueryPackage<m_Users> r = new RunQueryPackage<m_Users>();
            TablePack<UsersData> dataWork = new TablePack<UsersData>(Connection) { LoginUserID = accountId };
            dataWork.SelectFields(x => new { x.id });
            dataWork.WhereFields(x => x.account, account);

            r.SearchData = dataWork.DataByAdapter<m_Users>();
            r.Result = true;
            return r.SearchData.Count() > 0;
        }
        public Dictionary<String, String> MakeOption_Unit()
        {
            UnitData TObj = new UnitData();

            TablePack<UnitData> dataWork = new TablePack<UnitData>(Connection) { TableModule = TObj };

            dataWork.SelectFields(x => x.id);
            dataWork.SelectFields(x => x.name);
            dataWork.OrderByFields(x => x.sort, OrderByType.ASC);

            return dataWork.DataByAdapter().dicMakeKeyValue(0, 1);
        }
        public Dictionary<String, String> MakeOption_UsersState()
        {
            return CodeSheet.使用者狀態.ToDictionary();
        }
    }
    #endregion
    #region Unit
    public class q_Unit : QueryBase
    {
        public string s_name { set; get; }
    }

    public class m_Unit : ModuleBase
    {
        public int id { get; set; }
        public String name { get; set; }
        public int sort { get; set; }
    }

    public class a_Unit : LogicBase<m_Unit, q_Unit, UnitData>
    {
        public override RunInsertEnd InsertMaster(m_Unit md, int AccountID)
        {
            RunInsertEnd r = new RunInsertEnd();
            Connection.BeginTransaction();
            UnitData TObj = new UnitData();
            try
            {
                TablePack<UnitData> dataWork = new TablePack<UnitData>(Connection) { TableModule = TObj };

                dataWork.NewRow(); //開始新橧作業 產生新的一行
                #region 指派值
                dataWork.SetDataRowValue(x => x.id, md.id);
                dataWork.SetDataRowValue(x => x.name, md.name);
                dataWork.SetDataRowValue(x => x.sort, md.sort);
                #endregion
                dataWork.AddRow(); //加載至DataTable
                dataWork.UpdateDataAdapter(); //更新 DataBase Server

                Connection.EndCommit();

                r.Rows = dataWork.AffetCount; //取得影響筆數
                r.InsertId = dataWork.InsertAutoFieldsID; //取得新增後自動新增欄位的值
                r.Result = true; //回傳本次執行結果為成功

                return r;
            }
            catch (LogicRoll ex)
            {
                Connection.Rollback();
                r.Result = false;
                r.ErrType = BusinessErrType.Logic;
                r.Message = ex.Message;
                return r;
            }
            catch (Exception ex)
            {
                Connection.Rollback();
                r.Result = false;
                r.ErrType = BusinessErrType.System;
                r.Message = PackErrMessage(ex);
                return r;
            }
        }
        public override RunUpdateEnd UpdateMaster(m_Unit md, int AccountID)
        {
            RunUpdateEnd r = new RunUpdateEnd();
            Connection.BeginTransaction();
            UnitData TObj = new UnitData();
            try
            {
                TablePack<UnitData> dataWork = new TablePack<UnitData>(Connection) { TableModule = TObj };

                dataWork.TableModule.KeyFieldModules[TObj.id.N].V = md.id; //取得ID欄位的值
                DataTable dt_Origin = dataWork.GetDataByKey(); //取得Key值的Data

                dataWork.EditFirstRow();
                #region 指派值
                dataWork.SetDataRowValue(x => x.name, md.name);
                dataWork.SetDataRowValue(x => x.sort, md.sort);

                dataWork.UpdateFieldsInfo(UpdateFieldsInfoType.Update);
                #endregion
                dt_Origin.Dispose();
                dataWork.UpdateDataAdapter();

                Connection.EndCommit();

                r.Rows = dataWork.AffetCount;
                r.Result = true;

                return r;
            }
            catch (LogicRoll ex)
            {
                Connection.Rollback();

                r.Result = false;
                r.ErrType = BusinessErrType.Logic;
                r.Message = ex.Message;
                return r;
            }
            catch (Exception ex)
            {
                Connection.Rollback();
                r.Result = false;
                r.ErrType = BusinessErrType.System;
                r.Message = PackErrMessage(ex);
                return r;
            }
            finally
            {
            }

        }
        public RunDeleteEnd DeleteMaster(String[] DeleteID, int AccountID)
        {
            RunDeleteEnd r = new RunDeleteEnd();
            Connection.BeginTransaction();
            UnitData TObj = new UnitData();
            try
            {
                //1、要刪除的資料先選出來
                TablePack<UnitData> dataWork = new TablePack<UnitData>(Connection) { TableModule = TObj };
                dataWork.SelectFields(x => x.id);
                dataWork.WhereFields(x => x.id, DeleteID);

                DataTable dt_Origin = dataWork.DataByAdapter();

                //2、進行全部刪除
                dataWork.DeleteAll(); //先刪除DataTable
                dataWork.UpdateDataAdapter(); //在更新至DataBase Server
                Connection.EndCommit();
                r.Result = true;
                return r;
            }
            catch (LogicRoll ex)
            {
                Connection.Rollback();
                r.Result = false;
                r.ErrType = BusinessErrType.Logic;
                r.Message = ex.Message;
                return r;
            }
            catch (Exception ex)
            {
                Connection.Rollback();
                r.Result = false;
                r.ErrType = BusinessErrType.System;
                r.Message = PackErrMessage(ex);
                return r;
            }
        }
        public override RunQueryPackage<m_Unit> SearchMaster(q_Unit qr, int AccountID)
        {
            #region 全域變數宣告
            RunQueryPackage<m_Unit> r = new RunQueryPackage<m_Unit>();
            UnitData TObj = new UnitData();
            #endregion

            try
            {
                #region Select Data 區段 By 條件

                #region 設定輸出至Grid欄位 以下方式請注音 1、只適合單一Table 2、主要用於Grid顯示，如此方式不適合，可自行組SQL字串再夜由至ExecuteData達行
                TablePack<UnitData> dataWork = new TablePack<UnitData>(Connection) { TableModule = TObj };
                dataWork.SelectFields(x => new { x.id, x.name, x.sort });
                #endregion

                #region 設定Where條件
                if (qr.s_name != null)
                {
                    dataWork.WhereFields(x => x.name, qr.s_name, WhereCompareType.Like);
                }
                #endregion

                #region 設定排序
                if (qr.sidx == null)
                {
                    //預設排序
                    dataWork.OrderByFields(x => x.sort, OrderByType.DESC);
                }
                else
                {
                    dataWork.OrderByFields(x => x.sort, OrderByType.ASC);
                }
                #endregion

                #region 輸出成DataTable
                r.SearchData = dataWork.DataByAdapter<m_Unit>();
                r.Result = true;
                return r;
                #endregion

                #endregion
            }

            catch (LogicRoll ex)
            {
                #region 羅輯錯誤區區段
                r.Result = false;
                r.ErrType = BusinessErrType.Logic;
                r.Message = ex.Message;
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region 系統錯誤區
                r.Result = false;
                r.ErrType = BusinessErrType.System;
                r.Message = PackErrMessage(ex);
                return r;
                #endregion
            }
        }
        public override RunOneDataEnd<m_Unit> GetDataMaster(int[] ids, int accountId)
        {
            throw new NotImplementedException();
        }
        public override RunOneDataEnd<m_Unit> GetDataMaster(int id, int AccountID)
        {
            RunOneDataEnd<m_Unit> r = new RunOneDataEnd<m_Unit>();
            string sql = string.Empty;
            m_Unit md = new m_Unit();
            try
            {
                // 取得Table物件 簡化長度
                UnitData TObj = new UnitData();

                TablePack<UnitData> dataWork = new TablePack<UnitData>(Connection) { TableModule = TObj };
                TObj.KeyFieldModules[TObj.id.N].V = id; //設定KeyValue
                r.SearchData = dataWork.GetDataByKey<m_Unit>(); //取得Key該筆資料

                if (r.SearchData == null) //如有資料
                    throw new LogicRoll("Log_Err_MustHaveData"); //此區一定有資料傳出，如無資料因檢查前端id值是否有誤


                r.Result = true;
                return r;
            }
            catch (LogicRoll ex)
            {
                r.Result = false;
                r.ErrType = BusinessErrType.Logic;
                r.Message = ex.Message;
                return r;
            }
            catch (Exception ex)
            {
                r.Result = false;
                r.ErrType = BusinessErrType.System;
                r.Message = PackErrMessage(ex);
                return r;
            }
        }
        public override RunDeleteEnd DeleteMaster(int[] ids, int accountId)
        {
            throw new NotImplementedException();
        }
    }
    #endregion
    #region NewsData
    /// <summary>
    /// 查詢表單模組
    /// </summary>
    public class q_NewsData : QueryBase
    {
        public String s_title { set; get; }
    }
    /// <summary>
    /// 系統資料結構描述模組    
    /// </summary>
    public class m_NewsData : ModuleBase
    {
        public Int32 id { set; get; }
        public String Title { set; get; }
        public DateTime SetDate { set; get; }
        public Boolean IsOpen { set; get; }
        public String NewsKind { set; get; }
        public String Context { set; get; }
    }
    /// <summary>
    /// The system database communicate module.   
    /// </summary>
    public class a_NewsData : LogicBase<m_NewsData, q_NewsData, NewsData>
    {
        /// <summary>
        /// NewsData Table進行新增資料動作。
        /// </summary>
        /// <param name="md">傳入m_NewsData class，class需先行建立(new)，其class各項屬性值需指定完成再行傳入</param>
        /// <param name="accountId">傳入Login使用者Id值</param>
        /// <returns>回傳RunInsertEnd class，請參閱RunInsertEnd說明。</returns>
        public override RunInsertEnd InsertMaster(m_NewsData md, int accountId)
        {
            #region Variable declare area
            RunInsertEnd r = new RunInsertEnd(); //宣告回傳物件
            #endregion
            try
            {
                #region Main working
                Connection.BeginTransaction(); //開始交易鎖定

                TablePack<NewsData> dataWork = new TablePack<NewsData>(Connection) { LoginUserID = accountId };

                dataWork.NewRow(); //開始新橧作業 產生新的一行
                #region 指派值
                dataWork.SetDataRowValue(x => x.id, md.id);
                dataWork.SetDataRowValue(x => x.Title, md.Title);
                dataWork.SetDataRowValue(x => x.NewsKind, md.NewsKind);
                dataWork.SetDataRowValue(x => x.SetDate, md.SetDate);
                dataWork.SetDataRowValue(x => x.IsOpen, md.IsOpen);
                dataWork.SetDataRowValue(x => x.Context, md.Context);
                dataWork.UpdateFieldsInfo(UpdateFieldsInfoType.Insert); //進行更新時 需同時更新系統欄位 _InsertUserID，_InsertDateTime
                dataWork.UpdateFieldsInfo(UpdateFieldsInfoType.Lang); //語系欄位設定
                #endregion
                dataWork.AddRow(); //加載至DataTable
                dataWork.UpdateDataAdapter(); //更新 DataBase Server
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, "Insert DataTable");

                Connection.EndCommit(); //交易確認

                r.Rows = dataWork.AffetCount; //取得影響筆數
                r.InsertId = dataWork.InsertAutoFieldsID; //取得新增後欄位的值
                r.Result = true; //回傳本次執行結果為成功
                dataWork.Dispose(); //釋放資料

                return r;
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System;//系統執行失敗
                r.Message = PackErrMessage(ex); //回傳失敗訊息
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
        }
        /// <summary>
        /// NewsData Table進行更新資料動作。
        /// </summary>
        /// <param name="md">傳入m_NewsData class，class需先行建立(new)，其class各項屬性值需指定完成再行傳入</param>
        /// <param name="accountId">傳入Login使用者Id值</param>
        /// <returns>回傳RunUpdateEnd class，請參閱RunUpdateEnd說明。</returns>
        public override RunUpdateEnd UpdateMaster(m_NewsData md, int accountId)
        {
            #region Variable declare area
            RunUpdateEnd r = new RunUpdateEnd();
            #endregion
            try
            {
                #region Main Working
                Connection.BeginTransaction();
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, "Start BeginTransaction");

                TablePack<NewsData> dataWork = new TablePack<NewsData>(Connection) { LoginUserID = accountId };

                dataWork.TableModule.KeyFieldModules[dataWork.TableModule.id.N].V = md.id; //取得ID欄位的值
                m_NewsData md_Origin = dataWork.GetDataByKey<m_NewsData>(); //取得Key值的Data
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, "Select Modily One Data");

                if (md_Origin == null) //如無資料
                    throw new LogicRoll("Log_Err_MustHaveData"); //此區一定有資料傳出，如無資料應檢查前端id值是否有誤

                dataWork.EditFirstRow(); //編輯第一筆資料，正常只會有一筆資料。
                #region 指派值
                dataWork.SetDataRowValue(x => x.Title, md.Title);
                dataWork.SetDataRowValue(x => x.NewsKind, md.NewsKind);
                dataWork.SetDataRowValue(x => x.SetDate, md.SetDate);
                dataWork.SetDataRowValue(x => x.IsOpen, md.IsOpen);
                dataWork.SetDataRowValue(x => x.Context, md.Context);
                dataWork.UpdateFieldsInfo(UpdateFieldsInfoType.Update); //進行更新時 需同時更新系統欄位 _UpdateUserID，_UpdateDateTime
                #endregion
                md = null;
                dataWork.UpdateDataAdapter(); //進行變更至Database Server
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, "Update DataTable");

                Connection.EndCommit();
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, "End BeginTransaction");

                r.Rows = dataWork.AffetCount; //取得影響筆數
                r.Result = true;
                dataWork.Dispose(); //釋放資料
                return r;
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System;//系統執行失敗
                r.Message = PackErrMessage(ex); //回傳失敗訊息
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
        }
        /// <summary>
        /// NewsData Table進行刪除資料動作。
        /// </summary>
        /// <param name="deleteIds">傳入要刪除資料的主鍵值，此適用該Table只有單一主鍵欄位。</param>
        /// <param name="accountId">傳入Login使用者Id值</param>
        /// <returns>回傳RunDeleteEnd class，請參閱RunDeleteEnd說明。</returns>
        public override RunDeleteEnd DeleteMaster(Int32[] ids, int accountId)
        {
            //此功能主要搭配Grid介面刪除功能製作
            #region Variable declare area
            RunDeleteEnd r = new RunDeleteEnd(); //宣告刪除Result回物件
            #endregion
            try
            {
                #region Main working
                Connection.BeginTransaction();
                //1、要刪除的資料先選出來
                TablePack<NewsData> dataWork = new TablePack<NewsData>(Connection) { LoginUserID = accountId }; //宣告泛型物件並連接Connection
                dataWork.SelectFields(x => x.id); //只Select 主Key欄位
                dataWork.WhereFields(x => x.id, ids); //代入陣列Id值
                m_NewsData[] md_Origin = dataWork.DataByAdapter<m_NewsData>(); //取得Key值的Data

                //2、進行全部刪除
                dataWork.DeleteAll(); //先刪除DataTable
                dataWork.UpdateDataAdapter(); //在更新至DataBase Server
                Connection.EndCommit();
                r.Result = true;
                dataWork.Dispose(); //釋放資料
                md_Origin = null;
                return r;
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System;//系統執行失敗
                r.Message = PackErrMessage(ex); //回傳失敗訊息
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
        }
        /// <summary>
        /// NewsData Table進行動態多條件查詢資料動作。
        /// </summary>
        /// <param name="qr">>傳入q_NewsData class，class需先行建立(new)，其class各項屬性值需指定完成再行傳入</param>
        /// <param name="accountId">傳入Login使用者Id值</param>
        /// <returns>回傳RunQueryPackage<m_NewsData>m_NewsData</m_NewsData> class，請參閱RunQueryPackage說明。</returns>
        public override RunQueryPackage<m_NewsData> SearchMaster(q_NewsData qr, int accountId)
        {
            #region 變數宣告
            RunQueryPackage<m_NewsData> r = new RunQueryPackage<m_NewsData>();
            #endregion
            try
            {
                #region Select Data 區段 By 條件
                #region 設定輸出至Grid欄位 以下方式請注音 1、只適合單一Table 2、主要用於Grid顯示，如此方式不適合，可自行組SQL字串再夜由至ExecuteData達行
                TablePack<NewsData> dataWork = new TablePack<NewsData>(Connection) { LoginUserID = accountId, TableAlias = "A" };
                dataWork.SelectFields(x => new { x.id, x.Title, x.NewsKind, x.SetDate, x.IsOpen });
                #endregion

                #region 設定Where條件
                if (qr.s_title != null)
                    dataWork.WhereFields(x => x.Title, qr.s_title, WhereCompareType.Like);

                dataWork.WhereLang(); //使用語系條件
                #endregion

                #region 設定排序
                if (qr.sidx == null)
                {
                    dataWork.OrderByFields(x => x.SetDate, OrderByType.DESC);
                    dataWork.OrderByFields(x => x.id, OrderByType.DESC);
                }
                else
                    dataWork.OrderByFields(x => x.SetDate, OrderByType.ASC);

                #endregion

                #region 輸出物件陣列
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, "Start");
                r.SearchData = dataWork.DataByAdapter<m_NewsData>();
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, "End");
                r.Result = true;
                return r;
                #endregion
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System;//系統執行失敗
                r.Message = PackErrMessage(ex); //回傳失敗訊息
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
        }
        /// <summary>
        /// NewsData Table進行主鍵值查詢資料動作。
        /// </summary>
        /// <param name="id">傳入主鍵Value</param>
        /// <param name="accountId">傳入Login使用者Id值</param>
        /// <returns>回傳RunOneDataEnd<m_NewsData>m_NewsData</m_NewsData> class，請參閱RunOneDataEnd說明。</returns>
        public override RunOneDataEnd<m_NewsData> GetDataMaster(int id, int accountId)
        {
            #region 變數宣告
            RunOneDataEnd<m_NewsData> r = new RunOneDataEnd<m_NewsData>();

            #endregion
            try
            {
                #region Main working
                TablePack<NewsData> dataWork = new TablePack<NewsData>(Connection) { LoginUserID = accountId };
                dataWork.TableModule.KeyFieldModules[dataWork.TableModule.id.N].V = id; //設定KeyValue
                m_NewsData md = dataWork.GetDataByKey<m_NewsData>(); //取得Key該筆資料

                if (md == null) //如無資料
                    throw new LogicRoll("Log_Err_MustHaveData"); //此區一定有資料傳出，如無資料因檢查前端id值是否有誤

                r.SearchData = md;
                r.Result = true;
                dataWork.Dispose(); //釋放資料
                return r;
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System;//系統執行失敗
                r.Message = PackErrMessage(ex); //回傳失敗訊息
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
        }
        public override RunOneDataEnd<m_NewsData> GetDataMaster(int[] id, int accountId)
        {
            throw new NotImplementedException();
        }
    }
    #endregion
    #region _Lang
    /// <summary>
    /// 查詢表單模組
    /// </summary>
    public class q__Lang : QueryBase
    {
        public String s_lang { get; set; }
        public Boolean? s_isuse { get; set; }
    }
    public class m__Lang : ModuleBase
    {
        public String lang { get; set; }
        public String area { get; set; }
        public String memo { get; set; }
        public Boolean isuse { get; set; }
        public Byte sort { get; set; }
    }
    /// <summary>
    /// The _Lang system database communicate module.   
    /// </summary>
    public class a__Lang : LogicBase<m__Lang, q__Lang, _Lang>
    {
        public override RunInsertEnd InsertMaster(m__Lang md, int accountId)
        {
            throw new NotImplementedException();
        }
        public override RunUpdateEnd UpdateMaster(m__Lang md, int accountId)
        {
            throw new NotImplementedException();
        }
        public override RunDeleteEnd DeleteMaster(int[] ids, int accountId)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// _Lang Table進行動態多條件查詢資料動作。
        /// </summary>
        /// <param name="qr">>傳入q__Lang class，class需先行建立(new)，其class各項屬性值需指定完成再行傳入</param>
        /// <param name="accountId">傳入Login使用者Id值</param>
        /// <returns>回傳RunQueryPackage<m__Lang>m__Lang</m__Lang> class，請參閱RunQueryPackage說明。</returns>
        public override RunQueryPackage<m__Lang> SearchMaster(q__Lang qr, int accountId)
        {
            #region 變數宣告
            RunQueryPackage<m__Lang> r = new RunQueryPackage<m__Lang>();
            #endregion
            try
            {
                #region Select Data 區段 By 條件
                #region 設定輸出至Grid欄位 以下方式請注意 1、只適合單一Table 2、主要用於Grid顯示，如此方式不適合，可自行組SQL字串再夜由至ExecuteData執行
                TablePack<_Lang> dataWork = new TablePack<_Lang>(Connection) { LoginUserID = accountId };
                dataWork.SelectFields(x => new { x.lang, x.area });
                #endregion

                #region 設定Where條件

                if(qr.s_isuse!=null)
                    dataWork.WhereFields(x => x.isuse, qr.s_isuse);

                if(qr.s_lang!=null)
                    dataWork.WhereFields(x => x.lang, qr.s_lang);

                #endregion

                #region 設定排序
                dataWork.OrderByFields(x => x.sort); //預設排序

                #endregion

                #region 輸出物件陣列
                r.SearchData = dataWork.DataByAdapter<m__Lang>();
                r.Result = true;
                return r;
                #endregion
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
        }
        /// <summary>
        /// _Lang Table進行主鍵值查詢資料動作。
        /// </summary>
        /// <param name="id">傳入主鍵Value</param>
        /// <param name="accountId">傳入Login使用者Id值</param>
        /// <returns>回傳RunOneDataEnd<m__Lang>m__Lang</m__Lang> class，請參閱RunOneDataEnd說明。</returns>
        public override RunOneDataEnd<m__Lang> GetDataMaster(int id, int accountId)
        {
            #region 變數宣告
            RunOneDataEnd<m__Lang> r = new RunOneDataEnd<m__Lang>();

            #endregion
            try
            {
                #region Main working
                TablePack<_Lang> dataWork = new TablePack<_Lang>(Connection) { LoginUserID = accountId };
                //dataWork.TableModule.KeyFieldModules[dataWork.TableModule.id.N].V = id; //設定KeyValue
                m__Lang md = dataWork.GetDataByKey<m__Lang>(); //取得Key該筆資料

                if (md == null) //如無資料
                    throw new LogicRoll("Log_Err_MustHaveData"); //此區一定有資料傳出，如無資料應檢查前端id值是否有誤

                r.SearchData = md;
                r.Result = true;
                dataWork.Dispose(); //釋放資料
                return r;
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
        }
        public override RunOneDataEnd<m__Lang> GetDataMaster(int[] id, int accountId)
        {
            throw new NotImplementedException();
        }
    }
    #endregion
    #region _WebVisitData
    /// <summary>
    /// 查詢表單模組
    /// </summary>
    public class q__WebVisitData : QueryBase
    {
        public String s_title { set; get; }
    }
    /// <summary>
    /// 系統資料結構描述模組    /// </summary>

    public class m__WebVisitData : ModuleBase
    {
        public Int32 id { get; set; }
        public String ip { get; set; }
        public DateTime setdate { get; set; }
        public String browser { get; set; }
        public String source { get; set; }
        public String page { get; set; }
    }

    /// <summary>
    /// The _WebVisitData system database communicate module.   /// </summary>
    public class a__WebVisitData : LogicBase<m__WebVisitData, q__WebVisitData, _WebVisitData>
    {
        /// <summary>
        /// _WebVisitData Table進行新增資料動作。
        /// </summary>
        /// <param name="md">傳入m__WebVisitData class，class需先行建立(new)，其class各項屬性值需指定完成再行傳入</param>
        /// <param name="accountId">傳入Login使用者Id值</param>
        /// <returns>回傳RunInsertEnd class，請參閱RunInsertEnd說明。</returns>
        public override RunInsertEnd InsertMaster(m__WebVisitData md, int accountId)
        {
            #region Variable declare area
            RunInsertEnd r = new RunInsertEnd(); //宣告回傳物件
            #endregion
            try
            {
                #region Main working
                Connection.BeginTransaction(); //開始交易鎖定
                TablePack<_WebVisitData> dataWork = new TablePack<_WebVisitData>(Connection) { LoginUserID = accountId };

                dataWork.NewRow(); //開始新橧作業 產生新的一行
                #region 指派值

                dataWork.SetDataRowValue(x => x.id, md.id);
                dataWork.SetDataRowValue(x => x.ip, md.ip);
                dataWork.SetDataRowValue(x => x.setdate, md.setdate);
                dataWork.SetDataRowValue(x => x.browser, md.browser);
                dataWork.SetDataRowValue(x => x.source, md.source);
                dataWork.SetDataRowValue(x => x.page, md.page);

                //dataWork.UpdateFieldsInfo(UpdateFieldsInfoType.Insert);
                //進行更新時 需同時更新系統欄位 _InsertUserID，_InsertDateTime
                #endregion
                dataWork.AddRow(); //加載至DataTable
                dataWork.UpdateDataAdapter(); //更新 DataBase Server

                Connection.EndCommit(); //交易確認

                r.Rows = dataWork.AffetCount; //取得影響筆數
                r.InsertId = dataWork.InsertAutoFieldsID; //取得新增後自動新增欄位的值
                r.Result = true; //回傳本次執行結果為成功
                dataWork.Dispose(); //釋放資料

                return r;
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼

                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);

                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System;//系統執行失敗
                r.Message = PackErrMessage(ex); //回傳失敗訊息
                return r;
                #endregion
            }
        }
        /// <summary>
        /// _WebVisitData Table進行更新資料動作。
        /// </summary>
        /// <param name="md">傳入m__WebVisitData class，class需先行建立(new)，其class各項屬性值需指定完成再行傳入</param>
        /// <param name="accountId">傳入Login使用者Id值</param>
        /// <returns>回傳RunUpdateEnd class，請參閱RunUpdateEnd說明。</returns>
        public override RunUpdateEnd UpdateMaster(m__WebVisitData md, int accountId)
        {
            #region Variable declare area
            RunUpdateEnd r = new RunUpdateEnd();
            #endregion
            try
            {
                #region Main Working
                Connection.BeginTransaction();
                TablePack<_WebVisitData> dataWork = new TablePack<_WebVisitData>(Connection) { LoginUserID = accountId };

                dataWork.TableModule.KeyFieldModules[dataWork.TableModule.id.N].V = md.id; //取得ID欄位的值
                m__WebVisitData md_Origin = dataWork.GetDataByKey<m__WebVisitData>(); //取得Key值的Data

                if (md_Origin == null) //如有資料
                    throw new LogicRoll("Log_Err_MustHaveData"); //此區一定有資料傳出，如無資料應檢查前端id值是否有誤

                dataWork.EditFirstRow(); //編輯第一筆資料，正常只會有一筆資料。
                #region 指派值

                dataWork.SetDataRowValue(x => x.id, md.id);
                dataWork.SetDataRowValue(x => x.ip, md.ip);
                dataWork.SetDataRowValue(x => x.setdate, md.setdate);
                dataWork.SetDataRowValue(x => x.browser, md.browser);
                dataWork.SetDataRowValue(x => x.source, md.source);
                dataWork.SetDataRowValue(x => x.page, md.page);


                dataWork.UpdateFieldsInfo(UpdateFieldsInfoType.Update); //指定進行更新時 需同時更新系統欄位 _UpdateUserID，_UpdateDateTime
                #endregion
                md_Origin = null;
                dataWork.UpdateDataAdapter(); //進行變更至Database Server

                Connection.EndCommit();

                r.Rows = dataWork.AffetCount; //取得影響筆數
                r.Result = true;
                dataWork.Dispose(); //釋放資料
                return r;
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System;//系統執行失敗
                r.Message = PackErrMessage(ex); //回傳失敗訊息
                return r;
                #endregion
            }
        }
        /// <summary>
        /// _WebVisitData Table進行刪除資料動作。
        /// </summary>
        /// <param name="deleteIds">傳入要刪除資料的主鍵值，此適用該Table只有單一主鍵欄位。</param>
        /// <param name="accountId">傳入Login使用者Id值</param>
        /// <returns>回傳RunDeleteEnd class，請參閱RunDeleteEnd說明。</returns>
        public override RunDeleteEnd DeleteMaster(Int32[] ids, int accountId)
        {
            //此功能主要搭配Grid介面刪除功能製作
            #region Variable declare area
            RunDeleteEnd r = new RunDeleteEnd(); //宣告刪除Result回物件
            #endregion
            try
            {
                #region Main working
                Connection.BeginTransaction();
                //1、要刪除的資料先選出來
                TablePack<_WebVisitData> dataWork = new TablePack<_WebVisitData>(Connection) { LoginUserID = accountId }; //宣告泛型物件並連接Connection
                dataWork.SelectFields(x => x.id); //只Select 主Key欄位
                dataWork.WhereFields(x => x.id, ids); //代入陣列Id值

                m__WebVisitData[] md_Origin = dataWork.DataByAdapter<m__WebVisitData>(); //取得Key值的Data

                //2、進行全部刪除
                dataWork.DeleteAll(); //先刪除DataTable
                dataWork.UpdateDataAdapter(); //在更新至DataBase Server
                Connection.EndCommit();
                r.Result = true;
                dataWork.Dispose(); //釋放資料
                md_Origin = null;
                return r;
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System;//系統執行失敗
                r.Message = PackErrMessage(ex); //回傳失敗訊息
                return r;
                #endregion
            }
        }
        /// <summary>
        /// _WebVisitData Table進行動態多條件查詢資料動作。
        /// </summary>
        /// <param name="qr">>傳入q__WebVisitData class，class需先行建立(new)，其class各項屬性值需指定完成再行傳入</param>
        /// <param name="accountId">傳入Login使用者Id值</param>
        /// <returns>回傳RunQueryPackage<m__WebVisitData>m__WebVisitData</m__WebVisitData> class，請參閱RunQueryPackage說明。</returns>
        public override RunQueryPackage<m__WebVisitData> SearchMaster(q__WebVisitData qr, int accountId)
        {
            #region 變數宣告
            RunQueryPackage<m__WebVisitData> r = new RunQueryPackage<m__WebVisitData>();
            #endregion
            try
            {
                #region Select Data 區段 By 條件
                #region 設定輸出至Grid欄位 以下方式請注意 1、只適合單一Table 2、主要用於Grid顯示，如此方式不適合，可自行組SQL字串再夜由至ExecuteData執行
                TablePack<_WebVisitData> dataWork = new TablePack<_WebVisitData>(Connection) { LoginUserID = accountId };
                //dataWork.SelectFields(x => new { x.id, x., x.NewsKind, x.SetDate, x.IsOpen });
                #endregion

                #region 設定Where條件
                if (qr.s_title != null)
                    dataWork.WhereFields(x => x.ip, qr.s_title);
                #endregion

                #region 設定排序
                if (qr.sidx == null)
                {
                    //預設排序
                    dataWork.OrderByFields(x => x.id, OrderByType.DESC);
                }
                else
                {
                    dataWork.OrderByFields(x => x.id, OrderByType.ASC);
                }
                #endregion

                #region 輸出物件陣列
                r.SearchData = dataWork.DataByAdapter<m__WebVisitData>();
                r.Result = true;
                return r;
                #endregion
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System;//系統執行失敗
                r.Message = PackErrMessage(ex); //回傳失敗訊息
                return r;
                #endregion
            }
        }
        /// <summary>
        /// _WebVisitData Table進行主鍵值查詢資料動作。
        /// </summary>
        /// <param name="id">傳入主鍵Value</param>
        /// <param name="accountId">傳入Login使用者Id值</param>
        /// <returns>回傳RunOneDataEnd<m__WebVisitData>m__WebVisitData</m__WebVisitData> class，請參閱RunOneDataEnd說明。</returns>
        public override RunOneDataEnd<m__WebVisitData> GetDataMaster(int id, int accountId)
        {
            #region 變數宣告
            RunOneDataEnd<m__WebVisitData> r = new RunOneDataEnd<m__WebVisitData>();

            #endregion
            try
            {
                #region Main working
                TablePack<_WebVisitData> dataWork = new TablePack<_WebVisitData>(Connection) { LoginUserID = accountId };
                dataWork.TableModule.KeyFieldModules[dataWork.TableModule.id.N].V = id; //設定KeyValue
                m__WebVisitData md = dataWork.GetDataByKey<m__WebVisitData>(); //取得Key該筆資料

                if (md == null) //如無資料
                    throw new LogicRoll("Log_Err_MustHaveData"); //此區一定有資料傳出，如無資料應檢查前端id值是否有誤

                r.SearchData = md;
                r.Result = true;
                dataWork.Dispose(); //釋放資料
                return r;
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System;//系統執行失敗
                r.Message = PackErrMessage(ex); //回傳失敗訊息
                return r;
                #endregion
            }
        }
        public override RunOneDataEnd<m__WebVisitData> GetDataMaster(int[] id, int accountId)
        {
            throw new NotImplementedException();
        }
    }
    #endregion
    #region _WebCount
    /// <summary>
    /// 查詢表單模組
    /// </summary>
    public class q__WebCount : QueryBase
    {
        public Int32 s_Cnt { set; get; }
    }
    /// <summary>
    /// 系統資料結構描述模組    /// </summary>

    public class m__WebCount : ModuleBase
    {
        public Int32 Cnt { get; set; }
    }

    /// <summary>
    /// The _WebVisitData system database communicate module.   /// </summary>
    public class a__WebCount : LogicBase
    {
        public RunUpdateEnd UpdateMaster(int accountId)
        {
            #region Variable declare area
            RunUpdateEnd r = new RunUpdateEnd();
            #endregion
            try
            {
                #region Main Working
                Connection.MustRunTrans = true;
                Connection.BeginTransaction();
                TablePack<_WebCount> dataWork = new TablePack<_WebCount>(Connection) { LoginUserID = accountId };
                m__WebCount md_Origin = dataWork.DataByAdapter<m__WebCount>().FirstOrDefault();
                if (md_Origin == null) //如有資料
                    throw new LogicRoll("Log_Err_MustHaveData"); //此區一定有資料傳出，如無資料應檢查前端id值是否有誤

                dataWork.EditFirstRow(); //編輯第一筆資料，正常只會有一筆資料。
                #region 指派值

                dataWork.SetDataRowValue(x => x.Cnt, md_Origin.Cnt + 1);
                //dataWork.UpdateFieldsInfo(UpdateFieldsInfoType.Update); //指定進行更新時 需同時更新系統欄位 _UpdateUserID，_UpdateDateTime
                #endregion
                md_Origin = null;
                dataWork.UpdateDataAdapter(); //進行變更至Database Server

                Connection.EndCommit();

                r.Rows = dataWork.AffetCount; //取得影響筆數
                r.Result = true;
                dataWork.Dispose(); //釋放資料
                return r;
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System;//系統執行失敗
                r.Message = PackErrMessage(ex); //回傳失敗訊息
                return r;
                #endregion
            }
        }
        public RunOneDataEnd<m__WebCount> SearchMaster(int accountId)
        {
            #region 變數宣告
            RunOneDataEnd<m__WebCount> r = new RunOneDataEnd<m__WebCount>();
            #endregion
            try
            {
                #region Select Data 區段 By 條件
                #region 設定輸出至Grid欄位 以下方式請注意 1、只適合單一Table 2、主要用於Grid顯示，如此方式不適合，可自行組SQL字串再夜由至ExecuteData執行
                TablePack<_WebCount> dataWork = new TablePack<_WebCount>(Connection) { LoginUserID = accountId };
                #endregion


                #region 輸出物件陣列
                r.SearchData = dataWork.DataByAdapter<m__WebCount>().FirstOrDefault();
                r.Result = true;
                return r;
                #endregion
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System;//系統執行失敗
                r.Message = PackErrMessage(ex); //回傳失敗訊息
                return r;
                #endregion
            }
        }
    }
    #endregion
    #region _CodeHead
    /// <summary>
    /// 查詢表單模組
    /// </summary>
    public class q__CodeHead : QueryBase
    {
        public String s_name { set; get; }
        public Boolean? s_IsEdit { get; set; }
    }
    public class m__CodeHead : ModuleBase
    {
        public Int32 id { get; set; }
        public String name { get; set; }
        public Boolean IsEdit { get; set; }
        public String Memo { get; set; }
    }
    /// <summary>
    /// The _CodeHead system database communicate module.   /// </summary>
    public class a__CodeHead : LogicBase<m__CodeHead, q__CodeHead, _CodeHead>
    {
        /// <summary>
        /// _CodeHead Table進行新增資料動作。
        /// </summary>
        /// <param name="md">傳入m__CodeHead class，class需先行建立(new)，其class各項屬性值需指定完成再行傳入</param>
        /// <param name="accountId">傳入Login使用者Id值</param>
        /// <returns>回傳RunInsertEnd class，請參閱RunInsertEnd說明。</returns>
        public override RunInsertEnd InsertMaster(m__CodeHead md, int accountId)
        {
            #region Variable declare area
            RunInsertEnd r = new RunInsertEnd(); //宣告回傳物件
            #endregion
            try
            {
                #region Main working
                Connection.BeginTransaction(); //開始交易鎖定
                TablePack<_CodeHead> dataWork = new TablePack<_CodeHead>(Connection) { LoginUserID = accountId };

                dataWork.NewRow(); //開始新橧作業 產生新的一行
                #region 指派值

                dataWork.SetDataRowValue(x => x.id, md.id);
                dataWork.SetDataRowValue(x => x.IsEdit, md.IsEdit);
                dataWork.SetDataRowValue(x => x.Memo, md.Memo);
                dataWork.SetDataRowValue(x => x.name, md.name);
                //dataWork.UpdateFieldsInfo(UpdateFieldsInfoType.Insert);
                dataWork.UpdateFieldsInfo(UpdateFieldsInfoType.Lang);
                //進行更新時 需同時更新系統欄位 _InsertUserID，_InsertDateTime
                #endregion
                dataWork.AddRow(); //加載至DataTable
                dataWork.UpdateDataAdapter(); //更新 DataBase Server

                Connection.EndCommit(); //交易確認

                r.Rows = dataWork.AffetCount; //取得影響筆數
                r.InsertId = dataWork.InsertAutoFieldsID; //取得新增後自動新增欄位的值
                r.Result = true; //回傳本次執行結果為成功
                dataWork.Dispose(); //釋放資料

                return r;
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
        }
        /// <summary>
        /// _CodeHead Table進行更新資料動作。
        /// </summary>
        /// <param name="md">傳入m__CodeHead class，class需先行建立(new)，其class各項屬性值需指定完成再行傳入</param>
        /// <param name="accountId">傳入Login使用者Id值</param>
        /// <returns>回傳RunUpdateEnd class，請參閱RunUpdateEnd說明。</returns>
        public override RunUpdateEnd UpdateMaster(m__CodeHead md, int accountId)
        {
            #region Variable declare area
            RunUpdateEnd r = new RunUpdateEnd();
            #endregion
            try
            {
                #region Main Working
                Connection.BeginTransaction();
                TablePack<_CodeHead> dataWork = new TablePack<_CodeHead>(Connection) { LoginUserID = accountId };

                dataWork.TableModule.KeyFieldModules[dataWork.TableModule.id.N].V = md.id; //取得ID欄位的值
                m__CodeHead md_Origin = dataWork.GetDataByKey<m__CodeHead>(); //取得Key值的Data

                if (md_Origin == null) //如有資料
                    throw new LogicRoll("Log_Err_MustHaveData"); //此區一定有資料傳出，如無資料應檢查前端id值是否有誤

                dataWork.EditFirstRow(); //編輯第一筆資料，正常只會有一筆資料。
                #region 指派值
                dataWork.SetDataRowValue(x => x.IsEdit, md.IsEdit);
                dataWork.SetDataRowValue(x => x.Memo, md.Memo);
                dataWork.SetDataRowValue(x => x.name, md.name);
                //dataWork.UpdateFieldsInfo(UpdateFieldsInfoType.Update); //指定進行更新時 需同時更新系統欄位 _UpdateUserID，_UpdateDateTime
                #endregion
                md_Origin = null;
                dataWork.UpdateDataAdapter(); //進行變更至Database Server

                Connection.EndCommit();

                r.Rows = dataWork.AffetCount; //取得影響筆數
                r.Result = true;
                dataWork.Dispose(); //釋放資料
                return r;
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
        }
        /// <summary>
        /// _CodeHead Table進行刪除資料動作。
        /// </summary>
        /// <param name="deleteIds">傳入要刪除資料的主鍵值，此適用該Table只有單一主鍵欄位。</param>
        /// <param name="accountId">傳入Login使用者Id值</param>
        /// <returns>回傳RunDeleteEnd class，請參閱RunDeleteEnd說明。</returns>
        public override RunDeleteEnd DeleteMaster(Int32[] ids, int accountId)
        {
            //此功能主要搭配Grid介面刪除功能製作
            #region Variable declare area
            RunDeleteEnd r = new RunDeleteEnd(); //宣告刪除Result回物件
            #endregion
            try
            {
                #region Main working
                Connection.BeginTransaction();
                //1、要刪除的資料先選出來
                TablePack<_CodeHead> dataWork = new TablePack<_CodeHead>(Connection) { LoginUserID = accountId }; //宣告泛型物件並連接Connection
                dataWork.SelectFields(x => x.id); //只Select 主Key欄位
                dataWork.WhereFields(x => x.id, ids); //代入陣列Id值

                m__CodeHead[] md_Origin = dataWork.DataByAdapter<m__CodeHead>(); //取得Key值的Data

                //2、進行全部刪除
                dataWork.DeleteAll(); //先刪除DataTable
                dataWork.UpdateDataAdapter(); //在更新至DataBase Server
                Connection.EndCommit();
                r.Result = true;
                dataWork.Dispose(); //釋放資料
                md_Origin = null;
                return r;
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
        }
        /// <summary>
        /// _CodeHead Table進行動態多條件查詢資料動作。
        /// </summary>
        /// <param name="qr">>傳入q__CodeHead class，class需先行建立(new)，其class各項屬性值需指定完成再行傳入</param>
        /// <param name="accountId">傳入Login使用者Id值</param>
        /// <returns>回傳RunQueryPackage<m__CodeHead>m__CodeHead</m__CodeHead> class，請參閱RunQueryPackage說明。</returns>
        public override RunQueryPackage<m__CodeHead> SearchMaster(q__CodeHead qr, int accountId)
        {
            #region 變數宣告
            RunQueryPackage<m__CodeHead> r = new RunQueryPackage<m__CodeHead>();
            #endregion
            try
            {
                #region Select Data 區段 By 條件
                #region 設定輸出至Grid欄位 以下方式請注意 1、只適合單一Table 2、主要用於Grid顯示，如此方式不適合，可自行組SQL字串再夜由至ExecuteData執行
                TablePack<_CodeHead> dataWork = new TablePack<_CodeHead>(Connection) { LoginUserID = accountId };
                //dataWork.SelectFields(x => new { x.id, x.CodeGroup, x.Memo, x.IsEdit });
                #endregion

                #region 設定Where條件
                //if (qr.s_CodeGroup != null)
                //dataWork.WhereFields(x => x.CodeGroup, qr.s_CodeGroup, WhereCompareType.Like);
                if (qr.s_IsEdit != null)
                    dataWork.WhereFields(x => x.IsEdit, qr.s_IsEdit);
                #endregion

                #region 設定排序
                dataWork.OrderByFields(x => x.id, OrderByType.ASC);
                #endregion

                #region 輸出物件陣列
                r.SearchData = dataWork.DataByAdapter<m__CodeHead>();
                r.Result = true;
                return r;
                #endregion
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
        }
        /// <summary>
        /// _CodeHead Table進行主鍵值查詢資料動作。
        /// </summary>
        /// <param name="id">傳入主鍵Value</param>
        /// <param name="accountId">傳入Login使用者Id值</param>
        /// <returns>回傳RunOneDataEnd<m__CodeHead>m__CodeHead</m__CodeHead> class，請參閱RunOneDataEnd說明。</returns>
        public override RunOneDataEnd<m__CodeHead> GetDataMaster(int id, int accountId)
        {
            #region 變數宣告
            RunOneDataEnd<m__CodeHead> r = new RunOneDataEnd<m__CodeHead>();

            #endregion
            try
            {
                #region Main working
                TablePack<_CodeHead> dataWork = new TablePack<_CodeHead>(Connection) { LoginUserID = accountId };
                dataWork.TableModule.KeyFieldModules[dataWork.TableModule.id.N].V = id; //設定KeyValue
                m__CodeHead md = dataWork.GetDataByKey<m__CodeHead>(); //取得Key該筆資料

                if (md == null) //如無資料
                    throw new LogicRoll("Log_Err_MustHaveData"); //此區一定有資料傳出，如無資料應檢查前端id值是否有誤

                r.SearchData = md;
                r.Result = true;
                dataWork.Dispose(); //釋放資料
                return r;
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
        }
        public override RunOneDataEnd<m__CodeHead> GetDataMaster(int[] id, int accountId)
        {
            throw new NotImplementedException();
        }
    }
    #endregion
    #region _CodeSheet
    /// <summary>
    /// 查詢表單模組
    /// </summary>
    public class q__CodeSheet : QueryBase
    {
        public String s_CodeHeadId { set; get; }
    }

    /// <summary>
    /// 系統資料結構描述模組    /// </summary>
    public class m__CodeSheet : ModuleBase
    {
        public Int32 id { get; set; }
        public Int32 CodeHeadId { get; set; }
        public String Code { get; set; }
        public String Value { get; set; }
        public Int32 Sort { get; set; }
        public Boolean IsUse { get; set; }
        public Boolean IsEdit { get; set; }
        public String Memo { get; set; }
    }

    /// <summary>
    /// The _CodeSheet system database communicate module.   /// </summary>
    public class a__CodeSheet : LogicBase<m__CodeSheet, q__CodeSheet, _CodeSheet>
    {
        /// <summary>
        /// _CodeSheet Table進行新增資料動作。
        /// </summary>
        /// <param name="md">傳入m__CodeSheet class，class需先行建立(new)，其class各項屬性值需指定完成再行傳入</param>
        /// <param name="accountId">傳入Login使用者Id值</param>
        /// <returns>回傳RunInsertEnd class，請參閱RunInsertEnd說明。</returns>
        public override RunInsertEnd InsertMaster(m__CodeSheet md, int accountId)
        {
            #region Variable declare area
            RunInsertEnd r = new RunInsertEnd(); //宣告回傳物件
            #endregion
            try
            {
                #region Main working
                Connection.BeginTransaction(); //開始交易鎖定
                TablePack<_CodeSheet> dataWork = new TablePack<_CodeSheet>(Connection) { LoginUserID = accountId };

                dataWork.NewRow(); //開始新橧作業 產生新的一行
                #region 指派值
                dataWork.SetDataRowValue(x => x.id, md.id);
                dataWork.SetDataRowValue(x => x.CodeHeadId, md.CodeHeadId);
                dataWork.SetDataRowValue(x => x.Code, md.Code);
                dataWork.SetDataRowValue(x => x.Value, md.Value);
                dataWork.SetDataRowValue(x => x.Sort, md.Sort);
                dataWork.SetDataRowValue(x => x.IsUse, md.IsUse);
                dataWork.SetDataRowValue(x => x.IsEdit, md.IsEdit);
                dataWork.SetDataRowValue(x => x.Memo, md.Memo);

                //dataWork.UpdateFieldsInfo(UpdateFieldsInfoType.Insert);
                dataWork.UpdateFieldsInfo(UpdateFieldsInfoType.Lang);
                //進行更新時 需同時更新系統欄位 _InsertUserID，_InsertDateTime
                #endregion
                dataWork.AddRow(); //加載至DataTable
                dataWork.UpdateDataAdapter(); //更新 DataBase Server

                Connection.EndCommit(); //交易確認

                r.Rows = dataWork.AffetCount; //取得影響筆數
                r.InsertId = dataWork.InsertAutoFieldsID; //取得新增後自動新增欄位的值
                r.Result = true; //回傳本次執行結果為成功
                dataWork.Dispose(); //釋放資料

                return r;
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼

                Log.Write(this.logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);

                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System;//系統執行失敗
                r.Message = PackErrMessage(ex); //回傳失敗訊息
                return r;
                #endregion
            }
        }
        /// <summary>
        /// _CodeSheet Table進行更新資料動作。
        /// </summary>
        /// <param name="md">傳入m__CodeSheet class，class需先行建立(new)，其class各項屬性值需指定完成再行傳入</param>
        /// <param name="accountId">傳入Login使用者Id值</param>
        /// <returns>回傳RunUpdateEnd class，請參閱RunUpdateEnd說明。</returns>
        public override RunUpdateEnd UpdateMaster(m__CodeSheet md, int accountId)
        {
            #region Variable declare area
            RunUpdateEnd r = new RunUpdateEnd();
            #endregion
            try
            {
                #region Main Working
                Connection.BeginTransaction();
                TablePack<_CodeSheet> dataWork = new TablePack<_CodeSheet>(Connection) { LoginUserID = accountId };
                dataWork.TableModule.KeyFieldModules[dataWork.TableModule.Code.N].V = md.Code; //取得ID欄位的值
                dataWork.TableModule.KeyFieldModules[dataWork.TableModule.CodeHeadId.N].V = md.CodeHeadId; //取得ID欄位的值
                dataWork.TableModule.KeyFieldModules[dataWork.TableModule._語系.N].V = System.Globalization.CultureInfo.CurrentCulture.Name; //取得ID欄位的值

                m__CodeSheet md_Origin = dataWork.GetDataByKey<m__CodeSheet>(); //取得Key值的Data

                if (md_Origin == null) //如有資料
                    throw new LogicRoll("Log_Err_MustHaveData"); //此區一定有資料傳出，如無資料應檢查前端id值是否有誤

                dataWork.EditFirstRow(); //編輯第一筆資料，正常只會有一筆資料。
                #region 指派值

                dataWork.SetDataRowValue(x => x.Code, md.Code);
                dataWork.SetDataRowValue(x => x.Value, md.Value);
                dataWork.SetDataRowValue(x => x.Sort, md.Sort);
                dataWork.SetDataRowValue(x => x.IsUse, md.IsUse);
                dataWork.SetDataRowValue(x => x.IsEdit, md.IsEdit);
                dataWork.SetDataRowValue(x => x.Memo, md.Memo);

                //dataWork.UpdateFieldsInfo(UpdateFieldsInfoType.Update); //指定進行更新時 需同時更新系統欄位 _UpdateUserID，_UpdateDateTime
                #endregion
                md_Origin = null;
                dataWork.UpdateDataAdapter(); //進行變更至Database Server

                Connection.EndCommit();

                r.Rows = dataWork.AffetCount; //取得影響筆數
                r.Result = true;
                dataWork.Dispose(); //釋放資料
                return r;
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System;//系統執行失敗
                r.Message = PackErrMessage(ex); //回傳失敗訊息
                return r;
                #endregion
            }
        }
        /// <summary>
        /// _CodeSheet Table進行刪除資料動作。
        /// </summary>
        /// <param name="deleteIds">傳入要刪除資料的主鍵值，此適用該Table只有單一主鍵欄位。</param>
        /// <param name="accountId">傳入Login使用者Id值</param>
        /// <returns>回傳RunDeleteEnd class，請參閱RunDeleteEnd說明。</returns>
        public override RunDeleteEnd DeleteMaster(Int32[] ids, int accountId)
        {
            //此功能主要搭配Grid介面刪除功能製作
            #region Variable declare area
            RunDeleteEnd r = new RunDeleteEnd(); //宣告刪除Result回物件
            #endregion
            try
            {
                #region Main working
                Connection.BeginTransaction();
                //1、要刪除的資料先選出來
                TablePack<_CodeSheet> dataWork = new TablePack<_CodeSheet>(Connection) { LoginUserID = accountId }; //宣告泛型物件並連接Connection
                dataWork.SelectFields(x => new { x.id, x._語系, x.CodeHeadId, x.Code }); //只Select 主Key欄位
                dataWork.WhereFields(x => x.id, ids); //代入陣列Id值
                m__CodeSheet[] md_Origin = dataWork.DataByAdapter<m__CodeSheet>(); //取得Key值的Data

                //2、進行全部刪除
                dataWork.DeleteAll(); //先刪除DataTable
                dataWork.UpdateDataAdapter(); //在更新至DataBase Server
                Connection.EndCommit();
                r.Result = true;
                dataWork.Dispose(); //釋放資料
                md_Origin = null;
                return r;
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System;//系統執行失敗
                r.Message = PackErrMessage(ex); //回傳失敗訊息
                return r;
                #endregion
            }
        }
        /// <summary>
        /// _CodeSheet Table進行動態多條件查詢資料動作。
        /// </summary>
        /// <param name="qr">>傳入q__CodeSheet class，class需先行建立(new)，其class各項屬性值需指定完成再行傳入</param>
        /// <param name="accountId">傳入Login使用者Id值</param>
        /// <returns>回傳RunQueryPackage<m__CodeSheet>m__CodeSheet</m__CodeSheet> class，請參閱RunQueryPackage說明。</returns>
        public override RunQueryPackage<m__CodeSheet> SearchMaster(q__CodeSheet qr, int accountId)
        {
            #region 變數宣告
            RunQueryPackage<m__CodeSheet> r = new RunQueryPackage<m__CodeSheet>();
            #endregion
            try
            {
                #region Select Data 區段 By 條件
                #region 設定輸出至Grid欄位 以下方式請注意 1、只適合單一Table 2、主要用於Grid顯示，如此方式不適合，可自行組SQL字串再夜由至ExecuteData執行
                TablePack<_CodeSheet> dataWork = new TablePack<_CodeSheet>(Connection) { LoginUserID = accountId };
                dataWork.SelectFields(x => new { x.id, x.Code, x.CodeHeadId, x.Value, x.Sort, x.IsUse });
                #endregion

                #region 設定Where條件
                if (qr.s_CodeHeadId != null)
                    dataWork.WhereFields(x => x.CodeHeadId, qr.s_CodeHeadId);

                dataWork.WhereLang();
                #endregion

                #region 設定排序

                dataWork.OrderByFields(x => x.Sort);

                #endregion

                #region 輸出物件陣列
                r.SearchData = dataWork.DataByAdapter<m__CodeSheet>();
                r.Result = true;
                return r;
                #endregion
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System;//系統執行失敗
                r.Message = PackErrMessage(ex); //回傳失敗訊息
                return r;
                #endregion
            }
        }
        /// <summary>
        /// _CodeSheet Table進行主鍵值查詢資料動作。
        /// </summary>
        /// <param name="id">傳入主鍵Value</param>
        /// <param name="accountId">傳入Login使用者Id值</param>
        /// <returns>回傳RunOneDataEnd<m__CodeSheet>m__CodeSheet</m__CodeSheet> class，請參閱RunOneDataEnd說明。</returns>
        public override RunOneDataEnd<m__CodeSheet> GetDataMaster(int id, int accountId)
        {
            #region 變數宣告
            RunOneDataEnd<m__CodeSheet> r = new RunOneDataEnd<m__CodeSheet>();

            #endregion
            try
            {
                #region Main working
                TablePack<_CodeSheet> dataWork = new TablePack<_CodeSheet>(Connection) { LoginUserID = accountId };
                dataWork.TableModule.KeyFieldModules[dataWork.TableModule.Code.N].V = id; //設定KeyValue
                m__CodeSheet md = dataWork.GetDataByKey<m__CodeSheet>(); //取得Key該筆資料

                if (md == null) //如無資料
                    throw new LogicRoll("Log_Err_MustHaveData"); //此區一定有資料傳出，如無資料應檢查前端id值是否有誤

                r.SearchData = md;
                r.Result = true;
                dataWork.Dispose(); //釋放資料
                return r;
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System;//系統執行失敗
                r.Message = PackErrMessage(ex); //回傳失敗訊息
                return r;
                #endregion
            }
        }
        public override RunOneDataEnd<m__CodeSheet> GetDataMaster(int[] id, int accountId)
        {
            throw new NotImplementedException();
        }

        public RunQueryPackage<_Code> GroupCodes(BaseSheet qr, int accountId)
        {
            #region 變數宣告
            RunQueryPackage<_Code> r = new RunQueryPackage<_Code>();
            #endregion
            try
            {
                #region Select Data 區段 By 條件
                #region 設定輸出至Grid欄位 以下方式請注意 1、只適合單一Table 2、主要用於Grid顯示，如此方式不適合，可自行組SQL字串再夜由至ExecuteData執行
                TablePack<_CodeSheet> dataWork = new TablePack<_CodeSheet>(Connection) { LoginUserID = accountId };
                dataWork.SelectFields(x => new { x.Code, x.Value });
                #endregion

                #region 設定Where條件
                dataWork.WhereFields(x => x.CodeHeadId, qr.CodeGroup);
                dataWork.WhereLang();
                #endregion

                #region 設定排序
                dataWork.OrderByFields(x => x.Sort);
                #endregion

                #region 輸出物件陣列
                List<_Code> l_c = new List<_Code>();
                var ms = dataWork.DataByAdapter<m__CodeSheet>();
                foreach (var m in ms)
                    l_c.Add(new _Code() { Code = m.Code, Value = m.Value });

                r.SearchData = l_c.ToArray();
                r.Result = true;
                return r;
                #endregion
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System;//系統執行失敗
                r.Message = PackErrMessage(ex); //回傳失敗訊息
                return r;
                #endregion
            }
        }
    }
    #endregion
    #region _Parm

    #region Basic Function
    public class q__ParmFloat : QueryBase
    {
        public String s_ParmName { set; get; }
    }
    public class m__ParmFloat : ModuleBase
    {
        public String ParmName { get; set; }
        public Decimal Value { get; set; }
        public String Memo { get; set; }
    }
    public class a__ParmFloat : LogicBase<m__ParmFloat, q__ParmFloat, _ParmFloat>
    {
        public override RunInsertEnd InsertMaster(m__ParmFloat md, int accountId)
        {
            throw new NotImplementedException();
        }
        public override RunDeleteEnd DeleteMaster(int[] ids, int accountId)
        {
            throw new NotImplementedException();
        }
        public override RunOneDataEnd<m__ParmFloat> GetDataMaster(int id, int accountId)
        {
            throw new NotImplementedException();
        }
        public override RunOneDataEnd<m__ParmFloat> GetDataMaster(int[] id, int accountId)
        {
            throw new NotImplementedException();
        }

        public override RunUpdateEnd UpdateMaster(m__ParmFloat md, int accountId)
        {
            #region Variable declare area
            RunUpdateEnd r = new RunUpdateEnd();
            #endregion
            try
            {
                #region Main Working
                Connection.BeginTransaction();
                TablePack<_ParmFloat> dataWork = new TablePack<_ParmFloat>(Connection) { LoginUserID = this.logPlamInfo.UserId, LoginUnitID = this.logPlamInfo.UnitId };

                dataWork.TableModule.KeyFieldModules[dataWork.TableModule.ParmName.N].V = md.ParmName; //取得ID欄位的值
                m__ParmFloat md_Origin = dataWork.GetDataByKey<m__ParmFloat>(); //取得Key值的Data

                if (md_Origin == null) //如有資料
                    throw new LogicRoll("Log_Err_MustHaveData"); //此區一定有資料傳出，如無資料應檢查前端id值是否有誤

                dataWork.EditFirstRow(); //編輯第一筆資料，正常只會有一筆資料。
                #region 指派值

                dataWork.SetDataRowValue(x => x.Value, md.Value);

                dataWork.UpdateFieldsInfo(UpdateFieldsInfoType.Update); //指定進行更新時 需同時更新系統欄位 _修改人員，_修改日期
                #endregion
                md_Origin = null;
                dataWork.UpdateDataAdapter(); //進行變更至Database Server

                Connection.EndCommit();

                r.Rows = dataWork.AffetCount; //取得影響筆數
                r.Result = true;
                dataWork.Dispose(); //釋放資料
                return r;
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System; //系統失敗
                r.Message = PackErrMessage(ex); //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
        }
        public override RunQueryPackage<m__ParmFloat> SearchMaster(q__ParmFloat qr, int accountId)
        {
            #region 變數宣告
            RunQueryPackage<m__ParmFloat> r = new RunQueryPackage<m__ParmFloat>();
            #endregion
            try
            {
                #region Select Data 區段 By 條件
                #region 設定輸出至Grid欄位 以下方式請注意 1、只適合單一Table 2、主要用於Grid顯示，如此方式不適合，可自行組SQL字串再夜由至ExecuteData執行
                TablePack<_ParmFloat> dataWork = new TablePack<_ParmFloat>(Connection) { LoginUserID = this.logPlamInfo.UserId, LoginUnitID = this.logPlamInfo.UnitId };
                dataWork.SelectFields(x => new { x.ParmName, x.Value });
                #endregion

                #region 設定Where條件
                if (qr.s_ParmName != null)
                    dataWork.WhereFields(x => x.ParmName, qr.s_ParmName);

                #endregion

                #region 設定排序
                if (qr.sidx == null)
                    dataWork.OrderByFields(x => x.ParmName); //預設排序

                #endregion

                #region 輸出物件陣列
                r.SearchData = dataWork.DataByAdapter<m__ParmFloat>();
                r.Result = true;
                return r;
                #endregion
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System; //系統失敗
                r.Message = PackErrMessage(ex); //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
        }
        public RunOneDataEnd<m__ParmFloat> GetDataMaster(String id, int accountId)
        {
            #region 變數宣告
            RunOneDataEnd<m__ParmFloat> r = new RunOneDataEnd<m__ParmFloat>();

            #endregion
            try
            {
                #region Main working
                TablePack<_ParmFloat> dataWork = new TablePack<_ParmFloat>(Connection) { LoginUserID = this.logPlamInfo.UserId, LoginUnitID = this.logPlamInfo.UnitId };
                dataWork.TableModule.KeyFieldModules[dataWork.TableModule.ParmName.N].V = id; //設定KeyValue
                m__ParmFloat md = dataWork.GetDataByKey<m__ParmFloat>(); //取得Key該筆資料

                if (md == null) //如無資料
                    throw new LogicRoll("Log_Err_MustHaveData"); //此區一定有資料傳出，如無資料應檢查前端id值是否有誤

                r.SearchData = md;
                r.Result = true;
                dataWork.Dispose(); //釋放資料
                return r;
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System; //系統失敗
                r.Message = PackErrMessage(ex); //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
        }
    }

    public class q__ParmString : QueryBase
    {
        public String s_ParmName { set; get; }
    }
    public class m__ParmString : ModuleBase
    {
        public String ParmName { get; set; }
        public String Value { get; set; }
        public String Memo { get; set; }
    }
    public class a__ParmString : LogicBase<m__ParmString, q__ParmString, _ParmString>
    {
        public override RunInsertEnd InsertMaster(m__ParmString md, int accountId)
        {
            throw new NotImplementedException();
        }
        public override RunDeleteEnd DeleteMaster(int[] ids, int accountId)
        {
            throw new NotImplementedException();
        }
        public override RunOneDataEnd<m__ParmString> GetDataMaster(int id, int accountId)
        {
            throw new NotImplementedException();
        }
        public override RunOneDataEnd<m__ParmString> GetDataMaster(int[] id, int accountId)
        {
            throw new NotImplementedException();
        }

        public override RunUpdateEnd UpdateMaster(m__ParmString md, int accountId)
        {
            #region Variable declare area
            RunUpdateEnd r = new RunUpdateEnd();
            #endregion
            try
            {
                #region Main Working
                Connection.BeginTransaction();
                TablePack<_ParmString> dataWork = new TablePack<_ParmString>(Connection) { LoginUserID = this.logPlamInfo.UserId, LoginUnitID = this.logPlamInfo.UnitId };

                dataWork.TableModule.KeyFieldModules[dataWork.TableModule.ParmName.N].V = md.ParmName; //取得ID欄位的值
                m__ParmString md_Origin = dataWork.GetDataByKey<m__ParmString>(); //取得Key值的Data

                if (md_Origin == null) //如有資料
                    throw new LogicRoll("Log_Err_MustHaveData"); //此區一定有資料傳出，如無資料應檢查前端id值是否有誤

                dataWork.EditFirstRow(); //編輯第一筆資料，正常只會有一筆資料。
                #region 指派值

                dataWork.SetDataRowValue(x => x.Value, md.Value);

                dataWork.UpdateFieldsInfo(UpdateFieldsInfoType.Update); //指定進行更新時 需同時更新系統欄位 _修改人員，_修改日期
                #endregion
                md_Origin = null;
                dataWork.UpdateDataAdapter(); //進行變更至Database Server

                Connection.EndCommit();

                r.Rows = dataWork.AffetCount; //取得影響筆數
                r.Result = true;
                dataWork.Dispose(); //釋放資料
                return r;
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System; //系統失敗
                r.Message = PackErrMessage(ex); //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
        }
        public override RunQueryPackage<m__ParmString> SearchMaster(q__ParmString qr, int accountId)
        {
            #region 變數宣告
            RunQueryPackage<m__ParmString> r = new RunQueryPackage<m__ParmString>();
            #endregion
            try
            {
                #region Select Data 區段 By 條件
                #region 設定輸出至Grid欄位 以下方式請注意 1、只適合單一Table 2、主要用於Grid顯示，如此方式不適合，可自行組SQL字串再夜由至ExecuteData執行
                TablePack<_ParmString> dataWork = new TablePack<_ParmString>(Connection) { LoginUserID = this.logPlamInfo.UserId, LoginUnitID = this.logPlamInfo.UnitId };
                dataWork.SelectFields(x => new { x.ParmName, x.Value });
                #endregion

                #region 設定Where條件
                if (qr.s_ParmName != null)
                    dataWork.WhereFields(x => x.ParmName, qr.s_ParmName);

                #endregion

                #region 設定排序
                if (qr.sidx == null)
                    dataWork.OrderByFields(x => x.ParmName); //預設排序

                #endregion

                #region 輸出物件陣列
                r.SearchData = dataWork.DataByAdapter<m__ParmString>();
                r.Result = true;
                return r;
                #endregion
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System; //系統失敗
                r.Message = PackErrMessage(ex); //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
        }
        public RunOneDataEnd<m__ParmString> GetDataMaster(String id, int accountId)
        {
            #region 變數宣告
            RunOneDataEnd<m__ParmString> r = new RunOneDataEnd<m__ParmString>();

            #endregion
            try
            {
                #region Main working
                TablePack<_ParmString> dataWork = new TablePack<_ParmString>(Connection) { LoginUserID = this.logPlamInfo.UserId, LoginUnitID = this.logPlamInfo.UnitId };
                dataWork.TableModule.KeyFieldModules[dataWork.TableModule.ParmName.N].V = id; //設定KeyValue
                m__ParmString md = dataWork.GetDataByKey<m__ParmString>(); //取得Key該筆資料

                if (md == null) //如無資料
                    throw new LogicRoll("Log_Err_MustHaveData"); //此區一定有資料傳出，如無資料應檢查前端id值是否有誤

                r.SearchData = md;
                r.Result = true;
                dataWork.Dispose(); //釋放資料
                return r;
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System; //系統失敗
                r.Message = PackErrMessage(ex); //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
        }
    }

    public class q__ParmInt : QueryBase
    {
        public String s_ParmName { set; get; }
    }
    public class m__ParmInt : ModuleBase
    {
        public String ParmName { get; set; }
        public Int32 Value { get; set; }
        public String Memo { get; set; }
    }
    public class a__ParmInt : LogicBase<m__ParmInt, q__ParmInt, _ParmInt>
    {
        public override RunInsertEnd InsertMaster(m__ParmInt md, int accountId)
        {
            throw new NotImplementedException();
        }
        public override RunDeleteEnd DeleteMaster(int[] ids, int accountId)
        {
            throw new NotImplementedException();
        }
        public override RunOneDataEnd<m__ParmInt> GetDataMaster(int id, int accountId)
        {
            throw new NotImplementedException();
        }
        public override RunOneDataEnd<m__ParmInt> GetDataMaster(int[] id, int accountId)
        {
            throw new NotImplementedException();
        }

        public override RunUpdateEnd UpdateMaster(m__ParmInt md, int accountId)
        {
            #region Variable declare area
            RunUpdateEnd r = new RunUpdateEnd();
            #endregion
            try
            {
                #region Main Working
                Connection.BeginTransaction();
                TablePack<_ParmInt> dataWork = new TablePack<_ParmInt>(Connection) { LoginUserID = this.logPlamInfo.UserId, LoginUnitID = this.logPlamInfo.UnitId };

                dataWork.TableModule.KeyFieldModules[dataWork.TableModule.ParmName.N].V = md.ParmName; //取得ID欄位的值
                m__ParmInt md_Origin = dataWork.GetDataByKey<m__ParmInt>(); //取得Key值的Data

                if (md_Origin == null) //如有資料
                    throw new LogicRoll("Log_Err_MustHaveData"); //此區一定有資料傳出，如無資料應檢查前端id值是否有誤

                dataWork.EditFirstRow(); //編輯第一筆資料，正常只會有一筆資料。
                #region 指派值

                dataWork.SetDataRowValue(x => x.Value, md.Value);

                dataWork.UpdateFieldsInfo(UpdateFieldsInfoType.Update); //指定進行更新時 需同時更新系統欄位 _修改人員，_修改日期
                #endregion
                md_Origin = null;
                dataWork.UpdateDataAdapter(); //進行變更至Database Server

                Connection.EndCommit();

                r.Rows = dataWork.AffetCount; //取得影響筆數
                r.Result = true;
                dataWork.Dispose(); //釋放資料
                return r;
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System; //系統失敗
                r.Message = PackErrMessage(ex); //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
        }
        public override RunQueryPackage<m__ParmInt> SearchMaster(q__ParmInt qr, int accountId)
        {
            #region 變數宣告
            RunQueryPackage<m__ParmInt> r = new RunQueryPackage<m__ParmInt>();
            #endregion
            try
            {
                #region Select Data 區段 By 條件
                #region 設定輸出至Grid欄位 以下方式請注意 1、只適合單一Table 2、主要用於Grid顯示，如此方式不適合，可自行組SQL字串再夜由至ExecuteData執行
                TablePack<_ParmInt> dataWork = new TablePack<_ParmInt>(Connection) { LoginUserID = this.logPlamInfo.UserId, LoginUnitID = this.logPlamInfo.UnitId };
                dataWork.SelectFields(x => new { x.ParmName, x.Value });
                #endregion

                #region 設定Where條件
                if (qr.s_ParmName != null)
                    dataWork.WhereFields(x => x.ParmName, qr.s_ParmName);

                #endregion

                #region 設定排序
                if (qr.sidx == null)
                    dataWork.OrderByFields(x => x.ParmName); //預設排序

                #endregion

                #region 輸出物件陣列
                r.SearchData = dataWork.DataByAdapter<m__ParmInt>();
                r.Result = true;
                return r;
                #endregion
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System; //系統失敗
                r.Message = PackErrMessage(ex); //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
        }
        public RunOneDataEnd<m__ParmInt> GetDataMaster(String id, int accountId)
        {
            #region 變數宣告
            RunOneDataEnd<m__ParmInt> r = new RunOneDataEnd<m__ParmInt>();

            #endregion
            try
            {
                #region Main working
                TablePack<_ParmInt> dataWork = new TablePack<_ParmInt>(Connection) { LoginUserID = this.logPlamInfo.UserId, LoginUnitID = this.logPlamInfo.UnitId };
                dataWork.TableModule.KeyFieldModules[dataWork.TableModule.ParmName.N].V = id; //設定KeyValue
                m__ParmInt md = dataWork.GetDataByKey<m__ParmInt>(); //取得Key該筆資料

                if (md == null) //如無資料
                    throw new LogicRoll("Log_Err_MustHaveData"); //此區一定有資料傳出，如無資料應檢查前端id值是否有誤

                r.SearchData = md;
                r.Result = true;
                dataWork.Dispose(); //釋放資料
                return r;
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System; //系統失敗
                r.Message = PackErrMessage(ex); //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
        }
    }

    public class q__ParmDateTime : QueryBase
    {
        public String s_ParmName { set; get; }
    }
    public class m__ParmDateTime : ModuleBase
    {
        public String ParmName { get; set; }
        public DateTime Value { get; set; }
        public String Memo { get; set; }
    }
    public class a__ParmDateTime : LogicBase<m__ParmDateTime, q__ParmDateTime, _ParmDateTime>
    {
        public override RunInsertEnd InsertMaster(m__ParmDateTime md, int accountId)
        {
            throw new NotImplementedException();
        }
        public override RunDeleteEnd DeleteMaster(int[] ids, int accountId)
        {
            throw new NotImplementedException();
        }
        public override RunOneDataEnd<m__ParmDateTime> GetDataMaster(int id, int accountId)
        {
            throw new NotImplementedException();
        }
        public override RunOneDataEnd<m__ParmDateTime> GetDataMaster(int[] id, int accountId)
        {
            throw new NotImplementedException();
        }

        public override RunUpdateEnd UpdateMaster(m__ParmDateTime md, int accountId)
        {
            #region Variable declare area
            RunUpdateEnd r = new RunUpdateEnd();
            #endregion
            try
            {
                #region Main Working
                Connection.BeginTransaction();
                TablePack<_ParmDateTime> dataWork = new TablePack<_ParmDateTime>(Connection) { LoginUserID = this.logPlamInfo.UserId, LoginUnitID = this.logPlamInfo.UnitId };

                dataWork.TableModule.KeyFieldModules[dataWork.TableModule.ParmName.N].V = md.ParmName; //取得ID欄位的值
                m__ParmDateTime md_Origin = dataWork.GetDataByKey<m__ParmDateTime>(); //取得Key值的Data

                if (md_Origin == null) //如有資料
                    throw new LogicRoll("Log_Err_MustHaveData"); //此區一定有資料傳出，如無資料應檢查前端id值是否有誤

                dataWork.EditFirstRow(); //編輯第一筆資料，正常只會有一筆資料。
                #region 指派值

                dataWork.SetDataRowValue(x => x.Value, md.Value);

                dataWork.UpdateFieldsInfo(UpdateFieldsInfoType.Update); //指定進行更新時 需同時更新系統欄位 _修改人員，_修改日期
                #endregion
                md_Origin = null;
                dataWork.UpdateDataAdapter(); //進行變更至Database Server

                Connection.EndCommit();

                r.Rows = dataWork.AffetCount; //取得影響筆數
                r.Result = true;
                dataWork.Dispose(); //釋放資料
                return r;
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System; //系統失敗
                r.Message = PackErrMessage(ex); //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
        }
        public override RunQueryPackage<m__ParmDateTime> SearchMaster(q__ParmDateTime qr, int accountId)
        {
            #region 變數宣告
            RunQueryPackage<m__ParmDateTime> r = new RunQueryPackage<m__ParmDateTime>();
            #endregion
            try
            {
                #region Select Data 區段 By 條件
                #region 設定輸出至Grid欄位 以下方式請注意 1、只適合單一Table 2、主要用於Grid顯示，如此方式不適合，可自行組SQL字串再夜由至ExecuteData執行
                TablePack<_ParmDateTime> dataWork = new TablePack<_ParmDateTime>(Connection) { LoginUserID = this.logPlamInfo.UserId, LoginUnitID = this.logPlamInfo.UnitId };
                dataWork.SelectFields(x => new { x.ParmName, x.Value });
                #endregion

                #region 設定Where條件
                if (qr.s_ParmName != null)
                    dataWork.WhereFields(x => x.ParmName, qr.s_ParmName);

                #endregion

                #region 設定排序
                if (qr.sidx == null)
                    dataWork.OrderByFields(x => x.ParmName); //預設排序

                #endregion

                #region 輸出物件陣列
                r.SearchData = dataWork.DataByAdapter<m__ParmDateTime>();
                r.Result = true;
                return r;
                #endregion
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System; //系統失敗
                r.Message = PackErrMessage(ex); //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
        }
        public RunOneDataEnd<m__ParmDateTime> GetDataMaster(String id, int accountId)
        {
            #region 變數宣告
            RunOneDataEnd<m__ParmDateTime> r = new RunOneDataEnd<m__ParmDateTime>();

            #endregion
            try
            {
                #region Main working
                TablePack<_ParmDateTime> dataWork = new TablePack<_ParmDateTime>(Connection) { LoginUserID = this.logPlamInfo.UserId, LoginUnitID = this.logPlamInfo.UnitId };
                dataWork.TableModule.KeyFieldModules[dataWork.TableModule.ParmName.N].V = id; //設定KeyValue
                m__ParmDateTime md = dataWork.GetDataByKey<m__ParmDateTime>(); //取得Key該筆資料

                if (md == null) //如無資料
                    throw new LogicRoll("Log_Err_MustHaveData"); //此區一定有資料傳出，如無資料應檢查前端id值是否有誤

                r.SearchData = md;
                r.Result = true;
                dataWork.Dispose(); //釋放資料
                return r;
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System; //系統失敗
                r.Message = PackErrMessage(ex); //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
        }
    }

    public class q__ParmBit : QueryBase
    {
        public String s_ParmName { set; get; }
    }
    public class m__ParmBit : ModuleBase
    {
        public String ParmName { get; set; }
        public Boolean Value { get; set; }
        public String Memo { get; set; }
    }
    public class a__ParmBit : LogicBase<m__ParmBit, q__ParmBit, _ParmBit>
    {
        public override RunInsertEnd InsertMaster(m__ParmBit md, int accountId)
        {
            throw new NotImplementedException();
        }
        public override RunDeleteEnd DeleteMaster(int[] ids, int accountId)
        {
            throw new NotImplementedException();
        }
        public override RunOneDataEnd<m__ParmBit> GetDataMaster(int id, int accountId)
        {
            throw new NotImplementedException();
        }
        public override RunOneDataEnd<m__ParmBit> GetDataMaster(int[] id, int accountId)
        {
            throw new NotImplementedException();
        }

        public override RunUpdateEnd UpdateMaster(m__ParmBit md, int accountId)
        {
            #region Variable declare area
            RunUpdateEnd r = new RunUpdateEnd();
            #endregion
            try
            {
                #region Main Working
                Connection.BeginTransaction();
                TablePack<_ParmBit> dataWork = new TablePack<_ParmBit>(Connection) { LoginUserID = this.logPlamInfo.UserId, LoginUnitID = this.logPlamInfo.UnitId };

                dataWork.TableModule.KeyFieldModules[dataWork.TableModule.ParmName.N].V = md.ParmName; //取得ID欄位的值
                m__ParmBit md_Origin = dataWork.GetDataByKey<m__ParmBit>(); //取得Key值的Data

                if (md_Origin == null) //如有資料
                    throw new LogicRoll("Log_Err_MustHaveData"); //此區一定有資料傳出，如無資料應檢查前端id值是否有誤

                dataWork.EditFirstRow(); //編輯第一筆資料，正常只會有一筆資料。
                #region 指派值

                dataWork.SetDataRowValue(x => x.Value, md.Value);

                dataWork.UpdateFieldsInfo(UpdateFieldsInfoType.Update); //指定進行更新時 需同時更新系統欄位 _修改人員，_修改日期
                #endregion
                md_Origin = null;
                dataWork.UpdateDataAdapter(); //進行變更至Database Server

                Connection.EndCommit();

                r.Rows = dataWork.AffetCount; //取得影響筆數
                r.Result = true;
                dataWork.Dispose(); //釋放資料
                return r;
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System; //系統失敗
                r.Message = PackErrMessage(ex); //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
        }
        public override RunQueryPackage<m__ParmBit> SearchMaster(q__ParmBit qr, int accountId)
        {
            #region 變數宣告
            RunQueryPackage<m__ParmBit> r = new RunQueryPackage<m__ParmBit>();
            #endregion
            try
            {
                #region Select Data 區段 By 條件
                #region 設定輸出至Grid欄位 以下方式請注意 1、只適合單一Table 2、主要用於Grid顯示，如此方式不適合，可自行組SQL字串再夜由至ExecuteData執行
                TablePack<_ParmBit> dataWork = new TablePack<_ParmBit>(Connection) { LoginUserID = this.logPlamInfo.UserId, LoginUnitID = this.logPlamInfo.UnitId };
                dataWork.SelectFields(x => new { x.ParmName, x.Value });
                #endregion

                #region 設定Where條件
                if (qr.s_ParmName != null)
                    dataWork.WhereFields(x => x.ParmName, qr.s_ParmName);

                #endregion

                #region 設定排序
                if (qr.sidx == null)
                    dataWork.OrderByFields(x => x.ParmName); //預設排序

                #endregion

                #region 輸出物件陣列
                r.SearchData = dataWork.DataByAdapter<m__ParmBit>();
                r.Result = true;
                return r;
                #endregion
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System; //系統失敗
                r.Message = PackErrMessage(ex); //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
        }
        public RunOneDataEnd<m__ParmBit> GetDataMaster(String id, int accountId)
        {
            #region 變數宣告
            RunOneDataEnd<m__ParmBit> r = new RunOneDataEnd<m__ParmBit>();

            #endregion
            try
            {
                #region Main working
                TablePack<_ParmBit> dataWork = new TablePack<_ParmBit>(Connection) { LoginUserID = this.logPlamInfo.UserId, LoginUnitID = this.logPlamInfo.UnitId };
                dataWork.TableModule.KeyFieldModules[dataWork.TableModule.ParmName.N].V = id; //設定KeyValue
                m__ParmBit md = dataWork.GetDataByKey<m__ParmBit>(); //取得Key該筆資料

                if (md == null) //如無資料
                    throw new LogicRoll("Log_Err_MustHaveData"); //此區一定有資料傳出，如無資料應檢查前端id值是否有誤

                r.SearchData = md;
                r.Result = true;
                dataWork.Dispose(); //釋放資料
                return r;
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System; //系統失敗
                r.Message = PackErrMessage(ex); //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
        }
    }
    #endregion

    public class _Parm : LogicBase
    {
        public Boolean WebIsOpen
        {
            get
            {
                a__ParmBit ac_Parm = new a__ParmBit() { Connection = this.Connection, logPlamInfo = this.logPlamInfo };
                return ac_Parm.GetDataMaster("WebIsOpen", 0).SearchData.Value;
            }

            set
            {
                a__ParmBit ac_Parm = new a__ParmBit() { Connection = this.Connection, logPlamInfo = this.logPlamInfo };
                var r1 = ac_Parm.UpdateMaster(new m__ParmBit() { ParmName = "WebIsOpen", Value = value }, 0);
            }
        }
        public Decimal 兩件以上運費
        {
            get
            {
                a__ParmFloat ac_Parm = new a__ParmFloat() { Connection = this.Connection, logPlamInfo = this.logPlamInfo };
                return ac_Parm.GetDataMaster("兩件以上運費", 0).SearchData.Value;
            }

            set
            {
                a__ParmFloat ac_Parm = new a__ParmFloat() { Connection = this.Connection, logPlamInfo = this.logPlamInfo };
                var r1 = ac_Parm.UpdateMaster(new m__ParmFloat() { ParmName = "兩件以上運費", Value = value }, 0);
            }
        }
        public Decimal 訂單運費_少於
        {
            get
            {
                a__ParmFloat ac_Parm = new a__ParmFloat() { Connection = this.Connection, logPlamInfo = this.logPlamInfo };
                return ac_Parm.GetDataMaster("訂單運費_少於", 0).SearchData.Value;
            }

            set
            {
                a__ParmFloat ac_Parm = new a__ParmFloat() { Connection = this.Connection, logPlamInfo = this.logPlamInfo };
                var r1 = ac_Parm.UpdateMaster(new m__ParmFloat() { ParmName = "訂單運費_少於", Value = value }, 0);
            }
        }
        public Decimal 產品價格折扣
        {
            get
            {
                a__ParmFloat ac_Parm = new a__ParmFloat() { Connection = this.Connection, logPlamInfo = this.logPlamInfo };
                return ac_Parm.GetDataMaster("產品價格折扣", 0).SearchData.Value;
            }

            set
            {
                a__ParmFloat ac_Parm = new a__ParmFloat() { Connection = this.Connection, logPlamInfo = this.logPlamInfo };
                var r1 = ac_Parm.UpdateMaster(new m__ParmFloat() { ParmName = "產品價格折扣", Value = value }, 0);
            }
        }
        public Decimal 貨到付款手續費
        {
            get
            {
                a__ParmFloat ac_Parm = new a__ParmFloat() { Connection = this.Connection, logPlamInfo = this.logPlamInfo };
                return ac_Parm.GetDataMaster("貨到付款手續費", 0).SearchData.Value;
            }

            set
            {
                a__ParmFloat ac_Parm = new a__ParmFloat() { Connection = this.Connection, logPlamInfo = this.logPlamInfo };
                var r1 = ac_Parm.UpdateMaster(new m__ParmFloat() { ParmName = "貨到付款手續費", Value = value }, 0);
            }
        }
        public Decimal 單樣產品運費
        {
            get
            {
                a__ParmFloat ac_Parm = new a__ParmFloat() { Connection = this.Connection, logPlamInfo = this.logPlamInfo };
                return ac_Parm.GetDataMaster("單樣產品運費", 0).SearchData.Value;
            }

            set
            {
                a__ParmFloat ac_Parm = new a__ParmFloat() { Connection = this.Connection, logPlamInfo = this.logPlamInfo };
                var r1 = ac_Parm.UpdateMaster(new m__ParmFloat() { ParmName = "單樣產品運費", Value = value }, 0);
            }
        }
        public Decimal 需付運費
        {
            get
            {
                a__ParmFloat ac_Parm = new a__ParmFloat() { Connection = this.Connection, logPlamInfo = this.logPlamInfo };
                return ac_Parm.GetDataMaster("需付運費", 0).SearchData.Value;
            }

            set
            {
                a__ParmFloat ac_Parm = new a__ParmFloat() { Connection = this.Connection, logPlamInfo = this.logPlamInfo };
                var r1 = ac_Parm.UpdateMaster(new m__ParmFloat() { ParmName = "需付運費", Value = value }, 0);
            }
        }
        public Int32 訂單運費設定
        {
            get
            {
                a__ParmInt ac_Parm = new a__ParmInt() { Connection = this.Connection, logPlamInfo = this.logPlamInfo };
                return ac_Parm.GetDataMaster("訂單運費設定", 0).SearchData.Value;
            }

            set
            {
                a__ParmInt ac_Parm = new a__ParmInt() { Connection = this.Connection, logPlamInfo = this.logPlamInfo };
                var r1 = ac_Parm.UpdateMaster(new m__ParmInt() { ParmName = "訂單運費設定", Value = value }, 0);
            }
        }
        public Int32 產品價格方式
        {
            get
            {
                a__ParmInt ac_Parm = new a__ParmInt() { Connection = this.Connection, logPlamInfo = this.logPlamInfo };
                return ac_Parm.GetDataMaster("產品價格方式", 0).SearchData.Value;
            }

            set
            {
                a__ParmInt ac_Parm = new a__ParmInt() { Connection = this.Connection, logPlamInfo = this.logPlamInfo };
                var r1 = ac_Parm.UpdateMaster(new m__ParmInt() { ParmName = "產品價格方式", Value = value }, 0);
            }
        }
        public String ATM戶名
        {
            get
            {
                a__ParmString ac_Parm = new a__ParmString() { Connection = this.Connection, logPlamInfo = this.logPlamInfo };
                return ac_Parm.GetDataMaster("ATM戶名", 0).SearchData.Value;
            }

            set
            {
                a__ParmString ac_Parm = new a__ParmString() { Connection = this.Connection, logPlamInfo = this.logPlamInfo };
                var r1 = ac_Parm.UpdateMaster(new m__ParmString() { ParmName = "ATM戶名", Value = value }, 0);
            }
        }
        public String ATM代碼
        {
            get
            {
                a__ParmString ac_Parm = new a__ParmString() { Connection = this.Connection, logPlamInfo = this.logPlamInfo };
                return ac_Parm.GetDataMaster("ATM代碼", 0).SearchData.Value;
            }

            set
            {
                a__ParmString ac_Parm = new a__ParmString() { Connection = this.Connection, logPlamInfo = this.logPlamInfo };
                var r1 = ac_Parm.UpdateMaster(new m__ParmString() { ParmName = "ATM代碼", Value = value }, 0);
            }
        }
        public String ATM銀行
        {
            get
            {
                a__ParmString ac_Parm = new a__ParmString() { Connection = this.Connection, logPlamInfo = this.logPlamInfo };
                return ac_Parm.GetDataMaster("ATM銀行", 0).SearchData.Value;
            }

            set
            {
                a__ParmString ac_Parm = new a__ParmString() { Connection = this.Connection, logPlamInfo = this.logPlamInfo };
                var r1 = ac_Parm.UpdateMaster(new m__ParmString() { ParmName = "ATM銀行", Value = value }, 0);
            }
        }
        public String 轉入帳號
        {
            get
            {
                a__ParmString ac_Parm = new a__ParmString() { Connection = this.Connection, logPlamInfo = this.logPlamInfo };
                return ac_Parm.GetDataMaster("轉入帳號", 0).SearchData.Value;
            }

            set
            {
                a__ParmString ac_Parm = new a__ParmString() { Connection = this.Connection, logPlamInfo = this.logPlamInfo };
                var r1 = ac_Parm.UpdateMaster(new m__ParmString() { ParmName = "轉入帳號", Value = value }, 0);
            }
        }
    }
    public class Parm {
        public Boolean WebIsOpen { get; set; }
        public Decimal 兩件以上運費 { get; set; }
        public Decimal 訂單運費_少於 { get; set; }
        public Decimal 產品價格折扣 { get; set; }
        public Decimal 貨到付款手續費 { get; set; }
        public Decimal 單樣產品運費 { get; set; }
        public Decimal 需付運費 { get; set; }
        public Int32 訂單運費設定 { get; set; }
        public Int32 產品價格方式 { get; set; }
        public String ATM戶名 { get; set; }
        public String ATM代碼 { get; set; }
        public String ATM銀行 { get; set; }
        public String 轉入帳號 { get; set; } 
    }

    #endregion
    #region _UserLoginLog
    /// <summary>
    /// 查詢表單模組
    /// </summary>
    public class q__UserLoginLog : QueryBase
    {
        public String s_account { get; set; }
    }
    public class m__UserLoginLog : ModuleBase
    {
        public Int32 id { get; set; }
        public String ip { get; set; }
        public String account { get; set; }
        public DateTime logintime { get; set; }
        public String browers { get; set; }
    }
    /// <summary>
    /// The _UserLoginLog system database communicate module.   
    /// </summary>
    public class a__UserLoginLog : LogicBase<m__UserLoginLog, q__UserLoginLog, _UserLoginLog>
    {
        public override RunInsertEnd InsertMaster(m__UserLoginLog md, int accountId)
        {
            #region Variable declare area
            RunInsertEnd r = new RunInsertEnd(); //宣告回傳物件
            #endregion
            try
            {
                #region Main working
                Connection.BeginTransaction(); //開始交易鎖定
                TablePack<_UserLoginLog> dataWork = new TablePack<_UserLoginLog>(Connection) { LoginUserID = this.logPlamInfo.UserId, LoginUnitID = this.logPlamInfo.UnitId };

                dataWork.NewRow(); //開始新橧作業 產生新的一行
                #region 指派值
                md.id = GetIDX();
                dataWork.SetDataRowValue(x => x.id, md.id);
                dataWork.SetDataRowValue(x => x.account, md.account);
                dataWork.SetDataRowValue(x => x.ip, md.ip);
                dataWork.SetDataRowValue(x => x.logintime, md.logintime);
                dataWork.SetDataRowValue(x => x.browers, md.browers);

                //進行更新時 需同時更新系統欄位 _InsertUserID，_InsertDateTime
                #endregion
                dataWork.AddRow(); //加載至DataTable
                dataWork.UpdateDataAdapter(); //更新 DataBase Server

                Connection.EndCommit(); //交易確認
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, "OK");
                r.Rows = dataWork.AffetCount; //取得影響筆數
                r.InsertId = dataWork.InsertAutoFieldsID; //取得新增後自動新增欄位的值
                r.Result = true; //回傳本次執行結果為成功
                dataWork.Dispose(); //釋放資料

                return r;
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System; //系統失敗
                r.Message = PackErrMessage(ex); //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
        }
        public override RunUpdateEnd UpdateMaster(m__UserLoginLog md, int accountId)
        {
            throw new NotImplementedException();
        }
        public override RunDeleteEnd DeleteMaster(int[] ids, int accountId)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// _UserLoginLog Table進行動態多條件查詢資料動作。
        /// </summary>
        /// <param name="qr">>傳入q__UserLoginLog class，class需先行建立(new)，其class各項屬性值需指定完成再行傳入</param>
        /// <param name="accountId">傳入Login使用者Id值</param>
        /// <returns>回傳RunQueryPackage<m__UserLoginLog>m__UserLoginLog</m__UserLoginLog> class，請參閱RunQueryPackage說明。</returns>
        public override RunQueryPackage<m__UserLoginLog> SearchMaster(q__UserLoginLog qr, int accountId)
        {
            #region 變數宣告
            RunQueryPackage<m__UserLoginLog> r = new RunQueryPackage<m__UserLoginLog>();
            #endregion
            try
            {
                #region Select Data 區段 By 條件
                #region 設定輸出至Grid欄位 以下方式請注意 1、只適合單一Table 2、主要用於Grid顯示，如此方式不適合，可自行組SQL字串再夜由至ExecuteData執行
                TablePack<_UserLoginLog> dataWork = new TablePack<_UserLoginLog>(Connection) { LoginUserID = accountId };
                if (qr.MaxRecord > 0) dataWork.TopLimit = qr.MaxRecord;
                dataWork.SelectFields(x => new { x.logintime });
                #endregion

                #region 設定Where條件

                if (qr.s_account != null)
                    dataWork.WhereFields(x => x.account, qr.s_account);
                #endregion

                #region 設定排序
                if (qr.sidx == "logintime")
                {
                    if(qr.sord.ToLower() == "desc")
                        dataWork.OrderByFields(x => x.logintime, OrderByType.DESC); //預設排序
                    else
                        dataWork.OrderByFields(x => x.logintime); //預設排序
                }

                #endregion

                #region 輸出物件陣列
                r.SearchData = dataWork.DataByAdapter<m__UserLoginLog>();
                r.Result = true;
                return r;
                #endregion
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
        }
        /// <summary>
        /// _UserLoginLog Table進行主鍵值查詢資料動作。
        /// </summary>
        /// <param name="id">傳入主鍵Value</param>
        /// <param name="accountId">傳入Login使用者Id值</param>
        /// <returns>回傳RunOneDataEnd<m__UserLoginLog>m__UserLoginLog</m__UserLoginLog> class，請參閱RunOneDataEnd說明。</returns>
        public override RunOneDataEnd<m__UserLoginLog> GetDataMaster(int id, int accountId)
        {
            #region 變數宣告
            RunOneDataEnd<m__UserLoginLog> r = new RunOneDataEnd<m__UserLoginLog>();

            #endregion
            try
            {
                #region Main working
                TablePack<_UserLoginLog> dataWork = new TablePack<_UserLoginLog>(Connection) { LoginUserID = accountId };
                //dataWork.TableModule.KeyFieldModules[dataWork.TableModule.id.N].V = id; //設定KeyValue
                m__UserLoginLog md = dataWork.GetDataByKey<m__UserLoginLog>(); //取得Key該筆資料

                if (md == null) //如無資料
                    throw new LogicRoll("Log_Err_MustHaveData"); //此區一定有資料傳出，如無資料應檢查前端id值是否有誤

                r.SearchData = md;
                r.Result = true;
                dataWork.Dispose(); //釋放資料
                return r;
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
        }
        public override RunOneDataEnd<m__UserLoginLog> GetDataMaster(int[] id, int accountId)
        {
            throw new NotImplementedException();
        }
    }
    #endregion

    #endregion

    #region System Main Class

    #region Message
    public class q_Message : QueryBase
    {
        public String s_title { set; get; }
        public Boolean? s_isopen { set; get; }
    }
    public class m_Message : ModuleBase
    {
        public Int32 id { get; set; }
        ///<summary>
        ///Mapping:標題
        ///</summary>
        public String title { get; set; }
        ///<summary>
        ///Mapping:日期
        ///</summary>
        public DateTime set_date { get; set; }
        ///<summary>
        ///Mapping:內容
        ///</summary>
        public String context { get; set; }
        ///<summary>
        ///Mapping:開放
        ///</summary>
        public Boolean isopen { get; set; }
        ///<summary>
        ///Mapping:分類
        ///</summary>
        public String kind { get; set; }
    }
    public class a_Message : LogicBase<m_Message, q_Message, Message>
    {
        public override RunInsertEnd InsertMaster(m_Message md, int accountId)
        {
            #region Variable declare area
            RunInsertEnd r = new RunInsertEnd(); //宣告回傳物件
            #endregion
            try
            {
                #region Main working
                Connection.BeginTransaction(); //開始交易鎖定
                TablePack<Message> dataWork = new TablePack<Message>(Connection) { LoginUserID = this.logPlamInfo.UserId, LoginUnitID = this.logPlamInfo.UnitId };

                dataWork.NewRow(); //開始新橧作業 產生新的一行
                #region 指派值

                dataWork.SetDataRowValue(x => x.id, md.id);
                dataWork.SetDataRowValue(x => x.title, md.title);
                dataWork.SetDataRowValue(x => x.set_date, md.set_date);
                dataWork.SetDataRowValue(x => x.context, md.context);
                dataWork.SetDataRowValue(x => x.isopen, md.isopen);
                dataWork.SetDataRowValue(x => x.kind, md.kind);


                dataWork.UpdateFieldsInfo(UpdateFieldsInfoType.Insert); //進行更新時 需同時更新系統欄位 _新增人員，_新增日期
                dataWork.UpdateFieldsInfo(UpdateFieldsInfoType.Lang); //語系欄位設定
                //進行更新時 需同時更新系統欄位 _InsertUserID，_InsertDateTime
                #endregion
                dataWork.AddRow(); //加載至DataTable
                dataWork.UpdateDataAdapter(); //更新 DataBase Server

                Connection.EndCommit(); //交易確認
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, "OK");
                r.Rows = dataWork.AffetCount; //取得影響筆數
                r.InsertId = dataWork.InsertAutoFieldsID; //取得新增後自動新增欄位的值
                r.Result = true; //回傳本次執行結果為成功
                dataWork.Dispose(); //釋放資料

                return r;
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System; //系統失敗
                r.Message = PackErrMessage(ex); //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
        }
        public override RunUpdateEnd UpdateMaster(m_Message md, int accountId)
        {
            #region Variable declare area
            RunUpdateEnd r = new RunUpdateEnd();
            #endregion
            try
            {
                #region Main Working
                Connection.BeginTransaction();
                TablePack<Message> dataWork = new TablePack<Message>(Connection) { LoginUserID = this.logPlamInfo.UserId, LoginUnitID = this.logPlamInfo.UnitId };

                dataWork.TableModule.KeyFieldModules[dataWork.TableModule.id.N].V = md.id; //取得ID欄位的值
                m_Message md_Origin = dataWork.GetDataByKey<m_Message>(); //取得Key值的Data

                if (md_Origin == null) //如有資料
                    throw new LogicRoll("Log_Err_MustHaveData"); //此區一定有資料傳出，如無資料應檢查前端id值是否有誤

                dataWork.EditFirstRow(); //編輯第一筆資料，正常只會有一筆資料。
                #region 指派值

                dataWork.SetDataRowValue(x => x.title, md.title);
                dataWork.SetDataRowValue(x => x.set_date, md.set_date);
                dataWork.SetDataRowValue(x => x.context, md.context);
                dataWork.SetDataRowValue(x => x.isopen, md.isopen);
                dataWork.SetDataRowValue(x => x.kind, md.kind);

                dataWork.UpdateFieldsInfo(UpdateFieldsInfoType.Update); //指定進行更新時 需同時更新系統欄位 _修改人員，_修改日期
                #endregion
                md_Origin = null;
                dataWork.UpdateDataAdapter(); //進行變更至Database Server

                Connection.EndCommit();
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, "OK");
                r.Rows = dataWork.AffetCount; //取得影響筆數
                r.Result = true;
                dataWork.Dispose(); //釋放資料
                return r;
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System; //系統失敗
                r.Message = PackErrMessage(ex); //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
        }
        public override RunDeleteEnd DeleteMaster(Int32[] ids, int accountId)
        {
            //此功能主要搭配Grid介面刪除功能製作
            #region Variable declare area
            RunDeleteEnd r = new RunDeleteEnd(); //宣告刪除Result回物件
            #endregion
            try
            {
                #region Main working
                Connection.BeginTransaction();
                //1、要刪除的資料先選出來
                TablePack<Message> dataWork = new TablePack<Message>(Connection) { LoginUserID = this.logPlamInfo.UserId, LoginUnitID = this.logPlamInfo.UnitId }; //宣告泛型物件並連接Connection
                dataWork.SelectFields(x => x.id); //只Select 主Key欄位
                dataWork.WhereFields(x => x.id, ids); //代入陣列Id值

                m_Message[] md_Origin = dataWork.DataByAdapter<m_Message>(); //取得Key值的Data

                //2、進行全部刪除
                dataWork.DeleteAll(); //先刪除DataTable
                dataWork.UpdateDataAdapter(); //在更新至DataBase Server
                Connection.EndCommit();
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, "OK");
                r.Result = true;
                dataWork.Dispose(); //釋放資料
                md_Origin = null;
                return r;
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System; //系統失敗
                r.Message = PackErrMessage(ex); //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
        }
        public override RunQueryPackage<m_Message> SearchMaster(q_Message qr, int accountId)
        {
            #region 變數宣告
            RunQueryPackage<m_Message> r = new RunQueryPackage<m_Message>();
            #endregion
            try
            {
                #region Select Data 區段 By 條件
                #region 設定輸出至Grid欄位
                TablePack<Message> dataWork = new TablePack<Message>(Connection) { LoginUserID = this.logPlamInfo.UserId, LoginUnitID = this.logPlamInfo.UnitId };
                dataWork.SelectFields(x => new { x.id, x.title, x.set_date, x.isopen, x.kind });

                if (qr.MaxRecord > 0) dataWork.TopLimit = qr.MaxRecord;

                #endregion

                #region 設定Where條件
                if (qr.s_title != null)
                    dataWork.WhereFields(x => x.title, qr.s_title, WhereCompareType.Like);

                if (qr.s_isopen != null)
                    dataWork.WhereFields(x => x.isopen, qr.s_isopen);

                dataWork.WhereLang(); //使用語系條件
                #endregion

                #region 設定排序
                if (qr.sidx == null)
                {
                    dataWork.OrderByFields(x => x.set_date, OrderByType.DESC);
                    dataWork.OrderByFields(x => x.id, OrderByType.DESC);
                }
                else
                    dataWork.OrderByFields(x => x.id);
                #endregion

                #region 輸出物件陣列
                r.SearchData = dataWork.DataByAdapter<m_Message>();
                r.Result = true;
                return r;
                #endregion
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System; //系統失敗
                r.Message = PackErrMessage(ex); //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
        }

        public override RunOneDataEnd<m_Message> GetDataMaster(int id, int accountId)
        {
            #region 變數宣告
            RunOneDataEnd<m_Message> r = new RunOneDataEnd<m_Message>();

            #endregion
            try
            {
                #region Main working
                TablePack<Message> dataWork = new TablePack<Message>(Connection) { LoginUserID = this.logPlamInfo.UserId, LoginUnitID = this.logPlamInfo.UnitId };
                dataWork.TableModule.KeyFieldModules[dataWork.TableModule.id.N].V = id; //設定KeyValue
                m_Message md = dataWork.GetDataByKey<m_Message>(); //取得Key該筆資料

                if (md == null) //如無資料
                    throw new LogicRoll("Log_Err_MustHaveData"); //此區一定有資料傳出，如無資料應檢查前端id值是否有誤

                r.SearchData = md;
                r.Result = true;
                dataWork.Dispose(); //釋放資料
                return r;
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System; //系統失敗
                r.Message = PackErrMessage(ex); //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
        }
        public override RunOneDataEnd<m_Message> GetDataMaster(int[] id, int accountId)
        {
            throw new NotImplementedException();
        }
    }
    #endregion
    #region Member
    public class q_Member : QueryBase
    {
        public String s_email { set; get; }
        public String s_password { set; get; }
        public String s_name { set; get; }
    }
    public class m_Member : ModuleBase
    {
        public Int32 id { get; set; }
        ///<summary>
        ///Mapping:電子郵件
        ///</summary>
        public String email { get; set; }
        ///<summary>
        ///Mapping:密碼
        ///</summary>
        public String password { get; set; }
        ///<summary>
        ///Mapping:姓名
        ///</summary>
        public String name { get; set; }
        ///<summary>
        ///Mapping:聯絡電話
        ///</summary>
        public String tel { get; set; }
        ///<summary>
        ///Mapping:郵遞區號
        ///</summary>
        public String zip { get; set; }
        ///<summary>
        ///Mapping:地址
        ///</summary>
        public String address { get; set; }
        ///<summary>
        ///email 認證是否有效，預設為false。
        ///Mapping:認證
        ///</summary>
        public Boolean IsValidate { get; set; }
        ///<summary>
        ///Mapping:證號
        ///</summary>
        public String pid { get; set; }
        ///<summary>
        ///Mapping:性別
        ///</summary>
        public Boolean sex { get; set; }
        ///<summary>
        ///Mapping:生日
        ///</summary>
        public DateTime birthday { get; set; }
        ///<summary>
        ///Mapping:狀態
        ///</summary>
        public String member_state { get; set; }
        ///<summary>
        ///Mapping:等級
        ///</summary>
        public Byte member_level { get; set; }
        ///<summary>
        ///Mapping:績點
        ///</summary>
        public Int32 point { get; set; }
        ///<summary>
        ///Mapping:註冊時間
        ///</summary>
        public DateTime reg_time { get; set; }
        ///<summary>
        ///Mapping:登錄時間
        ///</summary>
        public DateTime login_time { get; set; }
        ///<summary>
        ///Mapping:團購身分
        ///</summary>
        public Boolean isgroup { get; set; }
        ///<summary>
        ///Mapping:團購單位
        ///</summary>
        public String groupunit { get; set; }
    }
    public class a_Member : LogicBase<m_Member, q_Member, Member>
    {
        public override RunInsertEnd InsertMaster(m_Member md, int accountId)
        {
            #region Variable declare area
            RunInsertEnd r = new RunInsertEnd(); //宣告回傳物件
            #endregion
            try
            {
                #region Main working
                Connection.BeginTransaction(); //開始交易鎖定
                TablePack<Member> dataWork = new TablePack<Member>(Connection) { LoginUserID = this.logPlamInfo.UserId, LoginUnitID = this.logPlamInfo.UnitId };

                dataWork.NewRow(); //開始新橧作業 產生新的一行
                #region 指派值

                dataWork.SetDataRowValue(x => x.id, md.id);
                dataWork.SetDataRowValue(x => x.email, md.email);
                dataWork.SetDataRowValue(x => x.password, md.password);
                dataWork.SetDataRowValue(x => x.name, md.name);
                dataWork.SetDataRowValue(x => x.tel, md.tel);
                dataWork.SetDataRowValue(x => x.zip, md.zip);
                dataWork.SetDataRowValue(x => x.address, md.address);
                dataWork.SetDataRowValue(x => x.IsValidate, false);
                //dataWork.SetDataRowValue(x => x.pid, md.pid);
                dataWork.SetDataRowValue(x => x.sex, md.sex);
                //dataWork.SetDataRowValue(x => x.birthday, md.birthday);
                dataWork.SetDataRowValue(x => x.member_state, md.member_state);
                //dataWork.SetDataRowValue(x => x.member_level, md.member_level);
                //dataWork.SetDataRowValue(x => x.point, md.point);
                dataWork.SetDataRowValue(x => x.reg_time, md.reg_time);
                //dataWork.SetDataRowValue(x => x.login_time, md.login_time);
                //dataWork.SetDataRowValue(x => x.isgroup, md.isgroup);
                //dataWork.SetDataRowValue(x => x.groupunit, md.groupunit);


                dataWork.UpdateFieldsInfo(UpdateFieldsInfoType.Insert); //進行更新時 需同時更新系統欄位 _新增人員，_新增日期
                dataWork.UpdateFieldsInfo(UpdateFieldsInfoType.Lang); //語系欄位設定
                //進行更新時 需同時更新系統欄位 _InsertUserID，_InsertDateTime
                #endregion
                dataWork.AddRow(); //加載至DataTable
                dataWork.UpdateDataAdapter(); //更新 DataBase Server

                Connection.EndCommit(); //交易確認

                r.Rows = dataWork.AffetCount; //取得影響筆數
                r.InsertId = dataWork.InsertAutoFieldsID; //取得新增後自動新增欄位的值
                r.Result = true; //回傳本次執行結果為成功
                dataWork.Dispose(); //釋放資料

                return r;
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System; //系統失敗
                r.Message = PackErrMessage(ex); //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
        }
        public override RunUpdateEnd UpdateMaster(m_Member md, int accountId)
        {
            #region Variable declare area
            RunUpdateEnd r = new RunUpdateEnd();
            #endregion
            try
            {
                #region Main Working
                Connection.BeginTransaction();
                TablePack<Member> dataWork = new TablePack<Member>(Connection) { LoginUserID = this.logPlamInfo.UserId, LoginUnitID = this.logPlamInfo.UnitId };

                dataWork.TableModule.KeyFieldModules[dataWork.TableModule.id.N].V = md.id; //取得ID欄位的值
                m_Member md_Origin = dataWork.GetDataByKey<m_Member>(); //取得Key值的Data

                if (md_Origin == null) //如有資料
                    throw new LogicRoll("Log_Err_MustHaveData"); //此區一定有資料傳出，如無資料應檢查前端id值是否有誤

                dataWork.EditFirstRow(); //編輯第一筆資料，正常只會有一筆資料。
                #region 指派值

                //dataWork.SetDataRowValue(x => x.id, md.id);
                //dataWork.SetDataRowValue(x => x.email, md.email);
                //dataWork.SetDataRowValue(x => x.password, md.password);
                dataWork.SetDataRowValue(x => x.name, md.name);
                dataWork.SetDataRowValue(x => x.tel, md.tel);
                //dataWork.SetDataRowValue(x => x.zip, md.zip);
                //dataWork.SetDataRowValue(x => x.address, md.address);
                //dataWork.SetDataRowValue(x => x.IsValidate, md.IsValidate);
                //dataWork.SetDataRowValue(x => x.pid, md.pid);
                //dataWork.SetDataRowValue(x => x.sex, md.sex);
                //dataWork.SetDataRowValue(x => x.birthday, md.birthday);
                dataWork.SetDataRowValue(x => x.member_state, md.member_state);
                //dataWork.SetDataRowValue(x => x.member_level, md.member_level);
                //dataWork.SetDataRowValue(x => x.point, md.point);
                //dataWork.SetDataRowValue(x => x.reg_time, md.reg_time);
                //dataWork.SetDataRowValue(x => x.login_time, md.login_time);
                //dataWork.SetDataRowValue(x => x.isgroup, md.isgroup);
                //dataWork.SetDataRowValue(x => x.groupunit, md.groupunit);


                dataWork.UpdateFieldsInfo(UpdateFieldsInfoType.Update); //指定進行更新時 需同時更新系統欄位 _修改人員，_修改日期
                #endregion
                md_Origin = null;
                dataWork.UpdateDataAdapter(); //進行變更至Database Server

                Connection.EndCommit();

                r.Rows = dataWork.AffetCount; //取得影響筆數
                r.Result = true;
                dataWork.Dispose(); //釋放資料
                return r;
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System; //系統失敗
                r.Message = PackErrMessage(ex); //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
        }
        public override RunDeleteEnd DeleteMaster(Int32[] ids, int accountId)
        {
            //此功能主要搭配Grid介面刪除功能製作
            #region Variable declare area
            RunDeleteEnd r = new RunDeleteEnd(); //宣告刪除Result回物件
            #endregion
            try
            {
                #region Main working
                Connection.BeginTransaction();
                //1、要刪除的資料先選出來
                TablePack<Member> dataWork = new TablePack<Member>(Connection) { LoginUserID = this.logPlamInfo.UserId, LoginUnitID = this.logPlamInfo.UnitId }; //宣告泛型物件並連接Connection
                dataWork.SelectFields(x => x.id); //只Select 主Key欄位
                dataWork.WhereFields(x => x.id, ids); //代入陣列Id值

                m_Member[] md_Origin = dataWork.DataByAdapter<m_Member>(); //取得Key值的Data

                //2、進行全部刪除
                dataWork.DeleteAll(); //先刪除DataTable
                dataWork.UpdateDataAdapter(); //在更新至DataBase Server
                Connection.EndCommit();
                r.Result = true;
                dataWork.Dispose(); //釋放資料
                md_Origin = null;
                return r;
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System; //系統失敗
                r.Message = PackErrMessage(ex); //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
        }
        public override RunQueryPackage<m_Member> SearchMaster(q_Member qr, int accountId)
        {
            #region 變數宣告
            RunQueryPackage<m_Member> r = new RunQueryPackage<m_Member>();
            #endregion
            try
            {
                #region Select Data 區段 By 條件
                #region 設定輸出至Grid欄位
                TablePack<Member> dataWork = new TablePack<Member>(Connection) { LoginUserID = this.logPlamInfo.UserId, LoginUnitID = this.logPlamInfo.UnitId };
                dataWork.SelectFields(x => new { x.id,x.name, x.email,x.member_state });
                #endregion

                #region 設定Where條件
                if (qr.s_email != null)
                    dataWork.WhereFields(x => x.email, qr.s_email);

                if (qr.s_password != null)
                    dataWork.WhereFields(x => x.password, qr.s_password);

                //dataWork.WhereLang(); //使用語系條件
                #endregion

                #region 設定排序
                if (qr.sidx == null)
                    dataWork.OrderByFields(x => x.id, OrderByType.DESC); //預設排序
                else
                    dataWork.OrderByFields(x => x.id, OrderByType.ASC);
                #endregion

                #region 輸出物件陣列
                r.SearchData = dataWork.DataByAdapter<m_Member>();
                r.Result = true;
                return r;
                #endregion
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System; //系統失敗
                r.Message = PackErrMessage(ex); //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
        }
        public override RunOneDataEnd<m_Member> GetDataMaster(int id, int accountId)
        {
            #region 變數宣告
            RunOneDataEnd<m_Member> r = new RunOneDataEnd<m_Member>();
            #endregion
            try
            {
                #region Main working
                TablePack<Member> dataWork = new TablePack<Member>(Connection) { LoginUserID = this.logPlamInfo.UserId, LoginUnitID = this.logPlamInfo.UnitId };
                dataWork.TableModule.KeyFieldModules[dataWork.TableModule.id.N].V = id; //設定KeyValue
                m_Member md = dataWork.GetDataByKey<m_Member>(); //取得Key該筆資料

                if (md == null) //如無資料
                    throw new LogicRoll("Log_Err_MustHaveData"); //此區一定有資料傳出，如無資料應檢查前端id值是否有誤

                r.SearchData = md;
                r.Result = true;
                dataWork.Dispose(); //釋放資料
                return r;
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System; //系統失敗
                r.Message = PackErrMessage(ex); //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
        }
        public override RunOneDataEnd<m_Member> GetDataMaster(int[] id, int accountId)
        {
            throw new NotImplementedException();
        }

        public RunOneDataEnd<m_Member> LoginMember(String email, String password)
        {
            #region 變數宣告
            RunOneDataEnd<m_Member> r = new RunOneDataEnd<m_Member>();
            #endregion

            var r1 = SearchMaster(new q_Member() { s_email = email, s_password = password }, 0);

            if (r1.SearchData.Count() > 0)
            {
                r.SearchData = GetDataMaster(r1.SearchData.FirstOrDefault().id, 0).SearchData;
                r.Result = true;
            }
            else
            {
                r.Result = false;
            }

            return r;
        }
        public Boolean Exist_MemberEmail(String email)
        {
            #region 變數宣告
            RunOneDataEnd<m_Member> r = new RunOneDataEnd<m_Member>();
            #endregion

            var r1 = SearchMaster(new q_Member() { s_email = email }, 0);

            if (r1.SearchData.Count() > 0)
                return true;
            else
                return false;
        }

        public RunUpdateEnd ResetPassword(int id, int accountId)
        {
            #region Variable declare area
            RunUpdateEnd r = new RunUpdateEnd();
            #endregion
            try
            {
                #region Main Working
                Connection.BeginTransaction();
                TablePack<Member> dataWork = new TablePack<Member>(Connection) { LoginUserID = this.logPlamInfo.UserId, LoginUnitID = this.logPlamInfo.UnitId };

                dataWork.TableModule.KeyFieldModules[dataWork.TableModule.id.N].V = id; //取得ID欄位的值
                m_Member md_Origin = dataWork.GetDataByKey<m_Member>(); //取得Key值的Data

                if (md_Origin == null) //如有資料
                    throw new LogicRoll("Log_Err_MustHaveData"); //此區一定有資料傳出，如無資料應檢查前端id值是否有誤

                dataWork.EditFirstRow(); //編輯第一筆資料，正常只會有一筆資料。
                #region 指派值

                String NewPassword = (new Random()).Next(1000, 9999).ToString();
                dataWork.SetDataRowValue(x => x.password, NewPassword);
                dataWork.UpdateFieldsInfo(UpdateFieldsInfoType.Update); //指定進行更新時 需同時更新系統欄位 _修改人員，_修改日期
                #endregion
                md_Origin = null;
                dataWork.UpdateDataAdapter(); //進行變更至Database Server

                Connection.EndCommit();

                r.Rows = dataWork.AffetCount; //取得影響筆數
                r.Result = true;
                dataWork.Dispose(); //釋放資料
                return r;
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System; //系統失敗
                r.Message = PackErrMessage(ex); //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
        }
        public RunUpdateEnd ChangePassword(int id, String newpwd, int accountId)
        {
            #region Variable declare area
            RunUpdateEnd r = new RunUpdateEnd();
            #endregion
            try
            {
                #region Main Working
                Connection.BeginTransaction();
                TablePack<Member> dataWork = new TablePack<Member>(Connection) { LoginUserID = this.logPlamInfo.UserId, LoginUnitID = this.logPlamInfo.UnitId };

                dataWork.TableModule.KeyFieldModules[dataWork.TableModule.id.N].V = id; //取得ID欄位的值
                m_Member md_Origin = dataWork.GetDataByKey<m_Member>(); //取得Key值的Data

                if (md_Origin == null) //如有資料
                    throw new LogicRoll("Log_Err_MustHaveData"); //此區一定有資料傳出，如無資料應檢查前端id值是否有誤

                dataWork.EditFirstRow(); //編輯第一筆資料，正常只會有一筆資料。
                #region 指派值

                dataWork.SetDataRowValue(x => x.password, newpwd);
                dataWork.UpdateFieldsInfo(UpdateFieldsInfoType.Update); //指定進行更新時 需同時更新系統欄位 _修改人員，_修改日期
                #endregion
                md_Origin = null;
                dataWork.UpdateDataAdapter(); //進行變更至Database Server

                Connection.EndCommit();

                r.Rows = dataWork.AffetCount; //取得影響筆數
                r.Result = true;
                dataWork.Dispose(); //釋放資料
                return r;
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System; //系統失敗
                r.Message = PackErrMessage(ex); //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
        }
    }
    #endregion
    #region PageContext
    public class q_PageContext : QueryBase
    {
        public int? s_kid { set; get; }
        public String s_pagename { set; get; }
    }
    public class m_PageContext : ModuleBase
    {
        public Int32 id { get; set; }
        public Int32 kid { get; set; }
        ///<summary>
        ///Mapping:網頁名稱
        ///</summary>
        public String pagename { get; set; }
        ///<summary>
        ///Mapping:排序
        ///</summary>
        public Int32 sort { get; set; }
        public String pagecontext { get; set; }
        public Boolean isopen { get; set; }
        public DateTime setdate { get; set; }
    }
    public class a_PageContext : LogicBase<m_PageContext, q_PageContext, PageContext>
    {

        public override RunInsertEnd InsertMaster(m_PageContext md, int accountId)
        {
            #region Variable declare area
            RunInsertEnd r = new RunInsertEnd(); //宣告回傳物件
            #endregion
            try
            {
                #region Main working
                Connection.BeginTransaction(); //開始交易鎖定
                TablePack<PageContext> dataWork = new TablePack<PageContext>(Connection) { LoginUserID = this.logPlamInfo.UserId, LoginUnitID = this.logPlamInfo.UnitId };

                dataWork.NewRow(); //開始新橧作業 產生新的一行
                #region 指派值

                dataWork.SetDataRowValue(x => x.id, md.id);
                dataWork.SetDataRowValue(x => x.kid, md.kid);
                dataWork.SetDataRowValue(x => x.pagename, md.pagename);
                dataWork.SetDataRowValue(x => x.sort, md.sort);
                dataWork.SetDataRowValue(x => x.pagecontext, md.pagecontext);


                dataWork.UpdateFieldsInfo(UpdateFieldsInfoType.Insert); //進行更新時 需同時更新系統欄位 _新增人員，_新增日期
                dataWork.UpdateFieldsInfo(UpdateFieldsInfoType.Lang); //語系欄位設定
                //進行更新時 需同時更新系統欄位 _InsertUserID，_InsertDateTime
                #endregion
                dataWork.AddRow(); //加載至DataTable
                dataWork.UpdateDataAdapter(); //更新 DataBase Server

                Connection.EndCommit(); //交易確認

                r.Rows = dataWork.AffetCount; //取得影響筆數
                r.InsertId = dataWork.InsertAutoFieldsID; //取得新增後自動新增欄位的值
                r.Result = true; //回傳本次執行結果為成功
                dataWork.Dispose(); //釋放資料

                return r;
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System; //系統失敗
                r.Message = PackErrMessage(ex); //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
        }
        public override RunUpdateEnd UpdateMaster(m_PageContext md, int accountId)
        {
            #region Variable declare area
            RunUpdateEnd r = new RunUpdateEnd();
            #endregion
            try
            {
                #region Main Working
                Connection.BeginTransaction();
                TablePack<PageContext> dataWork = new TablePack<PageContext>(Connection) { LoginUserID = this.logPlamInfo.UserId, LoginUnitID = this.logPlamInfo.UnitId };

                dataWork.TableModule.KeyFieldModules[dataWork.TableModule.id.N].V = md.id; //取得ID欄位的值
                m_PageContext md_Origin = dataWork.GetDataByKey<m_PageContext>(); //取得Key值的Data

                if (md_Origin == null) //如有資料
                    throw new LogicRoll("Log_Err_MustHaveData"); //此區一定有資料傳出，如無資料應檢查前端id值是否有誤

                dataWork.EditFirstRow(); //編輯第一筆資料，正常只會有一筆資料。
                #region 指派值

                dataWork.SetDataRowValue(x => x.pagename, md.pagename);
                dataWork.SetDataRowValue(x => x.sort, md.sort);
                dataWork.SetDataRowValue(x => x.pagecontext, md.pagecontext);

                dataWork.UpdateFieldsInfo(UpdateFieldsInfoType.Update); //指定進行更新時 需同時更新系統欄位 _修改人員，_修改日期
                #endregion
                md_Origin = null;
                dataWork.UpdateDataAdapter(); //進行變更至Database Server

                Connection.EndCommit();

                r.Rows = dataWork.AffetCount; //取得影響筆數
                r.Result = true;
                dataWork.Dispose(); //釋放資料
                return r;
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System; //系統失敗
                r.Message = PackErrMessage(ex); //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
        }

        public override RunDeleteEnd DeleteMaster(Int32[] ids, int accountId)
        {
            //此功能主要搭配Grid介面刪除功能製作
            #region Variable declare area
            RunDeleteEnd r = new RunDeleteEnd(); //宣告刪除Result回物件
            #endregion
            try
            {
                #region Main working
                Connection.BeginTransaction();
                //1、要刪除的資料先選出來
                TablePack<PageContext> dataWork = new TablePack<PageContext>(Connection) { LoginUserID = this.logPlamInfo.UserId, LoginUnitID = this.logPlamInfo.UnitId }; //宣告泛型物件並連接Connection
                dataWork.SelectFields(x => x.id); //只Select 主Key欄位
                dataWork.WhereFields(x => x.id, ids); //代入陣列Id值

                m_PageContext[] md_Origin = dataWork.DataByAdapter<m_PageContext>(); //取得Key值的Data

                //2、進行全部刪除
                dataWork.DeleteAll(); //先刪除DataTable
                dataWork.UpdateDataAdapter(); //在更新至DataBase Server
                Connection.EndCommit();
                r.Result = true;
                dataWork.Dispose(); //釋放資料
                md_Origin = null;
                return r;
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System; //系統失敗
                r.Message = PackErrMessage(ex); //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
        }

        public override RunQueryPackage<m_PageContext> SearchMaster(q_PageContext qr, int accountId)
        {
            #region 變數宣告
            RunQueryPackage<m_PageContext> r = new RunQueryPackage<m_PageContext>();
            #endregion
            try
            {
                #region Select Data 區段 By 條件
                #region 設定輸出至Grid欄位
                TablePack<PageContext> dataWork = new TablePack<PageContext>(Connection) { LoginUserID = this.logPlamInfo.UserId, LoginUnitID = this.logPlamInfo.UnitId };
                dataWork.SelectFields(x => new { x.id, x.kid, x.pagename, x.setdate, x.isopen, x.sort });
                #endregion

                #region 設定Where條件
                if (qr.s_kid != null)
                    dataWork.WhereFields(x => x.kid, qr.s_kid);

                if (qr.s_pagename != null)
                    dataWork.WhereFields(x => x.pagename, qr.s_pagename, WhereCompareType.Like);

                dataWork.WhereLang(); //使用語系條件
                #endregion

                #region 設定排序
                if (qr.sidx == null)
                {
                    dataWork.OrderByFields(x => x.kid); //預設排序
                    dataWork.OrderByFields(x => x.sort); //預設排序
                }
                else
                    dataWork.OrderByFields(x => x.id, OrderByType.ASC);
                #endregion

                #region 輸出物件陣列
                r.SearchData = dataWork.DataByAdapter<m_PageContext>();
                r.Result = true;
                return r;
                #endregion
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System; //系統失敗
                r.Message = PackErrMessage(ex); //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
        }

        public override RunOneDataEnd<m_PageContext> GetDataMaster(int id, int accountId)
        {
            #region 變數宣告
            RunOneDataEnd<m_PageContext> r = new RunOneDataEnd<m_PageContext>();

            #endregion
            try
            {
                #region Main working
                TablePack<PageContext> dataWork = new TablePack<PageContext>(Connection) { LoginUserID = this.logPlamInfo.UserId, LoginUnitID = this.logPlamInfo.UnitId };
                dataWork.TableModule.KeyFieldModules[dataWork.TableModule.id.N].V = id; //設定KeyValue
                m_PageContext md = dataWork.GetDataByKey<m_PageContext>(); //取得Key該筆資料

                if (md == null) //如無資料
                    throw new LogicRoll("Log_Err_MustHaveData"); //此區一定有資料傳出，如無資料應檢查前端id值是否有誤

                r.SearchData = md;
                r.Result = true;
                dataWork.Dispose(); //釋放資料
                return r;
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System; //系統失敗
                r.Message = PackErrMessage(ex); //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
        }
        public override RunOneDataEnd<m_PageContext> GetDataMaster(int[] id, int accountId)
        {
            throw new NotImplementedException();
        }
    }
    #endregion
    #region Product
    public class q_Product : QueryBase
    {
        public String s_product_name { set; get; }
        public String s_product_serial { set; get; }
        public Int32? s_product_category_l1 { get; set; }
        public Int32? s_product_category_l2 { get; set; }
        public Boolean? s_is_open { get; set; }
    }
    public class m_Product : ModuleBase
    {
        public Int32 id { get; set; }
        ///<summary>
        ///Mapping:產品名稱
        ///</summary>
        public String product_name { get; set; }
        public int price { get; set; }
        public String product_intro { get; set; }
        ///<summary>
        ///Mapping:型號
        ///</summary>
        public String product_serial { get; set; }
        ///<summary>
        ///Mapping:分類M
        ///</summary>
        public Int32 product_category_l1_id { get; set; }
        public String category_l1_name { get; set; }
        ///<summary>
        ///Mapping:分類S
        ///</summary>
        public Int32 product_category_l2_id { get; set; }
        public String product_category_l2_name { get; set; }
        ///<summary>
        ///Mapping:說明
        ///</summary>
        public String edit_memo { get; set; }
        ///<summary>
        ///Mapping:關閉
        ///</summary>
        public Boolean is_open { get; set; }
        public Int32 sort { get; set; }

        public String img_list_src { get; set; }
    }
    public class a_Product : LogicBase<m_Product, q_Product, Product>
    {
        public override RunInsertEnd InsertMaster(m_Product md, int accountId)
        {
            #region Variable declare area
            RunInsertEnd r = new RunInsertEnd(); //宣告回傳物件
            #endregion
            try
            {
                #region Main working
                Connection.BeginTransaction(); //開始交易鎖定
                TablePack<Product> dataWork = new TablePack<Product>(Connection) { LoginUserID = this.logPlamInfo.UserId, LoginUnitID = this.logPlamInfo.UnitId };

                dataWork.NewRow(); //開始新橧作業 產生新的一行
                #region 指派值

                dataWork.SetDataRowValue(x => x.id, md.id);
                dataWork.SetDataRowValue(x => x.product_name, md.product_name);
                dataWork.SetDataRowValue(x => x.product_serial, md.product_serial);
                dataWork.SetDataRowValue(x => x.edit_memo, md.edit_memo);
                dataWork.SetDataRowValue(x => x.product_intro, md.product_intro);
                dataWork.SetDataRowValue(x => x.product_category_l1_id, md.product_category_l1_id);
                dataWork.SetDataRowValue(x => x.product_category_l2_id, md.product_category_l2_id);
                dataWork.SetDataRowValue(x => x.edit_memo, md.edit_memo);
                dataWork.SetDataRowValue(x => x.is_open, md.is_open);
                dataWork.SetDataRowValue(x => x.sort, md.sort);
                dataWork.SetDataRowValue(x => x.price, md.price);

                dataWork.UpdateFieldsInfo(UpdateFieldsInfoType.Insert); //進行更新時 需同時更新系統欄位 _新增人員，_新增日期
                dataWork.UpdateFieldsInfo(UpdateFieldsInfoType.Lang); //語系欄位設定
                //進行更新時 需同時更新系統欄位 _InsertUserID，_InsertDateTime
                #endregion
                dataWork.AddRow(); //加載至DataTable
                dataWork.UpdateDataAdapter(); //更新 DataBase Server

                Connection.EndCommit(); //交易確認

                r.Rows = dataWork.AffetCount; //取得影響筆數
                r.InsertId = dataWork.InsertAutoFieldsID; //取得新增後自動新增欄位的值
                r.Result = true; //回傳本次執行結果為成功
                dataWork.Dispose(); //釋放資料

                return r;
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System; //系統失敗
                r.Message = PackErrMessage(ex); //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
        }
        public override RunUpdateEnd UpdateMaster(m_Product md, int accountId)
        {
            #region Variable declare area
            RunUpdateEnd r = new RunUpdateEnd();
            #endregion
            try
            {
                #region Main Working
                Connection.BeginTransaction();
                TablePack<Product> dataWork = new TablePack<Product>(Connection) { LoginUserID = this.logPlamInfo.UserId, LoginUnitID = this.logPlamInfo.UnitId };

                dataWork.TableModule.KeyFieldModules[dataWork.TableModule.id.N].V = md.id; //取得ID欄位的值
                m_Product md_Origin = dataWork.GetDataByKey<m_Product>(); //取得Key值的Data

                if (md_Origin == null) //如有資料
                    throw new LogicRoll("Log_Err_MustHaveData"); //此區一定有資料傳出，如無資料應檢查前端id值是否有誤

                dataWork.EditFirstRow(); //編輯第一筆資料，正常只會有一筆資料。
                #region 指派值

                dataWork.SetDataRowValue(x => x.product_name, md.product_name);
                dataWork.SetDataRowValue(x => x.product_intro, md.product_intro);
                dataWork.SetDataRowValue(x => x.product_serial, md.product_serial);
                dataWork.SetDataRowValue(x => x.edit_memo, md.edit_memo);
                dataWork.SetDataRowValue(x => x.product_category_l1_id, md.product_category_l1_id);
                dataWork.SetDataRowValue(x => x.product_category_l2_id, md.product_category_l2_id);
                dataWork.SetDataRowValue(x => x.edit_memo, md.edit_memo);
                dataWork.SetDataRowValue(x => x.is_open, md.is_open);
                dataWork.SetDataRowValue(x => x.sort, md.sort);
                dataWork.SetDataRowValue(x => x.price, md.price);

                dataWork.UpdateFieldsInfo(UpdateFieldsInfoType.Update);
                #endregion
                md_Origin = null;
                dataWork.UpdateDataAdapter(); //進行變更至Database Server

                Connection.EndCommit();

                r.Rows = dataWork.AffetCount; //取得影響筆數
                r.Result = true;
                dataWork.Dispose(); //釋放資料
                return r;
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System; //系統失敗
                r.Message = PackErrMessage(ex); //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
        }
        public override RunDeleteEnd DeleteMaster(Int32[] ids, int accountId)
        {
            //此功能主要搭配Grid介面刪除功能製作
            #region Variable declare area
            RunDeleteEnd r = new RunDeleteEnd(); //宣告刪除Result回物件
            #endregion
            try
            {
                #region Main working
                Connection.BeginTransaction();
                //1、要刪除的資料先選出來
                TablePack<Product> dataWork = new TablePack<Product>(Connection) { LoginUserID = this.logPlamInfo.UserId, LoginUnitID = this.logPlamInfo.UnitId }; //宣告泛型物件並連接Connection
                dataWork.SelectFields(x => x.id); //只Select 主Key欄位
                dataWork.WhereFields(x => x.id, ids); //代入陣列Id值

                m_Product[] md_Origin = dataWork.DataByAdapter<m_Product>(); //取得Key值的Data

                //2、進行全部刪除
                dataWork.DeleteAll(); //先刪除DataTable
                dataWork.UpdateDataAdapter(); //在更新至DataBase Server
                Connection.EndCommit();
                r.Result = true;
                dataWork.Dispose(); //釋放資料
                md_Origin = null;
                return r;
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System; //系統失敗
                r.Message = PackErrMessage(ex); //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
        }
        public override RunQueryPackage<m_Product> SearchMaster(q_Product qr, int accountId)
        {
            #region 變數宣告
            RunQueryPackage<m_Product> r = new RunQueryPackage<m_Product>();
            #endregion
            try
            {
                #region Select Data 區段 By 條件
                #region 設定輸出至Grid欄位
                Table3Join<Product, Product_Category_L1, Product_Category_L2> dataWork = new Table3Join<Product, Product_Category_L1, Product_Category_L2>(Connection);
                dataWork.SelectFields((x, y, z) => new { x.id, x.product_serial, x.product_name, x.product_intro, category_l1_name = y.category_l1_name, category_l2_name = z.category_l2_name, x.is_open });

                dataWork.JoinField_1_2(x => x.product_category_l1_id, x => x.id, JoinType.Inner);
                dataWork.JoinField_1_3(x => x.product_category_l2_id, x => x.id, JoinType.Inner);
                #endregion

                #region 設定Where條件
                if (qr.s_product_name != null)
                    dataWork.WhereFields((x, y, z) => x.product_name, qr.s_product_name, WhereCompareType.Like);

                if (qr.s_product_serial != null)
                    dataWork.WhereFields((x, y, z) => x.product_serial, qr.s_product_serial, WhereCompareType.LikeRight);

                if (qr.s_product_category_l1 != null)
                    dataWork.WhereFields((x, y, z) => x.product_category_l1_id, qr.s_product_category_l1);

                if (qr.s_product_category_l2 != null)
                    dataWork.WhereFields((x, y, z) => x.product_category_l2_id, qr.s_product_category_l2);

                if (qr.s_is_open != null)
                    dataWork.WhereFields((x, y, z) => x.is_open, qr.s_is_open);

                //dataWork.WhereLang(); //使用語系條件
                #endregion

                #region 設定排序
                if (qr.sidx == null)
                    dataWork.OrderByFields((x, y, z) => x.id, OrderByType.DESC); //預設排序
                else if (qr.sidx == "sort")
                    dataWork.OrderByFields((x, y, z) => x.sort, OrderByType.DESC);
                #endregion

                #region 輸出物件陣列
                r.SearchData = dataWork.DataByAdapter<m_Product>();
                r.Result = true;
                return r;
                #endregion
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System; //系統失敗
                r.Message = PackErrMessage(ex); //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
        }
        public override RunOneDataEnd<m_Product> GetDataMaster(int id, int accountId)
        {
            #region 變數宣告
            RunOneDataEnd<m_Product> r = new RunOneDataEnd<m_Product>();

            #endregion
            try
            {
                #region Main working
                Table1Join<Product> dataWork = new Table1Join<Product>(Connection) { };
                dataWork.JoinField(x => x.id, JoinType.Inner);
                dataWork.WhereFields(x => x.id, id);

                m_Product md = dataWork.DataByAdapter<m_Product>().FirstOrDefault(); //取得Key該筆資料

                if (md == null) //如無資料
                    throw new LogicRoll("Log_Err_MustHaveData"); //此區一定有資料傳出，如無資料應檢查前端id值是否有誤

                r.SearchData = md;
                r.Result = true;
                
                return r;
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System; //系統失敗
                r.Message = PackErrMessage(ex); //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
        }
        public override RunOneDataEnd<m_Product> GetDataMaster(int[] id, int accountId)
        {
            throw new NotImplementedException();
        }
    }
    #endregion
    #region Product_Category_L1
    public class q_Product_Category_L1 : QueryBase
    {
        public String s_title { set; get; }
    }

    public class m_Product_Category
    {
        public m_Product_Category_L1[] Product_Category { get; set; }
        public int Category_L1 { get; set; }
        public int Category_L2 { get; set; }
    }
    public class m_Product_Category_L1 : ModuleBase
    {
        public Int32 id { get; set; }
        public String category_l1_name { get; set; }
        public Int32 sort { get; set; }
        public m_Product_Category_L2[] Category_L2 { get; set; }
    }
    public class a_Product_Category_L1 : LogicBase<m_Product_Category_L1, q_Product_Category_L1, Product_Category_L1>
    {
        public override RunInsertEnd InsertMaster(m_Product_Category_L1 md, int accountId)
        {
            #region Variable declare area
            RunInsertEnd r = new RunInsertEnd(); //宣告回傳物件
            #endregion
            try
            {
                #region Main working
                Connection.BeginTransaction(); //開始交易鎖定
                TablePack<Product_Category_L1> dataWork = new TablePack<Product_Category_L1>(Connection) { LoginUserID = this.logPlamInfo.UserId, LoginUnitID = this.logPlamInfo.UnitId };

                dataWork.NewRow(); //開始新橧作業 產生新的一行
                #region 指派值

                dataWork.SetDataRowValue(x => x.id, md.id);
                dataWork.SetDataRowValue(x => x.category_l1_name, md.category_l1_name);
                dataWork.SetDataRowValue(x => x.sort, md.sort);
                dataWork.SetDataRowValue(x => x._隱藏, false);

                dataWork.UpdateFieldsInfo(UpdateFieldsInfoType.Insert); //進行更新時 需同時更新系統欄位 _新增人員，_新增日期
                dataWork.UpdateFieldsInfo(UpdateFieldsInfoType.Lang); //語系欄位設定
                //進行更新時 需同時更新系統欄位 _InsertUserID，_InsertDateTime
                #endregion
                dataWork.AddRow(); //加載至DataTable
                dataWork.UpdateDataAdapter(); //更新 DataBase Server

                Connection.EndCommit(); //交易確認

                r.Rows = dataWork.AffetCount; //取得影響筆數
                r.InsertId = dataWork.InsertAutoFieldsID; //取得新增後自動新增欄位的值
                r.Result = true; //回傳本次執行結果為成功
                dataWork.Dispose(); //釋放資料

                return r;
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System; //系統失敗
                r.Message = PackErrMessage(ex); //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
        }
        public override RunUpdateEnd UpdateMaster(m_Product_Category_L1 md, int accountId)
        {
            #region Variable declare area
            RunUpdateEnd r = new RunUpdateEnd();
            #endregion
            try
            {
                #region Main Working
                Connection.BeginTransaction();
                TablePack<Product_Category_L1> dataWork = new TablePack<Product_Category_L1>(Connection) { LoginUserID = this.logPlamInfo.UserId, LoginUnitID = this.logPlamInfo.UnitId };

                dataWork.TableModule.KeyFieldModules[dataWork.TableModule.id.N].V = md.id; //取得ID欄位的值
                m_Product_Category_L1 md_Origin = dataWork.GetDataByKey<m_Product_Category_L1>(); //取得Key值的Data

                if (md_Origin == null) //如有資料
                    throw new LogicRoll("Log_Err_MustHaveData"); //此區一定有資料傳出，如無資料應檢查前端id值是否有誤

                dataWork.EditFirstRow(); //編輯第一筆資料，正常只會有一筆資料。
                #region 指派值

                dataWork.SetDataRowValue(x => x.id, md.id);
                dataWork.SetDataRowValue(x => x.category_l1_name, md.category_l1_name);
                dataWork.SetDataRowValue(x => x.sort, md.sort);


                dataWork.UpdateFieldsInfo(UpdateFieldsInfoType.Update); //指定進行更新時 需同時更新系統欄位 _修改人員，_修改日期
                #endregion
                md_Origin = null;
                dataWork.UpdateDataAdapter(); //進行變更至Database Server

                Connection.EndCommit();

                r.Rows = dataWork.AffetCount; //取得影響筆數
                r.Result = true;
                dataWork.Dispose(); //釋放資料
                return r;
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System; //系統失敗
                r.Message = PackErrMessage(ex); //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
        }
        public override RunDeleteEnd DeleteMaster(Int32[] ids, int accountId)
        {
            //此功能主要搭配Grid介面刪除功能製作
            #region Variable declare area
            RunDeleteEnd r = new RunDeleteEnd(); //宣告刪除Result回物件
            #endregion
            try
            {
                #region Main working
                Connection.BeginTransaction();
                //1、要刪除的資料先選出來
                TablePack<Product_Category_L1> dataWork = new TablePack<Product_Category_L1>(Connection) { LoginUserID = this.logPlamInfo.UserId, LoginUnitID = this.logPlamInfo.UnitId }; //宣告泛型物件並連接Connection
                dataWork.SelectFields(x => x.id); //只Select 主Key欄位
                dataWork.WhereFields(x => x.id, ids); //代入陣列Id值

                m_Product_Category_L1[] md_Origin = dataWork.DataByAdapter<m_Product_Category_L1>(); //取得Key值的Data

                //2、進行全部刪除
                dataWork.DeleteAll(); //先刪除DataTable
                dataWork.UpdateDataAdapter(); //在更新至DataBase Server
                Connection.EndCommit();
                r.Result = true;
                dataWork.Dispose(); //釋放資料
                md_Origin = null;
                return r;
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System; //系統失敗
                r.Message = PackErrMessage(ex); //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
        }
        public override RunQueryPackage<m_Product_Category_L1> SearchMaster(q_Product_Category_L1 qr, int accountId)
        {
            #region 變數宣告
            RunQueryPackage<m_Product_Category_L1> r = new RunQueryPackage<m_Product_Category_L1>();
            #endregion
            try
            {
                #region Select Data 區段 By 條件
                #region 設定輸出至Grid欄位
                Table1Join<Product_Category_L1> dataWork = new Table1Join<Product_Category_L1>(Connection) { };
                dataWork.SelectFields(x => new { x.id, x.category_l1_name, x.sort });

                if (qr.MaxRecord > 0) dataWork.TopLimit = qr.MaxRecord;

                #endregion

                #region 設定Where條件
                if (qr.s_title != null)
                    dataWork.WhereFields(x => x.category_l1_name, qr.s_title, WhereCompareType.Like);

                dataWork.WhereFields(x => x._隱藏, false);
                dataWork.WhereLang(); //使用語系條件
                #endregion

                #region 設定排序
                dataWork.OrderByFields(x => x.sort);
                #endregion

                #region 輸出物件陣列
                r.SearchData = dataWork.DataByAdapter<m_Product_Category_L1>();
                r.Result = true;
                return r;
                #endregion
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System; //系統失敗
                r.Message = PackErrMessage(ex); //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
        }
        public override RunOneDataEnd<m_Product_Category_L1> GetDataMaster(int id, int accountId)
        {
            #region 變數宣告
            RunOneDataEnd<m_Product_Category_L1> r = new RunOneDataEnd<m_Product_Category_L1>();

            #endregion
            try
            {
                #region Main working
                TablePack<Product_Category_L1> dataWork = new TablePack<Product_Category_L1>(Connection) { LoginUserID = this.logPlamInfo.UserId, LoginUnitID = this.logPlamInfo.UnitId };
                dataWork.TableModule.KeyFieldModules[dataWork.TableModule.id.N].V = id; //設定KeyValue
                m_Product_Category_L1 md = dataWork.GetDataByKey<m_Product_Category_L1>(); //取得Key該筆資料

                if (md == null) //如無資料
                    throw new LogicRoll("Log_Err_MustHaveData"); //此區一定有資料傳出，如無資料應檢查前端id值是否有誤

                r.SearchData = md;
                r.Result = true;
                dataWork.Dispose(); //釋放資料
                return r;
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System; //系統失敗
                r.Message = PackErrMessage(ex); //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
        }
        public override RunOneDataEnd<m_Product_Category_L1> GetDataMaster(int[] id, int accountId)
        {
            throw new NotImplementedException();
        }
        public Dictionary<String, String> options_category_l1(int accountId)
        {
            #region 變數宣告
            //RunQueryPackage<m_車輛廠牌檔> r = new RunQueryPackage<m_車輛廠牌檔>();
            #endregion

            #region Select Data 區段 By 條件
            #region 設定輸出至Grid欄位 以下方式請注音 1、只適合單一Table 2、主要用於Grid顯示，如此方式不適合，可自行組SQL字串再夜由至ExecuteData達行
            TablePack<Product_Category_L1> dataWork = new TablePack<Product_Category_L1>(Connection) { LoginUserID = accountId };
            dataWork.SelectFields(x => new { x.id, product_category_l1_name = x.category_l1_name });
            #endregion

            #region 設定Where條件

            #endregion

            #region 設定排序
            dataWork.OrderByFields(x => x.sort);
            #endregion

            #region 輸出成DataTable
            return dataWork.DataByAdapter<m_Product_Category_L1>()
                .Select(x => new { x.id, kindname = x.category_l1_name })
                .ToDictionary(x => x.id.ToString(), x => x.kindname);

            #endregion
            #endregion
        }
        public m_Product_Category All_Product_Category()
        {

            Table1Join<Product_Category_L1> dataWork_L1 = new Table1Join<Product_Category_L1>(Connection) { };
            dataWork_L1.SelectFields(x => new { x.id, x.category_l1_name });
            dataWork_L1.WhereFields(x => x._隱藏, false);
            dataWork_L1.WhereLang();
            dataWork_L1.OrderByFields(x => x.sort);

            var r = dataWork_L1.DataByAdapter<m_Product_Category_L1>();

            foreach (var r1 in r)
            {

                Table1Join<Product_Category_L2> dataWork_L2 = new Table1Join<Product_Category_L2>(Connection) { };
                dataWork_L2.SelectFields(x => new { x.id, x.category_l2_name });
                dataWork_L2.WhereFields(x => x.product_category_l1_id, r1.id);
                dataWork_L1.WhereFields(x => x._隱藏, false);
                dataWork_L2.WhereLang();
                dataWork_L2.OrderByFields(x => x.sort);
                r1.Category_L2 = dataWork_L2.DataByAdapter<m_Product_Category_L2>();
            }
            return (new m_Product_Category { Product_Category = r });
        }
    }
    #endregion
    #region Product_Category_L2
    public class q_Product_Category_L2 : QueryBase
    {
        public int? s_product_category_l1_id { set; get; }
    }
    public class m_Product_Category_L2 : ModuleBase
    {
        ///<summary>
        ///Mapping:product_category_l2_id
        ///</summary>
        public Int32 id { get; set; }
        public Int32 product_category_l1_id { get; set; }
        public String category_l2_name { get; set; }
        public Int32 sort { get; set; }
    }
    public class a_Product_Category_L2 : LogicBase<m_Product_Category_L2, q_Product_Category_L2, Product_Category_L2>
    {
        public override RunInsertEnd InsertMaster(m_Product_Category_L2 md, int accountId)
        {
            #region Variable declare area
            RunInsertEnd r = new RunInsertEnd(); //宣告回傳物件
            #endregion
            try
            {
                #region Main working
                Connection.BeginTransaction(); //開始交易鎖定
                TablePack<Product_Category_L2> dataWork = new TablePack<Product_Category_L2>(Connection) { LoginUserID = this.logPlamInfo.UserId, LoginUnitID = this.logPlamInfo.UnitId };

                dataWork.NewRow(); //開始新橧作業 產生新的一行
                #region 指派值

                dataWork.SetDataRowValue(x => x.id, md.id);
                dataWork.SetDataRowValue(x => x.product_category_l1_id, md.product_category_l1_id);
                dataWork.SetDataRowValue(x => x.category_l2_name, md.category_l2_name);
                dataWork.SetDataRowValue(x => x.sort, md.sort);
                dataWork.SetDataRowValue(x => x._隱藏, false);

                dataWork.UpdateFieldsInfo(UpdateFieldsInfoType.Insert); //進行更新時 需同時更新系統欄位 _新增人員，_新增日期
                dataWork.UpdateFieldsInfo(UpdateFieldsInfoType.Lang); //語系欄位設定
                //進行更新時 需同時更新系統欄位 _InsertUserID，_InsertDateTime
                #endregion
                dataWork.AddRow(); //加載至DataTable
                dataWork.UpdateDataAdapter(); //更新 DataBase Server

                Connection.EndCommit(); //交易確認

                r.Rows = dataWork.AffetCount; //取得影響筆數
                r.InsertId = dataWork.InsertAutoFieldsID; //取得新增後自動新增欄位的值
                r.Result = true; //回傳本次執行結果為成功
                dataWork.Dispose(); //釋放資料

                return r;
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System; //系統失敗
                r.Message = PackErrMessage(ex); //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
        }
        public override RunUpdateEnd UpdateMaster(m_Product_Category_L2 md, int accountId)
        {
            #region Variable declare area
            RunUpdateEnd r = new RunUpdateEnd();
            #endregion
            try
            {
                #region Main Working
                Connection.BeginTransaction();
                TablePack<Product_Category_L2> dataWork = new TablePack<Product_Category_L2>(Connection) { LoginUserID = this.logPlamInfo.UserId, LoginUnitID = this.logPlamInfo.UnitId };

                dataWork.TableModule.KeyFieldModules[dataWork.TableModule.id.N].V = md.id; //取得ID欄位的值
                m_Product_Category_L2 md_Origin = dataWork.GetDataByKey<m_Product_Category_L2>(); //取得Key值的Data

                if (md_Origin == null) //如有資料
                    throw new LogicRoll("Log_Err_MustHaveData"); //此區一定有資料傳出，如無資料應檢查前端id值是否有誤

                dataWork.EditFirstRow(); //編輯第一筆資料，正常只會有一筆資料。
                #region 指派值

                dataWork.SetDataRowValue(x => x.id, md.id);
                dataWork.SetDataRowValue(x => x.product_category_l1_id, md.product_category_l1_id);
                dataWork.SetDataRowValue(x => x.category_l2_name, md.category_l2_name);
                dataWork.SetDataRowValue(x => x.sort, md.sort);


                dataWork.UpdateFieldsInfo(UpdateFieldsInfoType.Update); //指定進行更新時 需同時更新系統欄位 _修改人員，_修改日期
                #endregion
                md_Origin = null;
                dataWork.UpdateDataAdapter(); //進行變更至Database Server

                Connection.EndCommit();

                r.Rows = dataWork.AffetCount; //取得影響筆數
                r.Result = true;
                dataWork.Dispose(); //釋放資料
                return r;
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System; //系統失敗
                r.Message = PackErrMessage(ex); //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
        }
        public override RunDeleteEnd DeleteMaster(Int32[] ids, int accountId)
        {
            //此功能主要搭配Grid介面刪除功能製作
            #region Variable declare area
            RunDeleteEnd r = new RunDeleteEnd(); //宣告刪除Result回物件
            #endregion
            try
            {
                #region Main working
                Connection.BeginTransaction();
                //1、要刪除的資料先選出來
                TablePack<Product_Category_L2> dataWork = new TablePack<Product_Category_L2>(Connection) { LoginUserID = this.logPlamInfo.UserId, LoginUnitID = this.logPlamInfo.UnitId }; //宣告泛型物件並連接Connection
                dataWork.SelectFields(x => x.id); //只Select 主Key欄位
                dataWork.WhereFields(x => x.id, ids); //代入陣列Id值

                m_Product_Category_L2[] md_Origin = dataWork.DataByAdapter<m_Product_Category_L2>(); //取得Key值的Data

                //2、進行全部刪除
                dataWork.DeleteAll(); //先刪除DataTable
                dataWork.UpdateDataAdapter(); //在更新至DataBase Server
                Connection.EndCommit();
                r.Result = true;
                dataWork.Dispose(); //釋放資料
                md_Origin = null;
                return r;
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System; //系統失敗
                r.Message = PackErrMessage(ex); //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
        }
        public override RunQueryPackage<m_Product_Category_L2> SearchMaster(q_Product_Category_L2 qr, int accountId)
        {
            #region 變數宣告
            RunQueryPackage<m_Product_Category_L2> r = new RunQueryPackage<m_Product_Category_L2>();
            #endregion
            try
            {
                #region Select Data 區段 By 條件
                #region 設定輸出至Grid欄位
                TablePack<Product_Category_L2> dataWork = new TablePack<Product_Category_L2>(Connection) { LoginUserID = this.logPlamInfo.UserId, LoginUnitID = this.logPlamInfo.UnitId };
                dataWork.SelectFields(x => new { x.id, x.product_category_l1_id, x.category_l2_name, x.sort });

                if (qr.MaxRecord > 0) dataWork.TopLimit = qr.MaxRecord;

                #endregion

                #region 設定Where條件
                if (qr.s_product_category_l1_id != null)
                    dataWork.WhereFields(x => x.product_category_l1_id, qr.s_product_category_l1_id);

                dataWork.WhereFields(x => x._隱藏, false);
                dataWork.WhereLang(); //使用語系條件
                #endregion

                #region 設定排序
                if (qr.sidx == null)
                    dataWork.OrderByFields(x => x.sort); //預設排序
                else
                    dataWork.OrderByFields(x => x.id, OrderByType.ASC);
                #endregion

                #region 輸出物件陣列
                r.SearchData = dataWork.DataByAdapter<m_Product_Category_L2>();
                r.Result = true;
                return r;
                #endregion
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System; //系統失敗
                r.Message = PackErrMessage(ex); //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
        }
        public override RunOneDataEnd<m_Product_Category_L2> GetDataMaster(int id, int accountId)
        {
            #region 變數宣告
            RunOneDataEnd<m_Product_Category_L2> r = new RunOneDataEnd<m_Product_Category_L2>();

            #endregion
            try
            {
                #region Main working
                TablePack<Product_Category_L2> dataWork = new TablePack<Product_Category_L2>(Connection) { LoginUserID = this.logPlamInfo.UserId, LoginUnitID = this.logPlamInfo.UnitId };
                dataWork.TableModule.KeyFieldModules[dataWork.TableModule.id.N].V = id; //設定KeyValue
                m_Product_Category_L2 md = dataWork.GetDataByKey<m_Product_Category_L2>(); //取得Key該筆資料

                if (md == null) //如無資料
                    throw new LogicRoll("Log_Err_MustHaveData"); //此區一定有資料傳出，如無資料應檢查前端id值是否有誤

                r.SearchData = md;
                r.Result = true;
                dataWork.Dispose(); //釋放資料
                return r;
                #endregion
            }
            catch (LogicRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.Message = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System; //系統失敗
                r.Message = PackErrMessage(ex); //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                return r;
                #endregion
            }
        }
        public override RunOneDataEnd<m_Product_Category_L2> GetDataMaster(int[] id, int accountId)
        {
            throw new NotImplementedException();
        }
    }
    #endregion
    #endregion

    public class ItemsManage : LogicBase
    {
        public Dictionary<String, String> i_ProductData(int accountId)
        {
            #region Main working
            Product TObj = new Product();// 取得Table物
            TablePack<Product> dataWork = new TablePack<Product>(Connection) { LoginUserID = accountId };
            dataWork.SelectFields(x => new { x.id, x.product_serial, x.product_name });
            dataWork.OrderByFields(x => x.product_name);
            return dataWork.DataByAdapter().dicMakeKeyValue(1, 2);
            #endregion
        }
        public Dictionary<String, String> i_CurrencyData(int accountId)
        {
            #region Main working
            Product TObj = new Product();// 取得Table物
            TablePack<_Currency> dataWork = new TablePack<_Currency>(Connection) { LoginUserID = accountId };
            dataWork.SelectFields(x => new { x.code, x.name_currency });
            dataWork.OrderByFields(x => x.code);
            return dataWork.DataByAdapter().dicMakeKeyValue(0, 1);
            #endregion
        }
    }

    #region Data Handle Extension
    public static class BusinessLoginExtension
    {
        public static String BooleanValue(this Boolean s, BooleanSheetBase b)
        {
            if (s) { return b.TrueValue; } else { return b.FalseValue; }
        }
        public static String CodeValue(this String s, List<_Code> b)
        {
            var result = b.Where(x => x.Code == s);
            if (result.Count() > 0)
            {
                return result.FirstOrDefault().Value;
            }
            else
            {
                return "";
            }
        }
    }
    #endregion
}
