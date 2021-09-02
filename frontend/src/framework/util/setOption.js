async function getdict(dict) {
  if (!dict) return false;
  if (window.GF.utils.typeof(dict) === "function") {
    return Promise.resolve(dict.call(this));
  } else {
    return Promise.resolve(dict);
  }
}
//TODO: GF提供模块加载dict功能， 但要注意相互覆盖
export default function setOption(Table, localdict = {}, that) {
  Table.filter((eles) => eles.hasOwnProperty("dicDataType")).forEach((ele) => {
    let dicts =
      localdict[ele.dicDataCallCode] || window.GF.dicts[ele.dicDataCallCode];
    let dict = dicts;

    if (dict) {
      getdict(dict).then((res) => {
        setTimeout(() => {
          that.$nextTick(() => {
            let temp = that.$refs.scrud.option.column.find(
              (o) => o.prop == ele.prop
            );
            that.$set(temp, "dicData", res);
            // that.$refs.scrud.updateDic(ele.prop, res, () => {});
          });
        }, 500);
        ele.props = ele.props || { label: "label", value: "value" };
      });
      if (ele.rules) {
        for (let x = 0; x < ele.rules.length; x++) {
          if (ele.rules[x].hasOwnProperty("validator")) {
            ele.rules[x].validator = eval(ele.rules[x].validator);
          }
        }
      }
    }
  });

  // for (let i = 0; i < Table.length; i++) {
  //     if (ele.hasOwnProperty('dicDataType')) {
  //         if (ele.dicDataType == '1') {
  //             let dict = dicts[ele.dicDataCallCode];
  //             if (!dict) continue;
  //             dict(dict).then(res => {
  //                 Vue.set(ele, "dicData", res || []);
  //                 ele.props = ele.props || {label: 'label', value: 'value'};
  //             })
  //         }
  //     }
  //     if (ele.rules) {
  //         for (let x = 0; x < ele.rules.length; x++) {
  //             if (ele.rules[x].hasOwnProperty('validator')) {
  //                 ele.rules[x].validator = eval(
  //                     ele.rules[x].validator
  //                 );
  //             }
  //         }
  //     }
  // }
}
