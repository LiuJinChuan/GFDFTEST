<template>
  <el-dialog
    :close-on-click-modal="false"
    :close-on-press-escape="false"
    :visible.sync="dialogVisible"
    title="添加到"
    @close="handleClose"
    append-to-body
    width="60%"
  >
    <avue-form
      v-if="dialogVisible"
      ref="form"
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
      dialogVisible: false,
      data: {},
      option: {
        menuPosition: "right",
        labelWidth: 100,
        column: [
          {
            type: "select",
            label: "元数据列表",
            dicData: [],
            span: 24,
            display: true,
            prop: "selects",
            required: true,
            props: {
              label: "cname",
              value: "id",
            },
            rules: [
              {
                required: true,
                message: "请选择元数据列表",
              },
            ],
          },
        ],
      },
      form: {},
    };
  },
  methods: {
    handleClose() {
      this.$refs.form.resetForm();
    },

    handleSubmit(form, done) {
      var that = this;
      var table = that.data.table.find((ele) => ele.id == this.form.selects);
      var ext = JSON.parse(table.ext);
      var cols = JSON.parse(JSON.stringify(ext.cols || []));
      var addCols = that.data.content.map((ele) => {
        return that.$parent.formatRow(ele);
      });
      var filCols = cols.concat(addCols);
      ext.cols = filCols;
      table.ext = JSON.stringify(ext);
      window.GFAPI.dev.MetaApi.edit(table)
        .then(() => {
          done();
          that.dialogVisible = false;
          that.$message({
            message: "添加成功",
            type: "success",
            center: true,
            duration: 1000,
            onClose() {
              that.$parent.selection = [];
              that.$parent.refreshTree();
            },
          });
        })
        .catch(() => {});
    },
  },
};
</script>
<style  scoped>
</style>