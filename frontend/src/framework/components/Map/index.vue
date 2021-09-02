/* eslint-disable no-undef*/
<template>
  <div>
    <div id="allmap" style="overflow: hidden; zoom: 1; position: relative">
      <div
        id="map"
        style="
          height: 500px;
          -webkit-transition: all 0.5s ease-in-out;
          transition: all 0.5s ease-in-out;
        "
      ></div>
    </div>
  </div>
</template>
 
<script>
export default {
  name: "dbmap",
  props: {
    initpolygonStr: {
      required: true,
      default: false,
    },
    row: {
      type: Object,
      required: true,
    },
    nodeSelect: {
      type: Object,
      required: true,
    },
    mapVal: {
      type: String,
      default: "",
    },
  },
  created() {
    this.initpolygon = this.initpolygonStr
      ? JSON.parse(this.initpolygonStr)
      : [];
  },
  data() {
    return {
      map: null,
      drawingManager: null,
      polygon: null,
      initpolygon: [],
      overlays: [],
      areaStyle: {
        strokeColor: "rgba(244,25,30)", //边线颜色。
        fillColor: "rgba(244,25,30)", //填充颜色。当参数为空时，圆形将没有填充效果。
        strokeWeight: 2, //边线的宽度，以像素为单位。
        strokeOpacity: 0.8, //边线透明度，取值范围0 - 1。
        fillOpacity: 0.3, //填充的透明度，取值范围0 - 1。
        strokeStyle: "solid", //边线的样式，solid或dashed。
      },
    };
  },

  mounted() {
    this.initMap();
    this.initDrawing();
  },

  methods: {
    initMap() {
      let obj = JSON.parse(this.nodeSelect.localtion);
      // 百度地图API功能
      var map = new BMap.Map("map");
      this.map = map;
      // var poi = new BMap.Point(104.724151, 31.464994);
      var poi = new BMap.Point(obj.lng, obj.lat);
      let mark = new BMap.Marker(poi);
      map.addOverlay(mark);
      map.centerAndZoom(poi, 13);
      map.enableScrollWheelZoom();
      this.overlays = [];
      let tempPoint = this.initpolygon.map((e) => new BMap.Point(e.lng, e.lat));
      var polygon = new BMap.Polygon(tempPoint, this.areaStyle);
      this.polygon = polygon;
      this.map.addOverlay(polygon);
      this.local = new BMap.LocalSearch(this.map, {
        renderOptions: { map: this.map },
      });
    },
    initDrawing() {
      //实例化鼠标绘制工具
      var drawingManager = new BMapLib.DrawingManager(this.map, {
        isOpen: false, //是否开启绘制模式
        enableDrawingTool: true, //是否显示工具栏
        drawingToolOptions: {
          anchor: 1, //位置
          offset: new BMap.Size(5, 5), //偏离值
          drawingModes: ["polygon"],
        },
        circleOptions: this.areaStyle, //圆的样式
        polylineOptions: this.areaStyle, //线的样式
        polygonOptions: this.areaStyle, //多边形的样式
        rectangleOptions: this.areaStyle, //矩形的样式
      });
      //添加鼠标绘制工具监听事件，用于获取绘制结果
      drawingManager.addEventListener("overlaycomplete", this.overlaycomplete);
    },
    overlaycomplete(e) {
      for (var i = 0; i < this.overlays.length; i++) {
        this.map.removeOverlay(this.overlays[i]);
      }
      if (this.polygon) {
        this.map.removeOverlay(this.polygon);
      }
      this.overlays = [e.overlay];
      let temp = this.overlays[0].Ao.map((cc) => {
        return { lng: cc.lng, lat: cc.lat };
      });
      this.$emit("changeAera", { str: JSON.stringify(temp), row: this.row });
    },
    handleSeach() {
      this.local.search(this.mapVal);
    },
  },
};
</script>
 
<style>
/* The container of BaiduMap must be set width & height. */
.anchorBL {
  display: none;
}
</style>