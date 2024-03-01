import './assets/main.css'  
import '@vuepic/vue-datepicker/dist/main.css'

import { createApp } from 'vue'
import App from './App.vue'
import router from './router.js'

import { checkAuth } from '@/api/requests.js'

checkAuth();

createApp(App).use(router).mount('#app')
