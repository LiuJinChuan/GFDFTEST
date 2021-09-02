import setOption from "@/framework/util/setOption";

export const mixins = {
  data() {
    return {};
  },
  created() {
    let eventProp = ["Change", "Click", "Focus", "Blur"]; //写在容器内的事件
    let selfProp = ["change", "click", "focus", "blur"]; //页面配置内的事件
    this.permanentSearch = this.TableData.permanentSearch || {};
    this.$store.getters.sysDictsAsync.then(() => {
      for (const keys in this.TableData) {
        this[keys] = this.TableData[keys];
      }
      if (!this.TableData.optionType) {
        if (!this.TableData.staticlist) {
          this.option = this.TableData.staticOption;
          this.option.searchMenuPosition = "right";
          this.columnSlot = this.option.column.filter((o) => o.slot);
          this.columnFormSlot = this.option.column.filter((o) => o.formslot);
          this.columnSearchSlot = this.option.column.filter(
            (o) => o.searchslot
          );
          if (this.TableData.initList) {
            this.getList();
          }
          return;
        }
        if (this.TableData.ajax && this.TableData.ajax.data) {
          window.GFAPI.sys.PageApi.view(this.TableData.ajax.data.callcode).then(
            (res) => {
              window.GFAPI.sys.ButtonApi.search({
                order: [{ id: "desc" }],
                other: [{ page: res.data.id }],
              }).then((btn) => {
                let btnRole = window.GF.context
                  .get("actor")
                  .btns.map((e) => e.id);
                this.btnArr = btn.data
                  .filter((e) => btnRole.includes(e.id))
                  .sort((a, b) => a.seqno - b.seqno);
                this.nopermiBtn = btn.data.filter(
                  (e) => !btnRole.includes(e.id)
                );
                let Table = this.TableData.staticOption.column;
                Table.forEach((e) => {
                  e.display =
                    e.display === undefined ? true : Boolean(e.display);
                });
                setOption(Table, this.TableData.localDict, this);
                if (res.data.extobj.table.group) {
                  res.data.extobj.table.group.map((e) => {
                    setOption(e.column, this.TableData.localDict, this);
                  });
                }
                let btnCcode = this.nopermiBtn.map((e) => e.callcode);
                if (btnCcode.includes("add")) {
                  this.TableData.staticOption.addBtn = false;
                }
                if (btnCcode.includes("edit")) {
                  this.TableData.staticOption.editBtn = false;
                }
                if (btnCcode.includes("del")) {
                  this.TableData.staticOption.delBtn = false;
                }
                this.option = this.TableData.staticOption;
                this.option.searchMenuPosition = "right";
                this.columnSlot = this.option.column.filter((o) => o.slot);
                this.columnFormSlot = this.option.column.filter(
                  (o) => o.formslot
                );
                this.columnSearchSlot = this.option.column.filter(
                  (o) => o.searchslot
                );
                if (window.GF.context.get("onlyGrade").isTrue) {
                  this.selfOnlySearch =
                    this.TableData.selfGradeInitSearch || {};
                  this.getList();
                }
                if (this.TableData.initList) {
                  this.getList();
                }
              });
            }
          );
        } else {
          let Table = this.TableData.staticOption.column;
          Table.forEach((e) => {
            e.display = e.display === undefined ? true : Boolean(e.display);
          });
          setOption(Table, this.TableData.localDict, this);
          this.option = this.TableData.staticOption;
          this.option.searchMenuPosition = "right";
          this.columnSlot = this.option.column.filter((o) => o.slot);
          this.columnFormSlot = this.option.column.filter((o) => o.formslot);
          this.columnSearchSlot = this.option.column.filter(
            (o) => o.searchslot
          );
          if (window.GF.context.get("onlyGrade").isTrue) {
            this.selfOnlySearch = this.TableData.selfGradeInitSearch || {};
            this.getList();
          }
          if (this.TableData.initList) {
            this.getList();
          }
        }
      } else {
        let tableOptionData = this.TableData.ajax;
        window.GFAPI.sys.PageApi.view(this.TableData.ajax.data.callcode).then(
          (res) => {
            window.GFAPI.sys.ButtonApi.search({
              order: [{ id: "desc" }],
              other: [{ page: res.data.id }],
            }).then((btn) => {
              let btnRole = window.GF.context
                .get("actor")
                .btns.map((e) => e.id);
              this.btnArr = btn.data
                .filter((e) => btnRole.includes(e.id))
                .sort((a, b) => a.seqno - b.seqno);
              this.nopermiBtn = btn.data.filter((e) => !btnRole.includes(e.id));
              let Table = res.data.extobj.table.column;
              Table.forEach((e) => {
                e.display = e.display === undefined ? true : Boolean(e.display);
              });
              setOption(Table, this.TableData.localDict, this);
              if (res.data.extobj.table.group) {
                res.data.extobj.table.group.map((e) => {
                  setOption(e.column, this.TableData.localDict, this);
                });
              }
              let btnCcode = this.nopermiBtn.map((e) => e.callcode);
              if (btnCcode.includes("add")) {
                res.data.extobj.table.addBtn = false;
              }
              if (btnCcode.includes("edit")) {
                res.data.extobj.table.editBtn = false;
              }
              if (btnCcode.includes("del")) {
                res.data.extobj.table.delBtn = false;
              }
              this.option = res.data.extobj.table;
              this.option.searchMenuPosition = "right";
              this.columnSlot = this.option.column.filter((o) => o.slot);
              this.columnFormSlot = this.option.column.filter(
                (o) => o.formslot
              );
              this.columnSearchSlot = this.option.column.filter(
                (o) => o.searchslot
              );
              this.$nextTick(() => {
                this.option.column.forEach((el) => {
                  selfProp.forEach((els) => {
                    if (el[els]) {
                      el[els] = eval(el[els]);
                    }
                  });
                  if (el.dicDataType) {
                    if ("function" == typeof this.$refs.scrud.updateDic) {
                      this.$refs.scrud.updateDic(el.prop, el.dicData);
                    }
                  }
                  if (tableOptionData.methods) {
                    eventProp.forEach((evnt) => {
                      if (
                        "function" ===
                        typeof tableOptionData.methods[el.prop + evnt]
                      ) {
                        el[evnt.toLowerCase()] = tableOptionData.methods[
                          el.prop + evnt
                        ].call(this);
                      }
                    });
                  }
                });
              });
              if (window.GF.context.get("onlyGrade").isTrue) {
                this.selfOnlySearch = this.TableData.selfGradeInitSearch || {};
                this.getList();
              }
              if (this.TableData.initList) {
                this.getList();
              }
            });
          }
        );
      }
    });
  },
  methods: {},
};
