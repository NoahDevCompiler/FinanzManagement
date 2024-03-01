<script setup>
    import { ref, onMounted, computed } from 'vue'
    import { useUser } from '@/api/user.js'
    import { useAuth } from '@/api/auth.js'
    import { deleteSaving } from '../api/requests'

    import { formatMoney, formatDateRelativ } from '@/tools.js'
    import LoadingIcon from '@/icons/LoadingIcon.vue'

    const { loadSavings, savings } = useUser();
    const { isLoggedIn } = useAuth();

    const visualList = computed(() => {
        if(isExpanded.value || !savings.value) return savings.value;
        return savings.value.slice(0,3);
    })

    const isExpanded = ref(false);
    function setExpanded(value){
        isExpanded.value = value;
    }

    async function removeSaving(s){
        try {
            await deleteSaving(s.id);
        } catch (e) {
            console.log('Error when deleting ' + e);
        }

        loadSavings();
    }

    onMounted(() => loadSavings());
</script>

<template>
    <div>
        <div class="element card p-3">
            <b class="title">Sparziel</b>
            <div v-if="visualList">
                <div v-for="s in visualList" class="savings card m-1 p-2">
                    <b>{{ s.savingName }}</b>
                    <table>
                        <tr>
                            <th>Ziel </th>
                            <td>{{ formatMoney(s.savingGoal) }}</td>
                        </tr>
                        <tr>
                            <th>Monatliche Einzahlung </th>
                            <td>{{ formatMoney(s.monthlyAmount) }}</td>
                        </tr>
                        <tr>
                            <th>Spart seit </th>
                            <td>{{ formatDateRelativ(s.startDate)  }}</td>
                        </tr>
                    </table>
                    
                    <div style="text-align: right;">
                        <button @click="removeSaving(s)" class="btn btn-sm btn-danger">Delete</button>
                    </div>
                </div>
                <div v-if="savings.length > 3" class="card m-1 p-2">
                    <button v-if="!isExpanded" @click="setExpanded(true)" class="btn btn-secondary">Mehr Anzeigen</button>
                    <button v-else @click="setExpanded(false)" class="btn btn-secondary">Weniger Anzeigen</button>
                </div>
            </div>
            <LoadingIcon v-else-if="isLoggedIn" />
        </div>
    </div>
</template>

<style scoped>

.title {
    text-align: center;
}

.savings h2 {
    font-family: monospace;
    margin-bottom: 0px;
}

.savings small {
    color: rgba(255, 255, 255, 0.697);
}

.savings th {
    color: rgba(255, 255, 255, 0.697);

    font-weight: 400;
    
    text-align: right;

    padding: 2px;
    padding-right: 20px;
    width:50%;
}
.savings td {
    font-family: monospace;
    font-weight: 600;

    text-align: left;

    padding: 2px;
    padding-left: 20px;
    width:50%;
}
</style>