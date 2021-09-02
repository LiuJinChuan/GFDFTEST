<template>
  <div id="avue-view">
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
  name: "forminseg",
  data() {
    return {
      formid: null,
      form: {},
      data: {},
      option: {},
    };
  },
  created() {
    this.formid = this.$route.query.id;
    this.getForms();
  },
  methods: {
    getForms() {
      window.GFAPI.sys.FormApi.view(this.formid)
        .then((res) => {
          this.data = res.data;
          this.option = JSON.parse(res.data.ext);
        })
        .catch(() => {});
    },

    handleSubmit() {
      var that = this;
      var date = new Date().getTime();
      var data = {
        formid: that.formid,
        cname: "formins" + date,
        callcode: "formins" + date,
        ext: JSON.stringify(that.form),
      };

      window.GFAPI.sys.FormvalueApi.add(data)
        .then((res) => {
          if (res.code == 0) {
            that.$message({
              message: "新增成功",
              type: "success",
              center: true,
              duration: 1000,
              onClose() {
                window.close();
              },
            });
          }
        })
        .catch(() => {});
    },
  },
};
</script>
<style scoped>
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
</style>
