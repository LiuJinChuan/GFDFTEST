
import {getLists} from '@/framework/store'

export const UserStore= {
    state: {
      actor:window.GF.context.get('actor')||{},
      userInfo: {},
      permission: {},
      menuId: window.GF.context.get('menuId') || {},
      menu: window.GF.context.actor.menus || [],
      menuAll: window.GF.context.actor.menuAll || [],
      token: window.GF.context.get('token') || "",
    },
    actions: {
      //根据用户名登录  TODO:由于没有其他更多的，实际直接在页面进行API调用
      LoginByUsername($store, userInfo) {
        return new Promise((resolve, reject) => {
          window.GFAPI.sys.UserApi.Login(userInfo)
            .then((res) => {
              //const data = res.data;
              // commit("SET_TOKEN", data.token);
              //commit("DEL_ALL_TAG");
              //commit("CLEAR_LOCK");
              resolve(res.data);
            })
            .catch((res) => {
              reject(res);
            });
        });
      },
      //根据手机号登录
      LoginByPhone({ commit }, userInfo) {
        return new Promise((resolve) => {
          window.GFAPI.sys.LoginApi.LoginByUsername(userInfo).then((res) => {
            const data = res.data;
            commit("SET_TOKEN", data.token);
            commit("DEL_ALL_TAG");
            commit("CLEAR_LOCK");
            resolve();
          });
        });
      },

      //刷新token
      RefeshToken({ state, commit }) {
        return new Promise((resolve, reject) => {
          window.GFAPI.sys.LoginApi(state.refeshToken)
            .then((res) => {
              const data = res.data.data;
              commit("SET_TOKEN", data);
              resolve(data);
            })
            .catch((error) => {
              reject(error);
            });
        });
      },
      // 登出 TODO:由于没有其他更多的，实际直接在页面进行API调用
      LogOut() {
        return new Promise((resolve, reject) => {
          window.GFAPI.sys.UserApi.Logout().then(() => {
            window.GF.emitter.emit('gf-logout')
              resolve();
            })
            .catch((error) => {
              reject(error);
            });
        });
      },
      //注销session  
      FedLogOut({ commit }) {
        return new Promise((resolve) => {
          window.GF.emitter.emit('gf-logout')
          commit("CLEAR_LOCK");
          resolve();
        });
      },
      GetTopMenu() {
        return new Promise((resolve) => {
          resolve([]);
        });
      },
      //获取用户信息
       GetUserInfo({ commit }) {
        return new Promise((resolve, reject) => {
          window.GFAPI.sys.UserApi.GetUserInfo()
            .then((res) => {
              const data = res.data;
              //GFActor的预处理
              //var menusRes = formatMenus(data.menus);
              var menusRes = window.GF.utils.formatMenus(data.menus);
              menusRes.sort((a,b)=>a.seqno-b.seqno)
              var menu = window.GF.utils.buildListToTree(menusRes, "pid", "id", 0);
              menu.unshift({
                    label: "首页",
                    path: "/wel",
                    icon: "el-icon-s-platform",
                    children: [],
                });
              data.menus=menu;
              //TODO: 求最大值问题 建议用以下代码
              //let arrs = data.roles.map(e=>e.datascope);
              // data.user.datascope = Math.max(...arrs)
              let datascope=data.roles.map(e=>e.datascope);
              datascope=datascope.sort(function(a,b){return b-a;})[0];
              data.user.datascope=datascope;
              // GF.context.set('actor',data);
              // GFActor=data;
              commit("SET_USERIFNO", data);
              resolve(data);
            })
            .catch((err) => {
              reject(err);
            });
        });
      },

    },
    mutations: {
      SET_TOKEN: (state, token) => {
        state.token = token;
        window.GFActor.token=state.token;
      },
      SET_MENUID(state, menuId) {
        state.menuId = menuId;
        window.GF.context.set('menuId', state.menuId);
      },
      SET_USERIFNO: (state, userInfo) => {
          state.actor = userInfo;
          window.GFActor=userInfo;
      },
      SET_PERMISSION: (state, permission) => {
        state.permission = {};
        permission.forEach((ele) => {
          state.permission[ele] = true;
        });
      },
    },
    getters: {
      userInfo: (state) => state.userInfo,
      token: (state) => state.token,
      permission: (state) => state.permission,
      menuId: (state) => state.menuId,
      menu: (state) =>  state.menu,
      menuAll: (state) => state.menuAll,
      sysRoles: (state) => getLists(state, 'roles', 'setSysRoles'),
    }
  }

