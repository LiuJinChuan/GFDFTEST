/**
 * 全站路由配置
 *
 * meta参数说明
 * keepAlive是否缓冲页面
 * isTab是否加入到tag导航
 * isAuth是否需要授权
 */
import Vue from 'vue';
import VueRouter from 'vue-router';
import PageRouter from './page/'
import ViewsRouter from './views/'
//import AvueRouter from './avue-router';
import i18n from '@/framework/lang' // Internationalization
import Store from '../store/';
Vue.use(VueRouter);
//创建路由
export const createRouter = () => new VueRouter({
  scrollBehavior(to, from, savedPosition) {
    if (savedPosition) {
      return savedPosition
    } else {
      if (from.meta.keepAlive) {
        from.meta.savedPosition = document.body.scrollTop
      }
      return {
        x: 0,
        y: to.meta.savedPosition || 0
      }
    }
  },
  routes: [...PageRouter, ...ViewsRouter]
});
const Router = createRouter();


let RouterPlugin = function () {
  this.$router = null;
  this.$store = null;
};
RouterPlugin.install = function (vue, option = {}) {
  this.$router = option.router;
  this.$store = option.store;
  this.$vue = new vue({ i18n: option.i18n });
  function isURL(s) {
    if (s.includes('html')) return true;
    return /^http[s]?:\/\/.*/.test(s)
  }
  function objToform(obj) {
    let result = [];
    Object.keys(obj).forEach(ele => {
      result.push(`${ele}=${obj[ele]}`);
    })
    return result.join('&');
  }
  this.$router.$avueRouter = {
    //全局配置
    $website: window.GF.store.state.common.website,
    routerList: [],
    group: '',
    meta: {},
    safe: this,
    // 设置标题
    setTitle: (title) => {
      const defaultTitle = this.$vue.$t('title');
      title = title ? `${title}——${defaultTitle}` : defaultTitle;
      document.title = title;
    },
    // 关闭标题
    closeTag: (value) => {
      let tag = value || this.$store.getters.tag;
      if (typeof value === 'string') {
        tag = this.$store.getters.tagList.filter(ele => ele.value === value)[0]
      }
      this.$store.commit('DEL_TAG', tag)
    },
    generateTitle: (title, key) => {
      if (!key) return title;
      const hasKey = this.$vue.$te('route.' + key)
      if (hasKey) {
        // $t :this method from vue-i18n, inject in @/lang/index.js
        const translatedTitle = this.$vue.$t('route.' + key)

        return translatedTitle
      }
      return title
    },
    //处理路由
    getPath: function (params = {}, meta = {}) {
      let { src } = params;
      let result = src || '/';
      if (isURL(src)) {
        result = `/myiframe/urlPath?${objToform(Object.assign(meta, params))}`;
      }
      return result;
    },
    //正则处理路由
    vaildPath: function (list, path) {
      let result = false;
      list.forEach(ele => {
        if (new RegExp("^" + ele + ".*", "g").test(path)) {
          result = true
        }
      })
      return result;
    },
    //设置路由值
    getValue: function (route) {
      let value = "";
      if (route.query.src) {
        value = route.query.src;
      } else {
        value = route.path;
      }
      return value;
    },
    //动态路由
    formatRoutes: function (aMenu = [], first) {
      const aRouter = [];
      if (aMenu.length === 0) return aRouter;
      for (let i = 0; i < aMenu.length; i++) {
        const oMenu = aMenu[i];

        if (this.routerList.includes(oMenu.path)) return;

        let path = oMenu.path,
          component = oMenu.component,
          name = oMenu.label,
          icon = oMenu.icon,
          children = oMenu.children,
          meta = oMenu.meta || {};

        const isChild = children && children.length !== 0;
        const oRouter = {
          path: path,
          component(resolve) {
            // 判断是否为首路由
            if (first) {
              require(['../../framework/page/index'], resolve)
              return
              // 判断是否为多层路由
            } else if (isChild && !first) {
              require(['../../framework/page/index/layout'], resolve)
              return
              // 判断是否为最终的页面视图
            } else {
              require([`@/${component}.vue`], resolve)
            }
          },
          name: name,
          icon: icon,
          meta: meta,
          redirect: (() => {
            if (!isChild && first && !isURL(path)) return `${path}`
            else return '';
          })(),
          // 处理是否为一级路由
          children: !isChild ? (() => {
            if (first) {
              if (!isURL(path)) oMenu.path = `${path}`;
              return [{
                component(resolve) { require([`@/${component}.vue`], resolve) },
                icon: icon,
                name: name,
                meta: meta,
                path: ''
              }]
            }
            return [];
          })() : (() => {
            return this.formatRoutes(children, false)
          })()
        };
        aRouter.push(oRouter)
      }
      return aRouter
    }
  };
};
RouterPlugin.install(Vue, { router: Router, store: window.GF.store, i18n: i18n, keepAlive: true, });



//退出登录重置路由
export function resetRouter() {
  const newRouter = createRouter();
  Router.matcher = newRouter.matcher; // reset router
  RouterPlugin.install(Vue, { router: Router, store: Store, i18n: i18n, keepAlive: true, });
}

export default Router
