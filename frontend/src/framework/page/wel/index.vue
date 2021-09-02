<template>
  <div>
    <div class="wel__header">
      <div class="wel__info">
        <img
          src="../../../../public/img/welindex/head.png"
          alt
          class="wel__info-img"
        />
        <div class="wel__info-content">
          <div class="wel__info-title">{{ $t("wel.info") }}</div>
          <!-- <div class="wel__info-subtitle">
            {{$t('wel.dept')}}
          </div>-->
        </div>
      </div>
      <!-- <div class="wel__extra">
        <div class="wel__extra-item" @click="changRouter('/biz/memorialapply')">
          <div class="wel__extra-subtitleImg">
            <img src="../../../../public/img/welindex/create.png" />
          </div>
          <p class="wel__extra-titleImg">纪念馆申请审核</p>
        </div>
        <div class="wel__extra-item" @click="changRouter('/biz/verified')">
          <div class="wel__extra-subtitleImg">
            <img src="../../../../public/img/welindex/create.png" />
          </div>
          <p class="wel__extra-titleImg">实名认证审核</p>
        </div>
        <div class="wel__extra-item" @click="changRouter('/biz/agentapply')">
          <div class="wel__extra-subtitleImg">
            <img src="../../../../public/img/welindex/create.png" />
          </div>
          <p class="wel__extra-titleImg">代理商申请审核</p>
        </div>
      </div> -->
    </div>

    <!-- <el-row :span="24" type="flex" style="padding: 23px 28px 30px">
      <el-col :span="12">
        <div class="home-item home-item1">
          <div class="head">
            <div class="title">
              纪念馆申请审核
              <div
                class="color-div"
                :style="{ 'background-color': $store.state.common.colorName }"
              ></div>
            </div>
          </div>
          <apTable ref="apTable" :TableData="TableData">
            <template slot="cname" slot-scope="{ scope }">
              <span class="slotToDetail" @click="toDetail(scope.row)">{{
                scope.row.cname
              }}</span>
            </template>
          </apTable>
        </div>
        <span
          class="home-item-moreBtn"
          @click="changRouter('/biz/memorialapply')"
          >查看更多</span
        >
      </el-col>
      <el-col :span="12">
        <div class="home-item home-item1">
          <div class="head">
            <div class="title">
              今日生祭
              <div
                class="color-div"
                :style="{ 'background-color': $store.state.common.colorName }"
              ></div>
            </div>
          </div>
          <apTable ref="apTable1" :TableData="TableData1">
            <template slot="cname" slot-scope="{ scope }">
              <span class="slotToDetail" @click="toDetail1(scope.row)">{{
                scope.row.cname
              }}</span>
            </template>
          </apTable>
        </div>
        <span class="home-item-moreBtn" @click="changRouter('/biz/deceased')"
          >查看更多</span
        >
      </el-col></el-row
    > -->
  </div>
</template>

