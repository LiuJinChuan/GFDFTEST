<template>
  <el-form
    class="login-form"
    status-icon
    :rules="loginRules"
    ref="loginForm"
    :model="loginForm"
    label-width="0"
  >
    <el-form-item prop="account">
      <el-input
        size="small"
        @keyup.enter.native="handleLogin"
        v-model="loginForm.account"
        auto-complete="off"
        :placeholder="$t('login.username')"
      >
        <i slot="prefix" class="icon-yonghu"></i>
      </el-input>
    </el-form-item>
    <el-form-item prop="pwd">
      <el-input
        size="small"
        @keyup.enter.native="handleLogin"
        :type="passwordType"
        v-model="loginForm.pwd"
        auto-complete="off"
        :placeholder="$t('login.password')"
      >
        <i
          class="el-icon-view el-input__icon"
          slot="suffix"
          @click="showPassword"
        ></i>
        <i slot="prefix" class="icon-mima"></i>
      </el-input>
    </el-form-item>
    <el-form-item>
      <el-button
        type="primary"
        size="small"
        @click.native.prevent="handleLogin"
        class="login-submit"
        >{{ $t("login.submit") }}</el-button
      >
    </el-form-item>
  </el-form>
</template>

<script>
import { mapGetters } from "vuex";
// import { buildListToTree } from "../../util/util";
export default {
  name: "userlogin",
  data() {
    return {
      loginForm: {
        account: "",
        pwd: "",
      },
      checked: false,
      code: {
        src: "",
        value: "",
        len: 4,
        type: "text",
      },
      loginRules: {
        account: [{ required: true, message: "请输入用户名", trigger: "blur" }],
        pwd: [
          { required: true, message: "请输入密码", trigger: "blur" },
          { min: 6, message: "密码长度最少为6位", trigger: "blur" },
        ],
      },
      passwordType: "password",
    };
  },
  created() {},
  mounted() {},
  computed: {
    ...mapGetters(["tagWel"]),
  },
  props: [],
  methods: {
    showPassword() {
      this.passwordType == ""
        ? (this.passwordType = "password")
        : (this.passwordType = "");
    },
    handleLogin() {
      this.$refs.loginForm.validate((valid) => {
        if (valid) {
          //TODO:建议后端Login接口返回userinfo
          this.$store
            .dispatch("LoginByUsername", this.loginForm)
            .then((res) => {
              window.GF.context.set("token", res.token);
              window.GFActor.token = res.token;
              window.GF.emitter.emit("gf-logged", res);
              window.GF.store.dispatch("GetUserInfo").then((re) => {
                let Router = window.GF.router.$avueRouter.formatRoutes(
                  re.menus || [],
                  true
                );
                window.GF.router.addRoutes(Router);
                re.user.avatar = re.user.extobj.avatar;
                window.GF.context.set("actor", re);
                this.$router.push({ path: "/wel/index" });
              });
            })
            .catch(() => {});
        }
      });
    },
  },
};
</script>

<style>
</style>
