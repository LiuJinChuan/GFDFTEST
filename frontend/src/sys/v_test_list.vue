<template>
  <div>
    <div v-for="item in arr" :key="item">
      <el-image style="width: 300px; height: 400px" :src="item.src"></el-image>
    </div>
  </div>
</template>

<script>
export default {
  data() {
    return {
      arr: [
        {
          src: "https://cube.elemecdn.com/6/94/4d3ea53c084bad6931a56d5158a48jpeg.jpeg",
          isvip: 0,
          isdefa: 0,
        },
        {
          src: "https://cube.elemecdn.com/6/94/4d3ea53c084bad6931a56d5158a48jpeg.jpeg",
          isvip: 0,
          isdefa: 0,
        },
      ],
    };
  },
};
</script>

<style lang="scss" scoped>
</style>