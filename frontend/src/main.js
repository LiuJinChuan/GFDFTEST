import '../public/gf.js';
//挂载各模块的系统初始化信息, 主要订阅消息
import './sys/sysinit';
import './framework/sysinit';
import './framework/websocket/index.js'
//vue项目挂载Vuex
window.GF.store = new Vuex.Store({});
//GF
void(window.GF.state || window.GF.init());
import Vue from 'vue';
import AvueMap from 'avue-plugin-map'
Vue.use(AvueMap);
// import './error'; // 日志
import './cache'; //页面缓冲
import Element from 'element-ui';
import i18n from '@/framework/lang' // Internationalization
import '@/framework/styles/common.scss';
import App from './App';
import Vuex from "vuex";
import AvueUeditor from 'avue-plugin-ueditor';
import VCharts from 'v-charts';
Vue.use(VCharts);


Vue.use(AvueUeditor);
window.GF.emitter.on('gf-dictsloaded', () => {
    Vue.use(Element, {
        i18n: (key, value) => i18n.t(key, value)
    })
    Vue.use(window.AVUE, {
            i18n: (key, value) => i18n.t(key, value)
        })
        // 加载相关url地址
        // Object.keys(urls).forEach(key => {
        //     Vue.prototype[key] = urls[key];
        // })
        // 动态加载阿里云字体库
        // iconfontVersion.forEach(ele => {
        //     GF.utils.loadStyle(iconfontUrl.replace('$key', ele));
        // })
    Vue.config.productionTip = false;
    Vue.prototype.$Event = new Vue();
    new Vue({
        router: window.GF.router,
        store: window.GF.store,
        i18n,
        render: h => h(App)
    }).$mount('#app')
})