//import store from '@/framework/store'
import { SysStore } from "./sysStore";
import { UserStore } from "./userStore";

//注册utils

//GF初始化后挂载模块所属Store
window.GF.emitter.on("gfinit", () => {
  window.GF.store.registerModule(["system"], SysStore);
  window.GF.store.registerModule(["user"], UserStore);
});

//登录成功
window.GF.emitter.on("gf-logged", () => {});

/*登录后获取*/
window.GF.emitter.on("gf-index-created", () => {
  if (JSON.stringify(window.GF.dicts) == "{}") {
    window.GFAPI.sys.DictApi.load().then((res) => {
      let dicts = {};
      let datas = window.GF.utils.buildListToTree(res.data, "pid", "id", 0);
      datas.forEach((item) => {
        if (item.children.length > 0) {
          dicts[item.callcode] = item.children.map((e) => {
            return { label: e.cname, value: e.cvalue, ext: e };
          });
        }
      });
      window.GF.dicts = dicts;
    });
  }
  if (JSON.stringify(window.GF.regions) == "[]") {
    window.GFAPI.sys.RegionApi.search({ order: [{ code: "asc" }] }).then(
      (res) => {
        if (res.code == 0) {
          let regions = window.GF.utils.buildListToTree(
            res.data,
            "jparentcode",
            "jcode",
            "0"
          );
          window.GF.regiontree = regions;
          window.GF.regions = res.data;
        }
      }
    );
  }
});

//退出登录
window.GF.emitter.on("gf-logout", () => {});
