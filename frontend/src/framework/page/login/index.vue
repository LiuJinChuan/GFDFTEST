<template>
  <div class="login-container"
       @keyup.enter.native="handleLogin">
    <top-color v-show="false"></top-color>
    <div class="login-weaper animated bounceInDown">
      <div class="login-left">
        <div class="login-time">
          {{time}}<span style="font-size:16px">&nbsp;&nbsp;星期{{ weeks[new Date().getDay()] }}</span>
        </div>
          <!--<img class="img"-->
               <!--src="https://avuejs.com/images/logo.png"-->
               <!--alt="">-->
        <p class="title">{{ $t('login.info') }}</p>
      </div>
      <div class="login-border">
        <div class="login-main">
          <h4 class="login-title">
            {{ $t('login.title') }}{{website.title}}
            <!-- <top-lang></top-lang> -->
          </h4>
          <userLogin v-if="activeName==='user'"></userLogin>
          <codeLogin v-else-if="activeName==='code'"></codeLogin>
          <thirdLogin v-else-if="activeName==='third'"></thirdLogin>
          <faceLogin v-else-if="activeName==='face'"></faceLogin>
          <!-- <div class="login-menu">
            <a href="#"
               @click.stop="activeName='user'">{{ $t('login.userLogin') }}</a>
            <a href="#"
               @click.stop="activeName='code'">{{ $t('login.phoneLogin') }}</a>
            <a href="#"
               @click.stop="activeName='face'">{{ $t('login.faceLogin') }}</a>
            <a href="#"
               @click.stop="activeName='third'">{{ $t('login.thirdLogin') }}</a>
          </div> -->
        </div>

      </div>
    </div>
  </div>
</template>
<script>
import userLogin from "./userlogin";
import codeLogin from "./codelogin";
import thirdLogin from "./thirdlogin";
import faceLogin from "./facelogin";
import { mapGetters } from "vuex";
import { dateFormat } from "@/framework/util/date";
import { validatenull } from "@/framework/util/validate";
// import topLang from "@/framework/page/index/top/top-lang";
import topColor from "@/framework/page/index/top/top-color";
export default {
  name: "login",
  components: {
    userLogin,
    codeLogin,
    thirdLogin,
    faceLogin,
    // topLang,
    topColor
  },
  data () {
    return {
      time: "",
      activeName: "user",
      weeks: {
        1: '一',
        2: '二',
        3: '三',
        4: '四',
        5: '五',
        6: '六',
        0: '日',
      },

    };
  },
  watch: {
    $route () {
      const params = this.$route.query;
      this.socialForm.state = params.state;
      this.socialForm.code = params.code;
      if (!validatenull(this.socialForm.state)) {
        const loading = this.$loading({
          lock: true,
          text: `${
            this.socialForm.state === "WX" ? "微信" : "QQ"
            }登录中,请稍后。。。`,
          spinner: "el-icon-loading"
        });
        setTimeout(() => {
          loading.close();
        }, 2000);
      }
    }
  },
  created () {
    //进入前就触发gf-logout清除session
    window.GF.emitter.emit('gf-logout')

    this.getTime();
    setInterval(() => {
      this.getTime();
    }, 1000);
  },
  mounted () {
      },
  computed: {
    ...mapGetters(["website"])
  },
  props: [],
  methods: {
    getTime () {
      this.time = dateFormat(new Date());
    }
  }
};
</script>

<style lang="scss">
@import "@/framework/styles/login.scss";
</style>
