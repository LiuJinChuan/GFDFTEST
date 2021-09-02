<template>
  <div id="avue-view">
    <div class="head">
      <div class="title">
        表单列表
        <div
          class="color-div"
          :style="{ 'background-color': $store.state.common.colorName }"
        ></div>
      </div>
    </div>
    <el-container>
      <el-main>
        <div class="crudbox">
          <avue-crud
            :option="option"
            :data="data.page"
            :table-loading="loading"
            @row-save="handleCreate"
            @row-del="handleDelete"
            @row-update="handleUpdate"
            @refresh-change="handleFresh"
            @size-change="handleSize"
            @current-change="handlePage"
            :page="page"
          >
            <template slot="callcode" slot-scope="scope">
              <el-tag
                style="cursor: pointer"
                @click="handleCellClick(scope.row)"
                >{{ scope.row.callcode }}</el-tag
              >
            </template>
          </avue-crud>
        </div>
      </el-main>
    </el-container>
    <formview ref="formview" />
  </div>
</template>

<script>
import formview from "./d_form_dialog";
export default {
  name: "form",
  components: {
    formview,
  },
  data() {
    return {
      loading: true,
      data: {
        list: [],
        page: [],
      },
      pageConfig: {
        limit: 10,
        page: 1,
      },
      page: {
        currentPage: 1,
        pageSize: 10,
        total: 0,
        pageSizes: [10, 20, 50, 100, 500],
        pagerCount: 11,
      },
      list: [],
      option: {
        addBtn: true,
        editBtn: true,
        delBtn: true,
        refreshBtn: true,
        columnBtn: true,
        dialogType: "dialog",
        dialogEscape: false,
        dialogModal: true,
        dialogClickModal: false,
        dialogCloseBtn: true,
        menuWidth: "200",
        menuAlign: "center",
        menuType: "text",
        index: true,
        border: true,
        headerAlign: "center",
        align: "center",
        column: [
          {
            label: "表单名称",
            prop: "cname",
            type: "input",
            span: 12,
            rules: [
              {
                required: true,
                message: "请输入表单名称",
                trigger: "blur",
              },
            ],
          },
          {
            label: "调用标识",
            prop: "callcode",
            slot: true,
            type: "input",
            span: 12,
            rules: [
              {
                required: true,
                message: "请输入调用标识",
                trigger: "blur",
              },
            ],
          },
          {
            label: "类别编号",
            prop: "cid",
            type: "input",
            span: 12,
            rules: [
              {
                required: true,
                message: "请输入类别编号",
                trigger: "blur",
              },
            ],
          },
          {
            label: "表单版本",
            prop: "ver",
            type: "number",
            valueDefault: 1,
            span: 12,
            rules: [
              {
                required: true,
                message: "请输入表单版本",
                trigger: "blur",
              },
            ],
          },
          {
            label: "表单备注",
            prop: "memo",
            type: "textarea",
            span: 24,
          },
          {
            label: "表单配置",
            prop: "ext",
            type: "textarea",
            span: 24,
            hide: true,
            rules: [
              {
                required: true,
                message: "请输入表单配置",
                trigger: "blur",
              },
            ],
          },
        ],
      },
    };
  },
  created() {
    this.getList();
  },
  methods: {
    handleCellClick(row) {
      this.$refs.formview.title = row.cname;
      this.$refs.formview.option = Object.assign(JSON.parse(row.ext));
      this.$refs.formview.dialogVisible = true;
    },

    getList() {
      this.loading = true;

      window.GFAPI.sys.FormApi.load()
        .then((res) => {
          var data = res.data.sort((a, b) => {
            return b.id - a.id;
          });
          this.data.list = data;
          this.page.total = data.length;
          this.loading = false;
          this.getPageList();
        })
        .catch(() => {
          this.data.list = [];
          this.page.total = 0;
          this.loading = false;
        });
    },

    getPageList() {
      this.loading = true;
      setTimeout(() => {
        this.data.page = this.data.list.slice(
          (this.pageConfig.page - 1) * this.pageConfig.limit,
          this.pageConfig.page * this.pageConfig.limit
        );
        this.loading = false;
      }, 350);
    },

    handlePage(page) {
      this.pageConfig.page = page;
      this.getPageList();
    },

    handleSize(size) {
      this.pageConfig.page = 1;
      this.pageConfig.limit = size;
      this.getPageList();
    },

    handleFresh() {
      this.getList();
    },

    handleCreate(row, done, loading) {
      var that = this;

      window.GFAPI.sys.FormApi.add(row)
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
                that.getList();
              },
            });
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
          window.GFAPI.sys.FormApi.del(row.id)
            .then((res) => {
              if (res.code == 0) {
                that.$message({
                  message: "删除成功",
                  type: "success",
                  center: true,
                  duration: 1000,
                  onClose() {
                    that.pageConfig.page = 1;
                    that.getList();
                  },
                });
              }
            })
            .catch(() => {});
        })
        .catch(() => {});
    },

    handleUpdate(row, index, done, loading) {
      var that = this;

      window.GFAPI.sys.FormApi.edit(row)
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
                that.getList();
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
<style scoped>
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
