/**
 * @baseURL //不建议在此写全地址，而是写目标代理“^/pai”，以便多个代理的配置
 * 
 */


export default {
  baseURL:process.env.VUE_APP_BASE_API,  // --mode环境变量中配置 VUE_APP_BASE_API
  routes:[]//无权限限制页面
}