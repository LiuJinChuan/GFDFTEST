using System.Collections.Generic;
using System.Web.Http;
using System.Linq;
using GFDF.Infrastruct.Core;
using GFDF.Infrastruct.Extensions;
using SysContext = GFDP.Sys.GFContext;
using GFDP.Sys.Controllers;
using GFDP.Dev.Entity;
using GFDF.Infrastruct.Impl;
using GFDP.Sys.Filter;

namespace GFDP.Dev.Controllers
{
    [RoutePrefix("sys/formins")]
    public class FormInsController : BaseController<FormInsEntity> { }

    [RoutePrefix("sys/meta")]
    public class MetaController : BaseController<MetaEntity> { }

    [RoutePrefix("base/chart")]
    public class ChartController : BaseController<ChartEntity>
    {
        /// <summary>
        /// �Ǳ������ͼ��
        /// </summary>
        /// <returns></returns>
        [HttpGet,Route("db/{dbid:long}")]
        public ResponseResult QueryByDB(long dbid)
        {
            string sql = $@"select * from base_chart where CHARINDEX(cast(id as varchar),(select charts from base_dashboard where id={dbid} ))>0;";
            ResponseResult result = ResponseResult.Success();
            result.data = SysContext.repository.Query<ChartEntity>(sql);
            return result;
        }
    }

    [RoutePrefix("sys/code")]
    public class CodeSegController : BaseController<CodeSegEntity> { }

    [RoutePrefix("data/sy_csv")]
    public class SyCsvController : BaseController<Sy_CsvEntity> { }

    [RoutePrefix("sys/reportdtl")]
    public class ReportdtlController : BaseController<ReportdtlEntity> { }

    [RoutePrefix("sys/report")]
    public class ReportController : BaseController<ReportEntity> { }
    [RoutePrefix("sys/codedtl")]
    public class CodeSegDtlController : BaseController<CodeSegDtlEntity>
    {
        /// <summary>
        /// �б���ʾ
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("list/{segid:long}")]
        public ResponseResult ListBySeg(long segid)
        {
            return base.QueryList("segid=@segid", new { segid });
        }

