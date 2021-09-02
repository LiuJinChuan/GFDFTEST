<template>
  <div id="avue-view">
    <div class="head">
      <div class="title">
        角色管理列表
        <div
          class="color-div"
          :style="{ 'background-color': $store.state.common.colorName }"
        ></div>
      </div>
    </div>
    <el-container>
      <el-main>
        <div class="crudbox">
          <el-row :gutter="15" class="el-rowBox">
            <!--角色管理-->
            <el-col :xs="8" :sm="8" :md="8" :lg="8" :xl="8">
              <div class="roles-left">
                <avue-crud
                  :option="option"
                  :data="list"
                  ref="roleCrud"
                  @row-save="handleCreate"
                  @row-del="handleDelete"
                  @row-update="handleUpdate"
                  @row-click="handleRowClick"
                ></avue-crud>
              </div>
            </el-col>
            <el-col
              v-loading="tabsLoading"
              :xs="16"
              :sm="16"
              :md="16"
              :lg="16"
              :xl="16"
              class="tabBox"
            >
              <el-tabs v-model="tabActiveName" type="card">
                <el-tab-pane label="菜单权限" name="first">
                  <el-tree
                    ref="menus"
                    :default-checked-keys="treeOption.menus.default"
                    :props="treeDefaultProps"
                    :data="treeOption.menus.list"
                    show-checkbox
                    node-key="id"
                  ></el-tree>
                </el-tab-pane>
                <el-tab-pane label="页面权限" name="second">
                  <el-tree
                    ref="pages"
                    :default-checked-keys="treeOption.pages.default"
                    :props="treeDefaultProps"
                    :data="treeOption.pages.list"
                    show-checkbox
                    node-key="id"
                  >
                    <span
                      class="custom-tree-node"
                      @click="pageNodeClick(data)"
                      slot-scope="{ data }"
                    >
                      <span>{{ data.cname }}</span>
                    </span>
                  </el-tree>
                </el-tab-pane>
                <el-tab-pane label="接口权限" name="fourth">
                  <el-tree
                    ref="webapies"
                    :default-checked-keys="treeOption.webapies.default"
                    :props="treeDefaultProps"
                    :data="treeOption.webapies.list"
                    default-expand-all
                    show-checkbox
                    node-key="id"
                  ></el-tree>
                </el-tab-pane>
              </el-tabs>
              <div class="saveBtn">
                <el-button
                  size="small"
                  type="primary"
                  icon="el-icon-edit"
                  @click="handleSave"
                  >保存</el-button
                >
              </div>
            </el-col>
          </el-row>
        </div>
      </el-main>
    </el-container>
  </div>
</template>

