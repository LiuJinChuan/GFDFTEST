<template>
  <div id="avue-view">
    <div class="head">
      <div class="title">
        {{ data.cname }}列表
        <div
          class="color-div"
          :style="{ 'background-color': $store.state.common.colorName }"
        ></div>
      </div>
    </div>
    <div class="script-memo">
      <span>调用标识：{{ data.callcode }}</span>
      <span>数据标签：{{ data.tagsMsg }}</span>
    </div>
    <el-container>
      <el-main>
        <div class="crudbox">
          <avue-crud
            :option="option"
            :data="list"
            :table-loading="loading"
            @row-del="handleDelete"
          >
            <template slot-scope="{}" slot="menuLeft">
              <el-button
                type="primary"
                icon="el-icon-plus"
                size="small"
                @click.stop="handleAdd()"
                >新 增</el-button
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
        </div>
      </el-main>
    </el-container>
  </div>
</template>

<script>
export default {
  name: "codeview",
  data() {
    return {
      list: [],
      data: {},
      segid: null,
      loading: true,
      option: {
        refreshBtn: false,
        addBtn: false,
        editBtn: false,
        delBtn: false,
        menuType: "text",
        menuAlign: "center",
        menu: true,
        border: true,
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
            label: "ID",
            prop: "id",
            type: "input",
            size: "small",
            align: "center",
          },
          {
            label: "名称",
            prop: "cname",
            align: "center",
          },
          {
            label: "调用标识",
            prop: "callcode",
            align: "center",
          },
          {
            label: "版本号",
            prop: "ver",
            align: "center",
          },
        ],
      },
    };
  },
  created() {
    this.segid = this.$route.query.segid;
    this.getList();
    this.getInfo();
  },
  watch: {
    "$route.query.segid": {
      handler(val) {
        if (val) {
          this.segid = this.$route.query.segid;
          this.getList();
          this.getInfo();
        }
      },
    },
  },
  methods: {
    handleAdd() {
      this.$router.push({
        path: "/dev/codemirror",
        query: {
          segid: this.segid,
          title: this.data.cname,
          act: "add",
        },
      });
    },

    handleEdit(row) {
      this.$router.push({
        path: "/dev/codemirror",
        query: {
          id: row.id,
          title: row.cname,
          act: "edit",
        },
      });
    },

    handleDel(row) {
      var that = this;
      this.$confirm("请确认是否删除当前项, 再继续?", "提示", {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        type: "warning",
      })
        .then(() => {
          window.GFAPI.dev.CodeSegDtlApi.del(row.id)
            .then((res) => {
              if (res.code == 0) {
                that.$message({
                  message: "删除成功",
                  type: "success",
                  center: true,
                  duration: 1000,
                  onClose() {
                    that.getList();
                  },
                });
              }
            })
            .catch(() => {});
        })
        .catch(() => {});
    },

    handleDicts() {
      let types = window.GF.dicts.script_type;
      var tags = this.data.tags.split(",");
      var arr = [];
      tags.forEach(() => {
        types.forEach((ele) => {
          if (ele.value == tags) {
            arr.push(ele.label);
          }
        });
      });
      this.data.tagsMsg = arr.join(" | ");
    },

    getInfo() {
      window.GFAPI.dev.CodeSegApi.view(this.segid)
        .then((res) => {
          this.data = res.data;
          this.handleDicts();
        })
        .catch(() => {});
    },

    getList() {
      this.loading = true;

      window.GFAPI.dev.CodeSegDtlApi.view(this.segid)
        .then((res) => {
          this.list = res.data || [];
          this.loading = false;
        })
        .catch(() => {
          this.list = [];
          this.loading = false;
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
          window.GFAPI.sys.JobApi.del(row)
            .then((res) => {
              if (res.code == 0) {
                that.$message({
                  message: "删除成功",
                  type: "success",
                  center: true,
                  duration: 1000,
                  onClose() {
                    that.$store.dispatch("setSysJobs");
                  },
                });
              }
            })
            .catch(() => {});
        })
        .catch(() => {});
    },
  },
};
</script>
<style scoped lang='scss'>
.script-memo {
  font-size: 14px;
  color: #808695;
  margin: 10px 0px;
  span {
    padding-left: 20px;
  }
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
  background-color: #00aeff;
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
