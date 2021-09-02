<template>
  <div id="avue-view" ref="table">
    <el-container>
      <el-main>
        <div class="crudbox">
          <avue-crud
            v-if="option.column"
            ref="scrud"
            :option="option"
            :page.sync="page"
            :before-open="beforeopen"
            :data="TableData.list || list"
            :table-loading="loading"
            v-model="tempRow"
            :upload-after="uploadAfter"
            :row-style="rowStyle"
            @cell-mouse-enter="cellMouseHover"
            @refresh-change="getList"
            @row-click="RowClick"
            @row-save="handleCreate"
            @row-del="handleDelete"
            @row-update="handleUpdate"
            @search-change="searchChange"
            @search-reset="searchReset"
            @size-change="sizeChange"
            @current-change="currentChange"
            @selection-change="selectionChange"
          >
            <!--↓↓↓ 表格展开行处理 ↓↓↓-->
            <template slot="expand" slot-scope="{ row }">
              <slot name="expand" :scope="{ row }"></slot>
            </template>
            <!--↑↑↑ 表格展开行处理 ↑↑↑-->

            <!--↓↓↓ 表格显示组件的type处理 ↓↓↓-->
            <template
              v-for="(it, index) in columnSlot"
              :slot="it.prop"
              slot-scope="{ row, type, size }"
            >
              <div :key="index" v-if="it.slotType === 'head'">
                <div class="headBox">
                  <img v-if="row[it.prop]" :src="row[it.prop]" alt srcset />
                </div>
              </div>
              <div :key="index" v-if="it.slotType === 'tag'">
                <div style="text-align: center">
                  <el-tag :type="tagComputed(row[it.prop], it.prop).type">{{
                    tagComputed(row[it.prop], it.prop).label
                  }}</el-tag>
                </div>
              </div>
              <div :key="index" v-if="it.slotType === 'imgs'">
                <div class="imgBox">
                  <div
                    @click="openImgViews(row[it.prop], index)"
                    class="imgBox-item"
                    :style="{
                      backgroundImage: 'url(' + d + ')',
                    }"
                    v-for="(d, index) of row[it.prop].split(',').splice(0, 3)"
                    :key="index"
                  ></div>
                </div>
              </div>
              <slot
                v-else
                :name="it.prop"
                :scope="{
                  row,
                  type,
                  size,
                  prop: it.prop,
                  slotType: it.slotType,
                }"
              ></slot>
            </template>
            <!--↑↑↑ 表格显示组件的type处理 ↑↑↑-->

            <!--↓↓↓ 表单显示组件的type处理 ↓↓↓-->
            <template
              v-for="(it, indexc) in columnFormSlot"
              :slot="it.prop + 'Form'"
              slot-scope="{ row, type, size }"
            >
              <div :key="indexc" v-if="it.formslotType === 'json'">
                <vue-json-editor v-model="row[it.prop]" :expandedOnStart="true">
                </vue-json-editor>
              </div>
              <div :key="indexc" v-if="it.formslotType === 'head'">
                <div class="headUploadBox">
                  <div class="formHeadBox headBox">
                    <img :src="row[it.prop]" />
                  </div>
                  <span @click="setProp(it.prop)">
                    <el-upload
                      :ref="`upload${it.prop}`"
                      class="upload-demo"
                      :action="imgurl"
                      :headers="tokenHeaders"
                      :show-file-list="false"
                      name="files"
                      :on-success="uploadSuccess"
                      :limit="1"
                    >
                      <el-button size="small" type="primary"
                        >点击上传</el-button
                      >
                    </el-upload>
                  </span>
                </div>
              </div>
              <div :key="indexc" v-if="it.formslotType === 'file'">
                <div class="fileUploadBox">
                  <div class="fileBox">
                    <el-input v-model="row[it.prop]" readonly></el-input>
                  </div>
                  <span @click="setProp(it.prop)">
                    <el-upload
                      :ref="`upload${it.prop}`"
                      class="upload-demo"
                      :action="fileurl"
                      :accept="it.accept"
                      :headers="tokenHeaders"
                      :show-file-list="false"
                      name="files"
                      :on-success="uploadFileSuccess"
                      :limit="1"
                    >
                      <el-button size="small" type="primary"
                        >点击上传</el-button
                      >
                    </el-upload>
                  </span>
                </div>
              </div>
              <div :key="indexc" v-if="it.formslotType === 'img'">
                <div class="headUploadBox">
                  <div class="formHeadBox imgBox">
                    <img v-if="row[it.prop]" :src="row[it.prop]" />
                  </div>
                  <span @click="setProp(it.prop)">
                    <el-upload
                      :ref="`upload${it.prop}`"
                      class="upload-demo"
                      :action="imgurl"
                      :headers="tokenHeaders"
                      :show-file-list="false"
                      name="files"
                      :on-success="uploadSuccess"
                      :limit="1"
                    >
                      <el-button size="small" type="primary"
                        >点击上传</el-button
                      >
                    </el-upload>
                  </span>
                </div>
              </div>
              <div :key="indexc" v-if="it.formslotType === 'imgs'">
                <div class="imgBox">
                  <div
                    v-for="(d, iii) of row[it.prop].split(',').splice(0, 3)"
                    :key="iii"
                    @click="openImgViews(row[it.prop], iii)"
                    class="imgBox-item"
                    :style="{
                      backgroundImage: 'url(' + d + ')',
                    }"
                  ></div>
                </div>
              </div>
              <slot
                v-else
                :name="it.prop + 'Form'"
                :scope="{ row, type, size }"
              >
              </slot>
            </template>
            <!--↑↑↑ 表单显示组件的type处理 ↑↑↑-->

            <!--↓↓↓ search 组件的type处理↓↓↓-->
            <template
              v-for="it in columnSearchSlot"
              :slot="it.prop + 'Search'"
              slot-scope="{ disabled, size, row }"
            >
              <inputNum
                :key="it"
                v-if="it.searchSlotType == 'inputRanges'"
                :disabled="disabled"
                :size="size"
                v-model="row[it.prop]"
              ></inputNum>

              <slot
                v-else
                :name="it.prop"
                :scope="{
                  row,
                  type,
                  size,
                  prop: it.prop,
                  slotType: it.slotType,
                }"
              ></slot>
            </template>
            <!--↑↑↑ search 组件的type处理 ↑↑↑-->

            <!--↓↓↓ menu相关显示的处理 ↓↓↓-->
            <template slot="menu" slot-scope="{ row, type, size }">
              <slot
                name="menu"
                :scope="{
                  row,
                  type,
                  size,
                  btnArr: btnArr.filter((e) => !e.extobj.btnposition),
                }"
              ></slot>
            </template>
            <template slot="menuRight" slot-scope="{}">
              <slot
                name="menuRight"
                :scope="{
                  btnArr: btnArr.filter((e) => e.extobj.btnposition == 2),
                }"
              ></slot>
            </template>
            <template slot="menuLeft" slot-scope="{}">
              <div class="divs">
                <div
                  class="divss"
                  v-for="(item, index) in TableData.menuLeft"
                  :key="index"
                >
                  <el-button
                    v-if="!item.upload"
                    :key="index"
                    :type="item.type"
                    :size="item.size"
                    @click="menuLeftClick(item.method)"
                    >{{ item.title }}</el-button
                  >
                  <el-upload
                    ref="btnload"
                    v-if="item.upload"
                    :show-file-list="false"
                    accept=".csv,.xls,.xlsx"
                    :headers="tokenHeaders"
                    name="files"
                    class="upload-demo"
                    :action="item.url"
                    :limit="1"
                    :on-success="upsuccess"
                  >
                    <el-button size="small" type="primary">{{
                      item.title
                    }}</el-button>
                  </el-upload>
                </div>
              </div>

              <slot
                name="menuLeft"
                :scope="{
                  btnArr: btnArr.filter((e) => e.extobj.btnposition == 1),
                }"
              ></slot>
            </template>
            <!--↑↑↑ menu相关显示的处理 ↑↑↑-->
            <!--↓↓↓ menu相关显示的处理 ↓↓↓-->
            <template slot-scope="{ scope }" slot="menuForm">
              <slot name="menuForm" :scope="{ scope }"></slot>
            </template>
            <!--↑↑↑ menu相关显示的处理 ↑↑↑-->
          </avue-crud>
        </div>
      </el-main>
    </el-container>
  </div>
