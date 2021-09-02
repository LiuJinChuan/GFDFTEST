import website from '@/framework/config/website'
export const CommonStore=
  {
    state: {
      
      language: window.GF.cache.get('language') || 'zh',
      isCollapse: false,
      isFullScren: false,
      isMenu: true,
      isShade: false,
      screen: -1,
      
      isLock:  false,
      showTag: true,
      showDebug: true,
      showCollapse: true,
      showSearch: true,
      showLock: true,
      showFullScren: true,
      showTheme: true,
      showMenu: true,
      showColor: true,
      
      colorName: window.GF.cache.get('colorName') || '#409EFF',
      
      themeName: window.GF.cache.get('themeName') || 'default',
      
      lockPasswd: window.GF.cache.get('lockPasswd') || '',
      website: website,
    },
    mutations: {
      SET_LANGUAGE: (state, language) => {
        state.language = language
        
        window.GF.cache.set('language', state.language)
      },
      SET_SHADE: (state, active) => {
        state.isShade = active;
      },
      SET_COLLAPSE: (state) => {
        state.isCollapse = !state.isCollapse;
      },
      SET_IS_MENU: (state, menu) => {
        state.isMenu = menu;
      },
      SET_FULLSCREN: (state) => {
        state.isFullScren = !state.isFullScren;
      },
      SET_LOCK: (state) => {
        state.isLock = true;
        
        window.GF.context.set('isLock', state.isLock)

      },
      SET_SCREEN: (state, screen) => {
        state.screen = screen;
      },
      SET_COLOR_NAME: (state, colorName) => {
        state.colorName = colorName;
        
        window.GF.cache.set('colorName', state.colorName)

      },
      SET_THEME_NAME: (state, themeName) => {
        state.themeName = themeName;
        
        window.GF.cache.set('themeName', state.themeName)
      },
      SET_LOCK_PASSWD: (state, lockPasswd) => {
        state.lockPasswd = lockPasswd;
        
        window.GF.context.set('lockPasswd', state.lockPasswd)

      },
      CLEAR_LOCK: (state) => {
        state.isLock = false;
        state.lockPasswd = '';
      },
    },
    getters: {
      language: (state) => state.language,
      website: (state) => state.website,
      colorName: (state) => state.colorName,
      themeName: (state) => state.themeName,
      isShade: (state) => state.isShade,
      isCollapse: (state) => state.isCollapse,
      keyCollapse: (state, getters) =>
        getters.screen > 1 ? getters.isCollapse : false,
      screen: (state) => state.screen,
      isLock: (state) => state.isLock,
      isFullScren: (state) => state.isFullScren,
      isMenu: (state) => state.isMenu,
      lockPasswd: (state) => state.lockPasswd,

    }
  }
