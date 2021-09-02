<template>
  <div>
    <apLayouts :Layouts="Layouts">
      <template slot="Main">
        <apTable ref="apTable" :TableData="TableData"> </apTable>
      </template>
    </apLayouts>
  </div>
</template>
<script>
import apLayouts from "@/framework/components/layouts";
import apTable from "@/framework/components/Table";

export default {
  name: "advertspace",
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
        resetOrder: true,
        OrderObj: { order: [{ sort_id: "asc" }] },
        localDict: {}, //远端字典处理方案
        ajax: {
          Api: window.GFAPI.sys.Advert_SpaceApi,
          data: {
            callcode: "advertspace",
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
<style scoped>
</style>