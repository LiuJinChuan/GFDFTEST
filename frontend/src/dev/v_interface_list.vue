<template>
  <div>
    <Layouts :Layouts="Layouts">
      <template v-slot:Tree>
        <apTree
          ref="Tree"
          @click.native="resetSearch"
          @TreeClick="TreeClick"
          @add-success="initTreeList"
          @editSuccess="initTreeList"
          @delSuccess="initTreeList"
          :TreeData="TreeData"
        ></apTree>
      </template>
      <template v-slot:Main>
        <apTable
          ref="Table"
          @addSuccess="initTreeList"
          @delSuccess="initTreeList"
          :TableData="TableData"
        ></apTable>
      </template>
    </Layouts>
  </div>
</template>

<script>
import Layouts from "@/framework/components/layouts";
import apTree from "@/framework/components/Tree/index";
import apTable from "@/framework/components/Table/index";
export default {
  name: "webapi",
  components: {
    Layouts,
    apTree,
    apTable,
  },
  data() {
    return {
      Layouts: {
        type: "hasTree",
      },
      TreeData: {
        addBtn: true,
        editBtn: true,
        delBtn: true,
        operation: true, //是否需要配置项
        key: "value", //树选择的值
        showCheckbox: false, //是否需要选择框
        optionType: false, //是否需要远端配置
        staticOption: {
          column: [
            {
              label: "名称",
              prop: "cname",
              type: "input",
              span: 12,
              size: "small",
              display: true,
              rules: [
                {
                  required: true,
                  message: "名称必须填写",
                },
              ],
              required: true,
            },
            {
              label: "所属模块",
              prop: "module",
              type: "select",
              span: 12,
              size: "small",
              display: true,
              dicData: window.GF.dicts.module_type,
              required: true,
              rules: [
                {
                  required: true,
                  message: "请选择所属模块",
                },
              ],
            },
            {
              label: "控制器",
              prop: "controll",
              type: "input",
              span: 12,
              size: "small",
              display: true,
              required: true,
              rules: [
                {
                  required: true,
                  message: "属控制器必须填写",
                },
              ],
            },
            {
              label: "唯一编码",
              prop: "callcode",
              type: "input",
              span: 12,
              size: "small",
              display: true,
              required: true,
              rules: [
                {
                  required: true,
                  message: "唯一编码必须填写",
                },
              ],
            },
            {
              label: "排序",
              prop: "seqno",
              type: "number",
              span: 12,
              size: "small",
              display: true,
            },
          ],
        },
        beforDelete: ({ data }) => {
          if (!data.canDel) {
            this.$message({
              message: "不能删除带有子级的控制器",
              type: "warning",
            });
            return false;
          }
          return true;
        },
        strictly: false, //是否父子级联
        defaultProps: {
          children: "children",
          label: "cname",
        }, //树绑定的子级
        resetOrder: true,
        OrderObj: { order: [{ seqno: "asc" }] },
        list: [],
        ajax: {
          Api: window.GFAPI.sys.WebapiApi,
        },
      },
      TableData: {
        initList: false,
        TreeSelect: { data: {} },
        permanentSearch: {},
        optionType: false, //是否需要远端配置
        children: true, //是否只能点末级
        resetOrder: true,
        OrderObj: { order: [{ seqno: "asc" }] },
        staticOption: {
          dialogClickModal: false,
          column: [
            {
              label: "名称",
              prop: "cname",
              type: "input",
              span: 12,
              size: "small",
              display: true,
              rules: [
                {
                  required: true,
                  message: "名称必须填写",
                },
              ],
              required: true,
            },
            {
              label: "所属控制器",
              prop: "pid",
              type: "tree",
              span: 12,
              size: "small",
              display: true,
              hide: true,
              dicData: [],
              props: {
                label: "cname",
                value: "id",
              },
              required: true,
              rules: [
                {
                  required: true,
                  message: "请选择父级",
                },
              ],
            },
            {
              label: "所属模块",
              prop: "module",
              type: "select",
              span: 12,
              size: "small",
              display: false,
              dicData: [],
              props: {
                label: "cname",
                value: "cvalue",
              },
              required: true,
              rules: [
                {
                  required: true,
                  message: "请选择所属模块",
                },
              ],
            },
            {
              label: "控制器",
              prop: "controll",
              type: "input",
              span: 12,
              size: "small",
              display: false,
              required: true,
              rules: [
                {
                  required: true,
                  message: "属控制器必须填写",
                },
              ],
            },
            {
              label: "方法",
              prop: "action",
              type: "input",
              span: 12,
              size: "small",
              display: true,
              rules: [
                {
                  required: true,
                  message: "方法必须填写",
                },
              ],
              required: true,
            },
            {
              label: "请求方式",
              prop: "method",
              type: "select",
              span: 12,
              size: "small",
              dicData: [
                {
                  cname: "GET",
                  cvalue: "get",
                },
                {
                  cname: "POST",
                  cvalue: "post",
                },
                {
                  cname: "PUT",
                  cvalue: "put",
                },
                {
                  cname: "DELETE",
                  cvalue: "delete",
                },
              ],
              display: true,
              props: {
                label: "cname",
                value: "cvalue",
              },
              required: true,
              rules: [
                {
                  required: true,
                  message: "请选择请求方式",
                },
              ],
            },
            {
              label: "唯一编码",
              prop: "callcode",
              type: "input",
              span: 12,
              size: "small",
              display: true,
            },
            {
              label: "排序",
              prop: "seqno",
              type: "number",
              span: 12,
              size: "small",
              display: true,
            },
          ],
        },
        ajax: {
          Api: window.GFAPI.sys.WebapiApi,
          data: {
            callcode: "webapi",
          },
        },
      },
      remenberNode: {},
      dicts: [],
    };
  },
  watch: {},
  created() {
    this.initTreeList();
  },
  computed: {},
  methods: {
    initTreeList() {
      window.GFAPI.sys.WebapiApi.load().then((res) => {
        res.data.forEach((e) => {
          e.value = e.id;
          e.canDel = res.data.filter((el) => el.pid == e.id).length == 0;
        });
        let Arr = this.dicts;
        if (this.dicts.length == 0) {
          Arr = JSON.parse(JSON.stringify(window.GF.dicts.module_type));
          this.dicts = Arr;
        }
        Arr.forEach((e) => {
          e.id = 0;
          e.cname = e.label;
          e.withoutBtn = true;
          this.$delete(e, "children");
          e.children = res.data.filter((el) => el.module == e.value);
        });
        this.TreeData.list = Arr;
        let dictsArr = JSON.parse(JSON.stringify(Arr));
        dictsArr.forEach((e) => {
          e.disabled = true;
        });
        this.TableData.staticOption.column.find(
          (e) => e.prop == "pid"
        ).dicData = dictsArr;
        let temp = this.dicts.concat(res.data);
        let str = this.remenberNode[this.TreeData.key];
        let obj = temp.find((el) => el[this.TreeData.key] == str);
        if (obj) {
          this.$nextTick(() => {
            this.$refs.Tree.$refs.tree.setCurrentKey(obj[this.TreeData.key]);
          });
        }
      });
    },
    TreeClick({ data }) {
      this.remenberNode = data;
      if (data.id == 0) {
        this.$refs.Table.resetSearchData({
          other: [{ pid: data.id }, { module: data.value }],
        });
      } else {
        this.$refs.Table.resetSearchData({ other: [{ pid: data.id }] });
      }
    },
    resetSearch() {
      this.remenberNode = {};
      this.$refs.Tree.$refs.tree.setCurrentKey();
      this.$refs.Table.resetSearchData({ other: [{ pid: 0 }] });
    },
  },
};
</script>
<style lang="scss" scoped>
.avue-form__menu--center {
  text-align: right !important;
}
.el-main {
  padding: 5px !important;
}
.crudbox {
  padding: 10px;
  box-sizing: border-box;
  box-shadow: 1px 1px 10px rgba(0, 0, 0, 0.2);
}
.avue-crud {
  margin: 0 auto;
  width: 99%;
}
.head {
  position: relative;
}

