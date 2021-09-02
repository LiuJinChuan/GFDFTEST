<template>
  <div id="avue-view" v-loading="loading">
    <div class="head">
      <div class="title">公告设置</div>
    </div>
    <el-container>
      <el-main>
        <div class="crudbox">
          <avue-form
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
export default {
  name: "noticetype",
  data() {
    return {
      callcode: "notice_type",
      loading: true,
      data: {},
      option: {},
      form: {},
    };
  },
  created() {
    this.getInfo();
  },
  methods: {
    //判断路由-进入后的数据处理
    checkRoute() {
      this.form.noticeType.forEach((e) => {
        e.pcvalue ? e.pcvalue : (e.pcvalue = "");
      });
      let dic = JSON.parse(JSON.stringify(this.form.noticeType));
      dic = window.GF.utils.buildListToTree(dic, "pcvalue", "cvalue", "");
      dic.splice(0, 0, { cvalue: "", cname: "" });
      let parent = this.option.column[0].children.column.find(
        (ele) => ele.prop == "pcvalue"
      );
      this.$set(parent, "props", { value: "cvalue", label: "cname" });
      this.$set(parent, "dicData", dic);
    },
    handleSubmit(form, done) {
      var data = Object.assign(this.data, {
        ext: JSON.stringify(this.form),
      });
      window.GFAPI.sys.FormvalueApi.edit(data)
        .then((res) => {
          if (res.code == 0) {
            this.$message({
              message: "编辑成功",
              type: "success",
              center: true,
              duration: 1000,
              onClose() {
                done();
              },
            });
          }
        })
        .catch(() => {});
    },

    getInfo() {
      if (!this.callcode) {
        return;
      }
      window.GFAPI.sys.FormvalueApi.view(this.callcode)
        .then((res) => {
          this.data = res.data;
          this.form = JSON.parse(res.data.ext);
          window.GFAPI.sys.FormApi.view(res.data.formid)
            .then((resp) => {
              this.option = JSON.parse(resp.data.ext);
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