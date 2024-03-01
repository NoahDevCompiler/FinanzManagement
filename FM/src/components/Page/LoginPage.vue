<script setup>
    import { computed, ref } from 'vue'
    import { loginUser } from '@/api/requests.js'
    import router from '@/router.js'
    import { RouterLink } from 'vue-router';

    const username = ref("");
    const password = ref("");

    const errors = ref({
        username: '',
        password: '',
    })  

    async function login () {
        try {
            await loginUser(username.value, password.value);
            router.push('/');
        } catch (exception) {
            console.error('login error', exception);
            errors.value = exception.errors;
        }

        username.value = "";
        password.value = "";
    }

    const canSubmit = computed(() => {
        return username.value === "" || password.value === ""
    });
</script>

<template>
    <section class="p-3">
        <div>
            <div class="form-group m-2">
                <label for="InputUsername">Nutzername</label>
                <input v-model="username" type="text" class="form-control" id="InputUsername"
                    placeholder="Username">
                <div v-if="errors.username" class="text-danger m-2">
                    {{errors.username}}
                </div>
            </div>
            <div class="form-group m-2">
                <label for="InputPassword">Passwort</label>
                <input v-model="password" type="password" class="form-control" id="InputPassword" placeholder="Password">
                <div v-if="errors.password" class="text-danger m-2">
                    {{errors.password}}
                </div>
            </div>
            <hr>
            <div class="form-group m-2">
                <button :disabled="canSubmit" @click="login" class="btn btn-primary">Login</button>
                <RouterLink to="/register" style="color: white;" class="m-2">oder Registrieren</RouterLink>
            </div>
        </div>
    </section>

</template>