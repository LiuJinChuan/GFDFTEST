<template>
  <div class="Tree">
    <el-input
      style="margin-bottom: 15px"
      size="small"
      placeholder="输入关键字进行过滤"
      v-model="Tree.filterText"
    >
      <el-button v-if="TreeData.addBtn" @click="Treeappend" slot="append" icon="el-icon-plus"></el-button>
    </el-input>
    <el-tree
      ref="tree"
      :data="TreeData.list||Tree.data"
      :check-strictly="Tree.strictly"
      :node-key="Tree.key"
      :default-expanded-keys="['']"
      :show-checkbox="Tree.showCheckbox"
      :filter-node-method="filterNode"
      :expand-on-click-node="false"
      :props="Tree.defaultProps"
      @node-click="TreeClick"
      @check="TreeCheck"
    >
      <span class="custom-tree-node" slot-scope="{ node, data }">
        <span>{{ node.label }}</span>
        <span style="position: absolute;top:4px;right:3px;">
          <!--<i class="el-icon-circle-plus-outline" style="margin-right:8px;color:#409eff"-->
          <!--@click.stop="() => append(node,data)"></i>-->
          <slot name="menuRight" :node="node" :data="data"></slot>
          <i
            v-if="TreeData.editBtn"
            class="el-icon-edit-outline"
            style="margin-right:8px;color:#409eff"
            @click.stop="() => edit(node,data)"
          ></i>
          <i
            v-if="TreeData.delBtn"
            class="el-icon-delete-solid"
            style="color:#409eff"
            @click.stop="() => remove(node, data)"
          ></i>
        </span>
      </span>
    </el-tree>
    <el-dialog
      append-to-body="true"
      :close-on-click-modal="false"
      :title="Tree.dialogTitle"
      :visible.sync="Tree.dialogVisible"
      @close="beforeClose"
      :width="Tree.dialogWidth"
    >
      <div v-if="Tree.treetype=='treeappend'">
        <avue-form ref="form" v-model="Tree.form.form" :option="Tree.form.option"></avue-form>
      </div>
      <!--<div v-if="Tree.treetype=='nodeappend'">-->
      <!--&lt;!&ndash;<avue-form ref="form" v-model="Tree.form.form" :option="Tree.form.option"></avue-form>&ndash;&gt;-->
      <!--</div>-->
      <div v-if="Tree.treetype=='nodeedit'">
        <avue-form ref="form" v-model="Tree.form.form" :option="Tree.form.editoption"></avue-form>
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
        >保 存</el-button>
        <el-button size="small" icon="el-icon-circle-close" @click="Tree.dialogVisible = false">取 消</el-button>
      </span>
    </el-dialog>
  </div>
</template>

<script>

