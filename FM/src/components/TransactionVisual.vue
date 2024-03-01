<script setup>
    import { onMounted, computed } from 'vue'
    import { useUser } from '@/api/user.js'
    import { deleteTransaction } from '@/api/requests.js'

    import TransactionList from './TransactionList.vue'

    const { loadTransactions, transactions } = useUser();

    const newestTransaktions = computed(() => {
        if(!transactions.value) return null

        return transactions.value.filter((e) => !e.startDate).reverse();
    });

    const inTransaktions = computed(() => {
        if(!transactions.value) return nulls

        const filteredTrans = transactions.value.filter((e) => e.startDate && e.transactionValue >= 0);
        return filteredTrans.sort((a,b) => {
            a.transactionValue - b.transactionValue
        });
    });

    const outTransaktions = computed(() => {
        if(!transactions.value) return null

        const filteredTrans = transactions.value.filter((e) => e.startDate && e.transactionValue < 0);
        return filteredTrans.sort((a,b) => {
            b.transactionValue - a.transactionValue
        });
    });

    async function removeTransaction(t){
        try {
            await deleteTransaction(t.id);
        } catch (e) {
            console.log('Error when deleting ' + e);
        }

        loadTransactions();
    }

    onMounted(() => loadTransactions());

</script>

<template>
    <div>
        <TransactionList name="Neueste Transaktionen" :list="newestTransaktions" @remove-transaction="removeTransaction" />
        <TransactionList name="Einnahmen" :list="inTransaktions" @remove-transaction="removeTransaction" />
        <TransactionList name="Ausgaben" :list="outTransaktions" @remove-transaction="removeTransaction" />
    </div>
</template>