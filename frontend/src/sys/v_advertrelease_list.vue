<template>
  <div>
    <apLayouts :Layouts="Layouts">
      <template slot="Tree">
        <apTree @TreeClick="TreeClick" ref="apTree" :TreeData="TreeData">
        </apTree>
      </template>
      <template slot="Main">
        <apTable ref="apTable" :TableData="TableData">
          <template v-slot:menu="{ scope: { btnArr, row } }">
            <el-button
              v-for="(item, index) in btnArr"
              :key="index"
              :type="item.type || 'text'"
              :size="item.size || 'small'"
              @click="menuClick(item.callcode, row)"
              :icon="item.icon"
              >{{ item.cname }}</el-button
            >
          </template>
        </apTable>
      </template>
    </apLayouts>
    <el-dialog
      title="预览"
      :visible.sync="pubShow"
      :close-on-click-modal="false"
      width="80%"
      top="8vh"
      append-to-body
    >
      <div class="block">
        <div class="treeBox">
          <span>范围：</span>
          <el-tree
            :data="pubData"
            show-checkbox
            default-expand-all
            :default-checked-keys="defaultTreeArr"
            node-key="id"
            ref="tree"
            highlight-current
            :props="{
              children: 'children',
              label: 'cname',
            }"
          >
          </el-tree>
        </div>
        <div class="viewBox">
          <span>文件：</span>
          <div class="preview">
            <el-image
              v-if="viewType == 'img'"
              :src="pubRow.cpath"
              width="100%"
            ></el-image>
            <video
              v-if="viewType == 'video'"
              :src="pubRow.cpath"
              controls
              autoplay
              width="100%"
            ></video>
          </div>
        </div>
      </div>
    </el-dialog>
  </div>
</template>
<script>
import apLayouts from "@/framework/components/layouts";
import apTable from "@/framework/components/Table";
import apTree from "@/framework/components/Tree";

export default {
  name: "advertrelease",
  components: {
    apLayouts,
    apTable,
    apTree,
  },
  data: () => {
    return {
      pubShow: false,
      viewType: "",
      defaultTreeArr: [],
      pubRow: {},
      pubData: [],
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
        showCheckbox: false,
        clickisexpand: false,
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
          Api: window.GFAPI.sys.Advert_SpaceApi,
          listFormatter(data) {
            let temp = data.sort((x, y) => x.sort_id - y.sort_id);
            return temp;
          },
          data: {
            callcode: "advertspace",
          },
        },
      },
      TableData: {
        initList: false,
        optionType: true, //是否需要远端配置
        permanentSearch: {}, //初始化时的搜索条件,不会被清除的固定条件,异步赋值会出现问题
        formatterListData(data) {
          data.forEach((item) => {
            item.run_stime_etime = [item.run_stime, item.run_etime];
          });
          return data;
        },
        formatterFormData(data) {
          let dataTemp = JSON.parse(JSON.stringify(data));
          [data.run_stime, data.run_etime] = [
            dataTemp.run_stime_etime[0],
            dataTemp.run_stime_etime[1],
          ];
          return data;
        }, //格式化表格数据
        resetOrder: true,
        OrderObj: { order: [{ sort_id: "asc" }] },
        localDict: {
          advertsid: () => {
            return new Promise((resolve, reject) => {
              window.GFAPI.sys.Advert_SpaceApi.load()
                .then((res) => {
                  resolve(res.data);
                })
                .catch((err) => {
                  reject(err);
                });
            });
          },
        }, //远端字典处理方案
        ajax: {
          Api: window.GFAPI.sys.Advert_ReleaseApi,
          data: {
            callcode: "advertrelease",
          },
        },
      },
    };
  },
  computed: {},
  watch: {},
  created() {
    this.GetPubTree();
  },
  mounted() {},
  methods: {
    GetPubTree() {
      let regiontree = JSON.parse(JSON.stringify(window.GF.regiontree));
      this.pubData = regiontree;
    },
    menuClick(e, row) {
      if (e == "view") {
        this.viewType = this.$refs.apTree.Tree.defaultList.find(
          (o) => o.id == row.advertsid
        ).ctype;
        this.pubShow = true;
        this.pubRow = row;
        this.defaultTreeArr = row.range.split(",");
      }
    },
    cancel() {
      this.pubShow = false;
    },
    TreeClick({ data }) {
      this.currdata = data;
      let obj = window.GF.utils.getChildCodeArrBf(data, "id");
      if (obj.arr.length > 0) {
        this.$refs.apTable.resetSearchData({
          in: [{ advertsid: obj.arr.join(",") }],
        });
      } else {
        this.$refs.apTable.setEmptyArray();
      }
    },
  },
};
</script>
<style lang="scss" scoped>
.block {
  height: 72vh;
  overflow: scroll;
  display: flex;
  justify-content: space-between;
  .treeBox {
    width: 30%;
    border-right: 1px solid #f2f2f2;
    height: 90%;
    overflow: scroll;
  }
  .viewBox {
    width: 70%;
    padding-left: 10px;
    .preview {
      display: flex;
      justify-content: center;
      height: 60vh;
      align-items: center;
      width: 100%;
    }
  }
}
</style>