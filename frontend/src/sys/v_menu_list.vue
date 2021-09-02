<template>
  <div>
    <apLayouts :Layouts="Layouts">
      <template slot="Tree">
        <apTree
          @TreeClick="TreeClick"
          ref="atree"
          :TreeData="TreeData"
        ></apTree>
      </template>
      <template slot="Main">
        <apTable ref="apTable" :TableData="TableData">
          <template v-slot:menuLeft="">
            <el-button
              icon="el-icon-plus"
              size="small"
              :disabled="noteDialog.nodeData.type != 1"
              @click="oppenDialog('add')"
              type="primary"
              >新 增</el-button
            >
          </template>

          <template v-slot:menu="{ scope: { row, type, size } }">
            <el-button
              icon="el-icon-edit"
              :size="size"
              @click="oppenDialog('edit', row)"
              :type="type"
              >编辑</el-button
            >
            <el-button
              icon="el-icon-delete"
              @click="handleDelete(row)"
              :size="size"
              :type="type"
              >删除</el-button
            >
          </template>
        </apTable>
      </template>
    </apLayouts>
    <el-dialog
      title="提示"
      :visible.sync="noteDialog.dialogVisible"
      :close-on-click-modal="false"
      :append-to-body="true"
      :show-close="false"
      width="60%"
    >
      <span slot="title" class="dialog-title">
        <div v-if="noteDialog.type == 'add'">添加菜单</div>
        <div v-if="noteDialog.type == 'edit'">编辑菜单</div>
      </span>

      <el-tabs
        v-model="noteDialog.tabsData.activeName"
        type="card"
        @tab-click="handleTabsClick"
      >
        <el-tab-pane
          label="路由菜单"
          :disabled="noteDialog.isEdit && noteDialog.tabsData.activeName != 'x'"
          name="x"
        ></el-tab-pane>
        <el-tab-pane
          label="目录"
          :disabled="noteDialog.isEdit && noteDialog.tabsData.activeName != 'y'"
          name="y"
        ></el-tab-pane>
        <el-tab-pane
          label="链接菜单"
          :disabled="noteDialog.isEdit && noteDialog.tabsData.activeName != 'z'"
          name="z"
        ></el-tab-pane>
      </el-tabs>
      <avue-form
        ref="dialogForm"
        :option="noteDialog.crudOption"
        v-model="noteDialog.form"
      ></avue-form>
      <span slot="footer" class="dialog-footer">
        <el-button @click="noteDialog.dialogVisible = false">取 消</el-button>
        <el-button
          type="primary"
          :loading="noteDialog.loading"
          @click="noteDialogSubmit"
          >确 定</el-button
        >
      </span>
    </el-dialog>
  </div>
</template>

<script>
import apLayouts from "@/framework/components/layouts";
import apTree from "@/framework/components/Tree";
import apTable from "@/framework/components/Table";

