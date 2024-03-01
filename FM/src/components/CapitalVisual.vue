<script setup>
    import { onMounted, computed } from 'vue'
    import { useAuth } from '@/api/auth.js'
    import { useUser } from '@/api/user.js'
    import { formatMoney } from '@/tools.js'

    import LoadingIcon from '@/icons/LoadingIcon.vue'

    const { isLoggedIn } = useAuth();
    const { user, capital, loadUser, transactions, savings, loadTotals, totals } = useUser();

    const visualUser = computed(() => {
        if(isLoggedIn.value){
            return user.value;
        }

        return {
            capital: '',
        }
    });

    const earnings = computed(() => {
        if(!transactions.value) return '--,-- CHF'

        let totalValue = 0;

        const filteredTrans = transactions.value.filter((e) => e.startDate && e.transactionValue >= 0);
        filteredTrans.forEach(e => { totalValue += e.transactionValue });
        
        return formatMoney(totalValue);
    });
    
    const loses = computed(() => {
        if(!transactions.value) return '--,-- CHF'

        let totalValue = 0;
        
        const filteredTrans = transactions.value.filter((e) => e.startDate && e.transactionValue < 0);
        filteredTrans.forEach(e => { totalValue += e.transactionValue });
        
        return formatMoney(Math.abs(totalValue));
    });

    const savingTotal = computed(() => {
        if(!savings.value) return '--,-- CHF'

        let totalValue = 0;

        savings.value.forEach((e) => { totalValue += e.monthlyAmount });

        return formatMoney(totalValue);
    });

    const totalEarned = computed(() => {
        if(!totals.value.totalEarned) return '--,-- CHF'
        return formatMoney(totals.value.totalEarned);
    });

    const totalLost = computed(() => {
        if(!totals.value.totalLost) return '--,-- CHF'
        return formatMoney(totals.value.totalLost);
    });

    const totalSaved = computed(() => {
        if(!totals.value.totalSaved) return '--,-- CHF'
        return formatMoney(totals.value.totalSaved);
    });

    const visualCapital = computed(() => {
        if(!capital.value) return '--,-- CHF'
        return formatMoney(capital.value);
    });

    onMounted(() => loadUser())
    onMounted(() => loadTotals())
</script>

<template>
    <div  v-if="visualUser" class="card p-3 money ">
        <div class="data">
            <h2>
                <b v-if="visualUser.capital"> {{ visualCapital }} </b>
                <b v-else>--,-- CHF</b>
            </h2>
            <small><b>Kapital diesen Monat</b></small>
             
            <div style="margin-top: 15px;"><b>{{ formatMoney(visualUser.capital) }}</b></div>
            <small><b>Start Kapital</b></small>
        </div>
        <hr>
        <div class="prediction row">
            <table>
                    <tr>
                        <th>Einnahmen </th>
                        <td>{{ earnings }}</td>
                    </tr>
                    <tr>
                        <th>Ausgaben</th>
                        <td>{{ loses }}</td>
                    </tr>
                    <tr>
                        <th>Wachstum Sparziel</th>
                        <td>{{ savingTotal }}</td>
                    </tr>
            </table>
        </div>
        <hr>
        <div class="prediction row">
            <table>
                    <tr>
                        <th>Totale Einnahmen </th>
                        <td>{{ totalEarned }}</td>
                    </tr>
                    <tr>
                        <th>Totale Ausgaben</th>
                        <td>{{ totalLost }}</td>
                    </tr>
                    <tr>
                        <th>Totaler Wachstum Sparziel</th>
                        <td>{{ totalSaved }}</td>
                    </tr>
            </table>
        </div>
    </div>
    <section v-else>
        <LoadingIcon/>
    </section>
</template>

<style scoped>
    
    .money h2 {
        font-family: monospace;
        margin-bottom: 0px;
    }
    
    .money small {
        color: rgba(255, 255, 255, 0.697);
    }

    .money th {
        color: rgba(255, 255, 255, 0.697);

        font-weight: 400;
        
        text-align: right;

        padding: 2px;
        padding-right: 20px;
        width:50%;
    }
    .money td {
        font-family: monospace;
        font-weight: 600;

        text-align: left;

        padding: 2px;
        padding-left: 20px;
        width:50%;
    }

    .money .data {
        text-align: center;
    }

</style>