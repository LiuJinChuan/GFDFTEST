<template>
  <div id="avue-view" v-loading="loading">
    <div v-if="!show">ss</div>
    <div class="crudbox">
      <avue-form
        ref="forms"
        v-if="show"
        :option="options"
        v-model="forms"
        @submit="handleSubmit"
      ></avue-form>
    </div>
  </div>
</template>
<script>
export default {
  name: "csetting",
  props: {
    form: {
      type: Object,
      required: true,
      default: () => {
        return {};
      },
    },
    option: {
      type: Object,
      required: true,
      default: () => {
        return {};
      },
    },
  },
  data() {
    return {
      loading: false,
      data: {},
      print_template: {
        detail: {},
        steps: [],
      },
      printObj: {
        id: "printList",
        popTitle: "good print",
        standard: "html5",
      },
      options: {},
      forms: {},
    };
  },
  computed: {
    show() {
      let boo = !false;
      boo =
        JSON.stringify(this.forms) != "{}" &&
        JSON.stringify(this.options) != "{}";
      return boo;
    },
  },
  watch: {},
  created() {
    this.forms = JSON.parse(JSON.stringify(this.form));
    this.options = JSON.parse(JSON.stringify(this.option));
  },
  mounted() {},

  methods: {
    handleSubmit(form, loading) {
      this.$emit("submit", { form, loading });
      // loading()
    },
  },
};
</script>
<style lang="scss" scoped>
.avue-form__menu--center {
  text-align: right !important;
}
.el-main {
  padding: 5px !important;
}
.crudbox {
  padding: 10px;
  box-sizing: border-box;
  height: 100%;
  /* box-shadow: 1px 1px 10px rgba(0, 0, 0, 0.2); */
}
.avue-crud {
  margin: 0 auto;
  width: 99%;
}

#avue-view {
  /* padding: 10px 10px 40px 10px; */
  -webkit-box-sizing: border-box;
  box-sizing: border-box;
  height: 100%;
  overflow: hidden auto;
  background-color: white;
}
/deep/.el-tree-node__content {
  text-align: left;
}
</style>