.head .color-div {
  width: 4px;
  height: 14px;
  position: absolute;
  top: 6px;
  left: -12px;
}
.head .title {
  position: relative;
  color: #515a6e;
  font-size: 16px;
  line-height: 1.6;
}
.head {
  padding: 0 20px 12px;
  border-bottom: 1px solid #eef1f5;
  margin-bottom: 10px;
}
#avue-view {
  padding: 10px 10px 40px 10px;
  -webkit-box-sizing: border-box;
  box-sizing: border-box;
  height: 100%;
  overflow: hidden auto;
  background-color: white;
}
/deep/.el-dialog__header {
  border-bottom: 1px solid #e4e7ed;
}
/deep/.el-dialog__body {
  padding: 0px;
}
/deep/.el-dialog__footer {
  padding: 5px 10px;
}
/deep/.avue-tree__menu {
  display: none;
}
.inputbox {
  display: inline-flex;
  align-items: center;
  &-item {
    display: inline-flex;
    align-items: center;
    margin-right: 5px;
    > span {
      display: inline-block;
      white-space: nowrap;
      color: #606266;
    }
  }
}
.btnBox {
  display: inline-flex;
  align-items: center;
}
.headBox {
  display: inline-flex;
  align-items: center;
}

.crudbox_ {
  width: 100%;
  display: flex;
  flex-direction: row;
  padding: 10px;
  box-sizing: border-box;
  box-shadow: 1px 1px 10px rgba(0, 0, 0, 0.2);
  height: 750px;
  overflow: hidden;
  .aside-main {
    border-radius: 4px;
    border: 1px solid #e6ebf5;
    box-shadow: 1px 1px 10px rgba(0, 0, 0, 0.2);
    padding: 15px !important;
  }
  .crudbox {
    padding: 10px;
    box-sizing: border-box;
    box-shadow: 1px 1px 10px rgba(0, 0, 0, 0.2);
    /deep/ .avue-form-search {
      .el-form-item {
        margin-bottom: 0px !important;
      }
      /deep/.el-row {
        display: flex !important;
        flex-direction: row;
        .avue-group {
          width: 100%;
        }
      }
    }
    /deep/.avue-form__menu--center {
      text-align: right !important;
    }
  }
}
</style>
