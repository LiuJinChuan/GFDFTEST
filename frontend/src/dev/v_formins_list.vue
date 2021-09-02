<template>
  <div id="avue-view">
    <div class="head">
      <div class="title">
        表单实例列表
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
            <template slot-scope="scope" slot="menu">
              <el-button
                type="text"
                icon="el-icon-share"
                size="small"
                @click.stop="handleHref(scope.row)"
                >链 接</el-button
              >
            </template>
          </avue-crud>
        </div>
      </el-main>
    </el-container>
  </div>
</template>

<script>
import Vue from "vue";
export default {
  name: "formins",
  data() {
    return {
      loading: true,
      forms: [],
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
            label: "ID",
            prop: "id",
            span: 12,
            addDisplay: false,
            editDisplay: false,
          },
          {
            label: "名称",
            prop: "cname",
            type: "input",
            span: 12,
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
            label: "表单",
            prop: "formid",
            type: "select",
            span: 12,
            dicData: [],
            props: {
              label: "cname",
              value: "id",
            },
            rules: [
              {
                required: true,
                message: "请选择表单",
                trigger: "blur",
              },
            ],
          },
          {
            label: "创建者",
            prop: "creator",
            type: "input",
            span: 12,
            rules: [
              {
                required: true,
                message: "请填写创建者",
                trigger: "blur",
              },
            ],
          },
          {
            label: "业务编号",
            prop: "bno",
            type: "input",
            span: 12,
            rules: [
              {
                required: true,
                message: "请填写业务编号",
                trigger: "blur",
              },
            ],
          },
          {
            label: "填写控制",
            prop: "scopecfg",
            type: "input",
            span: 12,
            hide: true,
          },
          {
            label: "备注",
            prop: "memo",
            type: "textarea",
            span: 24,
            hide: true,
          },
        ],
      },
    };
  },
  created() {
    this.getList();
    this.getOthers();
  },
  methods: {
    handleHref(row) {
      const h = this.$createElement;
      const link = `http://${location.host}/#/formexample?id=${row.formid}`;
      this.$msgbox({
        title: "",
        message: h("p", null, [
          h(
            "el-link",
            {
              attrs: { href: link, target: "_black", underline: false },
            },
            link
          ),
        ]),
      });
    },

    getOthers() {
      var that = this;

      window.GFAPI.sys.FormApi.load()
        .then((res) => {
          that.forms = res.data.sort((a, b) => {
            return b.id - a.id;
          });
          var forms = that.option.column.find((ele) => ele.prop == "formid");
          Vue.set(forms, "dicData", that.forms);
        })
        .catch(() => {
          var forms = that.option.column.find((ele) => ele.prop == "formid");
          Vue.set(forms, "dicData", []);
        });
    },

    handleCellClick(row) {
      this.$refs.formvalue.title = row.cname;
      var form = this.forms.find((ele) => ele.id == row.formid);
      this.$refs.formvalue.option = JSON.parse(form.ext);
      this.$refs.formvalue.data = JSON.parse(JSON.stringify(row));
      this.$refs.formvalue.form = JSON.parse(row.ext);
      this.$refs.formvalue.dialogVisible = true;
    },

    getList() {
      this.loading = true;

      window.GFAPI.dev.ForminsApi.load()
        .then((res) => {
          var data = res.data.sort((a, b) => {
            return b.id - a.id;
          });
          this.data.list = data;
          this.page.total = res.count;
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

      window.GFAPI.sys.ForminsApi.add(row)
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
          window.GFAPI.sys.ForminsApi.del(row.id)
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

      window.GFAPI.sys.ForminsApi.edit(row)
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
