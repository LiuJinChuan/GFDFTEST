<template>
  <div class="Tree">
    <el-input
      style="margin-bottom: 15px"
      size="small"
      placeholder="输入关键字进行过滤"
      v-model="Tree.filterText"
    >
      <el-button
        v-if="TreeData.addBtn"
        @click="Treeappend"
        slot="append"
        icon="el-icon-plus"
      ></el-button>
    </el-input>
    <el-tree
      ref="tree"
      :data="TreeData.list || Tree.data"
      :check-strictly="Tree.strictly"
      :node-key="Tree.key"
      :default-expanded-keys="Tree.defaultExpandFloor"
      :default-expand-all="TreeData.defaultExpandAll"
      :show-checkbox="Tree.showCheckbox"
      :filter-node-method="filterNode"
      :expand-on-click-node="Tree.clickisexpand"
      :props="Tree.defaultProps"
      @node-click="TreeClick"
      @check="TreeCheck"
    >
      <span class="custom-tree-node" slot-scope="{ node, data }">
        <span>{{ node.label }}</span>
        <span style="position: absolute; top: 4px; right: 3px">
          <!--<i class="el-icon-circle-plus-outline" style="margin-right:8px;color:#409eff"-->
          <!--@click.stop="() => append(node,data)"></i>-->
          <slot name="menuRight" :node="node" :data="data"></slot>
          <i
            v-if="TreeData.editBtn && !data.withoutBtn"
            class="el-icon-edit-outline"
            style="margin-right: 8px; color: #409eff"
            @click.stop="() => edit(node, data)"
          ></i>
          <i
            v-if="TreeData.delBtn && !data.withoutBtn"
            class="el-icon-delete-solid"
            style="color: #409eff"
            @click.stop="() => remove(node, data)"
          ></i>
        </span>
      </span>
    </el-tree>
    <el-dialog
      :append-to-body="true"
      :close-on-click-modal="false"
      :title="Tree.dialogTitle"
      :visible.sync="Tree.dialogVisible"
      @close="beforeClose"
      :width="Tree.dialogWidth"
    >
      <div v-if="Tree.treetype == 'treeappend'">
        <avue-form
          ref="form"
          v-model="Tree.form.form"
          :option="Tree.form.option"
        ></avue-form>
      </div>
      <!--<div v-if="Tree.treetype=='nodeappend'">-->
      <!--&lt;!&ndash;<avue-form ref="form" v-model="Tree.form.form" :option="Tree.form.option"></avue-form>&ndash;&gt;-->
      <!--</div>-->
      <div v-if="Tree.treetype == 'nodeedit'">
        <avue-form
          ref="form"
          v-model="Tree.form.form"
          :option="Tree.form.editoption"
        ></avue-form>
      </div>
      <!--<div v-if="Tree.treetype=='removeedit'">-->
      <!--<p>是否删除节点,此操作不可逆,请谨慎操作!</p>-->
      <!--</div>-->

      <span slot="footer" class="dialog-footer">
        <el-button
          size="small"
          icon="el-icon-circle-plus-outline"
          :loading="Tree.btnload"
          type="primary"
          @click="TreeSend"
          >保 存</el-button
        >
        <el-button
          size="small"
          icon="el-icon-circle-close"
          @click="Tree.dialogVisible = false"
          >取 消</el-button
        >
      </span>
    </el-dialog>
  </div>
</template>

<script>
//import { GetDict, GetDictCallCode } from '@/framework/api/dict.js';

