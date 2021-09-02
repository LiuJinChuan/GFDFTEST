import { validatenull } from "./validate";
//表单序列化
export const serialize = (data) => {
  let list = [];
  Object.keys(data).forEach((ele) => {
    list.push(`${ele}=${data[ele]}`);
  });
  return list.join("&");
};
//TODO:此方法和typeof近似，建议合并 | typeof 只能区别基本类型的数据类型,引用类型全部都是Object,不适用这里的判断
export const getObjType = (obj) => {
  var toString = Object.prototype.toString;
  var map = {
    "[object Boolean]": "boolean",
    "[object Number]": "number",
    "[object String]": "string",
    "[object Function]": "function",
    "[object Array]": "array",
    "[object Date]": "date",
    "[object RegExp]": "regExp",
    "[object Undefined]": "undefined",
    "[object Null]": "null",
    "[object Object]": "object",
  };
  if (obj instanceof Element) {
    return "element";
  }

  return map[toString.call(obj)];
};
export const viewDom = () => {
  return window.document
    .getElementById("avue-view")
    .getElementsByClassName("el-scrollbar__wrap")[0];
};
/**
 * 对象深拷贝
 */
export const deepClone = (data) => {
  var type = getObjType(data);
  var obj;
  if (type === "array") {
    obj = [];
  } else if (type === "object") {
    obj = {};
  } else {
    //不再具有下一层次
    return data;
  }
  if (type === "array") {
    for (var i = 0, len = data.length; i < len; i++) {
      obj.push(deepClone(data[i]));
    }
  } else if (type === "object") {
    for (var key in data) {
      obj[key] = deepClone(data[key]);
    }
  }
  return obj;
};
/**
 * 设置灰度模式
 */
export const toggleGrayMode = (status) => {
  if (status) {
    document.body.className = document.body.className + " grayMode";
  } else {
    document.body.className = document.body.className.replace(" grayMode", "");
  }
};
/**
 * 设置主题
 */
export const setTheme = (name) => {
  document.body.className = name;
};

/**
 * 加密处理
 */
export const encryption = (params) => {
  let { data, type, param, key } = params;
  let result = JSON.parse(JSON.stringify(data));
  if (type == "Base64") {
    param.forEach((ele) => {
      result[ele] = btoa(result[ele]);
    });
  } else if (type == "Aes") {
    param.forEach((ele) => {
      result[ele] = window.CryptoJS.AES.encrypt(result[ele], key).toString();
    });
  }
  return result;
};

/**
 * 浏览器判断是否全屏
 */
export const fullscreenToggel = () => {
  if (fullscreenEnable()) {
    exitFullScreen();
  } else {
    reqFullScreen();
  }
};
/**
 * esc监听全屏
 */
export const listenfullscreen = (callback) => {
  function listen() {
    callback();
  }

  document.addEventListener("fullscreenchange", function() {
    listen();
  });
  document.addEventListener("mozfullscreenchange", function() {
    listen();
  });
  document.addEventListener("webkitfullscreenchange", function() {
    listen();
  });
  document.addEventListener("msfullscreenchange", function() {
    listen();
  });
};
/**
 * 浏览器判断是否全屏
 */
export const fullscreenEnable = () => {
  var isFullscreen =
    document.isFullScreen ||
    document.mozIsFullScreen ||
    document.webkitIsFullScreen;
  return isFullscreen;
};

/**
 * 浏览器全屏
 */
export const reqFullScreen = () => {
  if (document.documentElement.requestFullScreen) {
    document.documentElement.requestFullScreen();
  } else if (document.documentElement.webkitRequestFullScreen) {
    document.documentElement.webkitRequestFullScreen();
  } else if (document.documentElement.mozRequestFullScreen) {
    document.documentElement.mozRequestFullScreen();
  }
};
/**
 * 浏览器退出全屏
 */
export const exitFullScreen = () => {
  if (document.documentElement.requestFullScreen) {
    document.exitFullScreen();
  } else if (document.documentElement.webkitRequestFullScreen) {
    document.webkitCancelFullScreen();
  } else if (document.documentElement.mozRequestFullScreen) {
    document.mozCancelFullScreen();
  }
};
/**
 * 递归寻找子类的父类
 */

export const findParent = (menu, id) => {
  for (let i = 0; i < menu.length; i++) {
    if (menu[i].children.length != 0) {
      for (let j = 0; j < menu[i].children.length; j++) {
        if (menu[i].children[j].id == id) {
          return menu[i];
        } else {
          if (menu[i].children[j].children.length != 0) {
            return findParent(menu[i].children[j].children, id);
          }
        }
      }
    }
  }
};
/**
 * 判断2个对象属性和值是否相等
 */

/**
 * 动态插入css
 */

export const loadStyle = (url) => {
  const link = document.createElement("link");
  link.type = "text/css";
  link.rel = "stylesheet";
  link.href = url;
  const head = document.getElementsByTagName("head")[0];
  head.appendChild(link);
};
/**
 * 判断路由是否相等
 */
