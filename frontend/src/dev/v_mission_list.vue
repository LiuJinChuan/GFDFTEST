<template>
  <div id="avue-view">
    <div class="head">
      <div class="title">
        任务管理
        <div
          class="color-div"
          :style="{ 'background-color': $store.state.common.colorName }"
        ></div>
      </div>
    </div>
    <el-container>
      <div class="crudbox_">
        <el-main>
          <div class="crudbox">
            <!-- <avue-form class="avue-form-search"
                       ref="seacherForm"
                       :option="header.option"
                       v-model="header.form"
                       @reset-change="resetfilterCRUDData"
            @submit="filterCRUDData"></avue-form>-->
            <avue-crud
              :option="option"
              ref="crudForm"
              v-model="form"
              :data="list"
              @refresh-change="resetfilterCRUDData"
              @row-del="handleDelete"
            >
              <template slot="Disabled" slot-scope="{ row }">
                <el-tag type="info" v-if="!row.Disabled">停用</el-tag>
                <el-tag type="success" v-if="row.Disabled">启用</el-tag>
              </template>

              <template
                slot-scope="{ row, type, size }"
                style="width: '400px'"
                slot="menu"
              >
                <el-button
                  icon="el-icon-video-play"
                  :size="size"
                  v-if="!row.Disabled"
                  @click="oppenDetail(row, true)"
                  :type="type"
                  >启动</el-button
                >
                <el-button
                  icon="iconfont icon-tingzhi"
                  :size="size"
                  v-if="row.Disabled"
                  @click="oppenDetail(row, false)"
                  :type="type"
                  >停止</el-button
                >
                <el-button
                  icon="el-icon-delete"
                  @click="handleDelete(row)"
                  :size="size"
                  :type="type"
                  >清除</el-button
                >
              </template>
            </avue-crud>
          </div>
        </el-main>
      </div>
    </el-container>
    <el-drawer
      title="我是标题"
      :visible.sync="detailData.drawer"
      :append-to-body="true"
      :size="'50%'"
      :withHeader="false"
    >
      <div class="crudbox withPage" style="height: 100%">
        <avue-form
          class="avue-form-search"
          ref="seacherForm_drawer"
          :option="header_drawer.option"
          v-model="header_drawer.form"
          @reset-change="resetFilters"
          @submit="filters"
        ></avue-form>
        <avue-crud
          :option="option_drawer"
          :data="list_drawer"
          @refresh-change="resetFilters"
          @row-del="handleDelete"
        >
          <template slot-scope="{ row, type, size }" slot="menu">
            <el-button
              icon="el-icon-delete"
              @click="handleDelete(row)"
              :size="size"
              :type="type"
              >清除</el-button
            >
          </template>
        </avue-crud>
        <el-pagination
          class="pagination"
          layout="total,sizes,prev, pager, next,jumper"
          @size-change="setPageFilter"
          @current-change="setPageFilter"
          :current-page="pages.page"
          :page-sizes="[10, 20, 30, 40]"
          :page-size="pages.limit"
          :total="pages.total"
        ></el-pagination>
      </div>
    </el-drawer>
  </div>
</template>

<script>
let againGroup = function (arr, num) {
  var result = [];
  for (var i = 0, len = arr.length; i < len; i += num) {
    result.push(arr.slice(i, i + num));
  }
  return result;
};

