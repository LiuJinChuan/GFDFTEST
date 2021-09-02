<template>
  <div id="avue-view" v-loading="loading">
    <div class="head">
      <div class="title">
        {{ data.cname }}
        <div
          class="color-div"
          :style="{ 'background-color': $store.state.common.colorName }"
        ></div>
      </div>
    </div>
    <el-container>
      <el-main>
        <div class="crudbox">
          <avue-form
            ref="forms"
            :option="option"
            v-model="form"
            @submit="handleSubmit"
          ></avue-form>
        </div>
      </el-main>
    </el-container>
  </div>
</template>
<script>
import Vue from "vue";
export default {
  name: "",
  data() {
    return {
      callcode: null,
      loading: true,
      data: {},
      option: {},
      form: {},
      print_template: {
        detail: {},
        steps: [],
      },
      printObj: {
        id: "printList",
        popTitle: "good print",
        standard: "html5",
      },
    };
  },
  watch: {
    "$route.query.callcode"(val) {
      if (val) {
        this.loading = true;
        this.$refs.forms.resetForm();
        this.callcode = this.$route.query.callcode;
        this.getInfo();
      }
    },
  },
  created() {
    this.callcode = this.$route.query.callcode;
    this.getInfo();
  },
  methods: {
    //判断路由-进入后的数据处理
    checkRoute() {
      if (this.$route.query.callcode === "notice_type") {
        this.form.noticeType.forEach((e) => {
          e.pcvalue ? e.pcvalue : (e.pcvalue = "");
        });
        let dic = JSON.parse(JSON.stringify(this.form.noticeType));
        dic = window.GF.utils.buildListToTree(dic, "pcvalue", "cvalue", "");
        dic.splice(0, 0, { cvalue: "", cname: "" });

        let parent = this.option.column[0].children.column.find(
          (ele) => ele.prop == "pcvalue"
        );
        Vue.set(parent, "props", { value: "cvalue", label: "cname" });
        Vue.set(parent, "dicData", dic);
      }
    },
    //判断路由-提交后更新
    //
    checkRouteAfterReset() {
      if (this.$route.query.callcode === "print_template") {
        this.reSetPrint();
      }
    },
    //更新Storage打印模板
    reSetPrint() {
      localStorage.setItem("temp", this.form.print_template);
      window.location.reload();
    },
    //打印预览
    printDialog() {},

    handleSubmit(form, done) {
      var that = this;
      var data = Object.assign(that.data, {
        ext: JSON.stringify(that.form),
      });

      window.GFAPI.sys.FormvalueApi.edit(data)
        .then((res) => {
          if (res.code == 0) {
            that.$message({
              message: "编辑成功",
              type: "success",
              center: true,
              duration: 1000,
              onClose() {
                that.checkRouteAfterReset();
                done();
              },
            });
          }
        })
        .catch(() => {});
    },

    getInfo() {
      window.GFAPI.sys.FormvalueApi.view(this.callcode)
        .then((res) => {
          this.data = res.data || {};

          window.GFAPI.sys.FormApi.view(res.data.formid)
            .then((resp) => {
              this.option = JSON.parse(resp.data.ext);
              this.form = JSON.parse(res.data.ext);
              this.loading = false;
              this.checkRoute();
            })
            .catch(() => {});
        })
        .catch(() => {});
    },
  },
};
</script>
<style lang="scss" scoped>
.avue-form__menu--center {
  text-align: right !important;
}
.el-main {
  padding: 5px !important;
}
.crudbox {
  padding: 10px;
  box-sizing: border-box;
  box-shadow: 1px 1px 10px rgba(0, 0, 0, 0.2);
}
.avue-crud {
  margin: 0 auto;
  width: 99%;
}

.head {
  position: relative;
}

.head .color-div {
  width: 4px;
  height: 14px;
  position: absolute;
  top: 6px;
  left: -12px;
}

.head .title {
  position: relative;
  color: #515a6e;
  font-size: 16px;
  line-height: 1.6;
}
.head {
  padding: 0 20px 12px;
  border-bottom: 1px solid #eef1f5;
  margin-bottom: 10px;
}
#avue-view {
  padding: 10px 10px 40px 10px;
  -webkit-box-sizing: border-box;
  box-sizing: border-box;
  height: 100%;
  overflow: hidden auto;
  background-color: white;
}
/deep/.el-tree-node__content {
  text-align: left;
}
</style>
