<template>
  <div class="user-info">
    <el-row :span="24" style="padding: 15px">
      <el-col :span="8" style="background-color: white; padding: 15px; margin-right: 15px">
        <div>
          <div class="user-info__content">
            <img class="user-info__img" :src="sends.avatar" alt />
            <p class="user-info__name">{{ sends.cname }}</p>
            <router-link class="user-info__setting" :to="{ path: '/info/setting' }">个人设置</router-link>
            <p class="user-info__desc">{{ sends.msg }}</p>
            <div class="user-info__detail-desc">
              <p>
                <i class="el-icon-s-custom"></i>
                <span>{{ sends.sex ? "男" : "女" }}</span>
              </p>
              <p>
                <i class="el-icon-mobile-phone"></i>
                <span>{{ sends.phone }}</span>
              </p>
              <p>
                <i class="el-icon-message"></i>
                <span>{{ sends.email }}</span>
              </p>
              <p>
                <i class="el-icon-postcard"></i>
                <span>{{ sends.roleMsg }}</span>
              </p>
            </div>
            <div class="user-info__divider"></div>
          </div>
        </div>
      </el-col>
      <el-col :span="15" style="background-color: white">
        <div>
          <!--<avue-tabs :option="option" v-model="sends" @change="handleChange" @submit="handleSubmit"></avue-tabs>-->

          <avue-tabs :option="option" @change="handleChange"></avue-tabs>
          <span v-if="type.prop === 'tab1'">
            <avue-form :option="info.option" v-model="form" :uploadAfter="uploadAfter" @submit="handleSubmit">
            </avue-form>
          </span>
          <span v-else-if="type.prop === 'tab2'">
            <avue-form :option="change.option" v-model="forms" @submit="handleSubmit">
            </avue-form>
          </span>
        </div>
      </el-col>
    </el-row>
  </div>
</template>

