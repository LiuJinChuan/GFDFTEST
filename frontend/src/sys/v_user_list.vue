<template>
  <div>
    <apLayouts :Layouts="Layouts">
      <template slot="Tree">
        <apTree
          @TreeClick="TreeClick"
          ref="apTree"
          :TreeData="TreeData"
        ></apTree>
      </template>
      <template slot="Main">
        <apTable ref="apTable" :TableData="TableData">
          <template slot="menu" slot-scope="{ scope }">
            <el-button
              type="text"
              size="small"
              @click.native="resetPwd(scope.row)"
              v-if="scope.row.id"
              >重置密码</el-button
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

export default {
  name: "user",
  components: {
    apLayouts,
    apTable,
    apTree,
  },
  data: () => {
    return {
      isc: true,
      currdata: {},
      Layouts: {
        type: "hasTree", //hasTree、Main、deta
      },
      TreeData: {
        name: "tree1",
        node: "b", //node必填,并且不能改键,node就是包裹你想联动组件的id
        key: "id",
        isaddall: true,
        defaultExpandFloor: [0],
        editBtn: false,
        delBtn: false,
        clickisexpand: true,
        showCheckbox: false,
        strictly: false,
        optionType: false,
        staticOption: {
          column: [],
        },
        defaultProps: {
          children: "children",
          label: "cname",
        },
        ajax: {
          Api: window.GFAPI.sys.DeptApi,
          data: {
            callcode: "dept",
          },
          listFormatter(data) {
            let temp = window.GF.utils.buildListToTree(data, "pid", "id", "0");
            return temp;
          }, //格式化表格数据
        },
      },
      TableData: {
        initList: true,
        optionType: false, //是否需要远端配置
        staticlist: [],
        staticOption: {},
        permanentSearch: {}, //初始化时的搜索条件,不会被清除的固定条件,异步赋值会出现问题
        formatterListData: null, //格式化表格数据
        formatterFormData: null, //格式化表格数据
        localDict: {
          //远端字典处理方案
          dept: () => {
            return new Promise((resolve, reject) => {
              window.GFAPI.sys.DeptApi.load()
                .then((res) => {
                  resolve(res.data);
                })
                .catch((err) => {
                  reject(err);
                });
            });
          },
          roles: () => {
            return new Promise((resolve, reject) => {
              window.GFAPI.sys.RoleApi.load()
                .then((res) => {
                  res.data.forEach((element) => {
                    element.id = element.id + "";
                  });
                  resolve(res.data);
                })
                .catch((err) => {
                  reject(err);
                });
            });
          },
        },
        ajax: {
          Api: window.GFAPI.sys.UserApi,
          data: {
            callcode: "user",
          },
        },
      },
    };
  },
  computed: {},
  watch: {},
  created() {
    this.TableData.staticOption = {
      index: true,
      border: true,
      clearable: true,
      refreshBtn: true,
      header: true,
      addBtn: true,
      delBtn: true,
      editBtn: true,
      saveBtn: true,
      updateBtn: true,
      cancelBtn: true,
      menu: true,
      menuType: "text",
      menuWidth: "240",
      labelWidth: 120,
      height: "670",
      menuAlign: "center",
      menuPosition: "right",
      headerAlign: "center",
      dialogEscape: false,
      dialogClickModal: false,
      dialogFullscreen: false,
      searchMenuSpan: 24,
      searchIndex: 3,
      emptySize: "medium",
      column: [
        {
          label: "员工号",
          prop: "account",
          type: "input",
          align: "left",
          rules: [
            {
              required: true,
              message: "请输入账户",
              trigger: "blur",
            },
          ],
        },
        {
          label: "姓名",
          prop: "cname",
          type: "input",
          rules: [
            {
              required: true,
              message: "请输入用户名",
              trigger: "blur",
            },
          ],
          align: "left",
          required: true,
          maxlength: 20,
        },
        {
          label: "手机号码",
          prop: "phone",
          type: "input",
          align: "left",
          rules: [
            {
              required: true,
              message: "请输入电话",
              trigger: "blur",
            },
            {
              validator: "validatePhone",
              trigger: "blur",
            },
          ],
        },
        {
          label: "部门",
          prop: "dept",
          type: "tree",
          addDisplay: true,
          editDisplay: true,
          addDisabled: false,
          editDisabled: false,
          dicDataType: 2,
          dicDataCallCode: "dept",
          props: {
            label: "cname",
            value: "id",
          },
          align: "left",
          display: true,
          parent: true,
        },
        {
          label: "角色",
          prop: "roles",
          type: "select",
          hide: true,
          align: "left",
          tip: "请选择角色",
          dicDataType: 2,
          dicDataCallCode: "roles",
          multiple: true,
          props: {
            label: "cname",
            value: "id",
          },
        },
        {
          type: "select",
          label: "状态",
          dicData: [
            {
              label: "正常",
              value: 0,
              type: "",
            },
            {
              label: "停用",
              value: 1,
              type: "danger",
            },
          ],
          display: true,
          prop: "cstatus",
          slotType: "tag",
          slot: "tag",
          value: 0,
          required: true,
          rules: [
            {
              required: true,
              message: "请选择状态",
            },
          ],
        },
        {
          label: "扩展",
          prop: "ext",
          type: "input",
          hide: true,
          display: false,
          value: "{}",
          align: "left",
          tip: "请选择角色",
        },
      ],
    };
  },
  mounted() {},
  methods: {
    TreeClick({ data }) {
      let isc = data.children && data.children.length > 0;
      if (!isc) {
        this.$refs.apTable.resetSearchData({
          other: [{ dept: data.id }],
        });
      } else {
        this.$refs.apTable.setEmptyArray();
      }
    },
    //重置密码
    resetPwd(row) {
      let that = this;
      this.$confirm("是否确定重置密码?", "提示", {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        type: "warning",
      })
        .then(() => {
          //重置密码为：123456
          window.GFAPI.sys.UserApi.edit({
            id: row.id,
            pwd: "e10adc3949ba59abbe56e057f20f883e",
          })
            .then((res) => {
              if (res.code == 0) {
                that.$message({
                  message: "编辑成功",
                  type: "success",
                  center: true,
                  duration: 1000,
                  onClose() {
                    that.$refs.apTable.getList();
                  },
                });
              }
            })
            .catch(() => {});
        })
        .catch(() => {
          this.$message({
            type: "info",
            message: "放弃重置",
          });
        });
    },
  },
};
</script>
<style scoped>
</style>