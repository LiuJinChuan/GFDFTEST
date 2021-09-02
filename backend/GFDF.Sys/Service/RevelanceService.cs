using GFDP.Sys.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GFDP.Sys.Service
{
    public static class Define
    {
        public static string USERROLE = "UserRole";       //用户角色关联KEY
        public const string ROLERESOURCE = "RoleResource";  //角色资源关联KEY
        public const string USERORG = "UserOrg";  //用户机构关联KEY
        public const string ROLEORG = "RoleOrg";  //角色机构关联KEY
        public const string ROLEELEMENT = "RoleElement"; //角色菜单关联KEY
        public const string ROLEMODULE = "RoleModule";   //角色模块关联KEY
        public const string ROLEDATAPROPERTY = "RoleDataProperty";   //角色数据字段权限
    }

    public class RevelanceService 
    {
        ///// <summary>
        ///// 添加关联
        ///// <para>比如给用户分配资源，那么firstId就是用户ID，secIds就是资源ID列表</para>
        ///// </summary>
        ///// <param name="type">关联的类型，如Define.USERRESOURCE</param>
        //public void Assign(AssignReq request)
        //{
        //    Assign(request.type, request.secIds.ToLookup(u => request.firstId));
        //}

        /// <summary>
        /// 添加关联，需要人工删除以前的关联
        /// </summary>
        /// <param name="key"></param>
        /// <param name="idMaps"></param>
        public void Assign(string key, ILookup<long, long> idMaps)
        {
            IEnumerable<RelevanceEntity> objs = (from sameVals in idMaps
                from value in sameVals
                select new RelevanceEntity
                {
                    id = GFContext.idworker.nextId(),
                    key = key,
                    first = sameVals.Key,
                    second = value
                }).ToArray();
            GFContext.repository.InsertBatch<RelevanceEntity>(objs);
        }

        ///// <summary>
        ///// 取消关联
        ///// </summary>
        ///// <param name="type">关联的类型，如Define.USERRESOURCE</param>
        ///// <param name="firstId">The first identifier.</param>
        ///// <param name="secIds">The sec ids.</param>
        //public void UnAssign(AssignReq req)
        //{
        //    if (req.secIds == null || req.secIds.Length == 0)
        //    {
        //        DeleteBy(req.type, req.firstId);
        //    }
        //    else
        //    {
        //       DeleteBy(req.type, req.secIds.ToLookup(u => req.firstId));
        //    }
        //}

        /// <summary>
        /// 删除关联
        /// </summary>
        /// <param name="key">关联标识</param>
        /// <param name="idMaps">关联的&lt;firstId, secondId&gt;数组</param>
        private void DeleteBy(string key, ILookup<long, long> idMaps)
        {
            foreach (var sameVals in idMaps)
            {
                GFContext.repository.Execute($"delete sys_relevance where key={key} and first={sameVals.Key} and second in @ids", new { ids = sameVals.ToArray<long>() });
                //foreach (var value in sameVals)
                //{
                //    GFContext.repository.Execute("delete sys_relevance where key=@key and firstid=@firstid and secondid=@secondid", new { key, firstid = sameVals.Key, secondid = value });
                //}
            }
        }

        public void DeleteBy(string key, params long[] firstIds)
        {
            GFContext.repository.Execute($"delete sys_relevance where key={key} and first in ({string.Join(",", firstIds)})");
        }


        /// <summary>
        /// 根据关联表的一个键获取另外键的值
        /// </summary>
        /// <param name="key">映射标识</param>
        /// <param name="returnSecondIds">返回的是否为映射表的第二列，如果不是则返回第一列</param>
        /// <param name="ids">已知的ID列表</param>
        /// <returns>List&lt;System.String&gt;.</returns>
        public IEnumerable<long> Get(string key, bool returnSecondIds, params long[] ids)
        {
           return GFContext.repository.Query<RelevanceEntity>($"select * from sys_relevance where key=@key and {(returnSecondIds ? "first in @ids" : "second in @ids")}", new { key, ids }).Select(obj => returnSecondIds ? obj.first : obj.second);
        }

        /// <summary>
        /// 根据key ,firstId,secondId获取thirdId
        /// </summary>
        /// <param name="key"></param>
        /// <param name="firstId"></param>
        /// <param name="secondId"></param>
        /// <returns></returns>
        public IEnumerable<long> Get(string key, string firstId, string secondId)
        {
            return GFContext.repository.Query<RelevanceEntity>($"select * from sys_relevance where key=@key and first={firstId} and second={secondId}").Select(obj => obj.third);
        }

        ///// <summary>
        ///// 为角色分配用户，需要统一提交，会删除以前该角色的所有用户
        ///// </summary>
        ///// <param name="request"></param>
        //public void AssignRoleUsers(AssignRoleUsers request)
        //{
        //    //删除以前的所有用户
        //    UnitWork.Delete<Relevance>(u => u.SecondId == request.RoleId && u.Key == Define.USERROLE);
        //    //批量分配用户角色
        //    UnitWork.BatchAdd((from firstId in request.UserIds
        //        select new Relevance
        //        {
        //            Key = Define.USERROLE,
        //            FirstId = firstId,
        //            SecondId = request.RoleId,
        //            OperateTime = DateTime.Now
        //        }).ToArray());
        //    UnitWork.Save();
        //}

    }
}