export const diff = (obj1, obj2) => {
  delete obj1.close;
  var o1 = obj1 instanceof Object;
  var o2 = obj2 instanceof Object;
  if (!o1 || !o2) {
    /*  判断不是对象  */
    return obj1 === obj2;
  }

  if (Object.keys(obj1).length !== Object.keys(obj2).length) {
    return false;
    //Object.keys() 返回一个由对象的自身可枚举属性(key值)组成的数组,例如：数组返回下表：let arr = ["a", "b", "c"];console.log(Object.keys(arr))->0,1,2;
  }

  for (var attr in obj1) {
    var t1 = obj1[attr] instanceof Object;
    var t2 = obj2[attr] instanceof Object;
    if (t1 && t2) {
      return diff(obj1[attr], obj2[attr]);
    } else if (obj1[attr] !== obj2[attr]) {
      return false;
    }
  }
  return true;
};
/**
 * 根据字典的value显示label
 */
export const findByvalue = (dic, value) => {
  let result = "";
  if (validatenull(dic)) return value;
  if (
    typeof value == "string" ||
    typeof value == "number" ||
    typeof value == "boolean"
  ) {
    let index = 0;
    index = findArray(dic, value);
    if (index != -1) {
      result = dic[index].label;
    } else {
      result = value;
    }
  } else if (value instanceof Array) {
    result = [];
    let index = 0;
    value.forEach((ele) => {
      index = findArray(dic, ele);
      if (index != -1) {
        result.push(dic[index].label);
      } else {
        result.push(value);
      }
    });
    result = result.toString();
  }
  return result;
};
/**
 * 根据字典的value查找对应的index
 */
export const findArray = (dic, value) => {
  for (let i = 0; i < dic.length; i++) {
    if (dic[i].value == value) {
      return i;
    }
  }
  return -1;
};
/**
 * 生成随机len位数字
 */
export const randomLenNum = (len, date) => {
  let random = "";
  random = Math.ceil(Math.random() * 100000000000000)
    .toString()
    .substr(0, len ? len : 4);
  if (date) random = random + Date.now();
  return random;
};

/**
 * 打开小窗口
 */
export const openWindow = (url, title, w, h) => {
  // Fixes dual-screen position                            Most browsers       Firefox
  const dualScreenLeft =
    window.screenLeft !== undefined ? window.screenLeft : screen.left;
  const dualScreenTop =
    window.screenTop !== undefined ? window.screenTop : screen.top;

  const width = window.innerWidth
    ? window.innerWidth
    : document.documentElement.clientWidth
    ? document.documentElement.clientWidth
    : screen.width;
  const height = window.innerHeight
    ? window.innerHeight
    : document.documentElement.clientHeight
    ? document.documentElement.clientHeight
    : screen.height;

  const left = width / 2 - w / 2 + dualScreenLeft;
  const top = height / 2 - h / 2 + dualScreenTop;
  const newWindow = window.open(
    url,
    title,
    "toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=no, resizable=yes, copyhistory=no, width=" +
      w +
      ", height=" +
      h +
      ", top=" +
      top +
      ", left=" +
      left
  );

  // Puts focus on the newWindow
  if (window.focus) {
    newWindow.focus();
  }
};

/**
 * 列表转树
 */
export function buildListToTree(
  list,
  parentId = "pid",
  Id = "id",
  rootId = null,
  children = "children"
) {
  var arr = [];

  if (list == null || list.length == 0) {
    return;
  }

  //对象深拷贝
  list.forEach((ele) => {
    arr.push(JSON.parse(JSON.stringify(ele)));
  });

  let rootList = arr.filter((item) => {
    if (item[parentId] == rootId) {
      return item;
    }
  });
  function listToTreeData(bootItem, arr, parentId, Id, l) {
    bootItem[children] = [];
    arr.forEach((item) => {
      if (item[parentId] == bootItem[Id]) {
        item.clevel = l;
        bootItem[children].push(item);
      }
    });

    if (bootItem[children].length > 0) {
      bootItem[children].forEach((item) => {
        let j = item.clevel + 1;
        listToTreeData(item, arr, parentId, Id, j);
      });
    } else {
      return false;
    }
  }

  rootList.map((rootItem) => {
    rootItem.clevel = 0;
    let l = rootItem.clevel + 1;
    listToTreeData(rootItem, arr, parentId, Id, l);
    return rootItem;
  });

  return rootList;
}

/**
 *列表转对象
 */
export function listToObj(list, key = "index", value = "child") {
  if (list == undefined || list.length == 0) {
    return {};
  }

  let obj = {};
  list.forEach((ele) => {
    const keys = ele[`${key}`];
    obj[keys] = ele[`${value}`];
  });
  obj = JSON.parse(JSON.stringify(obj)); //?
  return obj;
}

/**
 * Parse the time to string
 * @param {(Object|string|number)} time
 * @param {string} cFormat
 * @returns {string}
 */