<script>
import { resetRouter } from "@/framework/router/router.js";
import Vue from "vue";
export default {
  data() {
    const img =
      "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1599560852925&di=6199cb5d372785567c5ffc9d1f554432&imgtype=0&src=http%3A%2F%2Fb-ssl.duitang.com%2Fuploads%2Fitem%2F201706%2F10%2F20170610192627_yhAMN.thumb.700_0.jpeg";
    var validatePhone = function (rule, value, callback) {
      if (!/^1(3|4|5|6|7|8|9)\d{9}$/.test(value)) {
        callback(new Error("电话格式有误"));
      } else {
        callback();
      }
    };
    var validateEmail = function (rule, value, callback) {
      if (
        !/^\w+((-\w+)|(\.\w+))*@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z0-9]+$/.test(
          value
        )
      ) {
        callback(new Error("邮箱格式有误"));
      } else {
        callback();
      }
    };
    var validatePass = (rule, value, callback) => {
      if (value.length != 6) {
        callback(new Error("密码需为6位"));
      } else {
        callback();
      }
    };
    var validateSurePass = (rule, value, callback) => {
      if (value.length != 6) {
        callback(new Error("密码需为6位"));
      } else if (this.forms.new != value) {
        callback(new Error("两次密码输入不一致"));
      } else {
        callback();
      }
    };
    return {
      type: "tab1",
      img: img,
      option: {
        column: [
          {
            label: "个人信息",
            prop: "tab1",
          },
          {
            label: "修改密码",
            prop: "tab2",
          },
        ],
      },
      info: {
        option: {
          column: [
            {
              label: "头像",
              type: "upload",
              limit: 1,
              listType: "picture-card",
              propsHttp: {
                fileName: "files",
              },
              action: "/common/upload",
              tip: "只能上传jpg/png用户头像，且不超过500kb",
              span: 16,
              prop: "avatar",
              headers: {
                Authorization: window.GFActor.cache.token,
              },
            },
            {
              label: "姓名",
              span: 24,
              prop: "cname",
              rules: [
                {
                  required: true,
                  message: "请输入姓名",
                  trigger: "blur",
                },
              ],
            },
            {
              label: "性别",
              type: "select",
              dicData: [
                {
                  label: "男",
                  value: 1,
                },
                {
                  label: "女",
                  value: 0,
                },
              ],
              rules: [
                {
                  required: true,
                  message: "请选择性别",
                  trigger: "change",
                },
              ],
              span: 24,
              prop: "sex",
            },
            {
              label: "电话",
              span: 24,
              prop: "phone",
              rules: [
                {
                  required: true,
                  message: "请输入电话",
                  trigger: "blur",
                },
                { validator: validatePhone, trigger: "blur" },
              ],
            },
            {
              label: "邮箱",
              row: true,
              span: 24,
              prop: "email",
              rules: [
                {
                  required: true,
                  message: "请输入邮箱",
                  trigger: "blur",
                },
                { validator: validateEmail, trigger: "blur" },
              ],
            },
            {
              label: "部门",
              prop: "dept",
              type: "tree",
              dicData: [],
              parent: false,
              props: {
                label: "cname",
                value: "id",
              },
            },
          ],
          gutter: 20,
          labelPosition: "right",
          menuBtn: true,
          submitBtn: true,
          menuPosition: "right",
          submitSize: "medium",
          submitText: "提交",
          emptyBtn: true,
          emptySize: "medium",
          emptyText: "重置",
        },
      },
      change: {
        option: {
          column: [
            {
              label: "原密码",
              span: 16,
              row: true,
              type: "password",
              prop: "old",
              rules: [
                {
                  required: true,
                  message: "请输入原密码",
                  trigger: "blur",
                },
                { validator: validatePass, trigger: "blur" },
              ],
            },
            {
              label: "新密码",
              span: 16,
              row: true,
              type: "password",
              prop: "new",
              rules: [
                {
                  required: true,
                  message: "请输入新密码",
                  trigger: "blur",
                },
                { validator: validatePass, trigger: "blur" },
              ],
            },
            {
              label: "确认密码",
              span: 16,
              row: true,
              type: "password",
              prop: "sure",
              rules: [
                {
                  required: true,
                  message: "请确认密码",
                  trigger: "blur",
                },
                { validator: validateSurePass, trigger: "blur" },
              ],
            },
          ],
          gutter: 20,
          labelPosition: "right",
          menuBtn: true,
          submitBtn: true,
          menuPosition: "right",
          submitSize: "medium",
          submitText: "提交",
          emptyBtn: true,
          emptySize: "medium",
          emptyText: "重置",
        },
      },
      form: {
        cname: "",
        msg: "",
        sex: "",
        phone: "",
        email: "",
        roleMsg: "",
        avatar: "",
      },
      forms: {
        old: "",
        new: "",
        sure: "",
      },
      sends: {},
    };
  },
  computed: {},

  created() {
    this.type = this.option.column[0];
    this.getInfo();
    this.getOthers();
  },
  methods: {
    getOthers() {
      window.GFAPI.sys.DeptApi.load().then((res) => {
        var infos = this.info.option.column.find((ele) => ele.prop == "dept");
        Vue.set(
          infos,
          "dicData",
          window.GF.utils.buildListToTree(res.data, "pid", "id", 0)
        );
      });
    },

    getInfo() {
      window.GFAPI.sys.RoleApi.load().then((res) => {
        var form = window.GFActor.user;
        form.avatar = form.extobj.avatar || "";
        form.msg = this.getTimeState();
        this.form = Object.assign(this.form, form);
        this.sends = JSON.parse(JSON.stringify(this.form));
        if (!this.sends.dept) {
          this.sends.dept = null;
        }
        this.sends.roles = this.sends.roles.split(",");
        Vue.set(this.sends, "roleMsg", []);
        this.sends.roles.forEach((item) => {
          res.data.forEach((ele) => {
            if (item == ele.id) {
              this.sends.roleMsg.push(ele.memo);
            }
          });
        });
        this.sends.roleMsg = this.sends.roleMsg.join(" | ");
      });
    },

    getTimeState() {
      var date = new Date();
      var hours = date.getHours();
      var msg = "";
      if (hours >= 0 && hours <= 10) {
        msg = `GOOD MORNNG!`;
      } else if (hours > 10 && hours <= 14) {
        msg = `GOOD NOON!`;
      } else if (hours > 14 && hours <= 18) {
        msg = `GOOD AFTERNOON!`;
      } else if (hours > 18 && hours <= 24) {
        msg = `GOOD NIGHT!`;
      }
      return msg;
    },
    uploadAfter(res, done) {
      res.url = res.Url;
      res.name = res.orgname;
      done(res);
    },
    handleSubmit() {
      var that = this;
      if (this.type.prop == "tab1") {
        let extobj = this.form.extobj;
        extobj.avatar = this.form.avatar;
        var form = {
          cname: this.form.cname,
          sex: this.form.sex,
          phone: this.form.phone,
          email: this.form.email,
          dept: this.form.dept,
          id: this.sends.id,
          ext: JSON.stringify(extobj),
        };
        for (const key in form) {
          const element = form[key] == null;
          element ? delete form[key] : null;
        }
        window.GFAPI.sys.UserApi.edit(form)
          .then(() => {
            that.$message({
              message: "提交成功",
              type: "success",
              center: true,
              duration: 1000,
              onClose() {
                that.$router.go(0);
              },
            });
          })
          .catch(() => { });
      }
      if (this.type.prop == "tab2") {
        var psd = {
          newpwd: that.forms.new,
          oldpwd: that.forms.old,
        };
        window.GFAPI.sys.UserApi.ChangePWD(psd)
          .then(() => {
            that.$store
              .dispatch("LogOut", that.$store.state.user.token)
              .then(() => {
                that.$message({
                  message: "修改成功，2秒后自动退出",
                  type: "success",
                  center: true,
                  duration: 2000,
                  onClose() {
                    resetRouter();
                    that.$store.commit("SET_MENU", []);
                    that.$router.push({ path: "/login" });
                  },
                });
              });
          })
          .catch(() => { });
      }
    },

    handleChange(column) {
      this.type = column;
      // this.type = item.prop;
    },
  },
};
</script>

<style lang="scss">
.user-info {
  .avue-tabs {
    padding: 0 10px;
  }

  .el-tabs__content {
    padding: 20px 0;
  }

  &__img {
    display: block;
    margin: 0 auto;
    border-radius: 100%;
    width: 100px;
    height: 100px;
  }

  &__name {
    text-align: center;
    font-size: 20px;
    line-height: 28px;
    font-weight: 500;
    color: rgba(0, 0, 0, 0.85);
    margin-bottom: 0;
    margin-top: 10px;
  }

  &__setting {
    margin-bottom: 12px;
    display: block;
    font-size: 12px;
    color: #409eff;
    text-align: center;
  }

  &__desc {
    text-align: center;
    width: 200px;
    margin: 0 auto;
  }

  &__detail-desc {
    margin-top: 50px;
    font-size: 14px;

    p {
      margin-bottom: 10px;
    }

    span {
      margin-left: 10px;
    }
  }

  &__divider {
    border-top: 1px dashed #e8e8e8;
    display: block;
    height: 0;
    width: 100%;
    margin: 24px 0;
    clear: both;
  }

  &__tags {
    .el-tag {
      margin: 0 5px 5px 0;
    }
  }
}
</style>

