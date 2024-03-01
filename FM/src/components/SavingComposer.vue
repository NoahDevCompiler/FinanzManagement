<script setup>
    import { ref, computed } from 'vue'
    import { createSaving } from '../api/requests';
    import { useAuth } from '../api/auth';
    import { useUser } from '../api/user';

    import VueDatePicker from '@vuepic/vue-datepicker'
    import MoneyInput from './MoneyInput.vue'

    const { isLoggedIn } = useAuth();
    const { loadSavings } = useUser();

    const savingName = ref('');
    const savingGoal = ref('');
    const monthlyAmount = ref('');
    const startdate = ref(null);

    const errors = ref({
        savingName: '',
        savingGoal: '',
        monthlyAmount: '',
        startdate: '',
    })

    const canSubmit = computed(() => {
        return savingName.value !== '' && savingGoal.value !== '' && monthlyAmount.value !== '' && isLoggedIn.value;
    });

    async function create(){
        try {
            await createSaving(savingName.value, savingGoal.value, monthlyAmount.value, startdate.value);
        } catch (exception) {
            console.error('login error', exception);
            errors.value = exception.errors;
        }

        savingName.value = '';
        savingGoal.value = '';
        monthlyAmount.value = '';
        startdate.value = null;

        loadSavings();
    }
</script>

<template>
    <div class="card p-3">
        <div>
            <div class="form-group m-2">
                <label for="SavingNameInput">Sparziel Name</label>
                <input v-model="savingName" type="text" class="form-control" id="SavingNameInput"
                    placeholder="Sparziel ...">
                <div v-if="errors.savingName" class="text-danger m-2">
                    {{errors.savingName}}
                </div>
            </div>
            <div class="form-group m-2">
                <label for="SavingGoalInput">Sparziel</label>
                <MoneyInput v-model="savingGoal" id="SavingGoalInput" />
                <div v-if="errors.savingGoal" class="text-danger m-2">
                    {{errors.savingGoal}}
                </div>
            </div>
            <div class="form-group m-2">
                <label for="MonthlyAmountInput">Monatliche Einzahlung</label>
                <MoneyInput v-model="monthlyAmount" id="MonthlyAmountInput" />
                <div v-if="errors.monthlyAmount" class="text-danger m-2">
                    {{errors.monthlyAmount}}
                </div>
            </div>
            <div class="form-group m-2">
                <label for="StartDateInput">Seit Datum:</label>
                <VueDatePicker v-model="startdate" id="StartDateInput" input-class-name="dp-custom"/>
                <div v-if="errors.startdate" class="text-danger m-2">
                    {{errors.startdate}}
                </div>
            </div>
            <hr>
            <div class="form-group m-2">
                <button :disabled="!canSubmit" @click="create" class="btn btn-primary">Erstellen</button>
            </div>
        </div>
    </div>
</template>