<template>
  <div id="avue-view">
    <div class="head">
      <div class="title">
        数据资源库列表
        <div
          class="color-div"
          :style="{ 'background-color': $store.state.common.colorName }"
        ></div>
      </div>
    </div>
    <el-container>
      <el-main>
        <div class="crudbox">
          <el-row :gutter="15">
            <el-col :xs="24" :sm="24" :md="16" :lg="16" :xl="17">
              <div class="roles-left">
                <avue-crud
                  :option="option"
                  :data="list"
                  :table-loading="loading"
                >
                  <template slot-scope="scope" slot="menu">
                    <el-button
                      type="text"
                      icon="el-icon-edit"
                      size="small"
                      @click.stop="handleImport(scope.row.id)"
                      >导 入</el-button
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
            </el-col>
            <el-col :xs="24" :sm="24" :md="8" :lg="8" :xl="7">
              <div class="roles-right" v-loading="upLoading">
                <el-upload
                  accept=".csv"
                  name="files"
                  class="upload-demo"
                  :file-list="fileList"
                  :limit="5"
                  :before-upload="beforeUpload"
                  :on-success="onSuccess"
                  :on-error="onError"
                  :data="params"
                  :headers="myHeaders"
                  drag
                  :action="url"
                  multiple
                >
                  <i class="el-icon-upload"></i>
                  <div class="el-upload__text">
                    将文件拖到此处，或
                    <em>点击上传</em>
                  </div>
                  <div class="el-upload__tip" slot="tip">只能上传csv文件</div>
                </el-upload>
              </div>
            </el-col>
          </el-row>
        </div>
      </el-main>
    </el-container>
  </div>
</template>

<script>
import { apiRequest } from "@/framework/api/apiRequest";
export default {
  name: "csv",
  data() {
    return {
      loading: true,
      list: [],
      params: {},
      myHeaders: {
        Authorization: window.GFActor.cache.token,
      },
      upLoading: false,
      fileList: [],
      url: null,
      option: {
        index: true,
        refreshBtn: false,
        border: true,
        headerAlign: "center",
        align: "center",
        menuAlign: "center",
        menuType: "text",
        menuWidth: "150",
        dialogType: "dialog",
        dialogEscape: false,
        dialogModal: true,
        dialogClickModal: false,
        dialogCloseBtn: true,
        addBtn: false,
        delBtn: false,
        editBtn: false,
        columnBtn: true,
        column: [
          {
            label: "原文件名称",
            prop: "orgname",
            type: "input",
            span: 12,
            size: "small",
            rules: [
              {
                required: true,
                message: "请输入文件名称",
                trigger: "blur",
              },
            ],
          },
          {
            label: "新文件名称",
            prop: "newname",
            type: "input",
            span: 12,
            size: "small",
            rules: [
              {
                required: true,
                message: "请输入文件名称",
                trigger: "blur",
              },
            ],
          },
          {
            label: "上传者",
            prop: "creator",
            type: "input",
            span: 12,
            size: "small",
            rules: [
              {
                required: true,
                message: "请输入上传者",
                trigger: "blur",
              },
            ],
          },
        ],
      },
    };
  },
  created() {
    //TODO: url？ 且调用方式 | 为了给el-upload的action使用
    this.url =
      window.GF.cfg.domainprot + window.GF.cfg.domain + `/common/upload`;
    this.getList();
  },
  methods: {
    onSuccess(e) {
      if (e.code == 0) {
        var that = this;
        that.$message({
          message: "上传成功",
          type: "success",
          duration: 1000,
          onClose: function () {
            that.getList();
          },
        });
      } else {
        this.$message({
          message: "上传失败",
          type: "error",
          duration: 1000,
        });
      }
      this.upLoading = false;
    },

    onError() {
      this.$message({
        message: "上传失败",
        type: "error",
        duration: 1000,
      });
      this.upLoading = false;
    },

    beforeUpload(file) {
      if (file.name.indexOf(".csv") == -1) {
        this.$message({
          message: "只能上传csv文件!",
          type: "warning",
          duration: 1000,
        });
        return false;
      } else {
        this.upLoading = true;
      }
    },

    handleImport(id) {
      this.$router.push({
        path: "/dev/csvImport",
        query: { id },
      });
    },

    getList() {
      let obj = {
        order: [{ id: "desc" }],
        other: [{ creator: window.GFActor.user.id }],
      };
      window.GFAPI.sys.FileResApi.search(obj)
        .then((res) => {
          this.list = res.data;
          this.loading = false;
        })
        .catch(() => {
          this.list = [];
          this.loading = false;
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
          apiRequest(`/data/sy_csv/delete/${row.id}`, "post")
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
  },
};
</script>
<style scoped lang='scss'>
/deep/ .el-upload-list {
  display: none;
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
.roles-left,
.roles-right {
  border-radius: 4px;
  border: 1px solid #e6ebf5;
  padding: 15px;
}
.title-btn-right {
  display: flex;
  align-items: center;
  justify-content: space-between;
}
</style>
