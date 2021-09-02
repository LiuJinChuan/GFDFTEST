/**
 * 全局配置文件
 */
// import '../../../public/gf'
export default {
  title: window.GF.cfg.proj.name,
  logo: "GF",
  indexTitle: window.GF.cfg.proj.name,
  lockPage: '/lock',
  tokenTime: 12*60*60,//token过期时间,单位秒,传0则永久有效
  Authorization: 'Authorization',
  //http的status默认放行不才用统一处理的,
  statusWhiteList: [400],
  //配置首页不可关闭
  isFirstPage: false,
  fistPage: {
    label: "首页",
    value: "/wel/index",
    params: {},
    query: {},
    meta: {
      i18n: 'dashboard'
    },
    group: [],
    close: false
  },
  //配置菜单的属性
  menu: {
    iconDefault: 'icon-caidan',
    props: {
      label: 'label',
      path: 'path',
      icon: 'icon',
      children: 'children'
    }
  }
}