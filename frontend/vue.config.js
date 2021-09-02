// 基础路径 注意发布之前要先修改这里
let baseUrl = "/";

module.exports = {
    publicPath: baseUrl,
    outputDir: "dist",
    configureWebpack() {
        return {
            entry: "./src/main.js",
        };
    },
    lintOnSave: true,
    productionSourceMap: false,
    devServer: {
        open: false,
        //public: '192.168.31.241:8080',
        port: '1234',
        proxy: {
            [process.env.VUE_APP_BASE_API]: {
                target: "http://localhost:8081/",
                changeOrigin: true,
                ws: true,
                pathRewrite: {
                    ["^" + process.env.VUE_APP_BASE_API]: "",
                }
            },
        },
    },
    chainWebpack: (config) => {
        //忽略的打包文件
        config.externals({
            vue: "Vue",
            "vue-router": "VueRouter",
            vuex: "Vuex",
            axios: "axios",
            "element-ui": "ELEMENT",
        });
        const entry = config.entry("app");
        entry.add("babel-polyfill").end();
        entry.add("classlist-polyfill").end();
        entry.add("@/mock").end();
    },
};