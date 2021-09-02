<template>
  <div>
    <apLayouts :Layouts="Layouts">
      <template v-slot:Main>
        <apTable ref="table" :TableData="TableData"> </apTable>
      </template>
    </apLayouts>
  </div>
</template>
<script>
import apLayouts from "@/framework/components/layouts";
import apTable from "@/framework/components/Table";

export default {
  name: "finflow",
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
        initPageSize: 15,
        initList: true,
        optionType: true, //是否需要远端配置
        permanentSearch: {}, //初始化时的搜索条件,不会被清除的固定条件,异步赋值会出现问题
        formatterListData: null, //格式化表格数据
        localDict: {},
        ajax: {
          Api: window.GFAPI.sys.F_flowApi,
          data: {
            callcode: "finflow",
          },
        },
      },
    };
  },
  computed: {},
  watch: {},
  created() {},
  mounted() {},
  methods: {},
};
</script>
<style lang="scss" scoped>
</style>