</template>
 
<script>
import inputNum from "./inputNumSearch";
import vueJsonEditor from "vue-json-editor";
// eslint-disable-next-line no-unused-vars
function moneyLint(rule, value, callback) {
  var patt = /^[0-9][0-9]*([.][0-9]{1,2})?$/;
  if (patt.test(value)) {
    callback();
  } else {
    callback(new Error("格式不正确"));
  }
}
function setParams(x, y, z, z2, type) {
  let obj = {};
  if (type == "rangType" || type == "ranges ") {
    if (z2[y][0] == "" || z2[y][1] == "") {
      obj[y] = "";
    } else {
      obj[y] = z2[y].join(",");
    }
  } else {
    obj[y] = z2[y];
  }
  if (!z[x]) {
    z[x] = [obj];
  } else {
    z[x].push(obj);
  }
}
import { mixins } from "../../mixins/setOptions";
import config from "@/framework/config";
export default {
  name: "",
  components: { inputNum, vueJsonEditor },
  props: {
    TableData: {
      type: Object,
      require: true,
    },
    btnState: {
      type: Object,
      default: () => {
        return {};
      },
    },
  },
  mixins: [mixins],
  data() {
    return {
      ishasfwb: false,
      initList: true,
      imgurl: config.baseURL + "common/uploadimg",
      fileurl: config.baseURL + "common/upload",
      changFormObj: {},
      loading: false,
      list: [],
      tempRow: {},
      btnArr: [],
      nopermiBtn: [],
      page: {
        currentPage: 1,
        pageSize: 10,
        total: 0,
        pageSizes: [10, 15, 20, 50, 100, 500],
        pagerCount: 11,
      },
      PrepareSearchForm: {},
      columnSlot: [],
      columnFormSlot: [],
      columnSearchSlot: [],
      option: {},
      tokenHeaders: {
        Authorization: window.GF.context.get("token"),
      },
      selfOnlySearch: {},
      searchOfContainer: {},
      tempNode: {},
      dicUpdata: [],
      initFrormData: {},
      headProp: "",
      row: {},
      updataDicTimer: null,
      getListTimer: null,
      select: [],
    };
  },
  created() {},
  watch: {},
  computed: {
    vmDom() {
      return this;
    },
    objParams() {
      let obj = {
        page: this.page.currentPage,
        limit: this.TableData.noPage ? 0 : this.page.pageSize,
        order: [{ id: "desc" }], //降序desc 升序asc,
        other: [],
      };

      if (this.TableData.resetOrder) {
        obj = Object.assign(obj, this.TableData.OrderObj);
      }

      return obj;
    },
    multipleKey() {
      let temp = this.option.column.filter((e) => {
        return (e.type === "select" || e.type === "tree") && e.multiple;
      });
      return temp.map((e) => e.prop);
    },
    dateTypeKey() {
      let temp = this.option.column.filter(
        (e) => e.type === "datetime" || e.type == "daterange"
      );
      return temp.map((e) => e.prop);
    },
    searchKeys() {
      let temp = {};
      if (this.option.column) {
        this.option.column.map((e) => {
          if (e.type === "datetime" && e.search) {
            temp[e.prop] = "ranges";
          }
          if (e.type === "input" && e.search) {
            temp[e.prop] = "like";
          }
          if (e.type === "select" && e.search) {
            temp[e.prop] = "other";
          }
          if (e.type === "number" && e.search) {
            temp[e.prop] = "ranges";
          }
          if (e.type === "daterange" && e.search) {
            temp[e.prop] = "ranges";
          }
        });
      }
      return temp;
    },
    searchTemp() {
      let SOC = JSON.parse(JSON.stringify(this.searchOfContainer));
      let SOG = JSON.parse(JSON.stringify(this.selfOnlySearch));
      let SOP = JSON.parse(JSON.stringify(this.permanentSearch));
      for (const key in SOC) {
        if (SOG.hasOwnProperty(key)) {
          SOG[key] = SOG[key].concat(SOC[key]);
        } else {
          SOG[key] = SOC[key];
        }
      }
      for (const key in SOP) {
        if (SOG.hasOwnProperty(key)) {
          SOG[key] = SOG[key].concat(SOP[key]);
        } else {
          SOG[key] = SOP[key];
        }
      }

      return SOG;
    },
  },
  mounted() {
    if (this.TableData.initPageSize)
      this.page.pageSize = this.TableData.initPageSize;
  },
  methods: {
    selectionChange(list) {
      this.select = list;
      this.$emit("SelectList", list);
    },
    rowStyle({ row }) {
      this.$emit("RowStyle", row);
    },

    menuLeftClick(method) {
      this.$emit("menuLeftClick", { vm: this, method });
    },
    cellMouseHover(row) {
      this.row = row;
    },
    upsuccess(response, file, fileList) {
      this.$refs.btnload[0].clearFiles();
      this.$emit("Upload", response);
      console.log(file, fileList);
    },
    getList() {
      if (this.TableData.list) {
        this.list = this.TableData.list;
      } else {
        this.loading = true;
        let params = JSON.parse(JSON.stringify(this.objParams));
        //处理搜索内容
        let searchParams = JSON.parse(JSON.stringify(this.searchKeys)); //包含键名对应的搜索类型
        let searchDatainit = JSON.parse(JSON.stringify(this.PrepareSearchForm)); //搜索键值对(键名可能为逻辑字段)

        //针对逻辑值更改键名
        if (
          this.TableData.hasOwnProperty("changFormObj") &&
          this.TableData.changFormObj.isFormChange
        ) {
          this.TableData.changFormObj.arr.forEach((el) => {
            if (searchDatainit.hasOwnProperty(el.target)) {
              switch (el.value) {
                case "Number":
                  searchDatainit[el.name] = Number(searchDatainit[el.target]);
                  this.$delete(searchDatainit, el.target);
                  break;
                default:
                  break;
              }
            }
            //同时更改 替换搜索类型的键名
            if (searchParams.hasOwnProperty(el.target)) {
              searchParams[el.name] = searchParams[el.target];
              this.$delete(searchParams, el.target);
            }
          });
        }
        //准备搜索对象 searchData是搜索类型与搜索值的键值对
        let searchData = {};
        for (const key in searchDatainit) {
          if (searchParams.hasOwnProperty(key)) {
            let type = searchParams[key];
            switch (type) {
              case "ranges":
                setParams(type, key, searchData, searchDatainit, "rangType");
                break;
              case "like":
                setParams(type, key, searchData, searchDatainit);
                break;
              default:
                setParams(type, key, searchData, searchDatainit);
                break;
            }
          }
        }
        for (const key in searchData) {
          if (params.hasOwnProperty(key)) {
            params[key] = params[key].concat(searchData[key]);
          } else {
            params[key] = searchData[key];
          }
        }
        for (const key in this.searchTemp) {
          if (params.hasOwnProperty(key)) {
            params[key] = params[key].concat(this.searchTemp[key]);
          } else {
            params[key] = this.searchTemp[key];
          }
        }

        if (this.TableData.hasOwnProperty("init") && !this.TableData.init) {
          this.loading = false;
          return;
        }
        let loads = true;
        //阻止首次自动加载
        if (
          this.TableData.hasOwnProperty("autoload") &&
          !this.TableData.autoload
        ) {
          loads = false;
          this.loading = false;
        } else {
          loads = true;
        }

        if (loads) {
          this.TableData.ajax.Api.search(params)
            .then((res) => {
              if (res.code == 0) {
                this.multipleKey.forEach((e) => {
                  res.data.forEach((el) => {
                    el[e] = el[e].split(",");
                  });
                });
                this.dateTypeKey.forEach((e) => {
                  res.data.forEach((el) => {
                    el[e] = el[e] === 0 ? null : Number(el[e]);
                  });
                });
                let fun = this.TableData.formatterListData;

                if (typeof fun === "function") {
                  this.list = fun(res.data);
                } else {
                  this.list = res.data;
                }

                this.page.total = res.count == 0 ? 0 : res.count;
                this.page.total = this.TableData.noPage ? 0 : this.page.total;
              } else {
                this.list = [];
                this.page.total = 0;
              }
              this.loading = false;
              this.$emit("SearchChange");
            })
            .catch(() => {
              this.list = [];
              this.page.total = 0;
              this.loading = false;
              this.$emit("SearchChange");
            });
        }
      }
    },
    handleCreate(row, done, loading) {
      this.multipleKey.forEach((e) => {
        row[e] = row[e].join(",");
      });
      if (this.changFormObj.hasOwnProperty("isFormChange")) {
        this.changFormObj.arr.forEach((el) => {
          switch (el.value) {
            case "Number":
              row[el.name] = Number(row[el.target]);
              break;
            case "String":
              row[el.name] = String(row[el.target]);
              break;
            default:
              break;
          }
        });
      }
      let tempInit = JSON.parse(JSON.stringify(this.initFrormData));
      let tempInit2 = JSON.parse(
        JSON.stringify(this.TableData.permanentAddFrormData || {})
      );
      for (const key in tempInit) {
        row[key] = tempInit[key];
      }
      for (const key in tempInit2) {
        row[key] = tempInit2[key];
      }
      let tempRow = row;
      let fun = this.TableData.formatterFormData;
      if (typeof fun === "function") {
        tempRow = fun(row, "add");

        if (tempRow.hasOwnProperty("error")) {
          this.$message({
            message: tempRow.errormsg,
            type: "info",
            center: true,
          });
          loading();
          return;
        }
      }

      this.TableData.ajax.Api.add(tempRow)
        .then((res) => {
          if (res.code == 0) {
            loading();
            done();
            this.getList();
            this.$message({
              message: "新增成功",
              type: "success",
              center: true,
            });
            this.$emit("addSuccess");
          }
        })
        .catch(() => {
          loading();
          //  done();
        });
    },
    handleDelete(row) {
      var that = this;
      this.$confirm("请确认是否删除当前项, 再继续?", "提示", {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        type: "warning",
      })
        .then(() => {
          this.TableData.ajax.Api.del(row.id)
            .then((res) => {
              if (res.code == 0) {
                that.getList();
                that.$message({
                  message: "删除成功",
                  type: "success",
                  center: true,
                });
                this.$emit("delSuccess");
              }
            })
            .catch(() => {});
        })
        .catch(() => {});
    },
    handleUpdate(rows, index, done, loading) {
      var that = this;
      let row = JSON.parse(JSON.stringify(rows));
      for (const key in row) {
        const element = row[key] == null;
        element ? delete row[key] : null;
      }
      that.multipleKey.forEach((e) => {
        row[e] = row[e].join(",");
      });

      if (that.changFormObj.hasOwnProperty("isFormChange")) {
        that.changFormObj.arr.forEach((el) => {
          switch (el.value) {
            case "Number":
              row[el.name] = Number(row[el.target]);
              break;
            case "String":
              row[el.name] = String(row[el.target]);
              break;
            default:
              break;
          }
        });
      }
      let tempRow = row;
      let fun = this.TableData.formatterFormData;
      if (typeof fun === "function") {
        tempRow = fun(row, "edit");
      }

      this.TableData.ajax.Api.edit(tempRow)
        .then((res) => {
          loading();
          if (res.code == 0) {
            done();
            that.getList();
            that.$message({
              message: "编辑成功",
              type: "success",
              center: true,
            });
            this.$emit("editSuccess");
          }
        })
        .catch(() => {
          done();
        });
    },
    searchChange(form, done) {
      done();
      this.PrepareSearchForm = form;
      this.getList();
    },
    searchReset() {
      this.PrepareSearchForm = {};
    },
    // 标签数据格式化
    tagComputed(data, prop) {
      let dictData = this.option.column.find((e) => e.prop === prop).dicData;
      if (!dictData || dictData.length === 0) {
        return { label: `无${prop}字典` };
      } else {
        let obj = dictData.find((e) => e.value == data || e.cvalue == data);
        return obj || { label: `无${prop}字典` };
      }
    },
    //图片展示
    openImgViews(item, index) {
      let Arr = item.split(",").map((e) => {
        return { url: e };
      });
      this.$ImagePreview(Arr, index);
    },
    //头像上传
    uploadSuccess(res, file, fileList) {
      if (res.code === 0) {
        this.$message({
          message: "上传成功",
          type: "success",
          center: true,
        });
        if (this.headProp) {
          this.tempRow[this.headProp] = res.data;
        }
        fileList.pop();
      } else {
        this.$message({
          message: res.Message,
          type: "warning",
          center: true,
        });
      }
    },
    //文件上传
    uploadFileSuccess(res, file, fileList) {
      if (res.code === 0) {
        this.$message({
          message: "上传成功",
          type: "success",
          center: true,
        });
        if (this.headProp) {
          this.tempRow[this.headProp] = res.data.Url;
        }
        fileList.pop();
      } else {
        this.$message({
          message: res.Message,
          type: "warning",
          center: true,
        });
      }
    },
    setProp(str) {
      this.headProp = str;
    },
    setTreeNode(data) {
      this.tempNode = data;
    },
    resetSearchData(data = {}, notSearch) {
      this.page.currentPage = 1;
      if (this.getListTimer) {
        clearInterval(this.getListTimer);
      }
      this.searchOfContainer = data;
      if (!notSearch) {
        this.getListTimer = setInterval(() => {
          if (this.option.column) {
            this.getList();
            clearInterval(this.getListTimer);
          }
        }, 200);
      }
    },
    updataDic(data) {
      this.updataDicTimer = setInterval(() => {
        if (this.option.column) {
          this.dicUpdata = data;
          data.forEach((item) => {
            this.option.column.forEach((items) => {
              if (item.prop == items.prop) {
                items.props = item.props;
              }
            });
          });
          if (this.option.group) {
            this.option.group.forEach((everyColumn) => {
              data.forEach((item) => {
                everyColumn.column.forEach((items) => {
                  if (item.prop == items.prop) {
                    items.props = item.props;
                  }
                });
              });
            });
          }
          clearInterval(this.updataDicTimer);
        }
      }, 200);
    },
    setInitForm(data) {
      this.initFrormData = data;
    },
    beforeopen(doen, type) {
      doen();
      if (this.TableData.isEdit) {
        if (type == "edit") {
          setTimeout(() => {
            this.TableData.ajax.Api.view(this.row.id).then((res) => {
              this.tempRow = res.data;
            });
          }, 10);
        }
      }
      setTimeout(() => {
        this.dicUpdata.forEach((item) => {
          this.$refs.scrud.$refs.dialogForm.$refs.tableForm.updateDic(
            item.prop,
            item.dicData,
            () => {}
          );
        });
      }, 30);
      if (this.TableData.ishasfwb) {
        this.$emit("TextInit", doen);
      }
    },
    sizeChange(val) {
      this.page.pageSize = val;
      this.getList();
    },
    currentChange(val) {
      this.page.currentPage = val;
      this.getList();
    },
    RowClick(val) {
      this.$emit("selectID", val.id);
    },
    uploadAfter(data, done, loading, column) {
      this.$set(this.tempRow, column.prop, data.Url);
      this.$message({
        message: "上传成功",
        type: "success",
        center: true,
      });
      loading();
      this.$emit("uploadAfter", { data, column });
    },
    setEmptyArray() {
      this.list = [];
      this.page.currentPage = 1;
      this.page.total = 0;
    },
  },
};
</script>
<style >
.divss .el-upload {
  border: none !important;
}
</style>
<style lang="scss" scoped>
.divs {
  display: flex;
  display: -webkit-flex;
}
.avue-form__menu--center {
  text-align: right !important;
}

