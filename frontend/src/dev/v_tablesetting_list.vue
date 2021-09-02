<template>
  <div>
    <Layouts :Layouts="Layouts">
      <template v-slot:Tree>
        <Tree @TreeClick="TreeClick" ref="trees" :TreeData="TreeData"></Tree>
      </template>
      <template v-slot:Main>
        <el-tabs v-model="tabsOption.activeName">
          <el-tab-pane label="表格字段" name="table">
            <avue-crud
              ref="crud"
              :option="TableData.option"
              :data="tabsOption.list"
              v-model="tabsOption.tableForm"
              @sortable-change="sortableChange"
              @selection-change="selectionChange"
              @row-save="handleCreate"
              @row-del="handleDelete"
              @row-update="handleUpdate"
            >
              <template slot-scope="{}" slot="menuLeft">
                <el-button
                  type="primary"
                  icon="el-icon-folder-add"
                  size="small"
                  :disabled="!tabsOption.tableSelect.length"
                  @click.stop="handleAddTo"
                  >添加到</el-button
                >
                <el-button
                  type="primary"
                  :disabled="!tabsOption.item.sortChange"
                  icon="el-icon-document-checked"
                  size="small"
                  @click.stop="handleSortSave()"
                  >拖动保存</el-button
                >
                <div>{{ selectNode.tname }}</div>
              </template>
              <template slot="colname" slot-scope="scope">
                <el-tag
                  style="cursor: pointer"
                  @click="handleCellClick(scope.row.colname)"
                  >{{ scope.row.colname }}</el-tag
                >
              </template>
              <template slot="desc" slot-scope="scope">
                <el-tag
                  style="cursor: pointer"
                  @click="handleCellClick(scope.row.desc)"
                  >{{ scope.row.desc }}</el-tag
                >
              </template>
            </avue-crud>
          </el-tab-pane>
          <el-tab-pane label="元数据" name="yuanshuju">
            <div style="padding: 0 10px">
              <el-button type="primary" size="small" @click.stop="handleExport"
                >导 出</el-button
              >
              <el-button
                type="primary"
                size="small"
                @click.stop="handleCopyCrudY"
                >复制到剪切板</el-button
              >
            </div>
            <json-editor ref="jsonEditorCrudY" v-model="jsonDataCrudY" />
          </el-tab-pane>
          <el-tab-pane label="CRUD配置" name="crud">
            <div style="padding: 0 10px">
              <el-button
                type="primary"
                size="small"
                @click.stop="handleCopyCrud"
                >复制到剪切板</el-button
              >
            </div>
            <json-editor ref="jsonEditorCrud" v-model="jsonDataCrud" />
          </el-tab-pane>
        </el-tabs>
      </template>
    </Layouts>
    <add-to ref="addTo" />
  </div>
</template>

