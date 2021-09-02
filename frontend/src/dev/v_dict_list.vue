<template>
  <div id="avue-view">
    <div class="head">
      <div class="title">
        {{ msg }}
        <div
          class="color-div"
          :style="{ 'background-color': $store.state.common.colorName }"
        ></div>
      </div>
    </div>
    <el-container>
      <el-main>
        <transition name="slide-fade">
          <div class="crudbox" v-if="isShow">
            <avue-crud
              :option="option"
              :data="list.filter((ele) => ele.pid === cellid)"
              :table-loading="loading"
              @row-save="handleCreate"
              @row-del="handleDelete"
              @row-update="handleUpdate"
            >
              <template slot="callcode" slot-scope="scope">
                <el-tag
                  style="cursor: pointer"
                  @click="handleCellClick(scope.row)"
                  >{{ scope.row.callcode }}</el-tag
                >
              </template>
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
            </avue-crud>
          </div>
        </transition>
      </el-main>
    </el-container>
  </div>
</template>

<script>
export default {
  name: "dict",
  data() {
    return {
      loading: false,
      isShow: true,
      isParent: true,
      msg: "字典管理列表",
      cellid: 0,
      option: {},
      parOption: {
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
        index: true,
        headerAlign: "center",
        border: true,
        column: [
          {
            label: "名称",
            prop: "cname",
            type: "input",
            size: "small",
            align: "left",
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
            label: "字典类型",
            prop: "flag",
            type: "select",
            size: "small",
            align: "left",
            value: 1,
            dicData: [
              {
                label: "系统字典",
                value: 0,
              },
              {
                label: "业务字典",
                value: 1,
              },
              {
                label: "固定业务",
                value: 2,
              },
            ],
          },
          // 字典类型
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
            label: "描述",
            prop: "memo",
            type: "textarea",
            span: 24,
            size: "small",
          },
        ],
      },
      sonOption: {
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
            label: "数据体现",
            prop: "ccolor",
            type: "input",
            size: "small",
            align: "left",
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
      list: [],
    };
  },
  computed: {
    // list() {
    //
    //   return this.$store.getters.sysDicts || [];
    // }
  },
  created() {
    this.option = JSON.parse(JSON.stringify(this.parOption));
    this.getList();
  },
  methods: {
    getList() {
      window.GFAPI.sys.DictApi.load().then((res) => {
        this.list = res.data.sort(function (a, b) {
          return a.seqno - b.seqno;
        });
      });
    },

    handleCellClick(row) {
      this.cellid = row.id;
      this.msg = "字典管理列表 =>" + row.cname;
      this.option = JSON.parse(JSON.stringify(this.sonOption));
      this.isShow = false;
      setTimeout(() => {
        this.isParent = false;
        this.isShow = true;
      }, 0);
    },

    handleBack() {
      this.cellid = 0;
      this.msg = "字典管理列表";
      this.option = JSON.parse(JSON.stringify(this.parOption));
      this.isShow = false;
      setTimeout(() => {
        this.isParent = true;
        this.isShow = true;
      }, 0);
    },

    handleCreate(row, done, loading) {
      var that = this;
      if (this.cellid) {
        row.pid = this.cellid;
      }

      window.GFAPI.sys.DictApi.add(row)
        .then((res) => {
          loading();
          if (res.code == 0) {
            done();
            that.$message({
              message: "新增成功",
              type: "success",
              center: true,
              duration: 1000,
              onClose() {},
            });
            this.getList();
          }
        })
        .catch(() => {
          done();
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
          window.GFAPI.sys.DictApi.del(row.id)
            .then((res) => {
              if (res.code == 0) {
                that.$message({
                  message: "删除成功",
                  type: "success",
                  center: true,
                  duration: 1000,
                  onClose() {},
                });
                this.getList();
              }
            })
            .catch(() => {});
        })
        .catch(() => {});
    },

    handleUpdate(rows, index, done, loading) {
      var that = this;
      let row = JSON.parse(JSON.stringify(rows));
      for (const key in row) {
        const element = row[key] == null;
        element ? delete row[key] : null;
      }

      window.GFAPI.sys.DictApi.edit(row)
        .then((res) => {
          loading();
          if (res.code == 0) {
            done();
            that.$message({
              message: "编辑成功",
              type: "success",
              center: true,
              duration: 1000,
              onClose() {},
            });
            this.getList();
          }
        })
        .catch(() => {
          done();
        });
    },
  },
};
</script>
<style scoped>
.slide-fade-enter-active,
.slide-fade-leave-active {
  transition: all 0.4s ease;
}

.slide-fade-enter,
.slide-fade-leave-to {
  transform: translateY(-80px);
  opacity: 0;
}
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
</style>
