window.GF.emitter.on('LogOut', () => {
  if (window.GF.socket) {
    window.GF.socket.close()
  }
})
window.GF.emitter.on('WebSocket', createWebSocket)

function createWebSocket() {
  let user = window.GF.context.get("actor").user;
  let urls = `${window.GF.cfg.websocketprot + window.GF.cfg.domain}/other/GetConnect?tcAdminId=${user.id}`;
  let socket = new WebSocket(urls);
  window.GF.socket = socket
  socket.onmessage = onMessage;
  socket.onclose = onClose;
  socket.onopen = onOpen;
  socket.onerror = onError;
}

let timer = null
function onOpen(e) {
  console.log("websockt-open", e);
  timer ? clearInterval(timer) : null;
}
function onMessage(e) {
  console.log("websockt-msg", e);
  window.GF.emitter.emit('WebSocket_msg', e)
}
function onError(e) {
  console.log("websockt-err", e);
}
function onClose(e) {
  console.log("websockt-close", e);
  timer = setInterval(() => {
    createWebSocket();
  }, 10000);
}