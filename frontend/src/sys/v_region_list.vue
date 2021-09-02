<template>
  <div>
    <Layouts :Layouts="Layouts">
      <template v-slot:Main>
        <apTable ref="apTable" :TableData="TableData">
          <template slot="cname" slot-scope="{ scope }">
            <span class="slotToDetail" @click="getChildList(scope.row.code)">{{
              scope.row.cname
            }}</span>
          </template>
          <template v-slot:menuLeft="{ scope: {} }">
            <el-button
              type="primary"
              size="small"
              @click="menuLeftClick('add')"
              icon="el-icon-plus"
              >新增</el-button
            >
            <el-button
              v-if="NextArr.length > 1"
              type="primary"
              size="small"
              @click="menuLeftClick('back')"
              icon="el-icon-arrow-left"
              >返回</el-button
            >
          </template>
        </apTable>
      </template>
    </Layouts>
  </div>
</template>
<script>
import Layouts from "@/framework/components/layouts";
import apTable from "@/framework/components/Table";

export default {
  name: "region",
  components: {
    Layouts,
    apTable,
  },
  data: () => {
    return {
      Layouts: {
        type: "Main", //hasTree、Main、deta
      },
      NextArr: ["0"],
      TableData: {
        initList: true,
        optionType: true, //是否需要远端配置
        permanentSearch: {}, //初始化时的搜索条件,不会被清除的固定条件,异步赋值会出现问题
        formatterListData: null, //格式化表格数据
        localDict: {}, //远端字典处理方案
        resetOrder: true,
        OrderObj: { other: [{ parentcode: 0 }], order: [{ code: "asc" }] },
        noPage: true,
        ajax: {
          Api: window.GFAPI.sys.RegionApi,
          data: {
            callcode: "region",
          },
        },
      },
    };
  },
  computed: {},
  watch: {},
  created() {},
  mounted() {},
  methods: {
    menuLeftClick(e) {
      if (e == "add") {
        this.$refs.apTable.$refs.scrud.rowAdd();
      }
      if (e == "back") {
        let code =
          this.NextArr.length > 2 ? this.NextArr[this.NextArr.length - 2] : "0";
        this.TableData.OrderObj = {
          other: [{ parentcode: code }],
          order: [{ code: "asc" }],
        };
        this.$refs.apTable.resetSearchData();
        if (this.NextArr.length != 1) this.NextArr = this.NextArr.slice(0, -1);
      }
    },
    getChildList(code) {
      this.NextArr.push(code);
      this.TableData.OrderObj = {
        other: [{ parentcode: code }],
        order: [{ code: "asc" }],
      };
      this.$refs.apTable.resetSearchData();
    },
  },
};
</script>
<style lang='' scoped>
</style>