<script>
import Vue from "vue";
export default {
  name: "role",
  data() {
    return {
      perid: 0,
      list: [],
      tabActiveName: "first",
      tabsLoading: false,
      treeOption: {
        menus: {
          list: [],
          default: [],
        },
        pages: {
          list: [],
          all: [],
          default: [],
        },
        webapies: {
          list: [],
          default: [],
        },
        row: {},
      },
      treeDefaultProps: {
        children: "children",
        label: "cname",
      },
      option: {
        menuType: "text",
        refreshBtn: false,
        highlightCurrentRow: true,
        menuAlign: "center",
        menu: true,
        border: true,
        maxHeight: "655",
        index: true,
        headerAlign: "center",
        dialogType: "dialog",
        dialogEscape: false,
        dialogModal: true,
        dialogClickModal: false,
        dialogCloseBtn: true,
        dialogFullscreen: false,
        menuWidth: "200",
        column: [
          {
            label: "角色名称",
            prop: "cname",
            type: "input",
            align: "center",
            addDisplay: true,
            editDisplay: true,
            rules: [
              {
                required: true,
                message: "请输入角色名称",
                trigger: "blur",
              },
            ],
          },
          {
            label: "角色级别",
            prop: "level",
            type: "number",
            align: "center",
            addDisplay: true,
            hide: true,
            editDisplay: true,
            change: () => {
              this.$nextTick(() => {
                if (
                  !!this.$refs.roleCrud &&
                  typeof this.$refs.roleCrud.clearValidate === "function"
                ) {
                  this.$refs.roleCrud.clearValidate();
                }
              });
            },
            rules: [
              {
                required: true,
                message: "请输入角色级别",
                trigger: "blur",
              },
            ],
          },
          {
            label: "数据范围",
            prop: "datascope",
            type: "select",
            hide: true,
            align: "center",
            addDisplay: true,
            editDisplay: true,
            dicData: [],
            rules: [
              {
                required: true,
                message: "请选择数据范围",
                trigger: "blur",
              },
            ],
          },
          {
            label: "描述信息",
            prop: "memo",
            span: 24,
            hide: true,
            type: "textarea",
            align: "left",
            addDisplay: true,
            editDisplay: true,
          },
        ],
      },
    };
  },
  created() {
    this.getList();
    this.getOthers();
    //role

    //permiss
    //menu
    //api
    //page
  },
  computed: {},
  methods: {
    getList() {
      window.GFAPI.sys.RoleApi.load().then((res) => {
        res.data.forEach((e) => {
          e.datascope == 0 ? (e.datascope = "") : (e.datascope += "");
        });
        this.list = res.data;
      });
    },
    getOthers() {
      this.permissionTree();
      // this.webapiTree();
      this.datascope_dict();
      window.GFAPI.sys.MenuApi.load().then((res) => {
        this.treeOption.menus.list = window.GF.utils.buildListToTree(
          res.data,
          "pid",
          "id",
          0
        );
      });
    },
    //数据范围字典
    datascope_dict() {
      var datascopes = window.GF.dicts.sys_role_limit;
      var datascope = this.option.column.find((ele) => ele.prop == "datascope");
      Vue.set(datascope, "dicData", datascopes);
    },

    //页面权限,按钮权限树处理
    permissionTree() {
      window.GFAPI.sys.PageApi.load().then((page) => {
        page.data.forEach((e) => (e.pbtype = "page"));
        let treeData = JSON.parse(JSON.stringify(window.GF.dicts.module_type));

        window.GFAPI.sys.ButtonApi.load().then((btn) => {
          btn.data.forEach((e) => (e.pbtype = "btn"));
          let arr = page.data
            .concat(btn.data)
            .sort((a, b) => a.seqno - b.seqno);
          let btns = window.GF.utils.buildListToTree(arr, "page", "id"); //.map(e => { if (e.children.length == 0) { delete e.children } return e })
          treeData.forEach((ele) => {
            ele.cname = ele.label;
            ele.children = btns.reduce((a, b) => {
              if (b.module == ele.value) {
                a.push(b);
              }
              return a;
            }, []);
            ele.children.sort((a, b) => a.seqno - b.seqno);
          });
          this.treeOption.pages.list = treeData;
          this.treeOption.pages.all = treeData.reduce((x, y) => {
            x = x.concat(y.children);
            return x;
          }, []);
          // this.treeOption.pages.arr = arr
        });
      });
    },

    //接口权限
    webapiTree() {
      window.GFAPI.sys.WebapiApi.load().then((res) => {
        let data = window.GF.utils.buildListToTree(res.data, "pid", "id", 0);
        let treeData = JSON.parse(JSON.stringify(window.GF.dicts.module_type));
        treeData.forEach((ele) => {
          ele.children = data.reduce((a, b) => {
            if (b.module == ele.cvalue) {
              a.push(b);
            }
            return a;
          }, []);
        });
        this.treeOption.webapies.list = treeData;
      });
    },
    handleCreate(row, done, loading) {
      var that = this;
      row.ext = JSON.stringify({
        leafs: "",
        halfs: "",
      });

      window.GFAPI.sys.RoleApi.add(row)
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
                that.$store.dispatch("setSysRoles");
              },
            });
          }
        })
        .catch(() => {
          done();
        });
    },

    handleSave() {
      if (!this.treeOption.row.id) {
        this.$message({
          message: "请选择角色",
          type: "error",
          center: true,
          duration: 1000,
        });
        return;
      }
      var that = this;
      let temp = that.$refs.pages.getCheckedNodes();
      let menuArr = this.$refs.menus.getCheckedKeys();
      let halfMenuArr = this.$refs.menus.getHalfCheckedKeys();
      menuArr = menuArr.concat(halfMenuArr);
      let btnArr = temp.filter((e) => e.pbtype == "btn").map((e) => e.id);
      let pageArr = temp.filter((e) => e.pbtype == "page").map((e) => e.id);
      window.GFAPI.sys.RolePermisApi.edit({
        id: this.treeOption.row.id,
        webapi: "",
        menu: menuArr.join(","),
        page: pageArr.join(","),
        button: btnArr.join(","),
      }).then((res) => {
        if (res.code == 0) {
          that.$message({
            message: "保存成功",
            type: "success",
            center: true,
            duration: 1000,
          });
          window.GFAPI.sys.PermisApi.edit({
            id: this.perid,
            ext: JSON.stringify({ halfmenus: halfMenuArr.join(",") }),
          }).then(() => {});
          this.$store.dispatch("setSysRoles");
          // location.reload();
        }
      });
    },

    handleDelete(row) {
      var that = this;
      this.$confirm("请确认是否删除当前项, 再继续?", "提示", {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        type: "warning",
      })
        .then(() => {
          window.GFAPI.sys.RoleApi.del(row.id)
            .then((res) => {
              if (res.code == 0) {
                that.$message({
                  message: "删除成功",
                  type: "success",
                  center: true,
                  duration: 1000,
                  onClose() {
                    that.$store.dispatch("setSysRoles");
                    // that.$refs.permissions.setCheckedKeys([]);
                    that.$refs.menus.setCheckedKeys([]);
                  },
                });
              }
            })
            .catch(() => {});
        })
        .catch(() => {});
    },

    handleRowClick(row) {
      this.treeOption.row = JSON.parse(JSON.stringify(row));
      this.$refs.menus.setCheckedKeys([]);
      this.$refs.pages.setCheckedKeys([]);
      this.$refs.webapies.setCheckedKeys([]);
      this.tabsLoading = true;
      window.GFAPI.sys.PermisApi.search({
        order: [{ id: "desc" }],
        other: [{ role: row.id }],
      }).then((res) => {
        res.data;
        if (res.data && res.data[0]) {
          let data = res.data[0];
          this.perid = data.id;
          let menu = data.menu == "" ? [] : data.menu.split(",").map(Number);
          let btn = data.button == "" ? [] : data.button.split(",").map(Number);
          let webapi =
            data.webapi == "" ? [] : data.webapi.split(",").map(Number);
          if (data.extobj.halfmenus) {
            let halfMenuArr = data.extobj.halfmenus.split(",");
            halfMenuArr.forEach((el) => {
              menu = menu.filter((o) => o != el);
            });
          }
          this.treeOption.menus.default = menu;
          this.treeOption.pages.default = btn;
          this.treeOption.webapies.default = webapi;
        }
        this.tabsLoading = false;
      });
    },

    handleUpdate(row, index, done, loading) {
      var that = this;

      window.GFAPI.sys.RoleApi.edit(row)
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
                that.$store.dispatch("setSysRoles");
                location.reload();
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
.roles-left,
.roles-right {
  border-radius: 4px;
  border: 1px solid #e6ebf5;
  padding: 15px;
}
.custom-tree-node {
  flex: 1;
  display: flex;
  align-items: center;
  justify-content: space-between;
  font-size: 14px;
  padding-right: 8px;
}
.tabBox {
  position: relative;
  height: 100%;
  border-radius: 4px;
  border: 1px solid #e6ebf5;
  overflow: scroll;
  padding: 5px;
  .saveBtn {
    position: absolute;
    width: 100%;
    text-align: right;
    top: 0;
    left: 0;
    padding-right: 10px;
    box-sizing: border-box;
  }
}
.el-rowBox {
  height: 77vh;
  overflow: hidden;
  box-sizing: border-box;
  /deep/.el-tabs {
    height: 100%;
    /deep/.el-tabs__content {
      height: 90%;
      overflow-y: auto;
      /deep/.el-tab-pane > div {
        box-sizing: border-box;
      }
    }
  }
}
/deep/.el-drawer__body {
  overflow-y: auto;
}
</style>
