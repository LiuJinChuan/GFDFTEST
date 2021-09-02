<template>
  <div>
    <apLayouts :Layouts="Layouts">
      <template slot="Main">
        <apTable ref="apTable" :TableData="TableData">
          <template slot="cpath" slot-scope="{ scope }">
            <a target="_blank" style="color: #409eff" :href="scope.row.cpath"
              >预览</a
            >
          </template>
          <template slot="clink" slot-scope="{ scope }">
            <a target="_blank" style="color: #409eff" :href="scope.row.clink">{{
              scope.row.clink
            }}</a>
          </template>
          <template v-slot:menuLeft="{ scope: { btnArr } }">
            <el-button
              v-for="(item, index) in btnArr"
              :key="index"
              :type="item.type || 'primary'"
              :size="item.size || 'small'"
              @click="menuLeftClick(item.callcode)"
              :icon="item.icon"
              >{{ item.cname }}</el-button
            >
          </template>
          <template v-slot:menu="{ scope: { btnArr, row } }">
            <el-button
              v-for="(item, index) in btnArr"
              :key="index"
              :disabled="
                !(
                  ((item.callcode == 'pub' ||
                    item.callcode == 'edit' ||
                    item.callcode == 'del') &&
                    row.flag == 0) ||
                  ((item.callcode == 'edit' || item.callcode == 'stop') &&
                    row.flag == 1)
                )
              "
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
      title="范围发布"
      :visible.sync="pubShow"
      :close-on-click-modal="false"
      width="60%"
      top="8vh"
      append-to-body
    >
      <div class="block">
        <div class="block-left">
          <div class="block-top">
            <div>请选择广告位：</div>
            <el-tree
              :data="advertSpaceArr"
              show-checkbox
              node-key="id"
              default-expand-all
              ref="adverttree"
              :props="{
                children: 'children',
                label: 'cname',
              }"
            >
            </el-tree>
          </div>
          <div class="block-bottom">
            <div style="margin-bottom: 10px">请选择呈现时间：</div>
            <el-date-picker
              v-model="run_stime_etime"
              type="datetimerange"
              range-separator="至"
              start-placeholder="开始日期"
              end-placeholder="结束日期"
            >
            </el-date-picker>
          </div>
        </div>
        <div class="block-right">
          <div>请选择呈现范围：</div>
          <el-tree
            :data="pubShowData"
            show-checkbox
            default-expand-all
            node-key="id"
            ref="rangetree"
            :props="{
              children: 'children',
              label: 'cname',
            }"
          >
          </el-tree>
        </div>
      </div>
      <span slot="footer" class="dialog-footer">
        <el-button type="primary" @click="pub" :loading="loading"
          >发 布</el-button
        >
        <el-button type="primary" @click="pubShow = false">取 消</el-button>
      </span>
    </el-dialog>
  </div>
</template>
<script>
import apLayouts from "@/framework/components/layouts";
import apTable from "@/framework/components/Table";

export default {
  name: "advert",
  components: {
    apLayouts,
    apTable,
  },
  data: () => {
    return {
      pubShow: false,
      loading: false,
      run_stime_etime: [new Date(), new Date()],
      pubRow: {},
      pubData: [],
      pubShowData: [],
      advertSpaceAllArr: [],
      advertSpaceArr: [],
      currdata: {},
      Layouts: {
        type: "Main", //hasTree、Main、deta
      },
      TableData: {
        initList: true,
        optionType: true, //是否需要远端配置
        permanentSearch: {}, //初始化时的搜索条件,不会被清除的固定条件,异步赋值会出现问题
        formatterFormData: null, //格式化表格数据
        resetOrder: true,
        OrderObj: { order: [{ sort_id: "asc" }] },
        localDict: {}, //远端字典处理方案
        ajax: {
          Api: window.GFAPI.sys.AdvertApi,
          data: {
            callcode: "advert",
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
      window.GFAPI.sys.Advert_SpaceApi.load().then((res) => {
        if (res.code == 0) {
          this.advertSpaceAllArr = res.data.sort(
            (x, y) => x.sort_id - y.sort_id
          );
        }
      });
    },
    menuLeftClick(e) {
      if (e == "add") {
        this.$set(
          this.$refs.apTable.$refs.scrud.tableForm,
          "advertsid",
          this.currdata.id
        );
        this.$refs.apTable.$refs.scrud.rowAdd();
      }
    },
    menuClick(e, row) {
      if (e == "stop") {
        this.stop(row);
      }
      if (e == "pub") {
        this.pubShow = true;
        this.pubRow = row;
        let A = this.advertSpaceAllArr.filter((o) => o.ctype == row.ctype);
        let All = [{ cname: "全部", id: "0", children: A }];
        this.advertSpaceArr = All;
        this.run_stime_etime = [new Date(), new Date()];
        this.pubShowData = JSON.parse(JSON.stringify(this.pubData));
      }
      if (e == "edit") {
        this.$refs.apTable.$refs.scrud.rowEdit(row, 0);
      }
      if (e == "del") {
        this.$refs.apTable.$refs.scrud.rowDel(row, 0);
      }
    },
    pub() {
      this.loading = true;
      let advertArr = this.$refs.adverttree.getCheckedKeys(true);
      if (advertArr.length == 0) {
        this.$message({
          message: "请选择广告位",
          type: "error",
          center: true,
        });
        this.loading = false;
        return;
      }
      let selectArr = this.$refs.rangetree.getCheckedKeys(true);
      if (
        this.run_stime_etime[0].getTime() == this.run_stime_etime[1].getTime()
      ) {
        this.$message({
          message: "请选择正确的时间范围",
          type: "error",
          center: true,
        });
        this.loading = false;
        return;
      }
      window.GFAPI.sys.AdvertApi.pub({
        id: this.pubRow.id,
        range: selectArr.join(","),
        run_stime: this.run_stime_etime[0].getTime(),
        run_etime: this.run_stime_etime[1].getTime(),
        adverts: advertArr.join(","),
      })
        .then((res) => {
          if (res.code == 0) {
            this.$refs.apTable.resetSearchData();
            this.pubShow = false;
            this.loading = false;
            this.$message({
              message: "操作成功",
              type: "success",
              center: true,
            });
          }
        })
        .catch(() => {
          this.pubShow = false;
          this.loading = false;
        });
    },
    stop(row) {
      this.$confirm("请确认是否暂停发布当前项, 再继续?", "提示", {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        type: "warning",
      })
        .then(() => {
          window.GFAPI.sys.AdvertApi.stop(row.id).then((res) => {
            if (res.code == 0) {
              this.$refs.apTable.resetSearchData();
              this.$message({
                message: "操作成功",
                type: "success",
                center: true,
              });
            }
          });
        })
        .catch(() => {});
    },
  },
};
</script>
<style lang="scss" scoped>
.block {
  height: 62vh;
  overflow: scroll;
  display: flex;
  justify-content: space-between;
  .block-left {
    width: 40%;
  }
  .block-right {
    overflow: scroll;
    width: 60%;
  }
  .block-top {
    overflow: scroll;
    height: 80%;
  }
}
</style>