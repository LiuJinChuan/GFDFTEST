<template>
  <div id="avue-view">
    <div class="head">
      <div class="title">
        代码段列表
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
            :data="list"
            :table-loading="loading"
            @row-save="handleCreate"
            @refresh-change="handleFresh"
            @row-del="handleDelete"
            @row-update="handleUpdate"
          >
            <template slot-scope="scope" slot="menu">
              <el-button
                type="text"
                icon="el-icon-s-goods"
                size="small"
                @click.stop="toCodeDtl(scope.row, scope.index)"
                >代码段版本</el-button
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
  name: "code",
  data() {
    return {
      loading: true,
      list: [],
      option: {
        addBtn: true,
        editBtn: true,
        delBtn: true,
        refreshBtn: true,
        columnBtn: true,
        menu: true,
        index: true,
        border: true,
        headerAlign: "center",
        align: "center",
        dialogType: "dialog",
        dialogFullscreen: false,
        dialogEscape: false,
        dialogModal: true,
        dialogClickModal: false,
        dialogCloseBtn: true,
        menuWidth: "250",
        menuAlign: "center",
        menuType: "text",
        labelWidth: "120",
        column: [
          {
            label: "ID",
            prop: "id",
            type: "input",
            addDisplay: false,
            editDisplay: false,
          },
          {
            label: "名称",
            prop: "cname",
            type: "input",
            span: 12,
            size: "small",
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
            size: "small",
            rules: [
              {
                required: true,
                message: "请输入调用标识",
                trigger: "blur",
              },
            ],
          },
          {
            label: "类别层次编号",
            prop: "cid",
            type: "input",
            span: 12,
            size: "small",
            rules: [
              {
                required: true,
                message: "请输入类别层次编号",
                trigger: "blur",
              },
            ],
          },
          {
            label: "数据标签",
            prop: "tags",
            type: "select",
            addDisplay: true,
            editDisplay: true,
            multiple: true,
            span: 24,
            dicData: [],
            rules: [
              {
                required: true,
                message: "请选择数据标签",
                trigger: "blur",
              },
            ],
          },
          {
            label: "备注",
            prop: "memo",
            hide: true,
            type: "textarea",
            span: 24,
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
    getOthers() {
      let types = window.GF.dicts.script_type;
      let tags = this.option.column.find((ele) => ele.prop == "tags");
      Vue.set(tags, "dicData", types);
    },

    toCodeDtl(row) {
      this.$router.push({
        path: "/dev/codedtl",
        query: {
          segid: row.id,
        },
      });
    },

    handleFresh() {
      this.getList();
    },

    getList() {
      this.loading = true;
      window.GFAPI.dev.CodeSegApi.load()
        .then((res) => {
          this.list = res.data;
          this.list.forEach((ele) => {
            Vue.set(ele, "tags", ele.tags.split(","));
          });
          this.loading = false;
        })
        .catch(() => {
          this.list = [];
          this.loading = false;
        });
    },

    handleCreate(row, done, loading) {
      var that = this;
      row.tags = row.tags.join(",");
      window.GFAPI.dev.CodeSegApi.add(row)
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
          window.GFAPI.dev.CodeSegApi.del(row)
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

    handleUpdate(row, index, done, loading) {
      var that = this;
      row.tags = row.tags.join(",");
      window.GFAPI.dev.CodeSegApi.edit(row)
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
