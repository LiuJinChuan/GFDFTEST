/**
 * 全站权限配置
 *
 */

import router from "@/framework/router/router";
//import store from "@/framework/store";
import { validatenull } from "@/framework/util/validate";
import NProgress from "nprogress"; // progress bar
import "nprogress/nprogress.css"; // progress bar style
let store = window.GF.store;
NProgress.configure({ showSpinner: false });
const lockPage = store.getters.website.lockPage; //锁屏页
router.beforeEach((to, from, next) => {
  const meta = to.meta || {};
  let bToken =  validatenull(window.GF.context.get('token'));   //取GF里面的token 暂时没取到 用缓存里面的先代替,再处理
  if (!bToken) {
    if (store.getters.isLock && to.path != lockPage) {
      //如果系统激活锁屏，全部跳转到锁屏页
      next({ path: lockPage });
    } else if (to.path === "/login") {
      //如果登录成功访问登录页跳转到主页
      next({ path: "/" });
    } else {
      //如果用户信息为空则获取用户信息，获取用户信息失败，跳转到登录页
      if (JSON.stringify(window.GF.context.actor)=='{}') {
        store.dispatch("GetUserInfo")
          .then(() => {
            next({ ...to, replace: true });
          })
          .catch(() => {
            // GF.emitter.emit('gf-logout')
            // store.dispatch("FedLogOut").then(() => {
            //   next({ path: "/login" });
            // });
          });
      }
      else {
        const value = to.query.src || to.fullPath;
        const label = to.query.name || to.name;
        const meta = to.meta || router.$avueRouter.meta || {};
        const i18n = to.query.i18n;

        if (meta.isTab !== false && !validatenull(value) && !validatenull(label)) {
          store.commit("ADD_TAG", {
            label: label,
            value: value,
            params: to.params,
            query: to.query,
            meta: (() => {
              if (!i18n) {
                return meta;
              }
              return {
                i18n: i18n,
              };
            })(),
            group: router.$avueRouter.group || [],
          });
        }
        next();
      }
    }
  }   else {
    //判断是否需要认证，没有登录访问去登录页
    if (meta.isAuth === false) {
      next();
    } else {
      next("/login");
    }
  }
});
router.afterEach(() => {
  NProgress.done();
  let title = store.getters.tag.label;
  let i18n = store.getters.tag.meta.i18n;
  title = router.$avueRouter.generateTitle(title, i18n);
  //根据当前的标签也获取label的值动态设置浏览器标题
  router.$avueRouter.setTitle(title);
});
