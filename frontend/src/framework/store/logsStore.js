import { dateFormat } from '@/framework/util/date'

export const LogsStore=
  {
    state: {
      logsList: window.GF.cache.get('logsList') || [],
    },
    actions: {
      //发送错误日志
      SendLogs() {
        return new Promise(() => {
          // // TODO
          // sendLogs(state.logsList).then(() => {
          //   commit('CLEAR_LOGS');
          //   resolve();
          // }).catch((error) => {
          //   reject(error)
          // })
        })
      },
    },
    mutations: {
      ADD_LOGS: (state, { type, message, stack, info }) => {
        state.logsList.push(Object.assign({
          url: window.location.href,
          time: dateFormat(new Date())
        }, {
          type,
          message,
          stack,
          info: info.toString()
        }))
        window.GF.cache.set('logsList', state.logsList)
      },
      CLEAR_LOGS: (state) => {
        state.logsList = [];
        window.GF.cache.set('logsList', state.logsList)
      }
    },
    getters:{
      logsList: (state) => state.logsList,
      logsLen: (state) => state.logsList.length || 0,
      logsFlag: (state, getters) => getters.logsLen === 0,
    }

  }
