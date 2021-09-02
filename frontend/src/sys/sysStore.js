import { getLists, getdata } from '@/framework/store'
import Vue from "vue";
function sort(data) {
  var finlly = data.sort((a, b) => {
    return a.seqno - b.seqno;
  });
  return finlly;
}

export const SysStore = {
  state: {
    jobs: null,
    permissions: null,
    dicts: null
  },
  mutations: {
    SET_SYSDICTS: (state, dicts) => {
      state.dicts = dicts
    },
  },
  actions: {

    setSysJobs({ commit }) {
      return new Promise((resolve, reject) => {

        window.GFAPI.sys.JobApi.load()
          .then((res) => {
            const data = res.data;
            data.forEach((element) => {
              if (!element.deptid) Vue.set(element, "deptid", null);
            });
            commit("SET_SYS_JOBS", sort(data));
            resolve(data);
          })
          .catch((error) => {
            reject(error);
          });
      });
    },

    setSysPermissions({ commit }) {
      return new Promise((resolve, reject) => {

        window.GFAPI.sys.PermisApi.load()
          .then((res) => {
            const data = res.data;
            data.forEach((element) => {
              if (!element.pid) Vue.set(element, "pid", null);
            });
            commit("SET_SYS_PERMISSIONS", sort(data));
            resolve(data);
          })
          .catch((error) => {
            reject(error);
          });
      });
    },
    setSysDicts({ commit }) {
      return new Promise((resolve) => {
        // window.GFAPI.sys.DictApi.load().then((res) => {
        //   let dicts = window.GF.utils.buildListToTree(res.data, 'pid', 'id', 0);
        //   commit('SET_SYSDICTS', dicts)
        //   resolve()
        // })
        commit('SET_SYSDICTS', [])
        resolve()
      })
    }
  },
  getters: {
    sysMenus: (state) => getLists(state, "menus", "setSysMenus"),
    sysDepts: (state) => getLists(state, 'depts', 'setSysDepts'),
    sysDicts: (state) => getLists(state, 'dicts', 'setSysDicts'),
    sysMenuAsync: (state) => getdata(state, "menus", "setSysMenus"),
    sysDeptsAsync: (state) => getdata(state, "depts", "setSysDepts"),
    sysDictsAsync: (state) => getdata(state, "dicts", "setSysDicts"),
  }
}
