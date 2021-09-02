<template>
  <div class="flexBox">
    <el-input
      class="rangInput"
      placeholder="请输入最小值"
      :size="size"
      :disabled="disabled"
      v-model="value[0]"
      v-on:input="change($event,0)"
      v-on:blur="blur($event,0)"
    ></el-input>
    <span class="centerText">至</span>
    <el-input
      class="rangInput"
      placeholder="请输入最大值"
      :size="size"
      :disabled="disabled"
      v-model="value[1]"
      v-on:input="change($event,1)"
      v-on:blur="blur($event,1)"
    ></el-input>
  </div>
</template>
<script>
export default {
  name: '',
  props: {
    size: {},
    disabled: {},
    value: {
      type: Array,
      default: () => []
    }
  },
  components: {},
  data: () => {
    return {};
  },
  computed: {},
  watch: {},
  created() {},
  mounted() {},
  methods: {
    change(e, index) {
      this.value[index] = e.replace(/[^\d.]/g, '');
      this.$emit('input', this.value);
    },
    blur(e, index) {
      if (!this.value) {
        this.value = [];
      }
      if (this.value[0] != '' && this.value[1] != '') {
        if (this.value[0] - this.value[1] >= 0) {
          this.$set(this.value, index, '');
          this.$emit('input', this.value);
          this.$message({
            message: '输入金额有误',
            type: 'error'
          });
        }
      }
    }
  }
};
</script>
<style lang='scss' scoped>
.flexBox {
  display: flex;
  display: -ms-flexbox;
  display: -webkit-flex;
  justify-content: space-between;
  .centerText {
    font-size: 13px;
    color: #303133;
  }
}
.rangInput{
  width: 45%;
}
</style>