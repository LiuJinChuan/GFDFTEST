<template>
  <div id="avue-view">
    <div class="head">
      <el-tabs class="tabBox">
        <el-tab-pane
          :label="item_top.label"
          v-for="(item_top, index) in list"
          class="moudleitem"
          :key="index"
        >
          <el-tabs
            tab-position="left"
            class="moudleitem-itemBox"
            type="border-card"
          >
            <el-tab-pane
              :label="item_left.cname"
              v-for="(item_left, index) in item_top.children"
              :key="index"
            >
              <formItem
                :option="item_left.option"
                :form="item_left.extobj"
                @submit="submit($event, item_left)"
              ></formItem>
            </el-tab-pane>
          </el-tabs>
        </el-tab-pane>
      </el-tabs>
    </div>
  </div>
</template>

<script>
import formItem from "./c_settting_form";
export default {
  name: "setting",
  components: { formItem },
  data() {
    return {
      aa: [],
      list: [],
    };
  },
  watch: {},
  computed: {},
  created() {
    this.getFormAndOption();
  },
  methods: {
    getFormAndOption() {
      Promise.all([
        window.GFAPI.sys.FormvalueApi.load(),
        window.GFAPI.sys.FormApi.search({
          order: [{ id: "desc" }],
          other: [{ flag: 1 }],
        }),
      ]).then((arrs) => {
        let Formvalue = [];
        arrs[1].data.forEach((o) => {
          let option = arrs[0].data.find((e) => e.formid == o.id);
          if (option) Formvalue.push(option);
        });
        Formvalue.forEach((ele) => {
          let option = arrs[1].data.find((e) => e.id == ele.formid);
          this.checkRoute(ele.callcode, option.extobj, ele.extobj);
          ele.option = option.extobj;
        });
        let temp = window.GF.dicts.module_type;
        temp.forEach((ele) => {
          ele.children = Formvalue.filter((e) => ele.value == e.module);
        });
        this.list = temp;
      });
    },
    checkRoute(callcode, option, form) {
      if (callcode === "notice_type") {
        form.noticeType.forEach((e) => {
          e.pcvalue ? e.pcvalue : (e.pcvalue = "");
        });
        let dic = JSON.parse(JSON.stringify(form.noticeType));
        dic = window.GF.utils.buildListToTree(dic, "pcvalue", "cvalue", "");
        dic.splice(0, 0, { cvalue: "", canem: "" });
        let parent = option.column[0].children.column.find(
          (ele) => ele.prop == "pcvalue"
        );
        this.$set(parent, "props", { value: "cvalue", label: "cname" });
        this.$set(parent, "dicData", dic);
      }
      if (callcode === "relation_type") {
        form.relationType.forEach((e) => {
          e.pcvalue ? e.pcvalue : (e.pcvalue = "");
        });
        let dic = JSON.parse(JSON.stringify(form.relationType));
        dic = window.GF.utils.buildListToTree(dic, "pcvalue", "cvalue", "");
        dic.splice(0, 0, { cvalue: "", canem: "" });
        let parent = option.column[0].children.column.find(
          (ele) => ele.prop == "pcvalue"
        );
        this.$set(parent, "props", { value: "cvalue", label: "cname" });
        this.$set(parent, "dicData", dic);
      }
      if (callcode === "cache_list") {
        let dictTree = JSON.parse(JSON.stringify(window.GF.dicts.module_type));
        let parent = option.column[0].children.column.find(
          (ele) => ele.prop == "type"
        );
        this.$set(parent, "dicData", dictTree);
      }
    },
    submit(a, b) {
      let ext = JSON.stringify(a.form);
      let data = { id: b.id, ext };
      window.GFAPI.sys.FormvalueApi.edit(data)
        .then(() => {
          this.$message({
            message: "修改成功",
            type: "success",
            center: true,
          });
        })
        .finally(() => {
          a.loading();
        });
    },
  },
};
</script>
<style lang="scss" scoped>
.head {
  box-sizing: border-box;
  padding: 0 10px;
  .tabBox {
    padding: 0 10px;
    box-shadow: 1px 1px 10px rgba(0, 0, 0, 0.15);
    // border: 1px solid red;
  }
  .moudleitem {
    height: calc(100vh - 185px);
    padding-bottom: 17px;
    &-itemBox {
      height: 100%;
    }
  }
}
/deep/.avue-form__menu {
  text-align: right;
}
/deep/.el-tabs__content {
  position: relative !important;
  height: 100% !important;
  overflow: scroll;
}
</style>