export default {
  name: "mission",
  data() {
    return {
      loading: true,
      tabLoading: true,
      form: {},
      option: {
        refreshBtn: true,
        editBtn: false,
        delBtn: false,
        addBtn: false,
        menuAlign: "center",
        menuWidth: "200",
        labelWidth: "120",
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
            prop: "Name",
            type: "input",
            size: "small",
            display: true,
            align: "center",
          },
          {
            label: "下次运行时间",
            prop: "NextRunTime",
            type: "input",
            size: "small",
            display: true,
            align: "center",
          },
          {
            label: "状态",
            prop: "Disabled",
            type: "input",
            slot: true,
            size: "small",
            display: true,
            align: "center",
          },
        ],
      },
      option_drawer: {
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
            label: "键名",
            prop: "call",
            type: "input",
            size: "small",
            align: "center",
          },
          {
            label: "值",
            prop: "resdata",
            overHidden: true,
            type: "number",
            size: "small",
          },
        ],
      },
      list: [],
      initlist: [],
      list_drawer: [],
      initlist_drawer: [],
      header: {
        option: {
          column: [
            {
              type: "select",
              label: "所属模块",
              dicData: [
                {
                  label: "系统模块",
                  value: "0",
                },
              ],
              span: 24,
              display: true,
              size: "small",
              prop: "type",
            },
          ],
          gutter: 20,
          labelPosition: "right",
          menuBtn: true,
          submitBtn: true,
          menuPosition: "right",
          submitSize: "medium",
          submitText: "提交",
          emptyBtn: true,
          emptySize: "medium",
          emptyText: "重置",
        },
        form: {},
      },
      header_drawer: {
        option: {
          column: [
            {
              type: "input",
              label: "说明",
              span: 24,
              display: true,
              size: "small",
              prop: "mark",
            },
          ],
          gutter: 20,
          labelPosition: "right",
          menuBtn: true,
          submitBtn: true,
          menuPosition: "right",
          submitSize: "medium",
          submitText: "提交",
          emptyBtn: true,
          emptySize: "medium",
          emptyText: "重置",
        },
        form: {},
      },
      detailData: {
        drawer: false,
      },
      pages: {
        limit: 10,
        page: 1,
        total: 0,
      },
    };
  },
  watch: {},
  created() {
    this.getOthers();
    this.getList();
    // getFormExt("mission_list").then(res => {
    //   let temp = JSON.parse(res.Message).mission_list;
    //   let obj = this.option.column.find(e => e.prop == "ctype");
    //   this.$set(obj, "dicData", temp);
    // });
  },
  computed: {},
  methods: {
    getOthers() {
      let dicts = this.$store.state.system.dicts;
      if (dicts == null) {
        this.$store.dispatch("setSysDicts").then(() => {
          // this.setDicts();
        });
      } else {
        // this.setDicts();
      }
    },
    //字典集中处理
    setDicts() {
      let dict = this.$store.state.system.dicts;

      let dictTree = window.GF.utils.buildListToTree(dict, "pid", "id", null);
      this.getMoudel(dictTree, "module_type");
      this.tabLoading = false;
      this.loading = false;
    },
    getMoudel(arr, callcode) {
      let temp = arr.find((e) => e.callcode == callcode).children;
      let obj = this.option.column.find((e) => e.prop == "moudle");
      this.$set(obj, "dicData", JSON.parse(JSON.stringify(temp)));
      this.lists = temp;
    },
    getList() {
      window.GFAPI.sys.CommonApi.Schedule.load().then((rs) => {
        this.list = rs.data.forEach((e) => {
          e.NextRunTime = `${e.NextRun.substr(0, 10)}  ${e.NextRun.substr(
            11,
            8
          )}`;
        });
        this.list = rs.data;
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
          window.GFAPI.sys.QrtzTaskApi.del(row.id)
            .then((res) => {
              if (res.code == 0) {
                that.$message({
                  message: "清除成功",
                  type: "success",
                  center: true,
                  duration: 1000,
                });
                this.getList();
              }
            })
            .catch();
        })
        .catch();
    },
    filterCRUDData(row, done) {
      if (!this.header.form.type) {
        let temp = JSON.parse(JSON.stringify(this.initlist));
        temp.forEach((e) => delete e.children);
        this.list = temp;
      } else {
        let temp = this.initlist.filter((e) => e.type == this.header.form.type);
        temp.forEach((e) => delete e.children);
        this.list = temp;
      }
      if (done) {
        done();
      }
    },
    resetfilterCRUDData() {
      let object = this.header.form;
      for (const key in object) {
        object[key] = "";
      }
      this.$nextTick(() => {
        this.$refs.seacherForm.clearValidate();
      });
      this.filterCRUDData();
    },
    oppenDetail(e, state) {
      window.GFAPI.sys.QrtzTaskApi.missionChange(state, e.Name).then((res) => {
        if (res.code == 0) {
          this.$message({
            message: state ? "停止成功" : "启用成功",
            type: "success",
            center: true,
            duration: 1000,
          });
          this.getList();
        }
      });
    },
    setPageFilter() {
      this.filters();
      let data = this.list_drawer;
      let temp = againGroup(data, this.pages.limit);
      this.list_drawer = temp[this.pages.page - 1];
    },
    filters(row, done) {
      if (!this.header_drawer.form.mark) {
        this.list_drawer = this.initlist_drawer;
      } else {
        let temp = this.initlist.filter(
          (e) => e.call == this.header_drawer.form.mark
        );
        this.list_drawer = temp;
      }
      if (done) {
        done();
      }
    },
    resetFilters() {
      let object = this.header_drawer.form;
      for (const key in object) {
        object[key] = "";
      }
      this.$nextTick(() => {
        this.$refs.seacherForm_drawer.clearValidate();
      });
      this.filters();
    },
    handleEdit(rwo, index) {
      this.$refs.crudForm.rowEdit(rwo, index);
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
.withPage {
  position: relative;
}
.el-pagination {
  text-align: right;
}
.pagination {
  width: 100%;
  position: absolute;
  bottom: 0;
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
// /deep/.avue-crud__menu {
//   display: none;
// }
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
/deep/.el-drawer__body {
  .el-row {
    display: flex;
    justify-content: space-between;
    > div.el-col {
      width: 50%;
    }
  }
}
/deep/.el-tooltip__popper {
  width: 50%;
}
/deep/.iconfont.icon-tingzhi {
  font-size: 12px !important;
}
</style>