export default {
  name: "menu",
  components: { apLayouts, apTree, apTable },
  data() {
    function listFormatter(params) {
      let temp = JSON.parse(JSON.stringify(params)).sort(
        (a, b) => a.seqno - b.seqno
      );
      let arr = window.GF.utils.buildListToTree(temp, "pid", "id", 0);
      return [
        {
          type: 1,
          cname: "全部",
          id: 0,
          children: arr,
        },
      ];
    }
    return {
      Layouts: {
        type: "hasTree",
      },
      TableData: {
        optionType: false, //配置类型是否为远程请求
        staticOption: {
          refreshBtn: true,
          editBtn: false,
          addBtn: false,
          delBtn: false,
          menuAlign: "center",
          menuWidth: "200",
          menuType: "text",
          dialogType: "dialog",
          dialogEscape: false,
          dialogModal: true,
          dialogCloseBtn: true,
          dialogClickModal: false,
          index: true,
          border: true,
          align: "center",
          column: [
            {
              label: "名称",
              prop: "cname",
              type: "input",
              size: "small",
              align: "left",
              width: "100px",
              rules: [
                {
                  required: true,
                  message: "请输入名称",
                  trigger: "blur",
                },
              ],
            },
            {
              label: "排序",
              prop: "seqno",
              type: "number",
              size: "small",
              rules: [
                {
                  required: true,
                  message: "请输入排序",
                  trigger: "blur",
                },
              ],
            },
            {
              label: "链接地址",
              prop: "link",
              align: "left",
              type: "input",
              size: "small",
              rules: [
                {
                  required: true,
                  message: "请输入连接地址",
                  trigger: "blur",
                },
              ],
            },
            {
              label: "类型",
              prop: "type",
              type: "select",
              size: "small",
              rules: [
                {
                  required: true,
                  message: "请选择类型",
                  trigger: "blur",
                },
              ],
              dicData: [
                { value: "x", label: "路由菜单" },
                { value: "y", label: "目录" },
                { value: "z", label: "链接菜单" },
              ],
            },
            {
              label: "菜单图标",
              prop: "icon",
              type: "icon",
              iconList: [
                {
                  label: "基本图标",
                  list: [
                    "el-icon-info",
                    "el-icon-error",
                    "el-icon-success",
                    "el-icon-warning",
                    "el-icon-question",
                    "el-icon-edit",
                    "el-icon-delete",
                  ],
                },
                {
                  label: "方向图标",
                  list: [
                    "el-icon-info",
                    "el-icon-back",
                    "el-icon-arrow-left",
                    "el-icon-arrow-down",
                    "el-icon-arrow-right",
                    "el-icon-arrow-up",
                  ],
                },
                {
                  label: "符号图标",
                  list: [
                    "el-icon-plus",
                    "el-icon-minus",
                    "el-icon-close",
                    "el-icon-check",
                  ],
                },
                {
                  label: "阿里云图标",
                  list: [
                    "iconfont icon-zhongyingwen",
                  ],
                },
              ],
            },
            {
              label: "组件路径",
              prop: "component",
              align: "left",
              type: "input",
              size: "small",
            },
            {
              label: "路由名称",
              prop: "callcode",
              align: "left",
              type: "input",
              size: "small",
            },
            {
              label: "上级类目",
              prop: "pid",
              type: "tree",
              size: "small",
              dicData: [],
              props: {
                label: "cname",
                value: "id",
              },
            },
            {
              label: "样式",
              hide: true,
              prop: "style",
              type: "input",
              size: "small",
            },
          ],
        },
        formatterListData(data) {
          data.forEach((e) => {
            e.type = e.type == 0 ? "x" : e.type == 1 ? "y" : "z";
          });
          return data;
        }, //格式化表格数据
        formatterFormData(data) {
          data.type = data.type == "x" ? 0 : data.type == "y" ? 1 : 2;
          return data;
        }, //新增编辑表单数据
        resetOrder: true,
        OrderObj: { order: [{ seqno: "asc" }] },
        ajax: {
          Api: window.GFAPI.sys.MenuApi,
        },
      },
      TreeData: {
        name: "tree1",
        node: "b", //node必填,并且不能改键,node就是包裹你想联动组件的id
        key: "id",
        editBtn: false,
        delBtn: false,
        defaultExpandAll: false,
        showCheckbox: false,
        strictly: false,
        optionType: false,
        staticOption: {
          column: [],
        },
        defaultProps: {
          children: "children",
          label: "cname",
        },
        ajax: {
          Api: window.GFAPI.sys.MenuApi,
          data: {},
          listFormatter,
        },
      },
      noteDialog: {
        dialogVisible: false,
        loading: false,
        tabsData: {
          activeName: "x",
        },
        form: {},
        nodeData: {},
        crudOption: {
          submitBtn: false,
          emptyBtn: false,
          column: [
            {
              type: "select",
              label: "父节点",
              span: 12,
              display: true,
              size: "small",
              prop: "pid",
              props: {
                label: "cname",
                value: "id",
                children: "childrens",
              },
              rules: [],
              dicData: [],
              // "disabled": true,
              parent: true,
            },
            {
              type: "switch",
              label: "是否隐藏",
              span: 12,
              display: true,
              value: 0,
              dicData: [
                {
                  label: "隐藏",
                  value: 1,
                },
                {
                  label: "显示",
                  value: 0,
                },
              ],
              prop: "flag",
            },
            {
              type: "input",
              label: "链接地址",
              span: 12,
              display: true,
              size: "small",
              prop: "link",
              rules: [
                {
                  required: true,
                  message: "链接地址必须填写",
                },
              ],
              required: true,
            },
            {
              type: "input",
              label: "名称",
              span: 12,
              display: true,
              width: "100px",
              size: "small",
              prop: "cname",
              rules: [
                {
                  required: true,
                  message: "名称必须填写",
                },
              ],
              required: true,
            },
            {
              type: "input",
              label: "组件路径",
              span: 12,
              display: true,
              size: "small",
              prop: "component",
              rules: [],
              required: false,
            },
            {
              type: "input",
              label: "路由名称",
              span: 12,
              display: true,
              size: "small",
              prop: "callcode",
              rules: [],
              required: false,
            },
            {
              label: "菜单图标",
              prop: "icon",
              type: "icon",
              iconList: [
                {
                  label: "基本图标",
                  list: [
                    "el-icon-info",
                    "el-icon-error",
                    "el-icon-success",
                    "el-icon-warning",
                    "el-icon-question",
                    "el-icon-edit",
                    "el-icon-delete",
                  ],
                },
                {
                  label: "方向图标",
                  list: [
                    "el-icon-info",
                    "el-icon-back",
                    "el-icon-arrow-left",
                    "el-icon-arrow-down",
                    "el-icon-arrow-right",
                    "el-icon-arrow-up",
                  ],
                },
                {
                  label: "符号图标",
                  list: [
                    "el-icon-plus",
                    "el-icon-minus",
                    "el-icon-close",
                    "el-icon-check",
                  ],
                },
                {
                  label: "阿里云图标",
                  list: [
                    "iconfont icon-zhongyingwen",
                  ],
                },
              ],
            },
            {
              type: "number",
              label: "排序",
              span: 12,
              display: true,
              size: "small",
              prop: "seqno",
            },
          ],
        },
      },
    };
  },
  watch: {},
  created() {
    this.getOthers();
  },
  computed: {},
  methods: {
    handleClick(e) {
      this.activeName = e.name;
      this.swidth = "99.9%";
      setTimeout(() => {
        this.swidth = "100%";
      }, 500);

      if (e.name == "first") {
        this.$refs.table.getList();
        this.$refs.tables.getList();
        return;
      }
      this.$refs.tables.getList();
      this.$refs.tables.getList();
    },
    getOthers() {
      window.GFAPI.sys.MenuApi.load().then((res) => {
        let list2 = res.data.filter((e) => e.type == 1);
        list2.unshift({ cname: "全部 ", id: 0, type: 1 });
        var pid1 = this.TableData.staticOption.column.find(
          (ele) => ele.prop == "pid"
        );
        this.$set(pid1, "dicData", list2);
        var pid2 = this.noteDialog.crudOption.column.find(
          (ele) => ele.prop == "pid"
        );
        this.$set(pid2, "dicData", list2);
      });
    },
    TreeClick({ data }) {
      let info = { other: [{ pid: data.id }] };
      this.$refs.apTable.resetSearchData(info);
      this.noteDialog.nodeData = data;
    },
    oppenDialog(e, row) {
      this.noteDialog.dialogVisible = true;
      this.noteDialog.type = e;
      if (e == "add") {
        this.noteDialog.isEdit = false;
        this.noteDialog.form.pid = this.noteDialog.nodeData.id
          ? this.noteDialog.nodeData.id
          : 0;
        this.handleTabsClick({ name: this.noteDialog.tabsData.activeName });
      }
      if (e == "edit") {
        this.noteDialog.isEdit = true;
        let temp = JSON.parse(JSON.stringify(row));
        this.noteDialog.tabsData.activeName = temp.type;
        this.handleTabsClick({ name: temp.type });
        this.noteDialog.form = temp;
        this.noteDialog.form.pid = temp.pid;
      }
    },
    handleTabsClick(e) {
      if (e.name == "x") {
        this.hidden([]);
      }
      if (e.name == "y") {
        let arr = ["link", "component"];
        this.hidden(arr);
      }
      if (e.name == "z") {
        this.hidden(["component"]);
      }
    },
    hidden(arr) {
      this.noteDialog.crudOption.column.forEach((e) => {
        e.display = !arr.includes(e.prop);
      });
    },
    noteDialogSubmit() {
      this[this.noteDialog.type]();
    },
    add() {
      //开启load
      this.$refs.dialogForm.validate((res) => {
        if (res) {
          this.noteDialog.loading = true;
          this.noteDialog.form.type = this.noteDialog.tabsData.activeName;
          this.noteDialog.form.type =
            this.noteDialog.tabsData.activeName == "x"
              ? 0
              : this.noteDialog.tabsData.activeName == "y"
              ? 1
              : 2;
          window.GFAPI.sys.MenuApi.add(this.noteDialog.form)
            .then((res) => {
              if (res.code == 0) {
                this.$message({
                  message: "新增成功",
                  type: "success",
                  center: true,
                  duration: 1000,
                });
                this.noteDialog.dialogVisible = false;
                window.GF.store.dispatch("setSysMenus").then(() => {});
                this.$refs.apTable.getList();
              }
            })
            .finally(() => {
              this.noteDialog.loading = false;
              this.$refs.dialogForm.clearValidate();
              this.$refs.dialogForm.allDisabled = false;
            });
        }
      });
    },
    edit() {
      this.$refs.dialogForm.validate((res) => {
        if (res) {
          this.noteDialog.loading = true;
          this.noteDialog.form.type = this.noteDialog.tabsData.activeName;
          this.noteDialog.form.type =
            this.noteDialog.tabsData.activeName == "x"
              ? 0
              : this.noteDialog.tabsData.activeName == "y"
              ? 1
              : 2;
          window.GFAPI.sys.MenuApi.edit(this.noteDialog.form)
            .then((res) => {
              if (res.code == 0) {
                this.$message({
                  message: "编辑成功",
                  type: "success",
                  center: true,
                  duration: 1000,
                });
                this.noteDialog.dialogVisible = false;
                this.$refs.apTable.getList();
                this.$refs.atree.getList();
              }
            })
            .finally(() => {
              this.noteDialog.loading = false;
              this.$refs.dialogForm.clearValidate();
              this.$refs.dialogForm.allDisabled = false;
            });
        }
      });
    },
    handleDelete(row) {
      this.$confirm("请确认是否删除当前项, 再继续?", "提示", {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        type: "warning",
      })
        .then(() => {
          window.GFAPI.sys.MenuApi.del(row.id)
            .then((res) => {
              if (res.code == 0) {
                this.$message({
                  message: "删除成功",
                  type: "success",
                  center: true,
                  duration: 1000,
                });
                this.$refs.apTable.getList();
              }
            })
            .catch(() => {});
        })
        .catch(() => {});
    },
  },
};
</script>
<style lang="scss" scoped></style>
