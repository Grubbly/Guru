import Vue from 'vue'
import VueFrappe from 'vue2-frappe'
import App from './App.vue'
import Vuetify from 'vuetify'
import 'vuetify/dist/vuetify.min.css'
import VueFire from 'vuefire'

Vue.use(VueFrappe)
Vue.use(Vuetify)
Vue.use(VueFire)

Vue.config.productionTip = false

new Vue({
  render: h => h(App),
}).$mount('#app')