.el-main {
  padding: 5px !important;
}
.crudbox {
  padding: 10px;
  box-sizing: border-box;
  box-shadow: 1px 1px 10px rgba(0, 0, 0, 0.2);
}
.avue-crud {
  margin: 0 auto;
  width: 99%;
}

#avue-view {
  -webkit-box-sizing: border-box;
  box-sizing: border-box;
  height: 100%;
  overflow: hidden auto;
  background-color: white;
}
.headBox {
  width: 40px;
  height: 40px;
  margin: 0 auto;
  border-radius: 20px;
  overflow: hidden;
  img {
    width: 100%;
    height: 100%;
  }
}
.imgBox {
  display: flex;
  flex-wrap: wrap;
  &-item {
    width: 40px;
    height: 40px;
    margin: 5px;
    overflow: hidden;
    display: inline-block;
    border: 1px solid #6b6b6b38;
    background-position: center;
    background-size: 100%;
    background-repeat: no-repeat;
  }
}
.headUploadBox {
  display: flex;
  justify-content: space-around;
  .formHeadBox {
    margin: 0;
  }
}
.fileUploadBox {
  display: flex;
  justify-content: space-between;
  .fileBox {
    width: 100%;
    padding-right: 20px;
  }
}
.imgBox img {
  width: 80px;
  height: 80px;
}
</style>