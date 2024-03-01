<script setup>
    import { RouterView } from 'vue-router'

    import NavBar from './components/NavBar.vue'
    import LoginInfo from './components/LoginInfo.vue'
</script>

<template>
    <div class="d-flex align-items-stretch">
        <NavBar/>
        <div class="content container">
            <LoginInfo/>  
            <RouterView/>  
        </div>
    </div>
</template>

<style scoped>
.content {
    max-width: 800px;
    min-height: 100vh;
}
</style>
