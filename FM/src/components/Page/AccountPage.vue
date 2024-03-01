<script setup>
    import { onMounted, ref } from 'vue';

    import { useAuth } from '@/api/auth.js'
    import { useUser } from '@/api/user.js'
    import router from '@/router.js'
    
    import PersonIcon from '@/icons/PersonIcon.vue'
    import LoadingIcon from '@/icons/LoadingIcon.vue'
    
    const { logout } = useAuth();
    const { user, reset, loadUser } = useUser();

    function onLogout(){
        logout();
        reset();
        router.push("/");
    }

    onMounted(() => loadUser())


</script>

<template>
    <section v-if="user" class="p-3 row">

        <div class="col-2">
            <PersonIcon style="width: 100%;"/>      
        </div>
        <div class="col">
            <div><b>Nutzername:</b> {{ user.username }} </div>
            <hr>
            <div style="text-align: right;">
                <button @click="onLogout" class="btn btn-danger">Abmelden</button>
            </div>
        </div>

    </section>
    <section v-else>
        <LoadingIcon/>
    </section>

</template>