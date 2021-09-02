<template>
  <div id="avue-view">
    <div class="head">
      <div class="title">
        元数据管理列表
        <div
          class="color-div"
          :style="{ 'background-color': $store.state.common.colorName }"
        ></div>
      </div>
    </div>
    <el-container>
      <el-main>
        <div class="crudbox">
          <el-container>
            <el-aside class="aside-main">
              <avue-tree
                @node-click="handleTabClick"
                v-loading="tabLoading"
                v-model="tabForm"
                @update="handleTabUpdate"
                @save="handleTabCreate"
                :option="treeOptions"
                :data="treeLists"
                highlight-current
              >
                <template slot-scope="scope" slot="menuBtn">
                  <el-dropdown-item
                    @click.native="handleTabDel(scope.node, scope.data)"
                    >删除</el-dropdown-item
                  >
                </template>
              </avue-tree>
            </el-aside>
            <el-main class="table-main" v-if="showRow">
              <div class="table-main-info" v-if="treeData.count > 0">
                <div class="header">
                  <span class="title"
                    >{{ treeData.data.tname }}({{ treeData.data.cname }})</span
                  >
                </div>
              </div>
              <el-tabs v-model="activeName">
                <el-tab-pane label="表格字段" name="table">
                  <avue-crud
                    :option="rowOption"
                    :data="rowList"
                    ref="crud"
                    :table-loading="loading"
                    @refresh-change="handleFresh"
                    @row-save="handleCreate"
                    @row-del="handleDelete"
                    @row-update="handleUpdate"
                    @selection-change="handleSelect"
                    @sortable-change="sortableChange"
                  >
                    <template slot-scope="{}" slot="menuLeft">
                      <el-button
                        type="primary"
                        icon="el-icon-folder-add"
                        size="small"
                        :disabled="!selection.length"
                        @click.stop="handleAddTo"
                        >添加到</el-button
                      >
                      <el-button
                        type="primary"
                        icon="el-icon-document-checked"
                        size="small"
                        @click.stop="handleSortSave"
                        >拖动保存</el-button
                      >
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
                    <template slot-scope="scope" slot="menu">
                      <el-button
                        type="text"
                        icon="el-icon-edit"
                        size="small"
                        @click.stop="handleEdit(scope.row, scope.index)"
                        >编 辑</el-button
                      >
                      <el-button
                        type="text"
                        icon="el-icon-delete"
                        size="small"
                        @click.stop="handleDel(scope.row, scope.index)"
                        >删 除</el-button
                      >
                    </template>
                  </avue-crud>
                </el-tab-pane>
                <el-tab-pane label="元数据" name="meta">
                  <div style="padding: 0 10px">
                    <el-button
                      type="primary"
                      icon="el-icon-upload"
                      size="small"
                      @click.stop="handleExport()"
                      >导 出</el-button
                    >
                    <el-button
                      type="primary"
                      size="small"
                      @click.stop="handleCopy()"
                      >复制到剪切板</el-button
                    >
                  </div>
                  <json-editor ref="jsonEditor" v-model="jsonData" />
                </el-tab-pane>
                <el-tab-pane label="表单配置" name="form">
                  <div style="padding: 0 10px">
                    <el-button
                      type="primary"
                      size="small"
                      @click.stop="handleCopyForm()"
                      >复制到剪切板</el-button
                    >
                  </div>
                  <json-editor ref="jsonEditorForm" v-model="jsonDataForm" />
                </el-tab-pane>
                <el-tab-pane label="CRUD配置" name="crud">
                  <div style="padding: 0 10px">
                    <el-button
                      type="primary"
                      size="small"
                      @click.stop="handleCopyCrud()"
                      >复制到剪切板</el-button
                    >
                  </div>
                  <json-editor ref="jsonEditorCrud" v-model="jsonDataCrud" />
                </el-tab-pane>
              </el-tabs>
            </el-main>
          </el-container>
        </div>
      </el-main>
    </el-container>
    <add-to ref="addTo" />
  </div>
