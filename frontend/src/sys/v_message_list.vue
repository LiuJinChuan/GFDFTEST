<template>
  <div>
    <Layouts :Layouts="Layouts">
      <template v-slot:Main>
        <apTable ref="table" :TableData="TableData"> </apTable>
      </template>
    </Layouts>
  </div>
</template>
<script>
import Layouts from "@/framework/components/layouts";
import apTable from "@/framework/components/Table";

export default {
  name: "message",
  components: {
    Layouts,
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
        localDict: {},
        ajax: {
          Api: window.GFAPI.sys.MsgApi,
          data: {
            callcode: "message",
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