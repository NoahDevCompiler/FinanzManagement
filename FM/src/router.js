import { createRouter, createWebHistory } from 'vue-router'

import HomePage from './components/Page/HomePage.vue'
import LoginPage from './components/Page/LoginPage.vue'
import AccountPage from './components/Page/AccountPage.vue'
import RegisterPage from './components/Page/RegisterPage.vue'
import CapitalPage from './components/Page/CapitalPage.vue'
import TransactionPage from './components/Page/TransactionPage.vue'
import SavingPage from './components/Page/SavingPage.vue'

import { useAuth } from '@/api/auth.js'
const { isLoggedIn } = useAuth();

const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    routes: [
        {
            path: '/',
            component: HomePage
        },
        {
            path: '/Capital',
            component: CapitalPage
        },
        {
            path: '/Transaction',
            component: TransactionPage
        },
        {
            path: '/Saving',
            component: SavingPage
        },
        {
            path: '/login',
            component: LoginPage,
            beforeEnter: (to, form, next) => {
                if(!isLoggedIn.value) {
                    next();
                } else {
                    next('/');
                }
            }
        },
        {
            path: '/register',
            component: RegisterPage,
            beforeEnter: (to, form, next) => {
                if(!isLoggedIn.value) {
                    next();
                } else {
                    next('/');
                }
            }
        },
        {
            path: '/account',
            component: AccountPage,
            beforeEnter: (to, form, next) => {
                if(isLoggedIn.value) {
                    next();
                } else {
                    next('/');
                }
            }
        }
    ]
})

export default router