using Dapper;
using System;
using System.Text;
using System.Web.Http;
using GFDF.Infrastruct.Core;
using GFDF.Infrastruct.Extensions;
using GFDP.Sys.Entity;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using GFDP.Sys.Filter;

namespace GFDP.Sys.Controllers
{
    [RoutePrefix("sys/user")]
    public class UserController : BaseController<UserEntity>
    {
        /// <summary>
        /// 获取当前令牌的个人信息
        /// </summary>
        /// <returns></returns>
        [Route("info/v1"), HttpGet]
        [EventBus(sAllowAud = "USER", bWrap = true)]
        public ResponseResult Info()
        {
            ResponseResult result = ResponseResult.Success();
            result.data = GFContext.cache.Get($"U_{Request.Properties["U_ID"]}");
            return result;
        }
        /// <summary>
        /// 获取当前令牌的个人信息
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("info/v2")]
        [EventBus(sAllowAud = "USER", bWrap = true)]
        public object Info2()
        {
            var queryMultiple = GFContext.repository.GetMultiple($"select * from sys_user where id=@userid;select * from sys_dept where id=(select dept from sys_user where id=@userid); ", out System.Data.IDbConnection conn, new { userid = Request.Properties["U_ID"] });
            var user = queryMultiple.ReadFirstOrDefault<UserEntity>();
            AssertHelper.EnsureNotNull(user, "用户不存在");
            var dept = queryMultiple.ReadFirstOrDefault<DeptEntity>();//用户的部门信息
            var roles = GFContext.repository.Query<RoleEntity>($"select * from sys_role where id in({user.roles});");//用户的角色信息
            var permis = GFContext.repository.Query<PermisEntity>($"select * from sys_permission where role in ({user.roles})");//用户的权限信息
            string strMenu = string.Join(",", permis.Select(obj => obj.menu));
            var menus = GFContext.repository.Query<MenuEntity>($"select * from sys_menu where id in({(string.IsNullOrEmpty(strMenu) ? "0" : strMenu)});");//用户有权限菜单信息
            conn.Dispose();
            conn.Close();
            var cache = GFContext.cache.Get($"U_{Request.Properties["U_ID"]}");
            return new { user, dept, roles, menus, cache };
        }
        /// <summary>
        /// 获取当前令牌的个人信息
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("info/v2_1")]
        [EventBus(sAllowAud = "USER", bWrap = true)]
        public object Info2_1()
        {
            var queryMultiple = GFContext.repository.GetMultiple($"select * from sys_user where id=@userid;select * from sys_dept where id=(select dept from sys_user where id=@userid);select roles from sys_user where id=@userid; ", out System.Data.IDbConnection conn, new { userid = Request.Properties["U_ID"] });
            var user = queryMultiple.ReadFirstOrDefault<UserEntity>();
            AssertHelper.EnsureNotNull(user, "用户不存在");
            var dept = queryMultiple.ReadFirstOrDefault<DeptEntity>();//用户的部门信息
            string roleStrs = queryMultiple.ReadSingle<string>();
            var roles = GFContext.repository.Query<RoleEntity>($"select * from sys_role where id in({roleStrs});");//用户的角色信息
            conn.Dispose();
            conn.Close();
            var permis = GFContext.repository.Query<PermisEntity>($"select * from sys_permission where role in ({roleStrs})");
            string webapiStrs = string.Join(",", permis.Select(obj => obj.webapi));
            var webapis = GFContext.repository.Query<PageEntity>($"select * from sys_webapi where id in({(string.IsNullOrEmpty(webapiStrs) ? "0" : webapiStrs)});");
            string pageStrs = string.Join(",", permis.Select(obj => obj.page));
            var pages = GFContext.repository.Query<PageEntity>($"select * from sys_page where id in({(string.IsNullOrEmpty(pageStrs) ? "0" : pageStrs)});");
            string btnStrs = string.Join(",", permis.Select(obj => obj.button));
            var btns = GFContext.repository.Query<PageEntity>($"select * from sys_button where id in({(string.IsNullOrEmpty(btnStrs) ? "0" : btnStrs)});");
            string menuStrs = string.Join(",", permis.Select(obj => obj.menu));
            var menus = GFContext.repository.Query<MenuEntity>($"select * from sys_menu where id in({(string.IsNullOrEmpty(menuStrs) ? "0" : menuStrs)});");
            var cache = GFContext.cache.Get($"U_{Request.Properties["U_ID"]}");
            return new { user, dept, roles, webapis, pages, btns, menus, cache };
        }

        /// <summary>
        /// 部门含子部门下人员信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("dept/{id:long}")]
        public ResponseResult GetByDept(long id)
        {
            ResponseResult result = ResponseResult.Success();
            string sql = "WITH TEMP AS ( SELECT * FROM sys_dept WHERE id =@id UNION ALL SELECT T0.* FROM TEMP, sys_dept T0 WHERE TEMP.id = T0.pid ) SELECT * FROM sys_user where dept in (select id from TEMP);";
            result.data = GFContext.repository.Query<UserEntity>(sql, new { id });
            return result;
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override ResponseResult Create(UserEntity entity)
        {
            bool exist = base.EstimateRe(new { entity.account });
            AssertHelper.EnsureTrue(!exist, "该用户名已存在！");
            entity.pwd = entity.pwd.ToMd5();
            return base.Create(entity);
        }

        /// <summary>
        /// 用户修改密码
        /// </summary>
        /// <param name="upPwd"></param>
        /// <returns></returns>
        [HttpPut, Route("updatepwd")]
        [EventBus(sAllowAud = "USER", bWrap = true)]
        public void UpdatePwd(dynamic upPwd)
        {
            AssertHelper.EnsureFalse((upPwd == null || string.IsNullOrEmpty((string)upPwd.oldpwd) || string.IsNullOrEmpty((string)upPwd.newpwd)), "数据不能为空！");
            bool exist = EstimateRe(new { id = Convert.ToInt64(Request.Properties["U_ID"]), pwd = ((string)upPwd.oldpwd).ToMd5() });
            AssertHelper.EnsureTrue(exist, "旧密码错误！");
            string sql = "update sys_user set pwd=@pwd where id=@id and cstatus&4=0";
            int affected = GFContext.repository.Execute(sql, new { id = Convert.ToInt64(Request.Properties["U_ID"]), pwd = ((string)upPwd.newpwd).ToMd5() });
            AssertHelper.EnsureTrue(affected > 0, "用户已被锁定，不能进行修改密码操作。");
        }

        /// <summary>
        /// 管理员登录
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost, Route("login"), AllowAnonymous]
        [EventBus(bAudit = true)]
        public ResponseResult Login(UserEntity user)
        {
            AssertHelper.EnsureFalse(user == null || string.IsNullOrEmpty(user.account) || string.IsNullOrEmpty(user.pwd), ExEnum.ArgNullEx);
            string pwd = user.pwd.ToMd5();
            var model = GFContext.repository.Query<dynamic>($"select sys_user.id,sys_user.cname,dept,account,sys_dept.cname as deptname,sys_user.roles from sys_user left join sys_dept on dept=sys_dept.id where account='{user.account}' and pwd='{pwd}'").FirstOrDefault();
            AssertHelper.EnsureNotNull(model, "登录失败，用户名或密码错误！");

            string token = GFContext.IssueToken(new { model.id }, "USER", model.id.ToString());
            string refToken = GFContext.IssueToken(new { model.id }, "REFRESH", model.id.ToString(), 86400);

            GFContext.cache.Insert($"U_{model.id}", new { model.id, model.account, model.cname, model.dept, model.deptname, model.roles, token }, new TimeSpan(0, 0, 0, 7200));
            //cache可增加多个属性以满足不同业务需求，由业务系统扩展。
            return ResponseResult.Success(new { token, refToken, model.account, model.cname, model.dept, model.deptname });
        }
        /// <summary>
        /// 管理员登出
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost, Route("loginout")]
        [EventBus(sAllowAud = "USER", bWrap = true)]
        public void LoginOut()
        {
            GFContext.cache.Remove($"U_{Request.Properties["U_ID"]}");
            var s = ObjectExt.Add(new { token = Request.Headers.Authorization.ToString(), U_ID = Request.Properties["U_ID"] }, "event", "User.LoginOut");
            EventBus.Emit("User.Login_Out", s);
        }

        /// <summary>
        /// 刷新token
        /// </summary>
        /// <returns></returns>
        [Route("refreshtoken"), HttpGet]
        [EventBus(sAllowAud = "REFRESH")]
        public ResponseResult RefreshToken()
        {
            string token = GFContext.IssueToken(new { id = Request.Properties["U_ID"] }, "USER", $"{Request.Properties["U_ID"]}");
            return ResponseResult.Success(new { token });
        }
    }
}