<script>
import Layouts from "@/framework/components/layouts";
import Tree from "@/framework/components/Tree/index.vue";
import JsonEditor from "@/framework/page/JsonEditor";
import AddTo from "./d_addto_dialog";
function listFormatter(params) {
  let arr = Array.from(new Set(params.map((e) => e.callcode)));
  arr = arr.map((e) => {
    return { tname: e, children: [], withoutBtn: true, iscallcode: true };
  });

  arr.forEach((e) => {
    e.children = params.filter((ele) => e.tname == ele.callcode);
  });
  return arr;
}
export default {
  name: "tablesetting",
  data() {
    return {
      Layouts: {
        type: "hasTree", //hasTree、Main、detail
      },
      selectNode: {},
      TreeData: {
        name: "tree1",
        node: "b", //node必填,并且不能改键,node就是包裹你想联动组件的id
        key: "id",
        showCheckbox: false,
        strictly: false,
        operation: true,
        optionType: false,
        addBtn: true,
        editBtn: true,
        delBtn: true,
        formFormatter(data) {
          let temp = JSON.parse(data.ext || "{}");
          temp.name = data.cname;
          temp.cname = data.tname;
          data.ext = JSON.stringify(temp);
          return data;
        },
        staticOption: {
          column: [
            {
              label: "中文表名",
              prop: "tname",
              rules: [
                {
                  required: true,
                  message: "请输入中文表名",
                  trigger: "blur",
                },
              ],
            },
            {
              label: "英文表名",
              prop: "cname",
              rules: [
                {
                  required: true,
                  message: "请输入英文表名",
                  trigger: "blur",
                },
              ],
            },
            {
              label: "分类",
              prop: "callcode",
              rules: [
                {
                  required: true,
                  message: "请输入分类",
                  trigger: "blur",
                },
              ],
            },
            {
              label: "描述",
              prop: "memo",
              type: "textarea",
              span: 24,
            },
          ],
        },
        defaultProps: {
          children: "children",
          label: "tname",
        },
        ajax: {
          Api: window.GFAPI.dev.MetaApi,
          listFormatter,
          data: {
            url: "/sys/meta",
          },
        },
      },
      tabsOption: {
        activeName: "table",
        item: {},
        list: [],
        tableSelect: [],
        tableForm: {},
      },
      TableData: {
        option: {
          addBtn: true,
          delBtn: true,
          editBtn: true,
          refreshBtn: false,
          columnBtn: true,
          selection: true,
          sortable: true,
          dialogType: "dialog",
          dialogEscape: false,
          dialogModal: true,
          dialogClickModal: false,
          dialogCloseBtn: true,
          menuWidth: "200",
          menuAlign: "center",
          menuType: "text",
          border: true,
          headerAlign: "center",
          align: "center",
          index: true,
          labelWidth: 120,
          menuBtn: true,
          menuPosition: "right",
          submitBtn: true,
          emptyBtn: true,
          maxHeight: 555,
          column: [
            {
              label: "列名（英文）",
              prop: "colname",
              slot: true,
              type: "input",
              size: "small",
              align: "center",
              span: 8,
              required: true,
              rules: [
                {
                  required: true,
                  message: "列名（英文）必须填写",
                },
              ],
            },
            {
              label: "列名（中文）",
              slot: true,
              prop: "desc",
              type: "input",
              span: 8,
              size: "small",
              align: "center",
              rules: [
                {
                  required: true,
                  message: "列名（中文）必须填写",
                },
              ],
              required: true,
            },
            {
              label: "是否加入列字段",
              prop: "iscol",
              type: "select",
              value: true,
              dicData: [
                {
                  label: "是",
                  value: true,
                },
                {
                  label: "否",
                  value: false,
                },
              ],
              span: 8,
            },
            {
              label: "类型(数据库)",
              prop: "sqltype",
              type: "select",
              dicData: [
                {
                  label: "tinyint",
                  value: "tinyint",
                },
                {
                  label: "int",
                  value: "int",
                },
                {
                  label: "bigint",
                  value: "bigint",
                },
                {
                  label: "bit",
                  value: "bit",
                },
                {
                  label: "datetime",
                  value: "datetime",
                },
                {
                  label: "varchar",
                  value: "varchar",
                },
                {
                  label: "text",
                  value: "text",
                },
                {
                  label: "decimal",
                  value: "decimal",
                },
              ],
              span: 8,
            },
            {
              type: "input",
              label: "数据库长度",
              span: 8,
              display: true,
              hide: true,
              prop: "length",
            },
            {
              type: "input",
              label: "数据库默认值",
              span: 8,
              display: true,
              hide: true,
              prop: "default",
            },
            {
              type: "switch",
              label: "数据库为空",
              hide: true,
              span: 8,
              display: true,
              dicData: [
                {
                  label: "否",
                  value: false,
                },
                {
                  label: "是",
                  value: true,
                },
              ],
              prop: "isNull",
            },
            {
              label: "类型(class)",
              prop: "clstype",
              type: "select",
              dicData: [
                {
                  label: "int",
                  value: "int",
                },
                {
                  label: "long",
                  value: "long",
                },
                {
                  label: "bool",
                  value: "bool",
                },
                {
                  label: "DateTime",
                  value: "DateTime",
                },
                {
                  label: "string",
                  value: "string",
                },
                {
                  label: "decimal",
                  value: "decimal",
                },
              ],
              span: 8,
            },
            {
              type: "textarea",
              label: "备注",
              span: 24,
              hide: true,
              prop: "memo",
            },
            {
              label: "控件类型",
              prop: "ctltype",
              type: "select",
              display: false,
              dicData: [
                {
                  label: "单行文本",
                  value: "input",
                },
                {
                  label: "数字",
                  value: "number",
                },
                {
                  label: "多行文本",
                  value: "textarea",
                },
                {
                  label: "下拉选择",
                  value: "select",
                },
                {
                  label: "开关",
                  value: "switch",
                },
                {
                  label: "日期时间",
                  value: "datetime",
                },
                {
                  label: "树形选择",
                  value: "tree",
                },
              ],
              span: 8,
            },
            {
              label: "是否可搜索",
              prop: "search",
              type: "select",
              value: false,
              dicData: [
                {
                  label: "是",
                  value: true,
                },
                {
                  label: "否",
                  value: false,
                },
              ],
              span: 8,
            },
            {
              type: "input",
              label: "表格宽度(width)",
              span: 8,
              prop: "width",
            },
            {
              type: "input",
              label: "表单宽度(span)",
              span: 8,
              prop: "span",
            },
            {
              label: "是否必填",
              prop: "required",
              type: "select",
              hide: true,
              value: true,
              display: false,
              dicData: [
                {
                  label: "是",
                  value: true,
                },
                {
                  label: "否",
                  value: false,
                },
              ],
              span: 8,
            },
            {
              type: "input",
              label: "规则",
              span: 8,
              hide: true,
              prop: "rules",
            },
            {
              label: "字典类型",
              prop: "dicDataType",
              type: "select",
              value: 0,
              display: false,
              hide: true,
              dicData: [
                {
                  label: "静态",
                  value: 0,
                },
                {
                  label: "系统字典",
                  value: 1,
                },
                // {
                //   label: '远端数据',
                //   value: 2
                // },
                {
                  label: "自定义字典",
                  value: 3,
                },
              ],
              span: 8,
            },
            {
              label: "字典值",
              prop: "dicDataText",
              type: "input",
              hide: true,
              display: false,
              span: 8,
            },
            {
              label: "字典调用名/链接",
              prop: "dicDataCallCode",
              type: "input",
              hide: true,
              display: false,
              span: 8,
            },
            {
              label: "是否多选",
              prop: "multiple",
              type: "select",
              hide: true,
              value: false,
              display: false,
              dicData: [
                {
                  label: "是",
                  value: true,
                },
                {
                  label: "否",
                  value: false,
                },
              ],
              span: 8,
            },
            {
              label: "是否能选父节点",
              prop: "parentAble",
              type: "select",
              value: false,
              hide: true,
              display: false,
              dicData: [
                {
                  label: "是",
                  value: true,
                },
                {
                  label: "否",
                  value: false,
                },
              ],
              span: 8,
            },
            {
              label: "时间格式",
              prop: "format", //valueFormat
              type: "select",
              value: "yyyy-MM-dd HH:mm:ss",
              hide: true,
              display: false,
              dicData: [
                {
                  label: "yyyy-MM-dd HH:mm:ss",
                  value: "yyyy-MM-dd HH:mm:ss",
                },
                {
                  label: "yyyy-MM-dd HH:mm",
                  value: "yyyy-MM-dd HH:mm",
                },
                {
                  label: "MM-dd HH:mm:ss",
                  value: "MM-dd HH:mm:ss",
                },
                {
                  label: "MM-dd HH:mm",
                  value: "MM-dd HH:mm",
                },
                {
                  label: "yyyy-MM-dd",
                  value: "yyyy-MM-dd",
                },
              ],
              span: 8,
            },
            {
              label: "开启超出隐藏",
              prop: "overHidden",
              type: "select",
              value: true,
              hide: true,
              display: false,
              dicData: [
                {
                  label: "开启",
                  value: true,
                },
                {
                  label: "关闭",
                  value: false,
                },
              ],
              span: 8,
            },
            {
              label: "表单增改显示",
              prop: "display",
              type: "select",
              hide: true,
              value: true,
              display: false,
              dicData: [
                {
                  label: "显示",
                  value: true,
                },
                {
                  label: "不显示",
                  value: false,
                },
              ],
              span: 8,
            },
            {
              label: "表格显示",
              prop: "hide",
              type: "select",
              hide: true,
              value: false,
              display: false,
              dicData: [
                {
                  label: "隐藏",
                  value: true,
                },
                {
                  label: "显示",
                  value: false,
                },
              ],
              span: 8,
            },
            {
              label: "开启表格插槽",
              prop: "slot",
              type: "select",
              hide: true,
              value: false,
              display: false,
              dicData: [
                {
                  label: "开启",
                  value: true,
                },
                {
                  label: "关闭",
                  value: false,
                },
              ],
              span: 8,
            },
            {
              label: "表格插槽类型",
              prop: "slotType",
              type: "select",
              hide: true,
              value: "",
              display: false,
              dicData: [
                {
                  label: "自定义",
                  value: "",
                },
                {
                  label: "头像",
                  value: "head",
                },
                {
                  label: "图片展示",
                  value: "imgs",
                },
                {
                  label: "标签",
                  value: "tag",
                },
              ],
              span: 8,
            },
            {
              label: "是否只读",
              prop: "readonly",
              type: "select",
              hide: true,
              value: false,
              display: false,
              dicData: [
                {
                  label: "是",
                  value: true,
                },
                {
                  label: "否",
                  value: false,
                },
              ],
              span: 8,
            },
            {
              label: "开启表单插槽",
              prop: "formslot",
              type: "select",
              hide: true,
              value: false,
              display: false,
              dicData: [
                {
                  label: "开启",
                  value: true,
                },
                {
                  label: "关闭",
                  value: false,
                },
              ],
              span: 8,
            },
            {
              label: "表单插槽类型",
              prop: "formslotType",
              type: "select",
              hide: true,
              value: "",
              display: false,
              dicData: [
                {
                  label: "自定义",
                  value: "",
                },
                {
                  label: "头像",
                  value: "head",
                },
              ],
              span: 8,
            },
          ],
        },
      },
      jsonDataCrud: {},
      jsonDataCrudY: {},
      treeLists: [],
    };
  },
  components: {
    Layouts,
    Tree,
    JsonEditor,
    AddTo,
  },
  watch: {
    "tabsOption.tableForm": {
      handler(val) {
        if (this.timer) {
          clearTimeout(this.timer);
        }
        this.timer = setTimeout(() => {
          this.changeDispaly(val);
        }, 30);
      },
      deep: true,
    },
    "tabsOption.activeName": {
      handler(value) {
        if (value == "crud") {
          try {
            this.createFormCrud();
          } catch (error) {
            console.error(error);
          }
        }
        if (value == "yuanshuju") {
          try {
            this.createFormCrudY();
          } catch (error) {
            console.error(error);
          }
        }
      },
    },
  },
  created() {
    window.GFAPI.dev.MetaApi.load().then((res) => {
      this.treeLists = res.data;
    });
  },
  mounted() {},
  methods: {
    //左侧树点击
    TreeClick(obj) {
      if (this.tabsOption.item.sortChange) {
        this.$confirm("排序未保存, 是否继续?", "提示", {
          confirmButtonText: "确定",
          cancelButtonText: "取消",
          type: "warning",
        }).then(() => {
          this.TreeClicked(obj);
        });
      } else {
        this.TreeClicked(obj);
      }
    },
    TreeClicked({ data }) {
      this.tabsOption.tableSelect = []; //重置选中的内容
      if (data.iscallcode) {
        this.selectNode = {};
        this.tabsOption.item = {}; //记录node
        this.tabsOption.list = []; //表格数据赋值
      } else {
        this.selectNode = data;
        let ondeItem = JSON.parse(JSON.stringify(data));
        this.tabsOption.item = ondeItem; //记录node
        ondeItem.extobj.cols = ondeItem.extobj.cols || [];
        ondeItem.extobj.cols.forEach((el) => {
          el.clstype = el.ht && el.ht.clstype ? el.ht.clstype : "";
          el.default = el.sj && el.sj.default ? el.sj.default : "";
          el.isNull = el.sj && el.sj.isNull ? el.sj.isNull : false;
          el.length = el.sj && el.sj.length ? el.sj.length : "";
          el.sqltype = el.sj && el.sj.sqltype ? el.sj.sqltype : "";
        });
        this.tabsOption.list = ondeItem.extobj.cols; //表格数据赋值
        this.createFormCrudY(); //同时生成对应元数据配置
        this.createFormCrud(); //同时生成对应crud配置
      }
    },
    //tag内容点击复制
    handleCellClick(str) {
      this.$Clipboard({
        text: str,
      })
        .then(() => {
          this.$message.success("复制成功");
        })
        .catch(() => {
          this.$message.error("复制失败");
        });
    },
    //创建列名
    handleCreate(row, done) {
      let ht = { clstype: row.clstype };
      let sj = {
        default: row.default,
        isNull: row.isNull,
        length: row.length,
        sqltype: row.sqltype,
      };
      row.ht = ht;
      row.sj = sj;
      this.tabsOption.list.push(row);
      let item = this.tabsOption.item;
      item.ext = JSON.stringify(item.extobj);

      window.GFAPI.dev.MetaApi.edit(item).then((res) => {
        if (res.code === 0) {
          this.tabsOption.item.sortChange = false;
          this.refreshTree();
          done();
          this.$message({
            message: "新增成功",
            type: "success",
            center: true,
            duration: 1000,
          });
        }
      });
    },
    //删除列
    handleDelete(row, index) {
      this.$confirm("请确认是否删除当前项, 再继续?", "提示", {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        type: "warning",
      }).then(() => {
        this.tabsOption.list.splice(index, 1);
        let item = this.tabsOption.item;
        item.ext = JSON.stringify(item.extobj);

        window.GFAPI.dev.MetaApi.edit(item).then((res) => {
          if (res.code === 0) {
            this.tabsOption.item.sortChange = false;
            this.refreshTree();
            this.$message({
              message: "删除成功",
              type: "success",
              center: true,
              duration: 1000,
            });
          }
        });
      });
    },
    //编辑列名
    handleUpdate(row, index, done) {
      let ht = { clstype: row.clstype };
      let sj = {
        default: row.default,
        isNull: row.isNull,
        length: row.length,
        sqltype: row.sqltype,
      };
      row.ht = ht;
      row.sj = sj;
      this.tabsOption.list[index] = row;
      let item = this.tabsOption.item;
      item.ext = JSON.stringify(item.extobj);

      window.GFAPI.dev.MetaApi.edit(item).then((res) => {
        if (res.code === 0) {
          this.tabsOption.item.sortChange = false;
          this.refreshTree();
          done();
          this.$message({
            message: "编辑成功",
            type: "success",
            center: true,
            duration: 1000,
          });

          this.TreeClicked({ data: item });
        }
      });
    },
    //配置显隐表单
    changeDispaly(row) {
      let whiteArr = [
        "colname",
        "desc",
        "memo",
        "iscol",
        "sqltype",
        "length",
        "default",
        "isNull",
        "clstype",
      ];
      if (row.iscol) {
        whiteArr.push(
          "slot",
          "ctltype",
          "formslot",
          "overHidden",
          "display",
          "hide",
          "required",
          "readonly",
          "rules",
          "span",
          "width"
        );
        if (row.slot) {
          whiteArr.push("slotType");
        }
        if (row.formslot) {
          whiteArr.push("formslotType");
        }
        let type = row.ctltype;
        switch (type) {
          case "select":
            if (!row.dicDataType) {
              whiteArr.push("dicDataText");
            } else {
              whiteArr.push("dicDataCallCode");
            }
            if (!row.multiple) {
              whiteArr.push("search");
            }
            whiteArr.push("dicDataType", "multiple");
            break;
          case "tree":
            if (!row.dicDataType) {
              whiteArr.push("dicDataText");
            } else {
              whiteArr.push("dicDataCallCode");
            }
            if (!row.multiple) {
              whiteArr.push("search");
            }
            whiteArr.push("dicDataType", "multiple", "parentAble");
            break;
          case "datetime":
            whiteArr.push("search");
            whiteArr.push("format");
            break;
          case "input":
            whiteArr.push("search");
            break;
          default:
            break;
        }
      }
      let colmunArr = this.TableData.option.column;
      colmunArr.forEach((e) => {
        this.$set(e, "display", whiteArr.indexOf(e.prop) != -1);
      });
    },
    //表格选中内容
    selectionChange(arr) {
      this.tabsOption.tableSelect = arr;
    },
    //选中内容添加到...
    handleAddTo() {
      this.$refs.addTo.dialogVisible = true;
      var option = this.$refs.addTo.option.column.find(
        (ele) => ele.prop == "selects"
      );
      var dicData = [];
      let treeLists = JSON.parse(JSON.stringify(this.treeLists));

      treeLists.forEach((ele) => {
        var obj = {
          cname: `${ele.tname}(${ele.cname})`,
          id: ele.id,
        };
        if (ele.id != this.selectNode.id) {
          dicData.push(obj);
        }
      });
      this.$set(option, "dicData", dicData);
      this.$refs.addTo.data = {
        table: treeLists,
        content: this.tabsOption.tableSelect,
      };
    },
    //格式化选中内容
    formatRow(arr) {
      return arr;
    },
    //排序保存
    handleSortSave() {
      let item = this.tabsOption.item;
      item.ext = JSON.stringify(item.extobj);
      window.GFAPI.dev.MetaApi.edit(item).then((res) => {
        if (res.code === 0) {
          this.tabsOption.item.sortChange = false;
          this.refreshTree();
          this.$message({
            message: "排序保存成功",
            type: "success",
            center: true,
            duration: 1000,
          });
        }
      });
      try {
        this.tabsOption.item.sortChange = false;
      } catch (error) {
        console.error(error);
      }
    },
    //切换排序保存按钮提示
    sortableChange(a, b, c, d) {
      this.tabsOption.list = [];
      this.$nextTick(() => {
        this.$set(this.tabsOption, "list", d);
      });
      this.tabsOption.item.sortChange = true;
    },
    //crud配置复制到剪切板
    handleCopyCrud() {
      this.handleCellClick(this.$refs.jsonEditorCrud.getValue());
    },
    //crud配置格式化函数
    createFormCrud() {
      var cols = this.tabsOption.list;
      var columns = [];
      // 格式化逻辑
      cols.forEach((ele) => {
        let obj = {};
        if (ele.iscol) {
          //在这里管理字段配置
          obj.label = ele.desc;
          obj.prop = ele.colname;
          obj.span = Number(ele.span) || 12;
          if (ele.width) {
            Number((obj.width = ele.width));
          }
          obj.type = ele.ctltype || "input";
          if (ele.ctltype == "number" && ele.clstype === "init") {
            obj.align = "right";
          } else {
            obj.align = "left";
          }
          if (ele.readonly) {
            obj.readonly = ele.readonly;
          }
          if (
            ele.search &&
            (ele.ctltype == "input" ||
              ele.ctltype == "datetime" ||
              ele.ctltype == "select")
          ) {
            obj.search = ele.search;
            if (ele.ctltype == "datetime") {
              obj.searchRange = true;
            }
          }
          if (ele.slot) {
            obj.slot = ele.slot;
            ele.slotType ? (obj.slotType = ele.slotType) : null;
          }
          if (ele.formslot) {
            obj.formslot = ele.formslot;
            ele.formslotType ? (obj.formslotType = ele.formslotType) : null;
          }
          if (ele.overHidden) {
            obj.overHidden = ele.overHidden;
          }
          if (!ele.display) {
            obj.display = ele.display;
          }
          if (ele.hide) {
            obj.hide = ele.hide;
          }
          if (ele.ctltype === "datetime") {
            obj.format = ele.format;
            obj.valueFormat = "timestamp";
          }
          if (ele.ctltype === "tree") {
            if (!ele.parentAble) {
              obj.parent = ele.parentAble;
            }
          }
          if (ele.ctltype === "tree" || ele.ctltype === "select") {
            if (ele.dicDataType === 0) {
              try {
                obj.dicData = eval(ele.dicDataText);
              } catch (error) {
                console.error(`${ele.colname}_${ele.desc} 列,字典值填写错误`);
                obj.dicData = [];
              }
            } else if (ele.dicDataType === 3) {
              obj.dicDataCallCode = ele.colname;
            } else {
              obj.dicDataType = ele.dicDataType;
              obj.dicDataCallCode = ele.dicDataCallCode;
            }
            if (ele.multiple) {
              obj.multiple = ele.multiple;
            }
          }
          if (ele.required) {
            obj.required = ele.required;
            let boo =
              obj.type == "number" ||
              obj.type === "input" ||
              obj.type === "textarea"; //确定验证语句
            obj.rules = [
              {
                required: true,
                message: boo ? `${obj.label}必须填写` : `请选择${obj.label}`,
                trigger: "blur",
              },
            ];
          }
          if (ele.rules) {
            if (ele.required) {
              obj.rules.push({ validator: ele.rules, trigger: "blur" });
            } else {
              obj.rules = [{ validator: ele.rules, trigger: "blur" }];
            }
          }

          columns.push(obj);
        }
      });
      let searchLeng = cols.filter((el) => el.search).length;
      //格式化内容赋值
      var options = {
        index: true,
        border: true,
        clearable: true,
        refreshBtn: true, //刷新按钮
        header: true, //头部显隐按钮
        addBtn: true,
        delBtn: true,
        editBtn: true,
        menu: true,
        menuType: "text",
        menuWidth: "150",
        height: "670",
        menuAlign: "center",
        menuPosition: "right",
        headerAlign: "center",
        dialogEscape: false,
        dialogClickModal: false,
        dialogFullscreen: false,
        searchMenuSpan: 24 - searchLeng * 6,
        searchIndex: 3,
        emptySize: "medium", //空数据时显示的文本内容大小
        column: columns,
      };
      this.jsonDataCrud = options;
    },
    //元数据配置复制到剪切板
    handleCopyCrudY() {
      this.handleCellClick(this.$refs.jsonEditorCrudY.getValue());
    },
    //元数据配置格式化函数
    createFormCrudY() {
      var cols = this.tabsOption.list;
      var columns = [];
      // 格式化逻辑
      cols.forEach((ele) => {
        columns.push({
          colname: ele.colname,
          desc: ele.desc,
          memo: ele.memo,
          sj: {
            sqltype: ele.sqltype,
            isNull: ele.isNull,
            default: ele.default,
            length: ele.length,
          },
          ht: {
            clstype: ele.clstype,
          },
        });
      });
      //格式化内容赋值
      var options = {
        name: this.selectNode.cname,
        cname: this.selectNode.tname,
        cols: columns,
      };
      this.jsonDataCrudY = options;
    },
    //元数据导出
    handleExport() {
      var editor = JSON.parse(this.$refs.jsonEditorCrudY.getValue());
      import("@/framework/util/vendor/ExportJson").then((json) => {
        json.export_json(this.$refs.jsonEditorCrudY.getValue(), editor.name);
      });
    },
    //树更新及其更新后逻辑
    refreshTree() {
      this.$refs.trees.getList();
    },
  },
};
</script>

<style scoped lang="scss">
/deep/.el-tabs {
  height: 100%;
  .el-tabs__content {
    height: calc(100% - 55px);
    .el-tab-pane {
      height: 100%;
      .json-editor {
        height: calc(100% - 47px);
        overflow-x: auto;
        margin-top: 15px;
      }
    }
  }
}
</style>
