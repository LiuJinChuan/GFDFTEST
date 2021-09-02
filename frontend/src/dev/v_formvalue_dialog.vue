<template>
  <el-dialog
    :close-on-click-modal="false"
    :close-on-press-escape="false"
    :visible.sync="dialogVisible"
    :title="title"
    @close="handleClose"
    append-to-body
    width="60%"
  >
    <avue-form
      v-if="dialogVisible"
      :option="option"
      v-model="form"
      @submit="handleSubmit"
    ></avue-form>
  </el-dialog>
</template>

<script>
export default {
  data() {
    return {
      data: {},
      form: {},
      dialogVisible: false,
      option: {},
      title: "",
    };
  },
  methods: {
    handleSubmit(form, done) {
      var that = this;
      var forms = Object.assign(this.data, {
        ext: JSON.stringify(that.form),
      });

      window.GFAPI.sys.FormvalueApi.edit(forms)
        .then((res) => {
          if (res.code == 0) {
            that.$message({
              message: "编辑成功",
              type: "success",
              center: true,
              duration: 1000,
              onClose() {
                done();
                that.dialogVisible = false;
                that.$emit("fn");
              },
            });
          }
        })
        .catch(() => {});
    },

    handleClose() {
      this.form = {};
      this.option = {};
    },
  },
};
</script>
<style scoped lang="scss">
/deep/ .el-dialog {
  margin-top: 0 !important;
  top: 50%;
  transform: translateY(-50%);
}

/deep/ .el-dialog .el-dialog__body {
  max-height: 700px;
  overflow: hidden;
  overflow-y: auto;
}
</style>
