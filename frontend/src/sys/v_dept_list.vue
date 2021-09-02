<template>
  <div>
    <apLayouts :Layouts="Layouts">
      <template slot="Main">
        <apTable ref="apTable" :TableData="TableData">
        </apTable>
      </template>
    </apLayouts>
  </div>
</template>
<script>
import apLayouts from "@/framework/components/layouts";
import apTable from "@/framework/components/Table";

export default {
  name: "dept",
  components: {
    apLayouts,
    apTable,
  },
  data: () => {
    return {
      Layouts: {
        type: "Main", //hasTree、Main、deta
      },
      TableData: {
        initList: true,
        optionType: true, //是否需要远端配置
        permanentSearch: {}, //初始化时的搜索条件,不会被清除的固定条件,异步赋值会出现问题
        formatterListData: null, //格式化表格数据
        localDict: {
          //远端字典处理方案
          pid: () => {
            return new Promise((resolve, reject) => {
              window.GFAPI.sys.DeptApi.load()
                .then((res) => {
                  resolve(res.data);
                })
                .catch((err) => {
                  reject(err);
                });
            });
          },
          incharge: () => {
            return new Promise((resolve, reject) => {
              window.GFAPI.sys.UserApi.load()
                .then((res) => {
                  resolve(res.data);
                })
                .catch((err) => {
                  reject(err);
                });
            });
          },
          deptleader: () => {
            return new Promise((resolve, reject) => {
              window.GFAPI.sys.UserApi.load()
                .then((res) => {
                  resolve(res.data);
                })
                .catch((err) => {
                  reject(err);
                });
            });
          },
        },
        ajax: {
          Api: window.GFAPI.sys.DeptApi,
          data: {
            callcode: "dept",
          },
        },
      },
    };
  },
  computed: {},
  watch: {},
  created() { },
  mounted() { },
  methods: {},
};
</script>
<style scoped>
</style>