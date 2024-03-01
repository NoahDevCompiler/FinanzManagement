<script setup>
    import { ref, computed } from 'vue'
    import { useAuth } from '@/api/auth.js'
    import { formatMoney } from '@/tools.js'
    import LoadingIcon from '@/icons/LoadingIcon.vue'

    const { isLoggedIn } = useAuth();

    const props = defineProps(['name', 'list']);
    const emits = defineEmits(['removeTransaction'])

    const visualList = computed(() => {
        if(isExpanded.value || !props.list) return props.list;
        return props.list.slice(0,3);
    })

    const isExpanded = ref(false);
    function setExpanded(value){
        isExpanded.value = value;
    }
</script>

<template>
    <div class="element card p-3">
        <b class="title">{{ props.name }}</b>
        <div v-if="visualList">
            <div v-for="t in visualList" class="transaction card m-1 p-2">
                <b>{{ t.transactionName }}</b>
                <table>
                    <tr>
                        <th>Wert </th>
                        <td>{{ formatMoney(t.transactionValue) }}</td>
                    </tr>
                </table>
                <div style="text-align: right;">
                    <button @click="$emit('removeTransaction', t)" class="btn btn-sm btn-danger">Delete</button>
                </div>
            </div>
            <div v-if="list.length > 3" class="card m-1 p-2">
                <button v-if="!isExpanded" @click="setExpanded(true)" class="btn btn-secondary">Mehr Anzeigen</button>
                <button v-else @click="setExpanded(false)" class="btn btn-secondary">Weniger Anzeigen</button>
            </div>
        </div>
        <LoadingIcon v-else-if="isLoggedIn" />
    </div>
</template>

<style scoped>

    .title {
        text-align: center;
    }

    .transaction h2 {
        font-family: monospace;
        margin-bottom: 0px;
    }
    
    .transaction small {
        color: rgba(255, 255, 255, 0.697);
    }

    .transaction th {
        color: rgba(255, 255, 255, 0.697);

        font-weight: 400;
        
        text-align: right;

        padding: 2px;
        padding-right: 20px;
        width:50%;
    }
    .transaction td {
        font-family: monospace;
        font-weight: 600;

        text-align: left;

        padding: 2px;
        padding-left: 20px;
        width:50%;
    }
</style>