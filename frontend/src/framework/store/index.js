export function getLists(state, key, method) {
  if (state[key] === null) {
    window.GF.store
      .dispatch(method)
      .then((res) => {
        return res;
      })
      .catch(() => { });
  } else {
    return state[key];
  }
}

export function getdata(state, key, method) {
  return new Promise((resolve, reject) => {
    if (!state[key]) {
      window.GF.store.dispatch(method)
        .then(() => {
          resolve(state[key]);
        })
        .catch((err) => {
          reject(err);
        });
    } else {
      resolve(state[key]);
    }
  });
}
