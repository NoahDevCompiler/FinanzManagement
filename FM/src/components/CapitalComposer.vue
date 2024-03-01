<script setup>
    import { ref, computed } from 'vue'
    import VueDatePicker from '@vuepic/vue-datepicker'
    import MoneyInput from './MoneyInput.vue'

    import { defineCapital } from '@/api/requests.js'

    import { useAuth } from '@/api/auth.js'
    import { useUser } from '@/api/user.js'
    const { isLoggedIn } = useAuth();
    const { loadUser } = useUser();

    const capital = ref('');
    const capitaldate = ref(new Date());

    const errors = ref({
        capital: '',
        capitaldate: '',
    })

    const canSubmit = computed(() => {
        return capital.value !== '' && isLoggedIn.value
    });

    async function update(){
        try {
            await defineCapital(capital.value, capitaldate.value);
        } catch (exception) {
            console.error('login error', exception);
            errors.value = exception.errors;
        }

        capital.value = '';
        capitaldate.value = new Date();

        loadUser();
    }

</script>


<template>
    <div class="card p-3">
        <div>
            <div class="form-group m-2">
                <label for="CapitalInput">Kapital</label>
                <MoneyInput v-model="capital" id="CapitalInput" />
                <div v-if="errors.capital" class="text-danger m-2">
                    {{errors.capital}}
                </div>
            </div>
            <div class="form-group m-2">
                <label for="CapitalDateInput">Von Datum:</label>
                <VueDatePicker v-model="capitaldate" id="CapitalDateInput" input-class-name="dp-custom"/>
                <div v-if="errors.capitaldate" class="text-danger m-2">
                    {{errors.capitaldate}}0
                </div>
            </div>
            <hr>
            <div class="form-group m-2">
                <button :disabled="!canSubmit" @click="update" class="btn btn-primary">Aktualisieren</button>
            </div>
        </div>
    </div>
</template>

<style>
    .dp-custom {
        display: block;
        width: 100%;
        font-size: 1rem;
        font-weight: 400;
        line-height: 1.5;
        color: #F1F0F3;
        appearance: none;
        background-color: #372b43;
        background-clip: padding-box;
        border: var(--bs-border-width) solid var(--bs-border-color);
        border-radius: var(--bs-border-radius);
        transition: border-color 0.15s ease-in-out, box-shadow 0.15s ease-in-out;
    }

</style>