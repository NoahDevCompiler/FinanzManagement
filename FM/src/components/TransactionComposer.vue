<script setup>
    import { ref, computed } from 'vue'
    import VueDatePicker from '@vuepic/vue-datepicker'
    import MoneyInput from './MoneyInput.vue'

    import { createTransaction } from '@/api/requests.js'
    
    import { useAuth } from '@/api/auth.js'
    import { useUser } from '@/api/user.js'
    const { isLoggedIn } = useAuth();
    const { loadTransactions } = useUser();

    const transactionName = ref('');
    const transaction = ref('');
    const isMonthly = ref(false);
    const isFuture = ref(true);
    const startdate = ref(null);
    const enddate = ref(null);

    const errors = ref({
        transactionName: '',
        transaction: '',
        startdate: '',
        enddate: '',
    })

    const canSubmit = computed(() => {
        return transactionName.value !== '' && transaction.value !== '' && isLoggedIn.value;
    });

    async function create(){
        let newStartDate = startdate.value;
        let newEnddate = enddate.value;

        if(!isMonthly.value){
            newStartDate = null;
            newEnddate = null;
        }

        if(isFuture.value){
            newEnddate = null;
        }

        try {
            await createTransaction(transactionName.value, transaction.value, newStartDate, newEnddate);
        } catch (exception) {
            console.error('login error', exception);
            errors.value = exception.errors;
        }

        transactionName.value = '';
        transaction.value = '';
        isMonthly.value = false;
        isFuture.value = true;
        startdate.value = null;
        enddate.value = null;

        loadTransactions();
    }
</script>

<template>
    <div class="card p-3">
        <div>
            <div class="form-group m-2">
                <label for="TransactionNameInput">Transaktion Name</label>
                <input v-model="transactionName" type="text" class="form-control" id="TransactionNameInput"
                    placeholder="Transaktion ...">
                <div v-if="errors.transactionName" class="text-danger m-2">
                    {{errors.transactionName}}
                </div>
            </div>
            <div class="form-group m-2">
                <label for="TransactionInput">Transaktion</label>
                <MoneyInput v-model="transaction" id="TransactionInput" />
                <small class="m-2">Negative Werte gelten als Verluste/Ausgaben.</small><br>
                <small class="m-2">Positive Werte als Profite/Einnahmen.</small>
                <div v-if="errors.transaction" class="text-danger m-2">
                    {{errors.transaction}}
                </div>
            </div>
            <div class="form-group">
                <input v-model="isMonthly" class="m-2 form-check-input" type="checkbox" id="flexCheckDefault">
                <label class="m-1 form-check-label" for="flexCheckDefault">
                  Ist Montatlich
                </label>
            </div>
            <div v-if="isMonthly">
                <div class="form-group m-2">
                    <label for="FromDateInput">Von Datum:</label>
                    <VueDatePicker v-model="startdate" id="FromDateInput" input-class-name="dp-custom"/>
                    <div v-if="errors.startdate" class="text-danger m-2">
                        {{errors.startdate}}
                    </div>
                </div>
                <div class="form-group">
                    <input v-model="isFuture" class="m-2 form-check-input" type="checkbox" id="flexCheckDefault">
                    <label class="m-1 form-check-label" for="flexCheckDefault">
                        In die Zukunft weiterlaufen
                    </label>
                </div>
                <div v-if="!isFuture" class="form-group m-2">
                    <label for="ToDateInput">Bis Datum:</label>
                    <VueDatePicker v-model="enddate" id="ToDateInput" input-class-name="dp-custom"/>
                    <div v-if="errors.enddate" class="text-danger m-2">
                        {{errors.enddate}}
                    </div>
                </div>

            </div>
            <hr>
            <div class="form-group m-2">
                <button :disabled="!canSubmit" @click="create" class="btn btn-primary">Erstellen</button>
            </div>
        </div>
    </div>
</template> 

<style scoped>
    small {
        color: rgba(255, 255, 255, 0.598);
    }
</style>