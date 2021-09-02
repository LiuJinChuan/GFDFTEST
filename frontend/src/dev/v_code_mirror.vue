<template>
  <div id="avue-view" class="codemirror-edit-container">
    <div class="head">
      <div class="title">
        {{ info.title }}{{ info.act == "add" ? " 新增" : " 编辑" }}
        <div
          class="color-div"
          :style="{ 'background-color': $store.state.common.colorName }"
        ></div>
      </div>
    </div>
    <el-container>
      <el-main>
        <div class="crudbox">
          <div style="margin: 10px 0px">
            <el-button
              type="primary"
              icon="el-icon-plus"
              size="small"
              @click="handleSave"
              >保 存</el-button
            >
          </div>
          <div class="codemirror-container">
            <div
              style="
                width: 68%;
                height: 600px;
                border: 1px solid #dcdfe6;
                border-radius: 4px;
              "
            >
              <textarea ref="code"></textarea>
            </div>
            <el-input
              type="textarea"
              class="remark"
              :rows="2"
              placeholder="请输入说明内容"
              v-model="form.ext"
            ></el-input>
          </div>
        </div>
      </el-main>
    </el-container>
    <el-dialog
      :close-on-click-modal="false"
      :close-on-press-escape="false"
      :visible.sync="dialogVisible"
      title="参数"
      append-to-body
      width="60%"
    >
      <avue-form
        v-if="dialogVisible"
        :option="option"
        v-model="form"
        @submit="handleSubmit"
      ></avue-form>
      <div slot="footer" class="dialog-footer"></div>
    </el-dialog>
  </div>
</template>

<script>
const codedtlView = window.GFAPI.dev.CodeSegApi.view;

const codedtlCreate = window.GFAPI.dev.CodeSegDtlApi.add;

const codedtlUpdate = window.GFAPI.dev.CodeSegDtlApi.edit;

import "codemirror/lib/codemirror.css";
import CodeMirror from "codemirror/lib/codemirror";
import "codemirror/theme/idea.css"; //编辑器主题
require("codemirror/mode/javascript/javascript"); //mode js
require("codemirror/addon/edit/matchbrackets"); //括号
export default {
  name: "codemirror",
  mounted() {
    this.setCodeMirror();
  },
  data() {
    return {
      info: {},
      option: {
        menuPosition: "right",
        column: [
          {
            type: "input",
            label: "名称",
            span: 12,
            display: true,
            prop: "cname",
            required: true,
            rules: [
              {
                required: true,
                message: "请输入名称",
              },
            ],
          },
          {
            type: "input",
            label: "调用标识",
            span: 12,
            display: true,
            prop: "callcode",
            rules: [
              {
                required: true,
                message: "请输入调用标识",
              },
            ],
            required: true,
          },
          {
            type: "number",
            label: "版本号",
            span: 12,
            valueDefault: 1,
            display: true,
            prop: "ver",
            required: true,
            rules: [
              {
                required: true,
                message: "请输入版本号",
              },
            ],
          },
        ],
      },
      form: {
        cname: "",
        callcode: "",
        ext: "",
      },
      editor: null,
      dialogVisible: false,
    };
  },
  created() {
    this.getInfo();
  },
  watch: {
    "$route.query"() {
      this.getInfo();
    },
  },
  methods: {
    handleSubmit() {
      if (this.info.act == "edit") {
        this.handleUpdate();
      }
      if (this.info.act == "add") {
        this.handleCreate();
      }
    },

    handleCreate() {
      var that = this;
      var form = Object.assign(this.form, {
        memo: that.editor.getValue(),
        segid: that.info.segid,
      });
      codedtlCreate(form)
        .then((res) => {
          if (res.code == 0) {
            that.$message({
              message: "新增成功",
              type: "success",
              center: true,
              duration: 1000,
              onClose() {
                that.$router.$avueRouter.closeTag();
                that.$router.back();
              },
            });
          }
        })
        .catch(() => {});
    },

    handleUpdate() {
      var that = this;
      var form = Object.assign(this.form, {
        memo: that.editor.getValue(),
      });
      codedtlUpdate(form)
        .then((res) => {
          if (res.code == 0) {
            that.$message({
              message: "编辑成功",
              type: "success",
              center: true,
              duration: 1000,
              onClose() {
                that.$router.$avueRouter.closeTag();
                that.$router.back();
              },
            });
          }
        })
        .catch(() => {});
    },

    handleSave() {
      this.dialogVisible = true;
    },

    setCodeMirror() {
      this.editor = CodeMirror.fromTextArea(this.$refs.code, {
        mode: "text/javascript",
        lineNumbers: true,
        theme: "idea",
        matchBrackets: true, //括号
      });
    },

    getInfo() {
      this.info = this.$route.query;
      if (this.info.act == "add") {
        null;
      }
      if (this.info.act == "edit") {
        //编辑 通过id 获取代码段
        codedtlView(this.info.id)
          .then((res) => {
            this.form = res.data;
            this.editor.setValue(this.form.memo);
          })
          .catch(() => {});
      }
    },
  },
};
</script>
<style lang="scss" scoped>
.codemirror-edit-container {
  padding: 16px 24px;
  /deep/ .CodeMirror {
    height: 598px;
    border-radius: 4px;
  }
  .codemirror-container {
    display: flex;
    justify-content: space-between;
    font-size: 14px;
  }
  /deep/ .remark {
    width: 30%;
    textarea {
      height: 600px;
    }
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