<script>
import { mapGetters } from "vuex";
import apTable from "@/framework/components/Table";
export default {
  name: "wel",
  components: {
    apTable,
  },
  data() {
    return {
      menus: [],
      // TableData: {
      //   initList: true,
      //   optionType: false, //是否需要远端配置
      //   permanentSearch: {}, //初始化时的搜索条件,不会被清除的固定条件,异步赋值会出现问题
      //   formatterListData: null, //格式化表格数据
      //   localDict: {}, //远端字典处理方案
      //   resetOrder: true,
      //   staticlist: [],
      //   staticOption: {
      //     index: true,
      //     border: true,
      //     clearable: true,
      //     refreshBtn: true,
      //     header: false,
      //     addBtn: false,
      //     delBtn: false,
      //     editBtn: false,
      //     saveBtn: false,
      //     updateBtn: false,
      //     cancelBtn: false,
      //     menu: false,
      //     menuType: "text",
      //     menuWidth: "120",
      //     labelWidth: 120,
      //     height: "350",
      //     menuAlign: "center",
      //     menuPosition: "right",
      //     headerAlign: "center",
      //     dialogEscape: false,
      //     dialogClickModal: false,
      //     dialogFullscreen: false,
      //     searchMenuSpan: 24,
      //     searchIndex: 3,
      //     emptySize: "medium",
      //     column: [
      //       {
      //         type: "input",
      //         label: "展示图片",
      //         width: 80,
      //         span: 12,
      //         slot: true,
      //         slotType: "imgs",
      //         formslot: true,
      //         formslotType: "img",
      //         prop: "imgurl",
      //       },
      //       {
      //         label: "名称",
      //         prop: "cname",
      //         slot: true,
      //         span: 12,
      //         width: 120,
      //         type: "input",
      //         align: "left",
      //         overHidden: true,
      //       },
      //       {
      //         label: "类型",
      //         prop: "ctype",
      //         width: 80,
      //         span: 12,
      //         type: "select",
      //         align: "left",
      //         dicDataType: 1,
      //         dicDataCallCode: "memorial_level",
      //         overHidden: true,
      //       },
      //       {
      //         label: "样式",
      //         prop: "style",
      //         span: 12,
      //         width: 80,
      //         type: "select",
      //         align: "left",
      //         dicDataType: 1,
      //         dicDataCallCode: "memorial_style",
      //         overHidden: true,
      //       },
      //       {
      //         label: "墓志铭",
      //         prop: "epitaph",
      //         span: 12,
      //         type: "input",
      //         align: "left",
      //         overHidden: true,
      //       },
      //       {
      //         label: "逝者简介",
      //         prop: "intro",
      //         span: 12,
      //         type: "input",
      //         align: "left",
      //         overHidden: true,
      //       },
      //     ],
      //   },
      //   OrderObj: { in: [{ flag: 0 }] },
      //   noPage: true,
      //   ajax: {
      //     Api: window.GFAPI.biz.Jg_applyApi,
      //     data: {
      //       callcode: "memorialapply",
      //     },
      //   },
      // },
      // TableData1: {
      //   initList: true,
      //   optionType: false, //是否需要远端配置
      //   permanentSearch: {}, //初始化时的搜索条件,不会被清除的固定条件,异步赋值会出现问题
      //   formatterListData: null, //格式化表格数据
      //   localDict: {}, //远端字典处理方案
      //   resetOrder: true,
      //   staticlist: [],
      //   staticOption: {
      //     index: true,
      //     border: true,
      //     clearable: true,
      //     refreshBtn: true,
      //     header: false,
      //     addBtn: false,
      //     delBtn: false,
      //     editBtn: false,
      //     saveBtn: false,
      //     updateBtn: false,
      //     cancelBtn: false,
      //     menu: false,
      //     menuType: "text",
      //     menuWidth: "120",
      //     labelWidth: 120,
      //     height: "350",
      //     menuAlign: "center",
      //     menuPosition: "right",
      //     headerAlign: "center",
      //     dialogEscape: false,
      //     dialogClickModal: false,
      //     dialogFullscreen: false,
      //     searchMenuSpan: 24,
      //     searchIndex: 3,
      //     emptySize: "medium",
      //     column: [
      //       {
      //         type: "input",
      //         label: "遗照",
      //         span: 12,
      //         slot: true,
      //         slotType: "head",
      //         formslot: true,
      //         formslotType: "img",
      //         prop: "photo",
      //       },
      //       {
      //         label: "姓名",
      //         prop: "cname",
      //         span: 12,
      //         slot: true,
      //         type: "input",
      //         align: "left",
      //         overHidden: true,
      //       },
      //       {
      //         label: "逝世天数",
      //         prop: "deathdays",
      //         span: 12,
      //         type: "number",
      //         align: "right",
      //         overHidden: true,
      //       },
      //       {
      //         label: "是否诞辰",
      //         prop: "birthday",
      //         span: 12,
      //         type: "switch",
      //         align: "left",
      //         overHidden: true,
      //         value: "0",
      //         dicData: [
      //           {
      //             label: "是",
      //             value: "1",
      //           },
      //           {
      //             label: "否",
      //             value: "0",
      //           },
      //         ],
      //       },
      //       {
      //         label: "是否忌日",
      //         prop: "deathday",
      //         span: 12,
      //         type: "switch",
      //         align: "left",
      //         overHidden: true,
      //         value: "0",
      //         dicData: [
      //           {
      //             label: "是",
      //             value: "1",
      //           },
      //           {
      //             label: "否",
      //             value: "0",
      //           },
      //         ],
      //       },
      //     ],
      //   },
      //   OrderObj: { or: [{ deathday: 1 }, { birthday: 1 }] },
      //   noPage: true,
      //   ajax: {
      //     Api: window.GFAPI.biz.DeceasedApi,
      //     data: {
      //       callcode: "deceased",
      //     },
      //   },
      // },
    };
  },
  computed: {
    ...mapGetters(["userInfo"]),
  },
  created() {
    // this.treetranslist(window.GF.context.get("actor").menus);
  },
  methods: {
    toDetail(row) {
      this.$router.push({
        path: "/biz/memorialapply/detail/" + row.id,
      });
    },
    toDetail1(row) {
      this.$router.push({
        path: "/biz/deceased/detail/" + row.id,
      });
    },
    changRouter(type) {
      let item = null;
      for (let i = 0; i < this.menus.length; i++) {
        if (this.menus[i].path == type) {
          item = this.menus[i];
          break;
        }
      }
      this.$router.$avueRouter.group = item.group;
      this.$router.$avueRouter.meta = item.meta;
      this.$router.push({
        path: this.$router.$avueRouter.getPath(
          {
            name: item.label,
            src: item.path,
          },
          item.meta
        ),
        query: item.query,
      });
    },
    treetranslist(source) {
      source.forEach((el) => {
        this.menus.push(el);
        el.children && el.children.length > 0
          ? this.treetranslist(el.children)
          : ""; // 子级递归
      });
    },
  },
};
</script>