</template>

<script>
import JsonEditor from "@/framework/page/JsonEditor";
import AddTo from "./d_addto_dialog";
export default {
  name: "meta",
  components: { JsonEditor, AddTo },
  data() {
    return {
      jsonData: {},
      jsonDataCrud: {},
      jsonDataForm: {},
      treeData: {
        count: 0,
        data: {},
      },
      selection: [],
      loading: true,
      treeLists: [],
      treeOptions: {
        nodeKey: "id",
        size: "small",
        delBtn: false,
        addBtn: true,
        formOption: {
          labelWidth: 100,
          menuPosition: "right",
          column: [
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
        props: {
          labelText: "中文表名",
          label: "tname",
          value: "id",
          children: "children",
        },
      },
      showRow: false,
      tabForm: {},
      tabLoading: true,
      activeName: "table",
      rowList: [],
      rowData: {},
      rowOption: {
        addBtn: true,
        delBtn: false,
        editBtn: false,
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
        column: [
          {
            label: "列名（英文）",
            prop: "colname",
            slot: true,
            type: "input",
            size: "small",
            align: "center",
            span: 24,
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
            span: 24,
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
            type: "textarea",
            label: "备注",
            span: 24,
            display: true,
            hide: true,
            prop: "memo",
          },
          {
            label: "控件类型",
            prop: "ctltype",
            type: "select",
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
                label: "单选框",
                value: "radio",
              },
              {
                label: "开关",
                value: "switch",
              },
              {
                label: "多选框",
                value: "checkbox",
              },
              {
                label: "日期时间",
                value: "datetime",
              },
              {
                label: "日期",
                value: "date",
              },
              {
                label: "时间",
                value: "time",
              },
              {
                label: "树形选择",
                value: "tree",
              },
              {
                label: "图标选择",
                value: "icon",
              },
            ],
            span: 24,
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
            span: 24,
          },
          {
            type: "input",
            label: "数据库长度",
            span: 12,
            display: true,
            hide: true,
            prop: "length",
          },
          {
            type: "input",
            label: "数据库默认值",
            span: 12,
            display: true,
            hide: true,
            prop: "default",
          },
          {
            type: "switch",
            label: "数据库为空",
            hide: true,
            span: 12,
            display: true,
            valueDefault: false,
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
            span: 24,
          },
        ],
      },
    };
  },
  watch: {
    activeName(value) {
      if (value == "form") {
        setTimeout(() => {
          this.createFormJson();
        }, 0);
      }
      if (value == "crud") {
        setTimeout(() => {
          this.createFormCrud();
        }, 0);
      }
      if (value == "meta") {
        setTimeout(() => {
          this.jsonData = JSON.parse(this.treeData.data.ext);
        }, 0);
      }
    },
  },
  created() {
    this.getList();
  },
  methods: {
    handleSortSave() {
      var that = this;
      var ext = JSON.parse(that.treeData.data.ext);
      ext.cols = that.formatList(that.rowList);
      var data = JSON.parse(JSON.stringify(that.treeData.data));
      data.ext = JSON.stringify(ext);

      window.GFAPI.dev.MetaApi.edit(data)
        .then((res) => {
          if (res.code == 0) {
            that.$message({
              message: "保存成功",
              type: "success",
              center: true,
              duration: 1000,
              onClose() {
                that.getRowView();
              },
            });
          }
        })
        .catch(() => {});
    },

    sortableChange() {},

    handleAddTo() {
      this.$refs.addTo.dialogVisible = true;
      var option = this.$refs.addTo.option.column.find(
        (ele) => ele.prop == "selects"
      );
      var dicData = [];
      this.treeLists.forEach((ele) => {
        var obj = {
          cname: `${ele.tname}(${ele.cname})`,
          id: ele.id,
        };
        if (ele.id != this.treeData.data.id) {
          dicData.push(obj);
        }
      });
      this.$set(option, "dicData", dicData);
      this.$refs.addTo.data = {
        table: this.treeLists,
        content: this.selection,
      };
    },

    handleSelect(list) {
      this.selection = JSON.parse(JSON.stringify(list));
    },

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

    handleTabDel(node, data) {
      var that = this;
      that
        .$confirm("请确认是否删除当前项, 再继续?", "提示", {
          confirmButtonText: "确定",
          cancelButtonText: "取消",
          type: "warning",
        })
        .then(() => {
          window.GFAPI.dev.MetaApi.del(data.id)
            .then(() => {
              that.tabLoading = true;
              that.$message({
                message: "删除成功",
                type: "success",
                center: true,
                duration: 1000,
                onClose() {
                  that.getList();
                },
              });
            })
            .catch(() => {});
        })
        .catch(() => {});
    },

    handleCopyForm() {
      this.handleCellClick(this.$refs.jsonEditorForm.getValue());
    },

    handleCopyCrud() {
      this.handleCellClick(this.$refs.jsonEditorCrud.getValue());
    },

    handleCopy() {
      this.handleCellClick(this.$refs.jsonEditor.getValue());
    },

    handleExport() {
      var editor = JSON.parse(this.$refs.jsonEditor.getValue());
      import("@/framework/util/vendor/ExportJson").then((json) => {
        json.export_json(this.$refs.jsonEditor.getValue(), editor.name);
      });
    },

    handleTabClick(data) {
      this.loading = true;
      setTimeout(() => {
        this.treeData.data = data;
        this.jsonData = JSON.parse(data.ext);
        var list = JSON.parse(data.ext).cols;
        var dealist = this.formatList(list);
        this.rowList = this.creareRandomId(dealist);
        this.createFormJson();
        this.createFormCrud();
        this.loading = false;
        this.showRow = true;
      }, 500);
    },

    createFormCrud() {
      var cols = JSON.parse(this.treeData.data.ext).cols;
      var columns = [];
      cols.forEach((ele) => {
        if (!ele.jm || !ele.jm.ctltype) {
          if (
            (ele.colname === "cstatus" || ele.colname === "flag") &&
            ele.memo
          ) {
            let temp = ele.memo.split(",");
            let dicData = temp.map((e) => {
              let data = e.split("-");
              return { label: data[1], value: parseInt(data[0]) };
            });
            var objSys = {
              type: "select",
              label: ele.desc,
              prop: ele.colname,
              dicData,
            };
            columns.push(objSys);
          }
        } else {
          var obj = {
            type: ele.jm.ctltype,
            label: ele.desc,
            prop: ele.colname,
          };
          var type = ele.jm.ctltype;
          if (type == "select" || type == "radio" || type == "checkbox") {
            obj.dicData = [
              {
                label: "选项一",
                value: 1,
              },
              {
                label: "选项二",
                value: 2,
              },
            ];
            obj.rules = [
              {
                required: true,
                message: `请选择${ele.desc}`,
                trigger: "change",
              },
            ];
            obj.props = {
              label: "label",
              value: "value",
            };
          } else if (type == "switch") {
            obj.dicData = [
              {
                label: "否",
                value: "0",
              },
              {
                label: "是",
                value: "1",
              },
            ];
            obj.props = {
              label: "label",
              value: "value",
            };
            obj.rules = [
              {
                required: true,
                message: `请选择${ele.desc}`,
                trigger: "change",
              },
            ];
          } else if (type == "tree") {
            obj.parent = false;
            obj.dicData = [
              {
                label: "选项一",
                value: "0",
                children: [
                  {
                    label: "选项二",
                    value: "1",
                  },
                  {
                    label: "选项三",
                    value: "2",
                  },
                ],
              },
            ];
            obj.props = {
              label: "label",
              value: "value",
            };
            obj.rules = [
              {
                required: true,
                message: `请选择${ele.desc}`,
                trigger: "change",
              },
            ];
          } else if (type == "datetime" || type == "date" || type == "time") {
            obj.rules = [
              {
                required: true,
                message: `请选择${ele.desc}`,
              },
            ];
            if (type == "datetime") {
              obj.format = "yyyy-MM-dd HH:mm:ss";
              obj.valueFormat = "yyyy-MM-dd HH:mm:ss";
            }
            if (type == "date") {
              obj.format = "yyyy-MM-dd";
              obj.valueFormat = "yyyy-MM-dd";
            }
            if (type == "time") {
              obj.format = "HH:mm:ss";
              obj.valueFormat = "HH:mm:ss";
            }
          } else {
            obj.rules = [
              {
                required: true,
                message: `请输入${ele.desc}`,
                trigger: "blur",
              },
            ];
          }
          if (type != "icon") {
            columns.push(obj);
          }
        }
      });
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
        menuAlign: "center",
        menuPosition: "right",
        headerAlign: "center",
        dialogEscape: false,
        dialogClickModal: false,
        dialogFullscreen: false,
        emptySize: "medium", //空数据时显示的文本内容大小
        column: columns,
      };
      this.jsonDataCrud = options;
    },

    createFormJson() {
      var cols = JSON.parse(this.treeData.data.ext).cols;
      var columns = [];
      cols.forEach((ele) => {
        if (!ele.jm || !ele.jm.ctltype) return;
        var obj = {
          type: ele.jm.ctltype,
          label: ele.desc,
          prop: ele.colname,
        };
        var type = ele.jm.ctltype;
        if (type == "select" || type == "radio" || type == "checkbox") {
          obj.dicData = [
            {
              label: "选项一",
              value: 1,
            },
            {
              label: "选项二",
              value: 2,
            },
          ];
          obj.rules = [
            {
              required: true,
              message: `请选择${ele.desc}`,
              trigger: "change",
            },
          ];
          obj.props = {
            label: "label",
            value: "value",
          };
        } else if (type == "switch") {
          obj.dicData = [
            {
              label: "否",
              value: "0",
            },
            {
              label: "是",
              value: "1",
            },
          ];
          obj.props = {
            label: "label",
            value: "value",
          };
          obj.rules = [
            {
              required: true,
              message: `请选择${ele.desc}`,
              trigger: "change",
            },
          ];
        } else if (type == "tree") {
          obj.parent = false;
          obj.dicData = [
            {
              label: "选项一",
              value: "0",
              children: [
                {
                  label: "选项二",
                  value: "1",
                },
                {
                  label: "选项三",
                  value: "2",
                },
              ],
            },
          ];
          obj.props = {
            label: "label",
            value: "value",
          };
          obj.rules = [
            {
              required: true,
              message: `请选择${ele.desc}`,
              trigger: "change",
            },
          ];
        } else if (type == "datetime" || type == "date" || type == "time") {
          obj.rules = [
            {
              required: true,
              message: `请选择${ele.desc}`,
            },
          ];
          if (type == "datetime") {
            obj.format = "yyyy-MM-dd HH:mm:ss";
            obj.valueFormat = "yyyy-MM-dd HH:mm:ss";
          }
          if (type == "date") {
            obj.format = "yyyy-MM-dd";
            obj.valueFormat = "yyyy-MM-dd";
          }
          if (type == "time") {
            obj.format = "HH:mm:ss";
            obj.valueFormat = "HH:mm:ss";
          }
        } else {
          obj.rules = [
            {
              required: true,
              message: `请输入${ele.desc}`,
              trigger: "blur",
            },
          ];
        }
        if (type != "icon") {
          columns.push(obj);
        }
      });
      var options = {
        column: columns,
        labelPosition: "right",
        labelWidth: 120,
        menuBtn: true,
        submitBtn: true,
        submitSize: "medium",
        submitText: "提交",
        emptyBtn: true,
        emptySize: "medium",
        emptyText: "无数据",
        menuPosition: "right",
      };
      this.jsonDataForm = options;
    },

    formatList(list) {
      var dealist = list.map((ele) => {
        if (ele.jm && ele.jm.ctltype) {
          ele["ctltype"] = ele.jm.ctltype;
        } else {
          ele["ctltype"] = "";
        }

        if (ele.sj && ele.sj.sqltype) {
          ele["sqltype"] = ele.sj.sqltype;
        } else {
          ele["sqltype"] = "";
        }

        if (ele.sj && ele.sj.length) {
          ele["length"] = ele.sj.length;
        } else {
          ele["length"] = "";
        }

        if (ele.sj && ele.sj.default) {
          ele["default"] = ele.sj.default;
        } else {
          ele["default"] = "";
        }

        if (ele.sj && ele.sj.isNull) {
          ele["isNull"] = ele.sj.isNull;
        } else {
          ele["isNull"] = false;
        }

        if (ele.ht && ele.ht.clstype) {
          ele["clstype"] = ele.ht.clstype;
        } else {
          ele["clstype"] = "";
        }
        return ele;
      });
      return dealist;
    },

    creareRandomId(arr) {
      var list = arr.map((ele, index) => {
        ele.id =
          Date.now().toString() + getRandomIntInclusive(100, 999) + index;
        return ele;
      });
      function getRandomIntInclusive(min, max) {
        min = Math.ceil(min);
        max = Math.floor(max);
        return Math.floor(Math.random() * (max - min + 1)) + min; //含最大值，含最小值
      }
      return list;
    },

    getList() {
      window.GFAPI.dev.MetaApi.load()
        .then((res) => {
          this.treeLists = res.data;
          this.treeData.count = res.count;
          this.tabLoading = false;
          this.showRow = false;
        })
        .catch(() => {
          this.treeLists = [];
          this.treeData.count = 0;
          this.tabLoading = false;
          this.showRow = false;
        });
    },

    handleTabUpdate(data, node, done) {
      var that = this;
      let temp = JSON.parse(data.ext);
      temp.name = data.cname;
      temp.cname = data.tname;
      data.ext = JSON.stringify(temp);
      window.GFAPI.dev.MetaApi.edit(data)
        .then((res) => {
          that.tabLoading = true;
          if (res.code == 0) {
            done();
            that.$message({
              message: "编辑成功",
              type: "success",
              center: true,
              duration: 1000,
              onClose() {
                that.getList();
              },
            });
          }
        })
        .catch(() => {
          done();
        });
    },

    handleTabCreate(data, node, done) {
      var that = this;
      let temp = Object.assign(this.tabForm, {
        ext: JSON.stringify({
          name: this.tabForm.cname,
          cname: this.tabForm.tname,
          cols: [],
        }),
      });

      window.GFAPI.dev.MetaApi.add(temp)
        .then((res) => {
          that.tabLoading = true;
          if (res.code == 0) {
            done();
            that.$message({
              message: "新增成功",
              type: "success",
              center: true,
              duration: 1000,
              onClose() {
                that.getList();
              },
            });
          }
        })
        .catch(() => {
          done();
        });
    },

    formatRow(row) {
      var obj = {
        colname: row.colname,
        desc: row.desc,
        memo: row.memo,
      };
      if (row.ctltype) {
        var jm = {
          ctltype: row.ctltype,
        };
        obj.jm = jm;
      }
      if (row.sqltype) {
        var sj = {
          sqltype: row.sqltype,
          isNull: row.isNull,
          default: row.default,
          length: row.length,
        };
        obj.sj = sj;
      }
      if (row.clstype) {
        var ht = {
          clstype: row.clstype,
        };
        obj.ht = ht;
      }
      return obj;
    },

    getRowView() {
      this.loading = true;

      window.GFAPI.dev.MetaApi.get(this.treeData.data.id).then((res) => {
        this.treeLists.forEach((ele) => {
          if (ele.id == this.treeData.data.id) {
            this.$set(ele, "ext", res.data.ext);
          }
        });
        this.handleTabClick(res.data);
      });
    },

    handleCreate(row, done, loading) {
      var data = JSON.parse(JSON.stringify(this.treeData.data));
      var ext = JSON.parse(data.ext);
      var cext = JSON.parse(JSON.stringify(ext));
      var cols = ext.cols;
      var obj = this.formatRow(row);
      cols.push(obj);
      cext.cols = cols;
      data.ext = JSON.stringify(cext);
      var that = this;

      window.GFAPI.dev.MetaApi.edit(data)
        .then((res) => {
          loading();
          if (res.code == 0) {
            done();
            that.$message({
              message: "新增成功",
              type: "success",
              center: true,
              duration: 1000,
              onClose() {
                that.getRowView();
              },
            });
          }
        })
        .catch(() => {
          done();
        });
    },

    handleFresh() {
      this.getRowView();
    },

    handleDelete() {
      var that = this;
      this.$confirm("请确认是否删除当前项, 再继续?", "提示", {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        type: "warning",
      })
        .then(() => {
          var list = [];
          that.rowList.forEach((ele) => {
            if (ele.id !== that.rowData.id) {
              list.push(JSON.parse(JSON.stringify(ele)));
            }
          });
          var data = JSON.parse(JSON.stringify(that.treeData.data));
          var ext = JSON.parse(data.ext);
          var cext = JSON.parse(JSON.stringify(ext));
          var rows = list.map((ele) => {
            return this.formatRow(ele);
          });
          cext.cols = rows;
          data.ext = JSON.stringify(cext);

          window.GFAPI.dev.MetaApi.edit(data)
            .then((res) => {
              if (res.code == 0) {
                that.$message({
                  message: "删除成功",
                  type: "success",
                  center: true,
                  duration: 1000,
                  onClose() {
                    that.getRowView();
                  },
                });
              }
            })
            .catch(() => {});
        })
        .catch(() => {});
    },

    handleDel(row, index) {
      this.rowData = JSON.parse(JSON.stringify(row));
      this.$refs.crud.rowDel(row, index);
    },

    handleEdit(row, index) {
      this.rowData = JSON.parse(JSON.stringify(row));
      this.$refs.crud.rowEdit(row, index);
    },

    handleUpdate(row, index, done, loading) {
      this.rowList.forEach((ele) => {
        if (ele.id == this.rowData.id) {
          var str = [
            "colname",
            "desc",
            "memo",
            "ctltype",
            "sqltype",
            "length",
            "isNull",
            "default",
            "clstype",
          ];
          str.forEach((e) => {
            this.$set(ele, e, row[e]);
          });
        }
      });
      var that = this;
      var data = JSON.parse(JSON.stringify(this.treeData.data));
      var ext = JSON.parse(data.ext);
      var cext = JSON.parse(JSON.stringify(ext));
      var rows = this.rowList.map((ele) => {
        return this.formatRow(ele);
      });
      cext.cols = rows;
      data.ext = JSON.stringify(cext);

      window.GFAPI.dev.MetaApi.edit(data)
        .then((res) => {
          loading();
          if (res.code == 0) {
            done();
            that.$message({
              message: "编辑成功",
              type: "success",
              center: true,
              duration: 1000,
              onClose() {
                that.getRowView();
              },
            });
          }
        })
        .catch(() => {
          done();
        });
    },
  },
};
</script>
<style scoped lang="scss">
.table-main-info {
  width: 100%;
  .header {
    border-bottom: 1px solid transparent;
    margin-bottom: 0px;
    padding-bottom: 5px;
  }
}
.avue-form__menu--center {
  text-align: right !important;
}
/deep/ .avue-tree__menu {
  right: -180px !important;
}

.el-main {
  padding: 5px !important;
}
.table-main,
.aside-main {
  border-radius: 4px;
  border: 1px solid #e6ebf5;
  padding: 15px !important;
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
/deep/ .json-editor {
  padding: 10px 10px 10px;
}
/deep/ .cm-string {
  color: #2c7ad6 !important;
}
/deep/ .cm-property {
  color: #8cdcfe !important;
}
/deep/ .cm-atom {
  color: #ce723b !important;
}
</style>
