import Cookies from 'js-cookie'
import website from '@/framework/config/website'
const Authorization = website.Authorization
export function getToken() {
  return Cookies.get(Authorization)
}

export function setToken(token) {
  if (website.tokenTime) {
    return Cookies.set(Authorization, token, { expires: new Date(new Date().getTime() + website.tokenTime * 1000) })
  } else {
    return Cookies.set(Authorization, token)
  }
}

export function removeToken() {
  return Cookies.remove(Authorization)
}