export default {
  name: "",
  props: ["TreeData", "ajax"],
  data() {
    return {
      ones: true,
      Tree: {
        nodes: "",
        name: "",
        strictly: false, //是否父子链接，勾选子节点自动勾选父节点，false为勾选父节点，true为不勾选父节点
        treetype: "", //弹窗类型，决定确定按钮的发送请求
        key: "id", //勾选树取值
        isaddall: false,
        defaultExpandFloor: [0], //默认展开第几个
        dialogTitle: "新增", //弹窗的名称
        dialogWidth: "50%", //弹窗宽度
        clickisexpand: false, //点击树是否展开子级
        dialogVisible: false, //弹窗的显示
        showCheckbox: true, //树是否可以勾选
        filterText: "", //过滤树绑定的值
        data: [], //树数据
        defaultProps: {}, //绑定显示和子节点的字符串
        btnload: false, //确定按钮反重提交
        checkdata: [], //勾选的数据
        defaultcheckdata: [], //默认勾选的数据
        selectid: "",
        defaultList: [],
        formdata: {},
        title: "",
        form: {
          option: {
            labelWidth: 120,
            menuBtn: false,
            enter: false,
            size: "small",
            column: [
              {
                label: "选项",
                prop: "text",
                dicdatatype: "",
                type: "select",
                required: true,
                rules: [
                  {
                    required: true,
                    message: "请选择一个选项",
                    trigger: "change",
                  },
                ],
                dicData: [
                  { label: "一级", value: "1" },
                  { label: "二级", value: "2" },
                ],
              },
              {
                label: "账号",
                prop: "text1",
                type: "input",
                required: true,
                rules: [
                  {
                    validator:
                      "(rule, value, callback) => {if (value.length < 6) {callback(new Error('账号不能少于6位'));} else {callback();}}",
                    trigger: "change",
                  },
                ],
                // dicData: getdicbytype('text1')
              },
            ],
          },
          editoption: {
            labelWidth: 150,
            menuBtn: false,
            enter: false,
            size: "small",
            column: [
              {
                label: "选项",
                prop: "text",
                dicdatatype: "",
                type: "select",
                required: true,
                rules: [
                  {
                    required: true,
                    message: "请选择一个选项",
                    trigger: "change",
                  },
                ],
                dicData: [
                  { label: "一级", value: "1" },
                  { label: "二级", value: "2" },
                ],
              },
              {
                label: "账号",
                prop: "text1",
                type: "input",
                required: true,
                rules: [
                  {
                    validator:
                      "(rule, value, callback) => {if (value.length < 6) {callback(new Error('账号不能少于6位'));} else {callback();}}",
                    trigger: "change",
                  },
                ],
                // dicData: getdicbytype('text1')
              },
            ],
          },
          form: {},
        },
      },
    };
  },
  watch: {
    "Tree.filterText"(val) {
      this.$refs.tree.setCheckedKeys([]);
      this.$refs.tree.filter(val);
      this.Tree.checkdata = [];
    },
    "TreeData.node"(val) {
      this.Tree.nodes = val;
    },
  },
  components: {},
  created() {
    for (let key in this.Tree) {
      for (let keys in this.TreeData) {
        if (key == keys) {
          this.Tree[key] = this.TreeData[key];
        }
      }
    }
  },
  mounted() {
    let obj = this.TreeData.ajax || {};
    this.Tree.nodes = this.TreeData.node;
    this.getList();
    this.GetOptionByCode(obj.data);
  },

  methods: {
    /*获取列表*/
    getList() {
      if (!this.TreeData.list) {
        this.TreeData.ajax.Api.load().then((res) => {
          let fun = this.TreeData.ajax.listFormatter;
          if (typeof fun === "function") {
            this.Tree.defaultList = fun(res.data);
            this.Tree.data = fun(res.data);
          } else {
            this.Tree.defaultList = res.data;
            this.Tree.data = res.data;
          }
          if (this.TreeData.isaddall) {
            let a =
              '[{ "cname": "全部",  "' +
              this.TreeData.key +
              '": "0", "children": [] }]';
            let All = JSON.parse(a);
            All[0].children = this.Tree.data;
            this.Tree.data = All;
          }
          if (this.ones) {
            this.$nextTick(() => {
              this.$refs.tree.setCurrentKey(this.Tree.data[0][this.Tree.key]);
              this.ones = false;
              if (this.Tree.data.length > 0) {
                this.Tree.defaultExpandFloor.push(
                  this.Tree.data[0][this.Tree.key]
                );
              }
              this.$emit("TreeClick", {
                id: this.Tree.data[0].id,
                data: this.Tree.data[0],
                key: this.Tree.data[0][this.Tree.key],
              });
            });
          }

          this.$nextTick(() => {
            this.$refs.tree.setCurrentKey(this.Tree.selectid);
          });
        });
      } else {
        this.Tree.defaultList = this.TreeData.list;
        this.Tree.data = this.TreeData.list;
        this.$nextTick(() => {
          this.$refs.tree.setCurrentKey(this.Tree.selectid);
        });
      }
    },

    /*根据code获取页面option*/
    GetOptionByCode(code) {
      if (!this.TreeData.operation) {
        return;
      }
      if (!this.TreeData.optionType) {
        this.Tree.form.option.column = this.TreeData.staticOption.column;
        this.Tree.form.editoption.column = this.TreeData.staticOption.column;
      } else {
        this.TreeData.ajax.Api.view(code).then((res) => {
          let Tree = [];
          res.data.extobj.tree.forEach((item) => {
            if (item.name == this.Tree.name) {
              Tree.push(item);
            }
          });
          for (let i = 0; i < Tree.length; i++) {
            for (let j = 0; j < Tree[i].column.length; j++) {
              if (Tree[i].column[j].dicDataType == "1") {
                // let data = GetDictCallCode(Tree[i].column[j].dicDataCallCode);
                // Tree[i].column[j].dicData = data;
              }
              if (Tree[i].column[j].dicDataType == "2") {
                // GetDict(Tree[i].column[j].dicDataCallCode).then((res) => {
                //   if (Tree[i].column[j].type == 'tree') {
                //
                //     Tree[i].column[j].dicData = GF.utils.buildListToTree(
                //       res,
                //       'parentcode',
                //       'code',
                //       0
                //     );
                //   } else {
                //     Tree[i].column[j].dicData = res;
                //   }
                // });
              }
              if (Tree[i].column[j].rules) {
                for (let x = 0; x < Tree[i].column[j].rules.length; x++) {
                  if (Tree[i].column[j].rules[x].hasOwnProperty("validator")) {
                    Tree[i].column[j].rules[x].validator = eval(
                      Tree[i].column[j].rules[x].validator
                    );
                  }
                }
              }
            }
          }
          this.Tree.form.option.column = Tree[0].column;
          this.Tree.form.editoption.column = Tree[1].column;
        });
      }
    },

    showDalog(type, title, width) {
      this.Tree.treetype = type;
      this.Tree.dialogTitle = title;
      this.Tree.dialogWidth = width;
      this.Tree.dialogVisible = true;
      this.Tree.btnload = false;
    },

    /*过滤节点*/
    filterNode(value, data) {
      if (!value) return true;
      return data[this.Tree.defaultProps.label].indexOf(value) !== -1;
    },

    /*树顶部添加*/
    Treeappend() {
      this.showDalog("treeappend", "新增", "50%");
      setTimeout(() => {
        this.$refs.form.allDisabled = false;
        this.$refs.form.resetForm();
        this.$refs.form.clearValidate();
      }, 30);
    },

    /*关闭时重置状态*/
    beforeClose() {
      this.$refs.form.allDisabled = false;
      this.$refs.form.resetForm();
    },
    /*添加节点*/
    append(node, data) {
      !data ? console.log(data) : console.log();
      this.showDalog("nodeappend", "新增", "50%");
      setTimeout(() => {
        this.$refs.form.clearValidate();
      }, 30);
    },

    /*编辑节点*/
    edit(node, data) {
      this.Tree.form.form = data;
      this.showDalog("nodeedit", "编辑", "50%");
      setTimeout(() => {
        this.$refs.form.clearValidate();
      }, 30);
    },
    /*删除节点*/
    remove(node, data) {
      let fun = this.TreeData.beforDelete;
      let boo = true;
      if (typeof fun == "function") {
        let temp = fun({ node, data });
        if (typeof temp == "boolean") {
          boo = temp;
        }
      }
      if (!boo) {
        return;
      }
      this.$confirm("请确认是否删除当前项, 再继续?", "提示", {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        type: "warning",
      }).then(() => {
        this.TreeData.ajax.Api.del(data.id).then((res) => {
          if (res.code == 0) {
            this.$message({
              message: "删除成功!",
              type: "success",
            });
            this.$emit("delSuccess");
            this.getList();
          }
        });
      });
    },

    /*勾选*/
    TreeCheck(checkedNodes, checkedKeys) {
      if (this.Tree.showCheckbox) {
        if (this.Tree.strictly) {
          this.Tree.checkdata = checkedKeys.checkedKeys;
          this.Tree.defaultcheckdata = checkedKeys.checkedKeys;
        } else {
          if (checkedKeys.halfCheckedKeys.length > 0) {
            this.Tree.checkdata = checkedKeys.checkedKeys.concat(
              checkedKeys.halfCheckedKeys
            );
            this.Tree.defaultcheckdata = checkedKeys.checkedKeys.concat(
              checkedKeys.halfCheckedKeys
            );
          } else {
            this.Tree.checkdata = checkedKeys.checkedKeys;
            this.Tree.defaultcheckdata = checkedKeys.checkedKeys;
          }
        }
      }
      let info = { node: this.Tree.nodes, id: this.Tree.checkdata };
      this.$Event.$emit("TreeCheck", info);
    },

    /*树点击*/
    TreeClick(data, node, e) {
      !node ? console.log(node, e) : console.log();
      this.Tree.selectid = data[this.Tree.key];
      let info = { data, id: this.Tree.selectid };
      this.$emit("TreeClick", info);
    },

    /*发送请求*/
    TreeSend() {
      this.Tree.btnload = true;
      this.$refs.form.validate((e) => {
        if (e) {
          if (this.Tree.treetype == "treeappend") {
            let data = this.Tree.form.form;
            let fun = this.TreeData.formFormatter;
            if (typeof fun == "function") {
              data = fun(this.Tree.form.form, "add") || this.Tree.form.form;
            }
            this.TreeData.ajax.Api.add(data).then((res) => {
              if (res.code === 0) {
                this.$message({
                  message: "新增成功!",
                  type: "success",
                });
                this.$emit("add-success");
                this.getList();
                this.Tree.dialogVisible = false;
              }
            });
          }
          if (this.Tree.treetype == "nodeappend") {
            let data = this.Tree.form.form;
            let fun = this.TreeData.formFormatter;
            if (typeof fun == "function") {
              data = fun(this.Tree.form.form, "add") || this.Tree.form.form;
            }
            this.TreeData.ajax.Api.add(data).then((res) => {
              if (res.code === 0) {
                this.$message({
                  message: "新增成功!",
                  type: "success",
                });
                this.$emit("add-success");
                this.getList();
                this.Tree.dialogVisible = false;
              }
            });
          }
          if (this.Tree.treetype == "nodeedit") {
            let data = this.Tree.form.form;
            let fun = this.TreeData.formFormatter;
            if (typeof fun == "function") {
              data = fun(this.Tree.form.form, "add") || this.Tree.form.form;
            }
            this.TreeData.ajax.Api.edit(data).then((res) => {
              if (res.code === 0) {
                this.$message({
                  message: "编辑成功!",
                  type: "success",
                });
                this.$emit("editSuccess");
                this.getList();
                this.Tree.dialogVisible = false;
              }
            });
          }
        } else {
          this.Tree.btnload = false;
        }
      });
    },
  },
};
</script>

<style scoped>
.Tree span {
  font-size: 12px;
}
.Tree /deep/ .el-tree-node__content {
  position: relative;
}
</style>
