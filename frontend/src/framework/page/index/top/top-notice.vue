<template>
  <el-popover placement="bottom" width="350" trigger="click">
    <el-tabs
      v-model="activeName"
      type="card"
      :stretch="true"
      @tab-click="tabClick"
    >
      <el-tab-pane
        v-for="(item, index) in titlelist"
        :key="index"
        :label="item.label"
        :name="item.value"
      >
        <el-scrollbar style="height: 300px">
          <avue-notice
            ref="cc"
            :data="list"
            :option="option"
            @click="changRouter"
            @page-change="pageChange"
          ></avue-notice>
        </el-scrollbar>
      </el-tab-pane>
    </el-tabs>
    <div slot="reference">
      <el-badge :value="sum" :hidden="sum < 1" :max="99">
        <i class="el-icon-bell"></i>
      </el-badge>
    </div>
  </el-popover>
</template>

<script>
export default {
  name: "top-notice",
  data() {
    return {
      titlelist: [],
      activeName: "1",
      option: {
        props: {
          img: "img",
          title: "title",
          subtitle: "subtitle",
          tag: "tag",
          status: "status",
        },
      },
      groupData: {},
      list: [],
      menus: [],
    };
  },
  computed: {
    sum() {
      let num = 0;
      for (var key in this.groupData) {
        num += this.groupData[key].length;
      }
      return num;
    },
  },
  created() {
    window.GFAPI.sys.FormvalueApi.getFormExt("msg_type").then((resd) => {
      if (resd.code == 0) {
        resd.data.msgType.forEach((o) => {
          o.label = o.cname;
          o.value = o.cvalue;
        });
        let temp = resd.data.msgType.sort((x, y) => x.speno - y.speno);
        this.titlelist = temp;
        this.getList();
      }
    });

    window.GF.emitter.on("WebSocket_msg", (e) => {
      let data = JSON.parse(e.data);
      const h = this.$createElement;
      this.$notify({
        title: "新消息",
        message: h("i", { style: "color: teal" }, data.msg),
        duration: 2000,
      });
      data.title = data.msg;
      data.subtitle = window.GF.utils.parseTime(data.createtime);
      this.groupData[this.activeName].unshift(data);
      this.titlelist.forEach((o) => {
        if (this.groupData[o.value]) {
          o.label = o.cname + `(${this.groupData[o.value].length})`;
        } else {
          o.label = o.cname + `(0)`;
        }
      });
      this.list = this.groupData[this.activeName];
    });
  },
  methods: {
    getList() {
      let obj = {
        other: [
          { flag: 0 },
          { touser: window.GF.context.get("actor").user.id },
        ],
        order: [{ id: "desc" }],
      };
      window.GFAPI.sys.MsgUserApi.search(obj)
        .then((res) => {
          if (res.code == 0) {
            if (res.data.length > 0) {
              res.data.forEach((item) => {
                item.title = item.msg;
                item.subtitle = window.GF.utils.parseTime(item.createtime);
              });
              this.groupData = window.GF.utils.ItemGroupBy(res.data, "code");
              this.titlelist.forEach((o) => {
                if (this.groupData[o.value]) {
                  o.label = o.cname + `(${this.groupData[o.value].length})`;
                } else {
                  o.label = o.cname + `(0)`;
                }
              });
              this.list = this.groupData[this.activeName];
            } else {
              this.list = [];
              this.groupData = {};
              this.titlelist.forEach((o) => {
                o.label = o.cname + `(0)`;
              });
            }
          }
        })
        .catch(() => {
          this.list = [];
          this.groupData = {};
          this.titlelist.forEach((o) => {
            o.label = o.cname + `(0)`;
          });
        });
    },
    pageChange(page, done) {
      setTimeout(() => {
        this.$message.success("没有新消息");
        done();
      }, 1000);
    },
    tabClick() {
      this.list = this.groupData[this.activeName];
    },
    //点击通知跳转页面
    changRouter(row) {
      window.GFAPI.sys.MsgUserApi.edit({ id: row.id, flag: 1 }).then(() => {
        for (let i = 0; i < this.groupData[this.activeName].length; i++) {
          if (this.groupData[this.activeName][i].id == row.id) {
            this.groupData[this.activeName].splice(i, 1);
          }
        }
        this.list = this.groupData[this.activeName];
        this.titlelist.forEach((o) => {
          if (o.value == this.activeName) {
            o.label = o.cname + `(${this.groupData[this.activeName].length})`;
          }
        });
      });
      // this.$router.push({
      //   path: row.clink,
      //   query: row.query,
      // });
    },
    // treetranslist(source) {
    //   source.forEach((el) => {
    //     this.menus.push(el);
    //     el.children && el.children.length > 0
    //       ? this.treetranslist(el.children)
    //       : ""; // 子级递归
    //   });
    // },
  },
};
</script>

<style lang="scss" scoped>
</style>
