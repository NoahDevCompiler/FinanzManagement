<script setup>
    import { useRoute, RouterLink } from 'vue-router'
    import { ref, computed } from 'vue';

    import { useAuth } from '@/api/auth.js'

    const { isLoggedIn } = useAuth();
    const route = useRoute()

    const show = computed(() => {
        if(isLoggedIn.value) {
            return false;
        }
        
        if(route.path === '/register' || route.path === '/login'){
            return false;
        }  

        return true;
    });


</script>

<template>
    <section v-if="show" class="m-2 p-2 login-info">
        Du bist nicht angemeldet. hier <RouterLink to="/login">anmelden</RouterLink>!
    </section>
</template>

<style scoped>
    .login-info {
        background-color: rgba(0, 0, 0, 0.284);
        border-radius: 10px;
        text-align: center;
    }
</style>