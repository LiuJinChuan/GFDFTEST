<template>
  <Layouts :Layouts="{ type: 'detail' }">
    <span slot="detail">
      <detaile :key="key"></detaile>
    </span>
  </Layouts>
</template>
<script>
import detaile from "./c_page_detali";
import Layouts from "@/framework/components/layouts";
export default {
  name: "paged",
  components: { Layouts, detaile },
  data: () => {
    return { key: "" };
  },
  computed: {},
  watch: {
    "$route.params.id"(val) {
      if (val && this.key != val) {
        this.key = val;
      }
    },
  },
  created() {},
  mounted() {},
  methods: {},
};
</script>
<style lang='' scoped>
</style>