import { apiRequest, BaseApi } from '@/framework/api/apiRequest'

export const UserApi = new BaseApi('/sys/user');
UserApi.ChangePWD = (obj) => apiRequest("/sys/user/updatepwd", "put", obj);
UserApi.GetUserInfo = () => apiRequest("/sys/user/info/v2_1", "get");
//多种登录方式
UserApi.Login = (obj) => apiRequest("/sys/user/login", "post", obj);
UserApi.RefreshToken = () => apiRequest("/sys/user/refreshtoken", "post");
UserApi.Logout = () => apiRequest("/sys/user/loginout", "post");
UserApi.GetByDept = (deptId) => apiRequest(`/sys/user/dept/${deptId}`, "get");

const Audit = {
  load: () => apiRequest(`/base/audit/loginlog`, "get"),
  search: (obj) => apiRequest(`/base/audit/list`, "get", { json: JSON.stringify(obj) }),
  del: (id) => apiRequest(`/base/audit/loginlog/${id}`, "delete")
}

const Cache = {
  search: (callcode) => apiRequest(`/cache/${callcode}`, "get"),
  del: (callcode) => apiRequest(`/cache/${callcode}`, "delete")
}
const Schedule = {
  missionChange: (state, name) => apiRequest(`/schedule/${(state ? "disable" : "enable")}/${name}`, "put"),
  del: (jobname) => apiRequest(`/schedule/${jobname}`, "delete"),
  load: () => apiRequest("/schedule/all", "get")
}
export const CommonApi = { Audit, Cache, Schedule };

export const DictApi = new BaseApi('/sys/dict');
export const MenuApi = new BaseApi('/sys/menu');
export const NoticeApi = new BaseApi('/sys/notice');
export const ButtonApi = new BaseApi('/sys/button');
export const PageApi = new BaseApi('/sys/page');
export const WebapiApi = new BaseApi('/sys/webapi');
export const FormApi = new BaseApi('/sys/form');
export const RolePermisApi = new BaseApi('/sys/role/permis');
export const RegionApi = new BaseApi('/sys/region');
export const FormvalueApi = new BaseApi('/sys/formvalue');
FormvalueApi.getFormExt = (callcode) => apiRequest(`/sys/formvalue/ext/${callcode}`, 'get');
export const FileResApi = new BaseApi('/base/fileres');

export const RoleApi = new BaseApi('/sys/role');
RoleApi.UpdatePermis = (obj) => apiRequest("/sys/role/permis", "post", obj);

export const PermisApi = new BaseApi('/sys/permis');
export const DeptApi = new BaseApi('/sys/dept');
export const JobApi = new BaseApi('/sys/job');

export const MsgApi = new BaseApi('/sys/message');
export const MsgUserApi = new BaseApi('/sys/message_user');
export const F_flowApi = new BaseApi('/fin/flow');//财务流水
export const F_accountApi = new BaseApi('/fin/account');//财务流水


// 广告相关
export const AdvertApi = new BaseApi('/adv/advert');
AdvertApi.pub = (obj) => apiRequest("/adv/advert/pub", "post", obj);
AdvertApi.stop = (c) => apiRequest(`/adv/advert/stop/${c}`, "get");
export const Advert_SpaceApi = new BaseApi('/adv/advert_space');
export const Advert_ReleaseApi = new BaseApi('/adv/advert_release');