import { ref } from 'vue'
import { fetchUserData, fetchTransactions, fetchSavings } from '@/api/requests.js'
import { fetchCapital, fetchTotalEarned, fetchTotalLost, fetchTotalSavings } from './requests'

const user = ref(null)
const transactions = ref(null)
const savings = ref(null)

const totals = ref({
    totalEarned: '',
    totalLost: '',
    totalSaved: '',
})

const capital = ref(null);

export function useUser(){
    async function loadUser(){
        try {
            user.value = await fetchUserData();
            capital.value = (await fetchCapital()).capital;
        } catch (e) {
            console.log('Error when loading user data ' + e);
        }
    }

    async function loadTransactions() {
        try {
            transactions.value = await fetchTransactions();
        } catch (e) {
            console.log('Error when loading user data ' + e);
        }
    }

    async function loadSavings(){
        try {
            savings.value = await fetchSavings();
        } catch (e) {
            console.log('Error when loading user data ' + e);
        }
    }

    async function loadTotals(){
        try {
            totals.value.totalEarned = (await fetchTotalEarned()).totalEarned;
            totals.value.totalLost = (await fetchTotalLost()).totalLost;
            totals.value.totalSaved = (await fetchTotalSavings()).totalSaved;
        } catch (e) {
            console.log('Error when loading user data ' + e);
        }
    }

    function reset(){
        user.value = null;
        transactions.value = null;
        savings.value = null;
        capital.value = null;

        totals.value = {
            totalEarned: '',
            totalLost: '',
            totalSaved: '',
        }
    }

    return {
        loadUser,
        loadTransactions,
        loadSavings,
        loadTotals,
        reset,
        user,
        transactions,
        savings,
        totals,
        capital
    }

}