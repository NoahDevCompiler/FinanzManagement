<script setup>
    import { ref, computed, defineModel } from 'vue'
    import { formatMoney } from '@/tools.js'

    const isFocus = ref(false);
    const value = defineModel()
    const visualValue = computed(() => {
        if(!value.value) return ''

        if(isFocus.value){
            return value.value
        }

        return formatMoney(value.value)
    })

    function updateValue(e){
        let v = e.target.value;
        value.value = Math.round(v*100)/100;
    }
</script>

<template>
    <input :type="isFocus ? 'number' : 'text'"
        @focusin="isFocus = true"  
        @focusout="isFocus = false" 
        @change="updateValue"
        :value="visualValue"
        class="form-control">
</template>