
<template>
  <div>
    <apLayouts :Layouts="Layouts">
      <template v-slot:Tree>
        <apTree @TreeClick="TreeClick" :TreeData="TreeData"></apTree>
      </template>
      <template v-slot:Main>
        <apTable ref="apTable" :TableData="TableData" @TextInit="TextInit">
          <template slot-scope="scope" slot="contentForm">
            <div>
              <div
                id="text"
                ref="text"
                :data-value="scope.scope.row.content"
                @click.once="Th"
              ></div>
              <el-upload
                action="/common/upload"
                ref="upload"
                :headers="tokenHeaders"
                name="files"
                :before-remove="Cannotdel"
                :before-upload="SetisUploading"
                :on-success="AppendFiles"
              >
                <div class="attachclass">
                  <el-button size="small" type="primary">点击上传</el-button>
                  <span class="note">注：额外附件上传</span>
                </div>
              </el-upload>
            </div>
          </template>
          <template slot="menuLeft" slot-scope="{}">
            <el-button
              type="primary"
              :disabled="!isc"
              icon="el-icon-plus"
              size="small"
              @click="OpenAdd"
              >新 增</el-button
            >
          </template>
        </apTable>
      </template>
    </apLayouts>
  </div>
</template>

<script>
import apLayouts from "@/framework/components/layouts";
import apTable from "@/framework/components/Table";
import apTree from "@/framework/components/Tree";
import E from "wangeditor";