export function parseTime(time, cFormat) {
  if (arguments.length === 0) {
    return null;
  }
  const format = cFormat || "{y}-{m}-{d} {h}:{i}:{s}";
  let date;
  if (typeof time === "object") {
    date = time;
  } else {
    if (typeof time === "string" && /^[0-9]+$/.test(time)) {
      time = parseInt(time);
    }
    if (typeof time === "number" && time.toString().length === 10) {
      time = time * 1000;
    }
    date = new Date(time);
  }

  const formatObj = {
    y: date.getFullYear(),
    m: date.getMonth() + 1,
    d: date.getDate(),
    h: date.getHours(),
    i: date.getMinutes(),
    s: date.getSeconds(),
    a: date.getDay(),
  };
  const time_str = format.replace(/{(y|m|d|h|i|s|a)+}/g, (result, key) => {
    let value = formatObj[key];
    // Note: getDay() returns 0 on Sunday
    if (key === "a") {
      return ["日", "一", "二", "三", "四", "五", "六"][value];
    }
    if (result.length > 0 && value < 10) {
      value = "0" + value;
    }
    return value || 0;
  });
  return time_str;
}

/**
 * 去重获取对象属性数据
 * @param {Array} arr
 * @param {String} key
 * @returns {Array}
 */
export function arrayUnique(arr, key) {
  var hash = {};
  return arr.reduce(function(item, next) {
    hash[next[key]] ? "" : (hash[next[key]] = true && item.push(next));
    return item;
  }, []);
}

/**
 * 获取几天前的代码
 * @param {Number} n
 * @returns {string}
 */
export function getTime(n) {
  var now = new Date();
  var year = now.getFullYear();
  //因为月份是从0开始的,所以获取这个月的月份数要加1才行
  var month = now.getMonth() + 1;
  var date = now.getDate();
  var day = now.getDay();
  //判断是否为周日,如果不是的话,就让今天的day-1(例如星期二就是2-1)
  if (day !== 0) {
    n = n + (day - 1);
  } else {
    n = n + day;
  }
  if (day) {
    //这个判断是为了解决跨年的问题,月份是从0开始的
    if (month <= 1) {
      year = year - 1;
      month = 12;
    }
  }
  now.setDate(now.getDate() - n);
  year = now.getFullYear();
  month = now.getMonth() + 1;
  date = now.getDate();
  var s =
    year +
    "-" +
    (month < 10 ? "0" + month : month) +
    "-" +
    (date < 10 ? "0" + date : date);
  return s;
}

/**
 * 返回范围内的时间
 * @param {String} start
 * @param {String} end
 * @returns {Array}
 */
export function getRangeDate(start, end) {
  let dateList = [];
  var startTime = new Date(start);
  var endTime = new Date(end);
  while (endTime.getTime() - startTime.getTime() >= 0) {
    var year = startTime.getFullYear();
    var month =
      startTime.getMonth() + 1 < 10
        ? "0" + (startTime.getMonth() + 1)
        : startTime.getMonth() + 1;
    var day =
      startTime.getDate().toString().length == 1
        ? "0" + startTime.getDate()
        : startTime.getDate();
    dateList.push(year + "-" + month + "-" + day);
    startTime.setDate(startTime.getDate() + 1);
  }
  return dateList;
}

/**
 * 获取子级
 * @param {选中树数据} data
 * @param {获取节点属性} nodecode
 * @param {是否有数据节点判断} ishasbfnode
 * @param {数据节点判断属性} bfnodecode
 * @param {子级属性} childcode
 * @returns
 */
export function getChildCodeArrBf(
  data,
  nodecode,
  ishasbfnode = false,
  bfnodecode = "id",
  isparent = false,
  childcode = "children"
) {
  let arr = [];
  let isb = false;
  function getSq(ex) {
    for (let i = 0; i < ex.length; i++) {
      if (isparent) arr.push(ex[i][nodecode]);
      if (ex[i][childcode] && ex[i][childcode].length > 0)
        getSq(ex[i][childcode]);
      else {
        if (ishasbfnode) {
          if (ex[i][bfnodecode]) arr.push(ex[i][nodecode]);
        } else arr.push(ex[i][nodecode]);
      }
    }
  }
  if (data[childcode] && data[childcode].length > 0) {
    getSq(data[childcode]);
    isb = false;
  } else {
    if (ishasbfnode) {
      if (data[bfnodecode]) {
        arr.push(data[nodecode]);
        isb = true;
      } else isb = false;
    } else {
      arr.push(data[nodecode]);
      isb = true;
    }
  }
  return { arr, isb };
}

/**
 * 分组
 * @param {数据} arr
 * @param {键} key
 * @returns
 */
export function ItemGroupBy(arr, key) {
  let types = {},
    i,
    j,
    cur;
  for (i = 0, j = arr.length; i < j; i++) {
    cur = arr[i];
    if (!(cur[key] in types)) {
      types[cur[key]] = [];
    }
    types[cur[key]].push(cur);
  }
  return types;
}
