import * as s from "../framework/util/util";
//import store from '@/framework/store'
import { CommonStore } from "./store/commonStore";
import { TagsStore } from "./store/tagsStore";
import { LogsStore } from "./store/logsStore";

//挂载util
Object.assign(window.GF.utils, s);

//GF初始化后执行注册APIS
window.GF.emitter.on("gfinit", () => {
  Promise.all([
    import("./api/sys.js"),
    import("./api/dev.js"),
    import("./api/biz.js"),
  ]).then((res) => {
    let apis = { sys: res[0], dev: res[1], biz: res[2] };
    window.GF.registerPartial({ api: apis });
    window.GFAPI = window.GFAPI || window.GF.partials.api;
    window.GF.emitter.emit("gf-apiloaded");
  });
});

//GF初始化后执行注册store
window.GF.emitter.on("gfinit", () => {
  window.GF.store.registerModule(["common"], CommonStore);
  window.GF.store.registerModule(["tags"], TagsStore);
  window.GF.store.registerModule(["logs"], LogsStore);
  window.GF.emitter.emit("gf-storereged");
});

//TO修改GFActor后出现了问题,先放一下,让业务能走下去

//GF初始化后 创建GFActor
window.GF.emitter.on("gfinit", () => {
  window.GF.context.actor = window.GF.context.get("actor") || {};
  window.GFActor = new Proxy(window.GF.context.actor, {
    set(target, key, value) {
      let bSucc = Reflect.set(target, key, value);
      // if (bSucc) {
      window.GF.context.set("actor", window.GF.context.actor);

      return bSucc;
      // } else {
      //   //测试一下是否需要此throw
      //   throw Error("GFActor为只读类型,请勿修改!")
      // }
    },
  });
  // Reflect.defineProperty(window, "GFActor", { writable: false });
});

//GF api加载后
window.GF.emitter.on("gf-apiloaded", () => {
  //为什么要做此判断，  是接口调用必须要token？ 一是有单独token获取，二是在gfinit时已经有GFActor，三可放到indexcreated确保登陆时也行
  if (!(JSON.stringify(window.GF.context.get("actor")) == "{}")) {
    window.GFAPI.sys.DictApi.load()
      .then((res) => {
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
      })
      .finally(() => {
        window.GF.emitter.emit("gf-dictsloaded");
      });
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
  } else {
    window.GF.emitter.emit("gf-dictsloaded");
  }
});

//GF store加载后建立路由,并在路由上加权限
window.GF.emitter.on("gf-storereged", () => {
  import("@/framework/router/router").then((router) => {
    window.GF.router = router.default;
    let Router = window.GF.router.$avueRouter.formatRoutes(
      window.GFActor.menus || [],
      true
    );
    window.GF.router.addRoutes(Router);
  });
  import("@/permission"); // 权限
});

/*登录后获取  gf-index-created进入后保证token已存在，且前
 */
window.GF.emitter.on("gf-index-created", () => {
  window.GF.context.actor = window.GFActor;
});

//登录成功
window.GF.emitter.on("gf-logged", () => {
  window.GF.store.commit("DEL_ALL_TAG");
  window.GF.store.commit("CLEAR_LOCK");
});

//退出登录
window.GF.emitter.on("gf-logout", () => {
  try {
    window.GF.context.del("token");
    window.GF.context.del("actor");
    window.GF.context.del("menuId");
    window.GF.cache.del("tagList");
    window.GF.cache.del("tag");
    window.GF.cache.del("themeName");
    window.GF.store.commit("CLEAR_LOCK");
  } catch (error) {
    console.log(error, 1);
  }
});
