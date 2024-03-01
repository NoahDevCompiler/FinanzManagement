<script setup>
    import { ref,computed } from 'vue';
    import { registerUser } from '@/api/requests.js'
    import router from '@/router.js'

    const username = ref("");
    const password = ref("");
    const email = ref("");

    const errors = ref({
        username: '', 
        password: '', 
        email: '', 
    });

    async function register() {
        try {
            await registerUser(username.value, password.value, email.value);
            router.push('/login');
        } catch (exception) {
            console.error('login error', exception);
            errors.value = exception.errors;
        }

        username.value = "";
        password.value = "";
        email.value = "";
    }

    const canSubmit = computed(() => {
        return username.value === "" || password.value === "" || email.value === "";
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
            <div class="form-group m-2">
                <label for="InputMail">E-Mail</label>
                <input v-model="email" type="email" class="form-control" id="InputMail" placeholder="E-Mail">
                <div v-if="errors.email" class="text-danger m-2">
                    {{errors.email}}
                </div>
            </div>
            <hr>
            <div class="form-group m-2">
                <button :disabled="canSubmit" @click="register" class="btn btn-primary">Registrieren</button>
            </div>
        </div>
    </section>

</template>

<style>


</style>