<template>
  <div id="app">
    <router-view />
  </div>
</template>

<script>
export default {
  name: "app",
  data() {
    return {};
  },
  created() {},
  methods: {},
  computed: {},
};
</script>
<style lang="scss">
#app {
  width: 100%;
  height: 100%;
  overflow: hidden;
}

body .avue-crud .el-table th.gutter {
  display: table-cell !important;
}

body .avue-crud .el-table colgroup.gutter {
  display: table-cell !important;
}

body .avue-crud .el-table table {
  width: 100% !important;
}

.avue-crud__search .el-collapse-item__content .avue-form__menu--right {
  width: 25%;
  padding: 0;
}
.avue-crud__search .el-collapse-item__content .avue-form__group--flex {
  justify-content: space-between;
}
.avue-crud__search .el-form-item--mini.el-form-item,
.avue-crud__search .el-form-item--small.el-form-item {
  margin-bottom: 0 !important;
}

.slotToDetail {
  color: #66b1ff;
  cursor: pointer;
}
.menuBox {
  span{
    margin: 0 2px;
  }
}
</style>
