<template>
  <div>
    <Layouts :Layouts="Layouts">
      <template v-slot:Tree>
        <Tree @TreeClick="TreeClick" ref="trees" :TreeData="TreeData"></Tree>
      </template>
      <template v-slot:Main>
        <apTable ref="table" :TableData="TableData">
          <template slot="menu" slot-scope="{ scope: { row, size, type } }">
            <el-button @click="toDetail(row)" :size="size" :type="type"
              >更多</el-button
            >
          </template>
        </apTable>
      </template>
    </Layouts>
  </div>
</template>

<script>
import Layouts from "@/framework/components/layouts";
import Tree from "@/framework/components/Tree/index.vue";
import apTable from "@/framework/components/Table";

export default {
  name: "page",
  data() {
    return {
      json: {},
      Layouts: {
        type: "hasTree", //hasTree、Main、detail
      },
      TreeData: {
        name: "tree1",
        key: "id",
        showCheckbox: false,
        strictly: false,
        optionType: false,
        addBtn: false,
        editBtn: false,
        delBtn: false,
        list: [],
        staticOption: {
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
              rules: [
                {
                  required: true,
                  message: "组件路径必须填写",
                },
              ],
              required: true,
            },
            {
              type: "input",
              label: "链接地址",
              span: 12,
              display: true,
              size: "small",
              prop: "link",
              rules: [
                {
                  required: true,
                  message: "链接地址必须填写",
                },
              ],
              required: true,
            },
            {
              type: "select",
              label: "所属模块",
              dicData: [],
              props: {
                label: "cname",
                value: "cvalue",
              },
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
            {
              type: "textarea",
              label: "页面配置",
              span: 24,
              display: true,
              hide: true,
              prop: "ext",
            },
          ],
        },
        ajax: {
          Api: window.GFAPI.sys.PageApi,
          data: {},
        },
      },
      TableData: {
        optionType: false,
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
              label: "组件路径",
              span: 12,
              display: true,
              size: "small",
              prop: "component",
              rules: [
                {
                  required: true,
                  message: "组件路径必须填写",
                },
              ],
              required: true,
            },
            {
              type: "input",
              label: "链接地址",
              span: 12,
              display: true,
              size: "small",
              prop: "link",
              rules: [
                {
                  required: true,
                  message: "链接地址必须填写",
                },
              ],
              required: true,
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
            {
              type: "textarea",
              label: "页面配置",
              span: 24,
              display: true,
              hide: true,
              prop: "ext",
              formslot: true,
              formslotType: "json",
            },
          ],
        },
        resetOrder: true,
        OrderObj: { order: [{ seqno: "asc" }] },
        ajax: {
          Api: window.GFAPI.sys.PageApi,
        },
        formatterListData(data) {
          data.forEach((item) => {
            item.ext = item.extobj;
          });
          return data;
        },
        formatterFormData(data) {
          data.ext = JSON.stringify(data.ext);
          return data;
        },
      },
    };
  },
  components: {
    Layouts,
    Tree,
    apTable,
  },
  computed: {
    treeNodeData() {
      let data = null;
      return data;
    },
  },
  watch: {},
  created() {
    this.getOthers();
  },
  mounted() {},
  methods: {
    getOthers() {
      let moduleData = window.GF.dicts.module_type;
      moduleData.forEach((mod) => {
        mod.isModule = true;
      });
      let moduleDic = JSON.parse(JSON.stringify(moduleData));
      let tabModule = this.TableData.staticOption.column.find(
        (e) => e.prop == "module"
      );
      this.$set(tabModule, "dicData", moduleDic);

      window.GFAPI.sys.PageApi.load().then((res) => {
        let pageArr = res.data;
        pageArr.forEach((page) => {
          page.isPage = true;
          moduleData.forEach((mod) => {
            if (page.module == mod.value) {
              if (!mod.childrens) {
                mod.childrens = [page];
              } else {
                mod.childrens.push(page);
              }
            }
          });
          this.TreeData.list = moduleData;
        });
      });
    },
    TreeClick({ data }) {
      let info = {
        other: [{ module: data.value }],
      };
      this.$refs.table.resetSearchData(info);
    },
    toDetail(e) {
      this.$router.push({ path: `/dev/pageSetting/${e.id}` });
    },
  },
};
</script>
<style >
.jsoneditor-vue {
  height: 360px;
}
</style>
