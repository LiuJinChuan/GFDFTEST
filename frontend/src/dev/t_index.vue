<template>
  <div>
    <!--<avue-form ref="sss" :option="ss"></avue-form>-->
    <FilterTree ref="atree" :TreeData="TreeData"></FilterTree>
  </div>
</template>
<script>
import FilterTree from "@/framework/components/Tree/FilterTree";
export default {
  name: "",
  components: { FilterTree },
  data: () => {
    return {
      TreeData: {
        filter: {
          ischildren: false,
          showchildren: true,
          num: 3,
        },
        operation: false, //是否需要配置项
        key: "id", //树选择的值
        showCheckbox: false, //是否需要选择框
        optionType: true, //是否需要远端配置
        strictly: false, //是否父子级联
        defaultProps: {
          children: "children",
          label: "cname",
        }, //树绑定的子级
        ajax: {
          Api: window.GFAPI.biz.DeptApi,

          Api1: window.GFAPI.sys.UserApi,
        },
      },
    };
  },
  computed: {},
  watch: {},
  created() {},
  mounted() {
    let arr = [];
    for (const key in this.$refs.sss) {
      arr.push(key);
    }
  },
  methods: {},
};
</script>
<style lang='' scoped>
</style>
