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
  name: "files",
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
        formatterListData(data) {
          data.forEach((e) => {
            e.orgname = e.orgname.replace(e.filetype, "");
            e.newname = e.newname.replace(e.filetype, "");
          });
          return data;
        }, //格式化表格数据
        formatterFormData(data) {
          data.orgname = data.orgname + data.filetype;
          data.newname = data.newname + data.filetype;
          return data;
        }, //新增编辑表单数据
        localDict: {},
        ajax: {
          Api: window.GFAPI.sys.FileResApi,
          data: {
            callcode: "files",
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
<style lang='' scoped>
</style>