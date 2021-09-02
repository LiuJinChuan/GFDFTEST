<template>
  <div class="layOutsBox">
    <el-container>
      <el-main>
        <div class="crudbox" v-if="Layouts.type=='hasTree'">
          <div v-if="!isOnlyGrade" class="Left">
            <slot name="Tree"></slot>
          </div>
          <div class="Right">
            <slot name="Main"></slot>
          </div>
        </div>
        <div class="crudbox" v-if="Layouts.type=='Main'">
          <div class="Right">
            <slot name="Main"></slot>
          </div>
        </div>
        <div class="crudbox crudbox_detail" v-if="Layouts.type=='detail'">
          <div class="Right">
            <slot name="detail">
              <div>详情</div>
            </slot>
          </div>
        </div>
      </el-main>
    </el-container>
  </div>
</template>

<script>
/* eslint-disable no-undef*/
export default {
  name: 'layouts',
  props: ['Layouts'],
  data() {
    return {
      isOnlyGrade: false
    };
  },
  components: {},
  created() {
    this.isOnlyGrade = GF.context.get('onlyGrade').isTrue;
  },
  mounted() { },
  methods: {}
};
</script>

<style scoped lang="scss">
.avue-form__menu--center {
  text-align: right !important;
}

.layOutsBox {
  height: 100%;
  box-sizing: border-box;
  .el-container {
    height: 100%;
    box-sizing: border-box;
    .crudbox {
      height: 100%;
      padding: 5px;
      padding-bottom: 25px;
      box-sizing: border-box;
      box-shadow: 1px 1px 10px rgba(0, 0, 0, 0.2);
      display: flex;
      display: -ms-flex;
      display: -webkit-flex;
      justify-content: center;
      background-color: #ffffff;
      .Left {
        height: 100%;
        width: 250px;
        border-right: 1px solid #eeeeee;
        padding-right: 5px;
        margin-right: 5px;
        overflow: hidden;
        box-sizing: border-box;
        /deep/.Tree {
          height: 100%;
          padding: 5px;
          box-sizing: border-box;
          border: 1px solid #eeeeee;
          border-radius: 5px;
          box-shadow: 1px 1px 10px rgba(0, 0, 0, 0.2);
          .el-tree {
            height: calc(100% - 47px);
            overflow-y: auto;
          }
        }
      }
      .Right {
        height: 100%;
        flex: 1;
        overflow: hidden;
        overflow-y: auto;
      }
    }
  }
}

.avue-crud {
  margin: 0 auto;
  width: 99%;
}
.crudbox_detail .Right {
  box-sizing: border-box;
  border: 1px solid #eeeeee;
  border-radius: 5px;
  padding: 0px 10px;
}
</style>
