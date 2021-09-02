<template>
  <div>
    <Layouts :Layouts="Layouts">
      <template v-slot:Main>
        <aTable ref="table" :TableData="TableData" v-if="isShow">
          <template slot-scope="{}" slot="menuLeft">
            <el-button
              type="primary"
              icon="el-icon-arrow-left"
              size="small"
              plain
              v-if="!isParent"
              @click.stop="handleBack"
              >返回</el-button
            >
          </template>
          <template slot="callcode" slot-scope="{ scope }">
            <el-tag style="cursor: pointer" @click="handleCellClick(scope.row)"
              >编辑字典</el-tag
            >
          </template>
        </aTable>
      </template>
    </Layouts>
  </div>
</template>

<script>
import Layouts from "@/framework/components/layouts";
import aTable from "@/framework/components/Table";

export default {
  name: "dicts",
  data() {
    return {
      isShow: true,
      isParent: true,
      /*布局*/
      Layouts: {
        type: "Main",
      },
      TableData: {
        initList: false, //是否初始化数据
        optionType: false, //是否需要远端配置,
        staticOption: {},
        permanentAddFrormData: {}, //新增时的传入参数,可变动,不支持异步
        ajax: {
          Api: window.GFAPI.sys.DictApi,
        },
      },
      pOption: {
        menu: false,
        addBtn: false,
        delBtn: false,
        editBtn: false,
        refreshBtn: false,
        columnBtn: true,
        dialogEscape: false,
        dialogModal: true,
        dialogClickModal: false,
        dialogCloseBtn: true,
        dialogType: "dialog",
        menuType: "text",
        menuAlign: "center",
        menuWidth: "200",
        height: 740,
        index: true,
        headerAlign: "center",
        border: true,
        column: [
          {
            label: "名称",
            prop: "cname",
            type: "input",
            size: "small",
            align: "center",
            rules: [
              {
                required: true,
                message: "请输入名称",
                trigger: "blur",
              },
            ],
          },
          {
            label: "调用标识",
            prop: "callcode",
            type: "input",
            size: "small",
            align: "center",
            slot: true,
            rules: [
              {
                required: true,
                message: "请输入调用标识",
                trigger: "blur",
              },
            ],
          },
          {
            label: "描述",
            prop: "memo",
            type: "textarea",
            span: 24,
            size: "small",
          },
        ],
      },
      cOption: {
        addBtn: true,
        refreshBtn: false,
        columnBtn: true,
        dialogEscape: false,
        dialogModal: true,
        dialogClickModal: false,
        dialogCloseBtn: true,
        dialogType: "dialog",
        menuType: "text",
        menuAlign: "center",
        menuWidth: "200",
        height: 740,
        index: true,
        headerAlign: "center",
        border: true,
        column: [
          {
            label: "标签",
            prop: "cname",
            type: "input",
            size: "small",
            align: "center",
          },
          {
            label: "值",
            prop: "cvalue",
            type: "input",
            size: "small",
            align: "center",
          },
          {
            label: "排序",
            prop: "seqno",
            type: "number",
            size: "small",
          },
          {
            label: "描述",
            prop: "memo",
            type: "textarea",
            span: 24,
            size: "small",
          },
        ],
      },
    };
  },
  components: {
    Layouts,
    aTable,
  },
  watch: {},
  created() {
    this.TableData.staticOption = JSON.parse(JSON.stringify(this.pOption));
  },
  mounted() {
    this.initList();
  },
  methods: {
    initList() {
      let data = {
        other: [{ pid: 0 }, { flag: 1 }],
      };
      this.$refs.table.resetSearchData(data);
    },
    handleCellClick(row) {
      this.TableData.permanentAddFrormData = { pid: row.id };
      this.TableData.staticOption = JSON.parse(JSON.stringify(this.cOption));
      this.isShow = false;
      setTimeout(() => {
        this.isParent = false;
        this.isShow = true;
        this.$nextTick(() => {
          let data = {
            other: [{ pid: row.id }],
          };
          this.$refs.table.resetSearchData(data);
        });
      }, 0);
    },

    handleBack() {
      this.TableData.permanentAddFrormData = { pid: 0 };
      this.cellid = null;
      this.TableData.staticOption = JSON.parse(JSON.stringify(this.pOption));
      this.isShow = false;
      setTimeout(() => {
        this.isParent = true;
        this.isShow = true;
        this.$nextTick(() => {
          this.initList();
        });
      }, 0);
    },
  },
};
</script>