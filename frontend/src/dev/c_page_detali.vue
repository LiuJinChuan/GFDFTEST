<template>
  <el-tabs class="tabsBox" v-model="activeName">
    <el-tab-pane label="基本配置" name="1">
      <avue-form :option="option1" v-model="form1"></avue-form>
    </el-tab-pane>
    <el-tab-pane label="按钮管理" name="2">
      <apTable :TableData="TableData"></apTable>
    </el-tab-pane>
    <el-tab-pane label="页面配置" name="3">
      <el-input
        type="textarea"
        :rows="2"
        placeholder="请输入内容"
        v-model="textarea"
      ></el-input>
    </el-tab-pane>
  </el-tabs>
</template>
<script>
import apTable from "@/framework/components/Table";
export default {
  name: "cpage",
  components: { apTable },
  data: () => {
    return {
      activeName: "1",
      option1: {
        column: [
          {
            type: "input",
            label: "名称",
            span: 12,
            display: true,
            size: "small",
            prop: "cname",
            rules: [
              {
                required: true,
                message: "名称必须填写",
              },
            ],
            required: true,
          },
          {
            type: "input",
            label: "组件路径",
            span: 12,
            display: true,
            size: "small",
            prop: "component",
          },
          {
            type: "input",
            label: "链接地址",
            span: 12,
            display: true,
            size: "small",
            prop: "link",
          },
          {
            type: "select",
            label: "所属模块",
            dicData: [],
            span: 12,
            display: true,
            size: "small",
            prop: "module",
            rules: [
              {
                required: true,
                message: "请选择所属模块",
              },
            ],
            required: true,
          },
          {
            type: "input",
            label: "调用名",
            span: 12,
            display: true,
            size: "small",
            prop: "callcode",
            rules: [
              {
                required: true,
                message: "调用名必须填写",
              },
            ],
            required: true,
          },
          {
            label: "菜单图标",
            prop: "icon",
            type: "icon",
            size: "small",
            iconList: [
              {
                label: "基本图标",
                list: [
                  "el-icon-info",
                  "el-icon-error",
                  "el-icon-success",
                  "el-icon-warning",
                  "el-icon-question",
                  "el-icon-edit",
                  "el-icon-delete",
                ],
              },
              {
                label: "方向图标",
                list: [
                  "el-icon-info",
                  "el-icon-back",
                  "el-icon-arrow-left",
                  "el-icon-arrow-down",
                  "el-icon-arrow-right",
                  "el-icon-arrow-up",
                ],
              },
              {
                label: "符号图标",
                list: [
                  "el-icon-plus",
                  "el-icon-minus",
                  "el-icon-close",
                  "el-icon-check",
                ],
              },
              {
                label: "阿里云图标",
                list: [
                  "iconfont icon-zhongyingwen",
                ],
              },
            ],
          },
          {
            type: "number",
            label: "排序",
            span: 12,
            display: true,
            size: "small",
            prop: "seqno",
          },
        ],
      },
      form1: {},
      TableData: {
        optionType: false,
        initList: true,
        permanentSearch: {},
        formatterListData(data) {
          data.forEach((e) => {
            e.btnposition = e.extobj.btnposition || 0;
          });
          return data;
        },
        formatterFormData(e) {
          e.extobj = {};
          e.extobj.btnposition = e.btnposition || 0;
          e.ext = JSON.stringify(e.extobj);
          return e;
        },
        staticOption: {
          refreshBtn: true,
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
              type: "input",
              label: "名称",
              span: 12,
              display: true,
              size: "small",
              prop: "cname",
              rules: [
                {
                  required: true,
                  message: "名称必须填写",
                },
              ],
              required: true,
            },
            {
              type: "input",
              label: "调用名",
              span: 12,
              display: true,
              size: "small",
              prop: "callcode",
              rules: [
                {
                  required: true,
                  message: "调用名必须填写",
                },
              ],
              required: true,
            },
            {
              type: "tree",
              label: "接口权限",
              span: 12,
              display: true,
              dicData: [],
              parent: true,
              prop: "webapi",
              multiple: true,
            },
            {
              type: "select",
              label: "按钮位置",
              span: 12,
              display: true,
              dicData: [
                {
                  label: "行按钮",
                  value: 0,
                },
                {
                  label: "左菜单",
                  value: 1,
                },
                {
                  label: "右菜单",
                  value: 2,
                },
                {
                  label: "默认按钮",
                  value: 3,
                },
                {
                  label: "浏览权限",
                  value: 4,
                },
              ],
              parent: true,
              prop: "btnposition",
            },
            {
              label: "菜单图标",
              prop: "icon",
              type: "icon",
              size: "small",
              iconList: [
                {
                  label: "基本图标",
                  list: [
                    "el-icon-info",
                    "el-icon-error",
                    "el-icon-success",
                    "el-icon-warning",
                    "el-icon-question",
                    "el-icon-edit",
                    "el-icon-delete",
                  ],
                },
                {
                  label: "方向图标",
                  list: [
                    "el-icon-info",
                    "el-icon-back",
                    "el-icon-arrow-left",
                    "el-icon-arrow-down",
                    "el-icon-arrow-right",
                    "el-icon-arrow-up",
                  ],
                },
                {
                  label: "符号图标",
                  list: [
                    "el-icon-plus",
                    "el-icon-minus",
                    "el-icon-close",
                    "el-icon-check",
                  ],
                },
                {
                  label: "阿里云图标",
                  list: [
                    "iconfont icon-zhongyingwen",
                  ],
                },
              ],
            },

            {
              type: "number",
              label: "排序",
              span: 12,
              display: true,
              size: "small",
              prop: "seqno",
            },
          ],
        },
        ajax: {
          Api: window.GFAPI.sys.ButtonApi,
        },
      },
      form2: {},
      textarea: "",
      dicts: [],
    };
  },
  computed: {
    id() {
      return this.$toute.params.id;
    },
  },
  watch: {},
  created() {
    this.TableData.permanentSearch = {
      other: [{ page: this.$route.params.id }],
    };
    this.TableData.permanentAddFrormData = { page: this.$route.params.id };
    this.getdetail();
  },
  mounted() {
    this.getOther();
  },
  methods: {
    getdetail() {
      window.GFAPI.sys.PageApi.view(this.$route.params.id).then((res) => {
        let form = JSON.parse(JSON.stringify(res.data));
        this.form1 = form;
      });
    },
    getOther() {
      let moduleData = window.GF.dicts.module_type;
      moduleData.forEach((mod) => {
        mod.isModule = true;
      });
      let moduleDic = JSON.parse(JSON.stringify(moduleData));
      let tabModule = this.option1.column.find((e) => e.prop == "module");
      this.$set(tabModule, "dicData", moduleDic);
      window.GFAPI.sys.WebapiApi.load().then((res) => {
        let webCtrlArr = window.GF.utils.buildListToTree(
          res.data,
          "pid",
          "id",
          0
        );
        webCtrlArr.forEach((el) => {
          el.value = el.callcode;
          el.label = el.cname;
        });
        moduleData.forEach((e) => {
          e.disabled = true;
          e.cname = e.label;
          e.module = "0";
        });
        let arrs = webCtrlArr.concat(moduleData);
        let temp = window.GF.utils.buildListToTree(
          arrs,
          "module",
          "value",
          "0"
        );
        let webapiModule = this.TableData.staticOption.column.find(
          (e) => e.prop == "webapi"
        );
        this.$set(webapiModule, "dicData", temp);
      });
    },
  },
};
</script>
<style lang='scss' scoped>
</style>