<style scoped="scoped" lang="scss">
.chuli {
  color: #409eff;
  cursor: pointer;
}
.chuli:hover {
  color: #1585f5;
}
.wel {
  &__header {
    padding: 25px 40px;
    border-bottom: 1px solid #e8e8e8;
    background-color: #fff;
    display: flex;
    justify-content: space-between;
    align-items: center;
  }

  &__info {
    display: flex;
    align-items: center;

    &-img {
      border-radius: 72px;
      display: block;
      width: 72px;
      height: 72px;

      img {
        width: 100%;
        height: 100%;
        display: block;
      }
    }

    &-content {
      position: relative;
      margin-left: 24px;
      color: rgba(0, 0, 0, 0.45);
      line-height: 22px;
    }

    &-title {
      font-size: 20px;
      line-height: 28px;
      font-weight: 500;
      color: rgba(0, 0, 0, 0.85);
    }

    &-subtitle {
      position: relative;
      font-size: 14px;
      color: rgba(0, 0, 0, 0.45);
      line-height: 22px;
    }
  }

  &__extra {
    &-item {
      cursor: pointer;
      text-align: center;
      position: relative;
      padding: 0 58px;
      display: inline-block;

      &:last-child {
        &::after {
          display: none;
        }
      }

      &:after {
        background-color: #e8f3ff;
        position: absolute;
        top: 50%;
        transform: translateY(-50%);
        right: 0;
        width: 1px;
        height: 59px;
        content: "";
      }
    }

    &-title {
      color: rgba(0, 0, 0, 0.45);
      font-size: 14px;
      line-height: 22px;
      margin-bottom: 4px;

      span {
        color: rgba(0, 0, 0, 0.45);
        font-size: 20px;
      }
    }

    &-subtitle {
      color: rgba(0, 0, 0, 0.85);
      font-size: 30px;
      line-height: 38px;
      margin: 0;
    }

    &-subtitleImg {
      line-height: 38px;

      img {
        vertical-align: bottom;
      }
    }

    &-titleImg {
      margin-block-start: 10px;
      color: rgba(0, 0, 0, 0.45);
      font-size: 14px;
      line-height: 22px;
      margin-bottom: 4px;
    }
  }
}

.home-item {
  padding: 16px 18px 0 26px;
  height: 100%;
  overflow-y: auto;
  .head {
    position: relative;
    padding: 0 12px 12px;
    border-bottom: 1px solid #eef1f5;
    margin-bottom: 10px;

    .color-div {
      width: 4px;
      height: 14px;
      position: absolute;
      top: 6px;
      left: -19px;
    }

    .title {
      position: relative;
      color: #515a6e;
      font-size: 16px;
      line-height: 1.6;
    }
  }

  &-moreBtn {
    display: block;
    text-align: center;
    position: absolute;
    width: 100%;
    height: 37px;
    line-height: 37px;
    cursor: pointer;
    left: 0px;
    bottom: 0px;
    background-color: #e8eef4;
  }

  &-crad1 {
    height: calc(100% - 80px);
    overflow-y: auto;
    padding-bottom: 25px;
    box-sizing: border-box;
    &-item {
      border-bottom: 1px solid #eeeeee;
      padding: 19px 0 22px;
      display: flex;
      justify-content: space-between;
      font-size: 14px;

      a {
        color: #409eff;
      }
    }
  }
}

.el-row {
  margin-left: -14px;
  margin-right: -14px;
  background-color: #f0f2f5;

  .el-col {
    margin-left: 14px;
    margin-right: 14px;
    background-color: #ffffff;
    height: 500px;
    position: relative;
    box-shadow: 0px 1px 10px 0px rgba(37, 79, 123, 0.14);
  }
}
.wel__extra-item {
  padding: 0 50px !important;
}
/deep/.avue-crud__menu {
  display: none;
}
#avue-view {
  height: 380px;
}
</style>