export default {
  name: '',
  props: ['TreeData', 'ajax'],
  data() {
    return {
      Tree: {
        nodes: '',
        name: '',
        strictly: false, //是否父子链接，勾选子节点自动勾选父节点，false为勾选父节点，true为不勾选父节点
        treetype: '', //弹窗类型，决定确定按钮的发送请求
        key: 'id', //勾选树取值
        dialogTitle: '新增', //弹窗的名称
        dialogWidth: '50%', //弹窗宽度
        dialogVisible: false, //弹窗的显示
        showCheckbox: true, //树是否可以勾选
        filterText: '', //过滤树绑定的值
        data: [], //树数据
        defaultProps: {}, //绑定显示和子节点的字符串
        btnload: false, //确定按钮反重提交
        checkdata: [], //勾选的数据
        defaultcheckdata: [], //默认勾选的数据
        selectid: '',
        defaultList: [],
        formdata: {},
        title: '',
        form: {
          option: {
            labelWidth: 120,
            menuBtn: false,
            enter: false,
            size: 'small',
            column: [
              {
                label: '选项',
                prop: 'text',
                dicdatatype: '',
                type: 'select',
                required: true,
                rules: [
                  {
                    required: true,
                    message: '请选择一个选项',
                    trigger: 'change'
                  }
                ],
                dicData: [
                  { label: '一级', value: '1' },
                  { label: '二级', value: '2' }
                ]
              },
              {
                label: '账号',
                prop: 'text1',
                type: 'input',
                required: true,
                rules: [
                  {
                    validator:
                      "(rule, value, callback) => {if (value.length < 6) {callback(new Error('账号不能少于6位'));} else {callback();}}",
                    trigger: 'change'
                  }
                ]
                // dicData: getdicbytype('text1')
              }
            ]
          },
          editoption: {
            labelWidth: 150,
            menuBtn: false,
            enter: false,
            size: 'small',
            column: [
              {
                label: '选项',
                prop: 'text',
                dicdatatype: '',
                type: 'select',
                required: true,
                rules: [
                  {
                    required: true,
                    message: '请选择一个选项',
                    trigger: 'change'
                  }
                ],
                dicData: [
                  { label: '一级', value: '1' },
                  { label: '二级', value: '2' }
                ]
              },
              {
                label: '账号',
                prop: 'text1',
                type: 'input',
                required: true,
                rules: [
                  {
                    validator:
                      "(rule, value, callback) => {if (value.length < 6) {callback(new Error('账号不能少于6位'));} else {callback();}}",
                    trigger: 'change'
                  }
                ]
                // dicData: getdicbytype('text1')
              }
            ]
          },
          form: {}
        }
      },
      ones: true
    };
  },
  watch: {
    'Tree.filterText'(val) {
      this.$refs.tree.setCheckedKeys([]);
      this.$refs.tree.filter(val);
      this.Tree.checkdata = [];
    },
    'TreeData.node'(val) {
      this.Tree.nodes = val;
    }
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
    this.Tree.nodes = this.TreeData.node;
    this.getList();
    this.GetOptionByCode(this.TreeData.ajax.data);
  },

  methods: {
    /*获取列表*/
    getList() {
      if (!this.TreeData.list) {
        this.TreeData.ajax.Api.load().then((res) => {
           // todo 角色权限树
          // let infos = GF.cache.get('userInfo').roles.split(',');
          // let roles = GF.cache.get('roles');
          // let Role = roles.map((e) => {
          //   if (infos.includes(e.id)) {
          //     return e.datascope;
          //   }
          // });
          // let roleStr = Role.join(',');


            let tmp = {};
            tmp[this.TreeData.defaultProps.label] = '全部';
            tmp[this.TreeData.key] = '';
            tmp[this.TreeData.defaultProps.children] = res.data;
            this.Tree.defaultList = [tmp];
            this.Tree.data = [tmp];
            if (this.ones) {
              this.$nextTick(() => {
                this.$refs.tree.setCurrentKey([tmp][0].id);
                this.ones = false;
                this.$emit('TreeClick', { id: [tmp][0].id, data: tmp });
              });
            }

          // if ('0'.includes(roleStr) || GF.cache.get('userInfo').dept == 0) {
          //
          // }
          // else {
          //   let arr = res.data.filter(
          //     (e) => e.id == GF.cache.get('userInfo').dept
          //   );
          //   this.Tree.defaultList = arr;
          //   this.Tree.data = arr;
          //   if (this.ones) {
          //     this.$nextTick(() => {
          //       this.$refs.tree.setCurrentKey(arr[0].id);
          //       this.ones = false;
          //       this.$emit('TreeClick', { id: arr[0].id, data: arr });
          //     });
          //   }
          // }
          this.$nextTick(() => {
            this.$emit('treeGetListEd', res);
          });
        });
      } else {
        this.Tree.defaultList = this.TreeData.list;
        this.Tree.data = this.TreeData.list;
        this.$nextTick(() => {
          this.$emit('treeGetListEd', { data: this.TreeData.list });
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
            Tree.push(item);
          });


          for (let i = 0; i < Tree.length; i++) {
            for (let x = 0; x < Tree[i].length; x++) {
              if (Tree[i][x].dicDataType == '1') {

                // GetDictCallCode(Tree[i][x].dicDataCallCode).then((res) => {
                //   Tree[i][x].props = { label: 'cname', value: 'cvalue' };
                //   Tree[i][x].dicData = res;
                // });
              }
              if (Tree[i][x].dicDataType == '2') {
                // GetDict(Tree[i][x].dicDataCallCode).then((res) => {
                //   if (Tree[i][x].type == 'tree') {
                //     Tree[i][x].dicData = buildListToTree(
                //       res,
                //       'parentcode',
                //       'code',
                //       0
                //     );
                //   } else {
                //     Tree[i][x].dicData = res;
                //   }
                // });
              }
              if (Tree[i][x].rules) {
                for (let j = 0; j < Tree[i][x].rules.length; j++) {
                  if (Tree[i][x].rules[j].hasOwnProperty('validator')) {
                    Tree[i][x].rules[j].validator = eval(
                      Tree[i][x].rules[j].validator
                    );
                  }
                }
              }
            }
          }
          this.Tree.form.option.column = Tree[0];
          this.Tree.form.editoption.column = Tree[1];
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
      this.showDalog('treeappend', '新增', '50%');
      this.$refs.form.resetForm();
      setTimeout(() => {
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
      this.showDalog('nodeappend', '新增', '50%');
      setTimeout(() => {
        this.$refs.form.allDisabled = false;
        this.$refs.form.resetForm();
        this.$refs.form.clearValidate();
      }, 30);
    },

    /*编辑节点*/
    edit(node, data) {
      this.Tree.form.form = data;
      this.showDalog('nodeedit', '编辑', '50%');
      setTimeout(() => {
        this.$refs.form.clearValidate();
      }, 30);
    },

    /*删除节点*/
    remove(node, data) {
      this.$confirm('请确认是否删除当前项, 再继续?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(() => {
        this.TreeData.ajax.Api.del(data.id).then((res) => {
          if (res.code == 0) {
            this.$message({
              message: '删除成功!',
              type: 'success'
            });
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
      this.$Event.$emit('TreeCheck', info);
    },

    /*树点击*/
    TreeClick(data, node, e) {
      !node ? console.log(node, e) : console.log();
      this.Tree.selectid = data[this.Tree.key];
      let info = { data, id: this.Tree.selectid };
      this.$emit('TreeClick', info);
    },

    /*发送请求*/
    TreeSend() {
      this.Tree.btnload = true;
      this.$refs.form.validate((e) => {
        if (e) {
          if (this.Tree.treetype == 'treeappend') {
            this.TreeData.ajax.Api.add(this.Tree.form.form).then((res) => {
              if (res.code === 0) {
                this.$message({
                  message: '新增成功!',
                  type: 'success'
                });
                this.getList();
                this.Tree.dialogVisible = false;
              }
            });
          }
          if (this.Tree.treetype == 'nodeappend') {
            this.TreeData.ajax.Api.add(this.Tree.form.form).then((res) => {
              if (res.code === 0) {
                this.$message({
                  message: '新增成功!',
                  type: 'success'
                });
                this.getList();
                this.Tree.dialogVisible = false;
              }
            });
          }
          if (this.Tree.treetype == 'nodeedit') {
            this.TreeData.ajax.Api.edit(this.Tree.form.form).then((res) => {
              if (res.code === 0) {
                this.$message({
                  message: '编辑成功!',
                  type: 'success'
                });
                this.getList();
                this.Tree.dialogVisible = false;
              }
            });
          }
        } else {
          this.Tree.btnload = false;
        }
      });
    }
  }
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