export default {
  name: "notice",
  components: {
    apLayouts,
    apTree,
    apTable,
  },
  data() {
    return {
      currTreeData: {},
      isc: false,
      tokenHeaders: {
        Authorization: window.GFActor.cache.token,
      },
      isUploading: false,
      loading: true,
      editorValue: "",
      /*布局*/
      Layouts: {
        type: "hasTree",
      },
      TreeData: {
        name: "tree1",
        node: "b", //node必填,并且不能改键,node就是包裹你想联动组件的id
        key: "cvalue",
        isaddall: true,
        editBtn: false,
        delBtn: false,
        defaultExpandAll: true,
        clickisexpand: false,
        showCheckbox: false,
        optionType: false,
        staticOption: {
          column: [],
        },
        defaultProps: {
          children: "children",
          label: "cname",
        },
        ajax: {
          Api: {
            load: () =>
              new Promise((resolve, reject) => {
                window.GFAPI.sys.FormvalueApi.view("notice_type")
                  .then((res) => {
                    let temp = window.GF.utils.buildListToTree(
                      res.data.extobj.noticeType,
                      "pcvalue",
                      "cvalue",
                      ""
                    );
                    res.data = temp;
                    resolve(res);
                  })
                  .catch((err) => {
                    reject(err);
                  });
              }),
          },
        },
      },
      TableData: {
        ishasfwb: true,
        initList: false,
        formatterListData: null,
        formatterFormData(data) {
          data.ext = JSON.stringify({
            form_type: "notice_type",
            commid: window.GFActor.user.extobj.commid,
            disid: window.GFActor.user.extobj.disid,
            regioncode: window.GFActor.user.extobj.regioncode,
            userid: window.GFActor.user.id,
          });
          return data;
        },
        optionType: true, //是否需要远端配置
        resetOrder: true,
        OrderObj: {
          like: [
            { ext: "notice_type" },
            { ext: window.GFActor.user.extobj.commid },
            { ext: window.GFActor.user.extobj.disid },
          ],
        },
        localDict: {
          cid: () => {
            return new Promise((resolve, reject) => {
              window.GFAPI.sys.FormvalueApi.view("notice_type")
                .then((res) => {
                  let temp = window.GF.utils.buildListToTree(
                    res.data.extobj.noticeType,
                    "pcvalue",
                    "cvalue",
                    ""
                  );
                  resolve(temp);
                })
                .catch((err) => {
                  reject(err);
                });
            });
          },
          scope: () => {
            return new Promise((resolve, reject) => {
              let extobj = window.GFActor.user.extobj;
              if (extobj.commid != 0 && extobj.disid != 0) {
                window.GFAPI.biz.DistrictApi.view(extobj.disid)
                  .then((res) => {
                    res.data.id = res.data.id + "";
                    let temp = [];
                    temp.push(res.data);
                    resolve(temp);
                  })
                  .catch((err) => {
                    reject(err);
                  });
              }
              if (extobj.commid != 0 && extobj.disid == 0) {
                window.GFAPI.biz.DistrictApi.search({
                  order: [{ id: "desc" }],
                  other: [{ commid: extobj.commid }],
                })
                  .then((res) => {
                    res.data.forEach((o) => {
                      o.id = o.id + "";
                    });
                    resolve(res.data);
                  })
                  .catch((err) => {
                    reject(err);
                  });
              }
              if (extobj.commid == 0 && extobj.disid == 0) {
                try {
                  let regions = JSON.parse(JSON.stringify(window.GF.regions));
                  regions = regions.filter(
                    (o) => o.level == "0" || o.level == "1"
                  );
                  regions.forEach((o) => {
                    o.id = o.code;
                  });
                  let temp = window.GF.utils.buildListToTree(
                    regions,
                    "jparentcode",
                    "jcode",
                    "0"
                  );
                  resolve(temp);
                } catch (err) {
                  reject(err);
                }
              }
            });
          },
        },
        ajax: {
          Api: window.GFAPI.sys.NoticeApi,
          data: {
            callcode: "notice",
          },
        },
      },
    };
  },
  created() {},
  mounted() {},
  methods: {
    //左侧树点击
    TreeClick({ data }) {
      this.currTreeData = data;
      let obj = window.GF.utils.getChildCodeArrBf(data, "cvalue", false, "");
      this.isc = obj.isb;
      if (obj.arr.length > 0) {
        let info = { in: [{ cid: obj.arr.join(",") }] };
        this.$refs.apTable.resetSearchData(info);
        if (data.children.length == 0) {
          if (data.teamcount) {
            this.$set(
              this.$refs.apTable.$refs.scrud.tableForm,
              "cid",
              data.cvalue
            );
          }
        }
      } else {
        this.$refs.apTable.setEmptyArray();
      }
    },
    /*富文本改变*/
    change(val) {
      !val ? console.log(val) : null;
      this.$refs.apTable.$refs.scrud.tableForm.content = val;
    },
    //新增
    OpenAdd() {
      this.$set(
        this.$refs.apTable.$refs.scrud.tableForm,
        "cid",
        this.currTreeData.cvalue
      );
      this.$refs.apTable.$refs.scrud.rowAdd();
    },
    //富文本
    TextInit(done) {
      let that = this;
      done();
      setTimeout(() => {
        that.editor2 = new E("#text");
        that.$refs.text.click();
        var editor2 = that.editor2;
        editor2.customConfig.uploadImgServer = "/common/upload";
        editor2.customConfig.uploadFileName = "files";
        editor2.customConfig.uploadImgHeaders = {
          Authorization: window.GFActor.cache.token,
        };
        editor2.customConfig.menus = [
          // 菜单配置
          "head", // 标题
          "bold", // 粗体
          "fontSize", // 字号
          "fontName", // 字体
          "italic", // 斜体
          "underline", // 下划线
          "strikeThrough", // 删除线
          "foreColor", // 文字颜色
          "backColor", // 背景颜色
          "link", // 插入链接
          "list", // 列表
          "justify", // 对齐方式
          "quote", // 引用
          "emoticon", // 表情
          "image", // 插入图片
          "table", // 表格
          "code", // 插入代码
          "undo", // 撤销
          "redo", // 重复
        ];
        editor2.customConfig.onchange = function (html) {
          that.editorValue = html;
          that.change(that.editorValue);
        };

        editor2.customConfig.uploadImgHooks = {
          before: function () {},
          success: function () {},
          fail: function () {},
          error: function () {},
          timeout: function () {},
          customInsert: function (insertImg, result) {
            if (result.code == 0) {
              that.$message({
                message: "图片上传成功",
                type: "success",
                center: true,
                duration: 1000,
              });
              var url = result.data.Url;
              insertImg(url);
            }
          },
        };

        editor2.create();
      }, 50);
    },
    //上传
    AppendFiles(response, file) {
      this.isUploading = false;
      if (response.code == 0) {
        let s = file.response.data.Url;
        let t = file.name;
        this.editor2.txt.append(`<a href=${s}>${t}</a>`);
        this.editor2.change();
        this.$message({
          message: "上传成功",
          type: "success",
          center: true,
          duration: 1000,
        });
        setTimeout(() => {
          this.$refs.upload.clearFiles();
        }, 1500);
      } else {
        this.$message({
          message: "上传失败",
          type: "info",
          center: true,
          duration: 1000,
        });
      }
    },
    SetisUploading() {
      this.isUploading = true;
    },
    //取消上传
    Cannotdel() {
      let that = this;
      return new Promise((resove, reject) => {
        if (that.isUploading) {
          that.$message({
            message: "上传已取消",
            type: "success",
            center: true,
            duration: 1000,
          });
          resove();
        } else {
          that.$message({
            message: "无法取消上传",
            type: "info",
            center: true,
            duration: 1000,
          });
          reject(undefined);
        }
      });
    },
    Th(e) {
      this.$nextTick(() => {
        this.editor2.txt.html(e.srcElement.dataset.value);
        this.editor2.change();
      });
    },
  },
};
</script>
<style lang='scss' scoped>
/deep/.w-e-text > a {
  cursor: pointer;
  color: #0000ee;
  text-decoration: underline;
}
/deep/#text div {
  z-index: 0 !important;
}
/deep/.el-row {
  display: flex !important;
  flex-direction: row;
  justify-content: space-between;
  .avue-group {
    width: 100%;
    flex-grow: 5;
  }
  .el-col {
    flex-grow: 1;
  }
}
.attachclass {
  margin-top: 10px;
  .note {
    color: red;
    margin-left: 8px;
  }
}
</style>