        /// <summary>
        /// ��ȡ��������
        /// </summary>
        /// <returns></returns>
        [HttpGet,Route("viewbyseg/{segid:long}")]
        public ResponseResult ViewBySeg(long segid)
        {
            string tblname = SysContext.repository.GetTableName<CodeSegDtlEntity>();

            var condition = new List<KeyValuePair<string, object>>();
            condition.Add(new KeyValuePair<string, object>("segid", segid));

            var data = SysContext.repository.QueryList<CodeSegDtlEntity>(condition, tblname).FirstOrDefault();
            return ResponseResult.Success(data);
        }
    }

    [RoutePrefix("sys/db")]
    public class DBController : BaseController<DBEntity>
    {
        /// <summary>
        /// ͬ�����ݿ�
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("dbsync/{id:long}"), HttpGet]
        [AllowAnonymous]
        public ResponseResult DbSync(long id)
        {
            var dB = SysContext.repository.GetById<DBEntity>(id);
            AssertHelper.EnsureNotNull(dB, "���ݿⲻ����");
            AssertHelper.EnsureFalse(string.IsNullOrEmpty(dB.connstr), "�����ַ����������");
            Repository repository = new Repository(dB.connstr);
            var tablesIn = repository.Query<DBTableEntity>($"SELECT Name ename,{id} btdatabase FROM {dB.ename}..SysObjects Where XType='U' ORDER BY Name;");
            foreach (var item in tablesIn)
            {
                var table = SysContext.repository.Query<DBTableEntity>($"select * from sys_dbtable where btdatabase=@btdatabase and ename=@ename", new { btdatabase = dB.id, item.ename }).FirstOrDefault();
                if (table == null)
                {
                    item.id = SysContext.idworker.nextId();
                    SysContext.repository.Insert(item);
                }
                else SysContext.repository.Update(table);
            }
            var tables = repository.Query<DBTableEntity>($"select * from sys_dbtable where btdatabase=@btdatabase", new { btdatabase = id });
            foreach (var table in tables)
            {
                var list = repository.Query<DBTableFieldEntity>($"SELECT SCOL.NAME ename,(SELECT SYS.EXTENDED_PROPERTIES.VALUE FROM SYSCOLUMNS INNER JOIN SYS.EXTENDED_PROPERTIES ON SYSCOLUMNS.ID = SYS.EXTENDED_PROPERTIES.MAJOR_ID AND SYSCOLUMNS.COLID = SYS.EXTENDED_PROPERTIES.MINOR_ID INNER JOIN SYSOBJECTS ON SYSCOLUMNS.ID = SYSOBJECTS.ID WHERE SYSOBJECTS.NAME = SO.NAME AND SYSCOLUMNS.NAME = SCOL.NAME) AS cname,STYPE.NAME AS ctype,SCOL.PREC clength,SCOL.SCALE accuracy,SCOL.ISNULLABLE allownull,{table.id} bttable,{dB.id} btdatabase,0 btserver from SYSCOLUMNS AS SCOL LEFT JOIN SYSOBJECTS SO ON SO.ID = SCOL.ID LEFT JOIN SYSTYPES AS STYPE ON STYPE.xtype = SCOL.xtype Where SCOL.ID = OBJECT_ID('{table.ename}') AND STYPE.NAME <> 'SYSNAME'");
                foreach (var item in list)
                {
                    var field = SysContext.repository.Query<DBTableFieldEntity>($"select * from sys_dbtablefield where bttable=@bttable and btdatabase=@btdatabase and ename=@ename", new { bttable = table.id, btdatabase = dB.id, item.ename }).FirstOrDefault();
                    if (field == null)
                    {
                        item.id = SysContext.idworker.nextId();
                        SysContext.repository.Insert(item);
                    }
                    else
                    {
                        field.cname = item.cname;
                        field.ctype = item.ctype;
                        field.clength = item.clength;
                        field.accuracy = item.accuracy;
                        field.allownull = item.allownull;
                        SysContext.repository.Update(field);
                    }
                }
            }
            return ResponseResult.Success();
        }

        /// <summary>
        /// ��ȡ���ݿ��
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("gettables/{id:long}"), HttpGet]
        [AllowAnonymous]
        public ResponseResult GetTables(long id)
        {
            var dBTable = SysContext.repository.GetById<DBEntity>(id);
            AssertHelper.EnsureNotNull(dBTable, "���ݿⲻ����");
            AssertHelper.EnsureFalse(string.IsNullOrEmpty(dBTable.connstr), "�����ַ����������");
            Repository repository = new Repository(dBTable.connstr);
            var list = repository.Query<dynamic>($"SELECT id,Name FROM {dBTable.ename}..SysObjects Where XType='U' ORDER BY Name;");
            return ResponseResult.Success(list);
        }
    };
    [RoutePrefix("sys/dbtable")]
    public class DBTableController : BaseController<DBTableEntity>
    {
        /// <summary>
        /// ͬ�����ݱ�
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("tablesync/{id:long}"), HttpGet]
        [AllowAnonymous]
        public ResponseResult TableSync(long id)
        {
            var dBTable = SysContext.repository.GetById<DBTableEntity>(id);
            AssertHelper.EnsureNotNull(dBTable, "���ݱ�����");
            var dB = SysContext.repository.GetById<DBEntity>(dBTable.btdatabase);
            AssertHelper.EnsureNotNull(dB, "���ݿⲻ����");
            AssertHelper.EnsureFalse(string.IsNullOrEmpty(dB.connstr), "�����ַ����������");
            Repository repository = new Repository(dB.connstr);
            var list = repository.Query<DBTableFieldEntity>($"SELECT SCOL.NAME ename,(SELECT SYS.EXTENDED_PROPERTIES.VALUE FROM SYSCOLUMNS INNER JOIN SYS.EXTENDED_PROPERTIES ON SYSCOLUMNS.ID = SYS.EXTENDED_PROPERTIES.MAJOR_ID AND SYSCOLUMNS.COLID = SYS.EXTENDED_PROPERTIES.MINOR_ID INNER JOIN SYSOBJECTS ON SYSCOLUMNS.ID = SYSOBJECTS.ID WHERE SYSOBJECTS.NAME = SO.NAME AND SYSCOLUMNS.NAME = SCOL.NAME) AS cname,STYPE.NAME AS ctype,SCOL.PREC clength,SCOL.SCALE accuracy,SCOL.ISNULLABLE allownull,{dBTable.id} bttable,{dB.id} btdatabase,0 btserver from SYSCOLUMNS AS SCOL LEFT JOIN SYSOBJECTS SO ON SO.ID = SCOL.ID LEFT JOIN SYSTYPES AS STYPE ON STYPE.xtype = SCOL.xtype Where SCOL.ID = OBJECT_ID('{dBTable.ename}') AND STYPE.NAME <> 'SYSNAME'");
            foreach (var item in list)
            {
                var field = SysContext.repository.Query<DBTableFieldEntity>($"select * from sys_dbtablefield where bttable=@bttable and btdatabase=@btdatabase and ename=@ename", new { bttable = dBTable.id, btdatabase = dB.id, item.ename }).FirstOrDefault();
                if (field == null)
                {
                    item.id = SysContext.idworker.nextId();
                    SysContext.repository.Insert(item);
                }
                else
                {
                    field.cname = item.cname;
                    field.ctype = item.ctype;
                    field.clength = item.clength;
                    field.accuracy = item.accuracy;
                    field.allownull = item.allownull;
                    SysContext.repository.Update(field);
                }
            }
            return ResponseResult.Success();
        }

        /// <summary>
        /// ��ȡ���ݿ���ֶ�
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("getfields/{id:long}"), HttpGet]
        [AllowAnonymous]
        public ResponseResult GetFields(long id)
        {
            var dBTable = SysContext.repository.GetById<DBTableEntity>(id);
            AssertHelper.EnsureNotNull(dBTable, "���ݱ�����");
            var dB = SysContext.repository.GetById<DBEntity>(dBTable.btdatabase);
            AssertHelper.EnsureNotNull(dB, "���ݿⲻ����");
            AssertHelper.EnsureFalse(string.IsNullOrEmpty(dB.connstr), "�����ַ����������");
            Repository repository = new Repository(dB.connstr);
            var list = repository.Query<dynamic>($"SELECT SCOL.NAME ename,(SELECT SYS.EXTENDED_PROPERTIES.VALUE FROM SYSCOLUMNS INNER JOIN SYS.EXTENDED_PROPERTIES ON SYSCOLUMNS.ID = SYS.EXTENDED_PROPERTIES.MAJOR_ID AND SYSCOLUMNS.COLID = SYS.EXTENDED_PROPERTIES.MINOR_ID INNER JOIN SYSOBJECTS ON SYSCOLUMNS.ID = SYSOBJECTS.ID WHERE SYSOBJECTS.NAME = SO.NAME AND SYSCOLUMNS.NAME = SCOL.NAME) AS cname,STYPE.NAME AS ctype,SCOL.PREC clength,SCOL.SCALE accuracy,SCOL.ISNULLABLE allownull,{dBTable.id} bttable,{dB.id} btdatabase,0 btserver from SYSCOLUMNS AS SCOL LEFT JOIN SYSOBJECTS SO ON SO.ID = SCOL.ID LEFT JOIN SYSTYPES AS STYPE ON STYPE.xtype = SCOL.xtype Where SCOL.ID = OBJECT_ID('{dBTable.ename}') AND STYPE.NAME <> 'SYSNAME'");
            return ResponseResult.Success(list);
        }
    }
    [RoutePrefix("sys/dbtablefield")]
    public class DBTableFieldController : BaseController<DBTableFieldEntity> { }

    [RoutePrefix("other")]
    public class DevOtherController : ApiController
    {
        [HttpPost,Route("execsql")]
        [AllowAnonymous]
        public ResponseResult ExecSQL(dynamic param)
        {
            IEnumerable<dynamic> list = SysContext.repository.Query<dynamic>(param.sql.Value);
            ResponseResult result = ResponseResult.Success();
            result.data = list;
            result.count = list.Count();
            return result;
        }
    }
}
