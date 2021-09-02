import mitt from 'mitt'

window.GF = top.window.GF || {
    //TODO: imgurl参数类放到cfg对象内。
    version: "0.1.0",
    state: 0,
    cfg: {
        domain: "localhost:8081",
        domainprot: "http://",
        websocketprot: "ws://",
        websockets: false,
        proj: { bno: "GF-EDU001", name: "谷夫SAAS管理后台" },
        cache_pre: "gf"
    },
    emitter: mitt(), //事件Hub
    logger: {
        DEBUG: 0,
        INFO: 1,
        WARN: 2,
        ERROR: 3,
        level: 3,
        methodMap: { 0: 'debug', 1: 'info', 2: 'warn', 3: 'error' },
        log: function (obj, level = 1) {
            if (this.level <= level) {
                var method = this.methodMap[level];
                if (typeof console !== 'undefined' && console[method]) {
                    console[method].call(console, obj);
                }
                //if(level==3){GF.emitter.emit("error",JSON.stringify(obj));};
            }
        }
    }, //日志记录器
    cache: {
        get: function (key) {
            const gfkey = GF.cfg.cache_pre + '-' + key;
            let obj = localStorage.getItem(gfkey);
            if (!obj) return;
            let { value, expired } = JSON.parse(obj);
            const now = new Date().getTime();
            if (now >= expired) {
                this.del(key);
                return;
            }
            return value;
        },
        set: function (key, value, expires = 31536000000) {
            if (!key || !value) return;
            const gfkey = GF.cfg.cache_pre + '-' + key;
            const expired = new Date().getTime() + expires;
            localStorage.setItem(gfkey, JSON.stringify({ value, expired }));
        },
        del: function (key) {
            const gfkey = GF.cfg.cache_pre + '-' + key;
            localStorage.removeItem(gfkey);
        },
        clear: function () { localStorage.clear(); },
    },
    context: {
        get: function (key) {
            let obj = sessionStorage.getItem(key);
            return obj ? JSON.parse(obj) : {};
        },
        set: function (key, value) {
            if (!key || !value) return;
            sessionStorage.setItem(key, JSON.stringify(value));
        },
        del: function (key) {
            sessionStorage.removeItem(key);
        },
        clear: function () { sessionStorage.clear(); },
        actor: {}
    },
    socket: {},
    dicts: {},
    regions: [],
    regiontree: [],
    partials: {},
    helpers: {}, //帮助方法  
    utils: {
        isEmpty: function (value) {
            return (!value && value !== 0) || (toString.call(value) === "[object Array]" && value.length === 0);
        }
        /**
         * Merge an Array of Objects into a single Object.
         */
        ,
        toObject: function (arr) {
            var res = {};
            Array.from(arr).forEach((value) => {
                Object.assign(res, value);
            });
            return res
        },
        noop: function (...arg) { }
        /**
         * Ensure a function is called only once.
         */
        ,
        once: (fn) => {
            var called = false;
            return function () {
                if (!called) {
                    called = true;
                    fn.apply(this, arguments);
                }
            }
        },
        emptyObject: Object.freeze({}),
        if_or: function (condition, options) {
            if (condition)
                return options.fn(this);
            return options.inverse(this);
        },
        typeof(obj) {
            return toString.call(obj).substr(8).replace("]", "").toLowerCase()
        },
        buildListToTree(list, parentId = "pid", Id = "id", rootId = null, children = "children") {
            if (list.length == 0) return;
            let arr = [];
            //对象深拷贝
            list.forEach((ele) => {
                arr.push(JSON.parse(JSON.stringify(ele)));
            });

            let rootList = arr.filter((item) => {
                if (item[parentId] == rootId) {
                    return item;
                }
            });

            function listToTreeData(bootItem, arr, parentId, Id) {
                bootItem[children] = [];
                arr.forEach((item) => {
                    if (item[parentId] == bootItem[Id]) {
                        bootItem[children].push(item);
                    }
                });
                if (bootItem[children].length <= 0) return false;
                bootItem[children].forEach((item) => {
                    listToTreeData(item, arr, parentId, Id);
                });
            }

            rootList.map((rootItem) => {
                listToTreeData(rootItem, arr, parentId, Id);
                return rootItem;
            });

            return rootList;
        },
        parseTime(time, cFormat) {
            if (arguments.length === 0) {
                return null
            }
            const format = cFormat || '{y}-{m}-{d} {h}:{i}:{s}'
            let date
            if (typeof time === 'object') {
                date = time
            } else {
                if ((typeof time === 'string') && (/^[0-9]+$/.test(time))) {
                    time = parseInt(time)
                }
                if ((typeof time === 'number') && (time.toString().length === 10)) {
                    time = time * 1000
                }
                date = new Date(time)
            }
            const formatObj = {
                y: date.getFullYear(),
                m: date.getMonth() + 1,
                d: date.getDate(),
                h: date.getHours(),
                i: date.getMinutes(),
                s: date.getSeconds(),
                a: date.getDay()
            }
            const time_str = format.replace(/{(y|m|d|h|i|s|a)+}/g, (result, key) => {
                let value = formatObj[key]
                // Note: getDay() returns 0 on Sunday
                if (key === 'a') {
                    return ['日', '一', '二', '三', '四', '五', '六'][value]
                }
                if (result.length > 0 && value < 10) {
                    value = '0' + value
                }
                return value || 0
            })
            return time_str
        }
        //两个数组去重union, fn为返回判断去重的key(字符串)。使用Map
        ,
        union: (arr1, arr2, fn) => {
            let set1 = new Map();
            Array.of(...arr1, ...arr2).forEach(item => {
                set1.set(fn(item), item);
            });
            return [...set1.values()];
        },
        pipe: (obj, value) => {
            var funcStack = [];
            var oproxy = new Proxy(obj, {
                get: function (pipeObject, fnName) {
                    if (fnName === 'get') {
                        return funcStack.reduce((val, fn) => fn(val), value);
                    }
                    funcStack.push(pipeObject[fnName]);
                    return oproxy;
                }
            });
            return oproxy;
        },
        formatMenus: (arr) => {
            var menusRes = arr.map((ele) => {
                var obj = {
                    label: ele.cname,
                    icon: ele.icon || "icon-caidan",
                    path: ele.link,
                    id: ele.id,
                    pid: ele.pid,
                    seqno: ele.seqno,
                    hide: ele.flag == 1,
                    meta: {
                        keepAlive: ele.flag == 0,
                        hide: ele.flag == 1,
                    }
                };
                if (ele.ext) {
                    obj.query = JSON.parse(ele.ext);
                    obj.path = ele.link + GF.utils.objToPath(obj.query);
                }
                if (ele.component) {
                    obj.component = ele.component;
                }
                return obj;
            });
            return menusRes;
        },
        objToPath: (object) => {
            var str = "";
            for (const key in object) {
                str += "/" + object[key];
            }
            return str;
        }
    },
    log: (obj, level = 3) => { GF.logger.log(obj, level); },
    registerHelper(name, fn) {
        if (toString.call(name) === '[object Object]') {
            throw 'Arg not supported with multiple helpers';
        } else {
            this.helpers[name] = fn;
        }
        return this;
    },
    registerPartial(name, str) {
        if (toString.call(name) === '[object Object]') {
            Object.assign(this.partials, name);
        } else {
            this.partials[name] = str;
        }
        return this;
    },
    init() {
        this.emitter.on("gferror", e => {
            //附加环境信息，推送到服务器端， 达到采集客户端错误信息
            let obj = { actor: (GFActor.user.Id || "anonymous"), eventdata: e };
            //GF.log("ERR:"+obj.actor+obj.eventdata);
        });
        this.logger.level = 0;
        this.state += 1;
        this.emitter.emit("gfinit");
    }
};

/*挂载各模块的sysinit, 以便接收 gfinit事件
this.emitter.on("gfinit",()=>{})
*/